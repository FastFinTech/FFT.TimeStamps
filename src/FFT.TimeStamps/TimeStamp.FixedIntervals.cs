using System;
using System.Runtime.CompilerServices;

namespace FFT.TimeStamps
{
  public partial struct TimeStamp
  {
    /// <summary>
    /// Divides time from <paramref name="intervalStart"/> into evenly-sized periods (intervals) of length <paramref name="intervalLength"/>
    /// and returns the beginning of the interval in progress.
    /// If the time is at an exact interval start, the same value is returned.
    /// </summary>
    /// <remarks>
    /// You will get unexpected results if the current time is less than <paramref name="intervalStart"/>
    /// or if <paramref name="intervalLength"/> is less than or equal to zero.
    /// </remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public TimeStamp ToIntervalFloor(in TimeStamp intervalStart, in TimeSpan intervalLength)
      => new TimeStamp(TicksUtc.ToIntervalFloor(intervalStart.TicksUtc, intervalLength.Ticks));

    /// <summary>
    /// Divides time from <paramref name="intervalStart"/> into evenly-sized periods (intervals) of length <paramref name="intervalLength"/>
    /// and returns the beginning of the next interval to start after the current time.
    /// If the time is at an exact interval start, the same value is returned.
    /// </summary>
    /// <remarks>
    /// You will get unexpected results if the current time is less than <paramref name="intervalStart"/>
    /// or if <paramref name="intervalLength"/> is less than or equal to zero.
    /// </remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public TimeStamp ToIntervalCeiling(in TimeStamp intervalStart, in TimeSpan intervalLength)
      => new TimeStamp(TicksUtc.ToIntervalCeiling(intervalStart.TicksUtc, intervalLength.Ticks));

    /// <summary>
    /// Divides time from <paramref name="intervalStart"/> into evenly-sized periods (intervals) of length <paramref name="intervalLength"/>
    /// and returns the beginning of the interval in progress.
    /// Daylight savings adjustments for the given <paramref name="timeZone"/> are respected.
    /// If the time is at an exact interval start, the same value is returned.
    /// Compute intensive. Do not use in a hot path.
    /// </summary>
    /// <remarks>
    /// You will get unexpected results if the current time is less than <paramref name="intervalStart"/>
    /// or if <paramref name="intervalLength"/> is less than or equal to zero.
    /// </remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public TimeStamp ToIntervalFloor(in TimeStamp intervalStart, in TimeSpan intervalLength, TimeZoneInfo timeZone)
      => new TimeStamp(AsTicks(timeZone).ToIntervalFloor(intervalStart.AsTicks(timeZone), intervalLength.Ticks), timeZone);

    /// <summary>
    /// Divides time from <paramref name="intervalStart"/> into evenly-sized periods (intervals) of length <paramref name="intervalLength"/>
    /// and returns the beginning of the next interval to start after the current time.
    /// Daylight savings adjustments for the given <paramref name="timeZone"/> are respected.
    /// If the time is at an exact interval start, the same value is returned.
    /// Compute intensive. Do not use in a hot path.
    /// </summary>
    /// <remarks>
    /// You will get unexpected results if the current time is less than <paramref name="intervalStart"/>
    /// or if <paramref name="intervalLength"/> is less than or equal to zero.
    /// </remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public TimeStamp ToIntervalCeiling(in TimeStamp intervalStart, in TimeSpan intervalLength, TimeZoneInfo timeZone)
      => new TimeStamp(AsTicks(timeZone).ToIntervalCeiling(intervalStart.AsTicks(timeZone), intervalLength.Ticks), timeZone);

    /// <summary>
    /// Divides time from <paramref name="intervalStart"/> into evenly-sized periods (intervals) of length <paramref name="intervalLength"/>
    /// Calculates the zero-based index of the interval in progress.
    /// </summary>
    /// <remarks>
    /// You will get unexpected results if the current time is less than <paramref name="intervalStart"/>
    /// or if <paramref name="intervalLength"/> is less than or equal to zero.
    /// </remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int ToIntervalIndex(in TimeStamp intervalStart, in TimeSpan intervalLength)
      => TicksUtc.ToIntervalIndex(intervalStart.TicksUtc, intervalLength.Ticks);

    /// <summary>
    /// Divides time from <paramref name="intervalStart"/> into evenly-sized periods (intervals) of length <paramref name="intervalLength"/>
    /// Calculates the zero-based index of the interval in progress.
    /// Daylight savings adjustments for the given <paramref name="timeZone"/> are respected.
    /// Compute intensive. Do not use in a hot path.
    /// </summary>
    /// <remarks>
    /// You will get unexpected results if the current time is less than <paramref name="intervalStart"/>
    /// or if <paramref name="intervalLength"/> is less than or equal to zero.
    /// </remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int ToIntervalIndex(in TimeStamp intervalStart, in TimeSpan intervalLength, TimeZoneInfo timeZone)
      => AsTicks(timeZone).ToIntervalIndex(intervalStart.AsTicks(timeZone), intervalLength.Ticks);

    /// <summary>
    /// Divides the week into periods of length <paramref name="periodLength"/>, and then calculates
    /// the beginning of the period active at the current time.
    /// Beginning of the first period is considered to be midnight, Sunday.
    /// Periods include the first tick (index == 0) and exclude the the first tick of the next period (index &lt; periodLengthInMinutes * TimeSpan.TicksPerMinute)
    /// If the current time is at an exact period start, the same value is returned
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public TimeStamp ToPeriodOfWeekFloor(in TimeSpan periodLength, TimeZoneInfo timeZone)
      => new TimeStamp(AsTicks(timeZone).ToPeriodOfWeekFloor(periodLength.Ticks), timeZone);

    /// <summary>
    /// Divides the week into periods of length <paramref name="periodLength"/>, and then calculates
    /// the beginning of the period active at the current time.
    /// Beginning of the first period is considered to be midnight, Sunday.
    /// Periods include the first tick (index == 0) and exclude the the first tick of the next period (index &lt; periodLengthInMinutes * TimeSpan.TicksPerMinute)
    /// If the current time is at an exact period start, the same value is returned
    /// Daylight savings adjustments for the given <paramref name="timeZone"/> are respected.
    /// Compute intensive. Do not use in a hot path.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public TimeStamp ToPeriodOfWeekFloor(in TimeSpan periodLength, in TimeSpan periodOffset, TimeZoneInfo timeZone)
      => new TimeStamp(AsTicks(timeZone).ToPeriodOfWeekFloor(periodLength.Ticks, periodOffset.Ticks), timeZone);

    /// <summary>
    /// Divides the week into periods of length <paramref name="periodLength"/>, and then calculates
    /// the beginning of the period after the period active at the current time.
    /// Beginning of the first period is considered to be midnight, Sunday.
    /// Periods include the first tick (index == 0) and exclude the the first tick of the next period (index &lt; periodLengthInMinutes * TimeSpan.TicksPerMinute)
    /// If the current time is at an exact period start, the same value is returned
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public TimeStamp ToPeriodOfWeekCeiling(in TimeSpan periodLength, TimeZoneInfo timeZone)
      => new TimeStamp(AsTicks(timeZone).ToPeriodOfWeekCeiling(periodLength.Ticks), timeZone);

    /// <summary>
    /// Divides the week into periods of length <paramref name="periodLength"/>, and then calculates
    /// the beginning of the period after the period active at the current time.
    /// Beginning of the first period is considered to be midnight, Sunday.
    /// Periods include the first tick (index == 0) and exclude the the first tick of the next period (index &lt; periodLengthInMinutes * TimeSpan.TicksPerMinute)
    /// If the current time is at an exact period start, the same value is returned
    /// Daylight savings adjustments for the given <paramref name="timeZone"/> are respected.
    /// Compute intensive. Do not use in a hot path.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public TimeStamp ToPeriodOfWeekCeiling(in TimeSpan periodLength, in TimeSpan periodOffset, TimeZoneInfo timeZone)
      => new TimeStamp(AsTicks(timeZone).ToPeriodOfWeekCeiling(periodLength.Ticks, periodOffset.Ticks), timeZone);

    /// <summary>
    /// Divides the week into periods of length <paramref name="periodLength"/>, and then calculates
    /// the zero-based index of the period at the current time.
    /// Beginning of the first period is considered to be midnight, Sunday.
    /// Periods include the first tick (index == 0) and exclude the the first tick of the next period (index &lt; periodLength)
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int ToPeriodOfWeekIndex(in TimeSpan periodLength, TimeZoneInfo timeZone)
      => AsTicks(timeZone).ToPeriodOfWeekIndex(periodLength.Ticks);

    /// <summary>
    /// Divides the week into periods of length <paramref name="periodLength"/>, and then calculates
    /// the zero-based index of the period at the current time.
    /// Beginning of the first period is considered to be midnight, Sunday.
    /// Periods include the first tick (index == 0) and exclude the the first tick of the next period (index &lt; periodLength)
    /// Daylight savings adjustments for the given <paramref name="timeZone"/> are respected.
    /// Compute intensive. Do not use in a hot path.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int ToPeriodOfWeekIndex(in TimeSpan periodLength, in TimeSpan periodOffset, TimeZoneInfo timeZone)
      => AsTicks(timeZone).ToPeriodOfWeekIndex(periodLength.Ticks, periodOffset.Ticks);
  }
}
