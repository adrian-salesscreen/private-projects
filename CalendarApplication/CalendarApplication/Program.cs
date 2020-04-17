using System;

namespace CalendarApplication
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var userInterface = new CalendarUserInterface();
                userInterface.Start();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}
