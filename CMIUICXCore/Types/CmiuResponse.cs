using CMIUICXCore.Code;
using System.Text.Json.Serialization;

namespace CMIUICXCore.Types
{
    /// <summary>
    /// Базовый класс для исходящих ответов на север ЦМИУ
    /// </summary>
    public class CmiuResponse : JsonToEnumString
    {
        /// <summary>
        /// Тип ответа, например: 'command', 'places'
        /// </summary>
        [JsonPropertyName("type")]
        public string Type { get; }

        /// <summary>
        /// код ошибки
        /// 0 – нет ошибок,
        /// 1 – есть ошибка
        /// </summary>
        [JsonPropertyName("error")]
        public int Error { get; }

        public CmiuResponse(string Type, int Error)
        {
            this.Type = Type;
            this.Error = Error;
        }
    }
}
