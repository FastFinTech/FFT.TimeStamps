// Copyright (c) True Goodwill. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace FFT.TimeStamps
{
  using System;

  /// <summary>
  /// Represents the beginning and end of a time period.
  /// </summary>
  public sealed class PeriodOfTime : IEquatable<PeriodOfTime>
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="PeriodOfTime"/> class.
    /// </summary>
    public PeriodOfTime(TimeStamp start, TimeStamp end)
    {
      Start = start;
      End = end;
    }

    /// <summary>
    /// The start of the time period.
    /// The time period is usually inclusive of this value, depending on how this object is being used.
    /// </summary>
    public TimeStamp Start { get; }

    /// <summary>
    /// The end of the time period.
    /// The time period is usually exclusive of this value, depending on how this object is being used.
    /// </summary>
    public TimeStamp End { get; }

    /// <inheritdoc/>
    public override bool Equals(object? obj)
      => Equals(obj as PeriodOfTime);

    /// <inheritdoc/>
    public bool Equals(PeriodOfTime? other)
      => other is not null && Start == other.Start && End == other.End;

    /// <inheritdoc/>
    public override int GetHashCode()
      => HashCode.Combine(Start, End);
  }
}
