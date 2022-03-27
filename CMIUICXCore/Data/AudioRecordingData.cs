using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CMIUICXCore.Data
{
    [Table("AudioRecording", Schema = "dbo")]
    public class AudioRecordingData
    {
        [Key]
        public long Id { get; set; }
        public long StartTime { get; set; }
        public long EndTime { get; set; }
        public int TimeZoneOffsetToUTC { get; set; }
        public Guid DirectoryReference { get; set; }
        public string RecordingId { get; set; }
        public RecordingType RecordingType { get; set; }
        public RecordingStatus Status { get; set; }
        public string Mp3Path { get; set; }
        public double Duration { get; set; }
        public string UserNames { get; set; }
        public string SipNumber { get; set; }

        [NotMapped]
        public DateTime StartDateLocalTime => new DateTime(StartTime) + TimeSpan.FromHours(TimeZoneOffsetToUTC);

        [NotMapped]
        public DateTime EndDateLocalTime => new DateTime(EndTime) + TimeSpan.FromHours(TimeZoneOffsetToUTC);
    }
}
