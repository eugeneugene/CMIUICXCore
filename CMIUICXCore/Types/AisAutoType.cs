using System.Runtime.Serialization;

namespace CMIUICXCore.Types
{
    public enum AisAutoType
    {
        [EnumMember(Value = "AUTO")]
        Auto,
        [EnumMember(Value = "BUS")]
        Bus,
        [EnumMember(Value = "TRUCK")]
        Truck
    }
}
