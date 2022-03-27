using CMIUICXCore.Types;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace CMIUICXCore.Services.HttpClients
{
    /// <summary>
    /// Defines the <see cref="ICmiuHttpClient" />.
    /// </summary>
    public interface ICmiuHttpClient
    {
        /// <summary>
        /// The PostEventAsync.
        /// </summary>
        /// <param name="evnt">The evnt<see cref="CmiuEventSender"/>.</param>
        /// <param name="cancellationToken">The cancellationToken<see cref="CancellationToken"/>.</param>
        /// <returns>The <see cref="Task{CmiuEventResponse}"/>.</returns>
        Task<CmiuEventResponse> PostEventAsync(CmiuEventSender evnt, CancellationToken cancellationToken = default);

        /// <summary>
        /// The PostPlacesAsync.
        /// </summary>
        /// <param name="places">The places<see cref="CmiuPlacesSender"/>.</param>
        /// <param name="cancellationToken">The cancellationToken<see cref="CancellationToken"/>.</param>
        /// <returns>The <see cref="Task{CmiuPlacesSenderResponse}"/>.</returns>
        Task<CmiuPlacesSenderResponse> PostPlacesAsync(CmiuPlacesSender places, CancellationToken cancellationToken = default);

        /// <summary>
        /// The PostPlacesAsync.
        /// </summary>
        /// <param name="places">The places<see cref="CmiuPlacesSender"/>.</param>
        /// <param name="uri">The uri<see cref="Uri"/>.</param>
        /// <param name="cancellationToken">The cancellationToken<see cref="CancellationToken"/>.</param>
        /// <returns>The <see cref="Task{CmiuPlacesSenderResponse}"/>.</returns>
        Task<CmiuPlacesSenderResponse> PostPlacesAsync(CmiuPlacesSender places, Uri uri, CancellationToken cancellationToken = default);

        /// <summary>
        /// The PostLevelsAsync.
        /// </summary>
        /// <param name="levels">The places<see cref="CmiuLevelsSender"/>.</param>
        /// <param name="cancellationToken">The cancellationToken<see cref="CancellationToken"/>.</param>
        /// <returns>The <see cref="Task{CmiuLevelsSenderResponse}"/>.</returns>
        Task<CmiuLevelsSenderResponse> PostLevelsAsync(CmiuLevelsSender levels, CancellationToken cancellationToken = default);

        /// <summary>
        /// The PostLevelsAsync.
        /// </summary>
        /// <param name="levels">The places<see cref="CmiuLevelsSender"/>.</param>
        /// <param name="uri">The uri<see cref="Uri"/>.</param>
        /// <param name="cancellationToken">The cancellationToken<see cref="CancellationToken"/>.</param>
        /// <returns>The <see cref="Task{CmiuPlacesSenderResponse}"/>.</returns>
        Task<CmiuLevelsSenderResponse> PostLevelsAsync(CmiuLevelsSender levels, Uri uri, CancellationToken cancellationToken = default);

        /// <summary>
        /// The PostMovementAsync.
        /// </summary>
        /// <param name="movement">The movement<see cref="CmiuMovementSender"/>.</param>
        /// <param name="cancellationToken">The cancellationToken<see cref="CancellationToken"/>.</param>
        /// <returns>The <see cref="Task{CmiuMovementResponse}"/>.</returns>
        Task<CmiuMovementResponse> PostMovementAsync(CmiuMovementSender movement, CancellationToken cancellationToken = default);

        /// <summary>
        /// The PostPaymentAsync.
        /// </summary>
        /// <param name="payment">The payment<see cref="CmiuPaymentSender"/>.</param>
        /// <param name="cancellationToken">The cancellationToken<see cref="CancellationToken"/>.</param>
        /// <returns>The <see cref="Task{CmiuPaymentResponse}"/>.</returns>
        Task<CmiuPaymentResponse> PostPaymentAsync(CmiuPaymentSender payment, CancellationToken cancellationToken = default);

        /// <summary>
        /// The PostCashSendAsync.
        /// </summary>
        /// <param name="cashStatus">The cashStatus<see cref="CmiuCashStatusSender"/>.</param>
        /// <param name="cancellationToken">The cancellationToken<see cref="CancellationToken"/>.</param>
        /// <returns>The <see cref="Task{CmiuCashStatusResponse}"/>.</returns>
        Task<CmiuCashStatusResponse> PostCashSendAsync(CmiuCashStatusSender cashStatus, CancellationToken cancellationToken = default);

        /// <summary>
        /// The PostImageAsync.
        /// </summary>
        /// <param name="image">The image<see cref="ICollection{T}"/>.</param>
        /// <param name="cmiuHeaders">The cmiuHeaders<see cref="IDictionary{TKey, TValue}"/>.</param>
        /// <param name="cancellationToken">The cancellationToken<see cref="CancellationToken"/>.</param>
        /// <returns>The <see cref="Task{TResult}"/>.</returns>
        Task<bool> PostImageAsync(ICollection<byte> image, IDictionary<string, string> cmiuHeaders, CancellationToken cancellationToken = default);

        /// <summary>
        /// PostCallEventAsync
        /// </summary>
        /// <param name="callEvent">The lpr<see cref="CallEventSender"/>.</param>
        /// <param name="cancellationToken">The cancellationToken<see cref="CancellationToken"/>.</param>
        /// <returns>The <see cref="Task{CmiuResponse}"/>.</returns>
        Task<CmiuResponse> PostCallEventAsync(CallEventSender callEvent, CancellationToken cancellationToken = default);

        /// <summary>
        /// The PostLprAsync.
        /// </summary>
        /// <param name="lpr">The lpr<see cref="CmiuLprSender"/>.</param>
        /// <param name="cancellationToken">The cancellationToken<see cref="CancellationToken"/>.</param>
        /// <returns>The <see cref="Task{CmiuEventResponse}"/>.</returns>
        Task<CmiuEventResponse> PostLprAsync(CmiuLprSender lpr, CancellationToken cancellationToken = default);

        /// <summary>
        /// Информация о доставке сессий в АИС ЕПП
        /// </summary>
        /// <param name="aisSessionDeliveryStatus">The cashStatus<see cref="CmiuCashStatusSender"/>.</param>
        /// <param name="cancellationToken">The cancellationToken<see cref="CancellationToken"/>.</param>
        /// <returns>The <see cref="Task{CmiuAisSessionDeliveryStatusResponse}"/>.</returns>
        Task<CmiuAisSessionDeliveryStatusResponse> PostEppSessionDeliveryAsync(CmiuAisSessionDeliveryStatusSender aisSessionDeliveryStatus, CancellationToken cancellationToken = default);
    }
}
