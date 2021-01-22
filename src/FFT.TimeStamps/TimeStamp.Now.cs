// Copyright (c) True Goodwill. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace FFT.TimeStamps
{
  using System.Runtime.InteropServices;
  using System.Runtime.Versioning;
  using FILETIME = System.Runtime.InteropServices.ComTypes.FILETIME;

  public partial struct TimeStamp
  {
    /*
     * See Benchmarks/NowMethods.cs for experiments and benchmarks
    */

    private const string KERNEL32 = "kernel32.dll";

    /// <summary>
    /// Gets the current time as a <see cref="TimeStamp"/>.
    /// </summary>
    public static TimeStamp Now
      => new TimeStamp(GetNowTicks());

    private static long GetNowTicks()
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
  }
}
