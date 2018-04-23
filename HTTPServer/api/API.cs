using System;
using System.Linq;
using HTTPServer.lib;
using System.Net;
using System.Reflection;

namespace HTTPServer.api
{
    public static class Api
    {
        #region Handler
        // ReSharper disable once UnusedMember.Global
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
                return new ResponseInfo(HttpStatusCode.OK, lib.Utils.PageBuilder.MethodListInApi(typeof(Api)));
            }

            string nameSpaceName = $"{typeof(Api).Namespace}.{pathElement}";

            Type nameSpace = (from type in Assembly.GetExecutingAssembly().GetTypes()
                                     where type.Namespace == nameSpaceName
                                     select type).FirstOrDefault();

            if (nameSpace != null)
            {
                MethodInfo subElementHandler = nameSpace.GetMethod(Steward.HANDLERNAME);
                if (subElementHandler != null)
                {
                    result = (ResponseInfo)subElementHandler.Invoke(typeof(Api), new object[] {request});
                }
            }
            else
            {
                // See if we have a method to call
                MethodInfo method = typeof(Api).GetMethod(pathElement);
                if (method != null)
                {
                    result = (ResponseInfo)method.Invoke(typeof(Api), new object[] { request });
                }
            }

            return result;
        }
        #endregion

        #region Public API Methods
        #endregion

        #region Helpers
        #endregion
    }
}
