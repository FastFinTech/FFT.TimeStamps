// Copyright (c) True Goodwill. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace FFT.TimeStamps
{
  using System;
  using System.Runtime.CompilerServices;

  public partial struct TimeStamp
  {
    /// <summary>
    /// Calculates the moment at <paramref name="timeOfDay"/> in UTC timezone that occurs at or just before the given timestamp.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public TimeStamp GetPrevious(in TimeSpan timeOfDay)
      => new TimeStamp(TicksUtc.GetPrevious(timeOfDay));

    /// <summary>
    /// Calculates the moment at <paramref name="timeOfDay"/> in UTC timezone that occurs at or just after the given timestamp.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public TimeStamp GetNext(in TimeSpan timeOfDay)
      => new TimeStamp(TicksUtc.GetNext(timeOfDay));

    /// <summary>
    /// Calculates the moment at <paramref name="timeOfDay"/> in <paramref name="timeZone"/> that occurs at or just before the given timestamp.
    /// Compute intensive. Do not use in a hot path.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public TimeStamp GetPrevious(in TimeSpan timeOfDay, TimeZoneInfo timeZone)
      => new TimeStamp(AsTicks(timeZone).GetPrevious(timeOfDay), timeZone);

    /// <summary>
    /// Calculates the moment at <paramref name="timeOfDay"/> in <paramref name="timeZone"/> that occurs at or just after the given timestamp.
    /// Compute intensive. Do not use in a hot path.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public TimeStamp GetNext(in TimeSpan timeOfDay, TimeZoneInfo timeZone)
      => new TimeStamp(AsTicks(timeZone).GetNext(timeOfDay), timeZone);

    /// <summary>
    /// Calculates the moment at <paramref name="timeOfWeek"/> in UTC timezone that occurs at or just before the given timestamp.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public TimeStamp GetPrevious(in TimeOfWeek timeOfWeek)
      => new TimeStamp(TicksUtc.GetPrevious(timeOfWeek));

    /// <summary>
    /// Calculates the moment at <paramref name="timeOfWeek"/> in UTC timezone that occurs at or just after the given timestamp.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public TimeStamp GetNext(in TimeOfWeek timeOfWeek)
      => new TimeStamp(TicksUtc.GetNext(timeOfWeek));

    /// <summary>
    /// Calculates the moment at <paramref name="timeOfWeek"/> in <paramref name="timeZone"/> that occurs at or just before the given timestamp.
    /// Compute intensive. Do not use in a hot path.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public TimeStamp GetPrevious(in TimeOfWeek timeOfWeek, TimeZoneInfo timeZone)
      => new TimeStamp(AsTicks(timeZone).GetPrevious(timeOfWeek), timeZone);

    /// <summary>
    /// Calculates the moment at <paramref name="timeOfWeek"/> in <paramref name="timeZone"/> that occurs at or just after the given timestamp.
    /// Compute intensive. Do not use in a hot path.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public TimeStamp GetNext(in TimeOfWeek timeOfWeek, TimeZoneInfo timeZone)
      => new TimeStamp(AsTicks(timeZone).GetNext(timeOfWeek), timeZone);
  }
}
