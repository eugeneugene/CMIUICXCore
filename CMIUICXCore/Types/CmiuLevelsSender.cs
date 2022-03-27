using System;
using System.Text.Json.Serialization;

namespace CMIUICXCore.Types
{
    /// <summary>
    /// Команда на изменение количества свободных мест на ППЗТ
    /// Используется как команда от Entervo в ЦМИУ на изменение мест в ЦМИУ"/>
    /// </summary>
    public class CmiuLevelsSender : CmiuSenderBase
    {
        /// <summary>
        /// номер парковки по ЦМИУ
        /// </summary>
        [JsonPropertyName("parking_number")]
        public uint CmiuParkingNumber { get; }

        /// <summary>
        /// номер уровня по ЦМИУ
        /// </summary>
        [JsonPropertyName("level_number")]
        public uint CmiuLevelNumber { get; }

        /// <summary>
        /// количество свободных мест
        /// </summary>
        [JsonPropertyName("free")]
        public int Free { get; }

        /// <summary>
        /// количество занятых мест
        /// </summary>
        [JsonPropertyName("busy")]
        public int Busy { get; }

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="CmiuPlacesSender"/>
        /// </summary>
        /// <param name="cmiuParkingNumber">номер парковки по ЦМИУ<see cref="uint"/>.</param>
        /// <param name="cmiuLevelNumber">номер уровня по ЦМИУ<see cref="uint"/>.</param>
        /// <param name="free">количество свободных мест<see cref="int"/>.</param>
        /// <param name="busy">количество занятых мест<see cref="int"/>.</param>
        /// <param name="dateEvent">Дата команды<see cref="DateTime"/>.</param>
        public CmiuLevelsSender(uint cmiuParkingNumber, uint cmiuLevelNumber, int free, int busy, DateTime dateEvent)
            : base("places", dateEvent, 0)
        {
            CmiuParkingNumber = cmiuParkingNumber;
            CmiuLevelNumber = cmiuLevelNumber;
            Free = free;
            Busy = busy;
        }

        public CmiuLevelsSender(CmiuLevelsData cmiuLevels)
            : base("places", ExtractActionTime(cmiuLevels), 0)
        {
            CmiuParkingNumber = cmiuLevels.CmiuParkingNumber;
            CmiuLevelNumber = cmiuLevels.CmiuLevelNumber;
            Free = cmiuLevels.Free;
            Busy = cmiuLevels.Busy;
        }

        public static DateTime ExtractActionTime(CmiuLevelsData cmiuLevels)
        {
            if (cmiuLevels == null)
                throw new ArgumentNullException(nameof(cmiuLevels));
            return cmiuLevels.ActionTime;
        }
    }
}
