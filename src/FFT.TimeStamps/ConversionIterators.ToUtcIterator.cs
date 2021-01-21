using System;
using System.Runtime.CompilerServices;

namespace FFT.TimeStamps
{
  public partial class ConversionIterators
  {
    private sealed class ToUtcIterator : ITimeZoneConversionIterator, IToTimeStampConversionIterator
    {
      private readonly TimeZoneCalculator _calculator;

      public TimeZoneInfo FromTimeZone { get; }
      public TimeZoneInfo ToTimeZone => TimeZoneInfo.Utc;
      public long DifferenceTicks { get; private set; }

      private long _previousTzTicks;
      private long _tzEndTicks;
      private TimeZoneCalculator.TimeZoneSegment? _segment;

      public ToUtcIterator(TimeZoneInfo fromTimeZone)
      {
        FromTimeZone = fromTimeZone;
        _calculator = TimeZoneCalculator.Get(fromTimeZone);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public bool MoveTo(in long ticks)
      {
        if (ticks >= _tzEndTicks)
        {
          _segment = _calculator.GetSegment(ticks, TimeKind.TimeZone);
          _tzEndTicks = _segment.EndTicks;
          DifferenceTicks = -_segment.OffsetTicks;

          // when first moving into an ambiguous zone (which typically happens as the clock flies back from
          // daylight savings time to standard time), we assume the clock is still in daylight savings for the
          // first movement into the ambigous segment.
          if (_segment.IsAmbiguous)
          {
            // since the ambiguous segments are by definition coded with standard time, we need to
            // search back to the most recent non-ambiguous segment to get the daylight savings offset.
            var previousSegment = _segment.Previous;
            while (previousSegment.IsAmbiguous)
              previousSegment = previousSegment.Previous;
            DifferenceTicks = -previousSegment.OffsetTicks;
          }

          return true;
        }

        // When in an ambiguous zone, we need to catch the moment when the clock
        // flies back as it reverts from daylight savings back to standard time.
        if (_segment!.IsAmbiguous && ticks < _previousTzTicks)
        {
          DifferenceTicks = -_segment.OffsetTicks;
          return true;
        }

        _previousTzTicks = ticks;
        return false;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public long GetTicks(in long fromTimeZoneTicks)
      {
        MoveTo(fromTimeZoneTicks);
        return fromTimeZoneTicks + DifferenceTicks;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public DateTime GetDateTime(in long fromTimeZoneTicks)
      {
        MoveTo(fromTimeZoneTicks);
        return new DateTime(fromTimeZoneTicks + DifferenceTicks, DateTimeKind.Unspecified);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public DateTimeOffset GetDateTimeOffset(in long fromTimeZoneTicks)
      {
        MoveTo(fromTimeZoneTicks);
        return new DateTimeOffset(fromTimeZoneTicks + DifferenceTicks, TimeSpan.Zero);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public TimeStamp GetTimeStamp(in long fromTimeZoneTicks)
      {
        MoveTo(fromTimeZoneTicks);
        return new TimeStamp(fromTimeZoneTicks + DifferenceTicks);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public TimeStamp GetTimeStamp(in DateTime fromTimeZone)
        => GetTimeStamp(fromTimeZone.Ticks);
    }
  }
}
