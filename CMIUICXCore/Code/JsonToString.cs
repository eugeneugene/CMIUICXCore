using CMIUICXCore.Converters;
using System;
using System.Globalization;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Unicode;

namespace CMIUICXCore.Code
{
    /// <summary>
    /// Универсальный конвертер типов в строки.
    /// </summary>
    public abstract class JsonToString
    {
        /// <summary>
        /// Перегруженный метод ToString.
        /// </summary>
        /// <returns>Строка.</returns>
        public override string ToString()
        {
            return ToString("");
        }

        /// <summary>
        /// The ToString.
        /// </summary>
        /// <param name="format">The format<see cref="string"/>.</param>
        /// <returns>The <see cref="string"/>.</returns>
        public string ToString(string format)
        {
            return MakeString(this, format);
        }

        /// <summary>
        /// The MakeString.
        /// </summary>
        /// <param name="ob">The ob<see cref="object"/>.</param>
        /// <returns>The <see cref="string"/>.</returns>
        public static string MakeString(object ob)
        {
            return MakeString(ob, "");
        }

        /// <summary>
        /// The MakeString.
        /// </summary>
        /// <param name="ob">The ob<see cref="object"/>.</param>
        /// <param name="format">The format<see cref="string"/>.</param>
        /// <returns>The <see cref="string"/>.</returns>
        public static string MakeString(object ob, string format)
        {
            if (ob == null)
                throw new ArgumentNullException(nameof(ob));
            if (format == null)
                throw new ArgumentNullException(nameof(format));

            var options = new JsonSerializerOptions()
            {
                Encoder = JavaScriptEncoder.Create(UnicodeRanges.BasicLatin, UnicodeRanges.Cyrillic),
                IgnoreNullValues = false,
            };
            options.Converters.Add(new JsonToStringNullableDateTimeConverter());
            options.Converters.Add(new JsonToStringDateTimeConverter());
            if (format.Contains('e'))
                options.Converters.Add(new JsonStringEnumConverter(new UpperCaseNamingPolicy()));
            var jsonUtf8Bytes = JsonSerializer.SerializeToUtf8Bytes(ob, ob.GetType(), options);
            return Encoding.UTF8.GetString(jsonUtf8Bytes);
        }
    }

    /// <summary>
    /// Defines the <see cref="JsonToEnumString" />.
    /// </summary>
    public abstract class JsonToEnumString : JsonToString
    {
        /// <summary>
        /// The ToString.
        /// </summary>
        /// <returns>The <see cref="string"/>.</returns>
        public override string ToString()
        {
            return ToString("e");
        }
    }

    /// <summary>
    /// Defines the <see cref="UpperCaseNamingPolicy" />.
    /// </summary>
    public class UpperCaseNamingPolicy : JsonNamingPolicy
    {
        /// <summary>
        /// The ConvertName.
        /// </summary>
        /// <param name="name">The name<see cref="string"/>.</param>
        /// <returns>The <see cref="string"/>.</returns>
        public override string ConvertName(string name) => name == null ? throw new ArgumentNullException(nameof(name)) : name.ToUpper(CultureInfo.CurrentCulture);
    }
}
