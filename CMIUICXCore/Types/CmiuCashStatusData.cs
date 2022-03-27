using CMIUICXCore.Code;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CMIUICXCore.Types
{
    [Table("CMIUGW_CASHSTATUS")]
    public class CmiuCashStatusData : JsonToEnumString
    {
        [Key]
        public string Id { get; set; }

        [Required]
        public uint CmiuDeviceNumber { get; set; }

        [Required]
        public CmiuDeviceType CmiuDeviceType { get; set; }

        [Required]
        public uint MoneyValue { get; set; }

        [Required]
        public uint Quantity { get; set; }

        [Required]
        public CmiuMoney CmiuMoneyType { get; set; }

        [Required]
        public CmiuMoneyMethod CmiuMoneyMethodType { get; set; }

        [Required]
        public DateTime DateEvent { get; set; }

        public CmiuCashStatusData(uint cmiuDeviceNumber, CmiuDeviceType cmiuDeviceType, uint moneyValue, uint quantity, CmiuMoney cmiuMoneyType, CmiuMoneyMethod cmiuMoneyMethodType, DateTime dateEvent)
        {
            CmiuDeviceNumber = cmiuDeviceNumber;
            CmiuDeviceType = cmiuDeviceType;
            MoneyValue = moneyValue;
            Quantity = quantity;
            CmiuMoneyType = cmiuMoneyType;
            CmiuMoneyMethodType = cmiuMoneyMethodType;
            DateEvent = dateEvent;
        }

        public CmiuCashStatusData()
        { }
    }
}
