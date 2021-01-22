// Copyright (c) True Goodwill. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace FFT.TimeStamps
{
  using System;
  using System.Runtime.CompilerServices;

  public partial struct TimeStamp
  {
    /// <summary>
    /// Gets the time of day in the utc timezone.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public TimeSpan ToTimeOfDay()
      => TicksUtc.ToTimeOfDay();

    /// <summary>
    /// Gets the time of day in the given <paramref name="timeZone"/>.
    /// Compute intensive. Do not use in a hot path.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public TimeSpan ToTimeOfDay(TimeZoneInfo timeZone)
      => AsTicks(timeZone).ToTimeOfDay();
  }
}
