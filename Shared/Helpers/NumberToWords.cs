using System;

namespace Shared.Helpers
{
    public static class NumberToWords
    {
        public static string Input(decimal amount)
        {
            int beforeFloatingPoint = (int)Math.Floor(amount);
            string beforeFloatingPointWord = string.Format("{0} Taka", NumericToWord(beforeFloatingPoint));
            string afterFloatingPointWord = string.Format("{0} Paisa Only.", SmallNumberToWord((int)((amount - beforeFloatingPoint) * 100), ""));
            if ((int)((amount - beforeFloatingPoint) * 100) > 0) {
                return string.Format("{0} and {1}", beforeFloatingPointWord, afterFloatingPointWord);
            }
            else {
                return string.Format("{0} only", beforeFloatingPointWord);
            }
        }
        private static string NumericToWord(int number)
        {
            if (number == 0)
                return "zero";

            if (number < 0)
                return "minus " + NumericToWord(Math.Abs(number));

            var words = "";

            if (number / 10000000 > 0) {
                words += NumericToWord(number / 10000000) + " Crore ";
                number %= 10000000;
            }

            if (number / 100000 > 0 && number < 10000000) {
                words += NumericToWord(number / 100000) + " Lac ";
                number %= 100000;
            }

            if (number / 1000 > 0 && number < 100000) {
                words += NumericToWord(number / 1000) + " Thousand ";
                number %= 1000;
            }

            if (number / 100 > 0 && number < 1000) {
                words += NumericToWord(number / 100) + " Hundred ";
                number %= 100;
            }

            words = SmallNumberToWord(number, words);

            return words;
        }
        private static string SmallNumberToWord(int number, string words)
        {
            if (number <= 0) return words;
            if (words != "")
                words += " ";

            var unitsMap = new[] { "Zero", "One", "Two", "Three", "Four", "Five", "Six", "Seven", "Eight", "Nine", "Ten", "Eleven", "Twelve", "Thirteen", "Fourteen", "Fifteen", "Sixteen", "Seventeen", "Eighteen", "Nineteen" };
            var tensMap = new[] { "Zero", "Ten", "Twenty", "Thirty", "Forty", "Fifty", "Sixty", "Seventy", "Eighty", "Ninety" };

            if (number < 20)
                words += unitsMap[number];
            else {
                words += tensMap[number / 10];
                if ((number % 10) > 0)
                    words += " " + unitsMap[number % 10];
            }
            return words;
        }
    }
}
