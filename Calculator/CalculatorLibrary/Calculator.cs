﻿using System;
using System.Collections.Generic;
using System.Text;

namespace CalculatorLibrary
{
    public class Calculator
    {
        public enum Operation
        {
            None,
            Add,
            Subtract,
            Multiply,
            Divide
        };
        public string InputString { get; set; } = "0";
        public double TotalValue { get; set; } = 0;
        public double MemoryValue { get; set; } = 0;
        public Operation CurrentOperation { get; set; } = Operation.None;

        public void AppendDigit(int digit)
        {
            if (digit < 0 || digit > 9)
            {
                throw new ArgumentOutOfRangeException();
            }
            if (InputString == "0")
            {
                InputString = digit.ToString();
            }
            else
            {
                InputString += digit;
            }
        }

        public void AppendDecimal()
        {
            if (InputString.IndexOf(".") == -1)
            {
                InputString += ".";
            }
        }

        public void RemoveLastDigit()
        {
            if (InputString.Length == 1 || InputString.Length == 2 && InputString[0] == '-')
            {
                InputString = "0";
            }
            else
            {
                InputString = InputString.Substring(0, InputString.Length - 1);
            }
        }

        private double GetInputStringValue()
        {
            if (double.TryParse(InputString, out double output))
            {
                return output;
            }
            return InputString[0] == '-' ? double.MinValue : double.MaxValue;
        }

        public void Add()
        {
            CurrentOperation = Operation.Add;
            TotalValue += GetInputStringValue();
        }

        public void Subtract()
        {
            CurrentOperation = Operation.Subtract;
            TotalValue -= GetInputStringValue();
        }

        public void Multiply()
        {
            CurrentOperation = Operation.Multiply;
            double total = TotalValue * GetInputStringValue();
            if (double.IsInfinity(total))
            {
                total = double.IsPositiveInfinity(total) ? double.MaxValue : double.MinValue;
            }
            TotalValue = total;
        }

        public void Divide()
        {
            CurrentOperation = Operation.Divide;
            if (InputString == "0")
            {
                throw new DivideByZeroException();
            }
            TotalValue /= GetInputStringValue();
        }

        public void MultiplicitiveInverse()
        {
            if (InputString == "0")
            {
                throw new DivideByZeroException();
            }
            InputString = (1 / GetInputStringValue()).ToString();
        }

        public void AdditiveInverse()
        {
            InputString = (-1 * GetInputStringValue()).ToString();
        }

        public void Percent()
        {
            InputString = (TotalValue / 100 * GetInputStringValue()).ToString();
        }

        public void Calculate()
        {
            switch(CurrentOperation)
            {
                default:
                    throw new NotImplementedException();
                case Operation.None:
                    // do nothing
                    break;
                case Operation.Add:
                    Add();
                    break;
                case Operation.Subtract:
                    Subtract();
                    break;
                case Operation.Multiply:
                    Multiply();
                    break;
                case Operation.Divide:
                    Divide();
                    break;
            }
        }

        public void SquareRoot()
        {
            if (InputString[0] == '-')
            {
                throw new ArgumentOutOfRangeException();
            }
            InputString = Math.Sqrt(GetInputStringValue()).ToString();
        }

        public void ClearEntry()
        {
            InputString = "0";
        }

        public void Clear()
        {
            //!!! stub
        }

        public void MemorySave()
        {
            //!!! stub
        }

        public void MemoryClear()
        {
            //!!! stub
        }

        public void MemoryReturn()
        {
            //!!! stub
        }

        public void MemoryAdd()
        {
            //!!! stub
        }

        public void MemorySubtract()
        {
            //!!! stub
        }
    }
}
