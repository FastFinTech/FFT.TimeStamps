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
  public sealed class TimeZoneOffsetCalculator
  {
    ///<summary>
    /// Used for collating groups of records into approximately one-year intervals.
    ///</summary>
    private const long APPROXIMATE_TICKS_PER_YEAR = TimeSpan.TicksPerDay * 365;

    /// <devremarks>
    /// Keying the dictionary off the TimeZoneInfo object didn't seem to work, so it's keyed off the TimeZoneInfo.Id string instead.
    /// </devremarks>
    private static readonly ConcurrentDictionary<string, TimeZoneOffsetCalculator> _store = new();

    /// <summary>
    /// Gets the TimeZoneOffsetCache for the given timeZone
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static TimeZoneOffsetCalculator Get(TimeZoneInfo timeZone)
      => _store.GetOrAdd(timeZone.Id, static (id, tz) => new(tz), timeZone);

    /// <summary>
    /// The timezone for which this offset cache is applicable.
    /// </summary>
    public TimeZoneInfo TimeZone { get; }

    private readonly Dictionary<int, List<Segment>> _utcRecords = new();
    private readonly Dictionary<int, List<Segment>> _timezoneRecords = new();

    private TimeZoneOffsetCalculator(TimeZoneInfo timeZone)
      => TimeZone = timeZone;

    private Segment GetUTCSegment(in long utcTicks)
    {
      var approximateYear = (int)(utcTicks / APPROXIMATE_TICKS_PER_YEAR);
      if (!_utcRecords.TryGetValue(approximateYear, out var records))
      {
        records = CreateSegments(TimeZone, approximateYear, OffsetFunc)
          .Reverse()
          .ToList();
        _utcRecords[approximateYear] = records;
      }

      foreach (var record in records)
        if (record.StartTicks <= utcTicks)
          return record;

      throw new Exception("Boom"); // compiler happiness - never actually executes.

      static long OffsetFunc(TimeZoneInfo timeZone, long ticks)
        => timeZone.GetUtcOffset(new DateTime(ticks, Utc)).Ticks;
      //=> timeZone.GetUtcOffset(new DateTimeOffset(ticks, TimeSpan.Zero)).Ticks;
    }

    private Segment GetTimeZoneSegment(in long timeZoneTicks)
    {
      var approximateYear = (int)(timeZoneTicks / APPROXIMATE_TICKS_PER_YEAR);
      if (!_timezoneRecords.TryGetValue(approximateYear, out var records))
      {
        records = CreateSegments(TimeZone, approximateYear, OffsetFunc)
          .Reverse()
          .ToList();
        _timezoneRecords[approximateYear] = records;
      }

      foreach (var record in records)
        if (record.StartTicks <= timeZoneTicks)
          return record;

      throw new Exception("Boom"); // compiler happiness - never actually executes.

      static long OffsetFunc(TimeZoneInfo timeZone, long ticks)
      {
        var at = new DateTime(ticks, Unspecified);
        if (timeZone.IsAmbiguousTime(at))
        {

        }
        while (timeZone.IsInvalidTime(at))
          at = at.AddMinutes(1);
        return timeZone.GetUtcOffset(at).Ticks;
      };
    }

    /// <summary>
    /// Returns the offset of <see cref="TimeZone"/> at the given moment <paramref name="utcTicks"/>.
    /// </summary>
    /// <param name="utcTicks">The moment in time, expressed in utc ticks.</param>
    /// <param name="segmentStartUtcTicks">The start of a span of time with the same offset, expressed in utc ticks.</param>
    /// <param name="segmentEndUtcTicks">
    /// The end of a span of time with the same offset, expressed in utc ticks.
    /// Note that the span of time is EXCLUSIVE of this value, which is actually the start time of another span of time with a different offset.
    /// </param>
    /// <remarks>
    /// This operation involves searching cached timezone conversion records.
    /// It's fast compared to the methods provided by <see cref="TimeZoneInfo"/>
    /// but don't use it in your application hot-path unless you are caching the segment start and end times and then optimizing from there.
    /// </remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public long GetOffsetFromUtcTicks(in long utcTicks, out long segmentStartUtcTicks, out long segmentEndUtcTicks)
    {
      var segment = GetUTCSegment(utcTicks);
      segmentStartUtcTicks = segment.StartTicks;
      segmentEndUtcTicks = segment.EndTicks;
      return segment.OffsetTicks;
    }

    /// <summary>
    /// Returns the offset of <see cref="TimeZone"/> at the given moment <paramref name="timeZoneTicks"/>.
    /// </summary>
    /// <param name="timeZoneTicks">The moment in time, expressed in timezone ticks.</param>
    /// <param name="segmentStartTimeZoneTicks">The start of a span of time with the same offset, expressed in timezone ticks.</param>
    /// <param name="segmentEndTimeZoneTicks">
    /// The end of a span of time with the same offset, expressed in timezone ticks.
    /// Note that the span of time is EXCLUSIVE of this value, which is actually the start time of another span of time with a different offset.
    /// </param>
    /// <remarks>
    /// This operation involves searching cached timezone conversion records.
    /// It's fast compared to the methods provided by <see cref="TimeZoneInfo"/>
    /// but don't use it in your application hot-path unless you are caching the segment start and end times and then optimizing from there.
    /// </remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public long GetOffsetFromTimeZoneTicks(in long timeZoneTicks, out long segmentStartTimeZoneTicks, out long segmentEndTimeZoneTicks)
    {
      var segment = GetTimeZoneSegment(timeZoneTicks);
      segmentStartTimeZoneTicks = segment.StartTicks;
      segmentEndTimeZoneTicks = segment.EndTicks;
      return segment.OffsetTicks;
    }

    /// <summary>
    /// Converts utc ticks to timezone ticks.
    /// Compute intensive. Do not use in hot path.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public long ToTimeZoneTicks(in long utcTicks)
      => utcTicks + GetOffsetFromUtcTicks(utcTicks, out _, out _);

    /// <summary>
    /// Converts timezone ticks to utc ticks.
    /// Compute intensive. Do not use in hot path.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public long ToUtcTicks(in long timeZoneTicks)
      => timeZoneTicks - GetOffsetFromUtcTicks(timeZoneTicks, out _, out _);

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

    private static IEnumerable<Segment> CreateSegments(TimeZoneInfo timeZone, int approximateYear, Func<TimeZoneInfo, long, long> offsetFunc)
    {
      var startOfYear = approximateYear * APPROXIMATE_TICKS_PER_YEAR;
      var endOfYear = startOfYear + APPROXIMATE_TICKS_PER_YEAR;
      var record = new Segment();
      record.StartTicks = startOfYear;
      record.EndTicks = startOfYear;
      record.OffsetTicks = offsetFunc(timeZone, startOfYear);

      while (true)
      {
        var ticks = Min(endOfYear, record.EndTicks.AddDays(7));
        var offset = offsetFunc(timeZone, ticks);
        if (offset == record.OffsetTicks)
        {
          record.EndTicks = ticks;
          if (ticks == endOfYear)
          {
            yield return record;
            yield break;
          }
        }
        else
        {
          while (true)
          {
            ticks = Min(endOfYear, record.EndTicks.AddMinutes(1));
            offset = offsetFunc(timeZone, ticks);
            record.EndTicks = ticks;
            if (ticks == endOfYear)
            {
              yield return record;
              yield break;
            }
            if (offset != record.OffsetTicks)
            {
              yield return record;
              record = new Segment();
              record.StartTicks = record.EndTicks = ticks;
              record.OffsetTicks = offset;
              break;
            }
          }
        }
      }
    }

    private sealed class Segment
    {
      public long StartTicks;
      public long EndTicks;
      public long OffsetTicks;
    }

    private sealed class OffsetInfo
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

    public sealed class TimeZoneSegment
    {
      public long StartTicks;
      public long EndTicks;
      public long OffsetTicks;
      public bool IsInvalid;
      public bool IsAmbiguous;
      public long OffsetTicks2;
    }
  }
}
