using System;
using System.Linq;
using HTTPServer.lib;
using System.Net;
using System.Reflection;

namespace HTTPServer.api
{
    public static class API
    {
        #region Handler
        public static ResponseInfo Handle(ResolvedHttpListenerRequest request)
        {
            ResponseInfo result = new ResponseInfo(HttpStatusCode.NotFound, Steward.StandardErrorResponseContent);
            string pathElement = string.Empty;

            for (int i = 0; i < request.Request.Url.Segments.Length; i++)
            {
                string thisSegment = request.Request.Url.Segments[i].Replace("/", string.Empty);
                if (thisSegment == "api")
                {
                    if (i + 1 < request.Request.Url.Segments.Length)
                    {
                        pathElement = request.Request.Url.Segments[i + 1].Replace("/", string.Empty);
                    }
                    break;
                }
            }

            if (pathElement == string.Empty)
            {
                return new ResponseInfo(HttpStatusCode.OK, lib.Utils.PageBuilder.MethodListInAPI(typeof(API)));
            }

            string nameSpaceName = string.Format("{0}.{1}", typeof(API).Namespace, pathElement);

            System.Type nameSpace = (from type in Assembly.GetExecutingAssembly().GetTypes()
                                     where type.Namespace == nameSpaceName
                                     select type).FirstOrDefault<Type>();

            if (nameSpace != null)
            {
                MethodInfo subElementHandler = nameSpace.GetMethod(Steward.HANDLERNAME);
                result = (ResponseInfo)subElementHandler.Invoke(typeof(API), new object[] { request });
            }
            else
            {
                // See if we have a method to call
                MethodInfo method = typeof(API).GetMethod(pathElement);
                result = (ResponseInfo)method.Invoke(typeof(API), new object[] { request });
            }

            return result;
        }
        #endregion

        #region Public API Methods
        [Steward.APIMethodAttributes(GetSupported = true, PutSupported = false, PostSupported = false, DeleteSupported = true)]
        public static ResponseInfo Test(ResolvedHttpListenerRequest request)
        {
            ResponseInfo result = new ResponseInfo(HttpStatusCode.OK, "42");

            return result;
        }

        [Steward.APIMethodAttributes(GetSupported = true, PutSupported = false, PostSupported = true, DeleteSupported = false)]
        public static ResponseInfo Test2(ResolvedHttpListenerRequest request)
        {
            ResponseInfo result = new ResponseInfo(HttpStatusCode.OK, "64");

            return result;
        }
        #endregion

        #region Helpers
        #endregion
    }
}
