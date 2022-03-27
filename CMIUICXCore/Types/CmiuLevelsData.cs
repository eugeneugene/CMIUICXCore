using CMIUICXCore.Code;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CMIUICXCore.Types
{
    [Table("CMIUGW_LEVELS")]
    public class CmiuLevelsData : JsonToString
    {
        [Key]
        public string Id { get; set; }

        public uint CmiuParkingNumber { get; set; }

        public uint CmiuLevelNumber { get; set; }

        public int Free { get; set; }

        public int Busy { get; set; }

        public DateTime ActionTime { get; set; }

        public CmiuLevelsData()
        { }

        public CmiuLevelsData(uint cmiuParkingNumber, uint cmiuLevelNumber, int free, int busy, DateTime actionTime)
        {
            CmiuParkingNumber = cmiuParkingNumber;
            CmiuLevelNumber = cmiuLevelNumber;
            Free = free;
            Busy = busy;
            ActionTime = actionTime;
        }
    }
}
