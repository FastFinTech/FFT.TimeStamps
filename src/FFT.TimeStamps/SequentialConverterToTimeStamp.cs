//using System;
//using System.Runtime.CompilerServices;

//namespace FFT.TimeStamps
//{
//  /// <summary>
//  /// Use this class to VERY quickly convert sequential, ascending-order DateTimes from a given TimeZone into TimeStamp objects.
//  /// It will only work properly if you guarantee to call the Get method with ascending order DateTimes.
//  /// </summary>
//  public class SequentialConverterToTimeStamp
//  {
//    /// <summary>
//    /// The timezone that <see cref="DateTime"/> objects will be converted from.
//    /// </summary>
//    public TimeZoneInfo TimeZone { get; }
//    private readonly ITimeZoneConversionIterator _iterator;

//    /// <summary></summary>
//    /// <param name="timeZone">The <see cref="TimeZoneInfo"/> that <see cref="DateTime"/> objects will be converted from.</param>
//    public SequentialConverterToTimeStamp(TimeZoneInfo timeZone)
//    {
//      TimeZone = timeZone;
//      _iterator = TimeZoneConversionIterator.Create(timeZone, TimeZoneInfo.Utc);
//    }

//    /// <summary>
//    /// Converts the given timezone time to a <see cref="TimeStamp"/>.
//    /// It will only work properly if you guarantee to call this method with ascending order <paramref name="atTimeZone"/>.
//    /// IMPORTANT! => The <see cref="DateTime.Kind"/> property is ignored.
//    /// IMPORTANT! => No checking is done to make sure you are calling this method with INCREASING <paramref name="atTimeZone"/>. 
//    ///               That's your responsibility, and if you don't, you will get unexpected results.
//    /// </summary>
//    [MethodImpl(MethodImplOptions.AggressiveInlining)]
//    public TimeStamp Get(in DateTime atTimeZone)
//      => Get(atTimeZone.Ticks);

//    /// <summary>
//    /// Converts the given timezone time to a <see cref="TimeStamp"/>.
//    /// It will only work properly if you guarantee to call this method with ascending order <paramref name="ticksTimeZone"/>
//    /// or you will get unexpected results.
//    /// </summary>
//    [MethodImpl(MethodImplOptions.AggressiveInlining)]
//    public TimeStamp Get(in long ticksTimeZone)
//    {
//      _iterator.MoveTo(ticksTimeZone);
//      return new TimeStamp(ticksTimeZone + _iterator.DifferenceTicks);
//    }
//  }
//}
