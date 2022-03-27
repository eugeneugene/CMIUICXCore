using CMIUICXCore.Code;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CMIUICXCore.Types
{
    [Table("CMIUGW_LPR")]
    public class CmiuLprData : JsonToString
    {
        [Key]
        public string Id { get; set; }

        [Required]
        public string LprNumber { get; set; }

        public DateTime DateEvent { get; set; }

        public uint CmiuDeviceNumber { get; set; }

        public Guid ActionUid { get; set; }

        /// <summary>
        /// Уникальный идентификатор транзакции ЦМИУ
        /// </summary>
        public Guid TransactionUid { get; set; }

        public CmiuLprData(string LprNumber, DateTime DateEvent, uint CmiuDeviceNumber, Guid ActionUid, Guid TransactionUid)
        {
            this.LprNumber = LprNumber;
            this.DateEvent = DateEvent;
            this.CmiuDeviceNumber = CmiuDeviceNumber;
            this.ActionUid = ActionUid;
            this.TransactionUid = TransactionUid;
        }

        public CmiuLprData()
        { }
    }
}
