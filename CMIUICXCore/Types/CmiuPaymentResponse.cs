using System.Text.Json.Serialization;

namespace CMIUICXCore.Types
{
    public class CmiuPaymentResponse : CmiuResponse
    {
        [JsonPropertyName("device_number")]
        public int? DeviceNumber { get; set; }

        public CmiuPaymentResponse(string Type, int? DeviceNumber, int Error)
            : base(Type, Error)
        {
            this.DeviceNumber = DeviceNumber;
        }
    }
}
