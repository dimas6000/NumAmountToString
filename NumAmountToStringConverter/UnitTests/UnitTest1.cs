using System;
using Xunit;
using NumAmountToStringConverter;

namespace UnitTests
{
    public class UnitTest1
    {
        [Fact]
        public void TestThreedigitToString()
        {
            for (int i = 1; i < 1000; i++)
            {
                Assert.Equal(NumberToWords(i), NumAmountToString.CreateStringFromThreedigitNumber(i));
            }
        }


        [Fact]
        public void TestConvertNumAmountToString()
        {
            Assert.Equal("zero DOLLARS AND one CENTS", NumAmountToString.ConvertNumAmountToString("0.01"));
            Assert.Equal("zero DOLLARS AND ten CENTS", NumAmountToString.ConvertNumAmountToString("0.1"));
            Assert.Equal("zero DOLLARS AND ten CENTS", NumAmountToString.ConvertNumAmountToString("0.10"));
            Assert.Equal("zero DOLLARS", NumAmountToString.ConvertNumAmountToString("0"));
            Assert.Equal("zero DOLLARS AND zero CENTS", NumAmountToString.ConvertNumAmountToString("0.0"));
            Assert.Equal("zero DOLLARS AND zero CENTS", NumAmountToString.ConvertNumAmountToString("0.00"));
            Assert.Equal("one DOLLARS", NumAmountToString.ConvertNumAmountToString("1"));
            Assert.Equal("twenty one DOLLARS", NumAmountToString.ConvertNumAmountToString("21"));
            Assert.Equal("ninety nine DOLLARS", NumAmountToString.ConvertNumAmountToString("99"));
            Assert.Equal("one hundred DOLLARS AND ninety CENTS", NumAmountToString.ConvertNumAmountToString("100.9"));
            Assert.Equal("one thousand DOLLARS", NumAmountToString.ConvertNumAmountToString("1000"));
            Assert.Equal("one thousand DOLLARS AND ninety CENTS", NumAmountToString.ConvertNumAmountToString("1000.9"));
            Assert.Equal("one thousand, one hundred DOLLARS AND ninety CENTS", NumAmountToString.ConvertNumAmountToString("1100.9"));
            Assert.Equal("one thousand and one DOLLARS AND ninety CENTS", NumAmountToString.ConvertNumAmountToString("1001.9"));
            Assert.Equal("one million, three hundred and fifty seven thousand, two hundred and fifty six DOLLARS AND thirty two CENTS", NumAmountToString.ConvertNumAmountToString("1357256.32"));
            Assert.Equal("one million and fifty six DOLLARS AND thirty two CENTS", NumAmountToString.ConvertNumAmountToString("1000056.32"));
            Assert.Equal("one million DOLLARS", NumAmountToString.ConvertNumAmountToString("1000000"));
            Assert.Equal("one million DOLLARS AND twenty CENTS", NumAmountToString.ConvertNumAmountToString("1000000.20"));
            Assert.Equal("two billion DOLLARS", NumAmountToString.ConvertNumAmountToString("2000000000"));

            Assert.Throws<NumAmountToStringException>(() => { NumAmountToString.ConvertNumAmountToString("20000000001"); });
            /* var ex = Assert.Throws<Exception>(() => NumAmountToString.ConvertNumAmountToString("2000000001"));
             Assert.That(ex.Message, Is.EqualTo("Actual exception message"));*/

        }

        // Method for generate expected value for test. Founded on github.
        private string NumberToWords(int number)
        {
            if (number == 0)
                return "zero";

            if (number < 0)
                return "minus " + NumberToWords(Math.Abs(number));

            string words = "";

            if ((number / 1000000000) > 0)
            {
                words += NumberToWords(number / 1000000000) + " billion ";
                number %= 1000000000;
            }

            if ((number / 1000000) > 0)
            {
                words += NumberToWords(number / 1000000) + " million ";
                number %= 1000000;
            }

            if ((number / 1000) > 0)
            {
                words += NumberToWords(number / 1000) + " thousand ";
                number %= 1000;
            }

            if ((number / 100) > 0)
            {
                words += NumberToWords(number / 100) + " hundred ";
                number %= 100;
            }

            if (number > 0)
            {
                if (words != "")
                    words += "and ";

                var unitsMap = new[] { "zero", "one", "two", "three", "four", "five", "six", "seven", "eight", "nine", "ten", "eleven", "twelve", "thirteen", "fourteen", "fifteen", "sixteen", "seventeen", "eighteen", "nineteen" };
                var tensMap = new[] { "zero", "ten", "twenty", "thirty", "forty", "fifty", "sixty", "seventy", "eighty", "ninety" };

                if (number < 20)
                    words += unitsMap[number];
                else
                {
                    words += tensMap[number / 10];
                    if ((number % 10) > 0)
                        words += " " + unitsMap[number % 10];
                }
            }

            return words.Trim();
        }
    }
}
