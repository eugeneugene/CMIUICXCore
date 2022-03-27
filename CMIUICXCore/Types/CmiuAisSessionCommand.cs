using System;
using System.Text.Json.Serialization;

namespace CMIUICXCore.Types
{
    /// <summary>
    /// Команды на управление сессиями АИС ЕПП
    /// </summary>
    public class CmiuAisSessionCommand : CmiuCommandBase
    {
        /// <summary>
        /// Наименование метода
        /// </summary>
        [JsonPropertyName("session_operation")]
        public string Operation { get; set; }

        /// <summary>
        /// Уникальный идентификатор парковочной сессии
        /// </summary>
        [JsonPropertyName("session_id")]
        public string SessionId { get; set; }

        /// <summary>
        /// Тип сессии; Необязательное поле 
        /// </summary>
        [JsonPropertyName("session_type")]
        public AisSessionType? SessionType { get; set; }

        /// <summary>
        /// Тип транспортного средства; Необязательное поле
        /// </summary>
        [JsonPropertyName("session_auto_type")]
        public AisAutoType? AutoType { get; set; }

        /// <summary>
        /// Уникальный идентификатор транзакции ЦМИУ
        /// </summary>
        [JsonPropertyName("transaction_uid")]
        public Guid TransactionUid { get; set; }
    }
}
