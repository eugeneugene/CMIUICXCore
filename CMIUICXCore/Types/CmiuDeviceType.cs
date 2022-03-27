using System.Runtime.Serialization;

namespace CMIUICXCore.Types
{
    public enum CmiuDeviceType
    {
        [EnumMember(Value = "Сервер парковки")]
        Server = 1,
        [EnumMember(Value = "Въезд")]
        Entry = 2,
        [EnumMember(Value = "Выезд")]
        Exit = 3,
        [EnumMember(Value = "Касса")]
        Cashier = 4,
        [EnumMember(Value = "Сервер видеонаблюдения")]
        VideoServer = 5,
        [EnumMember(Value = "Сервер ГРЗ")]
        ServerLPR = 6,
        [EnumMember(Value = "Сервер Интерком")]
        ServerIntercom = 7,
        [EnumMember(Value = "Маршрутизатор")]
        Router = 8,
        [EnumMember(Value = "Коммутатор")]
        Switch = 9,
        [EnumMember(Value = "Камера видеонаблюдения/ГРЗ")]
        Camera = 10,
        [EnumMember(Value = "Абонентское устройство интерком")]
        Intercom = 11,
        [EnumMember(Value = "Информационная стела")]
        Stela = 15,
        [EnumMember(Value = "Сервер Стрит-Фалькон")]
        StreetFalconServer = 16,
        [EnumMember(Value = "Все устройства")]
        All = int.MaxValue,
    }
}
