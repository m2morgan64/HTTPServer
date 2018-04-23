using System;
using System.Linq;
using HTTPServer.lib;
using System.Net;
using System.Net.Http;
using System.Reflection;
using VolumeController;
using Newtonsoft.Json;
using static VolumeController.VolumeManager;

namespace HTTPServer.api.VolumeController
{
    public static class VolumeController
    {
        #region Locals
        private static readonly VolumeManager VolMgr = new VolumeManager();
        #endregion

        #region Handler
        // ReSharper disable once UnusedMember.Global
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
                return new ResponseInfo(HttpStatusCode.OK, lib.Utils.PageBuilder.MethodListInApi(typeof(VolumeController)));
            }

            string nameSpaceName = $"{typeof(VolumeController).Namespace}.{pathElement}";

            Type nameSpace = (from type in Assembly.GetExecutingAssembly().GetTypes()
                                     where type.Namespace == nameSpaceName
                                     select type).FirstOrDefault();

            if (nameSpace != null)
            {
                MethodInfo subElementHandler = nameSpace.GetMethod(Steward.HANDLERNAME);
                if (subElementHandler != null)
                {
                    result = (ResponseInfo)subElementHandler.Invoke(typeof(VolumeController), new object[] {request});
                }
            }
            else
            {
                // See if we have a method to call
                MethodInfo method = typeof(VolumeController).GetMethod(pathElement);
                if (method != null)
                {
                    result = (ResponseInfo)method.Invoke(typeof(VolumeController), new object[] { request });
                }
            }

            return result;
        }
        #endregion

        #region Public API Methods
        [Steward.ApiMethodAttributes(GetSupported = true, PutSupported = false, PostSupported = true, DeleteSupported = false, Description = "The current sound volume of the server")]
        // ReSharper disable once UnusedMember.Global
        public static ResponseInfo Volume(ResolvedHttpListenerRequest request)
        {
            ResponseInfo result;
            HttpMethod method = new HttpMethod(request.Request.HttpMethod);

            if (method == HttpMethod.Get)
            {
                result = Volume_Get();
            }
            else if (method == HttpMethod.Post)
            {
                result = Volume_Post(request);
            }
            else
            {
                result = new ResponseInfo(HttpStatusCode.MethodNotAllowed, "Invalid Method");
            }

            return result;
        }

        private static ResponseInfo Volume_Get()
        {
            return new ResponseInfo(HttpStatusCode.OK, JsonConvert.SerializeObject(VolMgr.GetVolumeSettings()));
        }

        private static ResponseInfo Volume_Post(ResolvedHttpListenerRequest request)
        {
            VolumeSettings newSettings;

            switch (request.ContentType)
            {
                case "application/json":
                    newSettings = JsonConvert.DeserializeObject<VolumeSettings>(request.Content);
                    break;
                default:
                    return new ResponseInfo(HttpStatusCode.InternalServerError,
                                            "Unable to parse request body; expecting json object.");
            }

            VolMgr.SetVolume(newSettings);

            return new ResponseInfo(HttpStatusCode.OK, JsonConvert.SerializeObject(VolMgr.GetVolumeSettings()));
        }
        #endregion

        #region Pages
        [Steward.WebPagesAttributes()]
        // ReSharper disable once InconsistentNaming
        // ReSharper disable once UnusedMember.Global
        public static ResponseInfo UI(ResolvedHttpListenerRequest request)
        {
            return new ResponseInfo(HttpStatusCode.OK, Properties.Resources.VolumeControllerHTML);
        }
        #endregion

        #region Helpers
        #endregion
    }
}
