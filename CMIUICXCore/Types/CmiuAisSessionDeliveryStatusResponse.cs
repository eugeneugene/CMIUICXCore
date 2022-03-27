using System.Text.Json.Serialization;

namespace CMIUICXCore.Types
{
    public class CmiuAisSessionDeliveryStatusResponse : CmiuResponse
    {
        [JsonPropertyName("session_id")]
        public string SessionId { get; set; }

        public CmiuAisSessionDeliveryStatusResponse(string Type, string SessionId, int Error)
            : base(Type, Error)
        {
            this.SessionId = SessionId;
        }
    }
}
