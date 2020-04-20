using System.Collections.Generic;
using System.IO;
using CalendarApplication.DAL;
using CalendarApplication.Models;
using NUnit.Framework;

namespace CalendarApplication.test
{
    [TestFixture]
    class TestDatabaseAccessor
    {
        [Test]
        public void InitializeEntries_InvalidFile_ReturnsEmptyList()
        {
            const string pathToTestFile = "NullOrEmptyFile.json";

            var databaseAccessor = new DatabaseAccessor(pathToTestFile);

            Assert.AreEqual(new List<CalendarEntry>(), databaseAccessor.FetchEntries());
        }

        [Test]
        public void SaveChanges_FileNotExists_CreatesNewFile()
        {
            const string pathToTestFile = "TestCalendarEntries.json"; 

            File.Delete(pathToTestFile);

            var databaseAccessor = new DatabaseAccessor(pathToTestFile);

            databaseAccessor.SaveChanges(new List<CalendarEntry>
            {
                new CalendarEntry
                {
                    Date = "09/09/20 15:15",
                    Description = "Test"
                }
            });

            var fileExists = File.Exists(pathToTestFile);

            Assert.AreEqual(true, fileExists);
        }
    }
}