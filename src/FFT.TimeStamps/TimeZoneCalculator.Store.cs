// Copyright (c) True Goodwill. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace FFT.TimeStamps
{
  using System;
  using System.Collections.Generic;

  public sealed partial class TimeZoneCalculator
  {
    private static class Store
    {
      private static Dictionary<string, TimeZoneCalculator> _store = new();

      public static TimeZoneCalculator Get(TimeZoneInfo timeZone)
      {
        if (_store.TryGetValue(timeZone.Id, out var calculator))
          return calculator;

        calculator = new TimeZoneCalculator(timeZone);
        ImmutableInterlocked.Update(ref _store, x =>
        {
          if (x.ContainsKey(timeZone.Id))
            return x;

          var y = new Dictionary<string, TimeZoneCalculator>(x);
          y[timeZone.Id] = calculator;
          return y;
        });

        return calculator;
      }
    }

  }
}
