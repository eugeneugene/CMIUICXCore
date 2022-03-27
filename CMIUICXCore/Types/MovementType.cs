using System.Runtime.Serialization;

namespace CMIUICXCore.Types
{
    public enum MovementType
    {
        [EnumMember(Value = "enters")]
        Enter,
        [EnumMember(Value = "exits")]
        Exit,
    }
}
