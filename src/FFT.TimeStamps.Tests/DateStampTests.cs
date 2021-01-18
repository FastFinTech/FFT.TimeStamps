using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace FFT.TimeStamps.Test {

    [TestClass]
    public class DateStampTests {

        [TestMethod]
        public void DateStamp() {

            var x = TimeStamps.DateStamp.MaxValue;
            var y = TimeStamps.DateStamp.MinValue;


            var now = TimeStamp.Now;
            var nowDateString = now.GetDate().ToString();
            var expectedString = new DateTime(now.TicksUtc, DateTimeKind.Utc).Date.ToString("yyyy-MM-dd");
            Assert.AreEqual(nowDateString, expectedString);
        }

        [TestMethod]
        public void DateStampDaysSinceDayOfWeek() {
            var date = new DateStamp(2019, 11, 11); // Monday;
            Assert.AreEqual(date.GetDaysSince(DayOfWeek.Monday), 0);
            Assert.AreEqual(date.GetDaysSince(DayOfWeek.Sunday), 1);
            Assert.AreEqual(date.GetDaysSince(DayOfWeek.Saturday), 2);
            Assert.AreEqual(date.GetDaysSince(DayOfWeek.Friday), 3);
        }

        [TestMethod]
        public void DateStampDaysUntilDayOfWeek() {
            var date = new DateStamp(2019, 11, 12); // Tuesday;
            Assert.AreEqual(date.GetDaysUntil(DayOfWeek.Monday), 6);
            Assert.AreEqual(date.GetDaysUntil(DayOfWeek.Sunday), 5);
            Assert.AreEqual(date.GetDaysUntil(DayOfWeek.Saturday), 4);
            Assert.AreEqual(date.GetDaysUntil(DayOfWeek.Friday), 3);
            Assert.AreEqual(date.GetDaysUntil(DayOfWeek.Tuesday), 0);
        }
    }
}
