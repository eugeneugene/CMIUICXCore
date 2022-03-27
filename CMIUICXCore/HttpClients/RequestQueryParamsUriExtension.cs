using Microsoft.AspNetCore.WebUtilities;
using System;
using System.Collections.Generic;

namespace SBCommon.HttpClients.New
{
    public class RequestQueryParamsUriExtension : IUriExtension
    {
        public IDictionary<string, string> Params { get; }

        public Uri Uri { get; }

        public RequestQueryParamsUriExtension(IUriExtension uri, IDictionary<string, string> @params)
        {
            if (uri == null)
                throw new ArgumentNullException(nameof(uri));

            Params = @params ?? throw new ArgumentNullException(nameof(@params));
            Uri = new Uri(QueryHelpers.AddQueryString(uri.ToString(), Params));
        }
    }
}
