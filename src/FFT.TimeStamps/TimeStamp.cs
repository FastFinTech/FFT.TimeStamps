using System;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using Newtonsoft.Json;

namespace FFT.TimeStamps
{
  /// <summary>
  /// Use this to get extremely fast timestamping when:
  /// 1. Your primary purpose is fast, efficient storage of exact times, across multiple timezones.
  /// 2. You run very frequent comparisons of timestamps, possibly as a way of comparing across multiple timezones.
  /// 2. You DON'T often need to extract string representations, or get the day/month/year/hour/minute/second properties (compute intensive)
  /// </summary>
  [DebuggerTypeProxy(typeof(DebuggerView))]
  public readonly partial struct TimeStamp : IComparable, IComparable<TimeStamp>, IEquatable<TimeStamp>
  {
    /// <summary>
    /// Expresses the time to its full 1-tick (ten-millionth of a second) resolution and the timezone offset as well.
    /// </summary>
    private const string DEFAULT_FORMAT_STRING = "yyyy-MM-dd HH:mm:ss.fffffff zzz";

    //    Static fields

    /// <summary>
    /// Contains the minimum possible value of a <see cref="TimeStamp"/>
    /// </summary>
    public static readonly TimeStamp MinValue = new TimeStamp(0); // also the equivalent of default(TimeStamp), because of the 0

    /// <summary>
    /// Contains the maximum possible value of a <see cref="TimeStamp"/>
    /// </summary>
    public static readonly TimeStamp MaxValue = new TimeStamp(long.MaxValue);

    /// <summary>
    /// Contains the timestamp at 0 unix time, Midnight, 1 January 1970.
    /// </summary>
    public static readonly TimeStamp UnixEpoch = new TimeStamp(new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero));

    //    Instance fields

    /// <summary>
    /// The number of ticks in UTC timezone.
    /// </summary>
    public readonly long TicksUtc;

    //    Constructors 

    /// <summary>
    /// Creates a new <see cref="TimeStamp"/> with the given number of ticks UTC.
    /// </summary>
    /// <param name="ticksUtc"></param>
    public TimeStamp(in long ticksUtc)
      => TicksUtc = ticksUtc;

    /// <summary>
    /// Creates a TimeStamp from the ticks in the given TimeZone.
    /// Compute intensive. Don't use it in a hot path.
    /// </summary>
    public TimeStamp(in long ticksTimeZone, in TimeZoneInfo timeZone)
      => TicksUtc = ticksTimeZone - TimeZoneOffsetCalculator.Get(timeZone).GetOffsetFromTimeZoneTicks(ticksTimeZone, out _, out _);

    /// <summary>
    /// Creates a TimeStamp from the given DateTimeOffset
    /// </summary>
    public TimeStamp(in DateTimeOffset at)
      => TicksUtc = at.Ticks - at.Offset.Ticks;

    /// <summary>
    /// Constructs a <see cref="TimeStamp"/> at the <paramref name="date"/> and <paramref name="timeOfDay"/> in the given <paramref name="timeZone"/>.
    /// Compute intensive. Don't use it in a hot path.
    /// </summary>
    public TimeStamp(in DateStamp date, in TimeSpan timeOfDay, TimeZoneInfo timeZone)
    {
      var ticksTimeZone = date.DateTime.Ticks + timeOfDay.Ticks;
      TicksUtc = ticksTimeZone - TimeZoneOffsetCalculator.Get(timeZone).GetOffsetFromTimeZoneTicks(ticksTimeZone, out _, out _);
    }

    //    Now

    /// <summary>
    /// Gets the current time as a <see cref="TimeStamp"/>.
    /// </summary>
    public static TimeStamp Now
      // NB: I tried implementing this using fancy, supposedly faster methods like GetSystemTimePreciseAsFileTime blah blah blah 
      // but the implementation coded below turned out to be the fastest possible. I guess if you check out the code for DateTime.UtcNow
      // you will see that it does use a GetSystemTimeAsFileTime method call, but via one that is "embedded" somehow and runs much
      // faster, even with the DateTime overhead, than we can run it by calling
      //   [DllImport("Kernel32.dll", CallingConvention = CallingConvention.Winapi)]
      //   private static extern void GetSystemTimePreciseAsFileTime(out long filetime);
      // which is much slower.
      => new TimeStamp(DateTime.UtcNow.Ticks);

    //    ToString

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


#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

    //    Comparison

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int CompareTo(TimeStamp other)
      => TicksUtc.CompareTo(other.TicksUtc);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int CompareTo(object obj)
    {
      if (obj is TimeStamp other) return CompareTo(other);
      throw new ArgumentException($"{nameof(obj)} is not a TimeStamp.", nameof(obj));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override bool Equals(object obj)
      => obj is TimeStamp other && Equals(other);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool Equals(TimeStamp other)
      => TicksUtc == other.TicksUtc;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override int GetHashCode()
      => TicksUtc.GetHashCode();

    //    Operators

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator ==(TimeStamp a, TimeStamp b)
      => a.TicksUtc == b.TicksUtc;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator !=(TimeStamp a, TimeStamp b)
      => a.TicksUtc != b.TicksUtc;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator ==(in TimeStamp a, in TimeStamp b)
      => a.TicksUtc == b.TicksUtc;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator !=(in TimeStamp a, in TimeStamp b)
      => a.TicksUtc != b.TicksUtc;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator >(in TimeStamp a, in TimeStamp b)
      => a.TicksUtc > b.TicksUtc;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator <(in TimeStamp a, in TimeStamp b)
      => a.TicksUtc < b.TicksUtc;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator >=(in TimeStamp a, in TimeStamp b)
      => a.TicksUtc >= b.TicksUtc;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator <=(in TimeStamp a, in TimeStamp b)
      => a.TicksUtc <= b.TicksUtc;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static TimeStamp operator +(in TimeStamp a, in long ticks)
      => new TimeStamp(a.TicksUtc + ticks);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static TimeStamp operator -(in TimeStamp a, in long ticks)
      => new TimeStamp(a.TicksUtc - ticks);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static TimeStamp operator +(in TimeStamp a, in TimeSpan time)
      => new TimeStamp(a.TicksUtc + time.Ticks);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static TimeStamp operator -(in TimeStamp a, in TimeSpan time)
      => new TimeStamp(a.TicksUtc - time.Ticks);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static TimeSpan operator -(in TimeStamp a, in TimeStamp b)
      => new TimeSpan(a.TicksUtc - b.TicksUtc);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator long(in TimeStamp a)
      => a.TicksUtc;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator TimeStamp(in long ticksUtc)
      => new TimeStamp(ticksUtc);
  }
}
