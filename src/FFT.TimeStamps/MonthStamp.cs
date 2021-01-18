using System;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using Newtonsoft.Json;

namespace FFT.TimeStamps
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

  /// <summary>
  /// Use this class to specify a particular month in the Gregorian calendar.
  /// Its intent to to be very clear that it is a MONTH, and not a moment in time,
  /// and to properly serialize and deserialize as such without the influence of timezone conversions.
  /// </summary>
  [JsonConverter(typeof(MonthStampConverter))]
  public readonly struct MonthStamp : IEquatable<MonthStamp>, IComparable<MonthStamp>
  {

    /// <summary>
    /// Minimum possible <see cref="MonthStamp"/> value of 0001-01-01
    /// </summary>
    public static readonly MonthStamp MinValue = CreateFrom(DateTime.SpecifyKind(DateTime.MinValue, DateTimeKind.Utc).ToMonthCeiling());

    /// <summary>
    /// Maximum possible <see cref="DateStamp"/> value of 9999-12
    /// </summary>
    public static readonly MonthStamp MaxValue = CreateFrom(DateTime.SpecifyKind(DateTime.MaxValue, DateTimeKind.Utc).ToMonthFloor());

    /// <summary>
    /// A <see cref="DateTime"/> set at exactly midnight of the first day  of the month represented by this <see cref="MonthStamp"/>.
    /// Its <see cref="DateTime.Kind"/> property is set equal to <see cref="DateTimeKind.Utc"/>.
    /// </summary>
    public readonly DateTime DateTime;

    public int Year
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      get => DateTime.Year;
    }

    public int Month
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      get => DateTime.Month;
    }

    /// <summary>
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when the given values do not form a valid month.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public MonthStamp(int year, int month) : this(new DateTime(year, month, 1, 0, 0, 0, DateTimeKind.Utc)) { }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal MonthStamp(DateTime month)
    {
      DateTime = month;
    }

    /// <summary>
    /// Creates a <see cref="MonthStamp"/> from the given <paramref name="month"/> parameter.
    /// <paramref name="month"/> must be a <see cref="DateTime"/> with
    ///     a) <see cref="DateTime.Kind"/> equal to <see cref="DateTimeKind.Utc"/>,
    ///     b) <see cref="DateTime.Day"/> equal to 1,
    ///     c) <see cref="DateTime.TimeOfDay"/> equal to <see cref="TimeSpan.Zero"/>.
    /// </summary>
    /// <exception cref="ArgumentException">
    /// Thrown if: 
    ///     a) The <see cref="DateTime.Kind"/> property of <paramref name="month"/> is not equal to <see cref="DateTimeKind.Utc"/>.
    ///     b) The <see cref="DateTime.Day"/> property of <paramref name="month"/> is not equal to '1'.
    ///     c) The <see cref="DateTime.TimeOfDay"/> property of <paramref name="month"/> is not equal to <see cref="TimeSpan.Zero"/>.
    /// </exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static MonthStamp CreateFrom(DateTime month)
    {
      if (month.Kind != DateTimeKind.Utc) throw new ArgumentException("Kind must be Utc.", nameof(month));
      if (month.Day != 1) throw new ArgumentException("Day component must be one.", nameof(month));
      if (month.TimeOfDay != TimeSpan.Zero) throw new ArgumentException("TimeOfDay component must be zero", nameof(month));
      return new MonthStamp(month);
    }

    /// <summary>
    /// Gets the current month in Utc timezone.
    /// </summary>
    public static MonthStamp UtcThisMonth
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      get => TimeStamp.Now.GetMonth();
    }

    //    Add

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public MonthStamp AddMonths(int numMonths)
      => new MonthStamp(DateTime.AddMonths(numMonths));

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public MonthStamp AddYears(int numYears)
      => new MonthStamp(DateTime.AddYears(numYears));

    //    Get months since / until

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int GetMonthsSince(MonthStamp other)
      => 12 * (Year - other.Year) + Month - other.Month;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int GetMonthsUntil(MonthStamp other)
      => other.GetMonthsSince(this);

    //    Min / Max

    /// <summary>
    /// Returns the lesser of the two monthstamps.
    /// </summary>
    public MonthStamp OrValueIfLesser(in MonthStamp d)
        => DateTime <= d.DateTime ? this : d;

    /// <summary>
    /// Returns the greater of the two monthstamps.
    /// </summary>
    public MonthStamp OrValueIfGreater(in MonthStamp d)
        => DateTime >= d.DateTime ? this : d;

    /// <summary>
    /// Returns the minimum of the given values.
    /// </summary>
    public static MonthStamp Min(in MonthStamp m1, in MonthStamp m2)
      => m1 < m2 ? m1 : m2;

    /// <summary>
    /// Returns the minimum of the given values.
    /// </summary>
    /// <exception cref="ArgumentException">Thrown when <paramref name="values"/> is null or of length 0</exception>
    public static MonthStamp Min(params MonthStamp[] values)
    {
      if (values is not { Length: > 0 }) throw new ArgumentException("Number of values must be greater than zero.", nameof(values));
      var result = values[0];
      for (var i = values.Length - 1; i > 0; i--)
        if (values[i] < result)
          result = values[i];
      return result;
    }

    /// <summary>
    /// Returns the maximum of the given values.
    /// </summary>
    public static MonthStamp Max(in MonthStamp m1, in MonthStamp m2)
      => m1 > m2 ? m1 : m2;

    /// <summary>
    /// Returns the maximum of the given values.
    /// </summary>
    /// <exception cref="ArgumentException">Thrown when <paramref name="values"/> is null or of length 0</exception>
    public static MonthStamp Max(params MonthStamp[] values)
    {
      if (values is not { Length: > 0 }) throw new ArgumentException("Number of values must be greater than zero.", nameof(values));
      var result = values[0];
      for (var i = values.Length - 1; i > 0; i--)
        if (values[i] > result)
          result = values[i];
      return result;
    }

    //    ToString and FromString

    /// <summary>
    /// Gets a string representation of the current value in the format yyyy-MM
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override string ToString()
        => $"{Year:D4}-{Month:D2}";

    /// <summary>
    /// Parses a <see cref="DateStamp"/> from the format 'yyyy-MM-dd'
    /// </summary>
    /// <exception cref="ArgumentException">Thrown if <paramref name="value"/> is null or not in the correct format.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when the given values do not form a valid date.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static MonthStamp FromString(string value)
    {
      if (value is null)
        throw new ArgumentException($"${nameof(value)} is not in correct format 'yyyy-MM'.");
      return FromString(value.AsSpan());
    }

    /// <summary>
    /// Parses a <see cref="MonthStamp"/> from the format 'yyyy-MM'
    /// </summary>
    /// <exception cref="ArgumentException">Thrown if <paramref name="value"/> is null or not in the correct format.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when the given values do not form a valid date.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static MonthStamp FromString(ReadOnlySpan<char> value)
    {
      if (value.Length == 7
        && value[4] == '-'
        && int.TryParse(value.Slice(0, 4), NumberStyles.Integer, CultureInfo.InvariantCulture, out var year)
        && int.TryParse(value.Slice(5, 2), NumberStyles.Integer, CultureInfo.InvariantCulture, out var month))
      {
        return new MonthStamp(year, month);
      }
      throw new ArgumentException($"${nameof(value)} is not in correct format 'yyyy-MM'.");
    }

    //    Comparison operators

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override bool Equals(object obj)
      => obj is MonthStamp stamp && Equals(stamp);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool Equals(MonthStamp other)
      => DateTime == other.DateTime;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override int GetHashCode()
    {
      var hash = new HashCode();
      hash.Add(typeof(MonthStamp));
      hash.Add(DateTime);
      return hash.ToHashCode();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int CompareTo(MonthStamp other)
      => DateTime.CompareTo(other.DateTime);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator ==(MonthStamp left, MonthStamp right)
      => left.DateTime == right.DateTime;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator !=(MonthStamp left, MonthStamp right)
      => left.DateTime != right.DateTime;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator >(MonthStamp left, MonthStamp right)
      => left.DateTime > right.DateTime;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator <(MonthStamp left, MonthStamp right)
      => left.DateTime < right.DateTime;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator >=(MonthStamp left, MonthStamp right)
      => left.DateTime >= right.DateTime;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator <=(MonthStamp left, MonthStamp right)
      => left.DateTime <= right.DateTime;

    //    Json Converter

    internal class MonthStampConverter : JsonConverter
    {
      public override bool CanConvert(Type objectType)
          => objectType == typeof(MonthStamp) || objectType == typeof(MonthStamp?);

      public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
        => writer.WriteValue(value?.ToString());

      public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
        => reader.Value is null ? null : FromString((string)reader.Value);
    }
  }
}
