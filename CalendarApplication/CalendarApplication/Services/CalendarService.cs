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

        public CalendarService(ICalendarInterface calendarInterface)
        {
            _context = new DatabaseAccessor();
            _entries = _context.InitializeEntries("Constants\\CalendarEntries.json");
            _interface = calendarInterface;
        }

        public void PerformAction(MenuAction action)
        {
            switch (action)
            {
                case MenuAction.AddEntry:
                     AddEntries();
                     break;
                case MenuAction.DeleteEntry:
                    DeleteEntries();
                    break;
                case MenuAction.ListEntries:
                    ListEntriesByRange();
                    break;
                case MenuAction.Exit:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(action), action, "Action is invalid\n");
            }
        }

        private void AddEntries()
        {
            var isFinished = false;
            while (!isFinished)
            {
                Console.WriteLine("Please provide a date for your entry. Valid format is dd/MM/yy HH:mm\n");

                var entryDate = _interface.GetInput();
                if (DateTimeExtensions.ValidateDate(entryDate) == null) continue;
                
                if (Convert.ToDateTime(entryDate) < DateTime.Now)
                {
                    Console.WriteLine("Date must be later than current date \n");
                    continue;
                }

                Console.WriteLine("Description: ");
                var description = _interface.GetInput();

                while (string.IsNullOrEmpty(description))
                { 
                    Console.WriteLine("Please provide a description or press 'd' to exit:\n");
                    description = _interface.GetInput();
                    if (description?.ToLower() == "d") return;
                }

                _entries.Add(new CalendarEntry
                {
                    Date = entryDate,
                    Description = description
                });

                if (_interface.RepeatAction("Would you like to add more entries?\n")) continue;

                _context.SaveChanges(_entries);
                isFinished = true;
            }
        }

        private void DeleteEntries()
        {
            var isFinished = false;
            while (!isFinished)
            {
                Console.WriteLine("If you would like a list of all available entires, press 'L'.\n" +
                                  "Please input the date and time of the desired entry: \n");

                var inputDate = _interface.GetInput();
                if (inputDate.ToLower() == "l")
                {
                    PrintList(_entries);
                    continue;
                }

                var dateOfEntry = DateTimeExtensions.ValidateDate(inputDate);
                
                var entry = _entries.FirstOrDefault(x => x.Date == dateOfEntry);
                if (entry == null)
                {
                    Console.WriteLine("There is no calendar entry with this date or the format was wrong. Please input a valid date or press 'd' to return\n");

                    var input = _interface.GetInput();
                    if (input.ToLower() == "d") return;

                    continue;
                }

                _entries.Remove(entry);
                Console.WriteLine("Entry was found and deleted\n");

                if (_interface.RepeatAction("Would you like to delete more entries?\n")) continue;
                
                _context.SaveChanges(_entries);
                isFinished = true;
            }
        }

        private void ListEntriesByRange()
        {
            if (!_entries.Any())
            {
                Console.WriteLine("Calendar contains no entries\n");
                return;
            }

            var isFinished = false;
            while (!isFinished)
            {
                Console.WriteLine("Please input a valid date range." +
                                  "\n" +
                                  "Valid format is: dd/MM/yy HH:mm - dd/MM/yy HH:mm." +
                                  "Example: 09/05/20 10:15 - 12/05/20 10:15\n");

                var date = _interface.GetInput().Trim();
                if (string.IsNullOrEmpty(date)) continue;

                var dateList = date.Split("-");
                if (dateList.Length > 2)
                {
                    Console.WriteLine("Invalid format. Please try again\n");
                    continue;
                }

                if (DateTimeExtensions.ValidateDate(dateList.FirstOrDefault()) == null) continue;
                var dateFrom = Convert.ToDateTime(dateList.FirstOrDefault());

                if (DateTimeExtensions.ValidateDate(dateList.LastOrDefault()) == null) continue;
                var dateTo = Convert.ToDateTime(dateList.LastOrDefault());

                if (dateFrom > dateTo)
                {
                    Console.WriteLine("The initial date cannot be later than the last date. Please input a valid date range\n");
                    continue;
                }

                var result = _entries.Where(x => 
                    Convert.ToDateTime(x.Date) >= dateFrom && 
                    Convert.ToDateTime(x.Date) < dateTo).ToList();

                PrintList(result);

                Console.WriteLine("Press 'd' to return\n");
                isFinished = _interface.GetInput() == "d";
            }
        }

        private void PrintList(List<CalendarEntry> entries)
        {
            entries.OrderBy(x => x.Date)
                .ToList()
                .ForEach(x =>
                Console.WriteLine($"\n{x.Date}\n" +
                                  $"{x.Description}\n"));
        }
    }
}
