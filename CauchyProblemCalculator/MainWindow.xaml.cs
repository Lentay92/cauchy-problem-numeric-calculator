using System;
using System.Windows;
using System.Windows.Input;
using System.Text.RegularExpressions;
using System.Collections.Generic;

namespace CauchyProblem
{
    public partial class MainWindow : Window
    {
        static public float leftLimit;
        static public float rightLimit;
        static public float initialValue;

        static public int splitPointsNum;

        static public string inputFunction;

        readonly Regex Nums = new Regex("[^0-9-,]");
        readonly Regex IntPosNums = new Regex("[^0-9]");
        readonly Regex EquationSymbols = new Regex("[^0-9+--/*()xy]");

        private bool IsSymbol(string text, Regex exp)
        {
            return exp.IsMatch(text);
        }
        
        private void NumInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = IsSymbol(e.Text, Nums);
        }

        private void IntNumInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = IsSymbol(e.Text, IntPosNums);
        }

        private void EquationInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = IsSymbol(e.Text, EquationSymbols);
        }

        private void Input()
        {
            leftLimit = Convert.ToSingle(LLimit_TextBox.Text);
            rightLimit = Convert.ToSingle(RLimit_TextBox.Text);
            splitPointsNum = Convert.ToInt32(Points_TextBox.Text);
            initialValue = Convert.ToSingle(Initial_TextBox.Text);
            
            inputFunction = Regex.Replace(Equation_TextBox.Text, ",", ".");
        }

        private bool IsInputEmpty()
        {
            if (LLimit_TextBox.Text == "" || RLimit_TextBox.Text == "" || Points_TextBox.Text == "" || Initial_TextBox.Text == "")
                return true;
            return false;
        }

        private int IsInputRight()
        {
            if (leftLimit >= rightLimit)
                return 1;
            if (splitPointsNum <= 0 || splitPointsNum % 1 != 0)
                return 2;

            return 0;
        }

        private void Calculate_Button_Click(object sender, RoutedEventArgs e)
        {

            if (!IsInputEmpty())
            {
                Input();

                int index = -1;

                if (IsInputRight() != 0)
                {
                    index = IsInputRight();

                    if (index == 1)
                        MessageBox.Show("Left limit value must be less than right", "Cauchy Problem Calculator", MessageBoxButton.OK, MessageBoxImage.Error);

                    if (index == 2)
                        MessageBox.Show("Split points number must be positive integer", "Cauchy Problem Calculator", MessageBoxButton.OK, MessageBoxImage.Error);

                    return;
                }

                index = Method_ComboBox.SelectedIndex;

                if (index == 0)
                    Result_TextBlock.Text = Convert.ToString(Methods.Euler(splitPointsNum, leftLimit, initialValue));

                if (index == 1)
                    Result_TextBlock.Text = Convert.ToString(Methods.EulerMod(splitPointsNum, leftLimit, initialValue));

                if (index == 2)
                    Result_TextBlock.Text = Convert.ToString(Methods.Cauchy(splitPointsNum, leftLimit, initialValue));

                if (index == 3)
                    Result_TextBlock.Text = Convert.ToString(Methods.AdamsBashforth(splitPointsNum, leftLimit, initialValue));

                if (index == 4)
                    Result_TextBlock.Text = Convert.ToString(Methods.RungeKutta4th(splitPointsNum, leftLimit, initialValue));
            }

            else
                MessageBox.Show("Enter all required data", "Cauchy Problem Calculator", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        public MainWindow() { }
    }
}
