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
    public partial class ProgrammerInterface : Form
    {
        int numbersOfIterations;
        int numbersOfScouts;
        int numbersOfEliteAreas;
        int numbersOfSelectedAreas;
        int beesInEliteAreas;
        int beesInSelectedAreas;
        int iterationsWithoutImprovement;
        
        public ProgrammerInterface() // constructor
        {
            InitializeComponent();
            this.ControlBox = false; // hiding mini,maxi,exit
            //set init values
            //set values on boxes in programmer interface even if not clicked
        }

        private void ClosingButton_Click(object sender, EventArgs e) // get values from interface to implementation
        {
            if(NumberOfIterationsBox.Text == "" || NumbersOfScoutsBox.Text == "" || NumbersOfEliteAreasBox.Text == "" ||
                NumbersOfSelectedAreasBox.Text == "" || BeesInEliteAreasBox.Text == "" ||
                BeesInSelectedAreasBox.Text == "" || IterationsWithoutImprovementBox.Text == "")
            {
                System.Windows.Forms.MessageBox.Show("Zostawiłeś puste pole!");
            }
            else if(NumberOfIterationsBox.Text.All(Char.IsDigit) &&
                NumbersOfScoutsBox.Text.All(Char.IsDigit) &&
                NumbersOfEliteAreasBox.Text.All(Char.IsDigit) &&
                NumbersOfSelectedAreasBox.Text.All(Char.IsDigit) &&
                BeesInEliteAreasBox.Text.All(Char.IsDigit) &&
                BeesInSelectedAreasBox.Text.All(Char.IsDigit) &&
                IterationsWithoutImprovementBox.Text.All(Char.IsDigit) )
            {
                numbersOfIterations = Int32.Parse(NumberOfIterationsBox.Text);
                numbersOfScouts = Int32.Parse(NumbersOfScoutsBox.Text);
                numbersOfEliteAreas = Int32.Parse(NumbersOfEliteAreasBox.Text);
                numbersOfSelectedAreas = Int32.Parse(NumbersOfSelectedAreasBox.Text);
                beesInEliteAreas = Int32.Parse(BeesInEliteAreasBox.Text);
                beesInSelectedAreas = Int32.Parse(BeesInSelectedAreasBox.Text);
                iterationsWithoutImprovement = Int32.Parse(IterationsWithoutImprovementBox.Text);

                //this.Update();//not necessary
                this.Hide();
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("Dane mają być liczbami naturalnymi!");
            }
        }

        public List<int> GetValuesFromProgrammerInterface() //User interface uses this method to get programmer parameters
        {
            return new List<int>()
            {
                numbersOfIterations, numbersOfScouts, numbersOfEliteAreas, numbersOfSelectedAreas,
                beesInEliteAreas, beesInSelectedAreas, iterationsWithoutImprovement
            };
        }
    };
}