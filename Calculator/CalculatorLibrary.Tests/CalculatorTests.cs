using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CalculatorLibrary;
using Xunit;

namespace CalculatorLibrary.Tests
{
    public class CalculatorTests
    {
        const int DOUBLE_PRECISION = 14;

        [Theory]
        [InlineData(true,  true,  15, 23, "0.7", "0.7")]
        [InlineData(true,  false, 15, 23, "0.7", "0.7")]
        [InlineData(false, true,  15, 23, "0.7", "23")]
        [InlineData(false, false, 15, 23, "0.7", "15")]
        public void DisplayValue_ShouldReturnCorrectValue( bool isEntryMode, bool shouldOverwriteOperation,
            double totalValue, double operationValue, string entryString,
            string expected)
        {
            // Arrange
            Calculator calc = new Calculator()
            {
                IsEntryMode = isEntryMode,
                ShouldOverwriteOperation = shouldOverwriteOperation,
                EntryString = entryString,
                TotalValue = totalValue,
                OperationValue = operationValue
            };

            // Assert
            Assert.Equal(expected, calc.DisplayValue);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(5)]
        public void AppendDigit_ShouldReplaceEntryStringZero(int digit)
        {
            // Arrange
            Calculator calc = new Calculator()
            {
                EntryString = "0"
            };

            // Act
            calc.AppendDigit(digit);

            // Assert
            Assert.Equal(digit.ToString(), calc.EntryString);
        }

        [Theory]
        [InlineData("3", 0, "30")]
        [InlineData("5", 2, "52")]
        [InlineData("6.", 0, "6.0")]
        [InlineData("-8", 9, "-89")]
        [InlineData("-45.7", 1, "-45.71")]
        public void AppendDigit_ShouldModifyEntryString(string entryString, int digit, string expected)
        {
            // Arrange
            Calculator calculator = new Calculator()
            {
                EntryString = entryString
            };

            // Act
            calculator.AppendDigit(digit);

            // Assert
            Assert.Equal(expected, calculator.EntryString);
        }

        [Theory]
        [InlineData(true,  5, 5, 4, 1)]
        [InlineData(false, 5, 1, 4, 4)]
        public void AppendDigit_ShouldUpdateCorrectValue(bool shouldOverwriteOperation,
            double totalValue, double expectedTotal,
            double operationValue, double expectedOperation)
        {
            // Arrange
            string entryString = "0";
            int digit = 1;
            Calculator calculator = new Calculator()
            {
                EntryString = entryString,
                ShouldOverwriteOperation = shouldOverwriteOperation,
                TotalValue = totalValue,
                OperationValue = operationValue
            };

            // Act
            calculator.AppendDigit(digit);

            // Assert
            Assert.Equal(expectedTotal, calculator.TotalValue);
            Assert.Equal(expectedOperation, calculator.OperationValue);
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(10)]
        public void AppendDigit_ShouldThrowIfNotValidDigit(int digit)
        {
            // Arrange
            Calculator calc = new Calculator();

            // Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => calc.AppendDigit(digit));
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void AppendDigit_ShouldSetIsEntryModeTrue(bool isEntryMode)
        {
            // Arrange
            int digit = 5;
            Calculator calc = new Calculator()
            {
                IsEntryMode = isEntryMode
            };

            // Act
            calc.AppendDigit(digit);

            // Assert
            Assert.True(calc.IsEntryMode);
        }

        [Theory]
        [InlineData("0", "0.")]
        [InlineData("51", "51.")]
        public void AppendDecimal_ShouldModifyEntryString(string entryString, string expected)
        {
            // Arrange
            Calculator calc = new Calculator
            {
                EntryString = entryString
            };

            // Act
            calc.AppendDecimal();

            // Assert
            Assert.Equal(expected, calc.EntryString);
        }

        [Theory]
        [InlineData("0.")]
        [InlineData("-74.14")]
        public void AppendDecimal_ShouldIgnoreSecondDecimal(string entryString)
        {
            // Arrange
            Calculator calc = new Calculator
            {
                EntryString = entryString
            };

            // Act
            calc.AppendDecimal();

            // Assert
            Assert.Equal(entryString, calc.EntryString);
        }

        [Theory]
        [InlineData(true,  5, 5, 4, 0)]
        [InlineData(false, 5, 0, 4, 4)]
        public void AppendDecimal_ShouldUpdateCorrectValue(bool shouldOverwriteOperation,
            double totalValue, double expectedTotal,
            double operationValue, double expectedOperation)
        {
            // Arrange
            string entryString = "0";
            Calculator calculator = new Calculator()
            {
                EntryString = entryString,
                ShouldOverwriteOperation = shouldOverwriteOperation,
                TotalValue = totalValue,
                OperationValue = operationValue
            };

            // Act
            calculator.AppendDecimal();

            // Assert
            Assert.Equal(expectedTotal, calculator.TotalValue);
            Assert.Equal(expectedOperation, calculator.OperationValue);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void AppendDecimal_ShouldSetIsEntryModeTrue(bool isEntryMode)
        {
            // Arrange
            Calculator calc = new Calculator()
            {
                IsEntryMode = isEntryMode
            };

            // Act
            calc.AppendDecimal();

            // Assert
            Assert.True(calc.IsEntryMode);
        }

        [Theory]
        [InlineData(true, "20", "2")]
        [InlineData(false, "20", "20")]
        [InlineData(true, "1.5", "1.")]
        [InlineData(true, "84.", "84")]
        [InlineData(false, "84.", "84.")]
        [InlineData(true, "-34", "-3")]
        [InlineData(false, "-34", "-34")]
        public void RemoveLastDigit_ShouldModifyEntryString(bool isEntryMode, string entryString, string expected)
        {
            // Arrange
            Calculator calc = new Calculator()
            {
                EntryString = entryString,
                IsEntryMode = isEntryMode
            };

            // Act
            calc.RemoveLastDigit();

            // Assert
            Assert.Equal(expected, calc.EntryString);
        }

        [Theory]
        [InlineData("0")]
        [InlineData("8")]
        [InlineData("-4")]
        public void RemoveLastDigit_ShouldReplaceSingleDigitWithZero(string entryString)
        {
            // Arrange
            Calculator calc = new Calculator()
            {
                EntryString = entryString,
                IsEntryMode = true
            };

            // Act
            calc.RemoveLastDigit();

            // Assert
            Assert.Equal("0", calc.EntryString);
        }

        [Theory]
        [InlineData(true,  true,  5, 5, 4, 1)]
        [InlineData(true,  false, 5, 1, 4, 4)]
        [InlineData(false, true,  5, 5, 4, 4)]
        [InlineData(false, false, 5, 5, 4, 4)]
        public void RemoveLastDigit_ShouldUpdateCorrectValue(bool isEntryMode, bool shouldOverwriteOperation,
            double totalValue, double expectedTotal,
            double operationValue, double expectedOperation)
        {
            // Arrange
            string entryString = "10";
            Calculator calculator = new Calculator()
            {
                EntryString = entryString,
                IsEntryMode = isEntryMode,
                ShouldOverwriteOperation = shouldOverwriteOperation,
                TotalValue = totalValue,
                OperationValue = operationValue
            };

            // Act
            calculator.RemoveLastDigit();

            // Assert
            Assert.Equal(expectedTotal, calculator.TotalValue);
            Assert.Equal(expectedOperation, calculator.OperationValue);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void ExitEntryMode_ShouldSetIsEntryModeFalse(bool isEntryMode)
        {
            // Arrange
            Calculator calc = new Calculator()
            {
                IsEntryMode = isEntryMode
            };

            // Act
            calc.ExitEntryMode();

            // Assert
            Assert.False(calc.IsEntryMode);
        }

        [Fact]
        public void ExitEntryMode_ShouldSetEntryStringZero()
        {
            // Arrange
            Calculator calc = new Calculator()
            {
                EntryString = "48"
            };

            // Act
            calc.ExitEntryMode();

            // Assert
            Assert.Equal("0", calc.EntryString);
        }

        [Theory]
        [InlineData(7.94, 5.2, 13.14)]
        [InlineData(47, -4.67, 42.33)]
        [InlineData(-56, 12, -44)]
        [InlineData(14, double.MaxValue, double.MaxValue)]
        [InlineData(-4, double.MinValue, double.MinValue)]
        public void Add_ShouldModifyTotalValue(double operationValue, double totalValue, double expected)
        {
            // Arrange
            Calculator calc = new Calculator()
            {
                OperationValue = operationValue,
                TotalValue = totalValue
            };

            // Act
            calc.Add();

            // Assert
            Assert.Equal(expected, calc.TotalValue, DOUBLE_PRECISION);
        }

        [Fact]
        public void Add_ShouldSetCurrentOperation()
        {
            // Arrange
            Calculator.Operation expected = Calculator.Operation.Add;
            Calculator calc = new Calculator()
            {
                CurrentOperation = Calculator.Operation.None
            };

            // Act
            calc.Add();

            // Assert
            Assert.Equal(expected, calc.CurrentOperation);
        }

        [Theory]
        [InlineData(7.94, 5.2, -2.74)]
        [InlineData(4.67, 47, 42.33)]
        [InlineData(-56, 12, 68)]
        [InlineData(-14, double.MaxValue, double.MaxValue)]
        [InlineData(4, double.MinValue, double.MinValue)]
        public void Subtract_ShouldModifyTotalValue(double operationValue, double totalValue, double expected)
        {
            // Arrange
            Calculator calc = new Calculator()
            {
                OperationValue = operationValue,
                TotalValue = totalValue
            };

            // Act
            calc.Subtract();

            // Assert
            Assert.Equal(expected, calc.TotalValue, DOUBLE_PRECISION);
        }

        [Fact]
        public void Subtract_ShouldSetCurrentOperation()
        {
            // Arrange
            Calculator.Operation expected = Calculator.Operation.Subtract;
            Calculator calc = new Calculator()
            {
                CurrentOperation = Calculator.Operation.None
            };

            // Act
            calc.Subtract();

            // Assert
            Assert.Equal(expected, calc.CurrentOperation);
        }

        [Theory]
        [InlineData(5, 0, 0)]
        [InlineData(0, 7, 0)]
        [InlineData(-8, -4, 32)]
        [InlineData(-8, 4, -32)]
        [InlineData(8, -4, -32)]
        [InlineData(6.9, 1.5, 10.35)]
        [InlineData(3, double.MaxValue, double.MaxValue)]
        [InlineData(3, double.MinValue, double.MinValue)]
        [InlineData(-3, double.MaxValue, double.MinValue)]
        [InlineData(-3, double.MinValue, double.MaxValue)]
        public void Multiply_ShouldModifyTotalValue(double operationValue, double totalValue, double expected)
        {
            // Arrange
            Calculator calc = new Calculator()
            {
                OperationValue = operationValue,
                TotalValue = totalValue
            };

            // Act
            calc.Multiply();

            // Assert
            Assert.Equal(expected, calc.TotalValue, DOUBLE_PRECISION);
        }

        [Fact]
        public void Multiply_ShouldSetCurrentOperation()
        {
            // Arrange
            Calculator.Operation expected = Calculator.Operation.Multiply;
            Calculator calc = new Calculator()
            {
                CurrentOperation = Calculator.Operation.None
            };

            // Act
            calc.Multiply();

            // Assert
            Assert.Equal(expected, calc.CurrentOperation);
        }

        [Theory]
        [InlineData(7, 0, 0)]
        [InlineData(-8, -4, 0.5)]
        [InlineData(-8, 4, -0.5)]
        [InlineData(8, -4, -0.5)]
        [InlineData(6.9, 1.5, 1.5 / 6.9)]
        public void Divide_ShouldModifyTotalValue(double operationValue, double totalValue, double expected)
        {
            // Arrange
            Calculator calc = new Calculator()
            {
                OperationValue = operationValue,
                TotalValue = totalValue
            };

            // Act
            calc.Divide();

            // Assert
            Assert.Equal(expected, calc.TotalValue, DOUBLE_PRECISION);
        }

        [Fact]
        public void Divide_ShouldThrowIfInputZero()
        {
            // Arrange
            Calculator calc = new Calculator()
            {
                OperationValue = 0,
                TotalValue = 25
            };

            // Assert
            Assert.Throws<DivideByZeroException>(() => calc.Divide());
        }

        [Fact]
        public void Divide_ShouldSetCurrentOperation()
        {
            // Arrange
            Calculator.Operation expected = Calculator.Operation.Divide;
            Calculator calc = new Calculator()
            {
                CurrentOperation = Calculator.Operation.None,
                OperationValue = 1
            };

            // Act
            calc.Divide();

            // Assert
            Assert.Equal(expected, calc.CurrentOperation);
        }
      
        [Theory]
        [InlineData(true,  5, 5, 4, 0.25)]
        [InlineData(false, 5, 0.2, 4, 4)]
        public void MultiplicitiveInverse_ShouldUpdateCorrectValue(bool shouldOverwriteOperation,
            double totalValue, double expectedTotal,
            double operationValue, double expectedOperation)
        {
            // Arrange
            Calculator calc = new Calculator()
            {
                TotalValue = totalValue,
                OperationValue = operationValue,
                ShouldOverwriteOperation = shouldOverwriteOperation
            };

            // Act
            calc.MultiplicitiveInverse();

            // Assert
            Assert.Equal(expectedTotal, calc.TotalValue);
            Assert.Equal(expectedOperation, calc.OperationValue);
        }

        [Theory]
        [InlineData(false, 5, 0, false)]
        [InlineData(false, 0, 5, true)]
        [InlineData(true,  5, 0, true)]
        [InlineData(true,  0, 5, false)]
        public void MultiplicitiveInverse_ShouldThrowIfValueZero(bool shouldOverwriteOperation,
            double totalValue, double operationValue, bool shouldThrow)
        {
            // Arrange
            Calculator calc = new Calculator()
            {
                TotalValue = totalValue,
                OperationValue = operationValue,
                ShouldOverwriteOperation = shouldOverwriteOperation
            };

            // Assert
            if (shouldThrow)
            {
                Assert.Throws<DivideByZeroException>(() => calc.MultiplicitiveInverse());
            }
            else
            {
                calc.MultiplicitiveInverse();
            }
        }

        [Theory]
        [InlineData(true, "4", "-4")]
        [InlineData(false, "4", "4")]
        [InlineData(true, "-4", "4")]
        [InlineData(false, "-4", "-4")]
        [InlineData(true, "0.18", "-0.18")]
        [InlineData(true, "0.", "-0.")]
        public void AdditiveInverse_ShouldModifyEntryStringIfIsEntryMode(
            bool isEntryMode, string entryString, string expected)
        {
            // Arrange
            Calculator calc = new Calculator()
            {
                EntryString = entryString,
                IsEntryMode = isEntryMode
            };

            // Act
            calc.AdditiveInverse();

            // Assert
            Assert.Equal(expected, calc.EntryString);
        }

        [Fact]
        public void AdditiveInverse_ShouldIgnoreEntryStringZero()
        {
            // Arrange
            Calculator calc = new Calculator()
            {
                EntryString = "0",
                IsEntryMode = true
            };

            // Act
            calc.AdditiveInverse();

            // Assert
            Assert.Equal("0", calc.EntryString);
        }

        [Theory]
        [InlineData(true, 5, 5, 4, -4)]
        [InlineData(false, 5, -5, 4, 4)]
        public void AdditiveInverse_ShouldUpdateCorrectValue(bool shouldOverwriteOperation,
            double totalValue, double expectedTotal,
            double operationValue, double expectedOperation)
        {
            // Arrange
            Calculator calc = new Calculator()
            {
                TotalValue = totalValue,
                OperationValue = operationValue,
                ShouldOverwriteOperation = shouldOverwriteOperation
            };

            // Act
            calc.AdditiveInverse();

            // Assert
            Assert.Equal(expectedTotal, calc.TotalValue);
            Assert.Equal(expectedOperation, calc.OperationValue);
        }

        [Theory]
        [InlineData(true,  5, 5, 4, 0.2)]
        [InlineData(false, 5, 0.25, 4, 4)]
        public void Percent_ShouldUpdateCorrectValue(bool shouldOverwriteOperation,
            double totalValue, double expectedTotal,
            double operationValue, double expectedOperation)
        {
            // Arrange
            Calculator calc = new Calculator()
            {
                TotalValue = totalValue,
                OperationValue = operationValue,
                ShouldOverwriteOperation = shouldOverwriteOperation
            };

            // Act
            calc.Percent();

            // Assert
            Assert.Equal(expectedTotal, calc.TotalValue);
            Assert.Equal(expectedOperation, calc.OperationValue);
        }

        [Theory]
        [InlineData(Calculator.Operation.None, 20)]
        [InlineData(Calculator.Operation.Add, 25)]
        [InlineData(Calculator.Operation.Subtract, 15)]
        [InlineData(Calculator.Operation.Multiply, 100)]
        [InlineData(Calculator.Operation.Divide, 4)]
        public void Calculate_ShouldCallCorrectOperation(Calculator.Operation operation, double expectedTotal)
        {
            // Arrange
            Calculator calc = new Calculator()
            {
                CurrentOperation = operation,
                OperationValue = 5,
                TotalValue = 20
            };

            // Act
            calc.Calculate();

            // Assert
            Assert.Equal(expectedTotal, calc.TotalValue, DOUBLE_PRECISION);
        }

        [Theory]
        [InlineData(true,  25, 25, 16, 4)]
        [InlineData(false, 25, 5, 16, 16)]
        public void SquareRoot_ShouldUpdateCorrectValue(bool shouldOverwriteOperation,
            double totalValue, double expectedTotal,
            double operationValue, double expectedOperation)
        {
            // Arrange
            Calculator calc = new Calculator()
            {
                TotalValue = totalValue,
                OperationValue = operationValue,
                ShouldOverwriteOperation = shouldOverwriteOperation
            };

            // Act
            calc.SquareRoot();

            // Assert
            Assert.Equal(expectedTotal, calc.TotalValue);
            Assert.Equal(expectedOperation, calc.OperationValue);
        }

        [Theory]
        [InlineData(false,  1, -1, false)]
        [InlineData(false, -1,  1, true)]
        [InlineData(true,   1, -1, true)]
        [InlineData(true,  -1,  1, false)]
        public void SquareRoot_ShouldThrowIfValueNegative(bool shouldOverwriteOperation,
            double totalValue, double operationValue, bool shouldThrow)
        {
            // Arrange
            Calculator calc = new Calculator()
            {
                TotalValue = totalValue,
                OperationValue = operationValue,
                ShouldOverwriteOperation = shouldOverwriteOperation
            };

            // Assert
            if (shouldThrow)
            {
                Assert.Throws<ArgumentOutOfRangeException>(() => calc.SquareRoot());
            }
            else
            {
                calc.SquareRoot();
            }
        }

        [Fact]
        public void ClearEntry_ShouldModifyEntryString()
        {
            // Arrange
            Calculator calc = new Calculator()
            {
                EntryString = "15.8"
            };

            // Act
            calc.ClearEntry();

            // Assert
            Assert.Equal("0", calc.EntryString);
        }

        [Theory]
        [InlineData(true, 5, 5, 4, 0)]
        [InlineData(false, 5, 0, 4, 4)]
        public void ClearEntry_ShouldUpdateCorrectValue(bool shouldOverwriteOperation,
            double totalValue, double expectedTotal,
            double operationValue, double expectedOperation)
        {
            // Arrange
            Calculator calc = new Calculator()
            {
                TotalValue = totalValue,
                OperationValue = operationValue,
                ShouldOverwriteOperation = shouldOverwriteOperation
            };

            // Act
            calc.ClearEntry();

            // Assert
            Assert.Equal(expectedTotal, calc.TotalValue);
            Assert.Equal(expectedOperation, calc.OperationValue);
        }

        [Fact]
        public void Clear_ShouldResetProperties()
        {
            // Arrange
            Calculator calc = new Calculator()
            {
                EntryString = "15.8",
                TotalValue = 8.1,
                OperationValue = 3.7,
                CurrentOperation = Calculator.Operation.Multiply,
                MemoryValue = 23.4
            };

            // Act
            calc.Clear();

            // Assert
            Assert.Equal("0", calc.EntryString);
            Assert.Equal(0, calc.TotalValue);
            Assert.Equal(0, calc.OperationValue);
            Assert.Equal(0, calc.MemoryValue);
            Assert.Equal(Calculator.Operation.None, calc.CurrentOperation);
        }

        [Theory]
        [InlineData(true, 5, 4, 4)]
        [InlineData(false, 5, 4, 5)]
        public void MemorySave_ShouldUpdateMemoryValueWithCorrectValue(bool shouldOverwriteOperation,
            double totalValue, double operationValue, double expected)
        {
            // Arrange
            Calculator calc = new Calculator()
            {
                TotalValue = totalValue,
                OperationValue = operationValue,
                MemoryValue = 0,
                ShouldOverwriteOperation = shouldOverwriteOperation
            };

            // Act
            calc.MemorySave();

            // Assert
            Assert.Equal(expected, calc.MemoryValue, DOUBLE_PRECISION);
        }

        [Fact]
        public void MemoryClear_ShouldModifyMemoryValue()
        {
            // Arrange
            Calculator calc = new Calculator()
            {
                MemoryValue = -45.8
            };

            // Act
            calc.MemoryClear();

            // Assert
            Assert.Equal(0, calc.MemoryValue);
        }

        [Theory]
        [InlineData(true, 5, 5, 4, -2.63)]
        [InlineData(false, 5, -2.63, 4, 4)]
        public void MemoryRecall_ShouldUpdateCorrectValue(bool shouldOverwriteOperation,
            double totalValue, double expectedTotal,
            double operationValue, double expectedOperation)
        {
            // Arrange
            double memoryValue = -2.63;
            Calculator calc = new Calculator()
            {
                MemoryValue = memoryValue,
                TotalValue = totalValue,
                OperationValue = operationValue,
                ShouldOverwriteOperation = shouldOverwriteOperation
            };

            // Act
            calc.MemoryRecall();

            // Assert
            Assert.Equal(expectedTotal, calc.TotalValue);
            Assert.Equal(expectedOperation, calc.OperationValue);
        }

        [Theory]
        [InlineData(true, 5, -4, 10, 6)]
        [InlineData(true, 5, 4, 10, 14)]
        [InlineData(false, 5, -4, 10, 15)]
        [InlineData(false, -5, 4, 10, 5)]
        [InlineData(false, -5, 5, double.MaxValue, double.MaxValue)]
        [InlineData(false, 5, -4, double.MinValue, double.MinValue)]
        public void MemoryAdd_ShouldUpdateMemoryValueWithCorrectValue(bool shouldOverwriteOperation,
            double totalValue, double operationValue, double memoryValue, double expected)
        {
            // Arrange
            Calculator calc = new Calculator()
            {
                TotalValue = totalValue,
                OperationValue = operationValue,
                MemoryValue = memoryValue,
                ShouldOverwriteOperation = shouldOverwriteOperation
            };

            // Act
            calc.MemoryAdd();

            // Assert
            Assert.Equal(expected, calc.MemoryValue, DOUBLE_PRECISION);
        }

        [Theory]
        [InlineData(true, 5, -4, 10, 14)]
        [InlineData(true, 5, 4, 10, 6)]
        [InlineData(false, 5, -4, 10, 5)]
        [InlineData(false, -5, 4, 10, 15)]
        [InlineData(false, 5, -5, double.MaxValue, double.MaxValue)]
        [InlineData(false, -5, 4, double.MinValue, double.MinValue)]
        public void MemorySubtract_ShouldUpdateMemoryValueWithCorrectValue(bool shouldOverwriteOperation,
            double totalValue, double operationValue, double memoryValue, double expected)
        {
            // Arrange
            Calculator calc = new Calculator()
            {
                TotalValue = totalValue,
                OperationValue = operationValue,
                MemoryValue = memoryValue,
                ShouldOverwriteOperation = shouldOverwriteOperation
            };

            // Act
            calc.MemorySubtract();

            // Assert
            Assert.Equal(expected, calc.MemoryValue, DOUBLE_PRECISION);
        }
    }
}
