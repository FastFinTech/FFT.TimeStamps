


Your application code becomes faster, simpler, and easier when you can boil every time expression down to a single paradigm with extremely fast comparison operators -- the `TimeStamp`.

When you are able to express all events or moments in time with the ubiquitous `Timestamp` instead of the confusingly-timezoned `DateTime` objects, your comparisons/calculations become extremely fast and simple, and your code becomes very easy to read and maintain.

For the relatively rare times that the `TimeStamp` must be displayed to the user as a formatted string, you can use its `ToString("format", timeZone)` method which performs the timezone conversion and string formatting for you.

Timezone conversions are performed much faster in this library than using the conventional `TimeZoneInfo.Convert` methods. For extremely fast conversion of sequential-order timestamps, the `ConversionIterator` utilities are provided.

**Converting to various timezones.**

```csharp
TimeZoneInfo newYork = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");
TimeStamp someTimeStamp = TimeStamp.Now.AddHours(-200); // just a random time.
DateTimeOffset utcOffset = someTimeStamp.AsUtc(); // Expressed in UTC timezone.
DateTimeOffset localOffset = someTimeStamp.AsLocal(); // Expressed in local timezone.
DateTimeOffset estOffset = someTimeStamp.As(newYork); // Expressed in Eastern Standard timezone with daylight savings applied.
```

**Extremely rapid conversions**

Since a lot of what is done in a trading application is with sequentially-increasing timestamps, eg, a tick data stream, further optimization has been provided for forward-only sequential timezone conversion, both from a timestamp and to a timestamp, using the `Conversion iterator` objects.

```csharp
/// Example of converting TimeStamps to various other time objects in the local time zone
/// using the extremely-fast IFromTimeStampConversionIterator, which 
/// only works properly when the input TimeStamps are in ascending order.
IFromTimeStampConversionIterator converter = ConversionIterators.FromTimeStamp(toTimeZone: TimeZoneInfo.Local)
foreach(var tick in tickStream) { 
	DateTime localTime = converter.GetDateTime(tick.TimeStamp); // VERY fast conversion
    DateTimeOffset localTimeOffset = converter.GetDateTimeOffset(tick.TimeStamp);
    long localTimeTicks = converter.GetTicks(tickTimeStamp);
}

/// Reverse conversion example.
TimeZoneInfo newYork = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");
IToTimeStampConversionIterator reverseConverter = ConversionIterators.TimeStamp(fromTimeZone: newYork);
foreach(var dateTime in someStream) { 
    var timeStamp = converter.GetTimeStamp(dateTime.Ticks); // VERY fast conversion.
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

# See also

NodaTime is a very good library written by the famous John Skeet.
`TimeStamp` objects suit the trading platform's requirements better though. 

<script src="https://ajax.googleapis.com/ajax/libs/jquery/3.5.1/jquery.min.js"></script>
<script lang="JavaScript">
    $(window).on('load', function(){
        alert('iframe stuff loaded');
        if (parent) { 
            alert('posting message 2');
            parent.contentWindow.postMessage({}, '*');
        }
    });
  window.addEventListener('message', event => {
      alert('received message');
    // // IMPORTANT: check the origin of the data! 
    // if (event.origin.startsWith('http://your-first-site.com')) { 
    //     // The data was sent from your site.
    //     // Data sent with postMessage is stored in event.data:
    //     console.log(event.data); 
    // } else {
    //     // The data was NOT sent from your site! 
    //     // Be careful! Do not use it. This else branch is
    //     // here just for clarity, you usually shouldn't need it.
    //     return; 
    // } 
  }); 
</script>