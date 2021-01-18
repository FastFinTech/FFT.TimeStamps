using System;
using System.Runtime.CompilerServices;

namespace FFT.TimeStamps
{
  public partial class ConversionIterators
  {
    private sealed class DualTimeZoneIterator : ITimeZoneConversionIterator
    {
      public TimeZoneInfo FromTimeZone { get; }
      public TimeZoneInfo ToTimeZone { get; }
      public long DifferenceTicks { get; private set; }

      private readonly ToUtcIterator _toUtc;
      private readonly FromUtcIterator _fromUtc;

      public DualTimeZoneIterator(TimeZoneInfo fromTimeZone, TimeZoneInfo toTimeZone)
      {
        FromTimeZone = fromTimeZone;
        ToTimeZone = toTimeZone;
        _toUtc = new ToUtcIterator(fromTimeZone);
        _fromUtc = new FromUtcIterator(toTimeZone);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public bool MoveTo(in long fromTimeZoneTicks)
      {
        var change = _toUtc.MoveTo(fromTimeZoneTicks);
        var utcTicks = fromTimeZoneTicks + _toUtc.DifferenceTicks;
        change = _fromUtc.MoveTo(utcTicks) || change;
        if (change)
        {
          DifferenceTicks = _toUtc.DifferenceTicks + _fromUtc.DifferenceTicks;
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
        return new DateTimeOffset(fromTimeZoneTicks + DifferenceTicks, new TimeSpan(_fromUtc.DifferenceTicks));
      }
    }
  }
}
