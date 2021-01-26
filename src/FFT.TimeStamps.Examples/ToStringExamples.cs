// Copyright (c) True Goodwill. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace FFT.TimeStamps.Examples
{
  using System;
  using static FFT.TimeStamps.Examples.TimeZones;

  internal class ToStringExamples : IExample
  {
    public void Run()
    {
      // midnight, new year's day, 2020, New York.
      var time = new TimeStamp(new DateStamp(2020, 1, 1), TimeSpan.Zero, NewYork);
      Console.WriteLine($"Time in New York is '{time.ToString(NewYork, "yy-MM-dd HH:mm:ss")}'.");
      Console.WriteLine($"Time in Sydney is '{time.ToString(Sydney, "yy-MM-dd HH:mm:ss")}'.");
      /* Output:
        Time in New York is '20-01-01 00:00:00'.
        Time in Sydney is '20-01-01 16:00:00'.
      */
    }
  }
}
