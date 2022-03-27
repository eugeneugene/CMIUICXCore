using System.Runtime.Serialization;

namespace CMIUICXCore.Types
{
    public enum CmiuSeasonCardActionType
    {
        [EnumMember(Value = "create")]
        Create,
        [EnumMember(Value = "select")]
        Select,
        [EnumMember(Value = "modify")]
        Modify,
        [EnumMember(Value = "create_modify")]
        CreateModify,
        [EnumMember(Value = "delete")]
        Delete
    }
}
