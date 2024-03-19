// Copyright (c) True Goodwill. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace FFT.TimeStamps
{
  using System;
  using System.Runtime.CompilerServices;

  /// <summary>
  /// Use this class to get extremely fast timezone conversion data.
  /// see [Article](xref:article-timezonecalculator).
  /// </summary>
  public sealed partial class TimeZoneCalculator
  {
    /// <summary>
    /// Used for collating groups of records into approximately one-year intervals.
    /// </summary>
    private const long APPROXIMATE_TICKS_PER_YEAR = TimeSpan.TicksPerDay * 365;

    private readonly SegmentStore _utcStore;
    private readonly SegmentStore _timeZoneStore;

    private TimeZoneCalculator(TimeZoneInfo timeZone)
    {
      TimeZone = timeZone;
      _utcStore = new(TimeKind.Utc, timeZone);
      _timeZoneStore = new(TimeKind.TimeZone, timeZone);
    }

    /// <summary>
    /// The timezone for which this offset cache is applicable.
    /// </summary>
    public TimeZoneInfo TimeZone { get; }

    /// <summary>
    /// Converts <paramref name="fromTimeZoneTicks"/> from <paramref name="fromTimeZone"/> to <paramref name="toTimeZone"/>.
    /// Compute intensive. Do not use in hot path.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static long Convert(TimeZoneInfo fromTimeZone, TimeZoneInfo toTimeZone, long fromTimeZoneTicks)
    {
      if (fromTimeZone == toTimeZone) return fromTimeZoneTicks;
      var utcTicks = fromTimeZone == TimeZoneInfo.Utc ? fromTimeZoneTicks : Get(fromTimeZone).ToUtcTicks(fromTimeZoneTicks);
      return toTimeZone == TimeZoneInfo.Utc ? utcTicks : Get(toTimeZone).ToTimeZoneTicks(utcTicks);
    }

    /// <summary>
    /// Gets the TimeZoneOffsetCache for the given timeZone.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static TimeZoneCalculator Get(TimeZoneInfo timeZone)
      => Store.Get(timeZone);

    /// <summary>
    /// Returns a <see cref="TimeZoneSegment"/> active at the given <paramref name="timeStamp"/>.
    /// The returned segment will have its <see cref="TimeZoneSegment.SegmentKind"/> property set to <see cref="TimeKind.Utc"/>.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public TimeZoneSegment GetSegment(TimeStamp timeStamp)
      => GetSegment(timeStamp.TicksUtc, TimeKind.Utc);

    /// <summary>
    /// Returns a <see cref="TimeZoneSegment"/> active at the given time expressed in ticks in the timezone specified by <paramref name="ticksKind"/>.
    /// The returned segment will have its <see cref="TimeZoneSegment.SegmentKind"/> property set to <paramref name="ticksKind"/>.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public TimeZoneSegment GetSegment(long ticks, TimeKind ticksKind)
    {
      return ticksKind switch
      {
        TimeKind.Utc => _utcStore.GetSegmentAt(ticks),
        TimeKind.TimeZone => _timeZoneStore.GetSegmentAt(ticks),
        _ => throw new NotImplementedException(),
      };
    }

    /// <summary>
    /// Converts utc ticks to timezone ticks.
    /// Compute intensive. Do not use in hot path.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public long ToTimeZoneTicks(long utcTicks)
      => utcTicks + GetSegment(utcTicks, TimeKind.Utc).OffsetTicks;

    /// <summary>
    /// Converts timezone ticks to utc ticks.
    /// Ambiguous times are considered to be in the standard (not daylight savings) offset.
    /// Invalid times are considered to be in the standard (not daylight savings) offset.
    /// Compute intensive. Do not use in hot path.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public long ToUtcTicks(long timeZoneTicks)
      => timeZoneTicks - GetSegment(timeZoneTicks, TimeKind.TimeZone).OffsetTicks;

  }
}
