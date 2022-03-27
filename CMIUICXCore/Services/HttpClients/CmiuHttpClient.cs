using CMIUICXCore.Types;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SBCommon.HttpClients.New;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace CMIUICXCore.Services.HttpClients
{
    /// <summary>
    /// Defines the <see cref="CmiuHttpClient" />.
    /// </summary>
    public class CmiuHttpClient : ICmiuHttpClient
    {
        private readonly ILogger _logger;
        private readonly IHttpRequestFactory _httpRequestFactory;
        private readonly CmiuUriProvider _cmiuUriProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="CmiuHttpClient"/> class.
        /// </summary>
        /// <param name="logger">The logger<see cref="ILogger{CmiuHttpClient}"/>.</param>
        /// <param name="httpRequestFactory">The HttpRequestFactory<see cref="IHttpRequestFactory"/>.</param>
        /// <param name="cmiuUriProvider">The cmiuUriProvider<see cref="CmiuUriProvider"/>.</param>
        public CmiuHttpClient(ILogger<CmiuHttpClient> logger, HttpRequestFactory<CmiuHttpClientProvider> httpRequestFactory, CmiuUriProvider cmiuUriProvider)
        {
            _logger = logger;
            _httpRequestFactory = httpRequestFactory;
            _cmiuUriProvider = cmiuUriProvider;
            _logger.LogTrace("Создание экземпляра {0}", GetType().Name);
        }

        /// <summary>
        /// The TimeoutSetter.
        /// </summary>
        /// <param name="provider">The provider<see cref="IServiceProvider"/>.</param>
        /// <param name="client">The client<see cref="HttpClient"/>.</param>
        public static void TimeoutSetter(IServiceProvider provider, HttpClient client)
        {
            if (provider == null)
                throw new ArgumentNullException(nameof(provider));
            if (client == null)
                throw new ArgumentNullException(nameof(client));

            var _configuration = (IConfiguration)provider.GetService(typeof(IConfiguration));
            var _timeout = _configuration.GetValue("CmiuHttpClient:CmiuServerTimeout", 0U);
            if (_timeout != 0U)
                client.Timeout = TimeSpan.FromSeconds(_timeout);
        }

        /// <summary>
        /// The PostEventAsync.
        /// </summary>
        /// <param name="evnt">The evnt<see cref="CmiuEventSender"/>.</param>
        /// <param name="cancellationToken">The cancellationToken<see cref="CancellationToken"/>.</param>
        /// <returns>The <see cref="Task{CmiuEventResponse}"/>.</returns>
        public async Task<CmiuEventResponse> PostEventAsync(CmiuEventSender evnt, CancellationToken cancellationToken)
        {
            if (evnt == null)
                throw new ArgumentNullException(nameof(evnt));

            CmiuEventResponse eventResponse = null;

            var httpRequest = _httpRequestFactory.CreateRequest(new PostMethodProvider(), new CmiuEventsUriExtension(_cmiuUriProvider));
            var httpResponseMessage = await httpRequest
                .HandleRequest(new RequestHeadersAcceptDecorator<AppJsonAcceptHeader>(new AppJsonAcceptHeader()))
                .HandleRequest(new RequestJsonBodyWithLogDecorator<CmiuEventSender>(_logger, evnt))
                .HandleRequest(new RequestLogDecorator(_logger))
                .SendAsync(cancellationToken);

            try
            {
                var httpResponse = new HttpResponse(httpResponseMessage);
                httpResponse
                    .HandleResponse(new ResponseLogDecorator(_logger))
                    .HandleResponse(new ResponseSuccessWithLogHandler<CmiuEventResponse>(_logger, item =>
                    {
                        eventResponse = item;
                        _logger.LogDebug("Event: {0}", item);
                    }))
                    .HandleResponse(new ResponseErrorStringWithLogHandler(_logger, item =>
                    {
                        _logger.LogError("Error: {0}", item);
                    }));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
            }
            return eventResponse;
        }

        /// <summary>
        /// The PostPlacesAsync.
        /// </summary>
        /// <param name="places">The places<see cref="CmiuPlacesSender"/>.</param>
        /// <param name="cancellationToken">The cancellationToken<see cref="CancellationToken"/>.</param>
        /// <returns>The <see cref="Task{CmiuPlacesSenderResponse}"/>.</returns>
        public async Task<CmiuPlacesSenderResponse> PostPlacesAsync(CmiuPlacesSender places, CancellationToken cancellationToken)
        {
            return await PostPlacesAsync(places, new CmiuPlacesUriExtension(_cmiuUriProvider).Uri, cancellationToken);
        }

        /// <summary>
        /// The PostPlacesAsync.
        /// </summary>
        /// <param name="places">The places<see cref="CmiuPlacesSender"/>.</param>
        /// <param name="uri">The uri<see cref="Uri"/>.</param>
        /// <param name="cancellationToken">The cancellationToken<see cref="CancellationToken"/>.</param>
        /// <returns>The <see cref="Task{CmiuPlacesSenderResponse}"/>.</returns>
        public async Task<CmiuPlacesSenderResponse> PostPlacesAsync(CmiuPlacesSender places, Uri uri, CancellationToken cancellationToken)
        {
            if (places == null)
                throw new ArgumentNullException(nameof(places));
            if (uri == null)
                throw new ArgumentNullException(nameof(uri));

            CmiuPlacesSenderResponse placesResponse = null;

            var httpRequest = _httpRequestFactory.CreateRequest(new PostMethodProvider(), uri);
            var httpResponseMessage = await httpRequest
                .HandleRequest(new RequestHeadersAcceptDecorator<AppJsonAcceptHeader>(new AppJsonAcceptHeader()))
                .HandleRequest(new RequestJsonBodyWithLogDecorator<CmiuPlacesSender>(_logger, places))
                .HandleRequest(new RequestLogDecorator(_logger))
                .SendAsync(cancellationToken);

            try
            {
                var httpResponse = new HttpResponse(httpResponseMessage);
                httpResponse
                    .HandleResponse(new ResponseLogDecorator(_logger))
                    .HandleResponse(new ResponseSuccessWithLogHandler<CmiuPlacesSenderResponse>(_logger, item =>
                    {
                        placesResponse = item;
                        _logger.LogDebug("Places: {0}", item);
                    }))
                    .HandleResponse(new ResponseErrorStringWithLogHandler(_logger, item =>
                    {
                        _logger.LogError("Error: {0}", item);
                    }));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
            }
            return placesResponse;
        }

        /// <summary>
        /// The PostLevelsAsync.
        /// </summary>
        /// <param name="levels">The places<see cref="CmiuLevelsSender"/>.</param>
        /// <param name="cancellationToken">The cancellationToken<see cref="CancellationToken"/>.</param>
        /// <returns>The <see cref="Task{CmiuLevelsSenderResponse}"/>.</returns>
        public async Task<CmiuLevelsSenderResponse> PostLevelsAsync(CmiuLevelsSender levels, CancellationToken cancellationToken)
        {
            return await PostLevelsAsync(levels, new CmiuLevelsUriExtension(_cmiuUriProvider).Uri, cancellationToken);
        }

        /// <summary>
        /// The PostLevelsAsync.
        /// </summary>
        /// <param name="levels">The places<see cref="CmiuLevelsSender"/>.</param>
        /// <param name="uri">The uri<see cref="Uri"/>.</param>
        /// <param name="cancellationToken">The cancellationToken<see cref="CancellationToken"/>.</param>
        /// <returns>The <see cref="Task{CmiuLevelsSenderResponse}"/>.</returns>
        public async Task<CmiuLevelsSenderResponse> PostLevelsAsync(CmiuLevelsSender levels, Uri uri, CancellationToken cancellationToken)
        {
            if (levels == null)
                throw new ArgumentNullException(nameof(levels));
            if (uri == null)
                throw new ArgumentNullException(nameof(uri));

            CmiuLevelsSenderResponse levelsResponse = null;

            var httpRequest = _httpRequestFactory.CreateRequest(new PostMethodProvider(), uri);
            var httpResponseMessage = await httpRequest
                .HandleRequest(new RequestHeadersAcceptDecorator<AppJsonAcceptHeader>(new AppJsonAcceptHeader()))
                .HandleRequest(new RequestJsonBodyWithLogDecorator<CmiuLevelsSender>(_logger, levels))
                .HandleRequest(new RequestLogDecorator(_logger))
                .SendAsync(cancellationToken);

            try
            {
                var httpResponse = new HttpResponse(httpResponseMessage);
                httpResponse
                    .HandleResponse(new ResponseLogDecorator(_logger))
                    .HandleResponse(new ResponseSuccessWithLogHandler<CmiuLevelsSenderResponse>(_logger, item =>
                    {
                        levelsResponse = item;
                        _logger.LogDebug("Levels: {0}", item);
                    }))
                    .HandleResponse(new ResponseErrorStringWithLogHandler(_logger, item =>
                    {
                        _logger.LogError("Error: {0}", item);
                    }));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
            }
            return levelsResponse;
        }

        /// <summary>
        /// The PostMovementAsync.
        /// </summary>
        /// <param name="movement">The movement<see cref="CmiuMovementSender"/>.</param>
        /// <param name="cancellationToken">The cancellationToken<see cref="CancellationToken"/>.</param>
        /// <returns>The <see cref="Task{CmiuMovementResponse}"/>.</returns>
        public async Task<CmiuMovementResponse> PostMovementAsync(CmiuMovementSender movement, CancellationToken cancellationToken)
        {
            if (movement == null)
                throw new ArgumentNullException(nameof(movement));

            CmiuMovementResponse movementResponse = null;

            var httpRequest = _httpRequestFactory.CreateRequest(new PostMethodProvider(), new CmiuMovementUriExtension(_cmiuUriProvider, movement.Type));
            var httpResponseMessage = await httpRequest
                .HandleRequest(new RequestHeadersAcceptDecorator<AppJsonAcceptHeader>(new AppJsonAcceptHeader()))
                .HandleRequest(new RequestJsonBodyWithLogDecorator<CmiuMovementSender>(_logger, movement))
                .HandleRequest(new RequestLogDecorator(_logger))
                .SendAsync(cancellationToken);

            try
            {
                var httpResponse = new HttpResponse(httpResponseMessage);
                httpResponse
                    .HandleResponse(new ResponseLogDecorator(_logger))
                    .HandleResponse(new ResponseSuccessWithLogHandler<CmiuMovementResponse>(_logger, item =>
                    {
                        movementResponse = item;
                        _logger.LogDebug("Movement: {0}", item);
                    }))
                    .HandleResponse(new ResponseErrorStringWithLogHandler(_logger, item =>
                    {
                        _logger.LogError("Error: {0}", item);
                    }));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
            }
            return movementResponse;
        }

        /// <summary>
        /// The PostPaymentAsync.
        /// </summary>
        /// <param name="payment">The payment<see cref="CmiuPaymentSender"/>.</param>
        /// <param name="cancellationToken">The cancellationToken<see cref="CancellationToken"/>.</param>
        /// <returns>The <see cref="Task{CmiuPaymentResponse}"/>.</returns>
        public async Task<CmiuPaymentResponse> PostPaymentAsync(CmiuPaymentSender payment, CancellationToken cancellationToken)
        {
            if (payment == null)
                throw new ArgumentNullException(nameof(payment));

            CmiuPaymentResponse paymentResponse = null;

            var httpRequest = _httpRequestFactory.CreateRequest(new PostMethodProvider(), new CmiuPaymentUriExtension(_cmiuUriProvider));
            var httpResponseMessage = await httpRequest
                .HandleRequest(new RequestHeadersAcceptDecorator<AppJsonAcceptHeader>(new AppJsonAcceptHeader()))
                .HandleRequest(new RequestJsonBodyWithLogDecorator<CmiuPaymentSender>(_logger, payment))
                .HandleRequest(new RequestLogDecorator(_logger))
                .SendAsync(cancellationToken);

            try
            {
                var httpResponse = new HttpResponse(httpResponseMessage);
                httpResponse
                    .HandleResponse(new ResponseLogDecorator(_logger))
                    .HandleResponse(new ResponseSuccessWithLogHandler<CmiuPaymentResponse>(_logger, item =>
                    {
                        paymentResponse = item;
                        _logger.LogDebug("Payment: {0}", item);
                    }))
                    .HandleResponse(new ResponseErrorStringWithLogHandler(_logger, item =>
                    {
                        _logger.LogError("Error: {0}", item);
                    }));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
            }
            return paymentResponse;
        }

        /// <summary>
        /// The PostCashSendAsync.
        /// </summary>
        /// <param name="cashStatus">The cashStatus<see cref="CmiuCashStatusSender"/>.</param>
        /// <param name="cancellationToken">The cancellationToken<see cref="CancellationToken"/>.</param>
        /// <returns>The <see cref="Task{CmiuCashStatusResponse}"/>.</returns>
        public async Task<CmiuCashStatusResponse> PostCashSendAsync(CmiuCashStatusSender cashStatus, CancellationToken cancellationToken)
        {
            if (cashStatus == null)
                throw new ArgumentNullException(nameof(cashStatus));

            CmiuCashStatusResponse cashStatusResponse = null;

            var httpRequest = _httpRequestFactory.CreateRequest(new PostMethodProvider(), new CmiuMoneyUriExtension(_cmiuUriProvider));
            var httpResponseMessage = await httpRequest
                .HandleRequest(new RequestHeadersAcceptDecorator<AppJsonAcceptHeader>(new AppJsonAcceptHeader()))
                .HandleRequest(new RequestJsonBodyWithLogDecorator<CmiuCashStatusSender>(_logger, cashStatus))
                .HandleRequest(new RequestLogDecorator(_logger))
                .SendAsync(cancellationToken);

            try
            {
                var httpResponse = new HttpResponse(httpResponseMessage);
                httpResponse
                    .HandleResponse(new ResponseLogDecorator(_logger))
                    .HandleResponse(new ResponseSuccessWithLogHandler<CmiuCashStatusResponse>(_logger, item =>
                    {
                        cashStatusResponse = item;
                        _logger.LogDebug("Payment: {0}", item);
                    }))
                    .HandleResponse(new ResponseErrorStringWithLogHandler(_logger, item =>
                    {
                        _logger.LogError("Error: {0}", item);
                    }));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
            }
            return cashStatusResponse;
        }

        /// <summary>
        /// The PostImageAsync.
        /// </summary>
        /// <param name="image">The image<see cref="ICollection{T}"/>.</param>
        /// <param name="cmiuHeaders">The cmiuHeaders<see cref="IDictionary{TKey, TValue}"/>.</param>
        /// <param name="cancellationToken">The cancellationToken<see cref="CancellationToken"/>.</param>
        /// <returns>The <see cref="Task{TResult}"/>.</returns>
        public async Task<bool> PostImageAsync(ICollection<byte> image, IDictionary<string, string> cmiuHeaders, CancellationToken cancellationToken)
        {
            if (image == null)
                throw new ArgumentNullException(nameof(image));

            bool response = false;

            Guid image_id = Guid.NewGuid();

            string formDataBoundary = string.Format(CultureInfo.CurrentCulture, "----------{0:N}", Guid.NewGuid());
            MultipartFormDataContent form = new(formDataBoundary);

            using var filenameContent = new StringContent($"{image_id}.jpg");
            form.Add(filenameContent, "filename");

            using var fileformatContent = new StringContent("jpeg");
            form.Add(fileformatContent, "fileformat");

            using var imageContent = new ByteArrayContent(image.ToArray());
            imageContent.Headers.ContentType = new MediaTypeHeaderValue("image/jpeg");
            form.Add(imageContent, "cmiu_image", $"{image_id}.jpg");

            var httpRequest = _httpRequestFactory.CreateRequest(new PostMethodProvider(), new CmiuUploadImageUriExtension(_cmiuUriProvider));
            var httpResponseMessage = await httpRequest
                .HandleRequest(new RequestHeadersDecorator(cmiuHeaders ?? new Dictionary<string, string>()))
                .HandleRequest(new RequestContentDecorator(form))
                .HandleRequest(new RequestLogDecorator(_logger))
                .SendAsync(cancellationToken);

            try
            {
                var httpResponse = new HttpResponse(httpResponseMessage);
                httpResponse
                    .HandleResponse(new ResponseLogDecorator(_logger))
                    .HandleResponse(new ResponseSuccessWithLogHandler(_logger, () =>
                    {
                        response = true;
                        _logger.LogDebug("PostImage: Success");
                    }))
                    .HandleResponse(new ResponseErrorStringWithLogHandler(_logger, item =>
                    {
                        _logger.LogError("Error: {0}", item);
                    }));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
            }
            return response;
        }

        /// <summary>
        /// PostCallEventAsync
        /// </summary>
        /// <param name="callEvent">The lpr<see cref="CallEventSender"/>.</param>
        /// <param name="cancellationToken">The cancellationToken<see cref="CancellationToken"/>.</param>
        /// <returns>The <see cref="Task{CmiuResponse}"/>.</returns>
        public async Task<CmiuResponse> PostCallEventAsync(CallEventSender callEvent, CancellationToken cancellationToken = default)
        {
            if (callEvent == null)
                throw new ArgumentNullException(nameof(callEvent));

            CmiuResponse callEventResponse = null;

            var httpRequest = _httpRequestFactory.CreateRequest(new PostMethodProvider(), new CmiuCallEventUriExtension(_cmiuUriProvider));
            var httpResponseMessage = await httpRequest
                .HandleRequest(new RequestHeadersAcceptDecorator<AppJsonAcceptHeader>(new AppJsonAcceptHeader()))
                .HandleRequest(new RequestJsonBodyWithLogDecorator<CallEventSender>(_logger, callEvent))
                .HandleRequest(new RequestLogDecorator(_logger))
                .SendAsync(cancellationToken);

            try
            {
                var httpResponse = new HttpResponse(httpResponseMessage);
                httpResponse
                    .HandleResponse(new ResponseLogDecorator(_logger))
                    .HandleResponse(new ResponseSuccessWithLogHandler<CmiuResponse>(_logger, item =>
                    {
                        callEventResponse = item;
                        _logger.LogDebug("Payment: {0}", item);
                    }))
                    .HandleResponse(new ResponseErrorStringWithLogHandler(_logger, item =>
                    {
                        _logger.LogError("Error: {0}", item);
                    }));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
            }
            return callEventResponse;
        }

        /// <summary>
        /// The PostLprAsync.
        /// </summary>
        /// <param name="lpr">The lpr<see cref="CmiuLprSender"/>.</param>
        /// <param name="cancellationToken">The cancellationToken<see cref="CancellationToken"/>.</param>
        /// <returns>The <see cref="Task{CmiuEventResponse}"/>.</returns>
        public async Task<CmiuEventResponse> PostLprAsync(CmiuLprSender lpr, CancellationToken cancellationToken = default)
        {
            if (lpr == null)
                throw new ArgumentNullException(nameof(lpr));

            CmiuEventResponse cmiuEventResponse = null;

            var httpRequest = _httpRequestFactory.CreateRequest(new PostMethodProvider(), new CmiuLprUriExtension(_cmiuUriProvider));
            var httpResponseMessage = await httpRequest
                .HandleRequest(new RequestHeadersAcceptDecorator<AppJsonAcceptHeader>(new AppJsonAcceptHeader()))
                .HandleRequest(new RequestJsonBodyWithLogDecorator<CmiuLprSender>(_logger, lpr))
                .HandleRequest(new RequestLogDecorator(_logger))
                .SendAsync(cancellationToken);

            try
            {
                var httpResponse = new HttpResponse(httpResponseMessage);
                httpResponse
                    .HandleResponse(new ResponseLogDecorator(_logger))
                    .HandleResponse(new ResponseSuccessWithLogHandler<CmiuEventResponse>(_logger, item =>
                    {
                        cmiuEventResponse = item;
                        _logger.LogDebug("LPR: {0}", item);
                    }))
                    .HandleResponse(new ResponseErrorStringWithLogHandler(_logger, item =>
                    {
                        _logger.LogError("Error: {0}", item);
                    }));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
            }
            return cmiuEventResponse;
        }

        /// <summary>
        /// Информация о доставке сессий в АИС ЕПП
        /// </summary>
        /// <param name="aisSessionDeliveryStatus">The cashStatus<see cref="CmiuCashStatusSender"/>.</param>
        /// <param name="cancellationToken">The cancellationToken<see cref="CancellationToken"/>.</param>
        /// <returns>The <see cref="Task{CmiuAisSessionDeliveryStatusResponse}"/>.</returns>
        public async Task<CmiuAisSessionDeliveryStatusResponse> PostEppSessionDeliveryAsync(CmiuAisSessionDeliveryStatusSender aisSessionDeliveryStatus, CancellationToken cancellationToken = default)
        {
            if (aisSessionDeliveryStatus == null)
                throw new ArgumentNullException(nameof(aisSessionDeliveryStatus));

            CmiuAisSessionDeliveryStatusResponse deliveryStatusResponse = null;

            var httpRequest = _httpRequestFactory.CreateRequest(new PostMethodProvider(), new CmiuLprUriExtension(_cmiuUriProvider));
            var httpResponseMessage = await httpRequest
                .HandleRequest(new RequestHeadersAcceptDecorator<AppJsonAcceptHeader>(new AppJsonAcceptHeader()))
                .HandleRequest(new RequestJsonBodyWithLogDecorator<CmiuAisSessionDeliveryStatusSender>(_logger, aisSessionDeliveryStatus))
                .HandleRequest(new RequestLogDecorator(_logger))
                .SendAsync(cancellationToken);

            try
            {
                var httpResponse = new HttpResponse(httpResponseMessage);
                httpResponse
                    .HandleResponse(new ResponseLogDecorator(_logger))
                    .HandleResponse(new ResponseSuccessWithLogHandler<CmiuAisSessionDeliveryStatusResponse>(_logger, item =>
                    {
                        deliveryStatusResponse = item;
                        _logger.LogDebug("Delivery Status: {0}", item);
                    }))
                    .HandleResponse(new ResponseErrorStringWithLogHandler(_logger, item =>
                    {
                        _logger.LogError("Error: {0}", item);
                    }));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
            }
            return deliveryStatusResponse;     
        }
    }
}
