# FFT.TimeStamps

[![Source code](https://img.shields.io/static/v1?style=flat&label=&message=Source%20Code&logo=read-the-docs&color=informational)](https://github.com/FastFinTech/FFT.TimeStamps)
[![NuGet package](https://img.shields.io/nuget/v/FFT.TimeStamps.svg)](https://nuget.org/packages/FFT.TimeStamps)
[![Full documentation](https://img.shields.io/static/v1?style=flat&label=&message=Documentation&logo=read-the-docs&color=green)](https://fastfintech.github.io/FFT.TimeStamps/)

Use this library to get extremely fast timestamping with:
1. Fast, unambiguous representation of exact times in multiple timezones.
1. Fast comparisons of timestamps in multiple timezones.
1. Fast arithmetic operations on timestamps.
1. Fast timezone conversion.
1. Fast, unambiguous timestamp serialization.
1. A broad range of time calculation and comparison utilities that are particularly useful within FinTech applications.

### Brief overview

- The [`TimeStamp`](https://fastfintech.github.io/FFT.TimeStamps/api/FFT.TimeStamps.TimeStamp.html) represents an exact, unambiguous moment in time.
- The [`DateStamp`](https://fastfintech.github.io/FFT.TimeStamps/api/FFT.TimeStamps.DateStamp.html) represents a particular date.
- The [`MonthStamp`](https://fastfintech.github.io/FFT.TimeStamps/api/FFT.TimeStamps.MonthStamp.html) represents a particular month.
- The [`TimeOfWeek`](https://fastfintech.github.io/FFT.TimeStamps/api/FFT.TimeStamps.TimeOfWeek.html) represents a moment that occurs every week, 10am Monday, for example.
- The [`TimeZoneCalculator`](https://fastfintech.github.io/FFT.TimeStamps/articles/timezoneCalculator.html) provides extremely fast timezone conversions using cached conversion offset records.
- The various [Conversion iterators](https://fastfintech.github.io/FFT.TimeStamps/articles/conversionIterators.html) provide the fastest-possible way to perform timezone conversions on a stream of chronological-order times. They are also the best way to transform streams of TimeStamp objects to DateTime or DateTimeOffset objects or vice versa.



### Background information

`FFT.TimeStamps` was developed in response to needs encountered whilst developing financial applications for exchanges and traders.

Let's take for example a trading platform, which deals with time in at least these timezones simultaneously:
- The timezone that the user's computer is configured with.
- The timezone of the chart being displayed to the user.
- The timezone of each instrument's exchange.
- The timezone of each instrument's settlement time.
- The timezone of the trading session hours template applied to the chart.
- The timezone of the instrument's trading hours as defined by the exchange.
- The timezone in which historical market data is downloaded.
- The timezone used whilst storing market data to disk.
- The timezone of news events and other market events.
- The timezone of any custom session such as custom volume profile sessions.
- The timezone of exchange closures, particularly when the closure is only a partial day.
- The timezone used in data retrieved from external services.
- The timezone expected by external services consuming your data.
- etc.

Trading applications, especially those residing on a server, typically process millions of exchange events every second. Each event comes with a timestamp, and the trading application must perform cross-timezone calculations and comparisons for EVERY single event that comes through the data feeds.

