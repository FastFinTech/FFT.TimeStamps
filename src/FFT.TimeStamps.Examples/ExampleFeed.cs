using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FFT.TimeStamps.Examples
{
  internal static class ExampleFeed
  {
    static ExampleFeed()
    {
    }

    public static IEnumerable<DateTime> ChronologicalDateTimes()
    {
      var time = new DateTime(2020, 1, 1, 0, 0, 0, DateTimeKind.Unspecified);
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
