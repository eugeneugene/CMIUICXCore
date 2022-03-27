using CMIUICXCore.Code;
using System;
using System.Text.Json.Serialization;

namespace CMIUICXCore.Types
{
    /// <summary>
    /// Defines the <see cref="CmiuMovementSender" />.
    /// </summary>
    public class CmiuMovementSender : CmiuSenderBase
    {
        /// <summary>
        /// Номер устройства.
        /// </summary>
        [JsonPropertyName("device_number")]
        public uint DeviceNumber { get; set; }

        /// <summary>
        /// Тип устройства.
        /// </summary>
        [JsonPropertyName("device_type")]
        public CmiuDeviceType DeviceType { get; set; }

        /// <summary>
        /// Тип транзакции<see cref="CmiuTransactionType"/>.
        /// </summary>
        [JsonPropertyName("transaction_type")]
        public CmiuTransactionType TransactionType { get; set; }

        /// <summary>
        /// Номер карты.
        /// </summary>
        [JsonPropertyName("card")]
        public string Card { get; set; }

        /// <summary>
        /// Тип карты<see cref="CmiuCardType"/>.
        /// </summary>
        [JsonPropertyName("card_type")]
        public CmiuCardType CardType { get; set; }

        /// <summary>
        /// ГРЗ.
        /// </summary>
        [JsonPropertyName("lpr")]
        public string LPR { get; set; }

        /// <summary>
        /// Уникальный идентификатор события ЦМИУ
        /// </summary>
        [JsonPropertyName("action_uid")]
        public Guid ActionUid { get; set; }

        /// <summary>
        /// Уникальный идентификатор транзакции ЦМИУ
        /// </summary>
        [JsonPropertyName("transaction_uid")]
        public Guid TransactionUid { get; set; }

        /// <summary>
        /// Объект, хранящий данные открытия/закрытия сессии АИС ЕПП
        /// </summary>
        [JsonPropertyName("epp_session")]
        public EppMovementSession EppSession { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="CmiuMovementSender"/> class.
        /// </summary>
        /// <param name="cmiuMovement">The cmiuMovement<see cref="CmiuMovementData"/>.</param>
        public CmiuMovementSender(CmiuMovementData cmiuMovement)
            : base(MovementTypeEnumMember(cmiuMovement), cmiuMovement.DateEvent, 0)
        {
            DeviceNumber = cmiuMovement.CmiuDeviceNumber;
            DeviceType = cmiuMovement.CmiuDeviceType;
            TransactionType = cmiuMovement.CmiuTransactionType;
            Card = cmiuMovement.Card;
            CardType = cmiuMovement.CmiuCardType;
            LPR = cmiuMovement.LPR;
            ActionUid = cmiuMovement.ActionUid;
            TransactionUid = cmiuMovement.TransactionUid;

            if (cmiuMovement.EppSession)
            {
                EppSession = new()
                {
                    SessionId = cmiuMovement.SessionId,
                    AisSessionType = cmiuMovement.AisSessionType,
                    AisParkingOperation = (AisParkingOperation)cmiuMovement.Operation,
                    AisAutoType = cmiuMovement.AisAutoType,
                    FineCost = cmiuMovement.FineCost,
                    FinePaid = cmiuMovement.FinePaid,
                };
            }
        }

        /// <summary>
        /// The MovementTypeEnumMember.
        /// </summary>
        /// <param name="cmiuMovement">The cmiuMovement<see cref="CmiuMovementData"/>.</param>
        /// <returns>The <see cref="string"/>.</returns>
        private static string MovementTypeEnumMember(CmiuMovementData cmiuMovement)
        {
            if (cmiuMovement == null)
                throw new ArgumentNullException(nameof(cmiuMovement));

            return cmiuMovement.MovementType.EnumMember();
        }
    }
}
