// Copyright (c) True Goodwill. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace FFT.TimeStamps.Test
{
  using System;
  using Microsoft.VisualStudio.TestTools.UnitTesting;

  [TestClass]
  public class TimeOfDayTests
  {
    [TestMethod]
    public void TimeOfDay()
    {
      var nowUtc = DateTime.UtcNow;
      var nowStamp = new TimeStamp(nowUtc.Ticks);
      Assert.AreEqual(nowUtc.TimeOfDay, nowUtc.Ticks.ToTimeOfDay());
      Assert.AreEqual(nowUtc.TimeOfDay, nowStamp.ToTimeOfDay());

      nowUtc = DateTime.UtcNow.Date;
      nowStamp = new TimeStamp(nowUtc.Ticks);
      Assert.AreEqual(nowUtc.TimeOfDay, nowUtc.Ticks.ToTimeOfDay());
      Assert.AreEqual(nowUtc.TimeOfDay, nowStamp.ToTimeOfDay());
    }
  }
}
