﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace FFT.TimeStamps.Test {

    [TestClass]
    public class FloorAndCeilingTests {

        [TestMethod]
        public void AbsoluteFloorsAndCeilings() {

            var at = new DateTime(2011, 11, 11, 11, 11, 11, DateTimeKind.Utc).AddMilliseconds(111);
            Assert.AreEqual(at.ToTestString(), "2011-11-11 11:11:11.1110000");

            var x = new TimeStamp(at.Ticks);
            Assert.AreEqual(x.ToTestString(), "2011-11-11 11:11:11.1110000");

            x = x.AddTicks(1234);
            Assert.AreEqual(x.ToTestString(), "2011-11-11 11:11:11.1111234");

            Assert.AreEqual(x.ToMillisecondFloor().ToTestString(), "2011-11-11 11:11:11.1110000");
            Assert.AreEqual(x.ToMillisecondFloor().ToMillisecondFloor().ToTestString(), "2011-11-11 11:11:11.1110000");
            Assert.AreEqual(x.ToMillisecondCeiling().ToTestString(), "2011-11-11 11:11:11.1120000");
            Assert.AreEqual(x.ToMillisecondCeiling().ToMillisecondCeiling().ToTestString(), "2011-11-11 11:11:11.1120000");

            Assert.AreEqual(x.ToSecondFloor().ToTestString(), "2011-11-11 11:11:11.0000000");
            Assert.AreEqual(x.ToSecondFloor().ToSecondFloor().ToTestString(), "2011-11-11 11:11:11.0000000");
            Assert.AreEqual(x.ToSecondCeiling().ToTestString(), "2011-11-11 11:11:12.0000000");
            Assert.AreEqual(x.ToSecondCeiling().ToSecondCeiling().ToTestString(), "2011-11-11 11:11:12.0000000");

            Assert.AreEqual(x.ToMinuteFloor().ToTestString(), "2011-11-11 11:11:00.0000000");
            Assert.AreEqual(x.ToMinuteFloor().ToMinuteFloor().ToTestString(), "2011-11-11 11:11:00.0000000");
            Assert.AreEqual(x.ToMinuteCeiling().ToTestString(), "2011-11-11 11:12:00.0000000");
            Assert.AreEqual(x.ToMinuteCeiling().ToMinuteCeiling().ToTestString(), "2011-11-11 11:12:00.0000000");

            Assert.AreEqual(x.ToHourFloor().ToTestString(), "2011-11-11 11:00:00.0000000");
            Assert.AreEqual(x.ToHourFloor().ToHourFloor().ToTestString(), "2011-11-11 11:00:00.0000000");
            Assert.AreEqual(x.ToHourCeiling().ToTestString(), "2011-11-11 12:00:00.0000000");
            Assert.AreEqual(x.ToHourCeiling().ToHourCeiling().ToTestString(), "2011-11-11 12:00:00.0000000");

            Assert.AreEqual(x.ToDayFloor().ToTestString(), "2011-11-11 00:00:00.0000000");
            Assert.AreEqual(x.ToDayFloor().ToDayFloor().ToTestString(), "2011-11-11 00:00:00.0000000");
            Assert.AreEqual(x.ToDayCeiling().ToTestString(), "2011-11-12 00:00:00.0000000");
            Assert.AreEqual(x.ToDayCeiling().ToDayCeiling().ToTestString(), "2011-11-12 00:00:00.0000000");

            Assert.AreEqual(x.ToWeekFloor().ToTestString(), "2011-11-06 00:00:00.0000000");
            Assert.AreEqual(x.ToWeekFloor().ToWeekFloor().ToTestString(), "2011-11-06 00:00:00.0000000");
            Assert.AreEqual(x.ToWeekCeiling().ToTestString(), "2011-11-13 00:00:00.0000000");
            Assert.AreEqual(x.ToWeekCeiling().ToWeekCeiling().ToTestString(), "2011-11-13 00:00:00.0000000");
        }

        [TestMethod]
        public void ClockFloorsAndCeilings() {
            var est = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");

            var start = new TimeStamp(new DateTime(2019, 3, 10, 2, 0, 0, DateTimeKind.Utc).Ticks, est); // At 2am, the clock flies forward to 3am
            Assert.AreEqual(start.As(est).ToTestString(), "2019-03-10 03:00:00.0000000");

            var end = new TimeStamp(new DateTime(2019, 11, 3, 2, 0, 0, DateTimeKind.Utc).Ticks, est); // At 2am, the clock flies backward to 1am.
            Assert.AreEqual(end.As(est).ToTestString(), "2019-11-03 02:00:00.0000000");

            Assert.AreEqual(start.ToDayFloor(est).As(est).ToTestString(), "2019-03-10 00:00:00.0000000");
            Assert.AreEqual(start.ToDayFloor(est).ToDayFloor(est).As(est).ToTestString(), "2019-03-10 00:00:00.0000000");
            Assert.AreEqual(start.ToDayCeiling(est).As(est).ToTestString(), "2019-03-11 00:00:00.0000000");
            Assert.AreEqual(start.ToDayCeiling(est).ToDayCeiling(est).As(est).ToTestString(), "2019-03-11 00:00:00.0000000");


            Assert.AreEqual(start.AddMinutes(-1).As(est).ToTestString(), "2019-03-10 01:59:00.0000000");


            var startEST = end.As(est);
        }
    }

  internal static class XYZ {
        public static string ToTestString(this DateTimeOffset target) => target.ToString("yyyy-MM-dd HH:mm:ss.fffffff");
        public static string ToTestString(this DateTime target) => target.ToString("yyyy-MM-dd HH:mm:ss.fffffff");
        public static string ToTestString(this TimeStamp target) => target.AsUtc().ToString("yyyy-MM-dd HH:mm:ss.fffffff");

    }
}
