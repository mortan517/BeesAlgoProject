using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace mmwd
{
    public partial class UserInterface : Form
    {
        ProgrammerInterface hiddenInterface; // programmer interface
        Solver solver; // solver
        //System.Windows.Forms.DataVisualization.Charting.Chart myChart;
        

        int people; // how many people do u want to accomodate
        int money; // how much money do u have
        int time; // how much time do u have
        int room; // importance of appropriate ornamention (1-10)
        int cakes; // importance of cakes (1-10)
        int food; // importance of food (1-10)
        int beverages; // importance of beverages (1-10)


        public UserInterface() // constructor
        {
            InitializeComponent();
            hiddenInterface = new ProgrammerInterface(); // create programmer interface
            peopleText.Select(); // set cursor in appropriate box
        }

        private void StartButton_Click(object sender, EventArgs e) // get programmer & user values and send to Solver
        { 
            if (peopleText.Text == "" || moneyText.Text == "" || timeText.Text == "" || roomText.Text == ""
               || cakesText.Text == "" || foodText.Text == "" || beveragesText.Text == "")
            {
                System.Windows.Forms.MessageBox.Show("Zostawiłeś puste pole!");
            }
            else if (peopleText.Text.All(Char.IsDigit) && moneyText.Text.All(Char.IsDigit) &&
                timeText.Text.All(Char.IsDigit) && roomText.Text.All(Char.IsDigit) &&
                cakesText.Text.All(Char.IsDigit) && foodText.Text.All(Char.IsDigit) &&
                beveragesText.Text.All(Char.IsDigit))
            {
                people = Int32.Parse(peopleText.Text);
                money = Int32.Parse(moneyText.Text);
                time = Int32.Parse(timeText.Text);

                room = Int32.Parse(roomText.Text);
                cakes = Int32.Parse(cakesText.Text);
                food = Int32.Parse(foodText.Text);
                beverages = Int32.Parse(beveragesText.Text);
                

                //this.Update();//not necessary

                List<int> userParametersList = new List<int>() { people, money, time, room, cakes, food, beverages }; //1
                List<int> programmerParametersList = hiddenInterface.GetValuesFromProgrammerInterface(); //2

                solver = new Solver(userParametersList, programmerParametersList, ref MainChart); //3
                    //transfer parameters from Programer & User Interface and Chart to Solver
                MainChart.Series["Series1"].Points.Clear();
                var solution = solver.SolvingMethod(); // get results //4

                string[] tab = new string[] { "Wartość funkcji celu: ",
                                              "Liczba iteracji: ",
                                              "Ciasta dostarczone: ",
                                              "Ciasta odebrane: ",
                                              "Ciasta upieczone: ",
                                              "Liczba gości: ",
                                              "Napoje dostarczone: ",
                                              "Napoje odebrane: ",
                                              "Ozdoby dostarczone: ",
                                              "Ozdoby odebrane: ",
                                              "Ozdoby zrobione: ",
                                              "Potrawy dostarczone: ",
                                              "Potrawy zrobione: "};

                string tempString;
                tempString = "";

                for (int i = 0; i < solution.Count; i++)
                {
                    tempString += tab[i];
                    tempString += solution[i].ToString();
                    tempString += '\n';

                    if (1 == i)
                        tempString += '\n';
                }
                System.Windows.Forms.MessageBox.Show(tempString); //show results //5

            }
            else
            {
                System.Windows.Forms.MessageBox.Show("Dane mają być liczbami naturalnymi!");
            }

            //this.Update();
        }

        private void buttonOpenProgrammerInterface_Click(object sender, EventArgs e)
        {
            if (password.Text == "1234")
                hiddenInterface.Show();
        }
    }
}
