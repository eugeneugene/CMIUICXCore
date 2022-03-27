using System;
using System.Text.Json.Serialization;

namespace CMIUICXCore.Types
{
    public class CmiuAisSessionDeliveryStatusSender
    {
        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("session_id")]
        public string SessionId { get; set; }

        [JsonPropertyName("card")]
        public string Card { get; set; }

        [JsonPropertyName("parking_number")]
        public uint CmiuParkingNumber { get; set; }

        [JsonPropertyName("delivery_status")]
        public CmiuAisSessionDeliveryStatus? DeliveryStatus { get; set; }

        [JsonPropertyName("delivery_time")]
        public DateTime? DeliveryTime { get; set; }

        [JsonPropertyName("action_uid")]
        public Guid? ActionUid { get; set; }

        /// <summary>
        /// Уникальный идентификатор транзакции ЦМИУ
        /// </summary>
        [JsonPropertyName("transaction_uid")]
        public Guid TransactionUid { get; set; }

        public CmiuAisSessionDeliveryStatusSender(CmiuAisSessionDeliveryStatusData cmiuAisSessionDeliveryStatusData)
        {
            if (cmiuAisSessionDeliveryStatusData == null)
                throw new ArgumentNullException(nameof(cmiuAisSessionDeliveryStatusData));

            Type = "epp_session_delivery";
            SessionId = cmiuAisSessionDeliveryStatusData.SessionId;
            Card = cmiuAisSessionDeliveryStatusData.Card;
            CmiuParkingNumber = cmiuAisSessionDeliveryStatusData.CmiuParkingNumber;
            DeliveryStatus = cmiuAisSessionDeliveryStatusData.DeliveryStatus;
            DeliveryTime = cmiuAisSessionDeliveryStatusData.DeliveryTime;
            TransactionUid = cmiuAisSessionDeliveryStatusData.TransactionUid;
            ActionUid = null;
        }
    }
}
