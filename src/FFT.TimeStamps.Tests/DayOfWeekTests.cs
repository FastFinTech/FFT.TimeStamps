// Copyright (c) True Goodwill. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace FFT.TimeStamps.Test
{
  using System;
  using Microsoft.VisualStudio.TestTools.UnitTesting;

  [TestClass]
  public class DayOfWeekTests
  {
    [TestMethod]
    public void DayOfWeek()
    {
      var nowUtc = DateTime.UtcNow;
      for (var i = 0; i < 14; i++)
      {
        nowUtc = nowUtc.AddDays(1);
        var nowStamp = new TimeStamp(nowUtc.Ticks);
        Assert.AreEqual(nowUtc.DayOfWeek, nowUtc.Ticks.ToDayOfWeek());
        Assert.AreEqual(nowUtc.DayOfWeek, nowStamp.TicksUtc.ToDayOfWeek());
      }

      nowUtc = DateTime.UtcNow.Date;
      for (var i = 0; i < 14; i++)
      {
        nowUtc = nowUtc.AddDays(1);
        var nowStamp = new TimeStamp(nowUtc.Ticks);
        Assert.AreEqual(nowUtc.DayOfWeek, nowUtc.Ticks.ToDayOfWeek());
        Assert.AreEqual(nowUtc.DayOfWeek, nowStamp.TicksUtc.ToDayOfWeek());
      }
    }
  }
}
