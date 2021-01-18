using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FFT.TimeStamps.Test
{

  [TestClass]
  public class TimeZoneConversionTests
  {
    private static readonly TimeZoneInfo _est = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time"); // new york
    private static readonly TimeZoneInfo _aus = TimeZoneInfo.FindSystemTimeZoneById("AUS Eastern Standard Time"); // sydney

    private static readonly DateTime[] _estTimes;
    private static readonly DateTime[] _utcTimes;
    private static readonly DateTime[] _ausTimes;

    static TimeZoneConversionTests()
    {
      _estTimes = new DateTime[1000000];
      _ausTimes = new DateTime[1000000];
      _utcTimes = new DateTime[1000000];
      var start = new DateTime(2016, 1, 1, 0, 0, 0, DateTimeKind.Utc);
      var end = start.AddYears(4);
      var interval = (end.Ticks - start.Ticks) / 1000000;
      DateTime previousUtc = DateTime.MinValue;
      for (var i = 0; i < 1000000; i++)
      {
        _utcTimes[i] = new DateTime(start.Ticks + i * interval, DateTimeKind.Utc);
        _estTimes[i] = TimeZoneInfo.ConvertTimeFromUtc(_utcTimes[i], _est);
        _ausTimes[i] = TimeZoneInfo.ConvertTimeFromUtc(_utcTimes[i], _aus);
        previousUtc = _utcTimes[i];
      }
    }

    [TestMethod]
    public void BasicConversionTests()
    {
      for (var i = 0; i < _utcTimes.Length; i++)
      {
        var systemOffset = _est.GetUtcOffset(_estTimes[i]);
        var myOffset = new TimeSpan(TimeZoneOffsetCalculator.Get(_est).GetOffsetFromTimeZoneTicks(_estTimes[i].Ticks, out _, out _));
        Assert.AreEqual(systemOffset, myOffset);

        systemOffset = _aus.GetUtcOffset(_ausTimes[i]);
        myOffset = new TimeSpan(TimeZoneOffsetCalculator.Get(_aus).GetOffsetFromTimeZoneTicks(_ausTimes[i].Ticks, out _, out _));
        Assert.AreEqual(systemOffset, myOffset);
      }

      for (var i = 0; i < _utcTimes.Length; i++)
      {
        var t = new TimeStamp(_utcTimes[i].Ticks);
        Assert.AreEqual(_estTimes[i], t.As(_est).DateTime);
        Assert.AreEqual(_ausTimes[i], t.As(_aus).DateTime);
      }

      //var converterEST = new SequentialConverterToDateTimeOffset(_est);
      //var converterAUS = new SequentialConverterToDateTimeOffset(_aus);
      //for (var i = 0; i < _utcTimes.Length; i++)
      //{
      //  var t = new TimeStamp(_utcTimes[i].Ticks);
      //  var est = converterEST.Get(t);
      //  var aus = converterAUS.Get(t);
      //  Assert.AreEqual(est.DateTime, _estTimes[i]);
      //  Assert.AreEqual(aus.DateTime, _ausTimes[i]);
      //}

      var before = new DateTime(2016, 3, 13, 6, 59, 0, DateTimeKind.Utc);
      var at = new DateTime(2016, 3, 13, 7, 0, 0, DateTimeKind.Utc);
      var after = new DateTime(2016, 3, 13, 7, 1, 0, DateTimeKind.Utc);

      var beforeOffset = TimeZoneOffsetCalculator.Get(_est).GetOffsetFromUtcTicks(before.Ticks, out _, out _);
      var atOffset = TimeZoneOffsetCalculator.Get(_est).GetOffsetFromUtcTicks(at.Ticks, out _, out _);
      var afterOffset = TimeZoneOffsetCalculator.Get(_est).GetOffsetFromUtcTicks(after.Ticks, out _, out _);

      Assert.AreNotEqual(beforeOffset, atOffset);
      Assert.AreEqual(atOffset, afterOffset);

      before = new DateTime(2016, 3, 13, 1, 59, 0, DateTimeKind.Unspecified); // est jumps at 2am to 3am
      at = new DateTime(2016, 3, 13, 7, 3, 0, DateTimeKind.Unspecified);
      after = new DateTime(2016, 3, 13, 3, 1, 0, DateTimeKind.Unspecified);

      beforeOffset = TimeZoneOffsetCalculator.Get(_est).GetOffsetFromTimeZoneTicks(before.Ticks, out _, out _);
      atOffset = TimeZoneOffsetCalculator.Get(_est).GetOffsetFromTimeZoneTicks(at.Ticks, out _, out _);
      afterOffset = TimeZoneOffsetCalculator.Get(_est).GetOffsetFromTimeZoneTicks(after.Ticks, out _, out _);

      Assert.AreNotEqual(beforeOffset, atOffset);
      Assert.AreEqual(atOffset, afterOffset);

      var estUtc = ConversionIterators.Create(_est, TimeZoneInfo.Utc);
      var ausUtc = ConversionIterators.Create(_aus, TimeZoneInfo.Utc);
      var utcEst = ConversionIterators.Create(TimeZoneInfo.Utc, _est);
      var utcAus = ConversionIterators.Create(TimeZoneInfo.Utc, _aus);
      var estAus = ConversionIterators.Create(_est, _aus);
      var ausEst = ConversionIterators.Create(_aus, _est);
      for (var i = 0; i < _utcTimes.Length; i++)
      {
        if (i == 63398)
        {
          int j = 0;
        }
        //Assert.AreEqual(_utcTimes[i], estUtc.GetDateTime(_estTimes[i].Ticks));
        Assert.AreEqual(_utcTimes[i], ausUtc.GetDateTime(_ausTimes[i].Ticks));
        //Assert.AreEqual(_estTimes[i], utcEst.GetDateTime(_utcTimes[i].Ticks));
        //Assert.AreEqual(_ausTimes[i], utcAus.GetDateTime(_utcTimes[i].Ticks));
        //Assert.AreEqual(_ausTimes[i], estAus.GetDateTime(_estTimes[i].Ticks));
        //Assert.AreEqual(_estTimes[i], ausEst.GetDateTime(_ausTimes[i].Ticks));
      }

      estUtc = ConversionIterators.Create(_est, TimeZoneInfo.Utc);
      ausUtc = ConversionIterators.Create(_aus, TimeZoneInfo.Utc);
      utcEst = ConversionIterators.Create(TimeZoneInfo.Utc, _est);
      utcAus = ConversionIterators.Create(TimeZoneInfo.Utc, _aus);
      estAus = ConversionIterators.Create(_est, _aus);
      ausEst = ConversionIterators.Create(_aus, _est);
      for (var i = 0; i < _utcTimes.Length; i++)
      {
        Assert.AreEqual(_utcTimes[i], estUtc.GetDateTimeOffset(_estTimes[i].Ticks).DateTime);
        Assert.AreEqual(0, estUtc.GetDateTimeOffset(_estTimes[i].Ticks).Offset.Ticks);

        Assert.AreEqual(_utcTimes[i], ausUtc.GetDateTimeOffset(_ausTimes[i].Ticks).DateTime);
        Assert.AreEqual(0, ausUtc.GetDateTimeOffset(_ausTimes[i].Ticks).Offset.Ticks);

        Assert.AreEqual(_estTimes[i], utcEst.GetDateTimeOffset(_utcTimes[i].Ticks).DateTime);
        Assert.AreEqual(_estTimes[i].Ticks - _utcTimes[i].Ticks, utcEst.GetDateTimeOffset(_utcTimes[i].Ticks).Offset.Ticks);

        Assert.AreEqual(_ausTimes[i], utcAus.GetDateTimeOffset(_utcTimes[i].Ticks).DateTime);
        Assert.AreEqual(_ausTimes[i].Ticks - _utcTimes[i].Ticks, utcAus.GetDateTimeOffset(_utcTimes[i].Ticks).Offset.Ticks);

        Assert.AreEqual(_ausTimes[i], estAus.GetDateTimeOffset(_estTimes[i].Ticks).DateTime);
        Assert.AreEqual(_ausTimes[i].Ticks - _utcTimes[i].Ticks, estAus.GetDateTimeOffset(_estTimes[i].Ticks).Offset.Ticks);

        Assert.AreEqual(_estTimes[i], ausEst.GetDateTimeOffset(_ausTimes[i].Ticks).DateTime);
        Assert.AreEqual(_estTimes[i].Ticks - _utcTimes[i].Ticks, ausEst.GetDateTimeOffset(_ausTimes[i].Ticks).Offset.Ticks);
      }
    }
  }
}
