using System;
using System.IO;
using System.Net;
using NLog;
using HTTPServer.lib;
using System.Threading;
using static HTTPServer.lib.Utils.EventHelpers;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;

namespace HTTPServer
{
    // https://msdn.microsoft.com/en-us/library/system.net.httplistener(v=vs.110).aspx
    // https://stackoverflow.com/questions/9329707/httplistener-for-simple-http-server-in-production
    public class Steward
    {
        #region public Enums/Constants
        public enum RunStatus { Stopped, Starting, Running, Stopping };
        public const string HANDLERNAME = "Handle";
        public const string LogTarget = "logfile";
        #endregion

        #region Classes
        public class ApiMethodAttributes : Attribute
        {
            public bool GetSupported { get; set; }
            public bool PostSupported { get; set; }
            public bool PutSupported { get; set; }
            public bool DeleteSupported { get; set; }
            public string Description { get; set; }
        }
        public class WebPagesAttributes : Attribute
        {
            [SuppressMessage("ReSharper", "UnusedMember.Global")]
            public bool IsWebPage => true;
        }
        #endregion

        #region Events
        public event EventHandler<EventArgs> OnStatusChange;
        #endregion

        #region Properties
        public HttpPrefixes<String> Prefixes { get; protected set; }

        private RunStatus _status = RunStatus.Stopped;
        public RunStatus Status
        {
            get => _status;
            set
            {
                if (_status != value)
                {
                    _status = value;
                    RaiseSimpleEvent(this, OnStatusChange);
                }
            }
        }

        public static string StandardErrorResponseContent => Properties.Resources.ErrorResponseContentTemplate;

        public NLog.Config.LoggingConfiguration LogConfig { get; protected set; }
        #endregion

        #region Locals
        private HttpListener _listener = new HttpListener();
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();
        #endregion

        #region Constructors
        public Steward()
        {
            LogConfig = new NLog.Config.LoggingConfiguration();
            NLog.Targets.FileTarget logfile = new NLog.Targets.FileTarget() { FileName = Properties.Settings.Default.LogFile, Name = LogTarget , CreateDirs = true };
            NLog.Targets.ConsoleTarget logconsole = new NLog.Targets.ConsoleTarget() { Name = "logconsole" };

            LogConfig.LoggingRules.Add(new NLog.Config.LoggingRule("*", LogLevel.Info, logfile));
            LogConfig.LoggingRules.Add(new NLog.Config.LoggingRule("*", LogLevel.Debug, logconsole));

            LogManager.Configuration = LogConfig;

            Prefixes = new HttpPrefixes<string>();
            Prefixes.OnAdd += Prefixes_OnAdd;
            Prefixes.OnAddRange += Prefixes_OnAddRange;
            Prefixes.OnRemove += Prefixes_OnRemove;
            Prefixes.OnRemoveRange += Prefixes_OnRemoveRange;
        }

        [SuppressMessage("ReSharper", "UnusedMember.Global")]
        public Steward(string httpPrefix) : this()
        {
            Prefixes.Add(httpPrefix);
        }

        [SuppressMessage("ReSharper", "UnusedMember.Global")]
        public Steward(List<string> httpPrefixes) : this()
        {
            Prefixes.AddRange(httpPrefixes);
        }
        #endregion

        #region Prefixes Event Handlers
        private void Prefixes_OnAdd(object sender, SingleStringEventArgs e)
        {
            // No need to update the listener list unless the listener is running
            if (Status == RunStatus.Running)
            {
                try
                {
                    _listener.Prefixes.Add(e.Value);
                }
                catch
                {
                    Prefixes.Remove(e.Value);
                }
            }
        }

        private void Prefixes_OnAddRange(object sender, MultiStringEventArgs e)
        {
            foreach (string s in e.Values)
            {
                Prefixes_OnAdd(sender, new SingleStringEventArgs(s));
            }
        }

        private void Prefixes_OnRemove(object sender, SingleStringEventArgs e)
        {
            // No need to update the listener list unless the listener is running
            if (Status == RunStatus.Running)
            {
                // If this is the last listener in the list, we need to stop the server.
                if (_listener.Prefixes.Count == 1
                    && _listener.Prefixes.Contains(e.Value))
                {
                    Stop();
                }
                _listener.Prefixes.Remove(e.Value);
            }
        }
        
        private void Prefixes_OnRemoveRange(object sender, MultiStringEventArgs e)
        {
            foreach (string s in e.Values)
            {
                Prefixes_OnRemove(sender, new SingleStringEventArgs(s));
            }
        }
        #endregion

        #region Start/Stop/Run
        public void Start()
        {
            Status = RunStatus.Starting;

            if (Prefixes.Count == 0)
            {
                throw new Exception("At least one http prefix must be specified to start the server.\r\n for example http://+:6464/api/v1/");
            }

            _listener = new HttpListener();
            // Add the prefixes.
            foreach (string s in Prefixes)
            {
                _listener.Prefixes.Add(s);
            }

            Thread thread = new Thread(Run);
            thread.Start();
        }

        public void Run()
        { 
            _listener.Start();
            Status = RunStatus.Running;
            
            while (_listener.IsListening)
            {
                IAsyncResult result = _listener.BeginGetContext(Listen, _listener);
                result.AsyncWaitHandle.WaitOne();
            }
            _listener.Close();
            Status = RunStatus.Stopped;
        }
        
        private void Listen(IAsyncResult result)
        {
            HttpListener listener = (HttpListener)result.AsyncState;
            if (listener.IsListening)
            {
                HttpListenerContext context = listener.EndGetContext(result);

                ResolvedHttpListenerRequest req = new ResolvedHttpListenerRequest(context.Request);
                LogIncomingRequest(req.Request, req.Content);

                // Obtain a response object.
                ResponseInfo responseContent = Handle(req);

                // Respond
                HttpListenerResponse response = context.Response;
                response.StatusCode = (int)responseContent.StatusCode;
                response.StatusDescription = responseContent.ReasonPhrase;
                byte[] buffer = context.Request.ContentEncoding.GetBytes(responseContent.Content);
                response.ContentLength64 = buffer.Length;
                Stream output = response.OutputStream;
                output.Write(buffer, 0, buffer.Length);
                output.Close();
            }
        }

        private void LogIncomingRequest(HttpListenerRequest req, string content = "")
        {
            _logger.Debug("######################################################################");
            _logger.Debug("##########                                                  ##########");
            _logger.Debug("##########                 START OF REQUEST                 ##########");
            _logger.Debug("##########                                                  ##########");
            _logger.Debug("######################################################################");
            _logger.Info($"Received Message: {req.HttpMethod} {req.Url} {req.ProtocolVersion}");
            if (!_logger.IsDebugEnabled)
            {
                return;
            }

            _logger.Debug($"From: {req.UserAgent}");
            _logger.Debug($"User-Agent: {req.UserAgent}");
            _logger.Debug("______________________________________________________________________");
            _logger.Debug($"\t > Encoding: {req.ContentEncoding}");
            _logger.Debug($"\t > Content Type: {req.ContentType}");
            _logger.Debug($"\t > Server's Endpoint: {req.LocalEndPoint}");
            _logger.Debug($"\t > Client's Endpoint: {req.RemoteEndPoint}");
            _logger.Debug($"\t > Service Provider Name: {req.ServiceName}");
            _logger.Debug($"\t > User Host: {req.UserHostAddress} {((req.UserHostName == string.Empty) ? string.Empty :$" ({req.UserHostName})")}");
            _logger.Debug($"\t > User Language(s): { ((req.UserLanguages == null) ? "<NONE>" : String.Join(", ", req.UserLanguages)) }");
            _logger.Debug($"\t > Local Request: {req.IsLocal}");
            _logger.Debug($"\t > Web Socket Request: {req.IsWebSocketRequest}");
            _logger.Debug($"\t > Keep Alive Request: {req.KeepAlive}");
            _logger.Debug($"\t > Trace Identifier: {req.RequestTraceIdentifier}");
            _logger.Debug("\t > URLs:");
            _logger.Debug($"\t\t - Raw URL: {req.RawUrl}");
            _logger.Debug($"\t\t - URL: {req.Url}");
            _logger.Debug($"\t\t - URL Referrer: {req.UrlReferrer}");
            _logger.Debug("\t > Security: ");
            _logger.Debug($"\t\t - Is Authenticated: {req.IsAuthenticated}");
            _logger.Debug($"\t\t - Is Secured (SSL): {req.IsSecureConnection}");
            _logger.Debug($"\t\t - Has Certificate: {req.GetClientCertificate() == null}");
            _logger.Debug($"\t\t - Certificate Error Code: {((!req.IsAuthenticated || req.GetClientCertificate() == null) ? "N/A" : req.ClientCertificateError.ToString()) }");

            _logger.Debug("\t > Cookies:");
            if (req.Cookies.Count == 0)
            {
                _logger.Debug("\t\t - <NONE>");
            }
            foreach (Cookie c in req.Cookies)
            {
                _logger.Debug($"\t\t - {c.Name}");
                _logger.Debug($"\t\t\t * Value: {c.Value}");
                _logger.Debug($"\t\t\t * Domain: {c.Domain}");
                _logger.Debug($"\t\t\t * Path: {c.Path}");
                _logger.Debug($"\t\t\t * Port: {c.Port}");
                _logger.Debug($"\t\t\t * Secure: {c.Secure}");
                _logger.Debug($"\t\t\t * Issues On: {c.TimeStamp}");
                _logger.Debug($"\t\t\t * Expiration: {c.Expires} {((c.Expired) ? " (Expired)" : string.Empty)}");
                _logger.Debug($"\t\t\t * Don't Save: {c.Discard}");
                _logger.Debug($"\t\t\t * Comment: {c.Comment}");
                _logger.Debug($"\t\t\t * URI for Comments: {c.CommentUri}");
                _logger.Debug($"\t\t\t * Version: {((c.Version == 1) ? "2109" : "2965")}");
            }

            _logger.Debug("\t > Headers:");
            if (req.Headers.Count == 0)
            {
                _logger.Debug("\t\t - <NONE>");
            }
            foreach (string key in req.Headers.AllKeys)
            {
                string[] values = req.Headers.GetValues(key);
                _logger.Debug($"\t\t - {key}: {(values == null || values.Length == 0 ? "<NONE>" : String.Join(", ", values))}");
            }

            _logger.Debug("\t > Body:");
            _logger.Debug($"\t\t - Has Content: {req.HasEntityBody}");
            _logger.Debug($"\t\t - Content Length: {((!req.HasEntityBody) ? "N/A" : req.ContentLength64.ToString())}");
            _logger.Debug($"\t\t - Content: {content}");
            _logger.Debug($"\t > Query String: {req.QueryString}");
            _logger.Debug("######################################################################");
            _logger.Debug("##########                  END OF REQUEST                  ##########");
            _logger.Debug("######################################################################");
        }

        private ResponseInfo Handle(ResolvedHttpListenerRequest request)
        {
            ResponseInfo result = new ResponseInfo(HttpStatusCode.NotFound, StandardErrorResponseContent);

            if (request.Request.Url.Segments.Length < 2)
            {
                return result;
            }

            string pathElement = request.Request.Url.Segments[1].Replace("/", string.Empty);
            string nameSpaceName = $"{GetType().Namespace}.{pathElement}";

            Type nameSpace = (from type in Assembly.GetExecutingAssembly().GetTypes()
                                     where type.Namespace == nameSpaceName
                                     select type).FirstOrDefault();

            if (nameSpace != null)
            {
                MethodInfo subElementHandler = nameSpace.GetMethod(HANDLERNAME);
                if (subElementHandler != null)
                {
                    result = (ResponseInfo)subElementHandler.Invoke(this, new object[] {request});
                }
            }
            else
            {
                // See if we have a method to call
                MethodInfo method = typeof(Steward).GetMethod(pathElement);
                if (method != null)
                {
                    result = (ResponseInfo)method.Invoke(typeof(Steward), new object[] { request });
                }
            }

            return result;
        }

        public void Stop()
        {
            Status = RunStatus.Stopping;
            _listener.Stop();
        }
        #endregion

    }
}
