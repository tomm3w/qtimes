using System;

namespace iVeew.Core.Helpers
{
    public class DateHelper
    {
        public static string AddOrdinal(int num)
        {
            switch (num % 100)
            {
                case 11:
                case 12:
                case 13:
                    return num.ToString() + "th";
            }

            switch (num % 10)
            {
                case 1:
                    return num.ToString() + "st";
                case 2:
                    return num.ToString() + "nd";
                case 3:
                    return num.ToString() + "rd";
                default:
                    return num.ToString() + "th";
            }

        }
        public static string GetOrdinalDay(DateTime date)
        {
            int dayNo = date.Day;
            return AddOrdinal(dayNo);
        }
    }
}