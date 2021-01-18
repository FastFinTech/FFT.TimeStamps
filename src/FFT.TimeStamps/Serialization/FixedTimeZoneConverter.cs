using System;
using Newtonsoft.Json;

namespace FFT.TimeStamps
{
  internal class FixedTimeZoneTimeStampConverter : JsonConverter
  {
    private readonly TimeZoneInfo _timeZone;

    public FixedTimeZoneTimeStampConverter(TimeZoneInfo timeZone)
      => _timeZone = timeZone;

    public override bool CanConvert(Type objectType)
      => objectType == typeof(TimeStamp) || objectType == typeof(TimeStamp?);

    public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
      => reader.Value is null ? null : new TimeStamp(((DateTime)reader.Value).Ticks, _timeZone);

    public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
      => writer.WriteValue(value is TimeStamp ts ? ts.As(_timeZone).DateTime : null);
  }
}

