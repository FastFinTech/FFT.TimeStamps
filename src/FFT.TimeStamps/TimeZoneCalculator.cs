using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using static System.DateTimeKind;
using static System.Math;

namespace FFT.TimeStamps
{
  /// <summary>
  /// Use this class to get extremely fast timezone offset calculation results.
  /// </summary>
  public sealed partial class TimeZoneCalculator
  {
    ///<summary>
    /// Used for collating groups of records into approximately one-year intervals.
    ///</summary>
    private const long APPROXIMATE_TICKS_PER_YEAR = TimeSpan.TicksPerDay * 365;

    /// <devremarks>
    /// Keying the dictionary off the TimeZoneInfo object didn't seem to work, so it's keyed off the TimeZoneInfo.Id string instead.
    /// </devremarks>
    private static readonly ConcurrentDictionary<string, TimeZoneCalculator> _store = new();

    /// <summary>
    /// Gets the TimeZoneOffsetCache for the given timeZone
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static TimeZoneCalculator Get(TimeZoneInfo timeZone)
      => _store.GetOrAdd(timeZone.Id, static (id, tz) => new(tz), timeZone);

    /// <summary>
    /// The timezone for which this offset cache is applicable.
    /// </summary>
    public TimeZoneInfo TimeZone { get; }

    private readonly Dictionary<int, List<TimeZoneSegment>> _utcRecords = new();
    private readonly Dictionary<int, List<TimeZoneSegment>> _timezoneRecords = new();

    private TimeZoneCalculator(TimeZoneInfo timeZone)
      => TimeZone = timeZone;

    /// <summary>
    /// Returns a <see cref="TimeZoneSegment"/> active at the given <paramref name="timeStamp"/>.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public TimeZoneSegment GetSegment(in TimeStamp timeStamp)
      => GetSegment(timeStamp.TicksUtc, TimeKind.Utc);

    /// <summary>
    /// Returns a <see cref="TimeZoneSegment"/> active at the given time expressed in ticks in the timezone specified by <paramref name="ticksKind"/>.
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
        if (segment.StartTicks <= ticks)
          return segment;

      throw new Exception("Boom"); // compiler happiness - never actually executes.
    }

    ///// <summary>
    ///// Returns a <see cref="TimeZoneSegment"/> active at the given time <paramref name="utcTicks"/> expressed in the UTC timezone.
    ///// </summary>
    //public TimeZoneSegment GetSegmentByUtcTicks(in long utcTicks)
    //{
    //  var approximateYear = (int)(utcTicks / APPROXIMATE_TICKS_PER_YEAR);
    //  if (!_utcRecords.TryGetValue(approximateYear, out var records))
    //  {
    //    records = CreateSegments(TimeZone, approximateYear, InfoFunc);
    //    _utcRecords[approximateYear] = records;
    //  }

    //  foreach (var record in records)
    //    if (record.StartTicks <= utcTicks)
    //      return record;

    //  throw new Exception("Boom"); // compiler happiness - never actually executes.

    //  static OffsetInfo InfoFunc(TimeZoneInfo timeZone, long ticks) => new OffsetInfo
    //  {
    //    OffsetTicks = timeZone.GetUtcOffset(new DateTime(ticks, Utc)).Ticks,
    //    IsAmbiguous = false,
    //    IsInvalid = false,
    //  };
    //}

    ///// <summary>
    ///// Returns a <see cref="TimeZoneSegment"/> active at the given time <paramref name="timeZoneTicks"/> expressed in the <see cref="TimeZone"/> timezone.
    ///// </summary>
    //public TimeZoneSegment GetSegmentByTimezoneTicks(in long timeZoneTicks)
    //{
    //  var approximateYear = (int)(timeZoneTicks / APPROXIMATE_TICKS_PER_YEAR);
    //  if (!_timezoneRecords.TryGetValue(approximateYear, out var records))
    //  {
    //    records = CreateSegments(TimeZone, approximateYear, InfoFunc);
    //    _timezoneRecords[approximateYear] = records;
    //  }

    //  foreach (var record in records)
    //    if (record.StartTicks <= timeZoneTicks)
    //      return record;

    //  throw new Exception("Boom"); // compiler happiness - never actually executes.

    //  static OffsetInfo InfoFunc(TimeZoneInfo timeZone, long ticks)
    //  {
    //    var at = new DateTime(ticks, Unspecified);
    //    return new OffsetInfo
    //    {
    //      OffsetTicks = timeZone.GetUtcOffset(at).Ticks,
    //      IsAmbiguous = timeZone.IsAmbiguousTime(at),
    //      IsInvalid = timeZone.IsInvalidTime(at),
    //    };
    //  };
    //}

    ///// <summary>
    ///// Returns the offset of <see cref="TimeZone"/> at the given moment <paramref name="utcTicks"/>.
    ///// </summary>
    ///// <param name="utcTicks">The moment in time, expressed in utc ticks.</param>
    ///// <param name="segmentStartUtcTicks">The start of a span of time with the same offset, expressed in utc ticks.</param>
    ///// <param name="segmentEndUtcTicks">
    ///// The end of a span of time with the same offset, expressed in utc ticks.
    ///// Note that the span of time is EXCLUSIVE of this value, which is actually the start time of another span of time with a different offset.
    ///// </param>
    ///// <remarks>
    ///// This operation involves searching cached timezone conversion records.
    ///// It's fast compared to the methods provided by <see cref="TimeZoneInfo"/>
    ///// but don't use it in your application hot-path unless you are caching the segment start and end times and then optimizing from there.
    ///// </remarks>
    //[MethodImpl(MethodImplOptions.AggressiveInlining)]
    //public long GetOffsetFromUtcTicks(in long utcTicks, out long segmentStartUtcTicks, out long segmentEndUtcTicks)
    //{
    //  var segment = GetSegmentByUtcTicks(utcTicks);
    //  segmentStartUtcTicks = segment.StartTicks;
    //  segmentEndUtcTicks = segment.EndTicks;
    //  return segment.OffsetTicks;
    //}

    ///// <summary>
    ///// Returns the offset of <see cref="TimeZone"/> at the given moment <paramref name="timeZoneTicks"/>.
    ///// </summary>
    ///// <param name="timeZoneTicks">The moment in time, expressed in timezone ticks.</param>
    ///// <param name="segmentStartTimeZoneTicks">The start of a span of time with the same offset, expressed in timezone ticks.</param>
    ///// <param name="segmentEndTimeZoneTicks">
    ///// The end of a span of time with the same offset, expressed in timezone ticks.
    ///// Note that the span of time is EXCLUSIVE of this value, which is actually the start time of another span of time with a different offset.
    ///// </param>
    ///// <remarks>
    ///// This operation involves searching cached timezone conversion records.
    ///// It's fast compared to the methods provided by <see cref="TimeZoneInfo"/>
    ///// but don't use it in your application hot-path unless you are caching the segment start and end times and then optimizing from there.
    ///// </remarks>
    //[MethodImpl(MethodImplOptions.AggressiveInlining)]
    //public long GetOffsetFromTimeZoneTicks(in long timeZoneTicks, out long segmentStartTimeZoneTicks, out long segmentEndTimeZoneTicks)
    //{
    //  var segment = GetSegmentByTimezoneTicks(timeZoneTicks);
    //  segmentStartTimeZoneTicks = segment.StartTicks;
    //  segmentEndTimeZoneTicks = segment.EndTicks;
    //  return segment.OffsetTicks;
    //}

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
        var builder = new Builder();
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
                builder = new Builder();
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

    private struct OffsetInfo
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
