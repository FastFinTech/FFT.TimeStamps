using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace FFT.TimeStamps
{
  public partial struct TimeStamp
  {
    /// <summary>
    /// Gets the time of day in the utc timezone.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public TimeSpan ToTimeOfDay()
      => TicksUtc.ToTimeOfDay();

    /// <summary>
    /// Gets the time of day in the given <paramref name="timeZone"/>.
    /// Compute intensive. Do not use in a hot path.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public TimeSpan ToTimeOfDay(TimeZoneInfo timeZone)
      => AsTicks(timeZone).ToTimeOfDay();
  }
}
