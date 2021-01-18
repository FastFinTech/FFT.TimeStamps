using System;

namespace FFT.TimeStamps
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

    public interface IPeriodIterator
    {
        TimeZoneInfo TimeZone { get; }
        bool IsNewPeriod { get; }
        Period Previous { get; }
        Period Current { get; }
        Period Next { get; }
        void MoveTo(TimeStamp at);
    }

    public class Period
    {
        public TimeStamp Start;
        public TimeStamp End;
    }

    public class PeriodOfWeekIterator : IPeriodIterator
    {

        public readonly TimeSpan PeriodLength;
        public readonly TimeSpan PeriodOffset;
        public TimeZoneInfo TimeZone { get; }
        public TimeStamp At { get; private set; }
        public bool IsNewPeriod { get; private set; }
        public Period Previous { get; private set; }
        public Period Current { get; private set; }
        public Period Next { get; private set; }

        public PeriodOfWeekIterator(TimeZoneInfo timeZone, TimeSpan periodLength, TimeSpan periodOffset, TimeStamp at)
        {

            if (((TimeSpan.TicksPerDay * 7) % periodLength.Ticks) != 0)
                throw new ArgumentException($"{nameof(periodLength)} '{periodLength:c}' does not divide evenly into a week.");

            if (periodOffset >= periodLength)
                throw new ArgumentException($"{nameof(periodOffset)} '{periodOffset:c}' must be less than {nameof(periodLength)} '{periodLength:c}'.");

            if (periodOffset.Ticks < 0)
                throw new ArgumentException($"{nameof(periodOffset)} '{periodOffset:c}' must be >= 0.");

            if (periodLength.Ticks <= 0)
                throw new ArgumentException($"{nameof(periodLength)} '{periodLength:c}' must be > 0.");


            TimeZone = timeZone;
            PeriodLength = periodLength;
            PeriodOffset = periodOffset;
            At = at;

            var atTicksTimeZone = at.AsTicks(TimeZone);
            var startTicksTimeZone = atTicksTimeZone.ToPeriodOfWeekFloor(PeriodLength.Ticks, PeriodOffset.Ticks);
            var endTicksTimeZone = startTicksTimeZone + PeriodLength.Ticks;
            Current = new Period
            {
                Start = new TimeStamp(startTicksTimeZone, TimeZone),
                End = new TimeStamp(endTicksTimeZone, TimeZone),
            };
            Previous = new Period
            {
                Start = new TimeStamp(startTicksTimeZone - PeriodLength.Ticks, TimeZone),
                End = Current.Start,
            };
            Next = new Period
            {
                Start = Current.End,
                End = new TimeStamp(endTicksTimeZone + PeriodLength.Ticks, TimeZone),
            };

            IsNewPeriod = true;
        }

        public void MoveTo(TimeStamp at)
        {
            while (at.TicksUtc > Current.End.TicksUtc)
                MoveNext();
            At = at;
        }

        void MoveNext()
        {
            IsNewPeriod = true;
            Previous = Current;
            Current = Next;
            Next = new Period
            {
                Start = Current.End,
                End = new TimeStamp(Current.End.AsTicks(TimeZone) + PeriodLength.Ticks, TimeZone),
            };
        }
    }
}
