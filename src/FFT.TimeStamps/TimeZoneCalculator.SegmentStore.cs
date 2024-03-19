namespace FFT.TimeStamps;

using System;
using System.Collections.Generic;
using System.Linq;
using static System.Math;

public partial class TimeZoneCalculator
{
  internal sealed class SegmentStore
  {
    private readonly TimeKind _kind;
    private readonly TimeZoneInfo _timeZone;

    private Dictionary<int, List<TimeZoneSegment>> _segmentsByYear = new();

    public SegmentStore(TimeKind kind, TimeZoneInfo timeZone)
      => (_kind, _timeZone) = (kind, timeZone);

    public TimeZoneSegment GetSegmentAt(long ticks)
    {
      var year = (int)(ticks / APPROXIMATE_TICKS_PER_YEAR);
      foreach (var segment in GetSegmentsForYear(year))
      {
        if (segment.StartTicks <= ticks)
          return segment;
      }

      throw new Exception("Compiler happiness");
    }

    private List<TimeZoneSegment> GetSegmentsForYear(int year)
    {
      if (_segmentsByYear.TryGetValue(year, out var segments))
        return segments;

      segments = CreateSegments(year);
      ImmutableInterlocked.Update(ref _segmentsByYear, x =>
      {
        if (x.ContainsKey(year))
          return x;

        var newDictionary = new Dictionary<int, List<TimeZoneSegment>>(x);
        newDictionary[year] = segments;
        return newDictionary;
      });

      return segments;
    }

    private List<TimeZoneSegment> CreateSegments(int year)
    {
      return Create(year).Reverse().ToList();

      IEnumerable<TimeZoneSegment> Create(int year)
      {
        var startOfYear = year * APPROXIMATE_TICKS_PER_YEAR;
        var endOfYear = startOfYear + APPROXIMATE_TICKS_PER_YEAR;
        var info = new OffsetInfo(startOfYear, _kind, _timeZone);
        var startTicks = startOfYear;
        var endTicks = startOfYear;
        while (true)
        {
          var testTicks = Min(endOfYear, endTicks.AddDays(7));
          var testInfo = new OffsetInfo(testTicks, _kind, _timeZone);
          if (testInfo.Equals(info))
          {
            endTicks = testTicks;
            if (endTicks == endOfYear)
            {
              yield return new TimeZoneSegment(_kind, startTicks, endTicks, info.IsInvalid, info.IsAmbiguous, info.OffsetTicks, this);
              yield break;
            }
          }
          else
          {
            while (true)
            {
              testTicks = endTicks.AddMinutes(1);
              if (testTicks == endOfYear)
              {
                endTicks = testTicks;
                yield return new TimeZoneSegment(_kind, startTicks, endTicks, info.IsInvalid, info.IsAmbiguous, info.OffsetTicks, this);
                yield break;
              }

              testInfo = new OffsetInfo(testTicks, _kind, _timeZone);
              if (testInfo.Equals(info))
              {
                endTicks = testTicks;
              }
              else
              {
                endTicks = testTicks;
                yield return new TimeZoneSegment(_kind, startTicks, endTicks, info.IsInvalid, info.IsAmbiguous, info.OffsetTicks, this);

                startTicks = testTicks;
                info = testInfo;
              }
            }
          }
        }
      }
    }

    private record struct OffsetInfo
    {
      public long OffsetTicks { get; }
      public bool IsInvalid { get; }
      public bool IsAmbiguous { get; }

      public OffsetInfo(long ticks, TimeKind ticksKind, TimeZoneInfo timeZone)
      {
        if (ticksKind == TimeKind.Utc)
        {
          OffsetTicks = timeZone.GetUtcOffset(new DateTime(ticks, DateTimeKind.Utc)).Ticks;
          IsInvalid = false;
          IsAmbiguous = false;
        }
        else if (ticksKind == TimeKind.TimeZone)
        {
          var at = new DateTime(ticks, DateTimeKind.Unspecified);
          OffsetTicks = timeZone.GetUtcOffset(at).Ticks;
          IsInvalid = timeZone.IsInvalidTime(at);
          IsAmbiguous = timeZone.IsAmbiguousTime(at);
        }
        else
        {
          throw new ArgumentException(nameof(ticksKind));
        }
      }
    }
  }
}
