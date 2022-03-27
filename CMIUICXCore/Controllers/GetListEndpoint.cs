using CMIUICXCore.ApiEndpoints;
using CMIUICXCore.Data;
using CMIUICXCore.Types;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CMIUICXCore.Controllers
{
    /// <summary>
    /// Defines the <see cref="GetListEndpoint" />.
    /// </summary>
    public class GetListEndpoint : BaseEndpoint.WithRequest<CmiuCommandBase>.WithoutResponse
    {
        private readonly ILogger _logger;
        private readonly DataContext _dataContext;
        private readonly IConfiguration _configuration;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetListEndpoint"/> class.
        /// </summary>
        /// <param name="logger">The logger<see cref="ILogger{GetListEndpoint}"/>.</param>
        /// <param name="dataContext">The DataContext<see cref="DataContext"/>.</param>
        /// <param name="configuration">Configuration</param>
        public GetListEndpoint(ILogger<GetListEndpoint> logger, DataContext dataContext, IConfiguration configuration)
        {
            _logger = logger;
            _dataContext = dataContext;
            _configuration = configuration;
            _logger.LogTrace("Создание экземпляра {0}", GetType().Name);
        }

        /// <summary>
        /// Handle request
        /// </summary>
        /// <param name="request">The request<see cref="CmiuCommandBase"/>.</param>
        /// <returns>The <see cref="Task{ActionResult}"/>.</returns>
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        [Route("getlist")]
        [HttpPost]
        [ProducesResponseType(typeof(Mp3ListCommandResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(Mp3ListErrorResponse), StatusCodes.Status500InternalServerError)]
        [Consumes("application/json")]
        [Produces("application/json")]
        public override ActionResult Handle([FromBody] CmiuCommandBase request)
        {
            try
            {
                if (request == null)
                    throw new ArgumentNullException(nameof(request));

                _logger.LogDebug("CmiuCommandBase: {0}", request);

                if (request is Mp3ListCommand mp3ListCommand)
                {
                    if (mp3ListCommand.DateTimeFrom.HasValue && mp3ListCommand.DateTimeTill.HasValue &&
                        mp3ListCommand.DateTimeFrom > mp3ListCommand.DateTimeTill)
                    {
                        var result = new ObjectResult(new Mp3ListErrorResponse("mp3_file_list", DateTime.Now, "'date_from' must be earlier than 'date_till'", 1))
                        {
                            StatusCode = StatusCodes.Status500InternalServerError
                        };
                        return new ObjectResult(result);
                    }

                    var recordings = _dataContext.AudioRecordingDataSet
                        .Where(item => (!mp3ListCommand.DateTimeFrom.HasValue || item.StartTime >= mp3ListCommand.DateTimeFrom.Value.ToUniversalTime().Ticks)
                        && (!mp3ListCommand.DateTimeTill.HasValue || item.EndTime <= mp3ListCommand.DateTimeTill.Value.ToUniversalTime().Ticks)
                        && item.RecordingType == RecordingType.Audio && item.Status == RecordingStatus.Available).ToArray();
                    _logger.LogTrace("Recordings: {0}", recordings);
                    var mp3list = recordings.Select(item => GetMp3ListItem(item, _configuration));
                    var response = new Mp3ListCommandResponse("mp3_file_list", DateTime.Now, mp3list, 0);
                    _logger.LogTrace("Response: {0}", response);

                    return new JsonResult(response);
                }
                else
                {
                    _logger.LogTrace("Response: Not found");
                    return new NotFoundResult();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        // Static !!!
        // Error: System.InvalidOperationException: The client projection contains a reference to a constant expression of 'CMIUICXCore.Controllers.GetListEndpoint' through the instance method 'GetMp3ListItem'. This could potentially cause a memory leak; consider making the method static so that it does not capture constant in the instance. See https://go.microsoft.com/fwlink/?linkid=2103067 for more information.
        private static Mp3ListItem GetMp3ListItem(AudioRecordingData data, IConfiguration _configuration)
        {
            string Id = data.Id.ToString();
            string RelativePath = Path.GetRelativePath(_configuration.GetValue("CMIUICXCore:RecordingsRoot", string.Empty), data.Mp3Path);
            string FileName = Path.GetFileName(data.Mp3Path);
            DateTime CreationDateTime = data.EndDateLocalTime;
            long FileSize = 0L;
            string[] parts = Path.GetFileNameWithoutExtension(data.Mp3Path).Split('-');
            string Time = parts.Length >= 3 ? parts[2] : string.Empty;
            string SipGate = _configuration.GetValue("CMIUICXCore:SipGate", string.Empty);
            string[] numbers = data.UserNames.Split("->")
                .Select(item1 => item1?.Trim() ?? string.Empty)
                //  .Select(item2 => item2.TrimEnd('F', 'f'))
                .Select(item3 => item3.Equals(SipGate, StringComparison.InvariantCultureIgnoreCase) ? data.SipNumber ?? string.Empty : item3)
                .ToArray();
            string Number1 = numbers.Length >= 1 ? numbers[0].TrimEnd('F', 'f') : string.Empty;
            string Number2 = numbers.Length >= 2 ? numbers[1].TrimEnd('F', 'f') : string.Empty;
            return new Mp3ListItem(Id, RelativePath, FileName, CreationDateTime, FileSize, Time, Number1, Number2, data.Duration);
        }
    }
}
