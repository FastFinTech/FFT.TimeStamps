// Copyright (c) True Goodwill. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace FFT.TimeStamps
{
  using System;
  using System.Globalization;
  using System.Runtime.CompilerServices;

  /// <summary>
  /// Use this class to specify a particular month in the Gregorian calendar.
  /// Its intent to to be very clear that it is a MONTH, and not a moment in time,
  /// and to properly serialize and deserialize as such without the influence of timezone conversions.
  /// </summary>
  public readonly partial struct MonthStamp : IEquatable<MonthStamp>, IComparable<MonthStamp>
  {
    /// <summary>
    /// Minimum possible <see cref="MonthStamp"/> value of 0001-01-01.
    /// </summary>
    public static readonly MonthStamp MinValue = CreateFrom(DateTime.SpecifyKind(DateTime.MinValue, DateTimeKind.Utc).ToMonthCeiling());

    /// <summary>
    /// Maximum possible <see cref="DateStamp"/> value of 9999-12.
    /// </summary>
    public static readonly MonthStamp MaxValue = CreateFrom(DateTime.SpecifyKind(DateTime.MaxValue, DateTimeKind.Utc).ToMonthFloor());

    /// <summary>
    /// A <see cref="DateTime"/> set at exactly midnight of the first day  of the month represented by this <see cref="MonthStamp"/>.
    /// Its <see cref="DateTime.Kind"/> property is set equal to <see cref="DateTimeKind.Utc"/>.
    /// </summary>
    public readonly DateTime DateTime;

    /// <summary>
    /// Initializes a new instance of the <see cref="MonthStamp"/> struct.
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when the given values do not form a valid month.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public MonthStamp(int year, int month)
        : this(new DateTime(year, month, 1, 0, 0, 0, DateTimeKind.Utc))
    {
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal MonthStamp(DateTime month)
    {
      DateTime = month;
    }

    /// <summary>
    /// Gets the current month in Utc timezone.
    /// </summary>
    public static MonthStamp UtcThisMonth
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      get => TimeStamp.Now.GetMonth();
    }

    /// <summary>
    /// Gets the month component of the current value.
    /// </summary>
    public int Month
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      get => DateTime.Month;
    }

    /// <summary>
    /// Gets the year component of the current value.
    /// </summary>
    public int Year
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      get => DateTime.Year;
    }

    /// <summary>
    /// Gets a string representation of the current value in the format yyyy-MM.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override string ToString()
        => $"{Year:D4}-{Month:D2}";
  }

  // Add
  public partial struct MonthStamp
  {
    /// <summary>
    /// Adds the given number of months to the current value and returns the result.
    /// </summary>
    /// <param name="numMonths">The number of months to add. Can be negative.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public MonthStamp AddMonths(int numMonths)
      => new MonthStamp(DateTime.AddMonths(numMonths));

    /// <summary>
    /// Adds the given number of months to the current value and returns the result.
    /// </summary>
    /// <param name="numYears">The number of years to add. Can be negative.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public MonthStamp AddYears(int numYears)
      => new MonthStamp(DateTime.AddYears(numYears));
  }

  // Static constructors
  public partial struct MonthStamp
  {
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
    /// Parses a <see cref="DateStamp"/> from the format 'yyyy-MM-dd'.
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
    /// Parses a <see cref="MonthStamp"/> from the format 'yyyy-MM'.
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
  }

  // Get months since / until
  public partial struct MonthStamp
  {
    /// <summary>
    /// Calculates the difference, in months, between the current value and <paramref name="other"/>.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int GetMonthsSince(MonthStamp other)
      => (12 * (Year - other.Year)) + Month - other.Month;

    /// <summary>
    /// Calculates the difference, in months, between the current value and <paramref name="other"/>.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int GetMonthsUntil(MonthStamp other)
      => other.GetMonthsSince(this);
  }

  // Min Max etc
  public partial struct MonthStamp
  {
    /// <summary>
    /// Returns the minimum of the given values.
    /// </summary>
    public static MonthStamp Min(MonthStamp m1, MonthStamp m2)
      => m1 < m2 ? m1 : m2;

    /// <summary>
    /// Returns the minimum of the given values.
    /// </summary>
    /// <exception cref="ArgumentException">Thrown when <paramref name="values"/> is null or of length 0.</exception>
    public static MonthStamp Min(params MonthStamp[] values)
    {
      if (values is not { Length: > 0 }) throw new ArgumentException("Number of values must be greater than zero.", nameof(values));
      var result = values[0];
      for (var i = values.Length - 1; i > 0; i--)
      {
        if (values[i] < result)
          result = values[i];
      }

      return result;
    }

    /// <summary>
    /// Returns the maximum of the given values.
    /// </summary>
    public static MonthStamp Max(MonthStamp m1, MonthStamp m2)
      => m1 > m2 ? m1 : m2;

    /// <summary>
    /// Returns the maximum of the given values.
    /// </summary>
    /// <exception cref="ArgumentException">Thrown when <paramref name="values"/> is null or of length 0.</exception>
    public static MonthStamp Max(params MonthStamp[] values)
    {
      if (values is not { Length: > 0 }) throw new ArgumentException("Number of values must be greater than zero.", nameof(values));
      var result = values[0];
      for (var i = values.Length - 1; i > 0; i--)
      {
        if (values[i] > result)
          result = values[i];
      }

      return result;
    }

    /// <summary>
    /// Returns the lesser of the two monthstamps.
    /// </summary>
    public MonthStamp OrValueIfLesser(MonthStamp d)
        => DateTime <= d.DateTime ? this : d;

    /// <summary>
    /// Returns the greater of the two monthstamps.
    /// </summary>
    public MonthStamp OrValueIfGreater(MonthStamp d)
        => DateTime >= d.DateTime ? this : d;
  }

  // Comparison operators
  public partial struct MonthStamp
  {
    /// <inheritdoc/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override bool Equals(object obj)
      => obj is MonthStamp stamp && Equals(stamp);

    /// <inheritdoc/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool Equals(MonthStamp other)
      => DateTime == other.DateTime;

    /// <inheritdoc/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override int GetHashCode()
      => DateTime.GetHashCode();

    /// <inheritdoc/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int CompareTo(MonthStamp other)
      => DateTime.CompareTo(other.DateTime);
  }

  // Operators
  public partial struct MonthStamp
  {
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

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
  }
}
