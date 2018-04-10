using System;
using System.Linq;
using HTTPServer.lib;
using System.Net;
using System.Reflection;

namespace HTTPServer.api.VolumeController
{
    public static class VolumeController
    {
        #region Handler
        public static ResponseInfo Handle(ResolvedHttpListenerRequest request)
        {
            ResponseInfo result = new ResponseInfo(HttpStatusCode.NotFound, Steward.StandardErrorResponseContent);
            string pathElement = string.Empty;

            for (int i = 0; i < request.Request.Url.Segments.Length; i++)
            {
                string thisSegment = request.Request.Url.Segments[i].Replace("/", string.Empty);
                if (thisSegment == "VolumeController")
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
                return new ResponseInfo(HttpStatusCode.OK, lib.Utils.PageBuilder.MethodListInAPI(typeof(VolumeController)));
            }

            string nameSpaceName = string.Format("{0}.{1}", typeof(VolumeController).Namespace, pathElement);

            System.Type nameSpace = (from type in Assembly.GetExecutingAssembly().GetTypes()
                                     where type.Namespace == nameSpaceName
                                     select type).FirstOrDefault<Type>();

            if (nameSpace != null)
            {
                MethodInfo subElementHandler = nameSpace.GetMethod(Steward.HANDLERNAME);
                result = (ResponseInfo)subElementHandler.Invoke(typeof(VolumeController), new object[] { request });
            }
            else
            {
                // See if we have a method to call
                MethodInfo method = typeof(VolumeController).GetMethod(pathElement);
                result = (ResponseInfo)method.Invoke(typeof(VolumeController), new object[] { request });
            }

            return result;
        }
        #endregion

        #region Public API Methods
        [Steward.APIMethodAttributes(GetSupported = true, PutSupported = false, PostSupported = false, DeleteSupported = false, Description = "The current sound volume of the server")]
        public static ResponseInfo CurrentVolume(ResolvedHttpListenerRequest request)
        {
            ResponseInfo result = new ResponseInfo(HttpStatusCode.OK, "42");

            return result;
        }
        #endregion
        
        #region Pages
        [Steward.WebPagesAttributes()]
        public static ResponseInfo UI(ResolvedHttpListenerRequest request)
        {
            return new ResponseInfo(HttpStatusCode.OK, HTTPServer.Properties.Resources.VolumeControllerHTML);
        }

        #endregion

        #region Helpers
        #endregion
    }
}
