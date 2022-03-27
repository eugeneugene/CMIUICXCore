using System.Text.Json.Serialization;

namespace CMIUICXCore.Types
{
    /// <summary>
    /// Команда на изменение количества свободных мест на ППЗТ
    /// Используется как команда от ЦМИУ на изменение мест в Entervo
    /// </summary>
    public class CmiuPlacesCommand : CmiuCommandBase
    {
        /// <summary>
        /// номер парковки по ЦМИУ
        /// </summary>
        [JsonPropertyName("parking_number")]
        public uint ParkingNumber { get; set; }

        /// <summary>
        /// количество свободных мест (разовые клиенты)
        /// </summary>
        [JsonPropertyName("client_free")]
        public int ClientFree { get; set; }

        /// <summary>
        /// количество занятых мест (разовые клиенты)
        /// </summary>
        [JsonPropertyName("client_busy")]
        public int ClientOccupied { get; set; }

        /// <summary>
        /// количество свободных мест (инвалиды)
        /// </summary>
        [JsonPropertyName("vip_client_free")]
        public int VipClientFree { get; set; }

        /// <summary>
        /// количество занятых мест (инвалиды)
        /// </summary>
        [JsonPropertyName("vip_client_busy")]
        public int VipClientOccupied { get; set; }
    }
}

