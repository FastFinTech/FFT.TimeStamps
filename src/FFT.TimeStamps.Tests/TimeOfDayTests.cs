using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace FFT.TimeStamps.Test {

    [TestClass]
    public class TimeOfDayTests {
         
        [TestMethod]
        public void TimeOfDay() {
            var nowUtc = DateTime.UtcNow;
            var nowStamp = new TimeStamp(nowUtc.Ticks);
            Assert.AreEqual(nowUtc.TimeOfDay, nowUtc.Ticks.ToTimeOfDay());
            Assert.AreEqual(nowUtc.TimeOfDay, nowStamp.ToTimeOfDay());

            nowUtc = DateTime.UtcNow.Date;
            nowStamp = new TimeStamp(nowUtc.Ticks);
            Assert.AreEqual(nowUtc.TimeOfDay, nowUtc.Ticks.ToTimeOfDay());
            Assert.AreEqual(nowUtc.TimeOfDay, nowStamp.ToTimeOfDay());
        }
    }
}
