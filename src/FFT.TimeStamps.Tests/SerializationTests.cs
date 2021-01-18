using System;
using System.Collections.Generic;
using System.Linq;
using FFT.TimeStamps.Serialization;
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

    [TestMethod]
    public void FixedTimeZoneConverter()
    {

      var settings = new JsonSerializerSettings();
      settings.UseTimeZoneSerializer(_est);

      var t1 = TimeStamp.Now;
      var json = JsonConvert.SerializeObject(t1, settings);
      var t2 = JsonConvert.DeserializeObject<TimeStamp>(json, settings);
      Assert.AreEqual(t1, t2);

      var list = new List<TimeStamp?> {
                TimeStamp.Now,
                null,
            };
      json = JsonConvert.SerializeObject(list, settings);
      var list2 = JsonConvert.DeserializeObject<List<TimeStamp?>>(json, settings);
      Assert.IsTrue(list.SequenceEqual(list2!));

      foreach (var test in new[] { new TestObject { T1 = TimeStamp.Now, T2 = TimeStamp.Now.AddMinutes(1), }, new TestObject { T1 = TimeStamp.Now, T2 = null } })
      {
        json = JsonConvert.SerializeObject(test, settings);
        var test2 = JsonConvert.DeserializeObject<TestObject>(json, settings);
        var json2 = JsonConvert.SerializeObject(test2, settings);
        Assert.AreEqual(json, json2);
      }
    }
  }

  internal class TestObject
  {
    public TimeStamp T1;
    public TimeStamp? T2;
  }
}
