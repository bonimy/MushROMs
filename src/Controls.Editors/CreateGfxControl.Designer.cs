// <copyright file="CreateGfxControl.Designer.cs" company="Public Domain">
//     Copyright (c) 2018 Nelson Garcia.
// </copyright>

namespace Controls.Editors
{
    partial class CreateGfxControl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CreateGfxControl));
            this.gbxNumTiles = new System.Windows.Forms.GroupBox();
            this.nudNumTiles = new System.Windows.Forms.NumericUpDown();
            this.chkFromCopy = new System.Windows.Forms.CheckBox();
            this.gpxGraphicsFormat = new System.Windows.Forms.GroupBox();
            this.cbxGraphicsFormat = new System.Windows.Forms.ComboBox();
            this.gbxNumTiles.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudNumTiles)).BeginInit();
            this.gpxGraphicsFormat.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbxNumTiles
            // 
            resources.ApplyResources(this.gbxNumTiles, "gbxNumTiles");
            this.gbxNumTiles.Controls.Add(this.nudNumTiles);
            this.gbxNumTiles.Name = "gbxNumTiles";
            this.gbxNumTiles.TabStop = false;
            // 
            // nudNumTiles
            // 
            resources.ApplyResources(this.nudNumTiles, "nudNumTiles");
            this.nudNumTiles.Hexadecimal = true;
            this.nudNumTiles.Maximum = new decimal(new int[] {
            8388608,
            0,
            0,
            0});
            this.nudNumTiles.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudNumTiles.Name = "nudNumTiles";
            this.nudNumTiles.Value = new decimal(new int[] {
            256,
            0,
            0,
            0});
            // 
            // chkFromCopy
            // 
            resources.ApplyResources(this.chkFromCopy, "chkFromCopy");
            this.chkFromCopy.Name = "chkFromCopy";
            this.chkFromCopy.UseVisualStyleBackColor = true;
            // 
            // gpxGraphicsFormat
            // 
            resources.ApplyResources(this.gpxGraphicsFormat, "gpxGraphicsFormat");
            this.gpxGraphicsFormat.Controls.Add(this.cbxGraphicsFormat);
            this.gpxGraphicsFormat.Name = "gpxGraphicsFormat";
            this.gpxGraphicsFormat.TabStop = false;
            // 
            // cbxGraphicsFormat
            // 
            resources.ApplyResources(this.cbxGraphicsFormat, "cbxGraphicsFormat");
            this.cbxGraphicsFormat.FormattingEnabled = true;
            this.cbxGraphicsFormat.Items.AddRange(new object[] {
            resources.GetString("cbxGraphicsFormat.Items"),
            resources.GetString("cbxGraphicsFormat.Items1"),
            resources.GetString("cbxGraphicsFormat.Items2"),
            resources.GetString("cbxGraphicsFormat.Items3"),
            resources.GetString("cbxGraphicsFormat.Items4"),
            resources.GetString("cbxGraphicsFormat.Items5"),
            resources.GetString("cbxGraphicsFormat.Items6"),
            resources.GetString("cbxGraphicsFormat.Items7"),
            resources.GetString("cbxGraphicsFormat.Items8"),
            resources.GetString("cbxGraphicsFormat.Items9"),
            resources.GetString("cbxGraphicsFormat.Items10"),
            resources.GetString("cbxGraphicsFormat.Items11"),
            resources.GetString("cbxGraphicsFormat.Items12"),
            resources.GetString("cbxGraphicsFormat.Items13"),
            resources.GetString("cbxGraphicsFormat.Items14"),
            resources.GetString("cbxGraphicsFormat.Items15"),
            resources.GetString("cbxGraphicsFormat.Items16"),
            resources.GetString("cbxGraphicsFormat.Items17")});
            this.cbxGraphicsFormat.Name = "cbxGraphicsFormat";
            // 
            // CreateGfxControl
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.gpxGraphicsFormat);
            this.Controls.Add(this.gbxNumTiles);
            this.Controls.Add(this.chkFromCopy);
            this.Name = "CreateGfxControl";
            this.gbxNumTiles.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.nudNumTiles)).EndInit();
            this.gpxGraphicsFormat.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox gbxNumTiles;
        private System.Windows.Forms.NumericUpDown nudNumTiles;
        private System.Windows.Forms.CheckBox chkFromCopy;
        private System.Windows.Forms.GroupBox gpxGraphicsFormat;
        private System.Windows.Forms.ComboBox cbxGraphicsFormat;
    }
}
