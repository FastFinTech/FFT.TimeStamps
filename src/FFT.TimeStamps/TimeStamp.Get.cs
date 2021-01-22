// Copyright (c) True Goodwill. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace FFT.TimeStamps
{
  using System;
  using System.Runtime.CompilerServices;

  public partial struct TimeStamp
  {
    /// <summary>
    /// Gets the date of the current moment in the utc timezone.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public DateStamp GetDate()
      => new DateStamp(new DateTime(TicksUtc.ToDayFloor(), DateTimeKind.Utc));

    /// <summary>
    /// Gets the date of the current moment in the given <paramref name="timeZone"/>.
    /// Compute intensive. Do not use in a hot path.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public DateStamp GetDate(TimeZoneInfo timeZone)
      => new DateStamp(new DateTime(AsTicks(timeZone).ToDayFloor(), DateTimeKind.Utc));

    /// <summary>
    /// Gets the month of the current moment in the utc timezone.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public MonthStamp GetMonth()
      => new MonthStamp(new DateTime(TicksUtc, DateTimeKind.Utc).ToMonthFloor());

    /// <summary>
    /// Gets the month of the current moment in the given <paramref name="timeZone"/>
    /// Compute intensive. Do not use in a hot path.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public MonthStamp GetMonth(TimeZoneInfo timeZone)
      => new MonthStamp(new DateTime(AsTicks(timeZone), DateTimeKind.Utc).ToMonthFloor());

    /// <summary>
    /// Gets the time of day of the current moment in the utc timezone.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public TimeSpan GetTimeOfDay()
      => new TimeSpan(TicksUtc.TicksPastDay());

    /// <summary>
    /// Gets the time of day of the current moment in the given <paramref name="timeZone"/>.
    /// Compute intensive. Do not use in a hot path.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public TimeSpan GetTimeOfDay(TimeZoneInfo timeZone)
      => new TimeSpan(AsTicks(timeZone).TicksPastDay());

    /// <summary>
    /// Gets the time of week of the current moment in the utc timezone.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public TimeOfWeek GetTimeOfWeek()
      => new TimeOfWeek(TicksUtc.TicksPastWeek());

    /// <summary>
    /// Gets the time of week of the current moment for a clock in the given <paramref name="timeZone"/>.
    /// Compute intensive. Do not use in a hot path.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public TimeOfWeek GetTimeOfWeek(TimeZoneInfo timeZone)
      => new TimeOfWeek(AsTicks(timeZone).TicksPastWeek());
  }
}
