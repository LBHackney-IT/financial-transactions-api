using System;

namespace FinancialTransactionsApi.V1.UseCase.Helpers
{
    public static class DateTimeHelper
    {
        public static DateTime? CheckAndConvertDateTime(this string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return null;
            }
            else
            {
                DateTime result;
                if (!DateTime.TryParse(input, out result))
                {
                    return null;
                }
                return result;
            }
        }

        public static DateTime StartOfWeek(this DateTime dt, DayOfWeek startOfWeek)
        {
            int diff = (7 + (dt.DayOfWeek - startOfWeek)) % 7;
            return dt.AddDays(-1 * diff).Date;
        }
        public static DateTime GetWeekStartDate(short year, short month, short weekNumber)
        {           
            var firstWeekDay = new DateTime(year, month, 1);
                
            return firstWeekDay.StartOfWeek(DayOfWeek.Monday).AddDays((weekNumber - 1) * 7);
        }
    }
}
