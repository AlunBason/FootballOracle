namespace System
{
    public static class Date
    {
        public static readonly DateTime LowDate = new DateTime(1850, 1, 1);
        public static readonly DateTime HighDate = new DateTime(2999, 12, 31, 23, 59, 59);

        public static bool _IsBetween(this DateTime test, DateTime startDate, DateTime endDate)
        {
            return test >= startDate && test <= endDate;
        }

        public static string ToUrlString(this DateTime value)
        {
            return value.ToString("ddMMMyyyy");
        }

        public static string ToDisplayString(this DateTime value, bool isDateDisplayed = true, bool isTimeDisplayed = false)
        {
            if (value == Date.LowDate)
                return "Low date";

            if (value == Date.HighDate)
                return "High date";

            var returnValue = string.Empty;

            if (isDateDisplayed)
                returnValue = string.Format("{0} {1} ", value.Day.AddOrdinal(), value.ToString("MMMM yyyy"));

            if (isTimeDisplayed)
            {
                if (value.Minute == 0)
                    return string.IsNullOrWhiteSpace(returnValue) ? value.ToString("htt").ToLower() : returnValue + value.ToString(" : htt").ToLower();
                else
                    return string.IsNullOrWhiteSpace(returnValue) ? value.ToString("h:mmtt").ToLower() : returnValue + value.ToString(" : h:mmtt").ToLower();
            }

            return returnValue.Trim(); ;
        }

        public static string ToShortString(this DateTime value)
        {
            if (value == Date.LowDate)
                return "Low date";

            if (value == Date.HighDate)
                return "High date";

            return value.ToShortDateString();
        }

        public static int ToAge(this DateTime value, DateTime atDate)
        {
            int age = atDate.Year - value.Year;
            if (value > atDate.AddYears(-age)) age--;

            return age;
        }
    }
}
