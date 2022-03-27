using System.Runtime.Serialization;

namespace CMIUICXCore.Types
{
    public enum CmiuMoney
    {
        [EnumMember(Value ="Монеты")]
        Coins = 1,
        [EnumMember(Value = "Банкноты")]
        Notes = 2,
    }
}
