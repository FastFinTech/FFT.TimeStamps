// Copyright (c) True Goodwill. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace FFT.TimeStamps
{
  using System;
  using System.Collections.Concurrent;
  using System.Collections.Generic;
  using System.Linq;
  using System.Runtime.CompilerServices;
  using static System.DateTimeKind;
  using static System.Math;

  /// <summary>
  /// Use this class to get extremely fast timezone offset calculation results.
  /// </summary>
  public sealed partial class TimeZoneCalculator
  {
    /// <summary>
    /// Used for collating groups of records into approximately one-year intervals.
    /// </summary>
    private const long APPROXIMATE_TICKS_PER_YEAR = TimeSpan.TicksPerDay * 365;

    /// <devremarks>
    /// Keying the dictionary off the TimeZoneInfo object didn't seem to work, so it's keyed off the TimeZoneInfo.Id string instead.
    /// </devremarks>
    private static readonly ConcurrentDictionary<string, TimeZoneCalculator> _store = new();

    private readonly Dictionary<int, List<TimeZoneSegment>> _utcRecords = new();
    private readonly Dictionary<int, List<TimeZoneSegment>> _timezoneRecords = new();

    private TimeZoneCalculator(TimeZoneInfo timeZone)
      => TimeZone = timeZone;

    /// <summary>
    /// The timezone for which this offset cache is applicable.
    /// </summary>
    public TimeZoneInfo TimeZone { get; }

    /// <summary>
    /// Converts <paramref name="fromTimeZoneTicks"/> from <paramref name="fromTimeZone"/> to <paramref name="toTimeZone"/>.
    /// Compute intensive. Do not use in hot path.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static long Convert(TimeZoneInfo fromTimeZone, TimeZoneInfo toTimeZone, in long fromTimeZoneTicks)
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
      => _store.GetOrAdd(timeZone.Id, static (id, tz) => new(tz), timeZone);

    /// <summary>
    /// Returns a <see cref="TimeZoneSegment"/> active at the given <paramref name="timeStamp"/>.
    /// The returned segment will have its <see cref="TimeZoneSegment.SegmentKind"/> property set to <see cref="TimeKind.Utc"/>.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public TimeZoneSegment GetSegment(in TimeStamp timeStamp)
      => GetSegment(timeStamp.TicksUtc, TimeKind.Utc);

    /// <summary>
    /// Returns a <see cref="TimeZoneSegment"/> active at the given time expressed in ticks in the timezone specified by <paramref name="ticksKind"/>.
    /// The returned segment will have its <see cref="TimeZoneSegment.SegmentKind"/> property set to <paramref name="ticksKind"/>.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public TimeZoneSegment GetSegment(in long ticks, TimeKind ticksKind)
    {
      var approximateYear = (int)(ticks / APPROXIMATE_TICKS_PER_YEAR);
      var dictionary = ticksKind switch
      {
        TimeKind.Utc => _utcRecords,
        TimeKind.TimeZone => _timezoneRecords,
        _ => throw new ArgumentException(nameof(ticksKind)),
      };

      if (!dictionary.TryGetValue(approximateYear, out var segments))
      {
        segments = CreateSegments(approximateYear, ticksKind);
        dictionary[approximateYear] = segments;
      }

      foreach (var segment in segments)
      {
        if (segment.StartTicks <= ticks)
          return segment;
      }

      throw new Exception("Boom"); // compiler happiness - never actually executes.
    }

    /// <summary>
    /// Converts utc ticks to timezone ticks.
    /// Compute intensive. Do not use in hot path.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public long ToTimeZoneTicks(in long utcTicks)
      => utcTicks + GetSegment(utcTicks, TimeKind.Utc).OffsetTicks;

    /// <summary>
    /// Converts timezone ticks to utc ticks.
    /// Ambiguous times are considered to be in the standard (not daylight savings) offset.
    /// Invalid times are considered to be in the standard (not daylight savings) offset.
    /// Compute intensive. Do not use in hot path.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public long ToUtcTicks(in long timeZoneTicks)
      => timeZoneTicks - GetSegment(timeZoneTicks, TimeKind.TimeZone).OffsetTicks;

    private List<TimeZoneSegment> CreateSegments(int approximateYear, TimeKind ticksKind)
    {
      return CreateCore(approximateYear, ticksKind)
        .Reverse()
        .ToList();

      IEnumerable<TimeZoneSegment> CreateCore(int approximateYear, TimeKind ticksKind)
      {
        var startOfYear = approximateYear * APPROXIMATE_TICKS_PER_YEAR;
        var endOfYear = startOfYear + APPROXIMATE_TICKS_PER_YEAR;
        var info = GetInfo(startOfYear, ticksKind);
        var builder = default(Builder);
        builder.Kind = ticksKind;
        builder.Calculator = this;
        builder.Info = info;
        builder.StartTicks = startOfYear;
        builder.EndTicks = startOfYear;
        while (true)
        {
          var ticks = Min(endOfYear, builder.EndTicks.AddDays(7));
          info = GetInfo(ticks, ticksKind);
          if (builder.Info.Equals(info))
          {
            builder.EndTicks = ticks;
            if (ticks == endOfYear)
            {
              yield return builder.Build();
              yield break;
            }
          }
          else
          {
            while (true)
            {
              ticks = Min(endOfYear, builder.EndTicks.AddMinutes(1));
              info = GetInfo(ticks, ticksKind);
              builder.EndTicks = ticks;
              if (ticks == endOfYear)
              {
                yield return builder.Build();
                yield break;
              }
              if (!builder.Info.Equals(info))
              {
                yield return builder.Build();
                builder = default(Builder);
                builder.Kind = ticksKind;
                builder.Calculator = this;
                builder.Info = info;
                builder.StartTicks = ticks;
                builder.EndTicks = ticks;
                break;
              }
            }
          }
        }
      }
    }

    private OffsetInfo GetInfo(in long ticks, TimeKind ticksKind)
    {
      if (ticksKind == TimeKind.Utc)
      {
        return new OffsetInfo
        {
          OffsetTicks = TimeZone.GetUtcOffset(new DateTime(ticks, Utc)).Ticks,
          IsInvalid = false,
          IsAmbiguous = false,
        };
      }
      else if (ticksKind == TimeKind.TimeZone)
      {
        var at = new DateTime(ticks, Unspecified);
        return new OffsetInfo
        {
          OffsetTicks = TimeZone.GetUtcOffset(at).Ticks,
          IsInvalid = TimeZone.IsInvalidTime(at),
          IsAmbiguous = TimeZone.IsAmbiguousTime(at),
        };
      }
      else
      {
        throw new ArgumentException(nameof(ticksKind));
      }
    }

#pragma warning disable CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
    private struct OffsetInfo
#pragma warning restore CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
    {
      public long OffsetTicks;
      public bool IsInvalid;
      public bool IsAmbiguous;

      public override bool Equals(object obj)
        => obj is OffsetInfo other
        && OffsetTicks == other.OffsetTicks
        && IsInvalid == other.IsInvalid
        && IsAmbiguous == other.IsAmbiguous;
    }

    private struct Builder
    {
      public OffsetInfo Info;
      public long StartTicks;
      public long EndTicks;
      public TimeKind Kind;
      public TimeZoneCalculator Calculator;

      public TimeZoneSegment Build() => new TimeZoneSegment(
        segmentKind: Kind,
        startTicks: StartTicks,
        endTicks: EndTicks,
        isInvalid: Info.IsInvalid,
        isAmbiguous: Info.IsAmbiguous,
        offsetTicks: Info.OffsetTicks,
        calculator: Calculator);
    }
  }
}
