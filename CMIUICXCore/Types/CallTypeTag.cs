using System.Runtime.Serialization;

namespace CMIUICXCore.Types
{
    public enum CallTypeTag
    {
        [EnumMember(Value = "IDLE")]
        Idle,
        [EnumMember(Value = "FIRST_CALL")]
        FirstCall,
        [EnumMember(Value = "SECOND_CALL")]
        SecondCall,
        [EnumMember(Value = "DELETE_CALL")]
        DeleteCall,
        [EnumMember(Value = "ANSWER_CALL")]
        AnswerCall,
        [EnumMember(Value = "BUSY_CALL")]
        BusyCall,
        [EnumMember(Value = "END_CALL")]
        EndCall,
    }
}
