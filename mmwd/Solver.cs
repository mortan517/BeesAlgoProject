using System;
using System.Collections.Generic;


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

        int numbersOfIterations;
        int numbersOfScouts;
        int numbersOfEliteAreas;
        int numbersOfSelectedAreas;
        int beesInEliteAreas;
        int beesInSelectedAreas;
        int iterationsWithoutImprovement;

        Random rand; //random numbers generator

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

            numbersOfIterations = programmerParametersList[0];
            numbersOfScouts = programmerParametersList[1];
            numbersOfEliteAreas = programmerParametersList[2];
            numbersOfSelectedAreas = programmerParametersList[3];
            beesInEliteAreas = programmerParametersList[4];
            beesInSelectedAreas = programmerParametersList[5];
            iterationsWithoutImprovement = programmerParametersList[6];

            rand = new Random();
        }

        class Bee   //class representing a solution
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
                //generating random values of variables
                //we use knowledge about model to limit space from which numbers are generated
                x_gosci = parent.rand.Next(0, parent.room + 1);
                x_ciast_o = parent.rand.Next(0, 6);
                x_ciast_d = parent.rand.Next(0, 6);
                x_ciast_u = parent.rand.Next(0, 6);
                x_napojow_o = parent.rand.Next(0, 6);
                x_napojow_d = parent.rand.Next(0, 6);
                x_ozdob_o = parent.rand.Next(0, 6);
                x_ozdob_d = parent.rand.Next(0, 6);
                x_ozdob_z = parent.rand.Next(0, 6);
                x_potraw_z = parent.rand.Next(0, 6);
                x_potraw_d = parent.rand.Next(0, 6);
                value = 0; //just to initialize
            }

            public Bee(Solver parent, Bee center, int neighbourhood_size)   //bee in elite or selected area constructor
            {
                this.parent = parent;
                //generating random values (but not lesser than 0 and not senselessly big) of variables from neighbourhood
                x_gosci = parent.rand.Next((center.x_gosci - neighbourhood_size >= 0) ? (center.x_gosci - neighbourhood_size) : 0, (center.x_gosci + neighbourhood_size + 1 < parent.room + 1) ? (center.x_gosci + neighbourhood_size + 1) : (parent.room + 1));
                x_ciast_o = parent.rand.Next((center.x_ciast_o - neighbourhood_size >= 0) ? (center.x_ciast_o - neighbourhood_size) : 0, (center.x_ciast_o + neighbourhood_size + 1 < 6) ? (center.x_ciast_o + neighbourhood_size + 1) : 6);
                x_ciast_d = parent.rand.Next((center.x_ciast_d - neighbourhood_size >= 0) ? (center.x_ciast_d - neighbourhood_size) : 0, (center.x_ciast_d + neighbourhood_size + 1 < 6) ? (center.x_ciast_d + neighbourhood_size + 1) : 6);
                x_ciast_u = parent.rand.Next((center.x_ciast_u - neighbourhood_size >= 0) ? (center.x_ciast_u - neighbourhood_size) : 0, (center.x_ciast_u + neighbourhood_size + 1 < 6) ? (center.x_ciast_u + neighbourhood_size + 1) : 6);
                x_napojow_o = parent.rand.Next((center.x_napojow_o - neighbourhood_size >= 0) ? (center.x_napojow_o - neighbourhood_size) : 0, (center.x_napojow_o + neighbourhood_size + 1 < 6) ? (center.x_napojow_o + neighbourhood_size + 1) : 6);
                x_napojow_d = parent.rand.Next((center.x_napojow_d - neighbourhood_size >= 0) ? (center.x_napojow_d - neighbourhood_size) : 0, (center.x_napojow_d + neighbourhood_size + 1 < 6) ? (center.x_napojow_d + neighbourhood_size + 1) : 6);
                x_ozdob_o = parent.rand.Next((center.x_ozdob_o - neighbourhood_size >= 0) ? (center.x_ozdob_o - neighbourhood_size) : 0, (center.x_ozdob_o + neighbourhood_size + 1 < 6) ? (center.x_ozdob_o + neighbourhood_size + 1) : 6);
                x_ozdob_d = parent.rand.Next((center.x_ozdob_d - neighbourhood_size >= 0) ? (center.x_ozdob_d - neighbourhood_size) : 0, (center.x_ozdob_d + neighbourhood_size + 1 < 6) ? (center.x_ozdob_d + neighbourhood_size + 1) : 6);
                x_ozdob_z = parent.rand.Next((center.x_ozdob_z - neighbourhood_size >= 0) ? (center.x_ozdob_z - neighbourhood_size) : 0, (center.x_ozdob_z + neighbourhood_size + 1 < 6) ? (center.x_ozdob_z + neighbourhood_size + 1) : 6);
                x_potraw_z = parent.rand.Next((center.x_potraw_z - neighbourhood_size >= 0) ? (center.x_potraw_z - neighbourhood_size) : 0, (center.x_potraw_z + neighbourhood_size + 1 < 6) ? (center.x_potraw_z + neighbourhood_size + 1) : 6);
                x_potraw_d = parent.rand.Next((center.x_potraw_d - neighbourhood_size >= 0) ? (center.x_potraw_d - neighbourhood_size) : 0, (center.x_potraw_d + neighbourhood_size + 1 < 6) ? (center.x_potraw_z + neighbourhood_size + 1) : 6);
                value = 0; //just to initialize
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
                value = x_gosci * (parent.space_bonus*f_gosci(x_gosci) + 2*parent.cakes*f_i(x_ciast_d + x_ciast_o + x_ciast_u) + parent.beverages*f_i(x_napojow_d + x_napojow_o) + parent.room*f_i(x_ozdob_d + x_ozdob_o + x_ozdob_z) + 3*parent.food*f_i(x_potraw_d + x_potraw_z));
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
                switch(number)
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
        }

        public int SolvingMethod() //solve and return results //todo //it's fucked
        {
            /*
            List<Bee> beeVector = new List<Bee>();
            beeVector.Capacity = 50;
            
            for(int i=0; i<50; i++)
            {
                beeVector.Add(new Bee(this));
                //
            }
            
            int maxIndex = 10;

            foreach(var i in beeVector)
            {
                if (i.check_if_allowed())
                {
                    if (i.evaluate() > maxIndex)
                    {
                        maxIndex = i.evaluate();
                        //System.Console.WriteLine(i);
                    }
                }  
            }
            */

            int k_dozwolonych = 0;
            int k_iteracji = 0;

            while (k_dozwolonych < 2 && k_iteracji < 1000)
            {
                k_iteracji += 1;
                Bee temp = new mmwd.Solver.Bee(this);
                if (temp.check_if_allowed())
                    k_dozwolonych += 1;
                temp.evaluate();
                Console.Out.Write(k_iteracji + "  ");
                Console.Out.Write(k_dozwolonych + "  ");
                Console.Out.Write(temp.value + "  ");
                Console.Out.Write(temp.x_gosci + "  ");
                Console.Out.Write(temp.x_ciast_o + "  ");
                Console.Out.Write(temp.x_ciast_d + "  ");
                Console.Out.Write(temp.x_ciast_u + "  ");
                Console.Out.Write(temp.x_napojow_o + "  ");
                Console.Out.Write(temp.x_napojow_d + "  ");
                Console.Out.Write(temp.x_ozdob_o + "  ");
                Console.Out.Write(temp.x_ozdob_z + "  ");
                Console.Out.Write(temp.x_ozdob_d + "  ");
                Console.Out.Write(temp.x_potraw_z + "  ");
                Console.Out.Write(temp.x_potraw_d + "\n");
            }
            return k_iteracji;
        }
    }
}