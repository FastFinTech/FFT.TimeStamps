// Copyright (c) True Goodwill. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace FFT.TimeStamps
{
  using System;

  /// <summary>
  /// Provides methods for creating <see cref="ITimeZoneConversionIterator"/> and <see cref="IToTimeStampConversionIterator"/>.
  /// </summary>
  public sealed partial class ConversionIterators
  {
    /// <summary>
    /// Creates a <see cref="ITimeZoneConversionIterator"/> that is capable of providing the
    /// offset of a converstion between <paramref name="fromTimeZone"/> and <paramref name="toTimeZone"/>.
    /// IMPORTANT!! Inputs must be in SEQUENTIAL ascending chronological order to get correct results from a conversion iterator.
    /// </summary>
    public static ITimeZoneConversionIterator Create(TimeZoneInfo fromTimeZone, TimeZoneInfo toTimeZone)
    {
      if (fromTimeZone == toTimeZone) throw new ArgumentException("The given timezones should be different");
      if (fromTimeZone == TimeZoneInfo.Utc) return new FromUtcIterator(toTimeZone);
      if (toTimeZone == TimeZoneInfo.Utc) return new ToUtcIterator(fromTimeZone);
      return new DualTimeZoneIterator(fromTimeZone, toTimeZone);
    }

    /// <summary>
    /// Creates an <see cref="IToTimeStampConversionIterator"/> that converts sequential <see cref="DateTime"/> and <see cref="DateTimeOffset"/> values into
    /// their equivalant <see cref="TimeStamp"/> values.
    /// IMPORTANT!! Inputs must be in SEQUENTIAL ascending chronological order to get correct results from a conversion iterator.
    /// </summary>
    public static IToTimeStampConversionIterator ToTimeStamp(TimeZoneInfo fromTimeZone)
    {
      if (fromTimeZone == TimeZoneInfo.Utc) throw new ArgumentException("There is no point using a converter to convert from utc.", nameof(fromTimeZone));
      return new ToUtcIterator(fromTimeZone);
    }

    /// <summary>
    /// Creates an <see cref="IFromTimeStampConversionIterator"/> that converts <see cref="TimeStamp"/> values into
    /// their equivalant <see cref="DateTime"/> and <see cref="DateTimeOffset"/> values.
    /// IMPORTANT!! Inputs must be in SEQUENTIAL ascending chronological order to get correct results from a conversion iterator.
    /// </summary>
    public static IFromTimeStampConversionIterator FromTimeStamp(TimeZoneInfo toTimeZone)
    {
      if (toTimeZone == TimeZoneInfo.Utc) throw new ArgumentException("There is no point using a converter to convert to utc.", nameof(toTimeZone));
      return new FromUtcIterator(toTimeZone);
    }
  }
}
