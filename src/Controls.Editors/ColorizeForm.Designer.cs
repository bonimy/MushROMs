// <copyright file="ColorizeForm.Designer.cs" company="Public Domain">
//     Copyright (c) 2018 Nelson Garcia.
// </copyright>

namespace Controls.Editors
{
    partial class ColorizeForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ColorizeForm));
            this.btnReset = new System.Windows.Forms.Button();
            this.chkPreview = new System.Windows.Forms.CheckBox();
            this.chkColorize = new System.Windows.Forms.CheckBox();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.gbxColorize = new System.Windows.Forms.GroupBox();
            this.lblWeight = new System.Windows.Forms.Label();
            this.ltnWeight = new Controls.LinkedTrackBar();
            this.ntbWeight = new Controls.IntegerTextBox();
            this.lblLightness = new System.Windows.Forms.Label();
            this.ltbLightness = new Controls.LinkedTrackBar();
            this.ntbLightness = new Controls.IntegerTextBox();
            this.lblSaturation = new System.Windows.Forms.Label();
            this.ltbSaturation = new Controls.LinkedTrackBar();
            this.ntbSaturation = new Controls.IntegerTextBox();
            this.lblHue = new System.Windows.Forms.Label();
            this.ltbHue = new Controls.LinkedTrackBar();
            this.ntbHue = new Controls.IntegerTextBox();
            this.chkLuma = new System.Windows.Forms.CheckBox();
            this.gbxColorize.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ltnWeight)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ltbLightness)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ltbSaturation)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ltbHue)).BeginInit();
            this.SuspendLayout();
            // 
            // btnReset
            // 
            resources.ApplyResources(this.btnReset, "btnReset");
            this.btnReset.Name = "btnReset";
            this.btnReset.UseVisualStyleBackColor = true;
            this.btnReset.Click += new System.EventHandler(this.Reset_Click);
            // 
            // chkPreview
            // 
            resources.ApplyResources(this.chkPreview, "chkPreview");
            this.chkPreview.Checked = true;
            this.chkPreview.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkPreview.Name = "chkPreview";
            this.chkPreview.UseVisualStyleBackColor = true;
            this.chkPreview.CheckedChanged += new System.EventHandler(this.Preview_CheckedChanged);
            // 
            // chkColorize
            // 
            resources.ApplyResources(this.chkColorize, "chkColorize");
            this.chkColorize.Name = "chkColorize";
            this.chkColorize.UseVisualStyleBackColor = true;
            this.chkColorize.CheckedChanged += new System.EventHandler(this.Colorize_CheckedChanged);
            // 
            // btnOK
            // 
            resources.ApplyResources(this.btnOK, "btnOK");
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Name = "btnOK";
            this.btnOK.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            resources.ApplyResources(this.btnCancel, "btnCancel");
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // gbxColorize
            // 
            resources.ApplyResources(this.gbxColorize, "gbxColorize");
            this.gbxColorize.Controls.Add(this.lblWeight);
            this.gbxColorize.Controls.Add(this.ltnWeight);
            this.gbxColorize.Controls.Add(this.ntbWeight);
            this.gbxColorize.Controls.Add(this.lblLightness);
            this.gbxColorize.Controls.Add(this.ltbLightness);
            this.gbxColorize.Controls.Add(this.ntbLightness);
            this.gbxColorize.Controls.Add(this.lblSaturation);
            this.gbxColorize.Controls.Add(this.ltbSaturation);
            this.gbxColorize.Controls.Add(this.ntbSaturation);
            this.gbxColorize.Controls.Add(this.lblHue);
            this.gbxColorize.Controls.Add(this.ltbHue);
            this.gbxColorize.Controls.Add(this.ntbHue);
            this.gbxColorize.Name = "gbxColorize";
            this.gbxColorize.TabStop = false;
            // 
            // lblWeight
            // 
            resources.ApplyResources(this.lblWeight, "lblWeight");
            this.lblWeight.Name = "lblWeight";
            // 
            // ltnWeight
            // 
            resources.ApplyResources(this.ltnWeight, "ltnWeight");
            this.ltnWeight.IntegerComponent = this.ntbWeight;
            this.ltnWeight.Maximum = 100;
            this.ltnWeight.Name = "ltnWeight";
            this.ltnWeight.TickFrequency = 5;
            this.ltnWeight.Value = 100;
            this.ltnWeight.ValueChanged += new System.EventHandler(this.TrackBar_ValueChanged);
            // 
            // ntbWeight
            // 
            resources.ApplyResources(this.ntbWeight, "ntbWeight");
            this.ntbWeight.Name = "ntbWeight";
            this.ntbWeight.Value = 100;
            // 
            // lblLightness
            // 
            resources.ApplyResources(this.lblLightness, "lblLightness");
            this.lblLightness.Name = "lblLightness";
            this.lblLightness.Tag = "Luma:";
            // 
            // ltbLightness
            // 
            resources.ApplyResources(this.ltbLightness, "ltbLightness");
            this.ltbLightness.IntegerComponent = this.ntbLightness;
            this.ltbLightness.LargeChange = 10;
            this.ltbLightness.Maximum = 100;
            this.ltbLightness.Minimum = -100;
            this.ltbLightness.Name = "ltbLightness";
            this.ltbLightness.TickFrequency = 10;
            this.ltbLightness.ValueChanged += new System.EventHandler(this.TrackBar_ValueChanged);
            // 
            // ntbLightness
            // 
            this.ntbLightness.AllowNegative = true;
            resources.ApplyResources(this.ntbLightness, "ntbLightness");
            this.ntbLightness.Name = "ntbLightness";
            // 
            // lblSaturation
            // 
            resources.ApplyResources(this.lblSaturation, "lblSaturation");
            this.lblSaturation.Name = "lblSaturation";
            this.lblSaturation.Tag = "Chroma:";
            // 
            // ltbSaturation
            // 
            resources.ApplyResources(this.ltbSaturation, "ltbSaturation");
            this.ltbSaturation.IntegerComponent = this.ntbSaturation;
            this.ltbSaturation.LargeChange = 10;
            this.ltbSaturation.Maximum = 100;
            this.ltbSaturation.Minimum = -100;
            this.ltbSaturation.Name = "ltbSaturation";
            this.ltbSaturation.TickFrequency = 10;
            this.ltbSaturation.ValueChanged += new System.EventHandler(this.TrackBar_ValueChanged);
            // 
            // ntbSaturation
            // 
            this.ntbSaturation.AllowNegative = true;
            resources.ApplyResources(this.ntbSaturation, "ntbSaturation");
            this.ntbSaturation.Name = "ntbSaturation";
            // 
            // lblHue
            // 
            resources.ApplyResources(this.lblHue, "lblHue");
            this.lblHue.Name = "lblHue";
            // 
            // ltbHue
            // 
            resources.ApplyResources(this.ltbHue, "ltbHue");
            this.ltbHue.IntegerComponent = this.ntbHue;
            this.ltbHue.LargeChange = 18;
            this.ltbHue.Maximum = 180;
            this.ltbHue.Minimum = -180;
            this.ltbHue.Name = "ltbHue";
            this.ltbHue.TickFrequency = 18;
            this.ltbHue.ValueChanged += new System.EventHandler(this.TrackBar_ValueChanged);
            // 
            // ntbHue
            // 
            this.ntbHue.AllowNegative = true;
            resources.ApplyResources(this.ntbHue, "ntbHue");
            this.ntbHue.Name = "ntbHue";
            // 
            // chkLuma
            // 
            resources.ApplyResources(this.chkLuma, "chkLuma");
            this.chkLuma.Name = "chkLuma";
            this.chkLuma.UseVisualStyleBackColor = true;
            this.chkLuma.CheckedChanged += new System.EventHandler(this.Luma_CheckedChanged);
            // 
            // ColorizeForm
            // 
            this.AcceptButton = this.btnOK;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.Controls.Add(this.chkLuma);
            this.Controls.Add(this.gbxColorize);
            this.Controls.Add(this.btnReset);
            this.Controls.Add(this.chkPreview);
            this.Controls.Add(this.chkColorize);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btnCancel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ColorizeForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Shown += new System.EventHandler(this.ColorizeForm_Shown);
            this.gbxColorize.ResumeLayout(false);
            this.gbxColorize.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ltnWeight)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ltbLightness)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ltbSaturation)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ltbHue)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Controls.LinkedTrackBar ltbHue;
        private Controls.IntegerTextBox ntbHue;
        private System.Windows.Forms.Button btnReset;
        private System.Windows.Forms.CheckBox chkPreview;
        private System.Windows.Forms.CheckBox chkColorize;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.GroupBox gbxColorize;
        private System.Windows.Forms.Label lblHue;
        private System.Windows.Forms.Label lblWeight;
        private Controls.LinkedTrackBar ltnWeight;
        private Controls.IntegerTextBox ntbWeight;
        private System.Windows.Forms.Label lblLightness;
        private Controls.LinkedTrackBar ltbLightness;
        private Controls.IntegerTextBox ntbLightness;
        private System.Windows.Forms.Label lblSaturation;
        private Controls.LinkedTrackBar ltbSaturation;
        private Controls.IntegerTextBox ntbSaturation;
        private System.Windows.Forms.CheckBox chkLuma;
    }
}