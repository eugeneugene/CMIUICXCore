using System;
using System.Text.Json.Serialization;

namespace CMIUICXCore.Types
{
    public class CallEventSenderEx : CallEventSender
    {
        [JsonPropertyName("prefix")]
        public string Prefix { get; }

        [JsonPropertyName("caller_id")]
        public string CallerId { get; }

        public CallEventSenderEx(string type, CallTypeTag call_type_tag, string call_number_1, string prefix, string call_number_2, string caller_id, DateTime date_event, uint error)
            : base(type, call_type_tag, call_number_1, call_number_2, date_event, error)
        {
            Prefix = prefix;
            CallerId = caller_id;
        }
    }
}
