using CMIUICXCore.Converters;
using System;
using System.Text.Json.Serialization;

namespace CMIUICXCore.Types
{
    /// <summary>
    /// Defines the <see cref="Mp3ListItem" />.
    /// </summary>
    public class Mp3ListItem
    {
        /// <summary>
        /// Gets or sets the Id.
        /// </summary>
        [JsonPropertyName("id")]
        public string Id { get; }

        /// <summary>
        /// Gets the RelativePath.
        /// </summary>
        [JsonPropertyName("relative_path")]
        public string RelativePath { get; }

        /// <summary>
        /// Gets the FileName.
        /// </summary>
        [JsonPropertyName("file_name")]
        public string FileName { get; }

        /// <summary>
        /// Gets the CreationDateTime.
        /// </summary>
        [JsonPropertyName("creation_date")]
        [JsonConverter(typeof(CmiuJsonDateTimeConverter))]
        public DateTime CreationDateTime { get; }

        /// <summary>
        /// Gets the FileSize.
        /// </summary>
        [JsonPropertyName("file_size")]
        public long FileSize { get; }

        /// <summary>
        /// Gets the Time.
        /// </summary>
        [JsonPropertyName("time")]
        public string Time { get; }

        /// <summary>
        /// Gets the Number1.
        /// </summary>
        [JsonPropertyName("number1")]
        public string Number1 { get; }

        /// <summary>
        /// Gets the Number2.
        /// </summary>
        [JsonPropertyName("number2")]
        public string Number2 { get; }

        /// <summary>
        /// Gets the Duration.
        /// </summary>
        [JsonPropertyName("duration")]
        public double Duration { get; }

        public Mp3ListItem(string Id, string RelativePath, string FileName, DateTime CreationDateTime, long FileSize, string Time, string Number1, string Number2, double Duration)
        {
            this.Id = Id;
            this.RelativePath = RelativePath;
            this.FileName = FileName;
            this.CreationDateTime = CreationDateTime;
            this.FileSize = FileSize;
            this.Time = Time;
            this.Number1 = Number1;
            this.Number2 = Number2;
            this.Duration = Duration;
        }
    }
}
