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
        [Theory]
        [InlineData(0)]
        [InlineData(5)]
        public void AppendDigit_ShouldReplaceInputStringZero(int digit)
        {
            // Arrange
            Calculator calc = new Calculator()
            {
                InputString = "0"
            };

            // Act
            calc.AppendDigit(digit);

            // Assert
            Assert.Equal(digit.ToString(), calc.InputString);
        }

        [Theory]
        [InlineData("3", 0, "30")]
        [InlineData("5", 2, "52")]
        [InlineData("6.", 0, "6.0")]
        [InlineData("-8", 9, "-89")]
        [InlineData("-45.7", 1, "-45.71")]
        public void AppendDigit_ShouldModifyInputString(string inputString, int digit, string expected)
        {
            // Arrange
            Calculator calculator = new Calculator()
            {
                InputString = inputString
            };

            // Act
            calculator.AppendDigit(digit);

            // Assert
            Assert.Equal(expected, calculator.InputString);
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
        [InlineData("0", "0.")]
        [InlineData("51", "51.")]
        public void AppendDecimal_ShouldModifyInputString(string inputString, string expected)
        {
            // Arrange
            Calculator calc = new Calculator
            {
                InputString = inputString
            };

            // Act
            calc.AppendDecimal();

            // Assert
            Assert.Equal(expected, calc.InputString);
        }

        [Theory]
        [InlineData("0.")]
        [InlineData("-74.14")]
        public void AppendDecimal_ShouldIgnoreSecondDecimal(string inputString)
        {
            // Arrange
            Calculator calc = new Calculator
            {
                InputString = inputString
            };

            // Act
            calc.AppendDecimal();

            // Assert
            Assert.Equal(inputString, calc.InputString);
        }

        [Theory]
        [InlineData("20", "2")]
        [InlineData("1.5", "1.")]
        [InlineData("84.", "84")]
        [InlineData("-34", "-3")]
        public void RemoveLastDigit_ShouldModifyInputString(string inputString, string expected)
        {
            // Arrange
            Calculator calc = new Calculator()
            {
                InputString = inputString
            };

            // Act
            calc.RemoveLastDigit();

            // Assert
            Assert.Equal(expected, calc.InputString);
        }

        [Theory]
        [InlineData("0")]
        [InlineData("8")]
        [InlineData("-4")]
        public void RemoveLastDigit_ShouldReplaceSingleDigitWithZero(string inputString)
        {
            // Arrange
            Calculator calc = new Calculator()
            {
                InputString = inputString
            };

            // Act
            calc.RemoveLastDigit();

            // Assert
            Assert.Equal("0", calc.InputString);
        }

        [Theory]
        [InlineData("7.94", 5.2, 13.14)]
        [InlineData("47", -4.67, 42.33)]
        [InlineData("-56", 12, -44)]
        [InlineData("14", double.MaxValue, double.MaxValue)]
        [InlineData("-4", double.MinValue, double.MinValue)]
        public void Add_ShouldModifyTotalValue(string inputString, double totalValue, double expected)
        {
            // Arrange
            Calculator calc = new Calculator()
            {
                InputString = inputString,
                TotalValue = totalValue
            };

            // Act
            calc.Add();

            // Assert
            Assert.Equal(expected, calc.TotalValue);
        }

        [Theory]
        [InlineData("7.94", 5.2, -2.74)]
        [InlineData("4.67", 47, 42.33)]
        [InlineData("-56", 12, 68)]
        [InlineData("-14", double.MaxValue, double.MaxValue)]
        [InlineData("4", double.MinValue, double.MinValue)]
        public void Subtract_ShouldModifyTotalValue(string inputString, double totalValue, double expected)
        {
            // Arrange
            Calculator calc = new Calculator()
            {
                InputString = inputString,
                TotalValue = totalValue
            };

            // Act
            calc.Subtract();

            // Assert
            Assert.Equal(expected, calc.TotalValue);
        }

        [Theory]
        [InlineData("5", 0, 0)]
        [InlineData("0", 7, 0)]
        [InlineData("-8", -4, 32)]
        [InlineData("-8", 4, -32)]
        [InlineData("8", -4, -32)]
        [InlineData("6.9", 1.5, 10.35)]
        [InlineData("3", double.MaxValue, double.MaxValue)]
        [InlineData("3", double.MinValue, double.MinValue)]
        [InlineData("-3", double.MaxValue, double.MinValue)]
        [InlineData("-3", double.MinValue, double.MaxValue)]
        public void Multiply_ShouldModifyTotalValue(string inputString, double totalValue, double expected)
        {
            // Arrange
            Calculator calc = new Calculator()
            {
                InputString = inputString,
                TotalValue = totalValue
            };

            // Act
            calc.Multiply();

            // Assert
            Assert.Equal(expected, calc.TotalValue);
        }
    }
}
