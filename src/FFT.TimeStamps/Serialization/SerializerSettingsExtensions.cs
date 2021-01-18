using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FFT.TimeStamps.Serialization
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

  /// <summary>
  /// https://stackoverflow.com/questions/57086654/overriding-jsonconvertertypeofusualconverter-class-attribute
  /// </summary>
  public static class SerializerSettingsExtensions
  {

    /// <summary>
    /// Adjust the <paramref name="settings"/> so that json serialization of <see cref="TimeStamp"/> objects
    /// will output a DateTime string in the given <paramref name="timeZone"/>.
    /// You would use this to produce (or deserialize) human-readable json output.
    /// https://stackoverflow.com/questions/57086654/overriding-jsonconvertertypeofusualconverter-class-attribute
    /// </summary>
    public static void UseTimeZoneSerializer(this JsonSerializerSettings settings, TimeZoneInfo timeZone)
    {
      if (null != settings.ContractResolver) throw new InvalidOperationException("The settings ContractResolver is not null. You should not overwrite it or you'll mess up custom serialization of other object types..");
      settings.ContractResolver = ConverterDisablingContractResolver.Instance;
      settings.Converters.Add(new FixedTimeZoneTimeStampConverter(timeZone));
    }

    private class ConverterDisablingContractResolver : DefaultContractResolver
    {
      public static readonly ConverterDisablingContractResolver Instance = new ConverterDisablingContractResolver();
      private ConverterDisablingContractResolver() { }
      protected override JsonConverter? ResolveContractConverter(Type objectType)
      {
        // Override the JsonConverter specified in the [JsonConverterAttribute] on the TimeStamp class declaration.
        if (objectType == typeof(TimeStamp) || objectType == typeof(TimeStamp?)) return null;
        // Use the FixedTimeZoneTimeStampConverter found in the serializer.Converters collection. 
        return base.ResolveContractConverter(objectType);
      }
    }
  }
}
