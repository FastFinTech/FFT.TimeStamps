using System;

namespace FFT.TimeStamps.Examples
{
  internal class ConversionIterators : IExample
  {
    // New york, USA
    private static readonly TimeZoneInfo _est = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");

    // Sydney, Australia
    private static readonly TimeZoneInfo _aus = TimeZoneInfo.FindSystemTimeZoneById("AUS Eastern Standard Time");

    public void Run()
    {
      UseIToTimeStampConversionIterator();
      UseIFromTimeStampConversionIterator();
      UseITimeZoneConversionIterator();
      DemonstrateLowLevelFeatures();
    }

    // demonstrates conversion of DateTime to TimeStamps
    private void UseIToTimeStampConversionIterator()
    {
      IToTimeStampConversionIterator converter = TimeStamps.ConversionIterators.ToTimeStamp(_est);
      foreach (DateTime estTime in ExampleFeed.ChronologicalDateTimes())
      {
        TimeStamp timeStamp = converter.GetTimeStamp(estTime.Ticks);
      }
    }

    // demonstrates conversion of TimeStamp to DateTime
    private void UseIFromTimeStampConversionIterator()
    {
      IFromTimeStampConversionIterator converter = TimeStamps.ConversionIterators.FromTimeStamp(_est);
      foreach (TimeStamp timestamp in ExampleFeed.ChronologicalTimeStamps())
      {
        DateTime estTime = converter.GetDateTime(timestamp);
        DateTimeOffset estTime2 = converter.GetDateTimeOffset(timestamp);
      }
    }

    // demonstrates conversion of DateTime or DateTimeOffset from one timezone to another.
    private void UseITimeZoneConversionIterator()
    {
      ITimeZoneConversionIterator converter = TimeStamps.ConversionIterators.Create(_est, _aus);
      foreach (DateTime newYorkTime in ExampleFeed.ChronologicalDateTimes())
      {
        DateTime sydneyTime = converter.GetDateTime(newYorkTime.Ticks);
        DateTimeOffset sydneyTime2 = converter.GetDateTimeOffset(newYorkTime.Ticks);
      }
    }

    // demonstrates use of the "MoveTo" method and the "DifferenceTicks" property
    // of the conversion iterators for low-level (and faster) operation.
    private void DemonstrateLowLevelFeatures()
    {
      ITimeZoneConversionIterator converter = TimeStamps.ConversionIterators.Create(_est, _aus);
      foreach (DateTime newYorkTime in ExampleFeed.ChronologicalDateTimes())
      {
        var differenceChanged = converter.MoveTo(newYorkTime.Ticks);
        if (differenceChanged)
        {
          var newDifferenceInHours = (int)converter.DifferenceTicks.ToHours();
          Console.WriteLine($"Difference between New York and Sydney time changed to {newDifferenceInHours} hours at {newYorkTime:yyyy-MM-dd HH:mm:ss}, New York time.");
        }
      }
    }
  }
}
