// Copyright (c) True Goodwill. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace FFT.TimeStamps
{
  using System;
  using System.Diagnostics;
  using System.Runtime.CompilerServices;
  using System.Runtime.InteropServices;
  using System.Text.Json.Serialization;

  /// <summary>
  /// Use this to get extremely fast timestamping when:
  /// 1. Your primary purpose is fast, efficient storage of exact times, across multiple timezones.
  /// 2. You run very frequent comparisons of timestamps, possibly as a way of comparing across multiple timezones.
  /// 2. You DON'T often need to extract string representations, or get the day/month/year/hour/minute/second properties (compute intensive).
  /// </summary>
  [DebuggerTypeProxy(typeof(DebuggerView))]
  [JsonConverter(typeof(TimeStampJsonConverter))]
  public readonly partial struct TimeStamp : IComparable<TimeStamp>, IEquatable<TimeStamp>
  {
    /// <summary>
    /// Contains the minimum possible value of a <see cref="TimeStamp"/>.
    /// </summary>
    public static readonly TimeStamp MinValue = new TimeStamp(0); // also the equivalent of default(TimeStamp), because of the 0

    /// <summary>
    /// Contains the maximum possible value of a <see cref="TimeStamp"/>.
    /// </summary>
    public static readonly TimeStamp MaxValue = new TimeStamp(long.MaxValue);

    /// <summary>
    /// Contains the timestamp at 0 unix time, Midnight, 1 January 1970.
    /// </summary>
    public static readonly TimeStamp UnixEpoch = new TimeStamp(new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero));

    /// <summary>
    /// The number of ticks in UTC timezone.
    /// </summary>
    public readonly long TicksUtc;

    /// <summary>
    /// Expresses the time to its full 1-tick (ten-millionth of a second) resolution and the timezone offset as well.
    /// </summary>
    private const string DEFAULT_FORMAT_STRING = "yyyy-MM-dd HH:mm:ss.fffffff zzz";

    static TimeStamp()
    {
      SetupGetNowTicks();
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="TimeStamp"/> struct with the given number of ticks UTC.
    /// </summary>
    public TimeStamp(long ticksUtc)
      => TicksUtc = ticksUtc;

    /// <summary>
    /// Initializes a new instance of the <see cref="TimeStamp"/> struct from the ticks in the given TimeZone.
    /// Ambiguous times are considered to be in the standard (not daylight savings) offset.
    /// Invalid times are considered to be in the standard (not daylight savings) offset.
    /// Compute intensive. Don't use it in a hot path.
    /// </summary>
    public TimeStamp(long ticksTimeZone, TimeZoneInfo timeZone)
      => TicksUtc = ticksTimeZone - TimeZoneCalculator.Get(timeZone).GetSegment(ticksTimeZone, TimeKind.TimeZone).OffsetTicks;

    /// <summary>
    /// Initializes a new instance of the <see cref="TimeStamp"/> struct from the given <see cref="DateTimeOffset"/>.
    /// </summary>
    public TimeStamp(DateTimeOffset at)
      => TicksUtc = at.Ticks - at.Offset.Ticks;

    /// <summary>
    /// Initializes a new instance of the <see cref="TimeStamp"/> struct
    /// at the <paramref name="date"/> and <paramref name="timeOfDay"/> in the given <paramref name="timeZone"/>.
    /// Ambiguous times are considered to be in the standard (not daylight savings) offset.
    /// Invalid times are considered to be in the standard (not daylight savings) offset.
    /// Compute intensive. Don't use it in a hot path.
    /// </summary>
    public TimeStamp(DateStamp date, TimeSpan timeOfDay, TimeZoneInfo timeZone)
    {
      var ticksTimeZone = date.DateTime.Ticks + timeOfDay.Ticks;
      TicksUtc = ticksTimeZone - TimeZoneCalculator.Get(timeZone).GetSegment(ticksTimeZone, TimeKind.TimeZone).OffsetTicks;
    }

    /// <summary>
    /// Creates a string representation of the utc time using the default format.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override string ToString()
      => AsUtc().ToString(DEFAULT_FORMAT_STRING);

    /// <summary>
    /// Creates a string representation of the utc time using the given <paramref name="format"/>.
    /// </summary>
    /// <param name="format">The <see cref="DateTimeOffset"/> format string.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public string ToString(string format)
      => AsUtc().ToString(format);

    /// <summary>
    /// Creates a string representation of the local time using the given <paramref name="format"/>.
    /// Compute intensive. Don't use it in a hot path.
    /// </summary>
    /// <param name="format">The <see cref="DateTimeOffset"/> format string.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public string ToLocalString(string format)
      => AsLocal().ToString(format);

    /// <summary>
    /// Creates a string representation of the <paramref name="timeZone"/> time with default formatting.
    /// Compute intensive. Don't use it in a hot path.
    /// </summary>
    /// <param name="timeZone">The time zone to be used for expressing this value as a time string.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public string ToString(TimeZoneInfo timeZone)
      => ToString(timeZone, DEFAULT_FORMAT_STRING);

    /// <summary>
    /// Creates a string representation of the <paramref name="timeZone"/> time using the given <paramref name="format"/>.
    /// Compute intensive. Don't use it in a hot path.
    /// </summary>
    /// <param name="timeZone">The time zone to be used for expressing this value as a time string.</param>
    /// <param name="format">The <see cref="DateTimeOffset"/> format string.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public string ToString(TimeZoneInfo timeZone, string format)
      => As(timeZone).ToString(format);

    /// <inheritdoc/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int CompareTo(TimeStamp other)
      => TicksUtc.CompareTo(other.TicksUtc);

    /// <inheritdoc/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override bool Equals(object obj)
      => obj is TimeStamp other && Equals(other);

    /// <inheritdoc/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool Equals(TimeStamp other)
      => TicksUtc == other.TicksUtc;

    /// <inheritdoc/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override int GetHashCode()
      => TicksUtc.GetHashCode();
  }
}
