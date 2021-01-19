using System;

namespace FFT.TimeStamps
{
  /// <summary>
  /// Provides methods for creating <see cref="ITimeZoneConversionIterator"/> and <see cref="IToTimeStampConversionIterator"/>.
  /// </summary>
  public sealed partial class ConversionIterators
  {
    /// <summary>
    /// Creates a <see cref="ITimeZoneConversionIterator"/> that is capable of providing the
    /// offset of a converstion between <paramref name="fromTimeZone"/> and <paramref name="toTimeZone"/>.
    /// </summary>
    public static ITimeZoneConversionIterator Create(TimeZoneInfo fromTimeZone, TimeZoneInfo toTimeZone)
    {
      if (fromTimeZone == toTimeZone) throw new ArgumentException("The given timezones should be different");
      if (fromTimeZone == TimeZoneInfo.Utc) return new FromUtcIterator(toTimeZone);
      if (toTimeZone == TimeZoneInfo.Utc) return new ToUtcIterator(fromTimeZone);
      return new DualTimeZoneIterator(fromTimeZone, toTimeZone);
    }

    public static IToTimeStampConversionIterator ToTimeStamp(TimeZoneInfo fromTimeZone)
    {
      if (fromTimeZone == TimeZoneInfo.Utc) throw new ArgumentException("There is no point using a converter to convert from utc.", nameof(fromTimeZone));
      return new ToUtcIterator(fromTimeZone);
    }

    public static IFromTimeStampConversionIterator FromTimeStamp(TimeZoneInfo toTimeZone)
    {
      if (toTimeZone == TimeZoneInfo.Utc) throw new ArgumentException("There is no point using a converter to convert to utc.", nameof(toTimeZone));
      return new FromUtcIterator(toTimeZone);
    }
  }
}
