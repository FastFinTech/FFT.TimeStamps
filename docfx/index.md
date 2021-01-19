# FFT.TimeStamps

[![NuGet package](https://img.shields.io/nuget/v/FFT.TimeStamps.svg)](https://nuget.org/packages/FFT.TimeStamps)
[![Source code](https://img.shields.io/static/v1?style=flat&label=&message=Source%20code&logo=read-the-docs&color=informational)](https://github.com/FastFinTech/FFT.TimeStamps)

Use this to get extremely fast timestamping with:
1. Fast, unambiguous representation of exact times in multiple timezones.
1. Fast comparisons of timestamps in multiple timezones.
1. Fast arithmetic operations on timestamps.
1. Fast timezone conversion.
1. Fast, unambiguous timestamp serialization.

### Brief overview

- The [`TimeStamp`](xref:FFT.TimeStamps.TimeStamp) represents an exact, unambiguous moment in time.
- The [`DateStamp`](xref:FFT.TimeStamps.DateStamp) represents a particular date.
- The [`MonthStamp`](xref:FFT.TimeStamps.MonthStamp) represents a particular month.
- The [`TimeOfWeek`](xref:FFT.TimeStamps.TimeOfWeek) represents a moment that occurs every week, 10am Monday, for example.
- The [`TimeZoneOffsetCalculator`](xref:FFT.TimeStamps.TimeZoneOffsetCalculator) provides extremely fast timezone conversions using cached conversion offset records.
- The [`ITimeStampConversionIterator`](xref:FFT.TimeStamps.ITimeStampConversionIterator) provides the fastest possible way to convert a sequential stream of `DateTime` objects to their equivalent `TimeStamp`. You would use this when processing the data from an external data source.
- The [`ITimeZoneConversionIterator`](xref:FFT.TimeStamps.ITimeZoneConversionIterator) provides the fastest possible opposite-direction conversion of a sequential stream of `TimeStamp` objects to their equivalent `DateTime` or `DateTimeOffset` counterparts in a given timezone. You would use this when converting internal data to a traditional format expected by an external consumer.

### Background information

This library was developed in response to needs encountered whilst developing financial applications for exchanges and traders.

Let's take for example a trading platform, which deals with time in at least these timezones simultaneously:
- The timezone that the user's computer is configured with.
- The timezone of the chart being displayed to the user.
- The timezone of each instrument's exchange.
- The timezone of each instrument's settlement time.
- The timezone of the trading session template applied to the chart.
- The timezone of the instrument's trading hours as defined by the exchange.
- The timezone in which historical market data is downloaded.
- The timezone used whilst storing market data to disk.
- The timezone of news events and other market events.
- The timezone of any custom session such as custom volume profile sessions.
- The timezone used in data retrieved from external services.
- The timezone expected by external services consuming your data.
- etc.

Trading applications, especially those residing on a server, typically process millions of exchange events every second. Each event comes with a timestamp attached, and the trading application must perform cross-timezone calculations and comparisons for EVERY single event that comes through the data feeds.

Your application code becomes faster, simpler, and easier when you can boil every time expression down to a single paradigm with extremely fast comparison operators -- the `TimeStamp`.

When you are able to express all events or moments in time with the ubiquitous `Timestamp` instead of the confusingly-timezoned `DateTime` objects, your comparisons/calculations become extremely fast and simple, and your code becomes very easy to read and maintain.

For the relatively rare times that the `TimeStamp` must be displayed to the user as a formatted string, you can use its `ToString("format", timeZone)` method which performs the timezone conversion and string formatting for you.

Timezone conversions are performed much faster in this library than using the conventional `TimeZoneInfo.Convert` methods. For extremely fast conversion of sequential-order timestamps, the `ConversionIterator` utilities are provided.

**Converting to various timezones.**

```csharp
TimeStamp someTimeStamp = TimeStamp.Now.AddHours(-200); // just a random time.
DateTimeOffset utcOffset = someTimeStamp.AsUtc(); // Expressed in UTC timezone.
DateTimeOffset localOffset = someTimeStamp.AsLocal(); // Expressed in local timezone.
DateTimeOffset estOffset = someTimeStamp.As(Apex.TimeZones.TimeZoneReferences.EasternStandardTime); // Expressed in Eastern Standard timezone with daylight savings applied.
```

**Extremely rapid conversions**

Since a lot of what is done in the trading platform is with sequentially-increasing timestamps, eg, a tick data stream, further optimization has been provided for forward-only sequential timezone conversion, both from a timestamp and to a timestamp, using the `SequentialConverterToDateTime` and `SequentialConverterToTimeStamp` objects.

```csharp
/// Example of converting TimeStamps to DateTimes in the local time zone
/// using the extremely-fast SequentialConverterToDateTime object, which 
/// only works properly when the input TimeStamps are in ascending order.
var converter = new SequentialConverterToDateTime(TimeZoneInfo.Local);
foreach(var tick in tickStream) { 
	var localTime = converter.Get(tick.TimeStamp); // VERY fast conversion
}

/// Reverse conversion example.
var reverseConverter = new SequentialConverterToTimeStamp(TimeZoneInfo.Local);
foreach(var dateTime in someStream) { 
    var timeStamp = converter.Get(dateTime); // VERY fast conversion.
}
```

# Removing ambiguity
`DateTime` objects create ambiguity and impossible times when passing sequentially through time.

For example, when *Eastern Standard Time* daylight savings comes to an end at 2am, the clock flies back to 1am. Therefore, the time `1:30am` is **ambiguous**. It could refer to the either of two moments in real time. It's impossible to step a `DateTime` object sequentially through time, minute by minute or second by second during this ambiguous period, without making computational compensation for the issue.

Additionally, when *Eastern Standard Time* daylight savings begins at 2am, the click flies forward to 3am. Therefore the time `2:30am` is **impossible** ... it never exists at all! But the `DateTime` object will happily step forward through impossible times without ever giving the using code any warning that the computational results are going to be wrong.

On the other hand, a `TimeStamp` object can be stepped sequentially though all of time without any kind of confusion.

```csharp
var est = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");
var start = new TimeStamp(new DateTime(2019, 3, 10, 2, 0, 0, DateTimeKind.Utc).Ticks, est); // At 2am, the clock flies forward to 3am
var end = new TimeStamp(new DateTime(2019, 11, 3, 2, 0, 0, DateTimeKind.Utc).Ticks, est); // At 2am, the clock flies backward to 1am.
var start = TimeStamp.Now.ToAbsoluteMinuteFloor();
var end = TimeStamp.AddAbsoluteDays(10);
for(var time = start; time <= end; time = time.AddAbsoluteMinutes(1)) { 
    // Do something with an unambiguous moment in time.
    // In this example, the console output would clearly show repeated (ambiguous) times as well as skipped (impossible) times.
    Console.WriteLine(time.As(est).ToString("yyyy-MM-dd HH:mm:ss");
}
```

# Solving serialization issues

Another issue solved by the `TimeStamp` is the issue of serialization and deserialization. JSON itself doesn't have a well-defined standard for DateTimes, and this leads to difficulty with interpretation of the `DateTime.Kind` property. A `DateTime` serialized as utc can appear on the destination computer with `Kind` property set to `Local`, leading to unexpected behaviours. This is just one of many examples of DateTime serialization failing us because of the ambiguity of the `DateTime` object.

For example, a `DateTime` object with `Kind == Utc` might be serialized by a server as `2020-05-20T00:00:00+00.00`, and appear when deserialized as a `DateTime` object with a value of `2020-05-19T20:00:00-04:00` and `Kind == Local`. This kind of thing has caused issues many times due to its unexpected nature and the intended use of the deserialized object.

For `DateTime` serialization to work as intended, developers must be extremely careful about setting up serialization/deserialization settings on both ends of the communication, and make sure that tests are setup to fail to prevent future coders from accidentally breaking things when they adjust serializer settings for some totally unrelated purpose. For `TimeStamp` serialization to work as intended, NOTHING needs to be done. It just works perfectly.

`TimeStamps` are not ambiguous, and they serialize exactly. They also serialize to perfect resolution (the tick, one ten-millionth of a second), unlike the `DateTime`, which serializes usually only to the millisecond or worse, depending on serializer settings.

# DateStamp and MonthStamp
The `DateStamp` and `MonthStamp` are intended to remove ambiguity when expressing a date or a month that is not a moment in time, but a particular date or month in the Gregorian calendar. Using these objects prevents bugs caused by many factors not limited to serialization/deserialization ambiguity, individual PC locale settings, and programmer confusion.

# Other ways the library improved using code.

Before I introduced this object to the toolkit, our trading platform objects were each plastered with multiple `DateTime` properties, each property expressing the same moment in a different timezone. It was horrible to read, code, and maintain, and program execution was much slower.

Using the `TimesStamp`, we have made our time-related code now easier to read and maintain. Program execution is also much faster, since we are dealing with streams of millions of time-related objects such as market tick data, and performing time comparisons on each of them as they are processed.

# See also

NodaTime is a very good library written by the famous John Skeet.
`TimeStamp` objects suit the trading platform's requirements better though. 