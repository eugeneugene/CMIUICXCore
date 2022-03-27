using System;
using System.Text.Json.Serialization;

namespace CMIUICXCore.Types
{
    public class CmiuCashStatusSender : CmiuSenderBase
    {
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
        /// номиналы денег в копейках
        /// </summary>
        [JsonPropertyName("money_value")]
        public uint MoneyValue { get; }

        /// <summary>
        /// количество банкнот или монет
        /// </summary>
        [JsonPropertyName("quantity")]
        public uint Quantity { get; }

        /// <summary>
        /// тип денег
        /// </summary>
        [JsonPropertyName("money_type")]
        public CmiuMoney MoneyType { get; }

        /// <summary>
        /// money_method_type
        /// </summary>
        [JsonPropertyName("money_method_type")]
        public CmiuMoneyMethod MoneyMethodType { get; }

        public CmiuCashStatusSender(CmiuCashStatusData cmiuCashStatus)
            : base("money", ExtractDateEvent(cmiuCashStatus), 0)
        {
            if (cmiuCashStatus == null)
                throw new ArgumentNullException(nameof(cmiuCashStatus));

            DeviceNumber = cmiuCashStatus.CmiuDeviceNumber;
            DeviceType = cmiuCashStatus.CmiuDeviceType;
            MoneyValue = cmiuCashStatus.MoneyValue;
            Quantity = cmiuCashStatus.Quantity;
            MoneyType = cmiuCashStatus.CmiuMoneyType;
            MoneyMethodType = cmiuCashStatus.CmiuMoneyMethodType;
        }

        public static DateTime ExtractDateEvent(CmiuCashStatusData cmiuCashStatus)
        {
            if (cmiuCashStatus == null)
                throw new ArgumentNullException(nameof(cmiuCashStatus));
            return cmiuCashStatus.DateEvent;
        }
    }
}
