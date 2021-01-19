# Conversion Iterators

Conversion iterators provide the fastest-possible way to convert a stream of chronological-order `DateTime` objects into `TimeStamp` objects. They outperform the .net `TimeZoneInfo` conversion methods by orders of magnitude.

> [!WARNING]
> Conversion iterators require that the input times be supplied in ascending chronological order in order to provide correct conversion results. This requirement allows code optimization to make the conversion algorithm extremely efficient, but it also means you can get incorrect results if you accidentally supply inputs in descending chronological order. The iterators are coded to be fast, so they do not check YOUR work.

Use methods provided in the [`ConversionIterators`](xref:FFT.TimeStamps.ConversionIterators) class to create an iterators of various types:

- [`IToTimeStampConversionIterator`](xref:FFT.TimeStamps.IToTimeStampConversionIterator) Converts `DateTime` and `DateTimeOffset` objects to `TimeStamp` objects.
- [`IFromTimeStampConversionIterator`](xref:FFT.TimeStamps.IFromTimeStampConversionIterator) Provides the opposite-direction conversion.
- [`ITimeZoneConversionIterator`](xref:FFT.TimeStamps.ITimeZoneConversionIterator) Converts `DateTime` and `DateTimeOffset` objects from one timezone to another.

All of the conversion iterators also provide access to low-level operations allowing you to bypass the `TimeStamp`, `DateTime`, and `DateTimeOffset` objects to work directly with time expressed in ticks (1 ten-millionth of a second) using the `long` variable type. See their api documentation for details.

[!code-csharp[Conversion iterators example code](../../src/FFT.TimeStamps.Examples/ConversionIterators.cs)]