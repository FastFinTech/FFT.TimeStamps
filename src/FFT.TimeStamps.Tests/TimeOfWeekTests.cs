using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace FFT.TimeStamps.Test {

    [TestClass]
    public class TimeOfWeekTests {

        [TestMethod]
        public void GetAbsolutePrevious_And_GetAbsoluteNext_TimeOfWeek() {

            var format = "yyyy-MM-dd HH:mm:ss.fffffff";

            /// Just after 5am on a Friday
            var start = DateTime.ParseExact("2019-12-20 05:37:23.4849404", format, CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal);

            var startStamp = new TimeStamp(start.Ticks);
            var fridayMidnight = new TimeOfWeek(DayOfWeek.Friday, TimeSpan.Zero);
            var saturdayMidnight = new TimeOfWeek(DayOfWeek.Saturday, TimeSpan.Zero);

            Assert.AreEqual(startStamp.GetPrevious(fridayMidnight).AsUtc().ToString(format), "2019-12-20 00:00:00.0000000");
            Assert.AreEqual(startStamp.GetPrevious(saturdayMidnight).AsUtc().ToString(format), "2019-12-14 00:00:00.0000000");

            Assert.AreEqual(startStamp.GetNext(saturdayMidnight).AsUtc().ToString(format), "2019-12-21 00:00:00.0000000");
            Assert.AreEqual(startStamp.GetNext(fridayMidnight).AsUtc().ToString(format), "2019-12-27 00:00:00.0000000");
        }

        [TestMethod]
        public void Serialization_Standard() {
            var t1 = TimeOfWeek.CreateFrom(DateTime.Now);
            var json = JsonConvert.SerializeObject(t1);
            var t2 = JsonConvert.DeserializeObject<TimeOfWeek>(json);
            Assert.AreEqual(t1, t2);
        }

        [TestMethod]
        public void Serialization_Nullables() {
            var t1 = (TimeOfWeek?)null;
            var json = JsonConvert.SerializeObject(t1);
            var t2 = JsonConvert.DeserializeObject<TimeOfWeek?>(json);
            Assert.AreEqual(t1, t2);

            t1 = TimeOfWeek.CreateFrom(DateTime.Now);
            json = JsonConvert.SerializeObject(t1);
            t2 = JsonConvert.DeserializeObject<TimeOfWeek?>(json);
            Assert.AreEqual(t1, t2);
        }
    }
}
