// Copyright (c) True Goodwill. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace FFT.TimeStamps.Test
{
  using System;
  using Microsoft.VisualStudio.TestTools.UnitTesting;

  [TestClass]
  public class FloorAndCeilingTests
  {
    [TestMethod]
    public void UtcFloorsAndCeilings()
    {
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
    public void ClockFloorsAndCeilings()
    {
      // TODO:
    }
  }

  internal static class XYZ
  {
    public static string ToTestString(this DateTimeOffset target) => target.ToString("yyyy-MM-dd HH:mm:ss.fffffff");

    public static string ToTestString(this DateTime target) => target.ToString("yyyy-MM-dd HH:mm:ss.fffffff");

    public static string ToTestString(this TimeStamp target) => target.ToString("yyyy-MM-dd HH:mm:ss.fffffff");

  }
}
