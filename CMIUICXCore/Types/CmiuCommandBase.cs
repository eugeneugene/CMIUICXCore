using CMIUICXCore.Code;
using CMIUICXCore.Converters;
using System;
using System.Text.Json.Serialization;

namespace CMIUICXCore.Types
{
    /// <summary>
    /// Родительский класс для команд от ЦМИУ, таких как <see cref="CmiuCommand" />, <see cref="CmiuPlacesCommand"/>, <see cref="CmiuSeasonCardCommand"/>
    /// Дочерний класс выбирается в <see cref="CmiuCommandConverter"/> по значению поля <see cref="Type"/>
    /// </summary>
    [JsonConverter(typeof(CmiuCommandConverter))]
    public class CmiuCommandBase : JsonToEnumString
    {
        /// <summary>
        /// Тип команды, например: command, places, season_card
        /// </summary>
        [JsonPropertyName("type")]
        public string Type { get; set; }

        /// <summary>
        /// Дата команды
        /// </summary>
        [JsonConverter(typeof(CmiuJsonDateTimeConverter))]
        [JsonPropertyName("date_event")]
        public DateTime DateEvent { get; set; }

        /// <summary>
        /// Ошибка, обычно равно 0
        /// </summary>
        [JsonPropertyName("error")]
        public uint Error { get; set; }
    }
}
