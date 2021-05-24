// Copyright (c) True Goodwill. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace FFT.TimeStamps
{
  using System;
  using System.Runtime.InteropServices;
  using System.Runtime.Versioning;
  using FILETIME = System.Runtime.InteropServices.ComTypes.FILETIME;

  public partial struct TimeStamp
  {
    /*
     * See Benchmarks/NowMethods.cs for experiments and benchmarks
    */

    private const string KERNEL32 = "kernel32.dll";

    private static Func<long> _getNowTicks;

    /// <summary>
    /// Gets the current time as a <see cref="TimeStamp"/>.
    /// </summary>
    public static TimeStamp Now
      => new TimeStamp(_getNowTicks());

    private static void SetupGetNowTicks()
    {
      if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
      {
        _getNowTicks = GetNowTicks_Windows;
      }
      else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
      {
        _getNowTicks = GetNowTicks_Linux;
      }
      else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
      {
        _getNowTicks = GetNowTicks_OSX;
      }
      else
      {
        throw new Exception("Unable to determine OS Platform.");
      }
    }

    private static long GetNowTicks_Windows()
    {
      GetSystemTimeAsFileTime(out var fileTime);
      long time = 0;
      time |= (uint)fileTime.dwHighDateTime;
      time <<= sizeof(uint) * 8;
      time |= (uint)fileTime.dwLowDateTime;
      return time + 0x701ce1722770000L;
    }

    [DllImport(KERNEL32, SetLastError = true)]
    [ResourceExposure(ResourceScope.None)]
    private static extern void GetSystemTimeAsFileTime([Out] out FILETIME time);

    // I ran out of time implementing these and had to just use a cheat,
    // slightly slower implemention.
    private static long GetNowTicks_Linux()
    {
      return DateTime.UtcNow.Ticks;
    }

    // I ran out of time implementing these and had to just use a cheat,
    // slightly slower implemention.
    private static long GetNowTicks_OSX()
    {
      return DateTime.UtcNow.Ticks;
    }
  }
}
