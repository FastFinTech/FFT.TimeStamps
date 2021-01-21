using System;
using System.Runtime.CompilerServices;

namespace FFT.TimeStamps
{
  public partial struct TimeStamp
  {
    /// <summary>
    /// Calculates a DateTimeOffset adjusted for the given timeZone.
    /// Compute intensive. Don't use in a hot path.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public DateTimeOffset As(TimeZoneInfo timeZone)
    {
      var offsetTicks = TimeZoneCalculator.Get(timeZone).GetSegment(this).OffsetTicks;
      return new DateTimeOffset(TicksUtc + offsetTicks, new TimeSpan(offsetTicks));
    }

    /// <summary>
    /// Creates a <see cref="DateTimeOffset"/> object offset for the local timezone.
    /// Compute intensive. Don't use in a hot path.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public DateTimeOffset AsLocal()
      => As(TimeZoneInfo.Local);

    /// <summary>
    /// Creates a <see cref="DateTimeOffset"/> object offset with zero (UTC) offset.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public DateTimeOffset AsUtc()
      => new DateTimeOffset(TicksUtc, TimeSpan.Zero);

    /// <summary>
    /// Creates a <see cref="DateTimeOffset"/> object offset with the given offset.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public DateTimeOffset AsOffset(in TimeSpan offset)
      => new DateTimeOffset(TicksUtc + offset.Ticks, offset);

    /// <summary>
    /// Gets the Ticks property of a clock in the given <paramref name="timeZone"/> at the current moment.
    /// Compute intensive. Don't use in a hot path.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public long AsTicks(TimeZoneInfo timeZone)
        => TicksUtc + TimeZoneCalculator.Get(timeZone).GetSegment(this).OffsetTicks;

    /// <summary>
    /// Gets the Ticks property of a clock in the local time zone at the current moment.
    /// Compute intensive. Don't use in a hot path.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public long AsLocalTicks()
      => AsTicks(TimeZoneInfo.Local);
  }
}
