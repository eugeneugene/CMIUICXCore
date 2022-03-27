using System;
using System.Text.Json.Serialization;

namespace CMIUICXCore.Types
{
    public class CmiuLprSender : CmiuSenderBase
    {
        [JsonPropertyName("lpr_number")]
        public string LprNumber { get; }

        [JsonPropertyName("device_number")]
        public uint DeviceNumber { get; }

        /// <summary>
        /// ActionUid.
        /// </summary>
        [JsonPropertyName("action_uid")]
        public Guid ActionUid { get; }

        /// <summary>
        /// Уникальный идентификатор транзакции ЦМИУ
        /// </summary>
        [JsonPropertyName("transaction_uid")]
        public Guid TransactionUid { get; }

        public CmiuLprSender(string lprNumber, uint deviceNumber, Guid actionUid, Guid transactionUid, DateTime dateEvent)
              : base("lpr", dateEvent, 0)
        {
            LprNumber = lprNumber;
            DeviceNumber = deviceNumber;
            ActionUid = actionUid;
            TransactionUid = transactionUid;
        }

        public CmiuLprSender(CmiuLprData cmiuLpr)
           : base("lpr", ExtractDateEvent(cmiuLpr), 0)
        {
            LprNumber = cmiuLpr.LprNumber;
            DeviceNumber = cmiuLpr.CmiuDeviceNumber;
            ActionUid = cmiuLpr.ActionUid;
            TransactionUid = cmiuLpr.TransactionUid;
        }

        public static DateTime ExtractDateEvent(CmiuLprData cmiuLpr)
        {
            if (cmiuLpr == null)
                throw new ArgumentNullException(nameof(cmiuLpr));
            return cmiuLpr.DateEvent;
        }
    }
}
