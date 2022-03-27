using CMIUICXCore.Converters;
using System;
using System.Text.Json.Serialization;

namespace CMIUICXCore.Types
{
    /// <summary>
    /// Команды на управление абонементами ППЗТ
    /// </summary>
    public class CmiuSeasonCardCommand : CmiuCommandBase
    {
        /// <summary>
        /// номер парковки по ЦМИУ
        /// </summary>
        [JsonPropertyName("parking_number")]
        public uint ParkingNumber { get; set; }

        /// <summary>
        /// тип операции: create/select/modify/create_modify/delete
        /// </summary>
        [JsonConverter(typeof(JsonStringEnumMemberConverter))]
        [JsonPropertyName("action_type")]
        public CmiuSeasonCardActionType ActionType { get; set; }

        /// <summary>
        /// наименование карты (владелец)
        /// </summary>
        [JsonPropertyName("card_name")]
        public string CardName { get; set; }

        /// <summary>
        /// номер карты
        /// </summary>
        [JsonPropertyName("card_number")]
        public string CardNumber { get; set; }

        /// <summary>
        /// тип карты
        /// </summary>
        [JsonPropertyName("card_type")]
        public CmiuCardType? CardType { get; set; }

        /// <summary>
        /// начало срока действия карты
        /// </summary>
        [JsonConverter(typeof(CmiuJsonNullableDateTimeConverter))]
        [JsonPropertyName("valid_from")]
        public DateTime? ValidFrom { get; set; }

        /// <summary>
        /// окончание срока действия карты
        /// </summary>
        [JsonConverter(typeof(CmiuJsonNullableDateTimeConverter))]
        [JsonPropertyName("valid_to")]
        public DateTime? ValidTo { get; set; }

        /// <summary>
        /// нахождение карты: neutral, inside, outside
        /// </summary>
        [JsonConverter(typeof(JsonStringEnumMemberConverter))]
        [JsonPropertyName("location")]
        public Location? Location { get; set; }

        /// <summary>
        /// номер ГРЗ
        /// </summary>
        [JsonPropertyName("lpr")]
        public string LPR { get; set; }

        /// <summary>
        /// дополнительная информация
        /// </summary>
        [JsonPropertyName("addition_1")]
        public string Addition1 { get; set; }

        /// <summary>
        /// дополнительная информация
        /// </summary>
        [JsonPropertyName("addition_2")]
        public string Addition2 { get; set; }
    }
}
