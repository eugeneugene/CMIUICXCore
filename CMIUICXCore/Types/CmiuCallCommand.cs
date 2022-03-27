using System.Text.Json.Serialization;

namespace CMIUICXCore.Types
{
    public class CmiuCallCommand : CmiuCommandBase
    {
        [JsonPropertyName("call_type_tag")]
        public string CallTypeTag { get; set; }

        [JsonPropertyName("call_number_1")]
        public string CallNumber1 { get; set; }

        [JsonPropertyName("call_number_2")]
        public string CallNumber2 { get; set; }
    }
}
