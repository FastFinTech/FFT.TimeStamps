using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
//using FFT.TimeZones;

namespace FFT.TimeStamps.Test
{


  [TestClass]
  public class SerializationTests
  {

    private static readonly TimeZoneInfo _est = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");

    [TestMethod]
    public void SingleObject()
    {
      var now = TimeStamp.Now;
      var json = JsonConvert.SerializeObject(now);
      var then = JsonConvert.DeserializeObject<TimeStamp>(json);
      Assert.AreEqual(now, then);
    }

    [TestMethod]
    public void ObjectProperties()
    {
      var value = new TestObject
      {
        T1 = TimeStamp.Now,
        T2 = null,
      };
      var json = JsonConvert.SerializeObject(value);
      var comparison = JsonConvert.DeserializeObject<TestObject>(json);
      Assert.AreEqual(value.T1, comparison.T1);
      Assert.AreEqual(value.T2, comparison.T2);
    }

    [TestMethod]
    public void Nullables()
    {
      var list = new List<TimeStamp?> {
                null,
                TimeStamp.Now,
            };
      var json = JsonConvert.SerializeObject(list);
      var list2 = JsonConvert.DeserializeObject<List<TimeStamp?>>(json);
      Assert.IsTrue(Enumerable.SequenceEqual(list, list2));
    }

    internal class TestObject
    {
      public TimeStamp T1;
      public TimeStamp? T2;
    }
  }
}
