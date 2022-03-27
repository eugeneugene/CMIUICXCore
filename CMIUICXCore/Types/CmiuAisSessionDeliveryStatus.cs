namespace CMIUICXCore.Types
{
    public enum CmiuAisSessionDeliveryStatus
    {
        /// <summary>
        /// Не доставлено
        /// </summary>
        NotDelivered,
        /// <summary>
        /// Доставлено
        /// </summary>
        Delivered,
        /// <summary>
        /// Ошибка при доставке. ЕПП ответил ошибкой
        /// </summary>
        Error = 2,
    }
}
