using System;
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
        public double TotalValue { get; set; } = 0;
        public double OperationValue { get; set; } = 0;
        public double MemoryValue { get; set; } = 0;
        public string EntryString = "0";
        public Operation CurrentOperation { get; set; } = Operation.None;
        public bool IsEntryMode = false;
        public bool ShouldOverwriteOperation = false;
        public string DisplayValue
        {
            get
            {
                if (IsEntryMode)
                {
                    return EntryString;
                }
                return (ShouldOverwriteOperation ? OperationValue : TotalValue).ToString();
            }
        }

        public void ExitEntryMode()
        {
            EntryString = "0";
            IsEntryMode = false;
        }

        public void AppendDigit(int digit)
        {
            IsEntryMode = true;
            if (digit < 0 || digit > 9)
            {
                throw new ArgumentOutOfRangeException();
            }
            if (EntryString == "0")
            {
                EntryString = digit.ToString();
            }
            else
            {
                EntryString += digit;
            }
            EntryStringUpdate();
        }

        public void AppendDecimal()
        {
            IsEntryMode = true;
            if (EntryString.IndexOf(".") == -1)
            {
                EntryString += ".";
            }
            EntryStringUpdate();
        }

        public void RemoveLastDigit()
        {
            if (IsEntryMode)
            {
                if (EntryString.Length == 1 || EntryString.Length == 2 && EntryString[0] == '-')
                {
                    EntryString = "0";
                }
                else
                {
                    EntryString = EntryString.Substring(0, EntryString.Length - 1);
                }
                EntryStringUpdate();
            }
        }

        private void EntryStringUpdate()
        {
            double entryValue = double.Parse(EntryString);
            if (ShouldOverwriteOperation)
            {
                OperationValue = entryValue;
            }
            else
            {
                TotalValue = entryValue;
            }
        }

        private double GetEntryStringValue() //!!! this should be removed
        {
            if (double.TryParse(EntryString, out double output))
            {
                return output;
            }
            return EntryString[0] == '-' ? double.MinValue : double.MaxValue;
        }

        public void Add()
        {
            CurrentOperation = Operation.Add;
            TotalValue += OperationValue;
        }

        public void Subtract()
        {
            CurrentOperation = Operation.Subtract;
            TotalValue -= OperationValue;
        }

        public void Multiply()
        {
            CurrentOperation = Operation.Multiply;
            double total = TotalValue * OperationValue;
            if (double.IsInfinity(total))
            {
                total = double.IsPositiveInfinity(total) ? double.MaxValue : double.MinValue;
            }
            TotalValue = total;
        }

        public void Divide()
        {
            CurrentOperation = Operation.Divide;
            if (OperationValue == 0)
            {
                throw new DivideByZeroException();
            }
            TotalValue /= OperationValue;
        }

        public void MultiplicitiveInverse()
        {
            if (ShouldOverwriteOperation)
            {
                if (0 == OperationValue)
                {
                    throw new DivideByZeroException();
                }
                OperationValue = 1 / OperationValue;
            }
            else
            {
                if (0 == TotalValue)
                {
                    throw new DivideByZeroException();
                }
                TotalValue = 1 / TotalValue;
            }
        }

        public void AdditiveInverse()
        {
            if (IsEntryMode && EntryString != "0")
            {
                if (EntryString[0] == '-')
                {
                    EntryString = EntryString.Substring(1);
                }
                else
                {
                    EntryString = "-" + EntryString;
                }
            }

            if (ShouldOverwriteOperation)
            {
                OperationValue *= -1;
            }
            else
            {
                TotalValue *= -1;
            }
        }

        public void Percent()
        {
            if (ShouldOverwriteOperation)
            {
                OperationValue = TotalValue / 100 * OperationValue;
            }
            else
            {
                TotalValue = TotalValue / 100 * TotalValue;
            }
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
            if (ShouldOverwriteOperation)
            {
                if (OperationValue < 0)
                {
                    throw new ArgumentOutOfRangeException();
                }
                OperationValue = Math.Sqrt(OperationValue);
            }
            else
            {
                if (TotalValue < 0)
                {
                    throw new ArgumentOutOfRangeException();
                }
                TotalValue = Math.Sqrt(TotalValue);
            }
        }

        public void ClearEntry()
        {
            EntryString = "0";
            if (ShouldOverwriteOperation)
            {
                OperationValue = 0;
            }
            else
            {
                TotalValue = 0;
            }
        }

        public void Clear()
        {
            ClearEntry();
            TotalValue = 0;
            OperationValue = 0;
            MemoryClear();
            CurrentOperation = Operation.None;
        }

        public void MemorySave()
        {
            if (ShouldOverwriteOperation)
            {
                MemoryValue = OperationValue;
            }
            else
            {
                MemoryValue = TotalValue;
            }
        }

        public void MemoryClear()
        {
            MemoryValue = 0;
        }

        public void MemoryRecall()
        {
            EntryString = MemoryValue.ToString();
        }

        public void MemoryAdd()
        {
            MemoryValue += GetEntryStringValue();
        }

        public void MemorySubtract()
        {
            MemoryValue -= GetEntryStringValue();
        }
    }
}
