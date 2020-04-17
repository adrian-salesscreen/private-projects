using System;
using System.Collections.Generic;
using System.IO;
using CalendarApplication.Models;
using Newtonsoft.Json;

namespace CalendarApplication.DAL
{
    public interface IDatabaseAccessor
    {
        List<CalendarEntry> InitializeEntries(string path);
        void SaveChanges(List<CalendarEntry> entryList);
    }

    public class DatabaseAccessor : IDatabaseAccessor
    {
        public List<CalendarEntry> InitializeEntries(string path)
        {
            var placeHolder = new List<CalendarEntry>();
            if (!File.Exists($"Constants\\CalendarEntries.json")) return placeHolder;

            var json = File.ReadAllText("Constants\\calendarEntries.json");
            if (string.IsNullOrEmpty(json)) return placeHolder;

            return JsonConvert.DeserializeObject<List<CalendarEntry>>(json) ?? placeHolder;
        }

        public void SaveChanges(List<CalendarEntry> entryList)
        {
            try
            {
                var json = JsonConvert.SerializeObject(entryList, Formatting.Indented);
                File.WriteAllText($"Constants\\CalendarEntries.json", json);

                Console.WriteLine("Success!");
            }
            catch (DirectoryNotFoundException dirNotFoundException)
            {
                Console.WriteLine(dirNotFoundException);
                throw;
            }
        }
    }
}
