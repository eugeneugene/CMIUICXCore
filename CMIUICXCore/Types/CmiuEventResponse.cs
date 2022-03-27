using System.Text.Json.Serialization;

namespace CMIUICXCore.Types
{
    public class CmiuEventResponse : CmiuResponse
    {
        [JsonPropertyName("device_number")]
        public int? DeviceNumber { get; set; }

        public CmiuEventResponse(string Type, int? DeviceNumber, int Error)
            : base(Type, Error)
        {
            this.DeviceNumber = DeviceNumber;
        }
    }
}
