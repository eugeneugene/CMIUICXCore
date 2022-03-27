using CMIUICXCore.Code;
using CMIUICXCore.Types;
using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace CMIUICXCore.Converters
{
    public class AisParkingOperationJsonConverter : JsonConverter<AisParkingOperation>
    {
        public override AisParkingOperation Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            string enumValue = reader.GetString();
            foreach (var val in Enum.GetValues<AisParkingOperation>())
            {
                if (val.XmlEnum() == enumValue)
                    return val;
            }
            throw new JsonException($"Argument '{enumValue}' is not of type 'AisParkingOperation'");
        }

        public override void Write(Utf8JsonWriter writer, AisParkingOperation value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.XmlEnum());
        }
    }
}
