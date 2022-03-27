using CMIUICXCore.Converters;
using System;
using System.Text.Json.Serialization;

namespace CMIUICXCore.Types
{
    class Mp3ListErrorResponse : CmiuResponse
    {
        [JsonPropertyName("date_event")]
        [JsonConverter(typeof(CmiuJsonDateTimeConverter))]
        public DateTime DateEvent { get; }

        [JsonPropertyName("error_msg")]
        public string ErrorMsg { get; }

        public Mp3ListErrorResponse(string Type, DateTime DateEvent, string ErrorMsg, int Error)
            : base(Type, Error)
        {
            this.DateEvent = DateEvent;
            this.ErrorMsg = ErrorMsg;
        }
    }
}
