using CMIUICXCore.Code;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CMIUICXCore.Types
{
    /// <summary>
    /// Defines the <see cref="CmiuAisSessionDeliveryStatusData" />.
    /// </summary>
    [Table("CMIUGW_AISSESSION_DELIVERY_STATUS")]
    public class CmiuAisSessionDeliveryStatusData : JsonToEnumString
    {
        /// <summary>
        /// Ключевое поле
        /// </summary>
        [Key]
        public string Id { get; set; }

        /// <summary>
        /// уникальный идентификатор парковочной сессии
        /// </summary>
        [MaxLength(30)]
        [Display(Name = "Идентификатор сессии")]
        [Required]
        public string SessionId { get; set; }

        /// <summary>
        /// Номер парковки в системе ЦМИУ
        /// </summary>
        public uint CmiuParkingNumber { get; set; }


        /// <summary>
        /// Номер карты Entervo (EPAN)
        /// </summary>
        [MaxLength(30)]
        [Display(Name = "Номер карты Entervo (EPAN)")]
        [Required]
        public string Card { get; set; }

        /// <summary>
        /// Состояние доставки
        /// </summary>
        [Display(Name = "Состояние доставки")]
        public CmiuAisSessionDeliveryStatus? DeliveryStatus { get; set; }

        /// <summary>
        /// Время доставки
        /// </summary>
        [Display(Name = "Время доставки")]
        public DateTime? DeliveryTime { get; set; }

        /// <summary>
        /// Уникальный идентификатор транзакции ЦМИУ
        /// </summary>
        [Display(Name = "TransactionUid")]
        [Required]
        public Guid TransactionUid { get; set; }
    }
}
