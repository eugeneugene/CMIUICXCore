using System.Runtime.Serialization;

namespace CMIUICXCore.Types
{
    public enum CmiuMoneyMethod
    {
        [EnumMember(Value ="Подкрепление")]
        Cashbox = 1,
        [EnumMember(Value = "Сдача")]
        Dispenser = 2,
    }
}
