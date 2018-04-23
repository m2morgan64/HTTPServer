using System;
using System.Windows.Forms;
using NLog;
using Visualizer.Properties;

namespace Visualizer
{
    public partial class FrmVisualizer : Form
    {
        private readonly HTTPServer.Steward _server = new HTTPServer.Steward();
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();

        public FrmVisualizer()
        {
            LogManager.Configuration = _server.LogConfig;
            InitializeComponent();
            _server.OnStatusChange += Server_StatusChange;
        }

        private void frmVisualizer_Load(object sender, EventArgs e)
        {
            lblURI.Text = Settings.Default.URI;
            StartStop();
        }

        private void frmVisualizer_Closing(object sender, FormClosingEventArgs e)
        {
            _server.Stop();
        }

        private void cmdStartStop_Click(object sender, EventArgs e)
        {
            StartStop();
        }

        private void Server_StatusChange(object sender, EventArgs e)
        {
            SetServerStatus();
        }

        private delegate void SetServerStatusCallback();
        private void SetServerStatus()
        {
            if (Disposing || IsDisposed)
            {
                return;
            }

            if (InvokeRequired)
            {
                try
                {
                    SetServerStatusCallback d = SetServerStatus;
                    Invoke(d);
                }
                catch (Exception ex)
                {
                    _logger.Error(ex, "An exception occurred while attempting to set the server status.");
                }
            }
            else
            {
                switch (_server.Status)
                {
                    case HTTPServer.Steward.RunStatus.Stopped:
                        cmdStartStop.Image = Resources.Play;
                        cmdStartStop.Enabled = true;
                        lblStatus.Image = Resources.Dot_Grey;
                        break;
                    case HTTPServer.Steward.RunStatus.Running:
                        cmdStartStop.Image = Resources.Stop;
                        cmdStartStop.Enabled = true;
                        lblStatus.Image = Resources.Dot_Green;
                        break;
                    case HTTPServer.Steward.RunStatus.Starting:
                    case HTTPServer.Steward.RunStatus.Stopping:
                        cmdStartStop.Enabled = false;
                        lblStatus.Image = Resources.Dot_Purple;
                        break;
                }
            }
        }

        private void StartStop()
        {
            switch (_server.Status)
            {
                case HTTPServer.Steward.RunStatus.Running:
                    _logger.Info("Stopping the Server.");
                    _server.Stop();
                    break;
                case HTTPServer.Steward.RunStatus.Stopped:
                    _logger.Info("Starting the Server.");
                    _server.Prefixes.Add(Settings.Default.URI);
                    _server.Start();
                    break;
            }
        }

        private void cmdCopy_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(Settings.Default.URI);
        }
    }
}
