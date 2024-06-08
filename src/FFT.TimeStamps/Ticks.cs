// Copyright (c) True Goodwill. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Runtime.CompilerServices;

namespace FFT.TimeStamps;

/// <summary>
/// Contains methods for manipulating time when it is represented in ticks (ten-millionths of a second).
/// </summary>
public static partial class Ticks
{
  private const long TICKS_PER_WEEK = 7 * TimeSpan.TicksPerDay;

  /// <summary>
  /// Unix "zero" time represented in ticks.
  /// </summary>
  private static readonly long _unixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).Ticks;

  /// <summary>
  /// Calculates the ticks at the given moment in time.
  /// </summary>
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static long At(int year, int month, int day)
    => new DateTime(year, month, day, 0, 0, 0).Ticks;

  /// <summary>
  /// Calculates the ticks at the given moment in time.
  /// </summary>
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static long At(int year, int month, int day, int hour, int minute, int second)
    => new DateTime(year, month, day, hour, minute, second).Ticks;
}

// To
public partial class Ticks
{
  /// <summary>
  /// Calculates the total number of milliseconds represented by the given <paramref name="ticks"/>.
  /// </summary>
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static double ToMilliseconds(this long ticks)
    => ticks / (double)TimeSpan.TicksPerMillisecond;

  /// <summary>
  /// Calculates the total number of seconds represented by the given <paramref name="ticks"/>.
  /// </summary>
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static double ToSeconds(this long ticks)
    => ticks / (double)TimeSpan.TicksPerSecond;

  /// <summary>
  /// Calculates the total number of minutes represented by the given <paramref name="ticks"/>.
  /// </summary>
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static double ToMinutes(this long ticks)
    => ticks / (double)TimeSpan.TicksPerMinute;

  /// <summary>
  /// Calculates the total number of hours represented by the given <paramref name="ticks"/>.
  /// </summary>
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static double ToHours(this long ticks)
    => ticks / (double)TimeSpan.TicksPerHour;

  /// <summary>
  /// Calculates the total number of days represented by the given <paramref name="ticks"/>.
  /// </summary>
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static double ToDays(this long ticks)
    => ticks / (double)TimeSpan.TicksPerDay;

  /// <summary>
  /// Converts the given <paramref name="ticks"/> to an equivalent <see cref="TimeSpan"/>.
  /// </summary>
  public static TimeSpan ToTimeSpan(this long ticks)
    => new TimeSpan(ticks);

  /// <summary>
  /// Converts the given <paramref name="ticks"/> to an equivalent <see cref="TimeStamp"/>.
  /// </summary>
  public static TimeStamp ToTimeStamp(this long ticks)
    => new TimeStamp(ticks);
}

// Floor and Ceiling
public partial class Ticks
{
  /// <summary>
  /// Rounds the given value down to the beginning of the millisecond.
  /// </summary>
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static long ToMillisecondFloor(this long ticks)
    => ticks - (ticks % TimeSpan.TicksPerMillisecond);

  /// <summary>
  /// Rounds the given value up to the beginning of the next millisecond.
  /// If the given value is already at the beginning of the millisecond, a copy of the value is returned.
  /// </summary>
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static long ToMillisecondsCeiling(this long ticks)
  {
    var ticksPastMillisecond = ticks % TimeSpan.TicksPerMillisecond;
    return ticksPastMillisecond == 0 ? ticks : ticks - ticksPastMillisecond + TimeSpan.TicksPerMillisecond;
  }

  /// <summary>
  /// Rounds the given value down to the beginning of the second.
  /// </summary>
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static long ToSecondFloor(this long ticks)
    => ticks - (ticks % TimeSpan.TicksPerSecond);

  /// <summary>
  /// Rounds the given value up to the beginning of the next second.
  /// If the given value is already at the beginning of the second, a copy of the value is returned.
  /// </summary>
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static long ToSecondCeiling(this long ticks)
  {
    var ticksPastSecond = ticks % TimeSpan.TicksPerSecond;
    return ticksPastSecond == 0 ? ticks : ticks - ticksPastSecond + TimeSpan.TicksPerSecond;
  }

  /// <summary>
  /// Rounds the given value down to the beginning of the minute.
  /// </summary>
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static long ToMinuteFloor(this long ticks)
    => ticks - (ticks % TimeSpan.TicksPerMinute);

  /// <summary>
  /// Rounds the given value up to the beginning of the next minute.
  /// If the given value is already at the beginning of the minute, a copy of the value is returned.
  /// </summary>
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static long ToMinuteCeiling(this long ticks)
  {
    var ticksPastMinute = ticks % TimeSpan.TicksPerMinute;
    return ticksPastMinute == 0 ? ticks : ticks - ticksPastMinute + TimeSpan.TicksPerMinute;
  }

  /// <summary>
  /// Rounds the given value down to the beginning of the hour.
  /// </summary>
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static long ToHourFloor(this long ticks)
    => ticks - (ticks % TimeSpan.TicksPerHour);

  /// <summary>
  /// Rounds the given value up to the beginning of the next hour.
  /// If the given value is already at the beginning of the hour, a copy of the value is returned.
  /// </summary>
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static long ToHourCeiling(this long ticks)
  {
    var ticksPastHour = ticks % TimeSpan.TicksPerHour;
    return ticksPastHour == 0 ? ticks : ticks - ticksPastHour + TimeSpan.TicksPerHour;
  }

  /// <summary>
  /// Rounds the given value down to the beginning of the day.
  /// </summary>
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static long ToDayFloor(this long ticks)
    => ticks - (ticks % TimeSpan.TicksPerDay);

  /// <summary>
  /// Rounds the given value up to the beginning of the next day.
  /// If the given value is already at the beginning of the day, a copy of the value is returned.
  /// </summary>
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static long ToDayCeiling(this long ticks)
  {
    var ticksPastDay = ticks % TimeSpan.TicksPerDay;
    return ticksPastDay == 0 ? ticks : ticks - ticksPastDay + TimeSpan.TicksPerDay;
  }

  /// <summary>
  /// Rounds the given value down to the beginning of the week.
  /// Week floor is considered midnight, Sunday.
  /// </summary>
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static long ToWeekFloor(this long ticks)
    => ticks - ticks.TicksPastWeek();

  /// <summary>
  /// Rounds the given value up to the beginning of the next week.
  /// If the given value is already at the beginning of the week, a copy of the value is returned.
  /// Week ceiling is considered midnight, Sunday.
  /// </summary>
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static long ToWeekCeiling(this long ticks)
  {
    var ticksPastWeek = ticks.TicksPastWeek();
    return ticksPastWeek == 0 ? ticks : ticks - ticksPastWeek + TICKS_PER_WEEK;
  }
}

// Add
public partial class Ticks
{
  /// <summary>
  /// Adds the given number of <paramref name="milliseconds"/>.
  /// </summary>
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static long AddMilliseconds(this long ticks, double milliseconds)
      => ticks + (long)(milliseconds * TimeSpan.TicksPerMillisecond);

  /// <summary>
  /// Adds the given number of <paramref name="milliseconds"/>.
  /// </summary>
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static long AddMilliseconds(this long ticks, long milliseconds)
      => ticks + (milliseconds * TimeSpan.TicksPerMillisecond);

  /// <summary>
  /// Adds the given number of <paramref name="microseconds"/>.
  /// </summary>
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static long AddMicroseconds(this long ticks, double microseconds)
      => ticks + (long)(microseconds * TimeSpan.TicksPerMillisecond / 1000);

  /// <summary>
  /// Adds the given number of <paramref name="microseconds"/>.
  /// </summary>
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static long AddMicroseconds(this long ticks, long microseconds)
      => ticks + (microseconds * TimeSpan.TicksPerMillisecond / 1000);

  /// <summary>
  /// Adds the given number of <paramref name="seconds"/>.
  /// </summary>
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static long AddSeconds(this long ticks, double seconds)
      => ticks + (long)(seconds * TimeSpan.TicksPerSecond);

  /// <summary>
  /// Adds the given number of <paramref name="seconds"/>.
  /// </summary>
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static long AddSeconds(this long ticks, long seconds)
      => ticks + (seconds * TimeSpan.TicksPerSecond);

  /// <summary>
  /// Adds the given number of <paramref name="minutes"/>.
  /// </summary>
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static long AddMinutes(this long ticks, double minutes)
      => ticks + (long)(minutes * TimeSpan.TicksPerMinute);

  /// <summary>
  /// Adds the given number of <paramref name="minutes"/>.
  /// </summary>
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static long AddMinutes(this long ticks, long minutes)
      => ticks + (minutes * TimeSpan.TicksPerMinute);

  /// <summary>
  /// Adds the given number of <paramref name="hours"/>.
  /// </summary>
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static long AddHours(this long ticks, double hours)
      => ticks + (long)(hours * TimeSpan.TicksPerHour);

  /// <summary>
  /// Adds the given number of <paramref name="hours"/>.
  /// </summary>
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static long AddHours(this long ticks, long hours)
      => ticks + (hours * TimeSpan.TicksPerHour);

  /// <summary>
  /// Adds the given number of <paramref name="days"/>.
  /// </summary>
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static long AddDays(this long ticks, double days)
      => ticks + (long)(days * TimeSpan.TicksPerDay);

  /// <summary>
  /// Adds the given number of <paramref name="days"/>.
  /// </summary>
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static long AddDays(this long ticks, long days)
      => ticks + (days * TimeSpan.TicksPerDay);

  /// <summary>
  /// Adds the given number of <paramref name="weeks"/>.
  /// </summary>
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static long AddWeeks(this long ticks, double weeks)
      => ticks + (long)(weeks * TICKS_PER_WEEK);

  /// <summary>
  /// Adds the given number of <paramref name="weeks"/>.
  /// </summary>
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static long AddWeeks(this long ticks, long weeks)
      => ticks + (weeks * TICKS_PER_WEEK);
}

// ToIndex
public partial class Ticks
{
  /// <summary>
  /// Rounds the second down to the start of the second and then
  /// returns zero-based index of the second within the day.
  /// Values returned range from 0 to 1440 * 60 - 1.
  /// </summary>
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static int ToSecondOfDayIndex(this long ticks)
    => (int)(ticks.TicksPastDay() / TimeSpan.TicksPerSecond);

  /// <summary>
  /// Rounds the second down to the start of the second and then
  /// returns zero-based index of the second within the week.
  /// Values returned range from 0 to 7 * 1440 * 60 - 1.
  /// The zero point is midnight, Sunday.
  /// </summary>
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static int ToSecondOfWeekIndex(this long ticks)
    => (int)(ticks.TicksPastWeek() / TimeSpan.TicksPerSecond);

  /// <summary>
  /// Rounds the minute down to the start of the minute and then
  /// returns the zero-based index of the minute within the day.
  /// Values returned range from 0 to 1439
  /// The zero point is midnight, Sunday.
  /// </summary>
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static int ToMinuteOfDayIndex(this long ticks)
    => (int)(ticks.TicksPastDay() / TimeSpan.TicksPerMinute);

  /// <summary>
  /// Rounds the minute down to the start of the minute and then
  /// returns zero-based index of the minute within the week.
  /// Values returned range from 0 to 7 * 1440 - 1.
  /// The zero point is midnight, Sunday.
  /// </summary>
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static int ToMinuteOfWeekIndex(this long ticks)
    => (int)(ticks.TicksPastWeek() / TimeSpan.TicksPerMinute);
}

// GetPrevious and GetNext
public partial class Ticks
{
  /// <summary>
  /// Calculates and returns the moment at <paramref name="timeOfDay"/> that mostly recently occurs at or before <paramref name="ticks"/>.
  /// </summary>
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static long GetPrevious(this long ticks, TimeSpan timeOfDay)
  {
    if (timeOfDay.Ticks >= TimeSpan.TicksPerDay) throw new ArgumentException($"{nameof(timeOfDay)} must be less than 24 hours. Actual value: {timeOfDay}");
    var ticksPastDay = ticks.TicksPastDay();
    return ticksPastDay >= timeOfDay.Ticks
        ? ticks - ticksPastDay + timeOfDay.Ticks
        : ticks - ticksPastDay - TimeSpan.TicksPerDay + timeOfDay.Ticks;
  }

  /// <summary>
  /// Calculates and returns the moment at <paramref name="timeOfDay"/> that first occurs at or after <paramref name="ticks"/>.
  /// </summary>
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static long GetNext(this long ticks, TimeSpan timeOfDay)
  {
    if (timeOfDay.Ticks >= TimeSpan.TicksPerDay) throw new ArgumentException($"{nameof(timeOfDay)} must be less than 24 hours. Actual value: {timeOfDay}");
    var ticksPastDay = ticks.TicksPastDay();
    return ticksPastDay <= timeOfDay.Ticks
        ? ticks - ticksPastDay + timeOfDay.Ticks
        : ticks - ticksPastDay + TimeSpan.TicksPerDay + timeOfDay.Ticks;
  }

  /// <summary>
  /// Calculates and returns the moment at <paramref name="timeOfWeek"/> that mostly recently occurs at or before <paramref name="ticks"/>.
  /// </summary>
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static long GetPrevious(this long ticks, TimeOfWeek timeOfWeek)
  {
    var ticksPastWeek = ticks.TicksPastWeek();
    return ticksPastWeek >= timeOfWeek.TicksSinceWeekFloor
        ? ticks - ticksPastWeek + timeOfWeek.TicksSinceWeekFloor
        : ticks - ticksPastWeek - TICKS_PER_WEEK + timeOfWeek.TicksSinceWeekFloor;
  }

  /// <summary>
  /// Calculates and returns the moment at <paramref name="timeOfWeek"/> that first occurs at or after <paramref name="ticks"/>.
  /// </summary>
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static long GetNext(this long ticks, TimeOfWeek timeOfWeek)
  {
    var ticksPastWeek = ticks.TicksPastWeek();
    return ticksPastWeek <= timeOfWeek.TicksSinceWeekFloor
        ? ticks - ticksPastWeek + timeOfWeek.TicksSinceWeekFloor
        : ticks - ticksPastWeek + timeOfWeek.TicksSinceWeekFloor + TICKS_PER_WEEK;
  }
}

// PeriodOfWeek
public partial class Ticks
{
  /// <summary>
  /// Divides the week into periods of length <paramref name="periodLength"/>, and then calculates
  /// the beginning of the period at given time <paramref name="ticks"/>.
  /// Beginning of the first period is considered to be midnight, Sunday.
  /// Periods include the first tick (index == 0) and exclude the the first tick of the next period (index &lt; periodLengthInMinutes * TimeSpan.TicksPerMinute)
  /// If the timestamp is at an exact period start, it returns a copy of the same timestamp.
  /// </summary>
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static long ToPeriodOfWeekFloor(this long ticks, long periodLength)
  {
    var ticksPastBlock = ticks.TicksPastWeek() % periodLength;
    return ticks - ticksPastBlock;
  }

  /// <summary>
  /// Divides the week into periods of length <paramref name="periodLength"/>, and then calculates
  /// the beginning of the period at given time <paramref name="ticks"/>.
  /// Beginning of the first period is considered to be midnight, Sunday, PLUS <paramref name="periodOffset"/>.
  /// Periods include the first tick (index == 0) and exclude the the first tick of the next period (index &lt; periodLengthInMinutes * TimeSpan.TicksPerMinute)
  /// If the timestamp is at an exact period start, it returns a copy of the same timestamp.
  /// </summary>
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static long ToPeriodOfWeekFloor(this long ticks, long periodLength, long periodOffset)
    => (ticks - periodOffset).ToPeriodOfWeekFloor(periodLength) + periodOffset;

  /// <summary>
  /// Divides the week into periods of length <paramref name="periodLength"/>, and then calculates
  /// the beginning of the period after the given time <paramref name="ticks"/>.
  /// Beginning of the first period is considered to be midnight, Sunday.
  /// Periods include the first tick (index == 0) and exclude the the first tick of the next period (index &lt; periodLengthInMinutes * TimeSpan.TicksPerMinute)
  /// If the timestamp is at an exact period start, it returns a copy of the same timestamp.
  /// </summary>
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static long ToPeriodOfWeekCeiling(this long ticks, long periodLength)
  {
    var ticksPastBlock = ticks.TicksPastWeek() % periodLength;
    return ticksPastBlock == 0 ? ticks : ticks - ticksPastBlock + periodLength;
  }

  /// <summary>
  /// Divides the week into periods of length <paramref name="periodLength"/>, and then calculates
  /// the beginning of the period after the given time <paramref name="ticks"/>.
  /// Beginning of the first period is considered to be midnight, Sunday, PLUS <paramref name="periodOffset"/>.
  /// Periods include the first tick (index == 0) and exclude the the first tick of the next period (index &lt; periodLengthInMinutes * TimeSpan.TicksPerMinute)
  /// If the timestamp is at an exact period start, it returns a copy of the same timestamp.
  /// </summary>
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static long ToPeriodOfWeekCeiling(this long ticks, long periodLength, long periodOffset)
    => (ticks - periodOffset).ToPeriodOfWeekCeiling(periodLength) + periodOffset;

  /// <summary>
  /// Divides the week into periods of length <paramref name="periodLength"/>, and then calculates
  /// the zero-based index of the period at given time <paramref name="ticks"/>.
  /// Beginning of the first period is considered to be midnight, Sunday.
  /// Periods include the first tick (index == 0) and exclude the the first tick of the next period (index &lt; periodLength).
  /// </summary>
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static int ToPeriodOfWeekIndex(this long ticks, long periodLength)
    => (int)(ticks.TicksPastWeek() / periodLength);

  /// <summary>
  /// Divides the week into periods of length <paramref name="periodLength"/>, and then calculates
  /// the zero-based index of the period at given time <paramref name="ticks"/>.
  /// Beginning of the first period is considered to be midnight, Sunday, PLUS <paramref name="periodOffset"/>.
  /// Periods include the first tick (index == 0) and exclude the the first tick of the next period (index &lt; periodLength).
  /// </summary>
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static int ToPeriodOfWeekIndex(this long ticks, long periodLength, long periodOffset)
    => (ticks - periodOffset).ToPeriodOfWeekIndex(periodLength);
}

// Fixed interval
public partial class Ticks
{
  /// <summary>
  /// Divides time from <paramref name="intervalStart"/> into evenly-sized periods (intervals) of length <paramref name="intervalLength"/>
  /// and returns the beginning of the interval in progress at <paramref name="ticks"/>.
  /// If the <paramref name="ticks"/> is at an exact interval start, it returns a copy of the same <paramref name="ticks"/>.
  /// </summary>
  /// <remarks>You will get unexpected results if <paramref name="ticks"/> is less than <paramref name="intervalStart"/> or if <paramref name="intervalLength"/> is less than or equal to zero.</remarks>
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static long ToIntervalFloor(this long ticks, long intervalStart, long intervalLength)
  {
    var ticksPastInterval = (ticks - intervalStart) % intervalLength;
    return ticks - ticksPastInterval;
  }

  /// <summary>
  /// Divides time from <paramref name="intervalStart"/> into evenly-sized periods (intervals) of length <paramref name="intervalLength"/>
  /// and returns the beginning of the next interval to start after <paramref name="ticks"/>.
  /// If the <paramref name="ticks"/> is at an exact interval start, it returns a copy of the same <paramref name="ticks"/>.
  /// </summary>
  /// <remarks>You will get unexpected results if <paramref name="ticks"/> is less than <paramref name="intervalStart"/> or if <paramref name="intervalLength"/> is less than or equal to zero.</remarks>
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static long ToIntervalCeiling(this long ticks, long intervalStart, long intervalLength)
  {
    var ticksPastInterval = (ticks - intervalStart) % intervalLength;
    return ticksPastInterval == 0 ? ticks : ticks - ticksPastInterval + intervalLength;
  }

  /// <summary>
  /// Divides time from <paramref name="intervalStart"/> into evenly-sized periods (intervals) of length <paramref name="intervalLength"/>
  /// Calculates the zero-based index of the interval in progress.
  /// </summary>
  /// <remarks>You will get unexpected results if <paramref name="ticks"/> is less than <paramref name="intervalStart"/> or if <paramref name="intervalLength"/> is less than or equal to zero.</remarks>
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static int ToIntervalIndex(this long ticks, long intervalStart, long intervalLength)
      => (int)((ticks - intervalStart) / intervalLength);
}

// Miscellaneous
public partial class Ticks
{
  /// <summary>
  /// Gets the <see cref="DayOfWeek"/> for the given <paramref name="ticks"/>.
  /// </summary>
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static DayOfWeek ToDayOfWeek(this long ticks)
    => (DayOfWeek)(ticks.TicksPastWeek() / TimeSpan.TicksPerDay);

  /// <summary>
  /// Gets the <see cref="TimeSpan"/> time of day for the given <paramref name="ticks"/>.
  /// </summary>
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static TimeSpan ToTimeOfDay(this long ticks)
    => new TimeSpan(ticks.TicksPastDay());

  /// <summary>
  /// Gets the <see cref="TimeOfWeek"/> for the given <paramref name="ticks"/>.
  /// </summary>
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static TimeOfWeek ToTimeOfWeek(this long ticks)
    => new TimeOfWeek(ticks.TicksPastWeek());

  /// <summary>
  /// Gets the number of ticks that have elapsed since the beginning of the day.
  /// Beginning of the day is considered to be midnight.
  /// </summary>
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static long TicksPastDay(this long ticks)
    => ticks % TimeSpan.TicksPerDay;

  /// <summary>
  /// Gets the number of ticks that have elapsed since the beginning of the week.
  /// Beginning of the week is considered to be midnight, Sunday.
  /// </summary>
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static long TicksPastWeek(this long ticks)
    // Add TimeSpan.TicksPerDay first because the datetime at 0 ticks is midnight, Monday.
    => (ticks + TimeSpan.TicksPerDay) % TICKS_PER_WEEK;

  /// <summary>
  /// Gets the number of seconds since Midnight, 1 January 1970, UTC.
  /// </summary>
  /// <exception cref="ArgumentException">Thrown if <paramref name="ticks"/> is not exactly a whole second.</exception>
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static long ToUnixSeconds(this long ticks)
  {
    if (ticks % TimeSpan.TicksPerSecond != 0)
      throw new ArgumentException("Can only convert exact seconds to unix timestamp.", nameof(ticks));
    return (ticks - _unixEpoch) / TimeSpan.TicksPerSecond;
  }

  /// <summary>
  /// Gets the number of milliseconds since Midnight, 1 January 1970, UTC.
  /// </summary>
  /// <exception cref="ArgumentException">Thrown if <paramref name="ticks"/> is not exactly a whole millisecond.</exception>
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static long ToUnixMilliseconds(this long ticks)
  {
    if (ticks % TimeSpan.TicksPerMillisecond != 0)
      throw new ArgumentException("Can only convert exact milliseconds to unix timestamp.", nameof(ticks));
    return (ticks - _unixEpoch) / TimeSpan.TicksPerMillisecond;
  }

  /// <summary>
  /// Gets the number of milliseconds since Midnight, 1 January 1970, UTC.
  /// </summary>
  /// <exception cref="ArgumentException">Thrown if <paramref name="ticks"/> is not exactly a whole millisecond.</exception>
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static long ToUnixMicroseconds(this long ticks)
  {
    if (ticks % (TimeSpan.TicksPerMillisecond / 1000) != 0)
      throw new ArgumentException("Can only convert exact microseconds to unix timestamp.", nameof(ticks));
    return (ticks - _unixEpoch) / (TimeSpan.TicksPerMillisecond / 1000);
  }

  /// <summary>
  /// Converts the given unix time to ticks.
  /// </summary>
  /// <param name="unixSeconds">The time since midnight, 1 Jan 1970, in seconds.</param>
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static long FromUnixSeconds(this long unixSeconds)
    => _unixEpoch + (unixSeconds * TimeSpan.TicksPerSecond);

  /// <summary>
  /// Converts the given unix time to ticks.
  /// </summary>
  /// <param name="unixMilliseconds">The time since midnight, 1 Jan 1970, in milliseconds.</param>
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static long FromUnixMilliseconds(this long unixMilliseconds)
    => _unixEpoch + (unixMilliseconds * TimeSpan.TicksPerMillisecond);

  /// <summary>
  /// Converts the given unix time to ticks.
  /// </summary>
  /// <param name="unixMicroseconds">The time since midnight, 1 Jan 1970, in microseconds.</param>
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static long FromUnixMicroseconds(this long unixMicroseconds)
    => _unixEpoch + (unixMicroseconds * TimeSpan.TicksPerMillisecond / 1000);
}
