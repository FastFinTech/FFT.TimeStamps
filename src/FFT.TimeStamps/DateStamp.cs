using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using Newtonsoft.Json;
using static System.DayOfWeek;

namespace FFT.TimeStamps
{
  /// <summary>
  /// Use this class to specify a particular date in the Gregorian calendar.
  /// Its intent to to be very clear that it represents a DATE, and not a moment in time,
  /// and to properly serialize and deserialize as such without the influence of timezone conversions.
  /// </summary>
  public readonly struct DateStamp : IEquatable<DateStamp>, IComparable<DateStamp>
  {
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

    /**********************************************************************
     * Dev notes:
     * Yes, this class IS just a wrapper around the System.DateTime object.
     * But it is much better than that for its intended purpose, because
     * a) There is no ambiguity with TimeOfDay component.
     * b) There is no confusion with the DateTimeKind property.
     * c) The json serialization / deserialization works EXACTLY as intended,
     *    unlike the case with DateTime, where deserialization can result in a 
     *    warped time component with misadjusted DateTimeKind property.
     *    
     * I could have, and did consider, writing this struct using an int as 
     * the backing store variable instead of a DateTime. But many of the 
     * methods and properties in this object's api are made very simple by 
     * directly borrowing methods and properties from the DateTime, so it made
     * sense to simplify my code (and spend a lot less time writing it) using 
     * the DateTime as the backing variable. The disadvantages of this choice are:
     * a) The DateTime object is more than twice as large as an int, so this struct 
     *    is more than twice as large (and slow) as it could have been.
     * My personal code didn't seem to use DateStamps in hotpath methods, so I've
     * been quite happy with the tradeoff so far.
    ***********************************************************************/

    /// <summary>
    /// Minimum possible <see cref="DateStamp"/> value of 0001-01-01
    /// </summary>
    public static readonly DateStamp MinValue = new DateStamp(new DateTime(0, DateTimeKind.Utc));

    /// <summary>
    /// Maximum possible <see cref="DateStamp"/> value of 9999-12-31
    /// </summary>
    public static readonly DateStamp MaxValue = new DateStamp(DateTime.MaxValue.ToDayFloor().AssumeUniversal());

    /// <summary>
    /// A <see cref="DateTime"/> set at exactly midnight of the day represented by this <see cref="DateStamp"/>.
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

    public int Day
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      get => DateTime.Day;
    }

    public DayOfWeek DayOfWeek
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      get => DateTime.DayOfWeek;
    }

    /// <summary>
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException">Thrown vwhen the given values do not form a valid date.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public DateStamp(int year, int month, int day)
      : this(new DateTime(year, month, day, 0, 0, 0, DateTimeKind.Utc)) { }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal DateStamp(DateTime date)
      => DateTime = date;

    /// <summary>
    /// Creates a <see cref="DateStamp"/> from the given <paramref name="date"/> parameter.
    /// <paramref name="date"/> must be a <see cref="DateTime"/> with
    ///     a) <see cref="DateTime.Kind"/> equal to <see cref="DateTimeKind.Utc"/>,
    ///     b) <see cref="DateTime.TimeOfDay"/> equal to <see cref="TimeSpan.Zero"/>.
    /// </summary>
    /// <exception cref="ArgumentException">
    /// Thrown if: 
    ///     a) The <see cref="DateTime.Kind"/> property of <paramref name="date"/> is not equal to <see cref="DateTimeKind.Utc"/>.
    ///     b) The <see cref="DateTime.TimeOfDay"/> property of <paramref name="date"/> is not equal to <see cref="TimeSpan.Zero"/>.
    /// </exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static DateStamp CreateFrom(DateTime date)
    {
      if (date.Kind != DateTimeKind.Utc) throw new ArgumentException("Kind must be Utc.", nameof(date));
      if (date.TimeOfDay != TimeSpan.Zero) throw new ArgumentException("TimeOfDay component must be zero", nameof(date));
      return new DateStamp(date);
    }

    /// <summary>
    /// Gets the current date in Utc timezone.
    /// </summary>
    public static DateStamp UtcToday
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      get => TimeStamp.Now.GetDate();
    }

    /// <summary>
    /// Gets the current date in the <paramref name="timeZone"/> given.
    /// This operation involves a time zone conversion, so it is a little less efficient.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static DateStamp Today(TimeZoneInfo timeZone)
      => TimeStamp.Now.GetDate(timeZone);

    /// <summary>
    /// Returns an enumerator that yields each date in the range <paramref name="from"/> until <paramref name="to"/>.
    /// </summary>
    /// <exception cref="ArgumentException">Thrown when <paramref name="to"/> is less than <paramref name="from"/>.</exception>
    public static IEnumerable<DateStamp> Range(DateStamp from, DateStamp to)
    {
      if (to < from) throw new ArgumentException($"{nameof(to)} '{to}' must be greater than or equal to {nameof(from)} '{from}'.");
      for (var date = from; date <= to; date = date.AddDays(1))
        yield return date;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public DateStamp AddDays(int numDays)
      => new DateStamp(DateTime.AddDays(numDays));

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public DateStamp AddMonths(int numMonths)
      => new DateStamp(DateTime.AddMonths(numMonths));

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public DateStamp AddYears(int numYears)
      => new DateStamp(DateTime.AddYears(numYears));

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public MonthStamp GetMonthStamp()
        => new MonthStamp(Year, Month);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int GetDaysSince(DateStamp other)
        => (int)DateTime.Subtract(other.DateTime).TotalDays;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int GetDaysSince(DayOfWeek dayOfWeek)
    {
      var result = (int)DayOfWeek - (int)dayOfWeek;
      if (result >= 0) return result;
      return result + 7;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int GetDaysUntil(DayOfWeek dayOfWeek)
    {
      var result = (int)dayOfWeek - (int)DayOfWeek;
      if (result >= 0) return result;
      return result + 7;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int GetDaysUntil(DateStamp other)
      => other.GetDaysSince(this);

    /// <summary>
    /// Returns the Sunday at the beginning of the week. 
    /// The same value is returned if the value is already Sunday.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public DateStamp ToWeekFloor()
      => new DateStamp(DateTime.ToWeekFloor());

    //    WEEKENDS AND HOLIDAYS

    /// <summary>
    /// If the current value is a weekday, it will be returned unchanged.
    /// Otherwise, the first weekday in the future (a Monday) will be returned.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public DateStamp SkipWeekendMovingForward()
      => DayOfWeek switch
      {
        Saturday => AddDays(2),
        Sunday => AddDays(1),
        _ => this,
      };

    /// <summary>
    /// If the current value is a weekday, it will be returned unchanged.
    /// Otherwise, the most recent weekday in the past (a Friday) will be returned.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public DateStamp SkipWeekendMovingBackward()
      => DayOfWeek switch
      {
        Sunday => AddDays(-2),
        Saturday => AddDays(-1),
        _ => this,
      };

    /// <summary>
    /// Returns true if the current value is a Saturday or Sunday, false otherwise.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool IsWeekend()
      => DayOfWeek == Saturday || DayOfWeek == Sunday;

    /// <summary>
    /// If the current value a weekday and not contained by <paramref name="datesToSkip"/>, it will be returned.
    /// Otherise, the first weekday in the future that is not contained by <paramref name="datesToSkip"/> will be returned.
    /// </summary>
    public DateStamp SkipWeekendAndTheseDatesMovingForward(IEnumerable<DateStamp> datesToSkip)
    {
      // yes, this method has been coded without linq and in a slightly more complicated way
      // to make it alloation-free and avoid a stack dive.
      var value = this;
      while (true)
      {
        if (value.IsWeekend()) goto next;
        foreach (var date in datesToSkip)
          if (date == value) goto next;
        return value;
next:
        value = value.AddDays(1);
      }
    }

    /// <summary>
    /// If the current value a weekday and not contained by <paramref name="datesToSkip"/>, it will be returned.
    /// Otherise, the most recent weekday in the past that is not contained by <paramref name="datesToSkip"/> will be returned.
    /// </summary>
    public DateStamp SkipWeekendAndTheseDatesMovingBackward(IEnumerable<DateStamp> datesToSkip)
    {
      // yes, this method has been coded without linq and in a slightly more complicated way
      // to make it alloation-free and avoid a stack dive.
      var value = this;
      while (true)
      {
        if (value.IsWeekend()) goto next;
        foreach (var date in datesToSkip)
          if (date == value) goto next;
        return value;
next:
        value = value.AddDays(-1);
      }
    }

    //   TOSTRING / FROMSTRING

    /// <summary>
    /// Expresses the value in the format 'yyyy-MM-dd'.
    /// </summary>
    public override string ToString()
        => $"{Year:D4}-{Month:D2}-{Day:D2}";

    /// <summary>
    /// Parses a <see cref="DateStamp"/> from the format 'yyyy-MM-dd'
    /// </summary>
    /// <exception cref="ArgumentException">Thrown if <paramref name="value"/> is null or not in the correct format.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when the given values do not form a valid date.</exception>
    public static DateStamp FromString(string value)
    {
      if (value is null)
        throw new ArgumentException($"${nameof(value)} is not in correct format 'yyyy-MM-dd'.");
      return FromString(value.AsSpan());
    }

    /// <summary>
    /// Parses a <see cref="DateStamp"/> from the format 'yyyy-MM-dd'
    /// </summary>
    /// <exception cref="ArgumentException">Thrown if <paramref name="value"/> is not in the correct format.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when the given values do not form a valid date.</exception>
    public static DateStamp FromString(in ReadOnlySpan<char> value)
    {
      if (value.Length == 10
        && value[4] == '-'
        && value[7] == '-'
        && int.TryParse(value.Slice(0, 4), NumberStyles.Integer, CultureInfo.InvariantCulture, out var year)
        && int.TryParse(value.Slice(5, 2), NumberStyles.Integer, CultureInfo.InvariantCulture, out var month)
        && int.TryParse(value.Slice(8, 2), NumberStyles.Integer, CultureInfo.InvariantCulture, out var day))
      {
        return new DateStamp(year, month, day);
      }
      throw new ArgumentException($"${nameof(value)} is not in correct format 'yyyy-MM-dd'.");
    }

    //   MIN / MAX METHODS

    /// <summary>
    /// Returns the lesser of the two datestamps.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public DateStamp OrValueIfLesser(in DateStamp d)
      => DateTime <= d.DateTime ? this : d;

    /// <summary>
    /// Returns the greater of the two datestamps.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public DateStamp OrValueIfGreater(in DateStamp d)
      => DateTime >= d.DateTime ? this : d;

    /// <summary>
    /// Returns the minimum of the given values.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static DateStamp Min(in DateStamp v1, in DateStamp v2)
      => v1 < v2 ? v1 : v2;

    /// <summary>
    /// Returns the minimum of the given values.
    /// </summary>
    /// <exception cref="ArgumentException">Thrown when <paramref name="values"/> is null or of length 0</exception>
    public static DateStamp Min(params DateStamp[] values)
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
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static DateStamp Max(in DateStamp v1, in DateStamp v2)
      => v1 > v2 ? v1 : v2;

    /// <summary>
    /// Returns the maximum of the given values.
    /// </summary>
    /// <exception cref="ArgumentException">Thrown when <paramref name="values"/> is null or of length 0</exception>
    public static DateStamp Max(params DateStamp[] values)
    {
      if (values is not { Length: > 0 }) throw new ArgumentException("Number of values must be greater than zero.", nameof(values));
      var result = values[0];
      for (var i = values.Length - 1; i > 0; i--)
        if (values[i] > result)
          result = values[i];
      return result;
    }

    //    COMPARISON OPERATORS

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override bool Equals(object obj)
      => obj is DateStamp stamp && Equals(stamp);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool Equals(DateStamp other)
      => DateTime == other.DateTime;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override int GetHashCode()
      => -1937169414 + DateTime.GetHashCode();

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int CompareTo(DateStamp other)
      => DateTime.CompareTo(other.DateTime);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator ==(DateStamp left, DateStamp right)
      => left.DateTime == right.DateTime;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator !=(DateStamp left, DateStamp right)
      => left.DateTime != right.DateTime;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator >(DateStamp left, DateStamp right)
      => left.DateTime > right.DateTime;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator <(DateStamp left, DateStamp right)
      => left.DateTime < right.DateTime;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator >=(DateStamp left, DateStamp right)
      => left.DateTime >= right.DateTime;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator <=(DateStamp left, DateStamp right)

      => left.DateTime <= right.DateTime;
  }
}
