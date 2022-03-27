using CMIUICXCore.Code;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CMIUICXCore.Types
{
    [Table("CMIUGW_MOVEMENTS")]
    public class CmiuMovementData : JsonToEnumString
    {
        [Key]
        public string Id { get; set; }

        [Required]
        public MovementType MovementType { get; set; }

        [Required]
        public uint CmiuDeviceNumber { get; set; }

        [Required]
        public CmiuDeviceType CmiuDeviceType { get; set; }

        [Required]
        public CmiuTransactionType CmiuTransactionType { get; set; }

        [MaxLength(30)]
        [Required]
        public string Card { get; set; }

        [Required]
        public CmiuCardType CmiuCardType { get; set; }

        [MaxLength(20)]
        public string LPR { get; set; }

        [Required]
        public Guid ActionUid { get; set; }

        /// <summary>
        /// Уникальный идентификатор транзакции ЦМИУ
        /// </summary>
        [Required]
        public Guid TransactionUid { get; set; }

        [Required]
        public DateTime DateEvent { get; set; }

        [Required]
        public bool EppSession { get; set; }

        [MaxLength(20)]
        public string SessionId { get; set; }

        public AisParkingOperation? Operation { get; set; }

        public AisSessionType? AisSessionType { get; set; }

        public AisAutoType? AisAutoType { get; set; }

        public decimal? FineCost { get; set; }

        public decimal? FinePaid { get; set; }

        public CmiuMovementData()
        { }

        public CmiuMovementData(MovementType movementType, uint cmiuDeviceNumber, CmiuDeviceType cmiuDeviceType,
            CmiuTransactionType cmiuTransactionType, string card, CmiuCardType cmiuCardType, string lpr,
            Guid actionUid, Guid transactionUid, DateTime dateEvent,
            bool eppSession = false, string sessionId = null, AisParkingOperation? operation = null,
            AisSessionType? aisSessionType = null, AisAutoType? aisAutoType = null,
            decimal? fineCost = null, decimal? finePaid = null)
        {
            MovementType = movementType;
            CmiuDeviceNumber = cmiuDeviceNumber;
            CmiuDeviceType = cmiuDeviceType;
            CmiuTransactionType = cmiuTransactionType;
            Card = card;
            CmiuCardType = cmiuCardType;
            LPR = lpr;
            ActionUid = actionUid;
            TransactionUid = transactionUid;
            DateEvent = dateEvent;
            EppSession = eppSession;
            if (eppSession)
            {
                SessionId = sessionId;
                Operation = operation;
                AisSessionType = aisSessionType;
                AisAutoType = aisAutoType;
                FineCost = fineCost;
                FinePaid = finePaid;
            }
        }
    }
}
