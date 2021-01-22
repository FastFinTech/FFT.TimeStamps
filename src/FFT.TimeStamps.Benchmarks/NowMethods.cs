//// Copyright (c) True Goodwill. All rights reserved.
//// Licensed under the MIT license. See LICENSE file in the project root for full license information.

//namespace FFT.TimeStamps.Benchmarks
//{
//  using System;
//  using System.Runtime.InteropServices;
//  using System.Runtime.Versioning;
//  using BenchmarkDotNet.Attributes;
//  using FILETIME = System.Runtime.InteropServices.ComTypes.FILETIME;

//  /// <summary>
//  /// This benchmark code was used to help determine the best way to get a value for TimeStamp.Now.
//  /// </summary>
//  public class NowMethods
//  {
//    private const string KERNEL32 = "kernel32.dll";

//    // Benchmark results - make sure you update this if you modify the code below.
//    /*
//    |              Method |     Mean |    Error |   StdDev |
//    |-------------------- |---------:|---------:|---------:|
//    |   GetNowViaDateTime | 79.18 ns | 0.908 ns | 0.804 ns |
//    | GetNowViaSystemCall | 16.32 ns | 0.161 ns | 0.143 ns |
//    */

//    [Benchmark]
//    public long GetNowViaDateTime() => DateTime.UtcNow.Ticks;

//    [Benchmark]
//    public long GetNowViaSystemCall() => GetSystemTimeAsFileTime();

//    private static long GetSystemTimeAsFileTime()
//    {
//      GetSystemTimeAsFileTime(out var fileTime);
//      long time = 0;
//      time |= (uint)fileTime.dwHighDateTime;
//      time <<= sizeof(uint) * 8;
//      time |= (uint)fileTime.dwLowDateTime;
//      return time + 0x701ce1722770000L;
//    }

//    [DllImport(KERNEL32, SetLastError = true)]
//    [ResourceExposure(ResourceScope.None)]
//    private static extern void GetSystemTimeAsFileTime([Out] out FILETIME time);
//  }
//}
