using CMIUICXCore.Types;
using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace CMIUICXCore.Converters
{
    public class CmiuCommandConverter : JsonConverter<CmiuCommandBase>
    {
        public override CmiuCommandBase Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType != JsonTokenType.StartObject)
                throw new JsonException();

            using var jsonDocument = JsonDocument.ParseValue(ref reader);
            if (!jsonDocument.RootElement.TryGetProperty("type", out var typeProperty))
                throw new JsonException();

            var jsonObject = jsonDocument.RootElement.GetRawText();

            return (typeProperty.GetString()) switch
            {
                "command" => JsonSerializer.Deserialize<CmiuCommand>(jsonObject),
                "places" => JsonSerializer.Deserialize<CmiuPlacesCommand>(jsonObject),
                "season_card" => JsonSerializer.Deserialize<CmiuSeasonCardCommand>(jsonObject),
                "epp_session" => JsonSerializer.Deserialize<CmiuAisSessionCommand>(jsonObject),
                "levels" => JsonSerializer.Deserialize<CmiuLevelsCommand>(jsonObject),
                "call" => JsonSerializer.Deserialize<CmiuCallCommand>(jsonObject),
                "mp3_file_list" => JsonSerializer.Deserialize<Mp3ListCommand>(jsonObject),
                _ => throw new JsonException(),
            };
        }

        public override void Write(Utf8JsonWriter writer, CmiuCommandBase value, JsonSerializerOptions options)
        {
            JsonSerializer.Serialize(writer, (object)value, options);
        }
    }
}
