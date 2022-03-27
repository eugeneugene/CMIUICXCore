using System.Runtime.Serialization;

namespace CMIUICXCore.Types
{
    public enum AisSessionType
    {
        [EnumMember(Value = "TRANSACTION")]
        Transaction,
        [EnumMember(Value = "SUBSCRIPTION")]
        Subscription,
        [EnumMember(Value = "OFFICIAL")]
        Official,
        [EnumMember(Value = "HANDICAPPED")]
        Handicapped,
        [EnumMember(Value = "OVERDUE_SUBSCRIPTION")]
        OverdueSubscription,
        [EnumMember(Value = "LOST")]
        Lost,
        [EnumMember(Value = "UNAUTHORIZED")]
        Unauthorized,
        [EnumMember(Value = "CRASH")]
        Crash
    }
}
