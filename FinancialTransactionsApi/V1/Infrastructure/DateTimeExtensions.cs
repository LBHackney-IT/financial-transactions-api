using System;

namespace FinancialTransactionsApi.V1.Infrastructure
{
    public static class DateTimeExtensions
    {
        //public static DateTime StartOfWeek(this DateTime dt, DayOfWeek startOfWeek)
        //{
        //    int diff = dt.DayOfWeek - startOfWeek;
        //    if (diff < 0)
        //    {
        //        diff += 7;
        //    }
        //    return dt.AddDays(-1 * diff).Date;
        //}


        public static DateTime StartOfWeek(this DateTime dt, DayOfWeek startOfWeek)
        {
            int diff = (7 + (dt.DayOfWeek - startOfWeek)) % 7;
            return dt.AddDays(-1 * diff).Date;
        }
    }
}
