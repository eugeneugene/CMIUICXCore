using System.Text.Json.Serialization;

namespace CMIUICXCore.Types
{
    public class CmiuCashStatusResponse : CmiuResponse
    {
        [JsonPropertyName("device_number")]
        public int? DeviceNumber { get; set; }
        public CmiuCashStatusResponse(string Type, int? DeviceNumber, int Error)
            : base(Type, Error)
        {
            this.DeviceNumber = DeviceNumber;
        }
    }
}
