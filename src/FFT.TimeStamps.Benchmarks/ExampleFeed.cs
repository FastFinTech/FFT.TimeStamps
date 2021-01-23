// Copyright (c) True Goodwill. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace FFT.TimeStamps.Benchmarks
{
  using System;
  using System.Collections.Generic;

  internal static class ExampleFeed
  {
    static ExampleFeed()
    {
    }

    public static IEnumerable<DateTime> ChronologicalUnspecifiedDateTimes()
    {
      var time = new DateTime(2020, 1, 1, 0, 0, 0, DateTimeKind.Unspecified);
      for (var i = 0; i < 1000000; i++)
      {
        yield return time;
        time = time.AddSeconds(1);
      }
    }

    public static IEnumerable<DateTime> ChronologicalUtcDateTimes()
    {
      var time = new DateTime(2020, 1, 1, 0, 0, 0, DateTimeKind.Utc);
      for (var i = 0; i < 1000000; i++)
      {
        yield return time;
        time = time.AddSeconds(1);
      }
    }

    public static IEnumerable<TimeStamp> ChronologicalTimeStamps()
    {
      var time = TimeStamp.Now;
      for (var i = 0; i < 1000000; i++)
      {
        yield return time;
        time = time.AddSeconds(1);
      }
    }
  }
}
