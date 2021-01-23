// Copyright (c) True Goodwill. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace FFT.TimeStamps
{
  using System;
  using System.Runtime.CompilerServices;

  public partial struct TimeStamp
  {
    /// <summary>
    /// Creates a timestamp the given number of seconds after midnight, 1 January 1970 UTC.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static TimeStamp FromUnixSeconds(long unixSeconds)
      => UnixEpoch.AddSeconds(unixSeconds);

    /// <summary>
    /// Creates a timestamp the given number of seconds after midnight, 1 January 1970, in the specified timezone.
    /// Compute intensive. Do not use in a hot path.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static TimeStamp FromUnixSeconds(long unixSeconds, TimeZoneInfo timeZone)
    {
      var ticksTimeZone = Ticks.FromUnixSeconds(unixSeconds);
      return new TimeStamp(ticksTimeZone, timeZone);
    }

    /// <summary>
    /// Creates a timestamp the given number of milliseconds after midnight, 1 January 1970 UTC.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static TimeStamp FromUnixMilliseconds(long unixMilliseconds)
      => UnixEpoch.AddMilliseconds(unixMilliseconds);

    /// <summary>
    /// Creates a timestamp the given number of milliseconds after midnight, 1 January 1970, in the specified timezone.
    /// Compute intensive. Do not use in a hot path.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static TimeStamp FromUnixMilliseconds(long unixMilliseconds, TimeZoneInfo timeZone)
    {
      var ticksTimeZone = Ticks.FromUnixMilliseconds(unixMilliseconds);
      return new TimeStamp(ticksTimeZone, timeZone);
    }

    /// <summary>
    /// Gets the number of seconds since Midnight, 1 January 1970, UTC.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public long ToUnixSeconds()
      => TicksUtc.ToUnixSeconds();

    /// <summary>
    /// Gets the number of milliseconds since Midnight, 1 January 1970, UTC.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public long ToUnixMillieconds()
      => TicksUtc.ToUnixMilliseconds();

    /// <summary>
    /// Gets the number of seconds since Midnight, 1 January 1970, in the given <paramref name="timeZone"/>
    /// Compute intensive. Do not use in a hot path.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public long ToUnixSeconds(TimeZoneInfo timeZone)
      => AsTicks(timeZone).ToUnixSeconds();

    /// <summary>
    /// Gets the number of milliseconds since Midnight, 1 January 1970, in the given <paramref name="timeZone"/>
    /// Compute intensive. Do not use in a hot path.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public long ToUnixMilliseconds(TimeZoneInfo timeZone)
      => AsTicks(timeZone).ToUnixMilliseconds();
  }
}
