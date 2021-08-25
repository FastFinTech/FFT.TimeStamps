// Copyright (c) True Goodwill. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace FFT.TimeStamps
{
  using System;
  using System.Text.Json;
  using System.Text.Json.Serialization;

  public readonly partial struct TimeStamp
  {
    /// <summary>
    /// This is the default <see
    /// cref="System.Text.Json.Serialization.JsonConverter"/> for the TimeStamp.
    /// It represents the <see cref="TimeStamp"/> in json text as an <see
    /// cref="long"/> containing the <see cref="TimeStamp.TicksUtc"/> value.
    /// </summary>
    public class TimeStampJsonConverter : JsonConverter<TimeStamp>
    {
      /// <inheritdoc />
      public override TimeStamp Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        => new TimeStamp(reader.GetInt64());

      /// <inheritdoc />
      public override void Write(Utf8JsonWriter writer, TimeStamp value, JsonSerializerOptions options)
        => writer.WriteNumberValue(value.TicksUtc);
    }
  }
}
