using System;
using System.Runtime.CompilerServices;

namespace FFT.TimeStamps
{
  public partial class ConversionIterators
  {
    private sealed class FromUtcIterator : ITimeZoneConversionIterator
    {
      private readonly TimeZoneOffsetCalculator _calculator;

      public TimeZoneInfo FromTimeZone => TimeZoneInfo.Utc;
      public TimeZoneInfo ToTimeZone { get; }
      public long DifferenceTicks { get; private set; }

      private long _utcEndTicks;

      public FromUtcIterator(TimeZoneInfo toTimeZone)
      {
        ToTimeZone = toTimeZone;
        _calculator = TimeZoneOffsetCalculator.Get(toTimeZone);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public bool MoveTo(in long utcTicks)
      {
        if (utcTicks >= _utcEndTicks)
        {
          DifferenceTicks = _calculator.GetOffsetFromUtcTicks(utcTicks, out _, out _utcEndTicks);
          return true;
        }
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
        return new DateTimeOffset(fromTimeZoneTicks + DifferenceTicks, new TimeSpan(DifferenceTicks));
      }
    }
  }
}
