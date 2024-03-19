// Copyright (c) True Goodwill. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace FFT.TimeStamps
{
  using System;
  using System.Runtime.CompilerServices;
  using System.Runtime.Serialization;

  /// <summary>
  /// Expresses a day and time as a point in the week.
  /// Comparison operators assume the beginning of the week is midnight, Sunday.
  /// </summary>

  [DataContract]
  public readonly partial struct TimeOfWeek : IEquatable<TimeOfWeek>, IComparable<TimeOfWeek>
  {
    private const long TICKS_PER_WEEK = 7 * TimeSpan.TicksPerDay;

    /// <summary>
    /// Initializes a new instance of the <see cref="TimeOfWeek"/> struct.
    /// </summary>
    /// <param name="ticksSinceWeekFloor">The number of ticks (ten-millionths of a second) since midnight, Sunday.</param>
    /// <exception cref="ArgumentException">Thrown if <paramref name="ticksSinceWeekFloor"/> is less than zero or greater than <see cref="EndOfWeek"/>.</exception>
    public TimeOfWeek(long ticksSinceWeekFloor)
    {
      if (ticksSinceWeekFloor < 0 || ticksSinceWeekFloor > TICKS_PER_WEEK)
        throw new ArgumentException("Value must be positive and less than or equal to the number of ticks in a week.", nameof(ticksSinceWeekFloor));
      TicksSinceWeekFloor = ticksSinceWeekFloor;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="TimeOfWeek"/> struct.
    /// </summary>
    /// <param name="dayOfWeek">The day of week for this value.</param>
    /// <param name="timeOfDay">The tine of day for this value.</param>
    /// <exception cref="ArgumentException">Thrown when invalid parameters are given.</exception>
    public TimeOfWeek(DayOfWeek dayOfWeek, TimeSpan timeOfDay)
    {
      if ((int)dayOfWeek < 0 || (int)dayOfWeek > 6) throw new ArgumentException($"{nameof(dayOfWeek)} '{dayOfWeek}' must be a valid day of the week.");
      if (timeOfDay.Ticks < 0 || timeOfDay.Ticks >= TimeSpan.TicksPerDay) throw new ArgumentException($"{nameof(timeOfDay)} '{timeOfDay}' must be at least zero and less than 24 hours.");
      TicksSinceWeekFloor = ((long)dayOfWeek * TimeSpan.TicksPerDay) + timeOfDay.Ticks;
    }

    /// <summary>
    /// Special value that represents the end of the week, or the beginning of the next week.
    /// </summary>
    public static TimeOfWeek EndOfWeek { get; } = new TimeOfWeek(TICKS_PER_WEEK);

    /// <summary>
    /// The number of ticks (ten-millionths of a second) that have elapsed since the beginning of the week, midnight Sunday.
    /// </summary>
    [DataMember]
    public long TicksSinceWeekFloor { get; }

    /// <summary>
    /// Gets the day of week component of the current value.
    /// </summary>
    public DayOfWeek DayOfWeek
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      get => (DayOfWeek)(TicksSinceWeekFloor / TimeSpan.TicksPerDay);
    }

    /// <summary>
    /// Gets the time of day component of the current value.
    /// </summary>
    public TimeSpan TimeOfDay
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      get => new TimeSpan(TicksSinceWeekFloor % TimeSpan.TicksPerDay);
    }

    /// <summary>
    /// Returns true if this value is equal to the special <see cref="EndOfWeek"/> value.
    /// </summary>
    public bool IsEndOfWeek
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      get => TicksSinceWeekFloor == TICKS_PER_WEEK;
    }

    /// <summary>
    /// Gets the <see cref="TimeOfWeek"/> for the given <paramref name="dateTime"/>.
    /// </summary>
    public static TimeOfWeek CreateFrom(DateTime dateTime)
        => new TimeOfWeek(dateTime.DayOfWeek, dateTime.TimeOfDay);

    /// <summary>
    /// Parses the given <paramref name="value"/> to create the <see cref="TimeOfWeek"/> it represents.
    /// Valid inputs are any string output by <see cref="ToString()"/>.
    /// </summary>
    public static TimeOfWeek FromString(string value)
    {
      if (value == "EndOfWeek") return EndOfWeek;
      var parts = value.Split(' ');
      var dayOfWeek = (DayOfWeek)Enum.Parse(typeof(DayOfWeek), parts[0]);
      var timeOfDay = TimeSpan.Parse(parts[1]);
      return new TimeOfWeek(dayOfWeek, timeOfDay);
    }

    /// <summary>
    /// Adds the given amount of time to the <see cref="TimeOfWeek"/> and returns the new <see cref="TimeOfWeek"/>.
    /// The result wraps around the end of the week back to the beginning, so the result of an add operation could
    /// be "less than" the original value. You can add positive or negative time values. Negative time values will
    /// also "wrap backwards" around the week.
    /// The result will always be less than <see cref="EndOfWeek"/>.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public TimeOfWeek Add(TimeSpan time)
    {
      var ticks = (TicksSinceWeekFloor + time.Ticks) % TICKS_PER_WEEK;
      if (ticks < 0)
        ticks += TICKS_PER_WEEK;
      return new TimeOfWeek(ticks);
    }

    /// <summary>
    /// Returns a string in the format $"{DayOfWeek} {TimeOfDay.ToString(timeOfDayFormat)}"
    /// If the value is <see cref="EndOfWeek"/>, the string "EndOfWeek" is returned.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public string ToString(string timeOfDayFormat)
        => IsEndOfWeek ? "EndOfWeek" : $"{DayOfWeek} {TimeOfDay.ToString(timeOfDayFormat)}";

    /// <summary>
    /// Returns a string in the format $"{DayOfWeek} {TimeOfDay:c}"
    /// If the value is <see cref="EndOfWeek"/>, the string "EndOfWeek" is returned.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override string ToString()
        => IsEndOfWeek ? "EndOfWeek" : $"{DayOfWeek} {TimeOfDay:c}";
  }

  // Equals, GetHashCode, IEquatable, IComparable
  public readonly partial struct TimeOfWeek
  {
    /// <inheritdoc/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override bool Equals(object? obj)
      => obj is TimeOfWeek timeOfWeek && Equals(timeOfWeek);

    /// <inheritdoc/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool Equals(TimeOfWeek other)
      => TicksSinceWeekFloor == other.TicksSinceWeekFloor;

    /// <inheritdoc/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override int GetHashCode()
      => TicksSinceWeekFloor.GetHashCode();

    /// <inheritdoc/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int CompareTo(TimeOfWeek other)
      => TicksSinceWeekFloor.CompareTo(other.TicksSinceWeekFloor);
  }

  // Operators
  public readonly partial struct TimeOfWeek
  {
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator >(TimeOfWeek left, TimeOfWeek right)
      => left.TicksSinceWeekFloor > right.TicksSinceWeekFloor;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator <(TimeOfWeek left, TimeOfWeek right)
      => left.TicksSinceWeekFloor < right.TicksSinceWeekFloor;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator >=(TimeOfWeek left, TimeOfWeek right)
      => left.TicksSinceWeekFloor >= right.TicksSinceWeekFloor;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator <=(TimeOfWeek left, TimeOfWeek right)
      => left.TicksSinceWeekFloor <= right.TicksSinceWeekFloor;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator ==(TimeOfWeek left, TimeOfWeek right)
      => left.Equals(right);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator !=(TimeOfWeek left, TimeOfWeek right)
      => !left.Equals(right);
  }
}
