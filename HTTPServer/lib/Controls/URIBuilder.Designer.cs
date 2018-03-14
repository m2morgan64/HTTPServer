namespace TCPServer.lib.Controls
{
    partial class URIBuilder
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(URIBuilder));
            this.cboScheme = new System.Windows.Forms.ComboBox();
            this.lblSchemaDivider = new System.Windows.Forms.Label();
            this.txtHost = new System.Windows.Forms.TextBox();
            this.lblHostDivider = new System.Windows.Forms.Label();
            this.txtPath = new System.Windows.Forms.TextBox();
            this.nudPort = new System.Windows.Forms.NumericUpDown();
            this.lblPortDivider = new System.Windows.Forms.Label();
            this.cmdCopy = new System.Windows.Forms.Button();
            this.tt = new System.Windows.Forms.ToolTip(this.components);
            this.cmdPaste = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.nudPort)).BeginInit();
            this.SuspendLayout();
            // 
            // cboScheme
            // 
            this.cboScheme.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboScheme.FormattingEnabled = true;
            this.cboScheme.Items.AddRange(new object[] {
            "http",
            "https"});
            this.cboScheme.Location = new System.Drawing.Point(4, 4);
            this.cboScheme.Name = "cboScheme";
            this.cboScheme.Size = new System.Drawing.Size(71, 27);
            this.cboScheme.TabIndex = 0;
            this.cboScheme.SelectedIndexChanged += new System.EventHandler(this.cboScheme_SelectedIndexChanged);
            // 
            // lblSchemaDivider
            // 
            this.lblSchemaDivider.AutoSize = true;
            this.lblSchemaDivider.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Bold);
            this.lblSchemaDivider.Location = new System.Drawing.Point(70, 7);
            this.lblSchemaDivider.Margin = new System.Windows.Forms.Padding(0);
            this.lblSchemaDivider.Name = "lblSchemaDivider";
            this.lblSchemaDivider.Size = new System.Drawing.Size(36, 19);
            this.lblSchemaDivider.TabIndex = 1;
            this.lblSchemaDivider.Text = "://";
            // 
            // txtHost
            // 
            this.txtHost.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Bold);
            this.txtHost.Location = new System.Drawing.Point(105, 5);
            this.txtHost.Name = "txtHost";
            this.txtHost.Size = new System.Drawing.Size(270, 26);
            this.txtHost.TabIndex = 2;
            this.txtHost.TextChanged += new System.EventHandler(this.txtHost_TextChanged);
            // 
            // lblHostDivider
            // 
            this.lblHostDivider.AutoSize = true;
            this.lblHostDivider.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Bold);
            this.lblHostDivider.Location = new System.Drawing.Point(372, 8);
            this.lblHostDivider.Margin = new System.Windows.Forms.Padding(0);
            this.lblHostDivider.Name = "lblHostDivider";
            this.lblHostDivider.Size = new System.Drawing.Size(18, 19);
            this.lblHostDivider.TabIndex = 3;
            this.lblHostDivider.Text = ":";
            // 
            // txtPath
            // 
            this.txtPath.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Bold);
            this.txtPath.Location = new System.Drawing.Point(469, 5);
            this.txtPath.Name = "txtPath";
            this.txtPath.Size = new System.Drawing.Size(333, 26);
            this.txtPath.TabIndex = 4;
            this.txtPath.TextChanged += new System.EventHandler(this.txtPath_TextChanged);
            // 
            // nudPort
            // 
            this.nudPort.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Bold);
            this.nudPort.Location = new System.Drawing.Point(388, 6);
            this.nudPort.Maximum = new decimal(new int[] {
            64000,
            0,
            0,
            0});
            this.nudPort.Name = "nudPort";
            this.nudPort.Size = new System.Drawing.Size(68, 26);
            this.nudPort.TabIndex = 5;
            this.nudPort.Value = new decimal(new int[] {
            64000,
            0,
            0,
            0});
            this.nudPort.ValueChanged += new System.EventHandler(this.nudPort_ValueChanged);
            // 
            // lblPortDivider
            // 
            this.lblPortDivider.AutoSize = true;
            this.lblPortDivider.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Bold);
            this.lblPortDivider.Location = new System.Drawing.Point(453, 8);
            this.lblPortDivider.Margin = new System.Windows.Forms.Padding(0);
            this.lblPortDivider.Name = "lblPortDivider";
            this.lblPortDivider.Size = new System.Drawing.Size(18, 19);
            this.lblPortDivider.TabIndex = 6;
            this.lblPortDivider.Text = "/";
            // 
            // cmdCopy
            // 
            this.cmdCopy.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdCopy.Image = ((System.Drawing.Image)(resources.GetObject("cmdCopy.Image")));
            this.cmdCopy.Location = new System.Drawing.Point(808, 6);
            this.cmdCopy.Name = "cmdCopy";
            this.cmdCopy.Size = new System.Drawing.Size(23, 23);
            this.cmdCopy.TabIndex = 7;
            this.tt.SetToolTip(this.cmdCopy, "Copy");
            this.cmdCopy.UseVisualStyleBackColor = true;
            this.cmdCopy.Click += new System.EventHandler(this.cmdCopy_Click);
            // 
            // cmdPaste
            // 
            this.cmdPaste.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdPaste.Image = ((System.Drawing.Image)(resources.GetObject("cmdPaste.Image")));
            this.cmdPaste.Location = new System.Drawing.Point(837, 7);
            this.cmdPaste.Name = "cmdPaste";
            this.cmdPaste.Size = new System.Drawing.Size(23, 23);
            this.cmdPaste.TabIndex = 8;
            this.tt.SetToolTip(this.cmdPaste, "Paste a URI from the clipboard");
            this.cmdPaste.UseVisualStyleBackColor = true;
            this.cmdPaste.Click += new System.EventHandler(this.cmdPaste_Click);
            // 
            // URIBuilder
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.cmdPaste);
            this.Controls.Add(this.cmdCopy);
            this.Controls.Add(this.nudPort);
            this.Controls.Add(this.txtPath);
            this.Controls.Add(this.txtHost);
            this.Controls.Add(this.cboScheme);
            this.Controls.Add(this.lblSchemaDivider);
            this.Controls.Add(this.lblHostDivider);
            this.Controls.Add(this.lblPortDivider);
            this.Name = "URIBuilder";
            this.Size = new System.Drawing.Size(863, 38);
            this.Load += new System.EventHandler(this.URIBuilder_Load);
            ((System.ComponentModel.ISupportInitialize)(this.nudPort)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cboScheme;
        private System.Windows.Forms.Label lblSchemaDivider;
        private System.Windows.Forms.TextBox txtHost;
        private System.Windows.Forms.Label lblHostDivider;
        private System.Windows.Forms.TextBox txtPath;
        private System.Windows.Forms.NumericUpDown nudPort;
        private System.Windows.Forms.Label lblPortDivider;
        private System.Windows.Forms.Button cmdCopy;
        private System.Windows.Forms.ToolTip tt;
        private System.Windows.Forms.Button cmdPaste;
    }
}
