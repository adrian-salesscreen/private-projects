using System;
using System.Collections.Generic;
using System.Linq;
using CalendarApplication.DAL;
using CalendarApplication.Models;
using CalendarApplication.Utils;

namespace CalendarApplication.Services
{
    public class CalendarService
    {
        private readonly List<CalendarEntry> _entries;
        private readonly ICalendarInterface _interface;
        private readonly IDatabaseAccessor _context;


        public CalendarService(ICalendarInterface calendarInterface, 
            string filePath = "Constants\\CalendarEntries.json")
        {
            _context = new DatabaseAccessor(filePath);
            _entries = _context.FetchEntries();
            _interface = calendarInterface;
        }

        public void AddEntry(string entryDate = null,
            string description = null, string defaultInput = null)
        {
            var isFinished = false;
            while (!isFinished)
            {
                Console.WriteLine("\nPlease provide a date for your entry. Valid format is dd/MM/yy HH:mm\n");

                var inputDate = entryDate ?? _interface.GetInput();
                var formattedDate = DateTimeExtensions.ValidateDate(inputDate);

                if (formattedDate == null || EntryExists(formattedDate))
                {
                    if (entryDate == null) continue;
                    break;
                }

                if (Convert.ToDateTime(formattedDate) < DateTime.Now)
                {
                    Console.WriteLine("\nDate must be later than current date\n");

                    if (entryDate == null) continue;
                    break;
                }

                Console.WriteLine("Description:\n");
                description ??= _interface.GetInput();

                while (string.IsNullOrEmpty(description))
                { 
                    Console.WriteLine("\nPlease provide a description or press 'd' to exit:\n");

                    var input = defaultInput ?? _interface.GetInput();
                    if (input.ToLower() == "d") return;
                    if (defaultInput != null) break;
                }

                _entries.Add(new CalendarEntry
                {
                    Date = formattedDate,
                    Description = description
                });

                _context.SaveChanges(_entries);
                isFinished = true;
            }
        }

        public void DeleteEntry(string entryDate = null, string defaultInput = null)
        {
            var isFinished = false;
            while (!isFinished)
            {
                Console.WriteLine("\nIf you would like a list of all available entires, press 'L'.\n" +
                                  "Please input the date and time of the desired entry:\n");

                var inputDate = entryDate ?? _interface.GetInput();
                if (inputDate.ToLower() == "l")
                {
                    _interface.PrintList(_entries);
                    continue;
                }

                var dateOfEntry = DateTimeExtensions.ValidateDate(inputDate);
                
                var entry = _entries.FirstOrDefault(x => x.Date == dateOfEntry);
                if (entry == null)
                {
                    Console.WriteLine("\nThere is no calendar entry with this date or the format was wrong. Please input a valid date or press 'd' to return\n");

                    var input = defaultInput ?? _interface.GetInput();
                    if (input.ToLower() == "d") return;
                    continue;
                }

                _entries.Remove(entry);
                Console.WriteLine("\nEntry was found and deleted\n");

                _context.SaveChanges(_entries);
                isFinished = true;
            }
        }

        public void GetEntriesByDateRange()
        {
            if (!_entries.Any())
            {
                Console.WriteLine("\nCalendar contains no entries\n");
                return;
            }

            var result = new List<CalendarEntry>();
            var isFinished = false;
            while (!isFinished)
            {
                Console.WriteLine("\nPlease input a valid date range." +
                                  "\n" +
                                  "Valid format is: dd/MM/yy HH:mm - dd/MM/yy HH:mm." +
                                  "Example: 09/05/20 10:15-12/05/20 10:15\n");

                var input = _interface.GetInput().Trim();
                if (string.IsNullOrEmpty(input)) continue;

                var dateList = input.Split("-");

                var dateRange = DateTimeExtensions.ValidateDateRange(dateList);
                if (dateRange == null && _interface.GetInput() == "d") break;
                if (dateRange == null) continue;
                
                Array.Sort(dateRange);
                _interface.PrintList(_entries.Where(x =>
                    Convert.ToDateTime(x.Date) >= dateRange[0] &&
                    Convert.ToDateTime(x.Date) < dateRange[1]).ToList()); 

                Console.WriteLine("\nPress 'd' to return\n");
                isFinished = _interface.GetInput() == "d";
            }
        }

        private bool EntryExists(string formattedDate)
        {
            if (!_entries.Select(x => x.Date).Contains(formattedDate)) return false;
            Console.WriteLine("A calendar entry with this date already exists");

            return true;
        }

        
    }
}
