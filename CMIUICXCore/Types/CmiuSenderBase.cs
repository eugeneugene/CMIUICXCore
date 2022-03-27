using CMIUICXCore.Code;
using CMIUICXCore.Converters;
using System;
using System.Text.Json.Serialization;

namespace CMIUICXCore.Types
{
    /// <summary>
    /// Родительский класс для событий с ППЗТ: <see cref="CmiuEventSender" />, <see cref="CmiuMovementSender"/>, <see cref="CmiuPlacesSender"/>,
    /// <see cref="CmiuPaymentSender"/>, <see cref="CmiuCashStatusSender"/>, <see cref="CmiuLprSender"/>
    /// </summary>
    public class CmiuSenderBase : JsonToEnumString
    {
        /// <summary>
        /// Тип команды, например: command, places, season_card
        /// </summary>
        [JsonPropertyName("type")]
        public string Type { get; }

        /// <summary>
        /// Дата команды
        /// </summary>
        [JsonConverter(typeof(CmiuJsonDateTimeConverter))]
        [JsonPropertyName("date_event")]
        public DateTime DateEvent { get; }

        /// <summary>
        /// Ошибка, обычно равно 0
        /// </summary>
        [JsonPropertyName("error")]
        public uint Error { get; }

        public CmiuSenderBase(string type, DateTime dateEvent, uint error)
        {
            Type = type;
            DateEvent = dateEvent;
            Error = error;
        }
    }
}
