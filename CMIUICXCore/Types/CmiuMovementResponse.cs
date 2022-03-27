using System.Text.Json.Serialization;

namespace CMIUICXCore.Types
{
    public class CmiuMovementResponse : CmiuResponse
    {
        [JsonPropertyName("device_number")]
        public int? DeviceNumber { get; set; }

        public CmiuMovementResponse(string Type, int? DeviceNumber, int Error)
        : base(Type, Error)
        {
            this.DeviceNumber = DeviceNumber;
        }
    }
}
