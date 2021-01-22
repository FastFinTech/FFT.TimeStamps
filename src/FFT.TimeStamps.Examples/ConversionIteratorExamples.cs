// Copyright (c) True Goodwill. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace FFT.TimeStamps.Examples
{
  using System;
  using static FFT.TimeStamps.Examples.TimeZones;

  internal class ConversionIteratorExamples : IExample
  {
    public void Run()
    {
      TimeZone_TimeStamp();
      TimeStamp_TimeZone();
      TimeZone_TimeZone();
      DemonstrateLowLevelFeatures();
    }

    // Demonstrates conversion of various time objects in the New York timezone to TimeStamps
    private void TimeZone_TimeStamp()
    {
      IToTimeStampConversionIterator converter = ConversionIterators.ToTimeStamp(fromTimeZone: NewYork);
      foreach (DateTime estTime in ExampleFeed.ChronologicalUnspecifiedDateTimes())
      {
        TimeStamp timeStamp = converter.GetTimeStamp(estTime.Ticks);
      }
    }

    // Demonstrates conversion of TimeStamp to various time objects in the New York timezone.
    private void TimeStamp_TimeZone()
    {
      IFromTimeStampConversionIterator converter = ConversionIterators.FromTimeStamp(toTimeZone: NewYork);
      foreach (TimeStamp timestamp in ExampleFeed.ChronologicalTimeStamps())
      {
        DateTime estTime = converter.GetDateTime(timestamp);
        DateTimeOffset estTime2 = converter.GetDateTimeOffset(timestamp);
        long estTicks = converter.GetTicks(timestamp);
      }
    }

    // Demonstrates conversion of time objects from one timezone to another.
    private void TimeZone_TimeZone()
    {
      ITimeZoneConversionIterator converter = ConversionIterators.Create(NewYork, Sydney);
      foreach (DateTime newYorkTime in ExampleFeed.ChronologicalUnspecifiedDateTimes())
      {
        DateTime sydneyTime = converter.GetDateTime(newYorkTime.Ticks);
        DateTimeOffset sydneyTime2 = converter.GetDateTimeOffset(newYorkTime.Ticks);
        long sydneyTicks = converter.GetTicks(newYorkTime.Ticks);
      }
    }

    // Demonstrates use of the "MoveTo" method and the "DifferenceTicks" property
    // of the conversion iterators for low-level (and faster) operation.
    private void DemonstrateLowLevelFeatures()
    {
      ITimeZoneConversionIterator converter = ConversionIterators.Create(NewYork, Sydney);
      foreach (DateTime newYorkTime in ExampleFeed.ChronologicalUnspecifiedDateTimes())
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
