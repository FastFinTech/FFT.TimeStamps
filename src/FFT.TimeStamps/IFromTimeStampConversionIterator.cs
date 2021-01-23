// Copyright (c) True Goodwill. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace FFT.TimeStamps
{
  using System;

  /// <summary>
  /// Provides the <see cref="DifferenceTicks"/> used to convert from a <see cref="TimeStamp"/> to <see cref="ToTimeZone"/>
  /// at the time given in the last call to <see cref="MoveTo(long)"/>.
  /// Also provides methods to directly convert a <see cref="TimeStamp"/> to a <see cref="DateTime"/> or <see cref="DateTimeOffset"/>.
  /// IMPORTANT!! Inputs must be in SEQUENTIAL ascending chronological order to get correct results from a conversion iterator.
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
    bool MoveTo(long utcTicks);

    /// <summary>
    /// Advances the <see cref="IFromTimeStampConversionIterator"/> with an internal call to <see cref="MoveTo(long)"/>,
    /// then performs a conversion and returns the result as a <see cref="DateTime"/>.
    /// </summary>
    /// <param name="timeStamp">The time to be converted.</param>
    DateTime GetDateTime(TimeStamp timeStamp);

    /// <summary>
    /// Advances the <see cref="IFromTimeStampConversionIterator"/> with an internal call to <see cref="MoveTo(long)"/>,
    /// then performs a conversion and returns the result as a <see cref="DateTimeOffset"/>.
    /// </summary>
    /// <param name="timeStamp">The time to be converted.</param>
    DateTimeOffset GetDateTimeOffset(TimeStamp timeStamp);

    /// <summary>
    /// Advances the <see cref="IFromTimeStampConversionIterator"/> with an internal call to <see cref="MoveTo(long)"/>,
    /// then performs a conversion and returns the result as ticks in <see cref="ToTimeZone"/>.
    /// </summary>
    /// <param name="timeStamp">The time to be converted.</param>
    long GetTicks(TimeStamp timeStamp);
  }
}
