using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CalendarApplication.DAL;
using CalendarApplication.Models;
using CalendarApplication.Services;
using NUnit.Framework;

namespace CalendarApplication.test
{
    [TestFixture]
    internal class TestCalendarService
    {
        private const string PathToTestFile = "CalendarEntriesTest.json";
        private readonly CalendarService _calendar = new CalendarService(new CalendarUserInterface(), PathToTestFile);
        private readonly DatabaseAccessor _databaseAccessor = new DatabaseAccessor(PathToTestFile);

        public TestCalendarService()
        {
            File.Delete(PathToTestFile);
        }

        [Test]
        public void AddEntry_DateIsValid_AddsEntry()
        {
            var entriesBefore = _databaseAccessor.FetchEntries().Count;

            _calendar.AddEntry("10/10/20 15:15", "Test description", "d");

            var entriesAfter = _databaseAccessor.FetchEntries().Count;
            Assert.Greater(entriesAfter, entriesBefore);
        }

        [Test]
        public void AddEntry_DateIsEarlierThanNow_DoesNotAddEntry()
        {
            var entriesBefore = _databaseAccessor.FetchEntries().Count;

            _calendar.AddEntry(DateTime.Now.AddMinutes(-1).ToString("g"), "Test description", "d");

            var entriesAfter = _databaseAccessor.FetchEntries().Count;
            Assert.AreEqual(entriesAfter, entriesBefore);
        }

        [Test]
        public void DeleteEntry_DateIsValid_DeletesEntry()
        {
            var entriesBefore = _databaseAccessor.FetchEntries();

            if (!entriesBefore.Any()) _databaseAccessor.SaveChanges(new List<CalendarEntry>
            {
                new CalendarEntry
                {
                    Date = "10/10/20 10:10",
                    Description = "Test",
                }
            });

            _calendar.DeleteEntry(entriesBefore.FirstOrDefault()?.Date, "d");
            var entriesAfter = _databaseAccessor.FetchEntries().Count;

            Assert.Less(entriesAfter, entriesBefore.Count);
        }

        [Test]
        public void DeleteEntry_EntryNotExistForDate_DoesNotDeleteEntry()
        {
            _databaseAccessor.SaveChanges(new List<CalendarEntry>
            {
                new CalendarEntry
                {
                    Date = "10/10/20 10:10",
                    Description = "Test"
                }
            });

            var entriesBefore = _databaseAccessor.FetchEntries();

            const string invalidDate = "10/10/20 09:09";

            _calendar.DeleteEntry(invalidDate, "d");
            var entriesAfter = _databaseAccessor.FetchEntries().Count;

            Assert.AreEqual(entriesAfter, entriesBefore.Count);
        }
    }
}
