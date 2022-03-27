using CMIUICXCore.Converters;
using System;
using System.Text.Json.Serialization;

namespace CMIUICXCore.Types
{
    public class CmiuCallCommandResponse : CmiuResponse
    {
        [JsonPropertyName("date_event")]
        [JsonConverter(typeof(CmiuJsonDateTimeConverter))]
        public DateTime DateEvent { get; set; }

        public CmiuCallCommandResponse(string Type, DateTime DateEvent, int Error)
            : base(Type, Error)
        {
            this.DateEvent = DateEvent;
        }
    }
}
