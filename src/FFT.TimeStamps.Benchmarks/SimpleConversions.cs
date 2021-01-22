// Copyright (c) True Goodwill. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace FFT.TimeStamps.Benchmarks
{
  using System;
  using System.Linq;
  using BenchmarkDotNet.Attributes;
  using static FFT.TimeStamps.Benchmarks.TimeZones;

  public class SimpleConversions
  {
    private static readonly DateTime[] _newYorkTimes;

    static SimpleConversions()
    {
      _newYorkTimes = ExampleFeed.ChronologicalUnspecifiedDateTimes().ToArray();
    }

    /// <summary>
    /// Performs test using built-in .net framework feature.
    /// </summary>
    [Benchmark]
    public void With_System_TimeZoneInfo()
    {
      foreach (DateTime newYorkTime in _newYorkTimes)
      {
        DateTime sydneyTime = TimeZoneInfo.ConvertTime(newYorkTime, NewYork, Sydney);
      }
    }

    /// <summary>
    /// Performs test using FFT.TimeStamps simplest and slowest conversion method.
    /// </summary>
    [Benchmark]
    public void With_TimeStamps_TimeZoneCalculator()
    {
      foreach (DateTime newYorkTime in _newYorkTimes)
      {
        long sydneyTicks = TimeZoneCalculator.Convert(NewYork, Sydney, newYorkTime.Ticks);
        DateTime sydneyTime = new DateTime(sydneyTicks, DateTimeKind.Unspecified);
      }
    }
  }
}
