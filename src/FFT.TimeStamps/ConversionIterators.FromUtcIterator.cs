// Copyright (c) True Goodwill. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace FFT.TimeStamps
{
  using System;
  using System.Runtime.CompilerServices;

  public partial class ConversionIterators
  {
    private sealed class FromUtcIterator : ITimeZoneConversionIterator, IFromTimeStampConversionIterator
    {
      private readonly TimeZoneCalculator _calculator;

      private long _utcEndTicks;

      public FromUtcIterator(TimeZoneInfo toTimeZone)
      {
        ToTimeZone = toTimeZone;
        _calculator = TimeZoneCalculator.Get(toTimeZone);
      }

      public TimeZoneInfo FromTimeZone => TimeZoneInfo.Utc;

      public TimeZoneInfo ToTimeZone { get; }

      public long DifferenceTicks { get; private set; }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public bool MoveTo(in long utcTicks)
      {
        if (utcTicks >= _utcEndTicks)
        {
          var segment = _calculator.GetSegment(utcTicks, TimeKind.Utc);
          DifferenceTicks = segment.OffsetTicks;
          _utcEndTicks = segment.EndTicks;
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

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public DateTime GetDateTime(in TimeStamp timeStamp)
        => GetDateTime(timeStamp.TicksUtc);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public DateTimeOffset GetDateTimeOffset(in TimeStamp timeStamp)
        => GetDateTimeOffset(timeStamp.TicksUtc);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public long GetTicks(in TimeStamp timeStamp)
      {
        MoveTo(timeStamp.TicksUtc);
        return timeStamp.TicksUtc + DifferenceTicks;
      }
    }
  }
}
