// Copyright (c) True Goodwill. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace FFT.TimeStamps
{
  using System;
  using System.Runtime.CompilerServices;

  public partial struct TimeStamp
  {
    /// <summary>
    /// Rounds the given timestamp down to the start of the millisecond, in absolute time.
    /// If the timestamp is at an exact millisecond, it returns a copy of the same timestamp.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public TimeStamp ToMillisecondFloor()
      => new TimeStamp(TicksUtc.ToMillisecondFloor());

    /// <summary>
    /// Rounds the given timestamp up to the start of the next millisecond, in absolute time.
    /// If the timestamp is at an exact millisecond, it returns a copy of the same timestamp.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public TimeStamp ToMillisecondCeiling()
      => new TimeStamp(TicksUtc.ToMillisecondsCeiling());

    /// <summary>
    /// Rounds the given timestamp down to the start of the second, in absolute time.
    /// If the timestamp is at an exact second, it returns a copy of the same timestamp.
    /// Time zone is completely ignored.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public TimeStamp ToSecondFloor()
      => new TimeStamp(TicksUtc.ToSecondFloor());

    /// <summary>
    /// Rounds the given timestamp up to the start of the next second, in absolute time.
    /// If the timestamp is at an exact second, it returns a copy of the same timestamp.
    /// Time zone is completely ignored.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public TimeStamp ToSecondCeiling()
      => new TimeStamp(TicksUtc.ToSecondCeiling());

    /// <summary>
    /// Rounds the given timestamp down to the start of the minute, in absolute time.
    /// If the timestamp is at an exact minute, it returns a copy of the same timestamp.
    /// Time zone is completely ignored.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public TimeStamp ToMinuteFloor()
      => new TimeStamp(TicksUtc.ToMinuteFloor());

    /// <summary>
    /// Rounds the given timestamp up to the start of the next minute, in absolute time.
    /// If the timestamp is at an exact minute, it returns a copy of the same timestamp.
    /// Time zone is completely ignored.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public TimeStamp ToMinuteCeiling()
      => new TimeStamp(TicksUtc.ToMinuteCeiling());

    /// <summary>
    /// Rounds the given timestamp down to the start of the hour, in absolute time.
    /// If the timestamp is at an exact hour, it returns a copy of the same timestamp.
    /// Time zone is completely ignored.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public TimeStamp ToHourFloor()
      => new TimeStamp(TicksUtc.ToHourFloor());

    /// <summary>
    /// Rounds the given timestamp up to the start of the next hour, in absolute time.
    /// If the timestamp is at an exact hour, it returns a copy of the same timestamp.
    /// Time zone is completely ignored.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public TimeStamp ToHourCeiling()
      => new TimeStamp(TicksUtc.ToHourCeiling());

    /// <summary>
    /// Rounds the given timestamp down to the start of the day, in absolute time.
    /// If the timestamp is at an exact day, it returns a copy of the same timestamp.
    /// Time zone is completely ignored.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public TimeStamp ToDayFloor()
      => new TimeStamp(TicksUtc.ToDayFloor());

    /// <summary>
    /// Rounds the given timestamp up to the start of the next day, in absolute time.
    /// If the timestamp is at an exact day, it returns a copy of the same timestamp.
    /// Time zone is completely ignored.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public TimeStamp ToDayCeiling()
      => new TimeStamp(TicksUtc.ToDayCeiling());

    /// <summary>
    /// Rounds the given timestamp down to the start of the day, in absolute time.
    /// If the timestamp is at an exact day, it returns a copy of the same timestamp.
    /// Time zone is completely ignored.
    /// Week floor is considered to be midnight, Sunday.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public TimeStamp ToWeekFloor()
      => new TimeStamp(TicksUtc.ToWeekFloor());

    /// <summary>
    /// Rounds the given timestamp up to the start of the next week, in absolute time.
    /// If the timestamp is at an exact week, it returns a copy of the same timestamp.
    /// Time zone is completely ignored.
    /// Week ceiling is considered to be midnight, Sunday.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public TimeStamp ToWeekCeiling()
      => new TimeStamp(TicksUtc.ToWeekCeiling());

    /************************************************************************************
     * It was decided not to provide "Clock" methods for minute, second, and millisecond
     * floors and ceilings, because timezone changes do not affect the clock at this level.
     * Hour floor and ceiling was included because some timezones have adjustments of only half an hour.
     ************************************************************************************/

    /// <summary>
    /// Rounds the given timestamp down to the start of the hour for a clock in the given <paramref name="timeZone"/>.
    /// If the current timestamp is already at the exact start of the hour, a copy of the same timestamp is returned.
    /// Compute intensive. Do not use in a hot path.
    /// </summary>
    /// <returns>A <see cref="TimeStamp"/> representing the calculated moment in time.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public TimeStamp ToHourFloor(TimeZoneInfo timeZone)
      => new TimeStamp(AsTicks(timeZone).ToHourFloor(), timeZone);

    /// <summary>
    /// Rounds the given timestamp up to the start of the hour for a clock in the given <paramref name="timeZone"/>.
    /// If the current timestamp is already at the exact start of the hour, a copy of the same timestamp is returned.
    /// Compute intensive. Do not use in a hot path.
    /// </summary>
    /// <returns>A <see cref="TimeStamp"/> representing the calculated moment in time.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public TimeStamp ToHourCeiling(TimeZoneInfo timeZone)
      => new TimeStamp(AsTicks(timeZone).ToHourCeiling(), timeZone);

    /// <summary>
    /// Rounds the given timestamp down to the start of the day for a clock in the given <paramref name="timeZone"/>.
    /// If the current timestamp is already at the exact start of the day, a copy of the same timestamp is returned.
    /// Compute intensive. Do not use in a hot path.
    /// </summary>
    /// <returns>A <see cref="TimeStamp"/> representing the calculated moment in time.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public TimeStamp ToDayFloor(TimeZoneInfo timeZone)
      => new TimeStamp(AsTicks(timeZone).ToDayFloor(), timeZone);

    /// <summary>
    /// Rounds the given timestamp up to the start of the day for a clock in the given <paramref name="timeZone"/>.
    /// If the current timestamp is already at the exact start of the day, a copy of the same timestamp is returned.
    /// Compute intensive. Do not use in a hot path.
    /// </summary>
    /// <returns>A <see cref="TimeStamp"/> representing the calculated moment in time.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public TimeStamp ToDayCeiling(TimeZoneInfo timeZone)
      => new TimeStamp(AsTicks(timeZone).ToDayCeiling(), timeZone);

    /// <summary>
    /// Rounds the given timestamp down to the start of the week for a clock in the given <paramref name="timeZone"/>.
    /// If the current timestamp is already at the exact start of the week, a copy of the same timestamp is returned.
    /// Start of the week is considered to be midnight, Sunday.
    /// Compute intensive. Do not use in a hot path.
    /// </summary>
    /// <returns>A <see cref="TimeStamp"/> representing the calculated moment in time.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public TimeStamp ToWeekFloor(TimeZoneInfo timeZone)
      => new TimeStamp(AsTicks(timeZone).ToWeekFloor(), timeZone);

    /// <summary>
    /// Rounds the given timestamp up to the start of the week for a clock in the given <paramref name="timeZone"/>.
    /// If the current timestamp is already at the exact start of the week, a copy of the same timestamp is returned.
    /// Start of the week is considered to be midnight, Sunday.
    /// Compute intensive. Do not use in a hot path.
    /// </summary>
    /// <returns>A <see cref="TimeStamp"/> representing the calculated moment in time.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public TimeStamp ToWeekCeiling(TimeZoneInfo timeZone)
      => new TimeStamp(AsTicks(timeZone).ToWeekCeiling(), timeZone);

    /// <summary>
    /// Rounds the given timestamp down to the start of the month for a clock in the given <paramref name="timeZone"/>.
    /// If the current timestamp is already at the exact start of the month, a copy of the same timestamp is returned.
    /// Start of the month is considered to be midnight of the first day of the month.
    /// Compute intensive. Do not use in a hot path.
    /// </summary>
    /// <returns>A <see cref="TimeStamp"/> representing the calculated moment in time.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public TimeStamp ToMonthFloor(TimeZoneInfo timeZone)
      => new TimeStamp(As(timeZone).DateTime.ToMonthFloor().Ticks, timeZone);

    /// <summary>
    /// Rounds the given timestamp up to the start of the month for a clock in the given <paramref name="timeZone"/>.
    /// If the current timestamp is already at the exact start of the month, a copy of the same timestamp is returned.
    /// Start of the month is considered to be midnight of the first day of the month.
    /// Compute intensive. Do not use in a hot path.
    /// </summary>
    /// <returns>A <see cref="TimeStamp"/> representing the calculated moment in time.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public TimeStamp ToMonthCeiling(TimeZoneInfo timeZone)
      => new TimeStamp(As(timeZone).DateTime.ToMonthCeiling().Ticks, timeZone);
  }
}
