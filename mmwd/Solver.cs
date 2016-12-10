using System.Collections.Generic;
using System;


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
                //generating random values of variables from neighbourhood
                x_gosci = parent.rand.Next(center.x_gosci - neighbourhood_size, center.x_gosci + neighbourhood_size + 1);
                x_ciast_o = parent.rand.Next(center.x_ciast_o - neighbourhood_size, center.x_ciast_o + neighbourhood_size + 1);
                x_ciast_d = parent.rand.Next(center.x_ciast_d - neighbourhood_size, center.x_ciast_d + neighbourhood_size + 1);
                x_ciast_u = parent.rand.Next(center.x_ciast_u - neighbourhood_size, center.x_ciast_u + neighbourhood_size + 1);
                x_napojow_o = parent.rand.Next(center.x_napojow_o - neighbourhood_size, center.x_napojow_o + neighbourhood_size + 1);
                x_napojow_d = parent.rand.Next(center.x_napojow_d - neighbourhood_size, center.x_napojow_d + neighbourhood_size + 1);
                x_ozdob_o = parent.rand.Next(center.x_ozdob_o- neighbourhood_size, center.x_ozdob_o + neighbourhood_size + 1);
                x_ozdob_d = parent.rand.Next(center.x_ozdob_d - neighbourhood_size, center.x_ozdob_d + neighbourhood_size + 1);
                x_ozdob_z = parent.rand.Next(center.x_ozdob_z - neighbourhood_size, center.x_ozdob_z + neighbourhood_size + 1);
                x_potraw_z = parent.rand.Next(center.x_potraw_z - neighbourhood_size, center.x_potraw_z + neighbourhood_size + 1);
                x_potraw_d = parent.rand.Next(center.x_potraw_d - neighbourhood_size, center.x_potraw_d + neighbourhood_size + 1);
                value = 0; //just to initialize
            }

            public int evaluate() //method evaluating objective function (value)
            {
                value = x_gosci * (space_bonus)
            }
        }

        public int SolvingMethod() //solve and return results //todo
        {
            return people + money + time + numbersOfIterations + numbersOfScouts + numbersOfEliteAreas;
            //testing commits
        }
    }
}