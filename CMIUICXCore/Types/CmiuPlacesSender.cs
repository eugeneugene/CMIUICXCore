using System;
using System.Text.Json.Serialization;

namespace CMIUICXCore.Types
{
    /// <summary>
    /// Команда на изменение количества свободных мест на ППЗТ
    /// Используется как команда от Entervo в ЦМИУ на изменение мест в ЦМИУ"/>
    /// </summary>
    public class CmiuPlacesSender : CmiuSenderBase
    {
        /// <summary>
        /// номер парковки по ЦМИУ
        /// </summary>
        [JsonPropertyName("parking_number")]
        public uint CmiuParkingNumber { get; }

        /// <summary>
        /// количество свободных мест (разовые клиенты)
        /// </summary>
        [JsonPropertyName("client_free")]
        public int ClientFree { get; }

        /// <summary>
        /// количество занятых мест (разовые клиенты)
        /// </summary>
        [JsonPropertyName("client_busy")]
        public int ClientOccupied { get; }

        /// <summary>
        /// количество свободных мест (инвалиды)
        /// </summary>
        [JsonPropertyName("vip_client_free")]
        public int VipClientFree { get; }

        /// <summary>
        /// количество занятых мест (инвалиды)
        /// </summary>
        [JsonPropertyName("vip_client_busy")]
        public int VipClientOccupied { get; }

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="CmiuPlacesSender"/>
        /// </summary>
        /// <param name="cmiuParkingNumber">номер парковки по ЦМИУ<see cref="uint"/>.</param>
        /// <param name="clientFree">количество свободных мест (разовые клиенты)<see cref="int"/>.</param>
        /// <param name="clientOccupied">количество занятых мест (разовые клиенты)<see cref="int"/>.</param>
        /// <param name="vipClientFree">количество свободных мест (инвалиды)<see cref="int"/>.</param>
        /// <param name="vipClientOccupied">количество занятых мест (инвалиды)<see cref="int"/>.</param>
        /// <param name="dateEvent">Дата команды<see cref="DateTime"/>.</param>
        public CmiuPlacesSender(uint cmiuParkingNumber, int clientFree, int clientOccupied, int vipClientFree, int vipClientOccupied, DateTime dateEvent)
            : base("places", dateEvent, 0)
        {
            CmiuParkingNumber = cmiuParkingNumber;
            ClientFree = clientFree;
            ClientOccupied = clientOccupied;
            VipClientFree = vipClientFree;
            VipClientOccupied = vipClientOccupied;
        }

        public CmiuPlacesSender(CmiuPlacesData cmiuPlaces)
            : base("places", ExtractActionTime(cmiuPlaces), 0)
        {
            CmiuParkingNumber = cmiuPlaces.CmiuParkingNumber;
            ClientFree = cmiuPlaces.NonReservedTotal - cmiuPlaces.NonReserved; 
            ClientOccupied = cmiuPlaces.NonReserved;
            VipClientFree = cmiuPlaces.ReservedTotal - cmiuPlaces.Reserved;
            VipClientOccupied = cmiuPlaces.Reserved;
        }

        public static DateTime ExtractActionTime(CmiuPlacesData cmiuPlaces)
        {
            if (cmiuPlaces == null)
                throw new ArgumentNullException(nameof(cmiuPlaces));
            return cmiuPlaces.ActionTime;
        }
    }
}
