using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Text;

namespace CMIUICXCore.Code
{
    public static class HeaderUtilities
    {
        public static string DumpHeaders(params HttpHeaders[] headers)
        {
            // Appends all headers as string similar to:
            // {
            //    HeaderName1: Value1
            //    HeaderName1: Value2
            //    HeaderName2: Value1
            //    ...
            // }

            if (headers == null)
                throw new ArgumentNullException(nameof(headers));

            StringBuilder sb = new();
            sb.AppendLine("{");

            for (int i = 0; i < headers.Length; i++)
            {
                if (headers[i] is HttpHeaders hh)
                {
                    foreach (KeyValuePair<string, IEnumerable<string>> header in hh)
                    {
                        foreach (string headerValue in header.Value)
                        {
                            sb.Append("  ");
                            sb.Append(header.Key);
                            sb.Append(": ");
                            sb.AppendLine(headerValue);
                        }
                    }
                }
            }

            sb.Append('}');
            return sb.ToString();
        }
    }
}
