// Copyright (c) True Goodwill. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace FFT.TimeStamps
{
  using System;
  using System.Runtime.CompilerServices;

  public partial struct TimeStamp
  {
    /// <summary>
    /// Returns zero-based index of the current second within the day in the utc timezone.
    /// Values returned range from 0 to 1440 * 60 - 1.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int ToSecondOfDayIndex()
      => TicksUtc.ToSecondOfDayIndex();

    /// <summary>
    /// Returns zero-based index of the current second within the week in the utc timezone.
    /// Values returned range from 0 to 7 * 1440 * 60 - 1.
    /// The zero point is midnight, Sunday.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int ToSecondOfWeekIndex()
      => TicksUtc.ToSecondOfWeekIndex();

    /// <summary>
    /// Returns zero-based index of the current second within the day in the given <paramref name="timeZone"/>.
    /// Values returned range from 0 to 1440 * 60 - 1
    /// Compute intensive. Do not use in hot path.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int ToSecondOfDayIndex(TimeZoneInfo timeZone)
      => AsTicks(timeZone).ToSecondOfDayIndex();

    /// <summary>
    /// Returns zero-based index of the current second within the week in the given <paramref name="timeZone"/>.
    /// Values returned range from 0 to 7 * 1440 * 60 - 1.
    /// The zero point is midnight, Sunday.
    /// Compute intensive. Do not use in hot path.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int ToSecondOfWeekIndex(TimeZoneInfo timeZone)
      => AsTicks(timeZone).ToSecondOfWeekIndex();

    /// <summary>
    /// Returns zero-based index of the current minute within the day in the utc timezone.
    /// Values returned range from 0 to 1440 - 1.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int ToMinuteOfDayIndex()
      => TicksUtc.ToMinuteOfDayIndex();

    /// <summary>
    /// Returns zero-based index of the current minute within the day in the given <paramref name="timeZone"/>.
    /// Values returned range from 0 to 1440 - 1.
    /// Compute intensive. Do not use in hot path.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int ToMinuteOfDayIndex(TimeZoneInfo timeZone)
      => AsTicks(timeZone).ToMinuteOfDayIndex();

    /// <summary>
    /// Returns zero-based index of the current minute within the week in the utc timezone.
    /// Values returned range from 0 to 7 * 1440 - 1.
    /// The zero point is midnight, Sunday.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int ToMinuteOfWeekIndex()
      => TicksUtc.ToMinuteOfWeekIndex();

    /// <summary>
    /// Returns zero-based index of the current minute within the week in the given <paramref name="timeZone"/>.
    /// Values returned range from 0 to 7 * 1440 - 1.
    /// The zero point is midnight, Sunday.
    /// Compute intensive. Do not use in hot path.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int ToMinuteOfWeekIndex(TimeZoneInfo timeZone)
      => AsTicks(timeZone).ToMinuteOfWeekIndex();
  }
}
