// <copyright file="PaletteForm.Designer.cs" company="Public Domain">
//     Copyright (c) 2018 Nelson Garcia.
// </copyright>

namespace Controls.Editors
{
    partial class PaletteForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PaletteForm));
            this.tileMapControl = new Controls.TileMapControl1D();
            this.hScrollBar1 = new System.Windows.Forms.HScrollBar();
            this.vScrollBar1 = new System.Windows.Forms.VScrollBar();
            this.stsMain = new System.Windows.Forms.StatusStrip();
            this.tssMain = new System.Windows.Forms.ToolStripStatusLabel();
            this.paletteStatus = new Controls.Editors.PaletteStatus();
            this.stsMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // tileMapControl
            // 
            resources.ApplyResources(this.tileMapControl, "tileMapControl");
            this.tileMapControl.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tileMapControl.GridSize = 0;
            this.tileMapControl.HorizontalScrollBar = this.hScrollBar1;
            this.tileMapControl.Name = "tileMapControl";
            this.tileMapControl.TileSize = new System.Drawing.Size(1, 1);
            this.tileMapControl.VerticalScrollBar = this.vScrollBar1;
            this.tileMapControl.ViewSize = new System.Drawing.Size(16, 16);
            this.tileMapControl.ZeroTile = 0;
            this.tileMapControl.ZoomSize = new System.Drawing.Size(16, 16);
            this.tileMapControl.ClientSizeChanged += new System.EventHandler(this.TileMapControl_ClientSizeChanged);
            // 
            // hScrollBar1
            // 
            resources.ApplyResources(this.hScrollBar1, "hScrollBar1");
            this.hScrollBar1.LargeChange = 15;
            this.hScrollBar1.Maximum = 29;
            this.hScrollBar1.Name = "hScrollBar1";
            // 
            // vScrollBar1
            // 
            resources.ApplyResources(this.vScrollBar1, "vScrollBar1");
            this.vScrollBar1.Name = "vScrollBar1";
            // 
            // stsMain
            // 
            this.stsMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tssMain});
            resources.ApplyResources(this.stsMain, "stsMain");
            this.stsMain.Name = "stsMain";
            this.stsMain.SizingGrip = false;
            // 
            // tssMain
            // 
            this.tssMain.Name = "tssMain";
            resources.ApplyResources(this.tssMain, "tssMain");
            // 
            // paletteStatus
            // 
            resources.ApplyResources(this.paletteStatus, "paletteStatus");
            this.paletteStatus.Name = "paletteStatus";
            this.paletteStatus.ZoomScaleChanged += new System.EventHandler(this.ZoomScaleChanged);
            // 
            // PaletteForm
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.stsMain);
            this.Controls.Add(this.paletteStatus);
            this.Controls.Add(this.vScrollBar1);
            this.Controls.Add(this.hScrollBar1);
            this.Controls.Add(this.tileMapControl);
            this.MaximizeBox = false;
            this.Name = "PaletteForm";
            this.TileMapControl = this.tileMapControl;
            this.Load += new System.EventHandler(this.PaletteForm_Load);
            this.ResizeEnd += new System.EventHandler(this.PaletteForm_ResizeEnd);
            this.stsMain.ResumeLayout(false);
            this.stsMain.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private TileMapControl1D tileMapControl;
        private System.Windows.Forms.HScrollBar hScrollBar1;
        private System.Windows.Forms.VScrollBar vScrollBar1;
        private PaletteStatus paletteStatus;
        private System.Windows.Forms.StatusStrip stsMain;
        private System.Windows.Forms.ToolStripStatusLabel tssMain;
    }
}