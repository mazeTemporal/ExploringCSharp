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
            //!!! stub
        }

        public void Add()
        {
            //!!! stub
        }

        public void Subtract()
        {
            //!!! stub
        }

        public void Multiply()
        {
            //!!! stub
        }

        public void Divide()
        {
            //!!! stub
        }

        public void MultiplicitiveInverse()
        {
            //!!! stub
        }

        public void AdditiveInverse()
        {
            //!!! stub
        }

        public void Percent()
        {
            //!!! stub
        }

        public void Calculate()
        {
            //!!! stub
        }

        public void SquareRoot()
        {
            //!!! stub
        }

        public void ClearEntry()
        {
            //!!! stub
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
