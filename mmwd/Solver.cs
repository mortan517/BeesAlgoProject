using System;
using System.Collections.Generic;
using System.Linq;

namespace mmwd
{
    internal class Solver
    {
        int people; // how many people do u want to accomodate
        int money; // how much money do u have
        int time; // how much time do u have
        int room; // importance of appropriate ornamention (1-10)
        int cakes; // importance of cakes (1-10)
        int food; // importance of food (1-10)
        int beverages; // importance of beverages (1-10)
        int space_bonus; // importance of appropriate number of guests

        int numberOfIterations;
        int numberOfScouts;
        int numberOfEliteAreas;
        int numberOfSelectedAreas;
        int beesInEliteAreas;
        int beesInSelectedAreas;
        int iterationsWithoutImprovement;
        int sizeOfNeighbourhood;

        const int max_iterations_multiplier = 1000; //max number of iterations while generating allowed bee = (bees to generate) * multiplier
        Random rand; //random numbers generator

        //some limits based on mathematical model; trying to generate numbers more effectively
        int limit_ciast_u;
        int limit_ozdob_o;
        int limit_ozdob_z;
        int limit_ozdob_d;
        int limit_potraw_z;

        public Solver(List<int> userParametersList, List<int> programmerParametersList) //constructor
        {
            people = userParametersList[0];
            money = userParametersList[1];
            time = userParametersList[2];
            room = userParametersList[3];
            cakes = userParametersList[4];
            food = userParametersList[5];
            beverages = userParametersList[6];
            space_bonus = (room + cakes + food + beverages); //definition of space_bonus parameter

            numberOfIterations = programmerParametersList[0];
            numberOfScouts = programmerParametersList[1];
            numberOfEliteAreas = programmerParametersList[2];
            numberOfSelectedAreas = programmerParametersList[3];
            beesInEliteAreas = programmerParametersList[4];
            beesInSelectedAreas = programmerParametersList[5];
            iterationsWithoutImprovement = programmerParametersList[6];
            sizeOfNeighbourhood = 1;    //to do: take this parameter from programmer interface

            rand = new Random();

            //based on mathematical model:
            limit_ciast_u = (time / 70 < 6) ? (time / 70) : 6;
            limit_potraw_z = (time / 90 < 6) ? (time / 90) : 6;
            limit_ozdob_z = (time / 50 < money / 15) ? (time / 50) : (money / 15);
            if (limit_ozdob_z > 6)
                limit_ozdob_z = 6;
            limit_ozdob_o = (money / 40 < 6) ? (money / 40) : 6;
            limit_ozdob_d = (money / 40 < 6) ? (money / 40) : 6;
        }

        class Bee //class representing a solution
        {
            private Solver parent;
            //using names specified in the model
            public int x_gosci; //number of invited guests

            public int x_ciast_o; //number of bought and collected cakes
            public int x_ciast_d; //number of bought and delivered cakes
            public int x_ciast_u; //number of baked cakes

            public int x_napojow_o; //number of bought and collected beverages
            public int x_napojow_d; //number of bought and delivered beverages

            public int x_ozdob_o; //number of bought and collected decorations
            public int x_ozdob_d; //number of bought and delivered decorations
            public int x_ozdob_z; //number of self-made decorations

            public int x_potraw_z; //number of cooked dishes
            public int x_potraw_d; //number of bought and deliver dishes

            public int value; //evaluation of objective function -- sum of guests satisfaction

            public Bee(Solver parent) //scout constructor
            {
                this.parent = parent;
                this.generate();    //in step 0 only scouts are generated
            }

            public void generate()   //generating a random scout bee
            {
                //generating random values of variables
                //we use knowledge about model to limit space from which numbers are generated
                x_gosci = parent.rand.Next(0, parent.people + 1);
                x_ciast_o = parent.rand.Next(0, 6);
                x_ciast_d = parent.rand.Next(0, 6);
                x_ciast_u = parent.rand.Next(0, parent.limit_ciast_u + 1);
                x_napojow_o = parent.rand.Next(0, 6);
                x_napojow_d = parent.rand.Next(0, 6);
                x_ozdob_o = parent.rand.Next(0, parent.limit_ozdob_o + 1);
                x_ozdob_d = parent.rand.Next(0, parent.limit_ozdob_d + 1);
                x_ozdob_z = parent.rand.Next(0, parent.limit_ozdob_z + 1);
                x_potraw_z = parent.rand.Next(0, parent.limit_potraw_z + 1);
                x_potraw_d = parent.rand.Next(0, 6);
                value = -1; //just to initialize
            }

            public void generate(Bee center, int neighbourhood_size)    //generating a random bee in elite or selected areas
            {
                //generating random values (but not lesser than 0 and not senselessly big) of variables from neighbourhood
                x_gosci = parent.rand.Next((center.x_gosci - neighbourhood_size >= 0) ? (center.x_gosci - neighbourhood_size) : 0, (center.x_gosci + neighbourhood_size + 1 < parent.people + 1) ? (center.x_gosci + neighbourhood_size + 1) : (parent.people + 1));
                x_ciast_o = parent.rand.Next((center.x_ciast_o - neighbourhood_size >= 0) ? (center.x_ciast_o - neighbourhood_size) : 0, (center.x_ciast_o + neighbourhood_size + 1 < 6) ? (center.x_ciast_o + neighbourhood_size + 1) : 6);
                x_ciast_d = parent.rand.Next((center.x_ciast_d - neighbourhood_size >= 0) ? (center.x_ciast_d - neighbourhood_size) : 0, (center.x_ciast_d + neighbourhood_size + 1 < 6) ? (center.x_ciast_d + neighbourhood_size + 1) : 6);
                x_ciast_u = parent.rand.Next((center.x_ciast_u - neighbourhood_size >= 0) ? (center.x_ciast_u - neighbourhood_size) : 0, (center.x_ciast_u + neighbourhood_size + 1 < parent.limit_ciast_u + 1) ? (center.x_ciast_u + neighbourhood_size + 1) : (parent.limit_ciast_u + 1));
                x_napojow_o = parent.rand.Next((center.x_napojow_o - neighbourhood_size >= 0) ? (center.x_napojow_o - neighbourhood_size) : 0, (center.x_napojow_o + neighbourhood_size + 1 < 6) ? (center.x_napojow_o + neighbourhood_size + 1) : 6);
                x_napojow_d = parent.rand.Next((center.x_napojow_d - neighbourhood_size >= 0) ? (center.x_napojow_d - neighbourhood_size) : 0, (center.x_napojow_d + neighbourhood_size + 1 < 6) ? (center.x_napojow_d + neighbourhood_size + 1) : 6);
                x_ozdob_o = parent.rand.Next((center.x_ozdob_o - neighbourhood_size >= 0) ? (center.x_ozdob_o - neighbourhood_size) : 0, (center.x_ozdob_o + neighbourhood_size + 1 < parent.limit_ozdob_o + 1) ? (center.x_ozdob_o + neighbourhood_size + 1) : (parent.limit_ozdob_o + 1));
                x_ozdob_d = parent.rand.Next((center.x_ozdob_d - neighbourhood_size >= 0) ? (center.x_ozdob_d - neighbourhood_size) : 0, (center.x_ozdob_d + neighbourhood_size + 1 < parent.limit_ozdob_d + 1) ? (center.x_ozdob_d + neighbourhood_size + 1) : (parent.limit_ozdob_d + 1));
                x_ozdob_z = parent.rand.Next((center.x_ozdob_z - neighbourhood_size >= 0) ? (center.x_ozdob_z - neighbourhood_size) : 0, (center.x_ozdob_z + neighbourhood_size + 1 < parent.limit_ozdob_z + 1) ? (center.x_ozdob_z + neighbourhood_size + 1) : (parent.limit_ozdob_z + 1));
                x_potraw_z = parent.rand.Next((center.x_potraw_z - neighbourhood_size >= 0) ? (center.x_potraw_z - neighbourhood_size) : 0, (center.x_potraw_z + neighbourhood_size + 1 < parent.limit_potraw_z + 1) ? (center.x_potraw_z + neighbourhood_size + 1) : (parent.limit_potraw_z + 1));
                x_potraw_d = parent.rand.Next((center.x_potraw_d - neighbourhood_size >= 0) ? (center.x_potraw_d - neighbourhood_size) : 0, (center.x_potraw_d + neighbourhood_size + 1 < 6) ? (center.x_potraw_d + neighbourhood_size + 1) : 6);
                value = -1; //just to initialize
            }

            public bool check_if_allowed() //checks if solution is allowed (are constraints satisfied? - 1-yes, 0-no)
            {
                //limited space satisfied during generating solution
                //limited time:
                if ((5 * (ifn_0(x_ciast_d) + ifn_0(x_napojow_d) + ifn_0(x_ozdob_d) + ifn_0(x_potraw_d)) + 40 * (ifn_0(x_ciast_o) + ifn_0(x_napojow_o) + ifn_0(x_ozdob_o)) + 20 * (ifn_0(x_ciast_u) + ifn_0(x_ozdob_z) + ifn_0(x_potraw_z)) + 70 * x_ciast_u + 90 * x_potraw_z + 50 * x_ozdob_z) > parent.time)
                    return false;
                //limited amount of money:
                else if ((15 * (ifn_0(x_ciast_d) + ifn_0(x_napojow_d) + ifn_0(x_ozdob_d) + ifn_0(x_potraw_d)) + x_gosci * (4 * (x_ciast_o + x_ciast_d) + (x_napojow_o + x_napojow_d) + 15 * x_potraw_d + x_ciast_u + 7 * x_potraw_z) + 40 * (x_ozdob_o + x_ozdob_d) + 15 * x_ozdob_z) > parent.money)
                    return false;
                else
                    return true;
            }

            private int ifn_0(int number) //checks if number is not a zero: 1 - is not a zero, 0 - is a zero
            {
                if (number != 0)
                    return 1;
                else
                    return 0;
            }

            public int evaluate() //method evaluating objective function (value)
            {
                value = x_gosci * (parent.space_bonus * f_gosci(x_gosci) + 2 * parent.cakes * f_i(x_ciast_d + x_ciast_o + x_ciast_u) + parent.beverages * f_i(x_napojow_d + x_napojow_o) + parent.room * f_i(x_ozdob_d + x_ozdob_o + x_ozdob_z) + 3 * parent.food * f_i(x_potraw_d + x_potraw_z));
                return value;
            }

            private int f_gosci(int x_gosci) //return 1 if space bonus should be given
            {
                if (x_gosci < 0.2 * parent.people)
                    return 0;
                else if (x_gosci > 0.8 * parent.people)
                    return 0;
                else
                    return 1;
            }

            private int f_i(int number) //evaluates happiness from variety
            {
                switch (number)
                {
                    case 0:
                        return 0;
                    case 1:
                        return 5;
                    case 2:
                        return 9;
                    case 3:
                        return 12;
                    case 4:
                        return 14;
                    default:
                        return 15;
                }
            }

            public void copy(Bee obj)
            {
                this.x_gosci = obj.x_gosci;
                this.x_ciast_o = obj.x_ciast_o;
                this.x_ciast_d = obj.x_ciast_d;
                this.x_ciast_u = obj.x_ciast_u;
                this.x_napojow_o = obj.x_napojow_o;
                this.x_napojow_d = obj.x_napojow_d;
                this.x_ozdob_o = obj.x_ozdob_o;
                this.x_ozdob_z = obj.x_ozdob_z;
                this.x_ozdob_d = obj.x_ozdob_d;
                this.x_potraw_z = obj.x_potraw_z;
                this.x_potraw_d = obj.x_potraw_d;
                this.value = obj.value;
            }
        }

        public int SolvingMethod() //solve and return results
        {
            /////////////////////// step 0  - initialization/////////////////////////////////

            //generating n_elite_areas + n_selected_areas + n_scouts
            List<Bee> beeVector = new List<Bee>();
            beeVector.Capacity = numberOfEliteAreas + numberOfSelectedAreas + numberOfScouts;
            try
            {
                int iterations_0 = 0; //number of iterations while trying to generate allowed beeVector
                for (int i = 0; i < (numberOfEliteAreas + numberOfSelectedAreas + numberOfScouts); i++)
                {
                    beeVector.Add(new Bee(this));
                    while (!(beeVector[i].check_if_allowed()))  //if not allowed generate till allowed
                    {
                        beeVector[i].generate();
                        iterations_0 += 1;
                        if (iterations_0 > (max_iterations_multiplier * (numberOfEliteAreas + numberOfSelectedAreas + numberOfScouts)))
                        {
                            throw new UnableToGenerateAllowed("Unable to generate bees satisfying constraints");
                        }
                    }
                    beeVector[i].evaluate();
                    /*
                    Console.Out.Write(i + "  ");
                    Console.Out.Write(iterations_0 + "  ");
                    Console.Out.Write(beeVector[i].value + "  ");
                    Console.Out.Write(beeVector[i].x_gosci + "  ");
                    Console.Out.Write(beeVector[i].x_ciast_o + "  ");
                    Console.Out.Write(beeVector[i].x_ciast_d + "  ");
                    Console.Out.Write(beeVector[i].x_ciast_u + "  ");
                    Console.Out.Write(beeVector[i].x_napojow_o + "  ");
                    Console.Out.Write(beeVector[i].x_napojow_d + "  ");
                    Console.Out.Write(beeVector[i].x_ozdob_o + "  ");
                    Console.Out.Write(beeVector[i].x_ozdob_z + "  ");
                    Console.Out.Write(beeVector[i].x_ozdob_d + "  ");
                    Console.Out.Write(beeVector[i].x_potraw_z + "  ");
                    Console.Out.Write(beeVector[i].x_potraw_d + "\n");
                    */
                }
            }
            catch (UnableToGenerateAllowed ex)
            {
                Console.Out.WriteLine("Unable to generate bees satisfying constraints");
                return -1;
            }

            Bee tempBee = new Bee(this); //temporary bee will be useful in following steps
            Bee areaBee = new mmwd.Solver.Bee(this);

            int nowWithoutImprovement = 0;  //to count iterations without improvement

            beeVector = beeVector.OrderByDescending(b => b.value).ToList();

            /*
            foreach(Bee b in beeVector)
            {
                Console.Out.Write(b.value + "  ");
                Console.Out.Write(b.x_gosci + "  ");
                Console.Out.Write(b.x_ciast_o + "  ");
                Console.Out.Write(b.x_ciast_d + "  ");
                Console.Out.Write(b.x_ciast_u + "  ");
                Console.Out.Write(b.x_napojow_o + "  ");
                Console.Out.Write(b.x_napojow_d + "  ");
                Console.Out.Write(b.x_ozdob_o + "  ");
                Console.Out.Write(b.x_ozdob_z + "  ");
                Console.Out.Write(b.x_ozdob_d + "  ");
                Console.Out.Write(b.x_potraw_z + "  ");
                Console.Out.Write(b.x_potraw_d + "\n");
            }
            */

            /////////////////////////////// step k -- iteration of the algorithm /////////////////////////

            for (int algorithmIt = 1; algorithmIt <= numberOfIterations; algorithmIt++) //1 of 2 alternative STOP criteria -- max number of iterations
            {
                if (nowWithoutImprovement > iterationsWithoutImprovement) //second STOP criteria -- max number of iterations without improvement
                    break;

                int best_value = beeVector[0].value; //save best value as for now

                /////operations on bees in elite areas:
                for(int e = 0; e < numberOfEliteAreas; e++)
                {
                    areaBee.copy(beeVector[e]); //initialize best bee in the area so far
                    try
                    {
                        int iterations_e = 0; //max number of iterations while generating random bees in elite area
                        for (int i = 0; i < beesInEliteAreas; i++)   //generating beesInEliteAreas bees in neighbourhood
                        {
                            tempBee.generate(beeVector[e], sizeOfNeighbourhood);
                            while (!(tempBee.check_if_allowed()))  //if not allowed generate till allowed
                            {
                                tempBee.generate(beeVector[e], sizeOfNeighbourhood);
                                iterations_e += 1;
                                if (iterations_e > (max_iterations_multiplier * beesInEliteAreas))
                                {
                                    throw new UnableToGenerateAllowed("Unable to generate bees satisfying constraints in elite area.");
                                }
                            }
                            tempBee.evaluate();
                            if (tempBee.value > areaBee.value)
                                areaBee.copy(tempBee);
                        }
                    }
                    catch (UnableToGenerateAllowed ex)
                    {
                        Console.Out.WriteLine("Unable to generate bees satisfying constraints in elite area.");
                        return -2;
                    }
                    beeVector[e].copy(areaBee); //save best bee in the area
                }
                

                /////operations on bees in selected areas
                for(int s = numberOfEliteAreas; s < (numberOfEliteAreas + numberOfSelectedAreas); s++)
                {
                    areaBee.copy(beeVector[s]); //initialize best bee in the area so far
                    try
                    {
                        int iterations_s = 0; //max number of iterations while generating random bees in elite area
                        for(int i = 0; i < beesInSelectedAreas; i++)
                        {
                            tempBee.generate(beeVector[s], sizeOfNeighbourhood);
                            while(!(tempBee.check_if_allowed())) //generate till allowed
                            {
                                tempBee.generate(beeVector[s], sizeOfNeighbourhood);
                                iterations_s += 1;
                                if (iterations_s > (max_iterations_multiplier * beesInSelectedAreas))
                                {
                                    throw new UnableToGenerateAllowed("Unable to generate bees satisfying constraints in elite area.");
                                }
                            }
                            tempBee.evaluate();
                            if (tempBee.value > areaBee.value)
                                areaBee.copy(tempBee);
                        }
                    }
                    catch (UnableToGenerateAllowed ex)
                    {
                        Console.Out.WriteLine("Unable to generate bees satisfying constraints in selected area.");
                        return -3;
                    }
                    beeVector[s].copy(areaBee); //save best bee in the area
                }
                

                /////generating new scout bees     
                try
                {
                    int iterations_0 = 0; //number of iterations while trying to generate allowed beeVector
                    for (int i = (numberOfEliteAreas + numberOfSelectedAreas); i < (numberOfEliteAreas + numberOfSelectedAreas + numberOfScouts); i++)
                    {
                        beeVector[i].generate();
                        while (!(beeVector[i].check_if_allowed()))  //if not allowed generate till allowed
                        {
                            beeVector[i].generate();
                            iterations_0 += 1;
                            if (iterations_0 > (max_iterations_multiplier * numberOfScouts))
                            {
                                throw new UnableToGenerateAllowed("Unable to generate bees satisfying constraints");
                            }
                        }
                        beeVector[i].evaluate();
                    }
                }
                catch (UnableToGenerateAllowed ex)
                {
                    Console.Out.WriteLine("Unable to generate new scout bees satisfying constraints");
                    return -4;
                }


                /////ending current iteration
                beeVector = beeVector.OrderByDescending(b => b.value).ToList();
                if (best_value == beeVector[0].value)
                    nowWithoutImprovement += 1;
                else
                    nowWithoutImprovement = 0;

                /*
                foreach (Bee b in beeVector)
                {
                    Console.Out.Write(b.value + "  ");
                    Console.Out.Write(b.x_gosci + "  ");
                    Console.Out.Write(b.x_ciast_o + "  ");
                    Console.Out.Write(b.x_ciast_d + "  ");
                    Console.Out.Write(b.x_ciast_u + "  ");
                    Console.Out.Write(b.x_napojow_o + "  ");
                    Console.Out.Write(b.x_napojow_d + "  ");
                    Console.Out.Write(b.x_ozdob_o + "  ");
                    Console.Out.Write(b.x_ozdob_z + "  ");
                    Console.Out.Write(b.x_ozdob_d + "  ");
                    Console.Out.Write(b.x_potraw_z + "  ");
                    Console.Out.Write(b.x_potraw_d + "\n");
                }
                Console.Out.WriteLine();
                Console.WriteLine(beeVector[0].value);
                Console.Out.WriteLine();
                Console.Out.WriteLine();
                Console.Out.WriteLine();
                Console.Out.WriteLine();
                */
            }

            return beeVector[0].value;
        }
    }

/////////////// exceptions

    public class UnableToGenerateAllowed : SystemException
    {
        public UnableToGenerateAllowed() : base() { }
        public UnableToGenerateAllowed(string message) : base(message) { }
        public UnableToGenerateAllowed(string message, System.Exception inner) : base(message, inner) { }
        protected UnableToGenerateAllowed(System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context)
        { }
    }

}



