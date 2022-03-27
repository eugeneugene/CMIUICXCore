using System.Xml.Serialization;

namespace CMIUICXCore.Types
{
    public enum AisParkingOperation
    {
        [XmlEnum(Name = "parking_start")]
        ParkingStart,
        [XmlEnum(Name = "parking_check")]
        ParkingCheck,
        [XmlEnum(Name = "parking_stop")]
        ParkingStop,
        [XmlEnum(Name = "parking_extend")]
        ParkingExtend,
        [XmlEnum(Name = "time_check")]
        TimeCheck,
        [XmlEnum(Name = "zone_verify")]
        ZoneVerify,
        [XmlEnum(Name = "cmiu_parking_stop")]
        CmiuParkingStop,
    }
}
