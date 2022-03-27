using System.Runtime.Serialization;

namespace CMIUICXCore.Types
{
    public enum Location
    {
        [EnumMember(Value = "neutral")]
        Neutral,
        [EnumMember(Value = "inside")]
        Inside,
        [EnumMember(Value = "outside")]
        Outside
    }
}
