using System;
using System.Collections.Generic;
using System.Globalization;
using CalendarApplication.Models;
using CalendarApplication.Services;
using NUnit.Framework;
using CalendarApplication.Utils;

namespace CalendarApplication.test
{
    [TestFixture]
    public class TestCalendarService
    {
        [Test]
        public void ValidateDate()
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
    }
}
