using System.Runtime.CompilerServices;

namespace FFT.TimeStamps
{
  public sealed partial class TimeZoneCalculator
  {
    /// <summary>
    /// Represents a period of time in which a particular timezone has a consistent offset from UTC.
    /// </summary>
    public sealed class TimeZoneSegment
    {
      /// <summary>
      /// Identifies the timezone type in which <see cref="StartTicks"/> and <see cref="EndTicks"/> are expressed.
      /// </summary>
      public TimeKind SegmentKind { get; private set; }

      /// <summary>
      /// The begin time of the <see cref="TimeZoneSegment"/>, expressed in ticks in the timezone specified by <see cref="SegmentKind"/>.
      /// </summary>
      public long StartTicks { get; private set; }

      /// <summary>
      /// The end time of the <see cref="TimeZoneSegment"/>, expressed in ticks in the timezone specified by <see cref="SegmentKind"/>.
      /// The segment itself is EXCLUSIVE of this time value. This time value is also the begin time of the next segment.
      /// </summary>
      public long EndTicks { get; private set; }

      /// <summary>
      /// When <see cref="SegmentKind"/> is <see cref="TimeKind.TimeZone"/>, this value is true if it falls in a period of time which does not exist
      /// and has no UTC equivalent. Typically this happens at the beginning of a daylight savings, when the clock skips forward an hour or so.
      /// </summary>
      public bool IsInvalid { get; private set; }

      /// <summary>
      /// When <see cref="SegmentKind"/> is <see cref="TimeKind.TimeZone"/>, this value is true if it falls in a period of time which has two
      /// or more UTC equivalents. Typically this happens at the end of a daylight savings period, when the clock skips backward an hour or so,
      /// repeating the most recent times.
      /// </summary>
      public bool IsAmbiguous { get; private set; }

      /// <summary>
      /// The UTC offset for the timezone during this <see cref="TimeZoneSegment"/>.
      /// When <see cref="IsInvalid"/> or <see cref="IsAmbiguous"/> is true, this value will contain the STANDARD offset for the timezone.
      /// </summary>
      public long OffsetTicks { get; private set; }

      private TimeZoneSegment? _next;
      private TimeZoneSegment? _previous;
      private readonly TimeZoneCalculator? _calculator;

      internal TimeZoneSegment(TimeKind segmentKind, long startTicks, long endTicks, bool isInvalid, bool isAmbiguous, long offsetTicks, TimeZoneCalculator calculator)
      {
        SegmentKind = segmentKind;
        StartTicks = startTicks;
        EndTicks = endTicks;
        IsInvalid = isInvalid;
        IsAmbiguous = isAmbiguous;
        OffsetTicks = offsetTicks;
        _calculator = calculator;
      }

      /// <summary>
      /// Gets the next segment.
      /// </summary>
      public TimeZoneSegment Next
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => _next ??= _calculator!.GetSegment(EndTicks, SegmentKind);
      }

      /// <summary>
      /// Gets the previous segment.
      /// </summary>
      public TimeZoneSegment Previous
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => _previous ??= _calculator!.GetSegment(StartTicks - 1, SegmentKind);
      }
    }
  }
}
