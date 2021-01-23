// Copyright (c) True Goodwill. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace FFT.TimeStamps.Benchmarks
{
  using System;
  using System.Linq;
  using BenchmarkDotNet.Attributes;

  /// <summary>
  /// This benchmark code was used to help determine whether values should be passed using the in parameter or not.
  /// </summary>
  public class InOrNot
  {
#pragma warning disable SA1120 // Comments should contain text
#pragma warning disable SA1134 // Attributes should not share line
#pragma warning disable SA1516 // Elements should be separated by blank line
#pragma warning disable IDE0040 // Add accessibility modifiers
#pragma warning disable SA1400 // Access modifier should be declared
#pragma warning disable CA1822 // Mark members as static
#pragma warning disable SA1502 // Element should not be on a single line

    private readonly long[] _long = Enumerable.Repeat(21L, 1000000).ToArray();
    private readonly DateTime[] _dateTime = Enumerable.Repeat(DateTime.UtcNow, 1000000).ToArray();
    private readonly TimeStamp[] _timeStamp = Enumerable.Repeat(TimeStamp.Now, 1000000).ToArray();
    private readonly DateStamp[] _dateStamp = Enumerable.Repeat(DateStamp.UtcToday, 1000000).ToArray();
    private readonly MonthStamp[] _monthStamp = Enumerable.Repeat(MonthStamp.UtcThisMonth, 1000000).ToArray();

    // Benchmark results - make sure you update this if you modify the code below.

    /*
    */

    [Benchmark] public void TimeStampIn() { foreach (var x in _timeStamp) TimeStampIn(x); }
    [Benchmark] public void TimeStampNotIn() { foreach (var x in _timeStamp) TimeStampNotIn(x); }
    [Benchmark] public void DateStampIn() { foreach (var x in _dateStamp) DateStampIn(x); }
    [Benchmark] public void DateStampNotIn() { foreach (var x in _dateStamp) DateStampNotIn(x); }
    [Benchmark] public void MonthStampIn() { foreach (var x in _monthStamp) MonthStampIn(x); }
    [Benchmark] public void MonthStampNotIn() { foreach (var x in _monthStamp) MonthStampNotIn(x); }
    [Benchmark] public void LongIn() { foreach (var x in _long) LongIn(x); }
    [Benchmark] public void LongNotIn() { foreach (var x in _long) LongNotIn(x); }
    [Benchmark] public void DateTimeIn() { foreach (var x in _dateTime) DateTimeIn(x); }
    [Benchmark] public void DateTimeNotIn() { foreach (var x in _dateTime) DateTimeNotIn(x); }

    void TimeStampIn(in TimeStamp value) { var x = value; }
    void TimeStampNotIn(TimeStamp value) { var x = value; }
    void DateStampIn(in DateStamp value) { var x = value; }
    void DateStampNotIn(DateStamp value) { var x = value; }
    void MonthStampIn(in MonthStamp value) { var x = value; }
    void MonthStampNotIn(MonthStamp value) { var x = value; }
    void LongIn(in long value) { var x = value; }
    void LongNotIn(long value) { var x = value; }
    void DateTimeIn(in DateTime value) { var x = value; }
    void DateTimeNotIn(DateTime value) { var x = value; }
  }
}
