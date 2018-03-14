using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using System.Xml;
using System.Xml.Linq;

namespace HTTPServer.lib
{
    public class ResolvedHttpListenerRequest
    {
        public HttpListenerRequest Request { get; protected set; }
        public string ContentType { get; protected set; }
        public string Content { get; protected set; }
        
        public object ContentObj { get; protected set; }

        public ResolvedHttpListenerRequest(HttpListenerRequest req) : base()
        {
            this.Request = req;
            this.ContentType = req.ContentType;
            if (req.HasEntityBody)
            {
                Stream body = req.InputStream;
                StreamReader reader = new System.IO.StreamReader(body, req.ContentEncoding);
                this.Content = reader.ReadToEnd();
            }
            else
            {
                this.Content = "N/A";
            }

            switch (this.ContentType)
            {
                case "application/json":
                    this.ContentObj = Newtonsoft.Json.JsonConvert.DeserializeObject(this.Content);
                    this.Content = Utils.StringUtils.PrettyJson(this.ContentObj);
                    break;
                case "text/xml":
                case "application/xml":
                    this.ContentObj = XDocument.Parse(this.Content);
                    this.Content = Utils.StringUtils.PrettyXml((XDocument)this.ContentObj);
                    break;
                default:
                    break;
            }
        }
    }
}
