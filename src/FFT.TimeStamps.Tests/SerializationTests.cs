// Copyright (c) True Goodwill. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace FFT.TimeStamps.Test
{
  using System;
  using System.Text.Json;
  using System.Text.Json.Serialization;
  using Microsoft.VisualStudio.TestTools.UnitTesting;

  [TestClass]
  public class SerializationTests
  {
    [TestMethod]
    public void Serialization()
    {
      var t1 = TimeStamp.Now;
      var json = JsonSerializer.Serialize(t1);
      var t2 = JsonSerializer.Deserialize<TimeStamp>(json);
      Assert.AreEqual(t1, t2);
    }

    [TestMethod]
    public void SerializationWorksWithCustomConverterOverride()
    {
      var settings = new JsonSerializerOptions();
      settings.Converters.Add(new CustomConverter());
      var t1 = TimeStamp.Now;
      var json = JsonSerializer.Serialize(t1, settings);
      var t2 = JsonSerializer.Deserialize<TimeStamp>(json, settings);
      Assert.AreEqual(t1, t2);
    }

    internal class CustomConverter : JsonConverter<TimeStamp>
    {
      public override TimeStamp Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
      {
        var dateTime = DateTime.ParseExact(reader.GetString()!, "yyyy-MM-dd HH:mm:ss.fffffff", null);
        return new TimeStamp(dateTime.Ticks);
      }

      public override void Write(Utf8JsonWriter writer, TimeStamp value, JsonSerializerOptions options)
        => writer.WriteStringValue(value.AsUtc().DateTime.ToString("yyyy-MM-dd HH:mm:ss.fffffff"));
    }
  }
}
