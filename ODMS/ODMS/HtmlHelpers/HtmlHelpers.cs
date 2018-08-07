using System;
using System.Text;

namespace ODMS.HtmlHelpers
{
    public static class HtmlHelpers
    {
        public static string EmptyIfZero(this int value)
        {
            if (value == 0)
                return string.Empty;

            return value.ToString();
        }

        
        public static string NumberToWords(this int number)
        {
            if (number == 0)
                return "শূন্য";

            if (number < 0)
                return "Minus " + NumberToWords(Math.Abs(number));

            string words = "";

            if ((number / 10000000) > 0)
            {
                words += NumberToWords(number / 10000000) + " কোটি ";
                number %= 10000000;
            }

            if ((number / 100000) > 0)
            {
                words += NumberToWords(number / 100000) + " লক্ষ ";
                number %= 100000;
            }

            if ((number / 1000) > 0)
            {
                words += NumberToWords(number / 1000) + " হাজার ";
                number %= 1000;
            }

            if ((number / 100) > 0)
            {
                words += NumberToWords(number / 100) + " শত ";
                number %= 100;
            }

            if (number > 0)
            {
                if (words != "")
                    words += " ";

                var unitsMap = new[] { "শূন্য","এক ","দুই ","তিন","চার","পাঁচ","ছয় ","সাত ","আট ","নয়","দশ","এগার ","বারো "," তের"," চৌদ্দ","পনের","ষোল ","সতের  ","আঠারো","উনিশ","বিশ ","একুশ ","বাইশ  ","তেইশ  ","চব্বিশ  ","পঁচিশ  ","ছাব্বিশ  ","সাতাশ  ","আটাশ  ","ঊনত্রিশ  ","ত্রিশ ","একত্রিশ","বত্রিশ","তেত্রিশ","চৌত্রিশ","পঁয়ত্রিশ","ছত্রিশ","সাইত্রিশ","আটত্রিশ","ঊনচল্লিশ","চল্লিশ","একচল্লিশ","বিয়াল্লিশ","তেতাল্লিশ","চুয়াল্লিশ","পঁয়তাল্লিশ","ছিচল্লিশ","সাতচল্লিশ","আটচল্লিশ","ঊনপঞ্চাশ","পঞ্চাশ","একান্ন","বাহান্ন","তেপ্পান্ন","চুয়ান্ন","পঞ্চান্ন","ছাপ্পান্ন"," সাতান্ন "," আটান্ন   ","ঊনষাট ","ষাট","একষট্টি","বাষট্টি","তেষট্টি","চৌষট্টি","পঁয়ষট্টি","ছেষট্টি","সাতষট্টি","আটষট্টি","উনসত্তুর","সত্তর","একাত্তর ","বাহাত্তর","তেহাত্তুর","চুহাত্তর ","পচাত্তর","ছিহাত্তর","সাতাত্তর","আটাত্তর","ঊনআশি","আশি","একাশি","বিরাশি","তিরাশি","চুরাশি","পঁচাশি","ছিয়াশি","সাতাশি","আটাশি","ঊনানব্বুই","নব্বই","একানব্বই","বিরানব্বই","তিরানব্বই","চুরানব্বই","পঁচানব্বই","ছিয়ানব্বুই","সাতানব্বই","আটানব্বই","নিরানব্বই"};
                var tensMap = new[] { "শূন্য", "দশ", "বিশ", "ত্রিশ", "চল্লিশ", "পঞ্চাশ", "ষাট", "সত্তর", "আশি", "নব্বই" };

                if (number < 100)
                    words += unitsMap[number];
                else
                {
                    words += tensMap[number / 10];
                    if ((number % 10) > 0)
                        words += "-" + unitsMap[number % 10];
                }
            }

            return words;
        }

        public static string NumberEtoB(this int number)
        {
            if (number == 0)
                return "";

            if (number < 0)
                return "Minus " + NumberEtoB(Math.Abs(number));

            string words = "";

            if ((number / 10000000) > 0)
            {
                words += NumberEtoB(number / 10000000) + "";
                number %= 10000000;
            }

            if ((number / 100000) > 0)
            {
                words += NumberEtoB(number / 100000) + "";
                number %= 100000;
            }

            if ((number / 1000) > 0)
            {
                words += NumberEtoB(number / 1000) + "";
                number %= 1000;
            }

            if ((number / 100) > 0)
            {
                words += NumberEtoB(number / 100) + "";
                number %= 100;
            }

            if (number > 0)
            {
                if (words != "")
                    words += "";

                var unitsMap = new[] { "০","১","২","৩","৪","৫","৬","৭","৮","৯"};
                var tensMap = new[] { "০","১০","২০","৩০","৪০","৫০","৬০","৭০","৮০","৯০" };

                if (number < 10)
                    words += unitsMap[number];
                else
                {
                    words += unitsMap[number / 10];
                    if ((number % 10) > 0)
                        words += "" + unitsMap[number % 10];
                }
            }

            return words;
        }
    }
    
}