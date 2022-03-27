using System;
using System.Text;

namespace CMIUICXCore.MiddleWare
{
    public class RequestResponseLog
    {
        public string Path { get; set; }
        public string QueryString { get; set; }
        public string Method { get; set; }
        public string Payload { get; set; }
        public string Response { get; set; }
        public int StatusCode { get; set; }
        public DateTime RequestedOn { get; set; }
        public DateTime RespondedOn { get; set; }
        public bool IsSuccessStatusCode { get; set; }

        public override string ToString()
        {
            StringBuilder sb = new();

            sb.AppendFormat("Request on: '{0}'", RequestedOn).Append(Environment.NewLine);
            sb.AppendFormat(" Method: '{0}'", Method).Append(Environment.NewLine);
            sb.AppendFormat(" Path: '{0}'", Path).Append(Environment.NewLine);
            sb.AppendFormat(" QueryString: '{0}'", QueryString).Append(Environment.NewLine);
            if (Payload != null)
                sb.AppendFormat(" Body: " + Environment.NewLine + "'{0}'", Payload).Append(Environment.NewLine);
            sb.AppendFormat("Response on: '{0}'", RespondedOn).Append(Environment.NewLine);
            sb.AppendFormat(" Success: '{0}'", IsSuccessStatusCode ? "Yes" : "No").Append(Environment.NewLine);
            sb.AppendFormat(" StatusCode: '{0}'", StatusCode).Append(Environment.NewLine);
            sb.AppendFormat(" Response: " + Environment.NewLine + "'{0}'", Response).Append(Environment.NewLine);
            return sb.ToString();
        }
    }
}
