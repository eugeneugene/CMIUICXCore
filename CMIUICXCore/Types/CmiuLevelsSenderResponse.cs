using System.Text.Json.Serialization;

namespace CMIUICXCore.Types
{
    /// <summary>
    /// Исходящий ответ от сервера ЦМИУ на сервер ППЗТ в ответ на отправленную команду о смене свободных мест 'levels'
    /// </summary>
    public class CmiuLevelsSenderResponse : CmiuResponse
    {
        /// <summary>
        /// номер парковки по ЦМИУ.
        /// </summary>
        [JsonPropertyName("parking_number")]
        public uint? CmiuParkingNumber { get; set; }

        /// <summary>
        /// номер уровня по ЦМИУ.
        /// </summary>
        [JsonPropertyName("level_number")]
        public uint? CmiuLevelNumber { get; set; }

        public CmiuLevelsSenderResponse(string Type, uint? CmiuParkingNumber, uint? CmiuLevelNumber, int Error)
            : base(Type, Error)
        {
            this.CmiuParkingNumber = CmiuParkingNumber;
            this.CmiuLevelNumber = CmiuLevelNumber;
        }
    }
}
