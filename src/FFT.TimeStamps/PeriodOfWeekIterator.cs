// Copyright (c) True Goodwill. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace FFT.TimeStamps
{
  using System;

  /// <summary>
  /// This class allows you to keep track of the current time in terms of "periods of the week" in a specific timezone.
  /// </summary>
  public sealed class PeriodOfWeekIterator
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="PeriodOfWeekIterator"/> class.
    /// </summary>
    public PeriodOfWeekIterator(TimeZoneInfo timeZone, TimeSpan periodLength, TimeSpan periodOffset, TimeStamp at)
    {
      if (((TimeSpan.TicksPerDay * 7) % periodLength.Ticks) != 0)
        throw new ArgumentException($"{nameof(periodLength)} '{periodLength:c}' does not divide evenly into a week.");

      if (periodOffset >= periodLength)
        throw new ArgumentException($"{nameof(periodOffset)} '{periodOffset:c}' must be less than {nameof(periodLength)} '{periodLength:c}'.");

      if (periodOffset.Ticks < 0)
        throw new ArgumentException($"{nameof(periodOffset)} '{periodOffset:c}' must be >= 0.");

      if (periodLength.Ticks <= 0)
        throw new ArgumentException($"{nameof(periodLength)} '{periodLength:c}' must be > 0.");

      TimeZone = timeZone;
      PeriodLength = periodLength;
      PeriodOffset = periodOffset;
      At = at;

      var start = at.ToPeriodOfWeekFloor(periodLength, periodOffset, timeZone);
      var end = start.Add(periodLength, timeZone);
      Current = new(start, end);
      Previous = new(start.Add(-periodLength, timeZone), start);
      Next = new(end, end.Add(periodLength, timeZone));
    }

    /// <summary>
    /// Gets the timezone that the periods of week are defined for.
    /// </summary>
    public TimeZoneInfo TimeZone { get; }

    /// <summary>
    /// Gets the length of the periods that the week is divided into.
    /// </summary>
    public TimeSpan PeriodLength { get; }

    /// <summary>
    /// Gets the offset that is used when the first period of week is not supposed to start at midday, Sunday.
    /// </summary>
    public TimeSpan PeriodOffset { get; }

    /// <summary>
    /// Gets the time that the iterator has been advanced to.
    /// </summary>
    public TimeStamp At { get; private set; }

    /// <summary>
    /// Gets a value representing the next period of week.
    /// </summary>
    public PeriodOfTime Previous { get; private set; }

    /// <summary>
    /// Gets a value representing the period of week in progress at the current time.
    /// </summary>
    public PeriodOfTime Current { get; private set; }

    /// <summary>
    /// Gets a value representing the next period of week.
    /// </summary>
    public PeriodOfTime Next { get; private set; }

    /// <summary>
    /// Advances the iterator to the given <paramref name="at"/>.
    /// </summary>
    /// <returns>True if the move resulted in a new period of week, False otherwise.</returns>
    public bool MoveTo(in TimeStamp at)
    {
      var isNewPeriod = at.TicksUtc >= Current.End.TicksUtc;

      if (isNewPeriod)
      {
        MoveNext();
        while (at.TicksUtc >= Current.End.TicksUtc)
          MoveNext();
      }

      At = at;
      return isNewPeriod;
    }

    private void MoveNext()
    {
      Previous = Current;
      Current = Next;
      Next = new(Current.End, Current.End.Add(PeriodLength, TimeZone));
    }
  }
}
