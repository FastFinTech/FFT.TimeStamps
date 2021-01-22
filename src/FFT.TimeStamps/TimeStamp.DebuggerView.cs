// Copyright (c) True Goodwill. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace FFT.TimeStamps
{
  public readonly partial struct TimeStamp
  {
    private readonly struct DebuggerView
    {
      public readonly long TicksUTC;
      public readonly string Local;
      public readonly string Utc;

      public DebuggerView(TimeStamp value)
      {
        TicksUTC = value.TicksUtc;
        Local = value.AsLocal().ToString(DEFAULT_FORMAT_STRING);
        Utc = value.AsUtc().ToString(DEFAULT_FORMAT_STRING);
      }
    }
  }
}
