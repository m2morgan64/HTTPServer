using System;
using System.IO;
using System.Collections;
using System.Net;
using System.Net.Sockets;
using System.Text;
using TCPServer.lib;
using System.Threading;
using static TCPServer.lib.EventHelpers;
using System.Collections.Generic;

namespace TCPServer
{
    // https://msdn.microsoft.com/en-us/library/system.net.httplistener(v=vs.110).aspx
    // https://stackoverflow.com/questions/9329707/httplistener-for-simple-http-server-in-production
    public class Steward
    {
        #region public Enums/Constants
        public enum RunStatus { Stopped, Starting, Started, Stopping };
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
        #endregion

        #region Locals
        private HttpListener listener = new HttpListener();
        #endregion

        #region Constructors
        public Steward()
        {
            if (!HttpListener.IsSupported)
            {
                throw new Exception("Windows XP SP2 or Server 2003 is required to use the HttpListener class.");
            }

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
            throw new NotImplementedException();
        }

        private void Prefixes_OnAddRange(object sender, MultiStringEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void Prefixes_OnRemove(object sender, SingleStringEventArgs e)
        {
            throw new NotImplementedException();
        }
        
        private void Prefixes_OnRemoveRange(object sender, MultiStringEventArgs e)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region Start/Stop/Run
        public void Start()
        {
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
            Console.WriteLine("Listening...");
            // Note: The GetContext method blocks while waiting for a request. 
            HttpListenerContext context = listener.GetContext();
            HttpListenerRequest request = context.Request;
            // Obtain a response object.
            HttpListenerResponse response = context.Response;
            // Construct a response.
            string responseString = "<HTML><BODY> Hello world!</BODY></HTML>";
            byte[] buffer = System.Text.Encoding.UTF8.GetBytes(responseString);
            // Get a response stream and write the response to it.
            response.ContentLength64 = buffer.Length;
            System.IO.Stream output = response.OutputStream;
            output.Write(buffer, 0, buffer.Length);
            // You must close the output stream.
            output.Close();
            listener.Stop();
        }
        #endregion

    }
}
