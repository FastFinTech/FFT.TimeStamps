// Copyright (c) True Goodwill. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

#if NET
namespace FFT.TimeStamps;

using System;
using System.Text.Json;

[JsonConverter(typeof(TimeOfWeekJsonConverter))]
public readonly partial struct TimeOfWeek
{
  /// <summary>
  /// This is the default <see cref="System.Text.Json.Serialization.JsonConverter"/> for the <see cref="TimeOfWeek"/>.
  /// It represents the <see cref="TimeOfWeek"/> in json text as a <see cref="long"/> containing the <see cref="TimeOfWeek.TicksSinceWeekFloor"/> value.
  /// </summary>
  public sealed class TimeOfWeekJsonConverter : JsonConverter<TimeOfWeek>
  {
    /// <inheritdoc/>
    public override void Write(Utf8JsonWriter writer, TimeOfWeek value, JsonSerializerOptions options)
      => writer.WriteNumberValue(value.TicksSinceWeekFloor);

    /// <inheritdoc/>
    public override TimeOfWeek Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
      => new TimeOfWeek(reader.GetInt64());
  }
}
#endif
