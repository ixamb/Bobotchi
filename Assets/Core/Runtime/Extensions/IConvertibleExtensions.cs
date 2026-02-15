using System;
using System.Globalization;

namespace Core.Runtime.Extensions
{
    public static class IConvertibleExtensions
    {
        public static string ToAbbreviatedString<T>(this T number, int decimalPlaces = 1) where T : IConvertible
        {
            string[] suffixes = { "", "K", "M", "B", "T", "Qa", "Qn" };
            
            double rawNumber;
            try
            {
                rawNumber = Convert.ToDouble(number, CultureInfo.InvariantCulture);
            }
            catch (Exception ex) when (ex is InvalidCastException or FormatException)
            {
                return number.ToString();
            }
            
            if (rawNumber == 0)
                return "0";

            var isNegative = rawNumber < 0;
            var absNumber = Math.Abs(rawNumber); 
            var magnitude = 0;
            var divided = absNumber;
            
            while (divided >= 1000 && magnitude < suffixes.Length - 1) 
            {
                divided /= 1000;
                magnitude++;
            }

            var decimalFormat = $"F{decimalPlaces}";
            var formattedValue = divided.ToString(decimalFormat, CultureInfo.InvariantCulture);
            var sign = isNegative ? "-" : string.Empty;
            return $"{sign}{formattedValue}{suffixes[magnitude]}";
        }
    }
}