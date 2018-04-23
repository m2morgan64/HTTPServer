using System;
using System.Net;

namespace HTTPServer.lib
{
    public class ResponseInfo : ICloneable
    {
        public HttpStatusCode StatusCode { get; set; }
        public string ReasonPhrase { get; set; }
        public string Content { get; set; }

        private void _ResponseInfo(HttpStatusCode statusCode, string reason, string content)
        {
            StatusCode = statusCode;
            ReasonPhrase = reason;
            Content = content;
            Content = Content.Replace("%CODE%", ((int)StatusCode).ToString()).Replace("%REASON%", ReasonPhrase);
        }

        public ResponseInfo(HttpStatusCode statusCode, string reason, string content)
        {
            _ResponseInfo(statusCode, reason, content);
        }

        public ResponseInfo(HttpStatusCode statusCode, string content)
        {
            string reason = Utils.StringUtils.AddSpacesToSentence(statusCode.ToString());
            _ResponseInfo(statusCode, reason, content);
        }

        public virtual object Clone()
        {
            return new ResponseInfo(StatusCode, ReasonPhrase, Content);
        }
    }
}
