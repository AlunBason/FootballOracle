using System.Globalization;
using System.IO;
using System.Text;
using ImageResizer;
using FootballOracle.Foundation;

namespace System
{
    public static class SystemExtensions
    {
        public static DateTime ToEndOfDay(this DateTime value)
        {
            return value.Date.AddDays(1).AddSeconds(-1);
        }

        public static string ToShortGuid(this Guid value)
        {
            var encoded = Convert.ToBase64String(value.ToByteArray());
            encoded = encoded.Replace("/", "_").Replace("+", "-");

            return encoded.Substring(0, 22);
        }

        public static Guid ToGuid(this string value)
        {
            value = value.Replace("_", "/").Replace("-", "+");
            byte[] buffer = Convert.FromBase64String(value + "==");

            return new Guid(buffer);
        }

        public static Guid? ToNullableGuid(this string value)
        {
            return string.IsNullOrWhiteSpace(value) ? null : (Guid?)(value.ToGuid());
        }

        public static string AddOrdinal(this int num)
        {
            if (num <= 0) return num.ToString();

            switch (num % 100)
            {
                case 11:
                case 12:
                case 13:
                    return num + "th";
            }

            switch (num % 10)
            {
                case 1:
                    return num + "st";
                case 2:
                    return num + "nd";
                case 3:
                    return num + "rd";
                default:
                    return num + "th";
            }

        }

        public static string GetTextBetween(this string fullText, string start, string end)
        {
            string returnValue = string.Empty;
            if (fullText.IndexOf(start) != -1 && fullText.IndexOf(end) != -1)
            {
                returnValue = fullText.Substring(fullText.IndexOf(start) + start.Length);
                returnValue = returnValue.Substring(0, returnValue.IndexOf(end));
            }
            return returnValue;
        }

        public static string GetTextBefore(this string fullText, string end)
        {
            return fullText.IndexOf(end) != -1 ? fullText.Substring(0, fullText.IndexOf(end)) : fullText;
        }

        public static string GetTextAfter(this string fullText, string start)
        {
            return fullText.IndexOf(start) != -1 ? fullText.Substring(fullText.IndexOf(start) + start.Length) : fullText;
        }

        public static string RemoveDiacritics(this string value)
        {
            var normalizedString = value.Normalize(NormalizationForm.FormD);
            var stringBuilder = new StringBuilder();

            for (int i = 0; i < normalizedString.Length; i++)
            {
                var c = normalizedString[i];
                if (CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
                    stringBuilder.Append(c);
            }

            return stringBuilder.ToString();
        }

        public static string ToBase64Image(this byte[] bytes, int width, int height)
        {
            var outStream = new MemoryStream();
            var settings = new ResizeSettings(width, height, FitMode.Pad, null);

            ImageResizer.ImageBuilder.Current.Build(bytes, outStream, settings);

            return Convert.ToBase64String(outStream.ToArray());
        }

        public static string ToSpacedString(this string value, bool allInitialLettersUpperCase = false)
        {
            var returnValue = string.Empty;

            for (var i = 0; i < value.Length; i++)
            {
                var c = value[i];
                returnValue = i > 0 && c == char.ToUpper(c) ? returnValue += string.Format(" {0}", allInitialLettersUpperCase ? char.ToUpper(c) : char.ToLower(c)) : returnValue += c;
            }

            return returnValue;
        }

        public static string ToSpacedString(this Enum value, bool allInitialLettersUpperCase = false)
        {
            return value.ToString().ToSpacedString(allInitialLettersUpperCase);
        }

        public static int? ToNullableInt(this string s)
        {
            int i;
            if (int.TryParse(s, out i)) 
                return i;

            return null;
        }

        public static short? ToNullableShort(this string s)
        {
            short i;
            if (short.TryParse(s, out i))
                return i;

            return null;
        }

        public static DateTime AddPeriod(this DateTime value, PeriodType periodType, int periodValue)
        {
            switch (periodType)
            {
                case PeriodType.Day:
                    return value.AddDays(periodValue);

                case PeriodType.Week:
                    return value.AddDays(periodValue * 7);

                case PeriodType.Month:
                    return value.AddMonths(periodValue);

                case PeriodType.Year:
                    return value.AddYears(periodValue);
            }

            return value;
        }

        public static string Truncate(this string value, int maxLength)
        {
            if (string.IsNullOrEmpty(value)) return value;
            return value.Length <= maxLength ? value : value.Substring(0, maxLength);
        }

    }
}
