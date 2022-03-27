using System;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace CMIUICXCore.Converters
{
    public abstract class JsonDateTimeConverterBase : JsonConverter<DateTime>
    {
        protected abstract string Format { get; }
        protected abstract CultureInfo CurrentCultureInfo { get; }

        public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return DateTime.ParseExact(reader.GetString(), Format, CurrentCultureInfo, DateTimeStyles.AssumeLocal);
        }

        public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
        {
            if (writer == null)
                throw new ArgumentNullException(nameof(writer));
            writer.WriteStringValue(value.ToString(Format, CurrentCultureInfo));
        }

        public string Convert(DateTime dateTime) => dateTime.ToString(Format, CurrentCultureInfo);
    }

    public sealed class JsonToStringDateTimeConverter : JsonDateTimeConverterBase
    {
        protected override string Format { get => "yyyy-MM-ddTHH:mm:ss.fff"; }
        protected override CultureInfo CurrentCultureInfo { get => new("ru-RU"); }
    }

    public sealed class IssDateTimeConverter : JsonDateTimeConverterBase
    {
        protected override string Format { get => "dd-MM-yyyy HH:mm:ss.fff"; }
        protected override CultureInfo CurrentCultureInfo { get => new("ru-RU"); }
    }

    public sealed class CmiuJsonDateTimeConverter : JsonDateTimeConverterBase
    {
        protected override string Format { get => "dd-MM-yyyy HH:mm:ss"; }
        protected override CultureInfo CurrentCultureInfo { get => new("ru-RU"); }
    }

    public sealed class SvcJsonDateTimeConverter : JsonDateTimeConverterBase
    {
        protected override string Format { get => "yyyy-MM-ddTHH:mm:ss.fff"; }
        protected override CultureInfo CurrentCultureInfo { get => new("ru-RU"); }
    }

    public sealed class AsopMetroJsonDateTimeConverter : JsonDateTimeConverterBase
    {
        protected override string Format { get => "yyyy-MM-ddTHH:mm:sszzz"; }
        protected override CultureInfo CurrentCultureInfo { get => new("ru-RU"); }
    }
}
