using CMIUICXCore.Converters;
using System;
using System.Text.Json.Serialization;

namespace CMIUICXCore.Types
{
    /// <summary>
    /// Defines the <see cref="Mp3ListCommand" />.
    /// </summary>
    public class Mp3ListCommand : CmiuCommandBase
    {
        /// <summary>
        /// Gets or sets the DateTimeFrom.
        /// </summary>
        [JsonPropertyName("date_from")]
        [JsonConverter(typeof(CmiuJsonDateTimeConverter))]
        public DateTime? DateTimeFrom { get; set; }

        /// <summary>
        /// Gets or sets the DateTimeTill.
        /// </summary>
        [JsonPropertyName("date_till")]
        [JsonConverter(typeof(CmiuJsonDateTimeConverter))]
        public DateTime? DateTimeTill { get; set; }
    }
}
