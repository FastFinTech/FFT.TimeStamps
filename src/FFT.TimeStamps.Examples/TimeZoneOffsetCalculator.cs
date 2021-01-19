using System;
using System.Diagnostics;

namespace FFT.TimeStamps.Examples
{
  internal class TimeZoneOffsetCalculatorExample : IExample
  {
    // New york, USA
    private static readonly TimeZoneInfo _est = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");

    // Sydney, Australia
    private static readonly TimeZoneInfo _aus = TimeZoneInfo.FindSystemTimeZoneById("AUS Eastern Standard Time");

    public void Run()
    {
      DemonstrateConversionOperation();
      DemonstrateLowLevelUtilities();
    }

    private void DemonstrateConversionOperation()
    {
      // get the current time in new york expressed in ticks.
      long ticksNewYorkTimeZone = TimeStamp.Now.AsTicks(_est); // this operation used TimeZoneOffsetCalculator internally.

      // use the calculator to find the equivalent time in sydney expressed in ticks.
      long ticksSydneyTimeZone = TimeZoneOffsetCalculator.Convert(_est, _aus, ticksNewYorkTimeZone);
    }

    private void DemonstrateLowLevelUtilities()
    {
      TimeStamp now = TimeStamp.Now;

      // get current time ticks in UTC timezone
      long nowUtcTicks = now.TicksUtc;
      // get current time ticks in EST timezone
      long nowESTTicks = now.AsTicks(_est); // this operation used TimeZoneOffsetCalculator internally.

      // get a reference to the calculator for the EST timezone.
      TimeZoneOffsetCalculator calculator = TimeZoneOffsetCalculator.Get(_est);

      // the calculator can calculate the current EST timezone offset when
      // supplied with ticks in EST timezone or in UTC timezone.
      long offsetTicks1 = calculator.GetOffsetFromTimeZoneTicks(nowESTTicks, out long segmentStartEstTicks, out long segmentEndEstTicks);
      long offsetTicks2 = calculator.GetOffsetFromUtcTicks(nowUtcTicks, out long segmentStartUtcTicks, out long segmentEndUtcTicks);
      Debug.Assert(offsetTicks1 == offsetTicks2);

      // the method calls above also supplied us with the range of times for which the offset is known to be the same.
      // the range is inclusive of "startTicks" and exclusive of "endTicks"
      Debug.Assert(offsetTicks1 == calculator.GetOffsetFromTimeZoneTicks(segmentStartEstTicks, out _, out _));
      Debug.Assert(offsetTicks1 == calculator.GetOffsetFromTimeZoneTicks(segmentEndEstTicks - 1, out _, out _));

      Debug.Assert(offsetTicks2 == calculator.GetOffsetFromTimeZoneTicks(segmentStartUtcTicks, out _, out _));
      Debug.Assert(offsetTicks2 == calculator.GetOffsetFromTimeZoneTicks(segmentEndUtcTicks - 1, out _, out _));
    }
  }
}
