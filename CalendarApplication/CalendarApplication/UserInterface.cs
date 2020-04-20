using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using CalendarApplication.Models;
using CalendarApplication.Services;

namespace CalendarApplication
{
    public interface ICalendarInterface
    {
        void Start();
        bool RepeatAction(string question, string input = null);
        string GetInput(string input = null);
        void PrintList(List<CalendarEntry> entries);
    }

    public class CalendarUserInterface : ICalendarInterface
    {
        public void Start()
        {
            var calendarService = new CalendarService(this);
            string input;
            do
            {
                PrintWelcomeMessage();
                
                input = GetInput();
                if (input == null)
                {
                    PrintInvalidInputMessage();
                    continue;
                }

                switch (input.ToLower())
                {
                    case "a":
                        do
                        {
                            calendarService.AddEntry();
                        } while (RepeatAction("Would you like to add more entries?"));
                        break;

                    case "b":
                        do
                        {
                            calendarService.DeleteEntry();
                        } while (RepeatAction("Would you like to delete more entries?"));
                        break;

                    case "c":
                        calendarService.GetEntriesByDateRange();
                        break;
                    case "d":
                        PrintTerminationMessage();
                        break;
                    default: PrintInvalidInputMessage(); continue;
                }

            } while (input?.ToLower() != "d");
        }

        public bool RepeatAction(string question, string input = null)
        {
            Console.WriteLine(question + "y/n \n");

            do {
                input ??= GetInput();
                switch (input)
                {
                    case "y":
                        return true;
                    case "n":
                        return false;
                    default:
                        Console.WriteLine("Please press 'y' or 'n' \n");
                        continue;
                }
            } while (input.ToLower() != "y" || input.ToLower() != "n");

            return false;
        }

        public void PrintList(List<CalendarEntry> entries)
        {
            entries.OrderBy(x => x.Date)
                .ToList()
                .ForEach(x =>
                    Console.WriteLine($"\n{x.Date}\n" +
                                      $"{x.Description}\n"));
        }

        public string GetInput(string input = null)
        {
            return input ?? Console.ReadLine();
        }

        private void PrintWelcomeMessage()
        {
            Console.WriteLine(
                "Welcome to Adrian Jensen's Calendar application. Please choose your desired functionality: " + 
                "\n" + 
                "a) Add calendar entry" + 
                "\n" + 
                "b) Delete calendar entry" + 
                "\n" + 
                "c) List entries for a provided date interval" + 
                "\n" + 
                "d) Exit \n"); 
        }

        private void PrintInvalidInputMessage()
        {
            Console.WriteLine("Please provide a valid input format. Accepted options are: a, b, c, d \n");
        }

        private void PrintTerminationMessage()
        {
            Console.WriteLine("The program has been terminated \n");
        }
    }
}
