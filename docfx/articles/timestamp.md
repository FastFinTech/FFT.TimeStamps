# TimeStamp

### Api specification links

The [`TimeStamp`](xref:FFT.TimeStamps.TimeStamp) gives you extremely fast timestamping, efficient comparison of times in differing zones bypassing timezone conversion calculations, and a large library of time-related methods that are heavily used by FinTech applications.

Related objects are the [`DateStamp`](xref:FFT.TimeStamps.DateStamp), the [`MonthStamp`](xref:FFT.TimeStamps.MonthStamp), the [`PeriodOfTime`](xref:FFT.TimeStamps.PeriodOfTime), and the [`TimeOfWeek`](xref:FFT.TimeStamps.TimeOfWeek).

### Basic usage

[`TimeStamp.Now`](xref:FFT.TimeStamps.TimeStamp.Now) gives you the current time, and there are a range of constructors as well.

#### ToString

There are a range of `ToString` methods available. The method with the most options allows you to define a timezone and a format string. 

[!code-csharp[Main](~/../src/FFT.TimeStamps.Examples/ToStringExamples.cs)]

#### Parsing strings

`FFT.TimeStamps` does not provide string parsing methods because the edge cases are broad and the possibility of you making mistakes because of a misunderstanding of the internal implemenation is high. Only you truly understand what various formats that you will need to parse, and what variants they come in. Therefore you should write your own string parsing code.

The below code shows an example of how you could parse a string to create a `TimeStamp`. 

The problem with the code below is that we never know what `System.DateTimeKind` property will be given to the `dateTime` object, because it changes depending on the input string. Therefore we don`t know if its `dateTime.Ticks` property will be expressed in local timezone, Utc timezone, or some other known timezone. The range of possibilities make it very difficult to include these kinds of methods in the library and still have a library that does exactly what its public api leads you to expect. Instead, the library gives you constructors that have a very clear intent and outcome.

```csharp
// Example of a parsing method that could be used by your application with a known
// range of inputs, but would cause unexpected results to many developers if included in the library.
public static bool TryParse(string value, TimeZoneInfo timeZone, out TimeStamp result)
{
  if (DateTime.TryParse(value, out var dateTime))
  {
    result = new TimeStamp(dateTime.Ticks, timeZone);
    return true;
  }

  result = default;
  return false;
}
```

#### Floor and Ceiling

The various floor and ceiling methods such as `TimeStamp.ToSecondFloor` allow you to round timestamp values to convenient intervals.

Some of the method overloads take a `TimeZoneInfo` parameter, allowing you to get a `TimeStamp` value that represents the rounded time interval in a particular timezone. 

For example, the method `TimeStamp.ToDayFloor(TimeZoneInfo timezone)` which returns the moment at midnight before the current value, would give very different results when passed different timezones.

Floor/Ceiling intervals provided in the library include: _millisecond, second, minute, hour, day, week, and month_.

#### GetNext and GetPrevious

These methods allow you to get the next or previous `TimeStamp` that occurs at a specific time of day or time of week. 

- [`GetPrevious(TimeSpan timeOfDay)`](xref:FFT.TimeStamps.TimeStamp.GetPrevious(System.TimeSpan))
- [`GetPrevious(TimeSpan timeOfDay, TimeZoneInfo timeZone)`](xref:FFT.TimeStamps.TimeStamp.GetPrevious(System.TimeSpan,System.TimeZoneInfo))
- [`GetNext(TimeSpan timeOfDay)`](xref:FFT.TimeStamps.TimeStamp.GetNext(System.TimeSpan))
- [`GetNext(TimeSpan timeOfDay, TimeZoneInfo timeZone)`](xref:FFT.TimeStamps.TimeStamp.GetNext(System.TimeSpan,System.TimeZoneInfo))
- [`GetPrevious(TimeOfWeek timeOfWeek)`](xref:FFT.TimeStamps.TimeStamp.GetPrevious(FFT.TimeStamps.TimeOfWeek))
- [`GetPrevious(TimeOfWeek timeOfWeek, TimeZoneInfo timeZone)`](xref:FFT.TimeStamps.TimeStamp.GetPrevious(FFT.TimeStamps.TimeOfWeek,System.TimeZoneInfo))
- [`GetNext(TimeOfWeek timeOfWeek)`](xref:FFT.TimeStamps.TimeStamp.GetNext(FFT.TimeStamps.TimeOfWeek))
- [`GetNext(TimeOfWeek timeOfWeek, TimeZoneInfo timeZone)`](xref:FFT.TimeStamps.TimeStamp.GetNext(FFT.TimeStamps.TimeOfWeek,System.TimeZoneInfo))

#### To index

These methods are helpful when aggregating timestamped data.

- [`ToSecondOfDayIndex()`](xref:FFT.TimeStamps.TimeStamp.ToSecondOfDayIndex)
- [`ToSecondOfDayIndex(TimeZoneInfo timeZone)`](xref:FFT.TimeStamps.TimeStamp.ToSecondOfDayIndex(System.TimeZoneInfo))
- [`ToSecondOfWeekIndex()`](xref:FFT.TimeStamps.TimeStamp.ToSecondOfWeekIndex)
- [`ToSecondOfWeekIndex(TimeZoneInfo timeZone)`](xref:FFT.TimeStamps.TimeStamp.ToSecondOfWeekIndex(System.TimeZoneInfo))
- [`ToMinuteOfDayIndex()`](xref:FFT.TimeStamps.TimeStamp.ToMinuteOfDayIndex)
- [`ToMinuteOfDayIndex(TimeZoneInfo timeZone)`](xref:FFT.TimeStamps.TimeStamp.ToMinuteOfDayIndex(System.TimeZoneInfo))
- [`ToMinuteOfWeekIndex()`](xref:FFT.TimeStamps.TimeStamp.ToMinuteOfWeekIndex)
- [`ToMinuteOfWeekIndex(TimeZoneInfo timeZone)`](xref:FFT.TimeStamps.TimeStamp.ToMinuteOfWeekIndex(System.TimeZoneInfo))

```csharp
// example of one possible use of an index:
// detecting when the timestamp has moved from one minute to the next
int previousMinuteIndex = -1;
foreach(TimeStamp time in someDataStream)
{
  int index = time.ToMinuteOfWeekIndex(someTimeZone);
  if (index != prevousMinuteIndex)
  {
    // it's the first timestamp of a new minute
    // do something 
    previousMinuteIndex = index;
  }
}
```

#### Period of week and fixed intervals

FinTech applications often need to compare data during a certain section of the week, eg 9am-10am on Monday morning, with data in the same period of the week in previous weeks. In particular, the application will need to make these calculations with sensitivity for timezone offset changes. The absolute time difference between 9am Monday from week to week might not always be exactly 7 x 24hrs due to timezone offset changes.

The general concept of a "period of week" is that the week can be broken into a number of evenly-spaced intervals, such as 10min, 1hr, 4hr, etc, and that data can be aggregated into summaries for each interval. `periodLength` is the length of the interval, and `periodOffset` is an optional "shift" that you can use to adjust the start/end points of each interval. For example, if you want to divide the week into hourly intervals starting at half-past the hour, such as 9:30 to 10:30, you would use `periodLength=1hr` and `periodOffset=30min`.

The following methods and their various overloads allow you to quickly correlate timestamps with the period of week that they fall within: 

- [`ToPeriodOfWeekFloor(TimeSpan periodLength, TimeZoneInfo timeZone)`](xref:FFT.TimeStamps.TimeStamp.ToPeriodOfWeekFloor(System.TimeSpan,System.TimeZoneInfo))
- [`ToPeriodOfWeekFloor(TimeSpan periodLength, TimeSpan periodOffset, TimeZoneInfo timeZone)`](xref:FFT.TimeStamps.TimeStamp.ToPeriodOfWeekFloor(System.TimeSpan,System.TimeSpan,System.TimeZoneInfo))
- [`ToPeriodOfWeekCeiling(TimeSpan periodLength, TimeZoneInfo timeZone)`](xref:FFT.TimeStamps.TimeStamp.ToPeriodOfWeekCeiling(System.TimeSpan,System.TimeZoneInfo))
- [`ToPeriodOfWeekCeiling(TimeSpan periodLength, TimeSpan periodOffset, TimeZoneInfo timeZone)`](xref:FFT.TimeStamps.TimeStamp.ToPeriodOfWeekCeiling(System.TimeSpan,System.TimeSpan,System.TimeZoneInfo))
- [`ToPeriodOfWeekIndex(TimeSpan periodLength, TimeZoneInfo timeZone)`](xref:FFT.TimeStamps.TimeStamp.ToPeriodOfWeekIndex(System.TimeSpan,System.TimeZoneInfo))
- [`ToPeriodOfWeekIndex(TimeSpan periodLength, TimeSpan periodOffset, TimeZoneInfo timeZone)`](xref:FFT.TimeStamps.TimeStamp.ToPeriodOfWeekIndex(System.TimeSpan,System.TimeSpan,System.TimeZoneInfo))

There is a [`PeriodOfWeekIterator`](xref:FFT.TimeStamps.PeriodOfWeekIterator) that allows you to track the period of the week when dealing with ascending chronological order timestamps.

Similar to period of week, **fixed intervals** allow you to break time into evenly-spaced intervals that are defined by a particular "start time" and "interval length".

- [`ToIntervalFloor(TimeStamp intervalStart, TimeSpan intervalLength)`](xref:FFT.TimeStamps.TimeStamp.ToIntervalFloor(FFT.TimeStamps.TimeStamp,System.TimeSpan))
- [`ToIntervalFloor(TimeStamp intervalStart, TimeSpan intervalLength, TimeZoneInfo timeZone)`](xref:FFT.TimeStamps.TimeStamp.ToIntervalFloor(FFT.TimeStamps.TimeStamp,System.TimeSpan,System.TimeZoneInfo))
- [`ToIntervalCeiling(TimeStamp intervalStart, TimeSpan intervalLength)`](xref:FFT.TimeStamps.TimeStamp.ToIntervalCeiling(FFT.TimeStamps.TimeStamp,System.TimeSpan))
- [`ToIntervalCeiling(TimeStamp intervalStart, TimeSpan intervalLength, TimeZoneInfo timeZone)`](xref:FFT.TimeStamps.TimeStamp.ToIntervalCeiling(FFT.TimeStamps.TimeStamp,System.TimeSpan,System.TimeZoneInfo))
- [`ToIntervalIndex(TimeStamp intervalStart, TimeSpan intervalLength)`](xref:FFT.TimeStamps.TimeStamp.ToIntervalIndex(FFT.TimeStamps.TimeStamp,System.TimeSpan))
- [`ToIntervalIndex(TimeStamp intervalStart, TimeSpan intervalLength, TimeZoneInfo timeZone)`](xref:FFT.TimeStamps.TimeStamp.ToIntervalIndex(FFT.TimeStamps.TimeStamp,System.TimeSpan,System.TimeZoneInfo))

```csharp
// example of one possible use of a period of week ceiling: 
// aggregating data by the period of week, using the ceiling as a dictionary key
var periodLength = TimeSpan.FromHours(1);
foreach(var item in someDataFeed)
{
  var key = item.Time.ToPeriodOfWeekCeiling(periodLength, someTimeZone);
  someDictionary[key] += item.value;
}
```