using CMIUICXCore.Code;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CMIUICXCore.Types
{
    [Table("CMIUGW_EVENTS")]
    public class CmiuEventData : JsonToEnumString
    {
        [Key]
        public string Id { get; set; }

        public uint CmiuStatusNumber { get; set; }

        public uint CmiuDeviceNumber { get; set; }

        public CmiuDeviceType CmiuDeviceType { get; set; }

        public Guid ActionUid { get; set; }

        /// <summary>
        /// Уникальный идентификатор транзакции ЦМИУ
        /// </summary>
        public Guid TransactionUid { get; set; }

        public DateTime DateEvent { get; set; }

        public CmiuEventData(uint cmiuStatusNumber, uint cmiuDeviceNumber, CmiuDeviceType cmiuDeviceType, Guid actionUid, Guid transactionUid, DateTime dateEvent)
        {
            CmiuStatusNumber = cmiuStatusNumber;
            CmiuDeviceNumber = cmiuDeviceNumber;
            CmiuDeviceType = cmiuDeviceType;
            ActionUid = actionUid;
            TransactionUid = transactionUid;
            DateEvent = dateEvent;
        }

        public CmiuEventData()
        { }
    }
}
