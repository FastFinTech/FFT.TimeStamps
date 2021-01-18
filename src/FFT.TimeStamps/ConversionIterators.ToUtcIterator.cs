using System;
using System.Runtime.CompilerServices;

namespace FFT.TimeStamps
{
  public partial class ConversionIterators
  {
    private sealed class ToUtcIterator : ITimeZoneConversionIterator, ITimeStampConversionIterator
    {
      private readonly TimeZoneOffsetCalculator _calculator;

      public TimeZoneInfo FromTimeZone { get; }
      public TimeZoneInfo ToTimeZone => TimeZoneInfo.Utc;
      public long DifferenceTicks { get; private set; }

      private long _previousTzTicks;
      private long _tzEndTicks;

      public ToUtcIterator(TimeZoneInfo fromTimeZone)
      {
        FromTimeZone = fromTimeZone;
        _calculator = TimeZoneOffsetCalculator.Get(fromTimeZone);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public bool MoveTo(in long fromTimeZoneTicks)
      {
        if (fromTimeZoneTicks < _previousTzTicks)
        {
          DifferenceTicks = -_calculator.GetOffsetFromTimeZoneTicks(_tzEndTicks + 1, out _, out _tzEndTicks);
          return true;
        }
        else if (fromTimeZoneTicks >= _tzEndTicks)
        {
          DifferenceTicks = -_calculator.GetOffsetFromTimeZoneTicks(fromTimeZoneTicks, out _, out _tzEndTicks);
          return true;
        }
        _previousTzTicks = fromTimeZoneTicks;
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
    }
  }
}
