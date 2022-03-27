using System.Text.Json.Serialization;

namespace CMIUICXCore.Types
{
    /// <summary>
    /// Исходящий ответ от сервера ЦМИУ на сервер ППЗТ в ответ на отправленную команду о смене свободных мест 'places'
    /// </summary>
    public class CmiuPlacesSenderResponse : CmiuResponse
    {
        /// <summary>
        /// номер парковки по ЦМИУ.
        /// </summary>
        [JsonPropertyName("parking_number")]
        public uint? CmiuParkingNumber { get; set; }

        public CmiuPlacesSenderResponse(string Type, uint? CmiuParkingNumber, int Error)
            : base(Type, Error)
        {
            this.CmiuParkingNumber = CmiuParkingNumber;
        }
    }
}
