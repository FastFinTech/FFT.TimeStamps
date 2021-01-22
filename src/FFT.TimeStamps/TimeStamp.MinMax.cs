// Copyright (c) True Goodwill. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace FFT.TimeStamps
{
  using System;
  using System.Runtime.CompilerServices;

  public partial struct TimeStamp
  {
    /// <summary>
    /// Returns the lesser of the two timestamps.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public TimeStamp OrValueIfLesser(in TimeStamp t)
      => TicksUtc <= t.TicksUtc ? this : t;

    /// <summary>
    /// Returns the greater of the two timestamps.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public TimeStamp OrValueIfGreater(in TimeStamp t)
        => TicksUtc >= t.TicksUtc ? this : t;

    /// <summary>
    /// Returns the minimum of the given values.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static TimeStamp Min(TimeStamp t1, TimeStamp t2)
      => t1.TicksUtc < t2.TicksUtc ? t1 : t2;

    /// <summary>
    /// Returns the maximum of the given values.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static TimeStamp Max(TimeStamp t1, TimeStamp t2)
      => t1.TicksUtc > t2.TicksUtc ? t1 : t2;

    /// <summary>
    /// Returns the minimum of the given values.
    /// </summary>
    /// <exception cref="ArgumentException">Thrown if <paramref name="values"/> is null or empty.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static TimeStamp Min(params TimeStamp[] values)
    {
      if (values is not { Length: > 0 }) throw new ArgumentException($"{nameof(values)} is empty.", nameof(values));
      // yes, this is written the long way, (particularly without using linq) to eliminate allocations.
      var result = values[0];
      for (var i = values.Length - 1; i >= 1; i--)
      {
        if (values[i].TicksUtc < result.TicksUtc)
          result = values[i];
      }

      return result;
    }

    /// <summary>
    /// Returns the maxmimum of the given values.
    /// <exception cref="ArgumentException">Thrown if <paramref name="values"/> is null or empty.</exception>
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static TimeStamp Max(params TimeStamp[] values)
    {
      if (values is not { Length: > 0 }) throw new ArgumentException($"{nameof(values)} is empty.", nameof(values));
      // yes, this is written the long way, (particularly without using linq) to eliminate allocations.
      var result = values[0];
      for (var i = values.Length - 1; i >= 1; i--)
      {
        if (values[i].TicksUtc > result.TicksUtc)
          result = values[i];
      }

      return result;
    }
  }
}
