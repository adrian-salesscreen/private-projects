using System;
using System.Globalization;

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

            if (date == null) Console.WriteLine("Invalid date. Valid format is: dd/MM/yy HH:mm \n");

            return date;
        }
    }
}
