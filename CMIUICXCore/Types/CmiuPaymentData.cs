using CMIUICXCore.Code;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CMIUICXCore.Types
{
    [Table("CMIUGW_PAYMENTS")]
    public class CmiuPaymentData : JsonToEnumString
    {
        [Key]
        public string Id { get; set; }

        [Required]
        public uint CmiuDeviceNumber { get; set; }

        [Required]
        public CmiuDeviceType CmiuDeviceType { get; set; }

        [Required]
        public CmiuPaymentMethod CmiuPaymentMethod { get; set; }

        [MaxLength(30)]
        public string PaymentMethodAddition { get; set; }

        [Required]
        [MaxLength(30)]
        public string Card { get; set; }

        [Required]
        public CmiuCardType CmiuCardType { get; set; }

        [Required]
        public CmiuPaymentType CmiuPaymentType { get; set; }

        [Required]
        public CmiuPaymentCount CmiuPaymentCount { get; set; }

        [Required]
        public long Price { get; set; }

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

        public AisPaymentType? AisPaymentType { get; set; }

        public AisExemptionType? AisExemptionType { get; set; }

        public decimal? FineCost { get; set; }

        public decimal? FinePaid { get; set; }

        public CmiuPaymentData()
        { }

        public CmiuPaymentData(uint cmiuDeviceNumber, CmiuDeviceType cmiuDeviceType, CmiuPaymentMethod cmiuPaymentMethod, string paymentMethodAddition, string card,
            CmiuCardType cmiuCardType, CmiuPaymentType cmiuPaymentType, CmiuPaymentCount cmiuPaymentCount, long price, Guid actionUid, Guid transactionUid, DateTime dateEvent,
            bool eppSession = false, string sessionId = null, AisParkingOperation? operation = null,
            AisSessionType? aisSessionType = null, AisAutoType? aisAutoType = null,
            AisPaymentType? aisPaymentType = null, AisExemptionType? aisExemptionType = null,
            decimal? fineCost = null, decimal? finePaid = null)
        {
            CmiuDeviceNumber = cmiuDeviceNumber;
            CmiuDeviceType = cmiuDeviceType;
            CmiuPaymentMethod = cmiuPaymentMethod;
            PaymentMethodAddition = paymentMethodAddition;
            Card = card;
            CmiuCardType = cmiuCardType;
            CmiuPaymentType = cmiuPaymentType;
            CmiuPaymentCount = cmiuPaymentCount;
            Price = price;
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
                AisPaymentType = aisPaymentType;
                AisExemptionType = aisExemptionType;
                FineCost = fineCost;
                FinePaid = finePaid;
            }
        }
    }
}
