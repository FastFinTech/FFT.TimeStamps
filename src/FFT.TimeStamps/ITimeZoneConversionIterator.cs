using System;

namespace FFT.TimeStamps
{
  /// <summary>
  /// Provides the <see cref="DifferenceTicks"/> used to convert from <see cref="FromTimeZone"/> to <see cref="ToTimeZone"/>
  /// at the time given in the last call to <see cref="MoveTo(in long)"/>.
  /// </summary>
  public interface ITimeZoneConversionIterator
  {
    /// <summary>
    /// The timezone that we are converting from.
    /// </summary>
    TimeZoneInfo FromTimeZone { get; }

    /// <summary>
    /// The timezone that we are converting tp.
    /// </summary>
    TimeZoneInfo ToTimeZone { get; }

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
    /// Advances the <see cref="ITimeZoneConversionIterator"/> with an internal call to <see cref="MoveTo(in long)"/>,
    /// then performs a conversion and returns the result expressed in ticks in the <see cref="ToTimeZone"/>.
    /// </summary>
    /// <param name="fromTimeZoneTicks">The time in <see cref="FromTimeZone"/> expressed in ticks.</param>
    long GetTicks(in long fromTimeZoneTicks);

    /// <summary>
    /// Advances the <see cref="ITimeZoneConversionIterator"/> with an internal call to <see cref="MoveTo(in long)"/>,
    /// then performs a conversion and returns the result as a <see cref="DateTime"/> in the <see cref="ToTimeZone"/>
    /// with its <see cref="DateTime.Kind"/> property set to <see cref="DateTimeKind.Unspecified"/>.
    /// </summary>
    /// <param name="fromTimeZoneTicks">The time in <see cref="FromTimeZone"/> expressed in ticks.</param>
    DateTime GetDateTime(in long fromTimeZoneTicks);

    /// <summary>
    /// Advances the <see cref="ITimeZoneConversionIterator"/> with an internal call to <see cref="MoveTo(in long)"/>,
    /// then performs a conversion and returns the result as a <see cref="DateTimeOffset"/> in the <see cref="ToTimeZone"/>
    /// with its <see cref="DateTimeOffset.Offset"/> property set to the offset of <see cref="ToTimeZone"/>.
    /// </summary>
    /// <param name="fromTimeZoneTicks">The time in <see cref="FromTimeZone"/> expressed in ticks.</param>
    DateTimeOffset GetDateTimeOffset(in long fromTimeZoneTicks);
  }
}
