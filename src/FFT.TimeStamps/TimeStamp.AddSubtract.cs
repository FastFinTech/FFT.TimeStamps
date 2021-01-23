// Copyright (c) True Goodwill. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace FFT.TimeStamps
{
  using System;
  using System.Runtime.CompilerServices;

  public partial struct TimeStamp
  {
    /// <summary>
    /// Creates a new <see cref="TimeStamp"/> by adding the given number of ticks to the instance.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public TimeStamp AddTicks(long ticks)
      => new TimeStamp(TicksUtc + ticks);

    /// <summary>
    /// Returns a <see cref="TimeStamp"/> advanced by the given number of <paramref name="ticks"/> in the given <paramref name="timeZone"/>.
    /// Compute intensive. Don't use in a hot path.
    /// </summary>
    /// <param name="ticks">The number of ticks of TIMEZONE time to advance the <see cref="TimeStamp"/>.</param>
    /// <param name="timeZone">The timezone of the clock being adjusted.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public TimeStamp AddTicks(long ticks, TimeZoneInfo timeZone)
      => new TimeStamp(AsTicks(timeZone) + ticks, timeZone);

    /// <summary>
    /// Returns a <see cref="TimeStamp"/> advanced by the given number of <paramref name="days"/>.
    /// </summary>
    /// <param name="days">The number of days of ABSOLUTE time to advance the <see cref="TimeStamp"/>.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public TimeStamp AddDays(double days)
      => new TimeStamp(TicksUtc.AddDays(days));

    /// <summary>
    /// Returns a <see cref="TimeStamp"/> advanced by the given number of <paramref name="days"/> in the given <paramref name="timeZone"/>.
    /// Compute intensive. Don't use in a hot path.
    /// </summary>
    /// <param name="days">The number of days of TIMEZONE time to advance the <see cref="TimeStamp"/>.</param>
    /// <param name="timeZone">The timezone of the clock being adjusted.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public TimeStamp AddDays(double days, TimeZoneInfo timeZone)
      => new TimeStamp(AsTicks(timeZone).AddDays(days), timeZone);

    /// <summary>
    /// Returns a <see cref="TimeStamp"/> advanced by the given number of <paramref name="hours"/>.
    /// </summary>
    /// <param name="hours">The number of hours of ABSOLUTE time to advance the <see cref="TimeStamp"/>.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public TimeStamp AddHours(double hours)
      => new TimeStamp(TicksUtc.AddHours(hours));

    /// <summary>
    /// Returns a <see cref="TimeStamp"/> advanced by the given number of <paramref name="hours"/> in the given <paramref name="timeZone"/>.
    /// Compute intensive. Don't use in a hot path.
    /// </summary>
    /// <param name="hours">The number of hours of TIMEZONE time to advance the <see cref="TimeStamp"/>.</param>
    /// <param name="timeZone">The timezone of the clock being adjusted.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public TimeStamp AddHours(double hours, TimeZoneInfo timeZone)
      => new TimeStamp(AsTicks(timeZone).AddHours(hours), timeZone);

    /// <summary>
    /// Returns a <see cref="TimeStamp"/> advanced by the given number of <paramref name="minutes"/>.
    /// </summary>
    /// <param name="minutes">The number of minutes of ABSOLUTE time to advance the <see cref="TimeStamp"/>.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public TimeStamp AddMinutes(double minutes)
      => new TimeStamp(TicksUtc.AddMinutes(minutes));

    /// <summary>
    /// Returns a <see cref="TimeStamp"/> advanced by the given number of <paramref name="minutes"/> in the given <paramref name="timeZone"/>.
    /// Compute intensive. Don't use in a hot path.
    /// </summary>
    /// <param name="minutes">The number of minutes of TIMEZONE time to advance the <see cref="TimeStamp"/>.</param>
    /// <param name="timeZone">The timezone of the clock being adjusted.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public TimeStamp AddMinutes(double minutes, TimeZoneInfo timeZone)
      => new TimeStamp(AsTicks(timeZone).AddMinutes(minutes), timeZone);

    /// <summary>
    /// Returns a <see cref="TimeStamp"/> advanced by the given number of <paramref name="seconds"/>.
    /// </summary>
    /// <param name="seconds">The number of seconds of ABSOLUTE time to advance the <see cref="TimeStamp"/>.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public TimeStamp AddSeconds(double seconds)
      => new TimeStamp(TicksUtc.AddSeconds(seconds));

    /// <summary>
    /// Returns a <see cref="TimeStamp"/> advanced by the given number of <paramref name="seconds"/> in the given <paramref name="timeZone"/>.
    /// Compute intensive. Don't use in a hot path.
    /// </summary>
    /// <param name="seconds">The number of seconds of TIMEZONE time to advance the <see cref="TimeStamp"/>.</param>
    /// <param name="timeZone">The timezone of the clock being adjusted.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public TimeStamp AddSeconds(double seconds, TimeZoneInfo timeZone)
      => new TimeStamp(AsTicks(timeZone).AddSeconds(seconds), timeZone);

    /// <summary>
    /// Returns a <see cref="TimeStamp"/> advanced by the given number of <paramref name="milliseconds"/>.
    /// </summary>
    /// <param name="milliseconds">The number of milliseconds of ABSOLUTE time to advance the <see cref="TimeStamp"/>.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public TimeStamp AddMilliseconds(double milliseconds)
      => new TimeStamp(TicksUtc.AddMilliseconds(milliseconds));

    /// <summary>
    /// Returns a <see cref="TimeStamp"/> advanced by the given number of <paramref name="milliseconds"/> in the given <paramref name="timeZone"/>.
    /// Compute intensive. Don't use in a hot path.
    /// </summary>
    /// <param name="milliseconds">The number of milliseconds of TIMEZONE time to advance the <see cref="TimeStamp"/>.</param>
    /// <param name="timeZone">The timezone of the clock being adjusted.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public TimeStamp AddMilliseconds(double milliseconds, TimeZoneInfo timeZone)
      => new TimeStamp(AsTicks(timeZone).AddMilliseconds(milliseconds), timeZone);

    /// <summary>
    /// Returns a <see cref="TimeStamp"/> advanced by the given <paramref name="timeSpan"/>.
    /// </summary>
    /// <param name="timeSpan">The amount of ABSOLUTE time to advance the <see cref="TimeStamp"/>.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public TimeStamp Add(TimeSpan timeSpan)
      => new TimeStamp(TicksUtc + timeSpan.Ticks);

    /// <summary>
    /// Returns a <see cref="TimeStamp"/> advanced by the given <paramref name="timeSpan"/> in the given <paramref name="timeZone"/>.
    /// Compute intensive. Don't use in a hot path.
    /// </summary>
    /// <param name="timeSpan">The amount of TIMEZONE time to advance the <see cref="TimeStamp"/>.</param>
    /// <param name="timeZone">The timezone of the clock being adjusted.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public TimeStamp Add(TimeSpan timeSpan, TimeZoneInfo timeZone)
      => new TimeStamp(AsTicks(timeZone) + timeSpan.Ticks, timeZone);

    /// <summary>
    /// Returns a <see cref="TimeStamp"/> retarded by the given <paramref name="timeSpan"/>.
    /// </summary>
    /// <param name="timeSpan">The amount of ABSOLUTE time to retard the <see cref="TimeStamp"/>.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public TimeStamp Subtract(TimeSpan timeSpan)
      => new TimeStamp(TicksUtc - timeSpan.Ticks);

    /// <summary>
    /// Returns a <see cref="TimeStamp"/> retarded by the given <paramref name="timeSpan"/> in the given <paramref name="timeZone"/>.
    /// Compute intensive. Don't use in a hot path.
    /// </summary>
    /// <param name="timeSpan">The amount of TIMEZONE time to retard the <see cref="TimeStamp"/>.</param>
    /// <param name="timeZone">The timezone of the clock being adjusted.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public TimeStamp Subtract(TimeSpan timeSpan, TimeZoneInfo timeZone)
      => new TimeStamp(AsTicks(timeZone) - timeSpan.Ticks, timeZone);

    /// <summary>
    /// Calculates the time difference between this time and the given <paramref name="timeStamp"/>.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public TimeSpan Subtract(TimeStamp timeStamp)
      => new TimeSpan(TicksUtc - timeStamp.TicksUtc);
  }
}
