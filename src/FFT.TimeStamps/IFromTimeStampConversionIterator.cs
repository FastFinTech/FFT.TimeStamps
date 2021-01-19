using System;

namespace FFT.TimeStamps
{
  /// <summary>
  /// Provides the <see cref="DifferenceTicks"/> used to convert from a <see cref="TimeStamp"/> to <see cref="ToTimeZone"/>
  /// at the time given in the last call to <see cref="MoveTo(in long)"/>.
  /// Also provides methods to directly convert a <see cref="TimeStamp"/> to a <see cref="DateTime"/> or <see cref="DateTimeOffset"/>.
  /// </summary>
  public interface IFromTimeStampConversionIterator
  {
    /// <summary>
    /// The timezone that we are converting to.
    /// </summary>
    TimeZoneInfo ToTimeZone { get; }

    /// <summary>
    /// The difference in ticks for a direct conversion from utc timezone to <see cref="ToTimeZone"/>.
    /// </summary>
    long DifferenceTicks { get; }

    /// <summary>
    /// Advances the <see cref="IFromTimeStampConversionIterator"/> to the given time expressed as ticks utc timezone.
    /// Returns true if <see cref="DifferenceTicks"/> has changed since the last advance, false otherwise.
    /// </summary>
    bool MoveTo(in long utcTicks);

    /// <summary>
    /// Advances the <see cref="IFromTimeStampConversionIterator"/> with an internal call to <see cref="MoveTo(in long)"/>,
    /// then performs a conversion and returns the result as a <see cref="DateTime"/>.
    /// </summary>
    /// <param name="timeStamp">The time to be converted.</param>
    DateTime GetDateTime(in TimeStamp timeStamp);

    /// <summary>
    /// Advances the <see cref="IFromTimeStampConversionIterator"/> with an internal call to <see cref="MoveTo(in long)"/>,
    /// then performs a conversion and returns the result as a <see cref="DateTimeOffset"/>.
    /// </summary>
    /// <param name="timeStamp">The time to be converted.</param>
    DateTimeOffset GetDateTimeOffset(in TimeStamp timeStamp);
  }
}
