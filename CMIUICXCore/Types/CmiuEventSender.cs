using System;
using System.Text.Json.Serialization;

namespace CMIUICXCore.Types
{
    /// <summary>
    /// Команда на отправка информации о произошедшем событии из Entervo в ЦМИУ
    /// Реакция на событие в контроллере"/>
    /// </summary>
    public class CmiuEventSender : CmiuSenderBase
    {
        /// <summary>
        /// Номер события
        /// </summary>
        [JsonPropertyName("status_number")]
        public uint StatusNumber { get; }

        /// <summary>
        /// Номер устройства
        /// </summary>
        [JsonPropertyName("device_number")]
        public uint DeviceNumber { get; }

        /// <summary>
        /// Тип устройства
        /// </summary>
        [JsonPropertyName("device_type")]
        public CmiuDeviceType DeviceType { get; }

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

        /// <summary>
        /// Инициализирует экземпляр класса <see cref="CmiuEventSender"/>
        /// </summary>
        /// <param name="statusNumber">Номер события<see cref="uint"/>.</param>
        /// <param name="deviceNumber">Номер устройства<see cref="uint"/>.</param>
        /// <param name="deviceType">Тип устройства<see cref="uint"/>.</param>
        /// <param name="actionUid">actionUid<see cref="Guid"/>.</param>
        /// <param name="transactionUid">transactionUid<see cref="Guid"/>.</param>
        /// <param name="dateEvent">Дата события<see cref="DateTime"/>.</param>
        public CmiuEventSender(uint statusNumber, uint deviceNumber, CmiuDeviceType deviceType, Guid actionUid, Guid transactionUid, DateTime dateEvent)
            : base("status", dateEvent, 0)
        {
            StatusNumber = statusNumber;
            DeviceNumber = deviceNumber;
            DeviceType = deviceType;
            ActionUid = actionUid;
            TransactionUid = transactionUid;
        }

        public CmiuEventSender(CmiuEventData cmiuEvent)
            : base("status", ExtractDateEvent(cmiuEvent), 0)
        {
            StatusNumber = cmiuEvent.CmiuStatusNumber;
            DeviceNumber = cmiuEvent.CmiuDeviceNumber;
            DeviceType = cmiuEvent.CmiuDeviceType;
            ActionUid = cmiuEvent.ActionUid;
            TransactionUid = cmiuEvent.TransactionUid;
        }

        public static DateTime ExtractDateEvent(CmiuEventData cmiuEvent)
        {
            if (cmiuEvent == null)
                throw new ArgumentNullException(nameof(cmiuEvent));
            return cmiuEvent.DateEvent;
        }
    }
}
