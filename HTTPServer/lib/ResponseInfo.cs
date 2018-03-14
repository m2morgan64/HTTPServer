using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            this.StatusCode = statusCode;
            this.ReasonPhrase = reason;
            this.Content = content;
            this.Content = this.Content.Replace("%CODE%", ((int)this.StatusCode).ToString()).Replace("%REASON%", this.ReasonPhrase);
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
            return new ResponseInfo(this.StatusCode, this.ReasonPhrase, this.Content);
        }
    }
}
