# TimeZoneOffsetCalculator

The [`TimeZoneOffsetCalculator`](xref:FFT.TimeStamps.TimeZoneOffsetCalculator) allows you to perform timezone conversion calculations an order of magnitude faster than the `TimeZoneInfo` methods supplied by the .net framework. 

The static `TimeZoneOffsetCalculator.Convert` method allows you to convert times directly from one timezone to another. This is the slowest utility method available in the library (but much faster than you can find anywhere else), and you would use it in situations where its either not used frequently, or where you don't know in advance which timezones will be converted.

The `TimeZoneOffsetCalculator.GetOffsetFromTimeZoneTicks` and `TimeZoneOffsetCalculator.GetOffsetFromUtcTicks` methods not only provide you with a very fast calculation, but they also provide you with a range of time in which the timezone offset does not change. Taking advantage of this range of time can help you write more efficient conversions. The [`Conversion iterators`](conversionIterators.md) use this feature internally which is why they are so incredibly efficient.

[!code-csharp[TimeZoneOffsetCalculator example code](../../src/FFT.TimeStamps.Examples/TimeZoneOffsetCalculator.cs)]
