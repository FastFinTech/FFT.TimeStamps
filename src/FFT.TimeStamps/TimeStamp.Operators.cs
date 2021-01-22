// Copyright (c) True Goodwill. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace FFT.TimeStamps
{
  using System;
  using System.Runtime.CompilerServices;

  public partial struct TimeStamp
  {
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator long(in TimeStamp a)
      => a.TicksUtc;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator TimeStamp(in long ticksUtc)
      => new TimeStamp(ticksUtc);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator ==(TimeStamp a, TimeStamp b)
      => a.TicksUtc == b.TicksUtc;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator !=(TimeStamp a, TimeStamp b)
      => a.TicksUtc != b.TicksUtc;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator ==(in TimeStamp a, in TimeStamp b)
      => a.TicksUtc == b.TicksUtc;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator !=(in TimeStamp a, in TimeStamp b)
      => a.TicksUtc != b.TicksUtc;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator >(in TimeStamp a, in TimeStamp b)
      => a.TicksUtc > b.TicksUtc;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator <(in TimeStamp a, in TimeStamp b)
      => a.TicksUtc < b.TicksUtc;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator >=(in TimeStamp a, in TimeStamp b)
      => a.TicksUtc >= b.TicksUtc;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator <=(in TimeStamp a, in TimeStamp b)
      => a.TicksUtc <= b.TicksUtc;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static TimeStamp operator +(in TimeStamp a, in long ticks)
      => new TimeStamp(a.TicksUtc + ticks);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static TimeStamp operator -(in TimeStamp a, in long ticks)
      => new TimeStamp(a.TicksUtc - ticks);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static TimeStamp operator +(in TimeStamp a, in TimeSpan time)
      => new TimeStamp(a.TicksUtc + time.Ticks);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static TimeStamp operator -(in TimeStamp a, in TimeSpan time)
      => new TimeStamp(a.TicksUtc - time.Ticks);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static TimeSpan operator -(in TimeStamp a, in TimeStamp b)
      => new TimeSpan(a.TicksUtc - b.TicksUtc);
  }
}
