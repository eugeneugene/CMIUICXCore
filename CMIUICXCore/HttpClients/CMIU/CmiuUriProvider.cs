using Microsoft.Extensions.Configuration;
using System;

namespace SBCommon.HttpClients.New
{
    public class CmiuUriProvider : IUriProvider
    {
        private readonly IConfiguration _configuration;
    
        public Uri Uri => new(_configuration[CmiuHttpClientProvider.HttpClientName + ":CmiuServerUrl"]);

        public CmiuUriProvider(IConfiguration configuration)
        {
            _configuration = configuration;
        }
    }
}
