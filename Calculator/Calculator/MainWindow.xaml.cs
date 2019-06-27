using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using CalculatorLibrary;

namespace Calculator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public CalculatorLibrary.Calculator calc = new CalculatorLibrary.Calculator();

        public MainWindow()
        {
            InitializeComponent();
        }

        private string GetInputString()
        {
            return calc.InputString;
        }

        private string GetTotalValueString()
        {
            return calc.TotalValue.ToString();
        }

        private void SetScreenContent(string s)
        {
            Screen.Text = s;
        }

        private void SetScreenInput()
        {
            SetScreenContent(GetInputString());
        }

        private void SetScreenTotal()
        {
            SetScreenContent(GetTotalValueString());
        }

        private void AppendDigit(int digit)
        {
            calc.AppendDigit(digit);
            SetScreenInput();
        }

        private void SetNextOperation(CalculatorLibrary.Calculator.Operation operation)
        {
            calc.CurrentOperation = operation;
            Operate();
        }

        private void Operate()
        {
            calc.Calculate();
            SetScreenTotal();
        }

        // Button Handlers
        private void Button_0_Click(object sender, RoutedEventArgs e) => AppendDigit(0);
        private void Button_1_Click(object sender, RoutedEventArgs e) => AppendDigit(1);
        private void Button_2_Click(object sender, RoutedEventArgs e) => AppendDigit(2);
        private void Button_3_Click(object sender, RoutedEventArgs e) => AppendDigit(3);
        private void Button_4_Click(object sender, RoutedEventArgs e) => AppendDigit(4);
        private void Button_5_Click(object sender, RoutedEventArgs e) => AppendDigit(5);
        private void Button_6_Click(object sender, RoutedEventArgs e) => AppendDigit(6);
        private void Button_7_Click(object sender, RoutedEventArgs e) => AppendDigit(7);
        private void Button_8_Click(object sender, RoutedEventArgs e) => AppendDigit(8);
        private void Button_9_Click(object sender, RoutedEventArgs e) => AppendDigit(9);

        private void Button_Dot_Click(object sender, RoutedEventArgs e)
        {
            calc.AppendDecimal();
            SetScreenInput();
        }

        private void Button_Plus_Click(object sender, RoutedEventArgs e) =>
            SetNextOperation(CalculatorLibrary.Calculator.Operation.Add);

        private void Button_Minus_Click(object sender, RoutedEventArgs e) =>
            SetNextOperation(CalculatorLibrary.Calculator.Operation.Subtract);

        private void Button_Multiply_Click(object sender, RoutedEventArgs e) =>
            SetNextOperation(CalculatorLibrary.Calculator.Operation.Multiply);

        private void Button_Divide_Click(object sender, RoutedEventArgs e) =>
            SetNextOperation(CalculatorLibrary.Calculator.Operation.Divide);

        private void Button_Equals_Click(object sender, RoutedEventArgs e) =>
            Operate();

        private void Button_Percent_Click(object sender, RoutedEventArgs e)
        {
            calc.Percent();
            SetScreenInput();
        }

        private void Button_Additive_Inverse_Click(object sender, RoutedEventArgs e)
        {
            calc.AdditiveInverse();
            SetScreenInput();
        }

        private void Button_Multiplicitive_Inverse_Click(object sender, RoutedEventArgs e)
        {
            calc.MultiplicitiveInverse();
            SetScreenInput();
        }

        private void Button_Square_Root_Click(object sender, RoutedEventArgs e)
        {
            calc.SquareRoot();
            SetScreenInput();
        }
    }
}
