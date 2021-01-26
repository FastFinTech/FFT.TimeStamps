// Copyright (c) True Goodwill. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace FFT.TimeStamps.Examples
{
  using static FFT.TimeStamps.Examples.TimeZones;

  public sealed class TicksExamples : IExample
  {
    public void Run()
    {
      // get the ticks value at a certain time.
      var ticksAtNewYear2020 = Ticks.At(2020, 01, 01, 0, 0, 0);
      // create a timestamp at that time in New York.
      TimeStamp newYearsInNewYOrk = new TimeStamp(ticksAtNewYear2020, NewYork);
      TimeStamp newYearsInSydney = new TimeStamp(ticksAtNewYear2020, Sydney);

      var differenceBetweenNewYorkAndSydney = newYearsInSydney.TicksUtc - newYearsInNewYOrk.TicksUtc;
      differenceBetweenNewYorkAndSydney.ToHours();

      TimeStamp now = TimeStamp.Now;
      long ticksPastWeek = now.TicksUtc.TicksPastWeek();
      double minutesPastWeek = ticksPastWeek.ToMinutes();
      // etc.
    }
  }
}
