using System;
using CalendarApplication.Models;
using CalendarApplication.Services;

namespace CalendarApplication
{
    public interface ICalendarInterface
    {
        void Start();
        bool RepeatAction(string question);
        string GetInput();
    }

    public class CalendarUserInterface : ICalendarInterface
    {
        private CalendarService _calendar;
        private MenuAction _action;

        public CalendarUserInterface()
        {
            _calendar = new CalendarService(this);
        }

        public void Start()
        {
            do
            {
                PrintWelcomeMessage();
                
                var input = GetInput();
                if (input == null)
                {
                    PrintInvalidInputMessage();
                    continue;
                }

                switch (input.ToLower())
                {
                    case "a":
                        _action = MenuAction.AddEntry;
                        break;
                    case "b":
                        _action = MenuAction.DeleteEntry;
                        break;
                    case "c":
                        _action = MenuAction.ListEntries;
                        break;
                    case "d":
                        _action = MenuAction.Exit;
                        break;
                    default: PrintInvalidInputMessage(); continue;
                }

                if (_action == MenuAction.Exit)
                {
                    PrintTerminationMessage();
                    break;
                }

                _calendar.PerformAction(_action);

            } while (_action != MenuAction.Exit);
        }

        public bool RepeatAction(string question)
        {
            Console.WriteLine(question + "y/n \n");

            string input;
            do {
                input = GetInput();
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

        public string GetInput()
        {
            return Console.ReadLine();
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
