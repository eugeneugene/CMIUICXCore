using System.Runtime.Serialization;

namespace CMIUICXCore.Types
{
    public enum AisExemptionType
    {
        [EnumMember(Value = "RZD")]
        rzd,
        [EnumMember(Value = "METRO")]
        metro
    }
}
