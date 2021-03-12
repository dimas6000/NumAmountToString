using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace NumAmountToStringConverter
{
    public static class NumAmountToString
    {
        private static readonly Dictionary<int, string> onesDictionary = new()
        {
            [0] = "zero",
            [1] = "one",
            [2] = "two",
            [3] = "three",
            [4] = "four",
            [5] = "five",
            [6] = "six",
            [7] = "seven",
            [8] = "eight",
            [9] = "nine"
        };

        private static readonly Dictionary<int, string> tensDictionary = new()
        {
            [10] = "ten",
            [11] = "eleven",
            [12] = "twelve",
            [13] = "thirteen",
            [14] = "fourteen",
            [15] = "fifteen",
            [16] = "sixteen",
            [17] = "seventeen",
            [18] = "eighteen",
            [19] = "nineteen",
            [20] = "twenty",
            [30] = "thirty",
            [40] = "forty",
            [50] = "fifty",
            [60] = "sixty",
            [70] = "seventy",
            [80] = "eighty",
            [90] = "ninety",
        };

        public static string CreateStringFromThreedigitNumber(int threedigitNumber)
        {
            string forReturn = "";
            if (threedigitNumber > 0)
            {
                int hundreds = threedigitNumber / 100;
                int lessThanHundred = threedigitNumber % 100;
                string hundredsString = "";
                string otherString = "";

                if (hundreds > 0)
                    hundredsString = $"{onesDictionary[hundreds]} hundred";

                if (lessThanHundred > 9)
                {
                    otherString = tensDictionary.ContainsKey(lessThanHundred)
                        ? $"{tensDictionary[lessThanHundred]}"
                        : $"{tensDictionary[lessThanHundred / 10 * 10]} {onesDictionary[lessThanHundred % 10]}";
                }
                else if (lessThanHundred != 0)
                {
                    otherString = onesDictionary[lessThanHundred];
                }

                forReturn = hundredsString + (hundredsString != string.Empty && otherString != string.Empty ? " and " : " ") + otherString;

            }
            // todo: проверка на число символов, должно быть трехзначное число, не более. Иначе exception.

            return forReturn.Trim();
        }
        /// <summary>
        /// Convert amount in numbers to amount in words.
        /// </summary>
        /// <param name="inputNumber">String with number in format 123.12</param>
        /// <returns>Amount in words.</returns>
        public static string ConvertNumAmountToString(string inputNumber)
        {
            if (inputNumber == "0")
                return "zero DOLLARS";

            if (inputNumber == "0.0" || inputNumber == "0.00")
                return "zero DOLLARS AND zero CENTS";

            if (!Regex.IsMatch(inputNumber, @"^[0-9]+(?:[.][0-9]{1,2})?\z"))
                throw new NumAmountToStringException("Invalid number format.");

            var splitNumber = inputNumber.Split('.');

            if (splitNumber[0].Length > 10 || Convert.ToInt64(splitNumber[0]) > 2000000000)
                throw new NumAmountToStringException("The number is over the limit.");

            var dollarsAmount = Convert.ToInt32(splitNumber[0]);
            var centsAmount = 0;
            if (splitNumber.Length > 1)
                centsAmount = splitNumber[1].Length == 1 ? Convert.ToInt32(splitNumber[1]) * 10 : Convert.ToInt32(splitNumber[1]);

            var scalesIntDictionary = new Dictionary<string, int>
            {
                { "billion", dollarsAmount / 1000000000 },
                { "million", dollarsAmount % 1000000000 / 1000000 },
                { "thousand", dollarsAmount % 1000000 / 1000 },
                { "DOLLARS", dollarsAmount % 1000 }
            };
            if (splitNumber.Length > 1)
            {
                scalesIntDictionary.Add("CENTS", centsAmount % 100);
            }

            var scalesStringDictionary = new Dictionary<string, string>();
            foreach (var intScale in scalesIntDictionary)
            {
                if (intScale.Value > 0 || intScale.Key == "DOLLARS")
                {
                    if (dollarsAmount == 0 && intScale.Key == "DOLLARS")
                        scalesStringDictionary.Add(intScale.Key, "zero");
                    else
                        scalesStringDictionary.Add(intScale.Key, CreateStringFromThreedigitNumber(intScale.Value));
                }
            }

            var forReturn = "";
            foreach (var stringScale in scalesStringDictionary)
            {
                if (stringScale.Key != "CENTS" && stringScale.Key != "DOLLARS")
                    forReturn += $"{stringScale.Value} {stringScale.Key}, ";
                else
                {
                    if (scalesIntDictionary["DOLLARS"] >= 0 && scalesIntDictionary["DOLLARS"] < 100 && stringScale.Key == "DOLLARS" && dollarsAmount > 999)
                    {
                        forReturn = scalesIntDictionary["DOLLARS"] == 0
                            ? forReturn.TrimEnd(new char[] { ' ', ',' })
                            : forReturn.TrimEnd(new char[] { ' ', ',' }) + " and ";
                    }

                    forReturn += scalesStringDictionary.ContainsKey("CENTS") && stringScale.Key == "DOLLARS"
                        ? $"{stringScale.Value} {stringScale.Key} AND "
                        : $"{stringScale.Value} {stringScale.Key} ";
                }
            }
            return forReturn.Trim();
        }
    }
}
