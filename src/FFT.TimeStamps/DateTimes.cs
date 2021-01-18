using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using static System.DayOfWeek;

namespace FFT.TimeStamps
{
  /// <summary>
  /// Contains methods for manipulating <see cref="DateTime"/> values.
  /// </summary>
  public static partial class DateTimes
  {
  }

  // ToIndex
  public partial class DateTimes
  {
    /// <summary>
    /// Rounds the <paramref name="timestamp"/> down to the start of the second and then 
    /// returns zero-based index of the second within the day. 
    /// Values returned range from 0 to 1440 * 60 - 1
    /// </summary>        
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int ToSecondOfDayIndex(in this DateTime timestamp)
      => timestamp.Ticks.ToSecondOfDayIndex();

    /// <summary>
    /// Rounds the second down to the start of the second and then 
    /// returns zero-based index of the second within the week. 
    /// Values returned range from 0 to 6 * 1440 * 60 - 1
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int ToSecondOfWeekIndex(in this DateTime timestamp)
      => timestamp.Ticks.ToSecondOfWeekIndex();

    /// <summary>
    /// Rounds the minute down to the start of the minute and then 
    /// returns the zero-based index of the minute within the day.
    /// Values returned range from 0 to 1439
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int ToMinuteOfDayIndex(in this DateTime timestamp)
      => timestamp.Ticks.ToMinuteOfDayIndex();

    /// <summary>
    /// Rounds the minute down to the start of the minute and then 
    /// returns zero-based index of the minute within the week. 
    /// Values returned range from 0 to 6 * 1440 - 1
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int ToMinuteOfWeekIndex(in this DateTime timestamp)
      => timestamp.Ticks.ToMinuteOfWeekIndex();
  }

  // Floor and Ceiling
  public partial class DateTimes
  {
    /// <summary>
    /// Rounds the given timestamp down to the start of the millisecond.
    /// If the timestamp is at an exact millisecond, it returns a copy of the same timestamp.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static DateTime ToMillisecondFloor(in this DateTime timestamp)
      => new DateTime(timestamp.Ticks.ToMillisecondFloor(), timestamp.Kind);

    /// <summary>
    /// Rounds the given timestamp up to the start of the millisecond.
    /// If the timestamp is at an exact millisecond, it returns a copy of the same timestamp.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static DateTime ToMillisecondCeiling(in this DateTime timestamp)
      => new DateTime(timestamp.Ticks.ToMillisecondsCeiling(), timestamp.Kind);

    /// <summary>
    /// Rounds the given timestamp down to the start of the second.
    /// If the timestamp is at an exact second, it returns a copy of the same timestamp.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static DateTime ToSecondFloor(in this DateTime timestamp)
      => new DateTime(timestamp.Ticks.ToSecondFloor(), timestamp.Kind);

    /// <summary>
    /// Rounds the given timestamp up to the start of the next second. 
    /// If the timestamp is at an exact second, it returns a copy of the same timestamp.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static DateTime ToSecondCeiling(in this DateTime timestamp)
      => new DateTime(timestamp.Ticks.ToSecondCeiling(), timestamp.Kind);

    /// <summary>
    /// Rounds the timestamp down to the start of the minute
    /// If the timestamp is at an exact minute, it returns a copy of the same timestamp
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static DateTime ToMinuteFloor(in this DateTime timestamp)
      => new DateTime(timestamp.Ticks.ToMinuteFloor(), timestamp.Kind);

    /// <summary>
    /// Rounds the timestamp up to the start of the next minute
    /// If the timestamp is at an exact minute, it returns a copy of the same timestamp
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static DateTime ToMinuteCeiling(in this DateTime timestamp)
      => new DateTime(timestamp.Ticks.ToMinuteCeiling(), timestamp.Kind);

    /// <summary>
    /// Rounds the timestamp down to the start of the hour.
    /// If the timestamp is at an exact hour, it returns a copy of the same timestamp
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static DateTime ToHourFloor(in this DateTime timestamp)
      => new DateTime(timestamp.Ticks.ToHourFloor(), timestamp.Kind);

    /// <summary>
    /// Rounds the timestamp up to the start of the next hour
    /// If the timestamp is at an exact hour, it returns a copy of the same timestamp
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static DateTime ToHourCeiling(in this DateTime timestamp)
      => new DateTime(timestamp.Ticks.ToHourCeiling(), timestamp.Kind);

    /// <summary>
    /// Rounds the timestamp down to the start of the day.
    /// If the timestamp is at an exact day, it returns a copy of the same timestamp
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static DateTime ToDayFloor(in this DateTime timestamp)
      => new DateTime(timestamp.Ticks.ToDayFloor(), timestamp.Kind);

    /// <summary>
    /// Rounds the timestamp up to the start of the next day.
    /// If the timestamp is at an exact day, it returns a copy of the same timestamp
    /// </summary>
    /// <param name="timestamp"></param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static DateTime ToDayCeiling(in this DateTime timestamp)
      => new DateTime(timestamp.Ticks.ToDayCeiling(), timestamp.Kind);

    /// <summary>
    /// Rounds the given timestamp down to the start of the week (00:00:00 Sunday), returning the same timestamp if it is already on Sunday midnight.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static DateTime ToWeekFloor(in this DateTime timestamp)
      => new DateTime(timestamp.Ticks.ToWeekFloor(), timestamp.Kind);

    /// <summary>
    /// Rounds the given timestamp up to the end of the week (00:00:00 Sunday), returning the same timestamp if it is already on Sunday midnight.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static DateTime ToWeekCeiling(in this DateTime timestamp)
      => new DateTime(timestamp.Ticks.ToWeekCeiling(), timestamp.Kind);

    /// <summary>
    /// Rounds the given timestamp down to the beginning of the month.
    /// If the timestamp is at the exact beginning of a month, it returns a copy of the same timestamp.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static DateTime ToMonthFloor(in this DateTime timestamp)
      => new DateTime(timestamp.Year, timestamp.Month, 1, 0, 0, 0, timestamp.Kind);

    /// <summary>
    /// Rounds the given timestamp up to the start of the next month.
    /// If the timestamp is at the exact beginning of a month, it returns a copy of the same timestamp.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static DateTime ToMonthCeiling(in this DateTime timestamp)
    {
      var floor = timestamp.ToMonthFloor();
      return timestamp == floor ? timestamp : floor.AddMonths(1);
    }
  }

  // GetNext and GetPrevious
  public partial class DateTimes
  {
    /// <summary>
    /// Calculates the moment at <paramref name="timeOfDay"/> that mostly recently occurs at or before <paramref name="timestamp"/>.
    /// You will get unexpected results if <paramref name="timeOfDay"/> is less than zero or greater than 24 hours.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static DateTime GetPrevious(in this DateTime timestamp, in TimeSpan timeOfDay)
      => new DateTime(timestamp.Ticks.GetPrevious(timeOfDay), timestamp.Kind);

    /// <summary>
    /// Calculates the moment at <paramref name="timeOfWeek"/> that first occurs at or before <paramref name="timestamp"/>.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static DateTime GetPrevious(in this DateTime timestamp, in TimeOfWeek timeOfWeek)
      => new DateTime(timestamp.Ticks.GetPrevious(timeOfWeek), timestamp.Kind);

    /// <summary>
    /// Calculates the moment at <paramref name="timeOfDay"/> that first occurs at or after <paramref name="timestamp"/>.
    /// You will get unexpected results if <paramref name="timeOfDay"/> is less than zero or greater than 24 hours.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static DateTime GetNext(in this DateTime timestamp, in TimeSpan timeOfDay)
      => new DateTime(timestamp.Ticks.GetNext(timeOfDay), timestamp.Kind);

    /// <summary>
    /// Calculates the moment at <paramref name="timeOfWeek"/> that first occurs at or after <paramref name="timestamp"/>.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static DateTime GetNext(in this DateTime timestamp, in TimeOfWeek timeOfWeek)
      => new DateTime(timestamp.Ticks.GetNext(timeOfWeek), timestamp.Kind);
  }

  // Weekends and holidays
  public partial class DateTimes
  {
    /// <summary>
    /// Returns True if the given time's <see cref="DayOfWeek"/> property is a weekend day (Saturday or Sunday)
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsWeekend(in this DateTime target)
      => target.DayOfWeek == Saturday || target.DayOfWeek == Sunday;

    /// <summary>
    /// Advances the given time 24 hours at a time until its <see cref="DateTime.DayOfWeek"/> property is no longer Saturday or Sunday.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static DateTime SkipWeekendMovingForward(in this DateTime date)
      => date.DayOfWeek switch
      {
        Saturday => date.AddDays(2),
        Sunday => date.AddDays(1),
        _ => date,
      };

    /// <summary>
    /// Retards the given time 24 hours at a time until its <see cref="DateTime.DayOfWeek"/> property is no longer Saturday or Sunday.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static DateTime SkipWeekendMovingBackward(in this DateTime date)
      => date.DayOfWeek switch
      {
        Sunday => date.AddDays(-2),
        Saturday => date.AddDays(-2),
        _ => date,
      };

    /// <summary>
    /// Advances the given time 24 hours at a time until the datesToSkip no longer contain the result.
    /// Matching is exact. Skipping only happens if the <paramref name="datesToSkip"/> values and the <paramref name="date"/>
    /// contain the same <see cref="DateTime.TimeOfDay"/> value.
    /// If your intention is to skip exact dates, you would be best to make sure that the <paramref name="date"/> <see cref="DateTime.TimeOfDay"/>
    /// value is zero before calling this method.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static DateTime SkipTheseDatesMovingForward(this DateTime date, IEnumerable<DateTime> datesToSkip)
    {
      // yes, this could have been coded with linq or with a recursive method.
      // I did it this (more efficient) way to avoid allocations or stack dive.
      while (true)
      {
        foreach (var d in datesToSkip)
          if (d == date) goto next;
        return date;
next:
        date = date.AddDays(1);
      }
    }

    /// <summary>
    /// Retards the given time 24 hours at a time until the datesToSkip no longer contain the result.
    /// Matching is exact. Skipping only happens if the <paramref name="datesToSkip"/> values and the <paramref name="date"/>
    /// contain the same <see cref="DateTime.TimeOfDay"/> value.
    /// If your intention is to skip exact dates, you would be best to make sure that the <paramref name="date"/> <see cref="DateTime.TimeOfDay"/>
    /// value is zero before calling this method.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static DateTime SkipTheseDatesMovingBackward(this DateTime date, IEnumerable<DateTime> datesToSkip)
    {
      // yes, this could have been coded with linq or with a recursive method.
      // I did it this (more efficient) way to avoid allocations or stack dive.
      while (true)
      {
        foreach (var d in datesToSkip)
          if (d == date) goto next;
        return date;
next:
        date = date.AddDays(-1);
      }
    }

    /// <summary>
    /// Gets the closest date on or after the given <paramref name="date"/> that does not fall on the weekend or in the <paramref name="datesToSkip"/>.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static DateTime SkipWeekendAndTheseDatesMovingForward(this DateTime date, IEnumerable<DateTime> datesToSkip)
    {
      // yes, this could have been coded with linq or with a recursive method.
      // I did it this (more efficient) way to avoid allocations or stack dive.
      while (true)
      {
        if (date.IsWeekend()) goto next;
        foreach (var d in datesToSkip)
          if (d == date) goto next;
        return date;
next:
        date = date.AddDays(1);
      }
    }

    /// <summary>
    /// Gets the closest date on or before the given <paramref name="date"/> that does not fall on the weekend or in the <paramref name="datesToSkip"/>.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static DateTime SkipWeekendAndTheseDatesMovingBackward(this DateTime date, IEnumerable<DateTime> datesToSkip)
    {
      // yes, this could have been coded with linq or with a recursive method.
      // I did it this (more efficient) way to avoid allocations or stack dive.
      while (true)
      {
        if (date.IsWeekend()) goto next;
        foreach (var d in datesToSkip)
          if (d == date) goto next;
        return date;
next:
        date = date.AddDays(-1);
      }
    }
  }

  // Period of week
  public partial class DateTimes
  {
    /// <summary>
    /// Divides a week into periods defined by a length in minutes, and a starting offset that shifts the periods' start/end times.
    /// Then finds the start of a period that includes the given timestamp.
    /// Beginning of the first period is considered to be midnight, Sunday.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static DateTime ToPeriodOfWeekFloor(in this DateTime timestamp, in TimeSpan periodLength)
      => new DateTime(timestamp.Ticks.ToPeriodOfWeekFloor(periodLength.Ticks), timestamp.Kind);

    /// <summary>
    /// Divides a week into periods defined by a length in minutes, and a starting offset that shifts the periods' start/end times.
    /// Then finds the end of a period that includes the given timestamp.
    /// Beginning of the first period is considered to be midnight, Sunday.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static DateTime ToPeriodOfWeekCeiling(in this DateTime timestamp, in TimeSpan periodLength)
      => new DateTime(timestamp.Ticks.ToPeriodOfWeekCeiling(periodLength.Ticks), timestamp.Kind);

    /// <summary>
    /// Divides a week into periods defined by a length in minutes, and a starting offset that shifts the periods' start/end times.
    /// Then finds the start of a period that includes the given timestamp.
    /// Beginning of the first period is considered to be midnight, Sunday, PLUS <paramref name="periodOffset"/>.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static DateTime ToPeriodOfWeekFloor(in this DateTime timestamp, in TimeSpan periodLength, in TimeSpan periodOffset)
      => new DateTime(timestamp.Ticks.ToPeriodOfWeekFloor(periodLength.Ticks, periodOffset.Ticks));

    /// <summary>
    /// Divides a week into periods defined by a length in minutes, and a starting offset that shifts the periods' start/end times.
    /// Then finds the end of a period that includes the given timestamp.
    /// Beginning of the first period is considered to be midnight, Sunday, PLUS <paramref name="periodOffset"/>.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static DateTime ToPeriodOfWeekCeiling(in this DateTime timestamp, in TimeSpan periodLength, in TimeSpan periodOffset)
      => new DateTime(timestamp.Ticks.ToPeriodOfWeekCeiling(periodLength.Ticks, periodOffset.Ticks));

    /// <summary>
    /// Divides the week into periods of length <paramref name="periodLength"/>, and then calculates
    /// the zero-based index of the period at given time <paramref name="timestamp"/>. 
    /// Beginning of the first period is considered to be midnight, Sunday.
    /// Periods include the first tick (index == 0) and exclude the the first tick of the next period (index &lt; periodLength)
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int ToPeriodOfWeekIndex(in this DateTime timestamp, in TimeSpan periodLength)
      => timestamp.Ticks.ToPeriodOfWeekIndex(periodLength.Ticks);

    /// <summary>
    /// Divides the week into periods of length <paramref name="periodLength"/>, and then calculates
    /// the zero-based index of the period at given time <paramref name="timestamp"/>. 
    /// Beginning of the first period is considered to be midnight, Sunday, PLUS <paramref name="periodOffset"/>.
    /// Periods include the first tick (index == 0) and exclude the the first tick of the next period (index &lt; periodLength)
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int ToPeriodOfWeekIndex(in this DateTime timestamp, in TimeSpan periodLength, in TimeSpan periodOffset)
      => timestamp.Ticks.ToPeriodOfWeekIndex(periodLength.Ticks, periodOffset.Ticks);
  }

  // Fixed interval calculations
  public partial class DateTimes
  {
    /// <summary>
    /// Divides time from <paramref name="intervalStart"/> into evenly-sized periods (intervals) of length <paramref name="intervalLength"/>
    /// and returns the beginning of the interval in progress at <paramref name="timestamp"/>.
    /// If the <paramref name="timestamp"/> is at an exact interval start, it returns a copy of the same <paramref name="timestamp"/>
    /// </summary>
    /// <remarks>You will get unexpected results if <paramref name="timestamp"/> is less than <paramref name="intervalStart"/> or if <paramref name="intervalLength"/> is less than or equal to zero.</remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static DateTime ToFixedIntervalFloor(in this DateTime timestamp, in DateTime intervalStart, in TimeSpan intervalLength)
      => new DateTime(timestamp.Ticks.ToIntervalFloor(intervalStart.Ticks, intervalLength.Ticks), timestamp.Kind);

    /// <summary>
    /// Divides time from <paramref name="intervalStart"/> into evenly-sized periods (intervals) of length <paramref name="intervalLength"/>
    /// and returns the beginning of the next interval to start after <paramref name="timestamp"/>.
    /// If the <paramref name="timestamp"/> is at an exact interval start, it returns a copy of the same <paramref name="timestamp"/>
    /// </summary>
    /// <remarks>You will get unexpected results if <paramref name="timestamp"/> is less than <paramref name="intervalStart"/> or if <paramref name="intervalLength"/> is less than or equal to zero.</remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static DateTime ToFixedIntervalCeiling(in this DateTime timestamp, in DateTime intervalStart, in TimeSpan intervalLength)
      => new DateTime(timestamp.Ticks.ToIntervalCeiling(intervalStart.Ticks, intervalLength.Ticks), timestamp.Kind);

    /// <summary>
    /// Calculates the zero-based index of the interval in progress.
    /// </summary>
    /// <remarks>You will get unexpected results if <paramref name="timestamp"/> is less than <paramref name="intervalStart"/> or if <paramref name="intervalLength"/> is less than or equal to zero.</remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int ToFixedIntervalIndex(in this DateTime timestamp, in DateTime intervalStart, in TimeSpan intervalLength)
      => timestamp.Ticks.ToIntervalIndex(intervalStart.Ticks, intervalLength.Ticks);
  }

  // Unix time
  public partial class DateTimes
  {
    /// <summary>
    /// Gets the number of seconds since Midnight, 1 January 1970, 
    /// in the same timezone as the given <paramref name="timestamp"/>
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static long ToUnixSeconds(in this DateTime timestamp)
      => timestamp.Ticks.ToUnixSeconds();

    /// <summary>
    /// Gets the number of milliseconds since Midnight, 1 January 1970, UTC.
    /// in the same timezone as the given <paramref name="timestamp"/>
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static long ToUnixMillieconds(in this DateTime timestamp)
      => timestamp.Ticks.ToUnixMilliseconds();

    /// <summary>
    /// Creates a timestamp the given number of seconds after midnight, 1 January 1970, of the <paramref name="kind"/> specified.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static DateTime FromUnixSeconds(in long unixSeconds, DateTimeKind kind)
      => new DateTime(Ticks.FromUnixSeconds(unixSeconds), kind);

    /// <summary>
    /// Creates a timestamp the given number of milliseconds after midnight, 1 January 1970, of the <paramref name="kind"/> specified.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static DateTime FromUnixMilliseconds(in long unixMilliseconds, DateTimeKind kind)
      => new DateTime(Ticks.FromUnixMilliseconds(unixMilliseconds), kind);
  }

  // Assume kind
  public partial class DateTimes
  {
    /// <summary>
    /// Creates a DateTime object with the same value but with the <see cref="DateTime.Kind"/>
    /// property set to <paramref name="kind"/>. No timezone conversion is performed.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static DateTime AssumeKind(in this DateTime value, DateTimeKind kind)
      => DateTime.SpecifyKind(value, kind);

    /// <summary>
    /// Creates a DateTime object with the same value but with the <see cref="DateTime.Kind"/>
    /// property set to <see cref="DateTimeKind.Utc"/>. No timezone conversion is performed.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static DateTime AssumeUniversal(in this DateTime value)
      => DateTime.SpecifyKind(value, DateTimeKind.Utc);

    /// <summary>
    /// Creates a DateTime object with the same value but with the <see cref="DateTime.Kind"/>
    /// property set to <see cref="DateTimeKind.Local"/>. No timezone conversion is performed.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static DateTime AssumeLocal(in this DateTime value)
      => DateTime.SpecifyKind(value, DateTimeKind.Local);

    /// <summary>
    /// Creates a DateTime object with the same value but with the <see cref="DateTime.Kind"/>
    /// property set to <see cref="DateTimeKind.Unspecified"/>. No timezone conversion is performed.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static DateTime AssumeUnspecified(in this DateTime value)
      => DateTime.SpecifyKind(value, DateTimeKind.Unspecified);
  }
}
