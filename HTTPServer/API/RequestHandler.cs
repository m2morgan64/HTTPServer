using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HTTPServer.lib;

namespace HTTPServer.API
{
    public static class RequestHandler
    {
        public static ResponseInfo Handle(ResolvedHttpListenerRequest req)
        {
            ResponseInfo result = new ResponseInfo(System.Net.HttpStatusCode.NotFound, Steward.StandardErrorResponseContent);

            return result;
        }
    }
}
