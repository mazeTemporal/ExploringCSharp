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
    }
}
