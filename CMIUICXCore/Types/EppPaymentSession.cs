using CMIUICXCore.Converters;
using System.Text.Json.Serialization;

namespace CMIUICXCore.Types
{
    /// <summary>
    /// Defines the <see cref="EppPaymentSession" />.
    /// </summary>
    public class EppPaymentSession
    {
        /// <summary>
        /// Уникальный идентификатор парковочной сессии
        /// </summary>
        [JsonPropertyName("session_id")]
        public string SessionId { get; set; }

        /// <summary>
        /// Наименование метода; Обязательное поле
        /// </summary>
        [JsonPropertyName("session_operation")]
        [JsonConverter(typeof(AisParkingOperationJsonConverter))]
        public AisParkingOperation AisParkingOperation { get; set; }

        /// <summary>
        /// Тип сессии
        /// </summary>
        [JsonPropertyName("session_type")]
        public AisSessionType? AisSessionType { get; set; }

        /// <summary>
        /// Тип транспортного средства
        /// </summary>
        [JsonPropertyName("session_auto_type")]
        public AisAutoType? AisAutoType { get; set; }

        /// <summary>
        /// Тип платежа
        /// </summary>
        [JsonPropertyName("session_payment_type")]
        public AisPaymentType AisPaymentType { get; set; }

        /// <summary>
        /// Тип скидки
        /// </summary>
        [JsonPropertyName("session_exemption_type")]
        public AisExemptionType? AisExemptionType { get; set; }

        /// <summary>
        /// Начисленная сумма штрафа
        /// </summary>
        [JsonPropertyName("session_fine_cost")]
        public decimal? FineCost { get; set; }

        /// <summary>
        /// Сумма оплаченного штрафа
        /// </summary>
        [JsonPropertyName("session_fine_paid")]
        public decimal? FinePaid { get; set; }
    }
}
