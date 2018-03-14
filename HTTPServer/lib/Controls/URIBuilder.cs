using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TCPServer.lib.Controls
{
    public partial class URIBuilder : UserControl
    {
        public EventHandler<EventArgs> URIChanged;
        private void RaiseURIChanged()
        {
            if (URIChanged != null && populateComplete)
            {
                URIChanged(this, new EventArgs());
            }
        }
        private bool loadComplete = false;
        private bool populateComplete = false;

        private UriBuilder _uri = new UriBuilder();
        public Uri Uri
        {
            get
            {
                return _uri.Uri;
            }
            set
            {
                _uri = new UriBuilder(value);
                Populate();
            }
        }

        private void _URIBuilder(Uri uri)
        {
            InitializeComponent();
        }

        public URIBuilder(string uri)
        {
            _URIBuilder(new Uri(uri));
        }

        public URIBuilder(Uri uri)
        {
            _URIBuilder(uri);
        }

        public URIBuilder()
        {
            
        }

        private void URIBuilder_Load(object sender, EventArgs e)
        {
            loadComplete = true;
        }

        private void Populate()
        {
            if (loadComplete)
            {
                for (int i = 0; i < cboScheme.Items.Count; i++)
                {
                    if ((string)cboScheme.Items[i] == _uri.Scheme)
                    {
                        cboScheme.SelectedItem = cboScheme.Items[i];
                        break;
                    }
                }

                txtHost.Text = _uri.Host;
                nudPort.Value = _uri.Port;
                txtPath.Text = _uri.Path;
            }

            populateComplete = true;
        }

        private void cboScheme_SelectedIndexChanged(object sender, EventArgs e)
        {
            _uri.Scheme = cboScheme.Text;
            RaiseURIChanged();
        }

        private void txtHost_TextChanged(object sender, EventArgs e)
        {
            _uri.Host = txtHost.Text;
            RaiseURIChanged();
        }

        private void nudPort_ValueChanged(object sender, EventArgs e)
        {
            _uri.Port = (int)nudPort.Value;
            RaiseURIChanged();
        }

        private void txtPath_TextChanged(object sender, EventArgs e)
        {
            _uri.Path = txtPath.Text;
            RaiseURIChanged();
        }

        private void cmdCopy_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(_uri.ToString());
        }

        private void cmdPaste_Click(object sender, EventArgs e)
        {
            populateComplete = false;
            string cb = Clipboard.GetText();
            try
            {
                this.Uri = new Uri(cb);
            }
            catch
            {
                MessageBox.Show($"'{cb}' is not a valid URI.", "Invalid URI", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            populateComplete = true;
            RaiseURIChanged();
        }
    }
}
