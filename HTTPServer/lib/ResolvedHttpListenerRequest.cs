using System.Net;
using System.IO;
using System.Xml.Linq;

namespace HTTPServer.lib
{
    public class ResolvedHttpListenerRequest
    {
        public HttpListenerRequest Request { get; protected set; }
        public string ContentType { get; protected set; }
        public string Content { get; protected set; }
        
        public object ContentObj { get; protected set; }

        public ResolvedHttpListenerRequest(HttpListenerRequest req)
        {
            Request = req;
            ContentType = req.ContentType;
            if (req.HasEntityBody)
            {
                Stream body = req.InputStream;
                StreamReader reader = new StreamReader(body, req.ContentEncoding);
                Content = reader.ReadToEnd();
            }
            else
            {
                Content = "N/A";
            }

            switch (ContentType)
            {
                case "application/json":
                    ContentObj = Newtonsoft.Json.JsonConvert.DeserializeObject(Content);
                    Content = Utils.StringUtils.PrettyJson(ContentObj);
                    break;
                case "text/xml":
                case "application/xml":
                    ContentObj = XDocument.Parse(Content);
                    Content = Utils.StringUtils.PrettyXml((XDocument)ContentObj);
                    break;
            }
        }
    }
}
