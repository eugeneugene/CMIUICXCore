using CMIUICXCore.ApiEndpoints;
using CMIUICXCore.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace CMIUICXCore.Controllers
{
    /// <summary>
    /// Defines the <see cref="GetRecordingEndpoint" />.
    /// </summary>
    [ApiController]
    public class GetRecordingEndpoint : BaseAsyncEndpoint.WithRequest<long>.WithoutResponse
    {
        private readonly ILogger _logger;
        private readonly DataContext _dataContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetRecordingEndpoint"/> class.
        /// </summary>
        /// <param name="logger">The logger<see cref="ILogger{Mp3Controller}"/>.</param>
        /// <param name="dataContext">The dataContext<see cref="DataContext"/>.</param>
        public GetRecordingEndpoint(ILogger<GetRecordingEndpoint> logger,  DataContext dataContext)
        {
            _logger = logger;
            _dataContext = dataContext;
            _logger.LogTrace("Создание экземпляра {0}", GetType().Name);
        }

        /// <summary>
        /// Handle request
        /// </summary>
        /// <param name="request">The request<see cref="long"/>.</param>
        /// <param name="cancellationToken">The cancellationToken<see cref="CancellationToken"/>.</param>
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        [Route("getfile/{request:long}")]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Produces("audio/mpeg")]
        public override async Task<ActionResult> HandleAsync(long request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogDebug("Id: {0}", request);

                var item = await _dataContext.AudioRecordingDataSet.SingleOrDefaultAsync(item => item.Id == request, cancellationToken);
                if (item == null)
                    return new NotFoundResult();

                var file = item.Mp3Path;

                if (!System.IO.File.Exists(file))
                    return new NotFoundResult();

                FileStream fs = new(file, FileMode.Open);
                string fileType = "audio/mpeg";
                string fileName = Path.GetFileName(item.Mp3Path);
                return File(fs, fileType, fileName);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
