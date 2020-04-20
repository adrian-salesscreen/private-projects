using System;
using System.Globalization;
using System.Linq;

namespace CalendarApplication.Utils
{
    public static class DateTimeExtensions
    {
        public static string ValidateDate(string inputDate)
        {
            string[] formats =
            {
                "dd/MM/yyyy HH:mm", "dd/M/yyyy HH:mm", "d/M/yyyy HH:mm", "d/MM/yyyy HH:mm",
                "dd/MM/yy HH:mm", "dd/M/yy HH:mm", "d/M/yy HH:mm", "d/MM/yy HH:mm"
            };
            var date = DateTime.TryParseExact(inputDate, formats, 
                CultureInfo.InvariantCulture, 
                DateTimeStyles.None, 
                out var dDate) ? $"{dDate:dd/MM/yy HH:mm}" : null;

            if (date == null) Console.WriteLine("\nInvalid date. Valid format is: dd/MM/yy HH:mm");

            return date;
        }

        public static DateTime[] ValidateDateRange(string[] dateList)
        {
            if (dateList.Length > 2)
            {
                Console.WriteLine("\nInvalid format. Please try again\n");
                return null;
            }

            if (ValidateDate(dateList.FirstOrDefault()) == null) return null;
            var dateFrom = Convert.ToDateTime(dateList.FirstOrDefault());

            if (ValidateDate(dateList.LastOrDefault()) == null) return null;
            var dateTo = Convert.ToDateTime(dateList.LastOrDefault());

            return DateIsEarlierThanNow(dateFrom, dateTo) ? null : new[] { dateTo, dateFrom };
        }

        public static bool DateIsEarlierThanNow(DateTime dateFrom, DateTime dateTo)
        {
            if (dateFrom < dateTo) return false;
            Console.WriteLine("\nThe initial date cannot be later than the final date. Please input a valid date range or press 'd' to return\n");

            return true;
        }
    }
}
