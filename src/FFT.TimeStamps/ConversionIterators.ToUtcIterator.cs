﻿// Copyright (c) True Goodwill. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace FFT.TimeStamps
{
  using System;
  using System.Runtime.CompilerServices;

  public partial class ConversionIterators
  {
    private sealed class ToUtcIterator : ITimeZoneConversionIterator, IToTimeStampConversionIterator
    {
      private readonly TimeZoneCalculator _calculator;

      private long _previousTzTicks;
      private long _tzEndTicks;
      private TimeZoneCalculator.TimeZoneSegment? _segment;

      public ToUtcIterator(TimeZoneInfo fromTimeZone)
      {
        FromTimeZone = fromTimeZone;
        _calculator = TimeZoneCalculator.Get(fromTimeZone);
      }

      public TimeZoneInfo FromTimeZone { get; }

      public TimeZoneInfo ToTimeZone => TimeZoneInfo.Utc;

      public long DifferenceTicks { get; private set; }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public bool MoveTo(long ticks)
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

          _previousTzTicks = ticks;
          return true;
        }

        // When in an ambiguous zone, we need to catch the moment when the clock
        // flies back as it reverts from daylight savings back to standard time.
        if (_segment!.IsAmbiguous && ticks < _previousTzTicks)
        {
          DifferenceTicks = -_segment.OffsetTicks;
          _previousTzTicks = ticks;
          return true;
        }

        _previousTzTicks = ticks;
        return false;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public long GetTicks(long fromTimeZoneTicks)
      {
        MoveTo(fromTimeZoneTicks);
        return fromTimeZoneTicks + DifferenceTicks;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public DateTime GetDateTime(long fromTimeZoneTicks)
      {
        MoveTo(fromTimeZoneTicks);
        return new DateTime(fromTimeZoneTicks + DifferenceTicks, DateTimeKind.Unspecified);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public DateTimeOffset GetDateTimeOffset(long fromTimeZoneTicks)
      {
        MoveTo(fromTimeZoneTicks);
        return new DateTimeOffset(fromTimeZoneTicks + DifferenceTicks, TimeSpan.Zero);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public TimeStamp GetTimeStamp(long fromTimeZoneTicks)
      {
        MoveTo(fromTimeZoneTicks);
        return new TimeStamp(fromTimeZoneTicks + DifferenceTicks);
      }
    }
  }
}
