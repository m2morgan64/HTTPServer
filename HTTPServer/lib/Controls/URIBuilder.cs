using System;
using System.Diagnostics.CodeAnalysis;
using System.Windows.Forms;

namespace HTTPServer.lib.Controls
{
    public partial class UriBuilder : UserControl
    {
        public EventHandler<EventArgs> UriChanged;
        private void RaiseUriChanged()
        {
            if (UriChanged != null && _populateComplete)
            {
                UriChanged(this, new EventArgs());
            }
        }
        private bool _loadComplete;
        private bool _populateComplete;

        private System.UriBuilder _uri = new System.UriBuilder();
        public Uri Uri
        {
            get => _uri.Uri;
            set
            {
                _uri = new System.UriBuilder(value);
                Populate();
            }
        }

        private void _URIBuilder(Uri uri)
        {
            InitializeComponent();
            Uri = uri;
        }

        [SuppressMessage("ReSharper", "UnusedMember.Global")]
        public UriBuilder(string uri)
        {
            _URIBuilder(new Uri(uri));
        }

        [SuppressMessage("ReSharper", "UnusedMember.Global")]
        public UriBuilder(Uri uri)
        {
            _URIBuilder(uri);
        }

        [SuppressMessage("ReSharper", "UnusedMember.Global")]
        public UriBuilder()
        {
            
        }

        private void URIBuilder_Load(object sender, EventArgs e)
        {
            _loadComplete = true;
        }

        private void Populate()
        {
            if (_loadComplete)
            {
                foreach (string t in cboScheme.Items)
                {
                    if (t == _uri.Scheme)
                    {
                        cboScheme.SelectedItem = t;
                        break;
                    }
                }

                txtHost.Text = _uri.Host;
                nudPort.Value = _uri.Port;
                txtPath.Text = _uri.Path;
            }

            _populateComplete = true;
        }

        private void cboScheme_SelectedIndexChanged(object sender, EventArgs e)
        {
            _uri.Scheme = cboScheme.Text;
            RaiseUriChanged();
        }

        private void txtHost_TextChanged(object sender, EventArgs e)
        {
            _uri.Host = txtHost.Text;
            RaiseUriChanged();
        }

        private void nudPort_ValueChanged(object sender, EventArgs e)
        {
            _uri.Port = (int)nudPort.Value;
            RaiseUriChanged();
        }

        private void txtPath_TextChanged(object sender, EventArgs e)
        {
            _uri.Path = txtPath.Text;
            RaiseUriChanged();
        }

        private void cmdCopy_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(_uri.ToString());
        }

        private void cmdPaste_Click(object sender, EventArgs e)
        {
            _populateComplete = false;
            string cb = Clipboard.GetText();
            try
            {
                Uri = new Uri(cb);
            }
            catch
            {
                MessageBox.Show($@"'{cb}' is not a valid URI.", @"Invalid URI", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            _populateComplete = true;
            RaiseUriChanged();
        }
    }
}
