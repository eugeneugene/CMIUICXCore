using CMIUICXCore.Code;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CMIUICXCore.Types
{
    [Table("CMIUGW_PLACES")]
    public class CmiuPlacesData : JsonToString
    {
        [Key]
        public string Id { get; set; }

        public uint CmiuParkingNumber { get; set; }

        public int NonReserved { get; set; }

        public int NonReservedTotal { get; set; }

        public int Reserved { get; set; }

        public int ReservedTotal { get; set; }

        public DateTime ActionTime { get; set; }

        public CmiuPlacesData()
        { }

        public CmiuPlacesData(uint cmiuParkingNumber, int nonReserved, int nonReservedTotal, int reserved, int reservedTotal, DateTime actionTime)
        {
            CmiuParkingNumber = cmiuParkingNumber;
            NonReserved = nonReserved;
            NonReservedTotal = nonReservedTotal;
            Reserved = reserved;
            ReservedTotal = reservedTotal;
            ActionTime = actionTime;
        }
    }
}
