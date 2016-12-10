using System.Collections.Generic;
//test
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

        int numbersOfIterations;
        int numbersOfScouts;
        int numbersOfEliteAreas;
        int numbersOfSelectedAreas;
        int beesInEliteAreas;
        int beesInSelectedAreas;
        int iterationsWithoutImprovement;


        public Solver(List<int> userParametersList, List<int> programmerParametersList) //constructor
        {
            people = userParametersList[0];
            money = userParametersList[1];
            time = userParametersList[2];
            room = userParametersList[3];
            cakes = userParametersList[4];
            food = userParametersList[5];
            beverages = userParametersList[6];

            numbersOfIterations = programmerParametersList[0];
            numbersOfScouts = programmerParametersList[1];
            numbersOfEliteAreas = programmerParametersList[2];
            numbersOfSelectedAreas = programmerParametersList[3];
            beesInEliteAreas = programmerParametersList[4];
            beesInSelectedAreas = programmerParametersList[5];
            iterationsWithoutImprovement = programmerParametersList[6];
        }

        public int SolvingMethod() //solve and return results //todo
        {
            return people + money + time + numbersOfIterations + numbersOfScouts + numbersOfEliteAreas;
            //testing commits
        }
    }
}