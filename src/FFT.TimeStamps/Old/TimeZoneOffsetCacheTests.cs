//using ApexInvesting.Platform.TimeZones;
//using ApexTough.TimeZoneConversion;
//using System;
//using System.Diagnostics;

//namespace ApexTough.TimeStamps {

//    public static class TimeZoneOffsetCacheTests {

//        public static void Run() {
//            var est = TimeZoneReferences.EasternStandardTime;
//            TimeZoneOffsetCache offsetCache = null;
//            Time("Creating cache", () => {
//                offsetCache = TimeZoneOffsetCache.Get(est);
//            });

//            var from = new DateTime(2013, 1, 1, 0, 0, 0, DateTimeKind.Unspecified);
//            var to = new DateTime(DateTime.Now.Year + 1, 1, 1, 0, 0, 0, DateTimeKind.Unspecified);

//            Time("Getting offsets from cache, timezone method, round 1", () => {
//                for (var i = 0; i < 10; i++) {
//                    for (var d = from; d < to; d = d.AddMinutes(1)) {
//                        var offsetMinutes = offsetCache.GetOffsetMinutes(d);
//                    }
//                }
//            });

//            Time("Getting offsets from cache, timezone method, round 2", () => {
//                for (var i = 0; i < 10; i++) {
//                    for (var d = from; d < to; d = d.AddMinutes(1)) {
//                        var offsetMinutes = offsetCache.GetOffsetMinutes(d);
//                    }
//                }
//            });

//            Time("Getting offsets from cache, utc method, round 1", () => {
//                for (var i = 0; i < 10; i++) {
//                    for (var timestamp = from.Ticks; timestamp < to.Ticks; timestamp += TimeSpan.TicksPerMinute) {
//                        var offsetMinutes = offsetCache.GetOffsetMinutes(timestamp);
//                    }
//                }
//            });

//            Time("Getting offsets from cache, utc method, round 2", () => {
//                for (var i = 0; i < 10; i++) {
//                    for (var timestamp = from.Ticks; timestamp < to.Ticks; timestamp += TimeSpan.TicksPerMinute) {
//                        var offsetMinutes = offsetCache.GetOffsetMinutes(timestamp);
//                    }
//                }
//            });

//            Time("Getting offsets from timeZoneInfo", () => {
//                for (var i = 0; i < 10; i++) {
//                    for (var d = from; d < to; d = d.AddMinutes(1)) {
//                        var offsetMinutes = (short)est.GetUtcOffset(d).TotalMinutes;
//                    }
//                }
//            });


//            ITimeZoneConverter oldConverterCache = null;
//            Time("Creating old converter cache", () => {
//                oldConverterCache = TimeZoneConverterCache.Get(TimeZoneInfo.Local, est);
//            });

//            Time("Converting from old converter cache, round 1", () => {
//                for (var i = 0; i < 10; i++) {
//                    for (var d = from; d < to; d = d.AddMinutes(1)) {
//                        try {
//                            var newD = oldConverterCache.Convert(d);
//                        } catch { }
//                    }
//                }
//            });
//            Time("Converting from old converter cache, round 2", () => {
//                for (var i = 0; i < 10; i++) {
//                    for (var d = from; d < to; d = d.AddMinutes(1)) {
//                        try {
//                            var x = oldConverterCache.Convert(d);
//                        } catch { }
//                    }
//                }
//            });

//            Time("Verifying timezone offset results", () => {
//                for (var d = from; d < to; d = d.AddMinutes(1)) {
//                    var officialResult = est.GetUtcOffset(d);
//                    var cacheResult = TimeSpan.FromMinutes(offsetCache.GetOffsetMinutes(d));
//                    if (officialResult != cacheResult) {
//                        Debugger.Break();
//                    }
//                }
//            });

//            Time("Verifying utc offset results", () => {
//                for (var d = from; d < to; d = d.AddMinutes(1)) {
//                    var ticks = d.Ticks;
//                    var officialResult = est.GetUtcOffset(new DateTimeOffset(ticks, TimeSpan.Zero));
//                    var cacheResult = TimeSpan.FromMinutes(offsetCache.GetOffsetMinutes(ticks));
//                    if (officialResult != cacheResult) {
//                        Debugger.Break();
//                    }
//                }
//            });

//        }

//        static void Time(string name, Action action) {
//            Console.Write($"{name}: ");
//            var sw = Stopwatch.StartNew();
//            action();
//            sw.Stop();
//            Console.WriteLine($"{sw.ElapsedMilliseconds}ms");
//        }
//    }
//}