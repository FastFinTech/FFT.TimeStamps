// Copyright (c) True Goodwill. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace FFT.TimeStamps.Benchmarks
{
  using System;
  using System.Linq;
  using BenchmarkDotNet.Attributes;
  using static FFT.TimeStamps.Benchmarks.TimeZones;

  /// <summary>
  /// Measures the speed of converting 1,000,000 UTC DateTimes in chronological order to New York timezone DateTimes.
  /// </summary>
  public class ConversionIteratorSpeed
  {
    private static readonly DateTime[] _dateTimes = ExampleFeed.ChronologicalUtcDateTimes().ToArray();

    /// <summary>
    /// Performs test using built-in .net framework feature.
    /// </summary>
    [Benchmark]
    public void With_DotNet()
    {
      foreach (var datetime in _dateTimes)
        TimeZoneInfo.ConvertTime(datetime, NewYork);
    }

    /// <summary>
    /// Performs test using FFT.TimeStamps conversion feature.
    /// </summary>
    [Benchmark]
    public void With_ConversionIterators()
    {
      var iterator = ConversionIterators.Create(TimeZoneInfo.Utc, NewYork);
      foreach (var datetime in _dateTimes)
        iterator.GetDateTime(datetime.Ticks);
    }
  }
}
