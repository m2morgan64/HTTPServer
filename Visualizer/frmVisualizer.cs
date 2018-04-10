using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using NLog;

namespace Visualizer
{
    public partial class frmVisualizer : Form
    {
        private HTTPServer.Steward server = new HTTPServer.Steward();
        private Logger logger = NLog.LogManager.GetCurrentClassLogger();

        public frmVisualizer()
        {
            NLog.LogManager.Configuration = server.LogConfig;
            InitializeComponent();
            server.OnStatusChange += new EventHandler<EventArgs>(this.Server_StatusChange);
        }

        private void frmVisualizer_Load(object sender, EventArgs e)
        {
            lblURI.Text = Properties.Settings.Default.URI;
            StartStop();
        }

        private void frmVisualizer_Closing(object sender, FormClosingEventArgs e)
        {
            server.Stop();
        }

        private void cmdStartStop_Click(object sender, EventArgs e)
        {
            StartStop();
        }

        private void Server_StatusChange(object sender, EventArgs e)
        {
            SetServerStatus();
        }

        delegate void SetServerStatusCallback();
        private void SetServerStatus()
        {
            if (this.Disposing || this.IsDisposed)
            {
                return;
            }

            if (this.InvokeRequired)
            {
                try
                {
                    SetServerStatusCallback d = new SetServerStatusCallback(SetServerStatus);
                    this.Invoke(d);
                }
                catch (Exception ex)
                {
                    logger.Error(ex, "An exception occurred while attempting to set the server status.");
                }
            }
            else
            {
                switch (server.Status)
                {
                    case HTTPServer.Steward.RunStatus.Stopped:
                        cmdStartStop.Image = Properties.Resources.Play;
                        cmdStartStop.Enabled = true;
                        lblStatus.Image = Properties.Resources.Dot_Grey;
                        break;
                    case HTTPServer.Steward.RunStatus.Running:
                        cmdStartStop.Image = Properties.Resources.Stop;
                        cmdStartStop.Enabled = true;
                        lblStatus.Image = Properties.Resources.Dot_Green;
                        break;
                    case HTTPServer.Steward.RunStatus.Starting:
                    case HTTPServer.Steward.RunStatus.Stopping:
                        cmdStartStop.Enabled = false;
                        lblStatus.Image = Properties.Resources.Dot_Purple;
                        break;
                    default:
                        break;
                }
            }
        }

        private void StartStop()
        {
            switch (server.Status)
            {
                case HTTPServer.Steward.RunStatus.Running:
                    logger.Info("Stopping the Server.");
                    server.Stop();
                    break;
                case HTTPServer.Steward.RunStatus.Stopped:
                    logger.Info("Starting the Server.");
                    server.Prefixes.Add(Properties.Settings.Default.URI);
                    server.Start();
                    break;
                default:
                    // If it's in a transition state, then we don't want to job it's elbow.
                    break;
            }
        }

        private void cmdCopy_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(Properties.Settings.Default.URI);
        }
    }
}
