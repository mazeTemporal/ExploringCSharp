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

        public string GetInputString()
        {
            return calc.InputString;
        }

        public string GetValueString()
        {
            return calc.TotalValue.ToString();
        }

        public void SetScreenContent(string s)
        {
            Screen.Text = s;
        }

        public void SetScreenInput()
        {
            SetScreenContent(GetInputString());
        }

        public void Button_1_Handler(object sender, EventArgs e)
        {
            calc.AppendDigit(1);
            SetScreenInput();
        }
    }
}
