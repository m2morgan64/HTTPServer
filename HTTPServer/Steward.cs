using System;
using System.IO;
using System.Collections;
using System.Net;
using System.Net.Sockets;
using System.Text;
using NLog;
using HTTPServer.lib;
using System.Threading;
using static HTTPServer.lib.Utils.EventHelpers;
using System.Collections.Generic;
using System.Linq;

namespace HTTPServer
{
    // https://msdn.microsoft.com/en-us/library/system.net.httplistener(v=vs.110).aspx
    // https://stackoverflow.com/questions/9329707/httplistener-for-simple-http-server-in-production
    public class Steward
    {
        #region public Enums/Constants
        public enum RunStatus { Stopped, Starting, Running, Stopping };
        public const string LogTarget = "logfile";
        #endregion

        #region Classes

        #endregion

        #region Events
        public event EventHandler<EventArgs> OnStatusChange;
        #endregion

        #region Properties
        public HTTPPrefixes<String> Prefixes { get; protected set; }

        private RunStatus _status = RunStatus.Stopped;
        public RunStatus Status
        {
            get { return _status; }
            set
            {
                if (_status != value)
                {
                    _status = value;
                    RaiseSimpleEvent(this, OnStatusChange);
                }
            }
        }

        private static string _StandardErrorResponseContent = Properties.Resources.ErrorResponseContentTemplate;
        public static string StandardErrorResponseContent { get { return _StandardErrorResponseContent; }  }

        public NLog.Config.LoggingConfiguration LogConfig { get; protected set; }
        #endregion

        #region Locals
        private HttpListener listener = new HttpListener();
        private Logger logger = NLog.LogManager.GetCurrentClassLogger();
        #endregion

        #region Constructors
        public Steward()
        {
            this.LogConfig = new NLog.Config.LoggingConfiguration();
            NLog.Targets.FileTarget logfile = new NLog.Targets.FileTarget() { FileName = Properties.Settings.Default.LogFile, Name = LogTarget , CreateDirs = true };
            NLog.Targets.ConsoleTarget logconsole = new NLog.Targets.ConsoleTarget() { Name = "logconsole" };

            this.LogConfig.LoggingRules.Add(new NLog.Config.LoggingRule("*", LogLevel.Info, logfile));
            this.LogConfig.LoggingRules.Add(new NLog.Config.LoggingRule("*", LogLevel.Debug, logconsole));

            NLog.LogManager.Configuration = this.LogConfig;

            Prefixes = new HTTPPrefixes<string>();
            Prefixes.OnAdd += new EventHandler<SingleStringEventArgs>(Prefixes_OnAdd);
            Prefixes.OnAddRange += new EventHandler<MultiStringEventArgs>(Prefixes_OnAddRange);
            Prefixes.OnRemove += new EventHandler<SingleStringEventArgs>(Prefixes_OnRemove);
            Prefixes.OnRemoveRange += new EventHandler<MultiStringEventArgs>(Prefixes_OnRemoveRange);
        }

        public Steward(string httpPrefix) : this()
        {
            Prefixes.Add(httpPrefix);
        }

        public Steward(List<string> httpPrefixes) : this()
        {
            Prefixes.AddRange(httpPrefixes);
        }
        #endregion

        #region Prefixes Event Handlers
        private void Prefixes_OnAdd(object sender, SingleStringEventArgs e)
        {
            // No need to update the listener list unless the listener is running
            if (this.Status == RunStatus.Running)
            {
                try
                {
                    listener.Prefixes.Add(e.Value);
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
            if (this.Status == RunStatus.Running)
            {
                // If this is the last listener in the list, we need to stop the server.
                if (listener.Prefixes.Count == 1
                    && listener.Prefixes.Contains(e.Value))
                {
                    Stop();
                }
                listener.Prefixes.Remove(e.Value);
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
            this.Status = RunStatus.Starting;

            if (this.Prefixes.Count == 0)
            {
                throw new Exception("At least one http prefix must be specified to start the server.\r\n for example http://+:6464/api/v1/");
            }

            listener = new HttpListener();
            // Add the prefixes.
            foreach (string s in this.Prefixes)
            {
                listener.Prefixes.Add(s);
            }

            Thread thread = new Thread(new ThreadStart(Run));
            thread.Start();
        }

        public void Run()
        { 
            listener.Start();
            this.Status = RunStatus.Running;
            
            while (listener.IsListening)
            {
                IAsyncResult result = listener.BeginGetContext(Handle, listener);
                result.AsyncWaitHandle.WaitOne();
            }
            listener.Close();
            this.Status = RunStatus.Stopped;
            //// Note: The GetContext method blocks while waiting for a request. 
            //HttpListenerContext context = _listener.GetContext();
            //HttpListenerRequest request = context.Request;
            //// Obtain a response object.
            //HttpListenerResponse response = context.Response;
            //// Construct a response.
            //string responseString = "<HTML><BODY> Hello world!</BODY></HTML>";
            //byte[] buffer = System.Text.Encoding.UTF8.GetBytes(responseString);
            //// Get a response stream and write the response to it.
            //response.ContentLength64 = buffer.Length;
            //System.IO.Stream output = response.OutputStream;
            //output.Write(buffer, 0, buffer.Length);
            //// You must close the output stream.
            //output.Close();
            //_listener.Stop();
        }
        
        private void Handle(IAsyncResult result)
        {
            HttpListener listener = (HttpListener)result.AsyncState;
            if (listener.IsListening)
            {
                HttpListenerContext context = listener.EndGetContext(result);

                ResolvedHttpListenerRequest req = new ResolvedHttpListenerRequest(context.Request);
                LogIncomingRequest(req.Request, req.Content);

                // Obtain a response object.
                ResponseInfo responseContent = RouteRequest(req);

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
            logger.Debug("######################################################################");
            logger.Debug("##########                                                  ##########");
            logger.Debug("##########                 START OF REQUEST                 ##########");
            logger.Debug("##########                                                  ##########");
            logger.Debug("######################################################################");
            logger.Info($"Received Message: {req.HttpMethod} {req.Url} {req.ProtocolVersion}");
            if (!logger.IsDebugEnabled)
            {
                return;
            }

            logger.Debug($"From: {req.UserAgent}");
            logger.Debug($"User-Agent: {req.UserAgent}");
            logger.Debug("______________________________________________________________________");
            logger.Debug($"\t > Encoding: {req.ContentEncoding}");
            logger.Debug($"\t > Content Type: {req.ContentType}");
            logger.Debug($"\t > Server's Endpoint: {req.LocalEndPoint}");
            logger.Debug($"\t > Client's Endpoint: {req.RemoteEndPoint}");
            logger.Debug($"\t > Service Provider Name: {req.ServiceName}");
            logger.Debug($"\t > User Host: {req.UserHostAddress} {((req.UserHostName == string.Empty) ? string.Empty :$" ({req.UserHostName})")}");
            logger.Debug($"\t > User Language(s): { ((req.UserLanguages == null) ? "<NONE>" : String.Join(", ", req.UserLanguages)) }");
            logger.Debug($"\t > Local Request: {req.IsLocal}");
            logger.Debug($"\t > Web Socket Request: {req.IsWebSocketRequest}");
            logger.Debug($"\t > Keep Alive Request: {req.KeepAlive}");
            logger.Debug($"\t > Trace Identifier: {req.RequestTraceIdentifier}");
            logger.Debug($"\t > URLs:");
            logger.Debug($"\t\t - Raw URL: {req.RawUrl}");
            logger.Debug($"\t\t - URL: {req.Url}");
            logger.Debug($"\t\t - URL Referrer: {req.UrlReferrer}");
            logger.Debug($"\t > Security: ");
            logger.Debug($"\t\t - Is Authenticated: {req.IsAuthenticated}");
            logger.Debug($"\t\t - Is Secured (SSL): {req.IsSecureConnection}");
            logger.Debug($"\t\t - Has Certificate: {req.GetClientCertificate() == null}");
            logger.Debug($"\t\t - Certificate Error Code: {((!req.IsAuthenticated || req.GetClientCertificate() == null) ? "N/A" : req.ClientCertificateError.ToString()) }");

            logger.Debug($"\t > Cookies:");
            if (req.Cookies.Count == 0)
            {
                logger.Debug($"\t\t - <NONE>");
            }
            foreach (Cookie c in req.Cookies)
            {
                logger.Debug($"\t\t - {c.Name}");
                logger.Debug($"\t\t\t * Value: {c.Value}");
                logger.Debug($"\t\t\t * Domain: {c.Domain}");
                logger.Debug($"\t\t\t * Path: {c.Path}");
                logger.Debug($"\t\t\t * Port: {c.Port}");
                logger.Debug($"\t\t\t * Secure: {c.Secure}");
                logger.Debug($"\t\t\t * Issues On: {c.TimeStamp}");
                logger.Debug($"\t\t\t * Expiration: {c.Expires} {((c.Expired) ? " (Expired)" : string.Empty)}");
                logger.Debug($"\t\t\t * Don't Save: {c.Discard}");
                logger.Debug($"\t\t\t * Comment: {c.Comment}");
                logger.Debug($"\t\t\t * URI for Comments: {c.CommentUri}");
                logger.Debug($"\t\t\t * Version: {((c.Version == 1) ? "2109" : "2965")}");
            }

            logger.Debug($"\t > Headers:");
            if (req.Headers.Count == 0)
            {
                logger.Debug($"\t\t - <NONE>");
            }
            foreach (string key in req.Headers.AllKeys)
            {
                string[] values = req.Headers.GetValues(key);
                logger.Debug($"\t\t - {key}: {((values.Length == 0) ? "<NONE>" : String.Join(", ", values))}");
            }

            logger.Debug($"\t > Body:");
            logger.Debug($"\t\t - Has Content: {req.HasEntityBody}");
            logger.Debug($"\t\t - Content Length: {((!req.HasEntityBody) ? "N/A" : req.ContentLength64.ToString())}");
            logger.Debug($"\t\t - Content: {content}");
            logger.Debug($"\t > Query String: {req.QueryString}");
            logger.Debug("######################################################################");
            logger.Debug("##########                  END OF REQUEST                  ##########");
            logger.Debug("######################################################################");
        }

        private ResponseInfo RouteRequest(ResolvedHttpListenerRequest request)
        {
            ResponseInfo result = new ResponseInfo(HttpStatusCode.NotFound, _StandardErrorResponseContent);

            if (request.Request.Url.Segments.Length < 2)
            {
                return result;
            }

            string methodName = request.Request.Url.Segments[1].Replace("/", string.Empty).ToUpper();
            
            switch (methodName)
            {
                case "API":
                    result = API.RequestHandler.Handle(request);
                    break;
            }

            //string[] strParams = request.Request.Url.Segments.Skip(2).Select(s => s.Replace("/", string.Empty)).ToArray();

            //System.Reflection.MethodInfo method = this.GetType().GetMethod(methodName);

            //object[] @params = method.GetParameters().Select((p, i) => Convert.ChangeType(strParams[i], p.ParameterType)).ToArray();

            //ResponseInfo ret = (ResponseInfo)method.Invoke(this, @params);

            return result;
        }

        private void RunServerCallback(IAsyncResult ar)
        {
            try
            {
                Action target = (Action)ar.AsyncState;
                target.EndInvoke(ar);
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Failed to initialize Server");
            }
        }

        public void Stop()
        {
            this.Status = RunStatus.Stopping;
            listener.Stop();
        }
        #endregion

    }
}
