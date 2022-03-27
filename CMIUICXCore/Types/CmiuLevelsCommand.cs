using System.Text.Json.Serialization;

namespace CMIUICXCore.Types
{
    /// <summary>
    /// Команда на изменение количества свободных мест на ППЗТ на уровнях
    /// Используется как команда от ЦМИУ на изменение мест в Entervo
    /// </summary>
    public class CmiuLevelsCommand : CmiuCommandBase
    {
        /// <summary>
        /// номер парковки по ЦМИУ
        /// </summary>
        [JsonPropertyName("parking_number")]
        public uint ParkingNumber { get; set; }

        /// <summary>
        /// номер уровня по ЦМИУ
        /// </summary>
        [JsonPropertyName("level_number")]
        public uint LevelNumber { get; set; }

        /// <summary>
        /// количество свободных мест
        /// </summary>
        [JsonPropertyName("free")]
        public int Free { get; set; }

        /// <summary>
        /// количество занятых мест
        /// </summary>
        [JsonPropertyName("busy")]
        public int Busy { get; set; }
    }
}
