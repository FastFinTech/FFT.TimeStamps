using System;
using System.Runtime.CompilerServices;
using Newtonsoft.Json;

// TODO: Use "in" method parameters.

namespace FFT.TimeStamps
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

  /// <summary>
  /// Expresses a day and time as a point in the week.
  /// Comparison operators assume the beginning of the week is midnight, Sunday.
  /// </summary>
  [JsonConverter(typeof(TimeOfWeekJsonConverter))]
  public readonly struct TimeOfWeek : IEquatable<TimeOfWeek>, IComparable<TimeOfWeek>
  {
    private const long TICKS_PER_WEEK = 7 * TimeSpan.TicksPerDay;

    /// <summary>
    /// Special value that represents the end of the week, or the beginning of the next week.
    /// </summary>
    public static TimeOfWeek EndOfWeek { get; } = new TimeOfWeek(TICKS_PER_WEEK);

    /// <summary>
    /// The number of ticks (ten-millionths of a second) that have elapsed since the beginning of the week, midnight Sunday.
    /// </summary>
    public long TicksSinceWeekFloor { get; }

    public DayOfWeek DayOfWeek
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      get => (DayOfWeek)(TicksSinceWeekFloor / TimeSpan.TicksPerDay);
    }

    public TimeSpan TimeOfDay
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      get => new TimeSpan(TicksSinceWeekFloor % TimeSpan.TicksPerDay);
    }

    public bool IsEndOfWeek
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      get => TicksSinceWeekFloor == TICKS_PER_WEEK;
    }

    //    Constructors

    /// <summary>
    /// Creates a <see cref="TimeOfWeek"/> with the given number of <paramref name="ticksSinceWeekFloor"/> (ten-millionths of a second)
    /// since midnight, Sunday.
    /// </summary>
    /// <exception cref="ArgumentException">Thrown if <paramref name="ticksSinceWeekFloor"/> is less than zero or greater than <see cref="EndOfWeek"/>.</exception>
    public TimeOfWeek(long ticksSinceWeekFloor)
    {
      if (ticksSinceWeekFloor < 0 || ticksSinceWeekFloor > TICKS_PER_WEEK)
        throw new ArgumentException("Value must be positive and less than or equal to the number of ticks in a week.", nameof(ticksSinceWeekFloor));
      TicksSinceWeekFloor = ticksSinceWeekFloor;
    }

    /// <summary>
    /// Gets the <see cref="TimeOfWeek"/> for the given <paramref name="dateTime"/>.
    /// </summary>
    public static TimeOfWeek CreateFrom(DateTime dateTime)
        => new TimeOfWeek(dateTime.DayOfWeek, dateTime.TimeOfDay);

    /// <summary>
    /// Constructs a <see cref="TimeOfWeek"/> at the given <paramref name="dayOfWeek"/> and <paramref name="timeOfDay"/>.
    /// </summary>
    /// <exception cref="ArgumentException">Thrown when invalid parameters are given.</exception>
    public TimeOfWeek(DayOfWeek dayOfWeek, TimeSpan timeOfDay)
    {
      if ((int)dayOfWeek < 0 || (int)dayOfWeek > 6) throw new ArgumentException($"{nameof(dayOfWeek)} '{dayOfWeek}' must be a valid day of the week.");
      if (timeOfDay.Ticks < 0 || timeOfDay.Ticks >= TimeSpan.TicksPerDay) throw new ArgumentException($"{nameof(timeOfDay)} '{timeOfDay}' must be at least zero and less than 24 hours.");
      TicksSinceWeekFloor = (long)dayOfWeek * TimeSpan.TicksPerDay + timeOfDay.Ticks;
    }

    //    Add

    /// <summary>
    /// Adds the given amount of time to the <see cref="TimeOfWeek"/> and returns the new <see cref="TimeOfWeek"/>.
    /// The result wraps around the end of the week back to the beginning, so the result of an add operation could 
    /// be "less than" the original value. The result will always be less than <see cref="EndOfWeek"/>.
    /// </summary>
    /// <remarks>
    /// I have not handled negative "time" values. You will get unexpected results if you pass in a negative value for <paramref name="time"/>.
    /// </remarks>
    public TimeOfWeek Add(TimeSpan time)
        => new TimeOfWeek((TicksSinceWeekFloor + time.Ticks) % TICKS_PER_WEEK);

    //    ToString / FromString

    /// <summary>
    /// Returns a string in the format $"{DayOfWeek} {TimeOfDay.ToString(timeOfDayFormat)}"
    /// If the value is <see cref="EndOfWeek"/>, the string "EndOfWeek" is returned.
    /// </summary>
    public string ToString(string timeOfDayFormat)
        => IsEndOfWeek ? "EndOfWeek" : $"{DayOfWeek} {TimeOfDay.ToString(timeOfDayFormat)}";

    /// <summary>
    /// Returns a string in the format $"{DayOfWeek} {TimeOfDay:c}"
    /// If the value is <see cref="EndOfWeek"/>, the string "EndOfWeek" is returned.
    /// </summary>
    public override string ToString()
        => IsEndOfWeek ? "EndOfWeek" : $"{DayOfWeek} {TimeOfDay:c}";

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

    //    Comparison operators

    public int CompareTo(TimeOfWeek other)
      => TicksSinceWeekFloor.CompareTo(other.TicksSinceWeekFloor);

    public static bool operator >(TimeOfWeek left, TimeOfWeek right)
      => left.TicksSinceWeekFloor > right.TicksSinceWeekFloor;

    public static bool operator <(TimeOfWeek left, TimeOfWeek right)
      => left.TicksSinceWeekFloor < right.TicksSinceWeekFloor;

    public static bool operator >=(TimeOfWeek left, TimeOfWeek right)
      => left.TicksSinceWeekFloor >= right.TicksSinceWeekFloor;

    public static bool operator <=(TimeOfWeek left, TimeOfWeek right)
      => left.TicksSinceWeekFloor <= right.TicksSinceWeekFloor;

    public override bool Equals(object obj)
      => obj is TimeOfWeek timeOfWeek && Equals(timeOfWeek);

    public bool Equals(TimeOfWeek other)
      => TicksSinceWeekFloor == other.TicksSinceWeekFloor;

    public override int GetHashCode()
      => TicksSinceWeekFloor.GetHashCode();

    public static bool operator ==(TimeOfWeek left, TimeOfWeek right)
      => left.Equals(right);

    public static bool operator !=(TimeOfWeek left, TimeOfWeek right)
      => !(left == right);

    private class TimeOfWeekJsonConverter : JsonConverter
    {
      public override bool CanConvert(Type objectType)
        => objectType == typeof(TimeOfWeek) || objectType == typeof(TimeOfWeek?);

      public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
        => writer.WriteValue(value?.ToString());

      public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
        => reader.Value is null ? null : FromString((string)reader.Value);
    }
  }
}
