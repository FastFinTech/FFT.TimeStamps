// Copyright (c) True Goodwill. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace FFT.TimeStamps.Test
{
  using System;
  using Microsoft.VisualStudio.TestTools.UnitTesting;

  [TestClass]
  public class MonthStampTests
  {
    [TestMethod]
    public void MonthStamp_General()
    {
      var x = MonthStamp.MinValue;
      var y = MonthStamp.MaxValue;

      var now = TimeStamp.Now;
      var nowMonthString = now.GetMonth().ToString();
      var expectedString = new DateTime(now.TicksUtc, DateTimeKind.Utc).Date.ToString("yyyy-MM");
      Assert.AreEqual(nowMonthString, expectedString);
    }

    [TestMethod]
    public void MonthStamp_GetMonthsSince()
    {
      var a = new MonthStamp(2000, 1);
      var b = new MonthStamp(2001, 1);
      var c = new MonthStamp(1999, 1);
      Assert.AreEqual(a.GetMonthsSince(b), -12);
      Assert.AreEqual(b.GetMonthsSince(a), 12);
      Assert.AreEqual(a.GetMonthsSince(c), 12);

      var d = new MonthStamp(2000, 6);
      Assert.AreEqual(d.GetMonthsSince(a), 5);
      Assert.AreEqual(a.GetMonthsSince(d), -5);

      var x = new MonthStamp(2000, 6);
      for (var i = 0; i < 240; i++)
      {
        var y = x.AddMonths(i);
        var z = x.AddMonths(-i);
        Assert.AreEqual(x.GetMonthsSince(y), -i);
        Assert.AreEqual(x.GetMonthsSince(z), i);
        Assert.AreEqual(y.GetMonthsSince(x), i);
        Assert.AreEqual(z.GetMonthsSince(x), -i);
      }
    }
  }
}
