//using System;
//using System.Runtime.CompilerServices;

//namespace FFT.TimeStamps
//{
//  /// <summary>
//  /// Use this class to VERY quickly convert sequential, ascending-order TimeStamps into <see cref="DateTimeOffset"/> objects for a given <see cref="TimeZoneInfo"/>.
//  /// It will only work properly if you guarantee to call the Get method with ascending order TimeStamps.
//  /// </summary>
//  public sealed class SequentialConverterToDateTimeOffset
//  {
//    /// <summary>
//    /// The timezone that <see cref="TimeStamp"/>s will be converted to.
//    /// </summary>
//    public TimeZoneInfo TimeZone { get; }
//    private readonly ITimeZoneConversionIterator _iterator;

//    /// <summary></summary>
//    /// <param name="timeZone">The timezone that <see cref="TimeStamp"/>s will be converted to.</param>
//    public SequentialConverterToDateTimeOffset(TimeZoneInfo timeZone)
//    {
//      TimeZone = timeZone;
//      _iterator = TimeZoneConversionIterator.Create(TimeZoneInfo.Utc, timeZone);
//    }

//    /// <summary>
//    /// Converts the given timestamp to the <see cref="TimeZone"/>
//    /// </summary>
//    [MethodImpl(MethodImplOptions.AggressiveInlining)]
//    public DateTimeOffset Get(in TimeStamp timeStamp)
//      => Get(timeStamp.TicksUtc);

//    /// <summary>
//    /// Converts the given utc timestamp to the <see cref="TimeZone"/>
//    /// </summary>
//    [MethodImpl(MethodImplOptions.AggressiveInlining)]
//    public DateTimeOffset Get(in long ticksUtc)
//    {
//      _iterator.MoveTo(ticksUtc);
//      return new DateTimeOffset(ticksUtc + _iterator.DifferenceTicks, new TimeSpan(_iterator.DifferenceTicks));
//    }
//  }
//}
