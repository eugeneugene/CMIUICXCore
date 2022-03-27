using System.ComponentModel.DataAnnotations;

namespace CMIUICXCore.Types
{
    public enum AisPaymentType
    {
        [Display(Name = "Наличные")]
        Cash = 0,
        [Display(Name = "Банковская карта")]
        BankCard = 1,
        [Display(Name = "Парковочный счет")]
        ParkingAccount = 2,
        [Display(Name = "Парковочная карта")]
        ParkingCard = 3,
        [Display(Name = "Элекснет")]
        Eleksnet = 50,
        [Display(Name = "Тройка")]
        Troyka = 51,
        // Тип 'мобильное приложение' не отправляется в АИС ЕПП
        [Display(Name = "Мобильное приложение")]
        Mobile = 100,
    }
}
