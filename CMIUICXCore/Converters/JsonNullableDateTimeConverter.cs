using System;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace CMIUICXCore.Converters
{
    public abstract class JsonNullableDateTimeConverterBase : JsonConverter<DateTime?>
    {
        protected abstract string Format { get; }
        protected abstract CultureInfo CurrentCultureInfo { get; }

        public override DateTime? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (DateTime.TryParseExact(reader.GetString(), Format, CurrentCultureInfo, DateTimeStyles.AssumeLocal, out DateTime result))
                return result;
            return null;
        }

        public override void Write(Utf8JsonWriter writer, DateTime? value, JsonSerializerOptions options)
        {
            if (writer == null)
                throw new ArgumentNullException(nameof(writer));
            if (!value.HasValue)
                writer.WriteNullValue();
            writer.WriteStringValue(value.Value.ToString(Format, CurrentCultureInfo));
        }

        public string Convert(DateTime? dateTime) => dateTime.HasValue ? dateTime.Value.ToString(Format, CurrentCultureInfo) : "null";
    }

    public sealed class JsonToStringNullableDateTimeConverter : JsonNullableDateTimeConverterBase
    {
        protected override string Format { get => "yyyy-MM-ddTHH:mm:ss.fff"; }
        protected override CultureInfo CurrentCultureInfo { get => new("ru-RU"); }
    }

    public sealed class IssNullableDateTimeConverter : JsonNullableDateTimeConverterBase
    {
        protected override string Format { get => "dd-MM-yyyy HH:mm:ss.fff"; }
        protected override CultureInfo CurrentCultureInfo { get => new("ru-RU"); }
    }

    public sealed class CmiuJsonNullableDateTimeConverter : JsonNullableDateTimeConverterBase
    {
        protected override string Format { get => "dd-MM-yyyy HH:mm:ss"; }
        protected override CultureInfo CurrentCultureInfo { get => new("ru-RU"); }
    }

    public sealed class SvcJsonNullableDateTimeConverter : JsonNullableDateTimeConverterBase
    {
        protected override string Format { get => "yyyy-MM-ddTHH:mm:ss.fff"; }
        protected override CultureInfo CurrentCultureInfo { get => new("ru-RU"); }
    }

    public sealed class AsopMetroJsonNullableDateTimeConverter : JsonNullableDateTimeConverterBase
    {
        protected override string Format { get => "yyyy-MM-ddTHH:mm:sszzz"; }
        protected override CultureInfo CurrentCultureInfo { get => new("ru-RU"); }
    }
}
