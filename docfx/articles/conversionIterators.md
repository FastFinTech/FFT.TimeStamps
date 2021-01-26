# Conversion Iterators

### Api specification links

The [`ConversionIterators`](xref:FFT.TimeStamps.ConversionIterators) class provides methods to create iterators of various types:

- [`IToTimeStampConversionIterator`](xref:FFT.TimeStamps.IToTimeStampConversionIterator)
- [`IFromTimeStampConversionIterator`](xref:FFT.TimeStamps.IFromTimeStampConversionIterator) Converts `TimeStamp` objects to times in a known timezone.
- [`ITimeZoneConversionIterator`](xref:FFT.TimeStamps.ITimeZoneConversionIterator) Converts time values from one timezone to another.

Conversion iterators provide the fastest-possible way to perform timezone conversions on a stream of chronological-order times. They are also the best way to transform streams of `TimeStamp` objects to `DateTime` or `DateTimeOffset` objects or vice versa.

[Benchmark testing](#benchmarking) shows that the Conversion iterators are 32 times faster at converting timezones than the built-in .net framework types.

### Basic usage

[!code-csharp[Conversion iterators example code](../../src/FFT.TimeStamps.Examples/ConversionIteratorExamples.cs)]

> [!WARNING]
> Conversion iterators require that the input times be supplied in ascending chronological order in order to provide correct conversion results. This requirement allows code optimization to make the conversion algorithm extremely efficient, but it also means you can get incorrect results if you accidentally supply inputs in descending chronological order. The iterators are coded to be fast, so they do not check YOUR work to ensure you have provided only ascending chronological order inputs.

>[!TIP]
> The exception to the warning rule above is for moments when a timezone clock flies back in time, typically from its advanced Daylight Savings offset to its normal Standard time offset. When the clock flies back in time, you will have to pass an out-of-chronological order timestamp to the Conversion iterator. The iterator has been coded to handle this correctly. See the [Ambiguous times](#ambiguous-times) section below for more information.

> [!TIP]
> Did you notice in the examples above that the conversion iterators can provide outputs in multiple formats (`long ticksTimezone`, `DateTime`, `DateTimeOffset`, and `TimeStamp`), but they accept inputs only in `long ticksTimezone`? 
>
> The conversion iterators should not accept `DateTime` objects as inputs because they can be ambiguous due to their `DateTimeKind` property, and checking this property and doing all the extra work would slow them down, not to mention introducing errors that you would only know to avoid if you read the documentation very carefully. They are meant to be fast and error-free. By accepting only `long ticks` inputs, Conversion iterators require you to think carefully when you are writing the code that uses them, and helps you avoid making mistakes from simply not knowing the internal implementation details.

### Ambiguous times

Imagine that at 3am in your timezone, the clock flies one hour back to 2am as Daylight Savings comes to an end and the Standard offset for your timezone is resumed. 

In this scenario, just on this day, the clock must complete the period of time from 2am to 3am TWICE on the same day! This causes ambiguity: On this particular day in this particular timezone, every time between 2am and 3am occurs twice, and has two equivalent UTC times.

Default behaviour in the .net framework classes and, therefore in `FFT.TimeStamps`, is to treat ambiguous times as though they are in the timezone's standard offset, which is the same as assuming that the clock is passing through the ambiguous period for the second time.

However, a conversion iterator must be able to handle the situation where times are streamed through it corresponding to the clock's first passage through the ambiguous period (with the timezone's Daylight Savings offset), and then to the clock's second passage through it (with the timezone's Standard Offset). When the conversion iterator detects the input flying back in time, it assumes that the clock is now passing through the ambiguous period for the second time.

Checkout the code for the [`ToUtcIterator`](https://github.com/FastFinTech/FFT.TimeStamps/blob/main/src/FFT.TimeStamps/ConversionIterators.ToUtcIterator.cs) class for an example of how to work with the [`TimeZoneCalculator`](timezoneCalculator.md) within ambiguous time periods.

>[!TIP]
> Ambiguous times happen when converting from timezones that adjustments such as daylight savings, but they never happen when converting from UTC or any timezone that does not change.

### Benchmarking

Converting 1 million sequential (in chronological order) times from UTC timezone to New York timezone is roughly 32 times faster with conversion iterators provided by `FFT.TimeStamps` than it is using in-built .net framework methods.

|                   Method |       Mean |     Error |    StdDev |
|------------------------- |-----------:|----------:|----------:|
|              With_DotNet | 211.477 ms | 3.2234 ms | 2.5166 ms |
| With_ConversionIterators |   6.607 ms | 0.1123 ms | 0.1248 ms |

[!code-csharp[Conversion iterators benchmark code](../../src/FFT.TimeStamps.Benchmarks/ConversionIteratorSpeed.cs)]
