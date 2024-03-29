﻿// Copyright (c) True Goodwill. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace FFT.TimeStamps
{
  using System.Runtime.CompilerServices;

  public sealed partial class TimeZoneCalculator
  {
    /// <summary>
    /// Represents a period of time in which a particular timezone has a consistent properties such as offset from UTC, ambiguity and validity.
    /// </summary>
    public sealed class TimeZoneSegment
    {
      private readonly SegmentStore _segmentStore;

      private TimeZoneSegment? _next;
      private TimeZoneSegment? _previous;

      internal TimeZoneSegment(TimeKind segmentKind, long startTicks, long endTicks, bool isInvalid, bool isAmbiguous, long offsetTicks, SegmentStore segmentStore)
      {
        SegmentKind = segmentKind;
        StartTicks = startTicks;
        EndTicks = endTicks;
        IsInvalid = isInvalid;
        IsAmbiguous = isAmbiguous;
        OffsetTicks = offsetTicks;
        _segmentStore = segmentStore;
      }

      /// <summary>
      /// Identifies the timezone type in which <see cref="StartTicks"/> and <see cref="EndTicks"/> are expressed.
      /// </summary>
      public TimeKind SegmentKind { get; }

      /// <summary>
      /// The begin time of the <see cref="TimeZoneSegment"/>, expressed in ticks in the timezone specified by <see cref="SegmentKind"/>.
      /// </summary>
      public long StartTicks { get; }

      /// <summary>
      /// The end time of the <see cref="TimeZoneSegment"/>, expressed in ticks in the timezone specified by <see cref="SegmentKind"/>.
      /// The segment itself is EXCLUSIVE of this time value. This time value is also the begin time of the next segment.
      /// </summary>
      public long EndTicks { get; }

      /// <summary>
      /// When <see cref="SegmentKind"/> is <see cref="TimeKind.TimeZone"/>, this value is true if it falls in a period of time which does not exist
      /// and has no UTC equivalent. Typically this happens at the beginning of a daylight savings, when the clock skips forward an hour or so.
      /// </summary>
      public bool IsInvalid { get; }

      /// <summary>
      /// When <see cref="SegmentKind"/> is <see cref="TimeKind.TimeZone"/>, this value is true if it falls in a period of time which has two
      /// or more UTC equivalents. Typically this happens at the end of a daylight savings period, when the clock skips backward an hour or so,
      /// repeating the most recent times.
      /// </summary>
      public bool IsAmbiguous { get; }

      /// <summary>
      /// The UTC offset for the timezone during this <see cref="TimeZoneSegment"/>.
      /// When <see cref="IsInvalid"/> or <see cref="IsAmbiguous"/> is true, this value will contain the STANDARD offset for the timezone.
      /// </summary>
      public long OffsetTicks { get; }

      /// <summary>
      /// Gets the next segment.
      /// </summary>
      public TimeZoneSegment Next
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => _next ??= _segmentStore!.GetSegmentAt(EndTicks);
      }

      /// <summary>
      /// Gets the previous segment.
      /// </summary>
      public TimeZoneSegment Previous
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => _previous ??= _segmentStore!.GetSegmentAt(StartTicks - 1);
      }
    }
  }
}
