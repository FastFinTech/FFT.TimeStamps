using System;

namespace FFT.TimeStamps
{
  /// <summary>
  /// Provides the <see cref="DifferenceTicks"/> used to convert from <see cref="FromTimeZone"/> to Utc timezone
  /// at the time given in the last call to <see cref="MoveTo(in long)"/>.
  /// Also provides the direct <see cref="GetTimeStamp(in long)"/> method.
  /// IMPORTANT!! Inputs must be in SEQUENTIAL ascending chronological order to get correct results from a conversion iterator!
  /// </summary>
  public interface IToTimeStampConversionIterator
  {
    /// <summary>
    /// The timezone that we are converting from.
    /// </summary>
    TimeZoneInfo FromTimeZone { get; }

    /// <summary>
    /// The difference in ticks for a direct conversion from <see cref="FromTimeZone"/> to <see cref="ToTimeZone"/>.
    /// IMPORTANT!! NOT to be confused with the utc offset for either timezone.
    /// </summary>
    long DifferenceTicks { get; }

    /// <summary>
    /// Advances the <see cref="ITimeZoneConversionIterator"/> to the given time expressed as ticks in <see cref="FromTimeZone"/>.
    /// Returns true if <see cref="DifferenceTicks"/> has changed since the last advance, false otherwise.
    /// </summary>
    bool MoveTo(in long fromTimeZoneTicks);

    /// <summary>
    /// Advances the <see cref="IToTimeStampConversionIterator"/> with an internal call to <see cref="MoveTo(in long)"/>,
    /// then performs a conversion and returns the result as a <see cref="TimeStamp"/>.
    /// </summary>
    /// <param name="fromTimeZoneTicks">The time in <see cref="FromTimeZone"/> expressed in ticks.</param>
    TimeStamp GetTimeStamp(in long fromTimeZoneTicks);

    TimeStamp GetTimeStamp(in DateTime fromTimeZone);
  }
}
