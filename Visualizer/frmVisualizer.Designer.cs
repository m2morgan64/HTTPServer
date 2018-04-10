using System;
using System.Windows.Forms;

namespace Visualizer
{
    partial class frmVisualizer
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmVisualizer));
            this.tt = new System.Windows.Forms.ToolTip(this.components);
            this.tsMain = new System.Windows.Forms.ToolStrip();
            this.cmdStartStop = new System.Windows.Forms.ToolStripButton();
            this.ssMain = new System.Windows.Forms.StatusStrip();
            this.lblStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.pnlMain = new System.Windows.Forms.Panel();
            this.pnlHeader = new System.Windows.Forms.Panel();
            this.pnlInfoMenu = new System.Windows.Forms.Panel();
            this.pnlMainMenu = new System.Windows.Forms.Panel();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.cmdCopy = new System.Windows.Forms.ToolStripButton();
            this.lblURI = new System.Windows.Forms.ToolStripLabel();
            this.tsMain.SuspendLayout();
            this.ssMain.SuspendLayout();
            this.pnlHeader.SuspendLayout();
            this.pnlInfoMenu.SuspendLayout();
            this.pnlMainMenu.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tsMain
            // 
            this.tsMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tsMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cmdStartStop});
            this.tsMain.Location = new System.Drawing.Point(0, 0);
            this.tsMain.Name = "tsMain";
            this.tsMain.Size = new System.Drawing.Size(535, 25);
            this.tsMain.TabIndex = 0;
            this.tsMain.Text = "tsMain";
            // 
            // cmdStartStop
            // 
            this.cmdStartStop.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.cmdStartStop.Image = global::Visualizer.Properties.Resources.Play;
            this.cmdStartStop.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.cmdStartStop.Name = "cmdStartStop";
            this.cmdStartStop.Size = new System.Drawing.Size(23, 22);
            this.cmdStartStop.Text = "Start/Stop the Server";
            this.cmdStartStop.ToolTipText = "Start/Stop the Server";
            this.cmdStartStop.Click += new System.EventHandler(this.cmdStartStop_Click);
            // 
            // ssMain
            // 
            this.ssMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblStatus});
            this.ssMain.Location = new System.Drawing.Point(0, 358);
            this.ssMain.Name = "ssMain";
            this.ssMain.Size = new System.Drawing.Size(802, 22);
            this.ssMain.TabIndex = 1;
            this.ssMain.Text = "statusStrip1";
            // 
            // lblStatus
            // 
            this.lblStatus.Image = global::Visualizer.Properties.Resources.Dot_Purple;
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(71, 17);
            this.lblStatus.Text = "<Status>";
            // 
            // pnlMain
            // 
            this.pnlMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlMain.Location = new System.Drawing.Point(0, 0);
            this.pnlMain.Name = "pnlMain";
            this.pnlMain.Size = new System.Drawing.Size(802, 358);
            this.pnlMain.TabIndex = 2;
            // 
            // pnlHeader
            // 
            this.pnlHeader.Controls.Add(this.pnlMainMenu);
            this.pnlHeader.Controls.Add(this.pnlInfoMenu);
            this.pnlHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlHeader.Location = new System.Drawing.Point(0, 0);
            this.pnlHeader.Name = "pnlHeader";
            this.pnlHeader.Size = new System.Drawing.Size(802, 25);
            this.pnlHeader.TabIndex = 3;
            // 
            // pnlInfoMenu
            // 
            this.pnlInfoMenu.Controls.Add(this.toolStrip1);
            this.pnlInfoMenu.Dock = System.Windows.Forms.DockStyle.Right;
            this.pnlInfoMenu.Location = new System.Drawing.Point(535, 0);
            this.pnlInfoMenu.Name = "pnlInfoMenu";
            this.pnlInfoMenu.Size = new System.Drawing.Size(267, 25);
            this.pnlInfoMenu.TabIndex = 0;
            // 
            // pnlMainMenu
            // 
            this.pnlMainMenu.Controls.Add(this.tsMain);
            this.pnlMainMenu.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlMainMenu.Location = new System.Drawing.Point(0, 0);
            this.pnlMainMenu.Name = "pnlMainMenu";
            this.pnlMainMenu.Size = new System.Drawing.Size(535, 25);
            this.pnlMainMenu.TabIndex = 1;
            // 
            // toolStrip1
            // 
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cmdCopy,
            this.lblURI});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.toolStrip1.Size = new System.Drawing.Size(267, 25);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "tsInfo";
            // 
            // cmdCopy
            // 
            this.cmdCopy.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.cmdCopy.Image = ((System.Drawing.Image)(resources.GetObject("cmdCopy.Image")));
            this.cmdCopy.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.cmdCopy.Name = "cmdCopy";
            this.cmdCopy.Size = new System.Drawing.Size(23, 22);
            this.cmdCopy.Text = "Copy";
            this.cmdCopy.Click += new System.EventHandler(this.cmdCopy_Click);
            // 
            // lblURI
            // 
            this.lblURI.Font = new System.Drawing.Font("Consolas", 12F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))));
            this.lblURI.ForeColor = System.Drawing.Color.Blue;
            this.lblURI.Name = "lblURI";
            this.lblURI.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.lblURI.Size = new System.Drawing.Size(54, 22);
            this.lblURI.Text = "<URI>";
            this.lblURI.ToolTipText = "URI";
            // 
            // frmVisualizer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(802, 380);
            this.Controls.Add(this.pnlHeader);
            this.Controls.Add(this.pnlMain);
            this.Controls.Add(this.ssMain);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmVisualizer";
            this.Text = "HTTP Server Visualizer";
            this.Load += new System.EventHandler(this.frmVisualizer_Load);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmVisualizer_Closing);
            this.tsMain.ResumeLayout(false);
            this.tsMain.PerformLayout();
            this.ssMain.ResumeLayout(false);
            this.ssMain.PerformLayout();
            this.pnlHeader.ResumeLayout(false);
            this.pnlInfoMenu.ResumeLayout(false);
            this.pnlInfoMenu.PerformLayout();
            this.pnlMainMenu.ResumeLayout(false);
            this.pnlMainMenu.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolTip tt;
        private System.Windows.Forms.ToolStrip tsMain;
        private System.Windows.Forms.StatusStrip ssMain;
        private System.Windows.Forms.ToolStripButton cmdStartStop;
        private System.Windows.Forms.ToolStripStatusLabel lblStatus;
        private System.Windows.Forms.Panel pnlMain;
        private System.Windows.Forms.Panel pnlHeader;
        private System.Windows.Forms.Panel pnlMainMenu;
        private System.Windows.Forms.Panel pnlInfoMenu;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton cmdCopy;
        private System.Windows.Forms.ToolStripLabel lblURI;
    }
}

