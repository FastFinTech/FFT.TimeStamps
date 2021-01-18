//using System;
//using System.Collections.Concurrent;
//using System.Collections.Generic;
//using System.Runtime.CompilerServices;

//namespace FFT.TimeStamps
//{

//  /// <summary>
//  /// Use this class to get extremely fast timezone offset calculation results.
//  /// </summary>
//  public sealed class OldTimeZoneOffsetCache
//  {
//    //    Static code

//    /// <devremarks>
//    /// Keying the dictionary off the TimeZoneInfo object didn't seem to work, so it's keyed off the TimeZoneInfo.Id string instead.
//    /// </devremarks>
//    static readonly ConcurrentDictionary<string, OldTimeZoneOffsetCache> _store = new();

//    /// <summary>
//    /// Gets the TimeZoneOffsetCache for the given timeZone
//    /// </summary>
//    [MethodImpl(MethodImplOptions.AggressiveInlining)]
//    public static OldTimeZoneOffsetCache Get(TimeZoneInfo timeZone) => _store.GetOrAdd(timeZone.Id, static (_, tz) => new(tz), timeZone);

//    //    Instance code

//    // Used for collating groups of records into approximately one-year intervals.
//    const long APPROXIMATE_TICKS_PER_YEAR = TimeSpan.TicksPerDay * 365;

//    /// <summary>
//    /// The timezone for which this offset cache is applicable.
//    /// </summary>
//    public TimeZoneInfo TimeZone { get; }

//    private OldTimeZoneOffsetCache(TimeZoneInfo timeZone)
//      => TimeZone = timeZone;

//    #region **** Getting offsets for a DateTime in the TimeZone ****
//    #endregion

//    private readonly Dictionary<long, List<TicksAndOffset>> _timeZoneRecords = new Dictionary<long, List<TicksAndOffset>>();
//    private List<TicksAndOffset> GetTimeZoneRecords(in DateTime atTimeZone)
//    {
//      return GetTimeZoneRecords(atTimeZone.Ticks);
//    }
//    private List<TicksAndOffset> GetTimeZoneRecords(in long ticksTimeZone)
//    {
//      List<TicksAndOffset> records;
//      // Get a key for the group of records that we want to access
//      var key = ticksTimeZone - ticksTimeZone % APPROXIMATE_TICKS_PER_YEAR;
//      // Use the try-lock-try pattern to make sure we definitely only create one copy of the group
//      if (!_timeZoneRecords.TryGetValue(key, out records))
//      {
//        lock (_timeZoneRecords)
//        {
//          if (!_timeZoneRecords.TryGetValue(key, out records))
//          {
//            records = new List<TicksAndOffset>();
//            var ticks = key;
//            // Setup a record for the beginning of the group
//            records.Add(new TicksAndOffset
//            {
//              Ticks = ticks,
//              OffsetMinutes = CalculateOffsetMinutesForTimeZoneTicks(ticks),
//            });
//            // Get the key for the begining of the next group. We'll stop creating records when we reach this key
//            var nextKey = key + APPROXIMATE_TICKS_PER_YEAR;
//            // Setup storage for detecting offset changes
//            var previousOffsetMinutes = records[0].OffsetMinutes;
//            while (ticks < nextKey)
//            {
//              // We could simply iterate through and check the offset of every minute of the year. 
//              // But in an effort to make the code more efficient, mechanisms are provided to skip ahead
//              // by as much as a week at a time.
//              // Start by checking if the offset in a week's time is different
//              var nextWeekTicks = ticks + 7 * TimeSpan.TicksPerDay;
//              var nextWeekOffsetMinutes = CalculateOffsetMinutesForTimeZoneTicks(nextWeekTicks);
//              if (nextWeekOffsetMinutes == previousOffsetMinutes)
//              {
//                // Since the offset in a week's time is NOT different, we can simply skip ahead to next 
//                // week - there's no need to check each and every minute inside this week.
//                ticks = nextWeekTicks;
//              }
//              else
//              {
//                // There has been an offset change during this week. We'll have to dig in and find where it happened.
//                while (ticks < nextWeekTicks)
//                {
//                  var offsetMinutes = CalculateOffsetMinutesForTimeZoneTicks(ticks);
//                  if (offsetMinutes != previousOffsetMinutes && offsetMinutes != short.MinValue)
//                  { // And ignore "invalid" times, flagged using short.MinValue
//                    // We found the offset change
//                    records.Add(new TicksAndOffset
//                    {
//                      Ticks = ticks,
//                      OffsetMinutes = offsetMinutes,
//                    });
//                    previousOffsetMinutes = offsetMinutes;
//                    // If there are no more offset changes until the start of next week, we can skip directly to next week
//                    if (offsetMinutes == nextWeekOffsetMinutes)
//                    {
//                      ticks = nextWeekTicks;
//                      break; // breaks out of the while loop to avoid incrementing ticks past nextWeekTicks
//                    }
//                  }
//                  // Check every minute this week until we find the change
//                  ticks += TimeSpan.TicksPerMinute;
//                }
//              }
//            }
//            for (var i = 0; i < records.Count - 1; i++)
//            {
//              records[i].EndTicks = records[i + 1].Ticks - 1;
//            }
//            records[records.Count - 1].EndTicks = nextKey - 1;
//            records.Capacity = records.Count; // clean up a bit of memory
//            records.Reverse(); // keep the records in reverse order to make searching faster inside the GetOffsetMinutes method
//            _timeZoneRecords[key] = records;
//          }
//        }
//      }
//      return records;
//    }
//    private short CalculateOffsetMinutesForTimeZoneTicks(in long ticksTimeZone)
//    {
//      var at = new DateTime(ticksTimeZone, DateTimeKind.Unspecified);
//      if (TimeZone.IsInvalidTime(at)) return short.MinValue; // 'Invalid' times occur when the clock has jumped forward and skipped over a particular period.
//      return (short)TimeZone.GetUtcOffset(at).TotalMinutes;
//    }
//    /// <summary>
//    /// Gets the offset (in minutes) that would be applied inside TimeZone at the given timezone ticks.
//    /// </summary>
//    public short GetOffsetMinutes(in long ticksTimeZone)
//    {
//      // Iterate through the group of records for the "approximate year".
//      // I initially had this coded using linq expression "First" but we got a great performance improvement getting rid of linq and using the foreach
//      foreach (var record in GetTimeZoneRecords(ticksTimeZone))
//      {
//        if (record.Ticks <= ticksTimeZone) return record.OffsetMinutes;
//      }
//      // Should never reach this point - but we need the exception to make it compile
//      throw new NotImplementedException();
//    }
//    /// <summary>
//    /// Gets the offset (in minutes) that would be applied inside TimeZone at the given time, where time is expressed in the timezone time.
//    /// </summary>
//    public short GetOffsetMinutes(in DateTime atTimeZone)
//    {
//      return GetOffsetMinutes(atTimeZone.Ticks);
//    }

//    public short GetOffsetMinutes(in long ticksTimeZone, out long offsetEndTicksTimeZone)
//    {
//      foreach (var record in GetTimeZoneRecords(ticksTimeZone))
//      {
//        if (record.Ticks <= ticksTimeZone)
//        {
//          offsetEndTicksTimeZone = record.EndTicks;
//          return record.OffsetMinutes;
//        }
//      }
//      throw new NotImplementedException();
//    }

//    public short GetOffsetMinutes(in DateTime atTimeZone, out DateTime offsetEndTimeZone)
//    {
//      var offsetMinutes = GetOffsetMinutes(atTimeZone.Ticks, out var offsetEndTicksTimeZone);
//      offsetEndTimeZone = new DateTime(offsetEndTicksTimeZone, DateTimeKind.Unspecified);
//      return offsetMinutes;
//    }

//    #region **** Getting offsets for a UTC timestamp ****
//    #endregion

//    private readonly Dictionary<long, List<TicksAndOffset>> _utcRecords = new Dictionary<long, List<TicksAndOffset>>();
//    private List<TicksAndOffset> GetUtcRecords(in long utcTicks)
//    {
//      List<TicksAndOffset> records;
//      // Get a key for the group of records that we want to access
//      var key = utcTicks - utcTicks % APPROXIMATE_TICKS_PER_YEAR;
//      // Use the try-lock-try pattern to make sure we definitely only create one copy
//      if (!_utcRecords.TryGetValue(key, out records))
//      {
//        lock (_utcRecords)
//        {
//          if (!_utcRecords.TryGetValue(key, out records))
//          {
//            records = new List<TicksAndOffset>();
//            var ticks = key;
//            // Setup a record for the beginning of the group
//            records.Add(new TicksAndOffset
//            {
//              Ticks = ticks,
//              OffsetMinutes = CalculateOffsetMinutesForUTCTicks(ticks),
//            });
//            // Get the key for the begining of the next group. We'll stop creating records when we reach this key
//            var nextKey = key + APPROXIMATE_TICKS_PER_YEAR;
//            // Setup storage for detecting offset changes
//            var previousOffsetMinutes = records[0].OffsetMinutes;
//            while (ticks < nextKey)
//            {
//              // We could simply iterate through and check the offset of every minute of the year. 
//              // But in an effort to make the code more efficient, mechanisms are provided to skip ahead
//              // by as much as a week at a time.
//              // Start by checking if the offset in a week's time is different
//              var nextWeekTicks = ticks + 7 * TimeSpan.TicksPerDay;
//              var nextWeekOffsetMinutes = CalculateOffsetMinutesForUTCTicks(nextWeekTicks);
//              if (nextWeekOffsetMinutes == previousOffsetMinutes)
//              {
//                // Since the offset in a week's time is NOT different, we can simply skip ahead to next 
//                // week - there's no need to check each and every minute inside this week.
//                ticks = nextWeekTicks;
//              }
//              else
//              {
//                // There has been an offset change during this week. We'll have to dig in and find where it happened.
//                while (ticks < nextWeekTicks)
//                {
//                  var offsetMinutes = CalculateOffsetMinutesForUTCTicks(ticks);
//                  if (offsetMinutes != previousOffsetMinutes)
//                  {
//                    // We found the offset change
//                    records.Add(new TicksAndOffset
//                    {
//                      Ticks = ticks,
//                      OffsetMinutes = offsetMinutes,
//                    });
//                    previousOffsetMinutes = offsetMinutes;
//                    // If there are no more offset changes until the start of next week, we can skip directly to next week
//                    if (offsetMinutes == nextWeekOffsetMinutes)
//                    {
//                      ticks = nextWeekTicks;
//                      break; // breaks out of the while loop to avoid incrementing ticks past nextWeekTicks
//                    }
//                  }
//                  // Check every minute this week until we find the change
//                  ticks += TimeSpan.TicksPerMinute;
//                }
//              }
//            }
//            for (var i = 0; i < records.Count - 1; i++)
//            {
//              records[i].EndTicks = records[i + 1].Ticks - 1;
//            }
//            records[records.Count - 1].EndTicks = nextKey - 1;
//            records.Capacity = records.Count; // clean up a bit of memory
//            records.Reverse();  // keep the records in reverse order to make searching faster inside the GetOffsetMinutes method
//            _utcRecords[key] = records;
//          }
//        }
//      }
//      return records;
//    }
//    private short CalculateOffsetMinutesForUTCTicks(in long ticksUtc)
//    {
//      var at = new DateTimeOffset(ticksUtc, TimeSpan.Zero);
//      return (short)TimeZone.GetUtcOffset(at).TotalMinutes;
//    }
//    /// <summary>
//    /// Gets the offset (in minutes) that would be applied inside TimeZone at the given Timestamp.
//    /// </summary>
//    public short GetOffsetMinutes(in TimeStamp timeStamp)
//    {
//      // Iterate through the group of records for the "approximate year".
//      // I initially had this coded using linq expression "First" but we got a great performance improvement getting rid of linq and using the foreach
//      foreach (var record in GetUtcRecords(timeStamp.TicksUtc))
//      {
//        if (record.Ticks <= timeStamp.TicksUtc) return record.OffsetMinutes;
//      }
//      // Should never reach this point - but we need the exception to make it compile
//      throw new NotImplementedException();
//    }
//    public short GetOffsetMinutes(in TimeStamp timeStamp, out TimeStamp offsetEnd)
//    {
//      foreach (var record in GetUtcRecords(timeStamp.TicksUtc))
//      {
//        if (record.Ticks <= timeStamp.TicksUtc)
//        {
//          offsetEnd = record.EndTicks;
//          return record.OffsetMinutes;
//        }
//      }
//      throw new NotImplementedException();
//    }

//    private class TicksAndOffset
//    {
//      public long Ticks;
//      public long EndTicks;
//      public short OffsetMinutes;
//    }
//  }
//}
