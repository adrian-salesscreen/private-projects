using System;
using System.Collections.Generic;
using System.IO;
using CalendarApplication.Models;
using Newtonsoft.Json;

namespace CalendarApplication.DAL
{
    public interface IDatabaseAccessor
    {
        List<CalendarEntry> FetchEntries();
        void SaveChanges(List<CalendarEntry> entryList);
    }

    public class DatabaseAccessor : IDatabaseAccessor
    {
        private readonly string _path;

        public DatabaseAccessor(string filePath)
        {
            _path = filePath;
        }

        public List<CalendarEntry> FetchEntries()
        {
            var placeHolder = new List<CalendarEntry>();
            if (!File.Exists(_path)) return placeHolder;

            var json = File.ReadAllText(_path);
            if (string.IsNullOrEmpty(json)) return placeHolder;

            return JsonConvert.DeserializeObject<List<CalendarEntry>>(json) ?? placeHolder;
        }

        public void SaveChanges(List<CalendarEntry> entryList)
        {
            try
            {
                var json = JsonConvert.SerializeObject(entryList, Formatting.Indented);
                File.WriteAllText(_path, json);

                Console.WriteLine("Success!\n");
            }
            catch (DirectoryNotFoundException dirNotFoundException)
            {
                Console.WriteLine(dirNotFoundException);
                throw;
            }
        }
    }
}
