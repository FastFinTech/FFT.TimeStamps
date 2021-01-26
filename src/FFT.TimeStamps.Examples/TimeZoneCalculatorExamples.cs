// Copyright (c) True Goodwill. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace FFT.TimeStamps.Examples
{
  using System;
  using System.Diagnostics;
  using static FFT.TimeStamps.Examples.TimeZones;

  internal class TimeZoneCalculatorExamples : IExample
  {
    public void Run()
    {
      DemonstrateSimpleConversion();
      DemonstrateSegments();
    }

    /// <summary>
    /// Demonstrates a conversion from one timezone to another timezone using the simplest calculator
    /// feature available. This is also the slowest method available.
    /// </summary>
    private void DemonstrateSimpleConversion()
    {
      // get the current time in new york expressed in ticks.
      long ticksNewYorkTimeZone = TimeStamp.Now.AsTicks(NewYork); // this operation used TimeZoneOffsetCalculator internally.

      // use the calculator to find the equivalent time in sydney expressed in ticks.
      long ticksSydneyTimeZone = TimeZoneCalculator.Convert(NewYork, Sydney, ticksNewYorkTimeZone);
    }

    /// <summary>
    /// Demonstrates use of the "GetSegment" method in the time zone calculator.
    /// </summary>
    /// <remarks>
    /// You can make your code very very fast at timezone conversions by caching and
    /// reusing the segments avoiding calling the "GetSegment" method over and over again.
    /// See the conversion iterators for examples of how this is done.
    /// </remarks>
    private void DemonstrateSegments()
    {
      TimeStamp now = TimeStamp.Now;

      // get current time ticks in UTC timezone
      long utcTicks = now.TicksUtc;

      // get a reference to the calculator for the EST timezone.
      TimeZoneCalculator calculator = TimeZoneCalculator.Get(NewYork);

      // get a segment with StartTicks and EndTicks expressed in UTC timezone.
      TimeZoneCalculator.TimeZoneSegment utcSeg = calculator.GetSegment(utcTicks, TimeKind.Utc);

      // use the segment information to perform a conversion from UTC timezone to EST timezone.
      long estTicks = utcTicks + utcSeg.OffsetTicks;

      // get a segment with StartTicks and EndTicks expressed in EST timezone.
      TimeZoneCalculator.TimeZoneSegment estSeg = calculator.GetSegment(estTicks, TimeKind.TimeZone);

      Debug.Assert(utcSeg.OffsetTicks == estSeg.OffsetTicks, "The offset tick values should be the same.");
    }
  }
}
