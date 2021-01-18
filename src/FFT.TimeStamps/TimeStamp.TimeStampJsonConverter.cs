using System;
using Newtonsoft.Json;

namespace FFT.TimeStamps
{
  public readonly partial struct TimeStamp
  {
    private class TimeStampJsonConverter : JsonConverter
    {
      public override bool CanConvert(Type objectType)
        => objectType == typeof(TimeStamp) || objectType == typeof(TimeStamp?);

      public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
        => reader.Value is null ? null : new TimeStamp((long)reader.Value);

      public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
        => writer.WriteValue(value is TimeStamp ts ? ts.TicksUtc : null);
    }
  }
}
