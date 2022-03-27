using System.Text.Json.Serialization;

namespace CMIUICXCore.Types
{
    /// <summary>
    /// Команды управления оборудованием ППЗТ
    /// </summary>
    public class CmiuCommand : CmiuCommandBase
    {
        /// <summary>
        /// номер устройства по ЦМИУ
        /// </summary>
        [JsonPropertyName("device_number")]
        public uint DeviceNumber { get; set; }

        /// <summary>
        /// тип устройства по ЦМИУ
        /// </summary>
        [JsonPropertyName("device_type")]
        public CmiuDeviceType DeviceType { get; set; }

        /// <summary>
        /// номер команды по ЦМИУ
        /// </summary>
        [JsonPropertyName("command_number")]
        public uint CommandNumber { get; set; }

        /// <summary>
        /// номер транзакции по ЦМИУ
        /// </summary>
        [JsonPropertyName("device_events_id")]
        public ulong DeviceEventsId { get; set; }
    }
}
