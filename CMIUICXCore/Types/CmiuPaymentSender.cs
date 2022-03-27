using System;
using System.Text.Json.Serialization;

namespace CMIUICXCore.Types
{
    public class CmiuPaymentSender : CmiuSenderBase
    {
        /// <summary>
        /// Номер устройства
        /// </summary>
        [JsonPropertyName("device_number")]
        public uint DeviceNumber { get; set; }

        /// <summary>
        /// Тип устройства
        /// </summary>
        [JsonPropertyName("device_type")]
        public CmiuDeviceType DeviceType { get; set; }

        /// <summary>
        /// Метод оплаты<see cref="CmiuPaymentMethod"/>
        /// </summary>
        [JsonPropertyName("payment_method")]
        public CmiuPaymentMethod PaymentMethod { get; set; }

        /// <summary>
        /// Метод оплаты дополнительно
        /// </summary>
        [JsonPropertyName("payment_method_addition")]
        public string PaymentMethodAddition { get; set; }

        /// <summary>
        /// Номер карты
        /// </summary>
        [JsonPropertyName("card")]
        public string Card { get; set; }

        /// <summary>
        /// Тип карты<see cref="CmiuCardType"/>
        /// </summary>
        [JsonPropertyName("card_type")]
        public CmiuCardType CardType { get; set; }

        /// <summary>
        /// Тип оплаты<see cref="CmiuPaymentType"/>
        /// </summary>
        [JsonPropertyName("payment_type")]
        public CmiuPaymentType PaymentType { get; set; }

        /// <summary>
        /// Первая или дополнительная оплата
        /// </summary>
        [JsonPropertyName("payment_count")]
        public CmiuPaymentCount PaymentCount { get; set; }

        /// <summary>
        /// Сумма, оплаченная клиентом в копейках
        /// </summary>
        [JsonPropertyName("price")]
        public long Price { get; set; }

        /// <summary>
        /// ActionUid
        /// </summary>
        [JsonPropertyName("action_uid")]
        public Guid ActionUid { get; set; }

        /// <summary>
        /// Уникальный идентификатор транзакции ЦМИУ
        /// </summary>
        [JsonPropertyName("transaction_uid")]
        public Guid TransactionUid { get; set; }

        /// <summary>
        /// Объект, хранящий данные оплаты сессии АИС ЕПП
        /// </summary>
        [JsonPropertyName("epp_session")]
        public EppPaymentSession EppSession { get; set; }


        public CmiuPaymentSender(CmiuPaymentData cmiuPayment)
            : base("payments", ExtractDateEvent(cmiuPayment), 0)
        {
            if (cmiuPayment == null)
                throw new ArgumentNullException(nameof(cmiuPayment));

            DeviceNumber = cmiuPayment.CmiuDeviceNumber;
            DeviceType = cmiuPayment.CmiuDeviceType;
            PaymentMethod = cmiuPayment.CmiuPaymentMethod;
            PaymentMethodAddition = cmiuPayment.PaymentMethodAddition;
            Card = cmiuPayment.Card;
            CardType = cmiuPayment.CmiuCardType;
            PaymentType = cmiuPayment.CmiuPaymentType;
            PaymentCount = cmiuPayment.CmiuPaymentCount;
            Price = cmiuPayment.Price;
            ActionUid = cmiuPayment.ActionUid;
            TransactionUid = cmiuPayment.TransactionUid;

            if (cmiuPayment.EppSession)
            {
                EppSession = new()
                {
                    SessionId = cmiuPayment.SessionId,
                    AisSessionType = cmiuPayment.AisSessionType,
                    AisAutoType = cmiuPayment.AisAutoType,
                    AisPaymentType = (AisPaymentType)cmiuPayment.AisPaymentType,
                    AisParkingOperation = (AisParkingOperation)cmiuPayment.Operation,
                    AisExemptionType = cmiuPayment.AisExemptionType,
                    FineCost = cmiuPayment.FineCost,
                    FinePaid = cmiuPayment.FinePaid,
                };
            }
        }

        public static DateTime ExtractDateEvent(CmiuPaymentData cmiuPayment)
        {
            if (cmiuPayment == null)
                throw new ArgumentNullException(nameof(cmiuPayment));
            return cmiuPayment.DateEvent;
        }
    }
}
