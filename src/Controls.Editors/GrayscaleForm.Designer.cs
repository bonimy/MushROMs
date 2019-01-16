// <copyright file="GrayscaleForm.Designer.cs" company="Public Domain">
//     Copyright (c) 2018 Nelson Garcia.
// </copyright>

namespace Controls.Editors
{
    partial class GrayscaleForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GrayscaleForm));
            this.gbxGrayscale = new System.Windows.Forms.GroupBox();
            this.lblBlue = new System.Windows.Forms.Label();
            this.ltbBlue = new Controls.LinkedTrackBar();
            this.ntbBlue = new Controls.IntegerTextBox();
            this.lblGreen = new System.Windows.Forms.Label();
            this.ltbGreen = new Controls.LinkedTrackBar();
            this.ntbGreen = new Controls.IntegerTextBox();
            this.lblRed = new System.Windows.Forms.Label();
            this.ltbRed = new Controls.LinkedTrackBar();
            this.ntbRed = new Controls.IntegerTextBox();
            this.chkPreview = new System.Windows.Forms.CheckBox();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnEven = new System.Windows.Forms.Button();
            this.btnLuma = new System.Windows.Forms.Button();
            this.gbxGrayscale.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ltbBlue)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ltbGreen)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ltbRed)).BeginInit();
            this.SuspendLayout();
            // 
            // gbxGrayscale
            // 
            resources.ApplyResources(this.gbxGrayscale, "gbxGrayscale");
            this.gbxGrayscale.Controls.Add(this.lblBlue);
            this.gbxGrayscale.Controls.Add(this.ltbBlue);
            this.gbxGrayscale.Controls.Add(this.ntbBlue);
            this.gbxGrayscale.Controls.Add(this.lblGreen);
            this.gbxGrayscale.Controls.Add(this.ltbGreen);
            this.gbxGrayscale.Controls.Add(this.ntbGreen);
            this.gbxGrayscale.Controls.Add(this.lblRed);
            this.gbxGrayscale.Controls.Add(this.ltbRed);
            this.gbxGrayscale.Controls.Add(this.ntbRed);
            this.gbxGrayscale.Name = "gbxGrayscale";
            this.gbxGrayscale.TabStop = false;
            // 
            // lblBlue
            // 
            resources.ApplyResources(this.lblBlue, "lblBlue");
            this.lblBlue.Name = "lblBlue";
            // 
            // ltbBlue
            // 
            resources.ApplyResources(this.ltbBlue, "ltbBlue");
            this.ltbBlue.IntegerComponent = this.ntbBlue;
            this.ltbBlue.Maximum = 255;
            this.ltbBlue.Name = "ltbBlue";
            this.ltbBlue.TickFrequency = 16;
            this.ltbBlue.Value = 255;
            this.ltbBlue.Scroll += new System.EventHandler(this.Color_ValueChanged);
            this.ltbBlue.ValueChanged += new System.EventHandler(this.Color_ValueChanged);
            // 
            // ntbBlue
            // 
            this.ntbBlue.AllowNegative = true;
            resources.ApplyResources(this.ntbBlue, "ntbBlue");
            this.ntbBlue.Name = "ntbBlue";
            this.ntbBlue.Value = 255;
            // 
            // lblGreen
            // 
            resources.ApplyResources(this.lblGreen, "lblGreen");
            this.lblGreen.Name = "lblGreen";
            // 
            // ltbGreen
            // 
            resources.ApplyResources(this.ltbGreen, "ltbGreen");
            this.ltbGreen.IntegerComponent = this.ntbGreen;
            this.ltbGreen.LargeChange = 16;
            this.ltbGreen.Maximum = 255;
            this.ltbGreen.Name = "ltbGreen";
            this.ltbGreen.TickFrequency = 16;
            this.ltbGreen.Value = 255;
            this.ltbGreen.Scroll += new System.EventHandler(this.Color_ValueChanged);
            this.ltbGreen.ValueChanged += new System.EventHandler(this.Color_ValueChanged);
            // 
            // ntbGreen
            // 
            this.ntbGreen.AllowNegative = true;
            resources.ApplyResources(this.ntbGreen, "ntbGreen");
            this.ntbGreen.Name = "ntbGreen";
            this.ntbGreen.Value = 255;
            // 
            // lblRed
            // 
            resources.ApplyResources(this.lblRed, "lblRed");
            this.lblRed.Name = "lblRed";
            // 
            // ltbRed
            // 
            resources.ApplyResources(this.ltbRed, "ltbRed");
            this.ltbRed.IntegerComponent = this.ntbRed;
            this.ltbRed.LargeChange = 16;
            this.ltbRed.Maximum = 255;
            this.ltbRed.Name = "ltbRed";
            this.ltbRed.TickFrequency = 16;
            this.ltbRed.Value = 255;
            this.ltbRed.Scroll += new System.EventHandler(this.Color_ValueChanged);
            this.ltbRed.ValueChanged += new System.EventHandler(this.Color_ValueChanged);
            // 
            // ntbRed
            // 
            this.ntbRed.AllowNegative = true;
            resources.ApplyResources(this.ntbRed, "ntbRed");
            this.ntbRed.Name = "ntbRed";
            this.ntbRed.Value = 255;
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
            // btnCancel
            // 
            resources.ApplyResources(this.btnCancel, "btnCancel");
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnOK
            // 
            resources.ApplyResources(this.btnOK, "btnOK");
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Name = "btnOK";
            this.btnOK.UseVisualStyleBackColor = true;
            // 
            // btnEven
            // 
            resources.ApplyResources(this.btnEven, "btnEven");
            this.btnEven.Name = "btnEven";
            this.btnEven.UseVisualStyleBackColor = true;
            // 
            // btnLuma
            // 
            resources.ApplyResources(this.btnLuma, "btnLuma");
            this.btnLuma.Name = "btnLuma";
            this.btnLuma.UseVisualStyleBackColor = true;
            // 
            // GrayscaleForm
            // 
            this.AcceptButton = this.btnOK;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.Controls.Add(this.btnEven);
            this.Controls.Add(this.btnLuma);
            this.Controls.Add(this.gbxGrayscale);
            this.Controls.Add(this.chkPreview);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "GrayscaleForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Shown += new System.EventHandler(this.Form_Shown);
            this.gbxGrayscale.ResumeLayout(false);
            this.gbxGrayscale.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ltbBlue)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ltbGreen)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ltbRed)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox gbxGrayscale;
        private System.Windows.Forms.CheckBox chkPreview;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;
        private Controls.LinkedTrackBar ltbGreen;
        private Controls.IntegerTextBox ntbGreen;
        private Controls.LinkedTrackBar ltbRed;
        private Controls.IntegerTextBox ntbRed;
        private Controls.LinkedTrackBar ltbBlue;
        private Controls.IntegerTextBox ntbBlue;
        private System.Windows.Forms.Label lblGreen;
        private System.Windows.Forms.Label lblRed;
        private System.Windows.Forms.Label lblBlue;
        private System.Windows.Forms.Button btnEven;
        private System.Windows.Forms.Button btnLuma;
    }
}