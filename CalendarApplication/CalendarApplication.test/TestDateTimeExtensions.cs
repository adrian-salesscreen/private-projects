using System;
using NUnit.Framework;
using CalendarApplication.Utils;

namespace CalendarApplication.test
{
    [TestFixture]
    public class TestDateTimeExtensions
    {
        [Test]
        public void ValidateDate_ValidFormat_ReturnsFormattedDate()
        {
            string[] formats =
            {
                "08/08/2020 10:10", "08/8/2020 10:10", "8/8/2020 10:10", "8/08/2020 10:10",
                "08/08/20 10:10", "08/8/20 10:10", "8/8/20 10:10", "8/08/20 10:10"
            };

            const string expectedDate = "08/08/20 10:10";

            foreach (var dateString in formats)
            {
                Assert.AreEqual(expectedDate, DateTimeExtensions.ValidateDate(dateString), $"Date result: {dateString}");
            }
        }

        [Test]
        public void ValidateDateRange_ValidRange_ReturnsFullDateArray()
        {
            string[] dates =
            {
                "08/08/2020 10:10",
                "09/09/2021 10:10"
            };

            DateTime[] expectedDates = 
            {
                new DateTime(2020, 08, 08, 10, 10, 0), 
                new DateTime(2021, 09, 09, 10, 10, 0)
            };

            var outputDates = DateTimeExtensions.ValidateDateRange(dates);

            Array.Sort(expectedDates);
            Array.Sort(outputDates);

            for (var i = 0; i < dates.Length; i++)
            {
                Assert.AreEqual(expectedDates[i], outputDates[i], $"Date result: {outputDates[i]}");
            }
        }
    }
}
