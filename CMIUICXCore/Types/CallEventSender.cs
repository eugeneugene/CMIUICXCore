using CMIUICXCore.Code;
using CMIUICXCore.Converters;
using System;
using System.Text.Json.Serialization;

namespace CMIUICXCore.Types
{
    public class CallEventSender : JsonToEnumString
    {
        [JsonPropertyName("type")]
        public string Type { get; }

        [JsonPropertyName("call_type_tag")]
        [JsonConverter(typeof(JsonStringEnumMemberConverter))]
        public CallTypeTag CallTypeTag { get; }

        [JsonPropertyName("call_number_1")]
        public string CallNumber1 { get; }

        [JsonPropertyName("call_number_2")]
        public string CallNumber2 { get; }

        [JsonPropertyName("date_event")]
        [JsonConverter(typeof(CmiuJsonDateTimeConverter))]
        public DateTime DateEvent { get; }

        [JsonPropertyName("error")]
        public uint Error { get; }

        public CallEventSender(string type, CallTypeTag call_type_tag, string call_number_1, string call_number_2, DateTime date_event, uint error)
        {
            Type = type;
            CallTypeTag = call_type_tag;
            CallNumber1 = call_number_1;
            CallNumber2 = call_number_2;
            DateEvent = date_event;
            Error = error;
        }
    }
}
