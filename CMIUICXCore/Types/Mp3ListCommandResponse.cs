using CMIUICXCore.Converters;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace CMIUICXCore.Types
{
    public class Mp3ListCommandResponse : CmiuResponse
    {
        [JsonPropertyName("date_event")]
        [JsonConverter(typeof(CmiuJsonDateTimeConverter))]
        public DateTime DateEvent { get; }

        [JsonPropertyName("mp3_file_list")]
        public List<Mp3ListItem> Mp3List { get; }

        public Mp3ListCommandResponse(string Type, DateTime DateEvent, IEnumerable<Mp3ListItem> Mp3List, int Error)
            : base(Type ?? throw new ArgumentNullException(nameof(Type)), Error)
        {
            this.DateEvent = DateEvent;
            this.Mp3List = new List<Mp3ListItem>(Mp3List ?? throw new ArgumentNullException(nameof(Mp3List)));
        }
    }
}
