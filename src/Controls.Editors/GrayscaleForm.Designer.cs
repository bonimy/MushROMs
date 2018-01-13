// <copyright file="GrayscaleForm.Designer.cs" company="Public Domain">
//     Copyright (c) 2017 Nelson Garcia.
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
            this.gbxGrayscale.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gbxGrayscale.Controls.Add(this.lblBlue);
            this.gbxGrayscale.Controls.Add(this.ltbBlue);
            this.gbxGrayscale.Controls.Add(this.ntbBlue);
            this.gbxGrayscale.Controls.Add(this.lblGreen);
            this.gbxGrayscale.Controls.Add(this.ltbGreen);
            this.gbxGrayscale.Controls.Add(this.ntbGreen);
            this.gbxGrayscale.Controls.Add(this.lblRed);
            this.gbxGrayscale.Controls.Add(this.ltbRed);
            this.gbxGrayscale.Controls.Add(this.ntbRed);
            this.gbxGrayscale.Location = new System.Drawing.Point(12, 12);
            this.gbxGrayscale.Name = "gbxGrayscale";
            this.gbxGrayscale.Size = new System.Drawing.Size(388, 211);
            this.gbxGrayscale.TabIndex = 0;
            this.gbxGrayscale.TabStop = false;
            //
            // lblBlue
            //
            this.lblBlue.AutoSize = true;
            this.lblBlue.Location = new System.Drawing.Point(6, 144);
            this.lblBlue.Name = "lblBlue";
            this.lblBlue.Size = new System.Drawing.Size(31, 13);
            this.lblBlue.TabIndex = 0;
            this.lblBlue.Text = "Blue:";
            //
            // ltbBlue
            //
            this.ltbBlue.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ltbBlue.IntegerComponent = this.ntbBlue;
            this.ltbBlue.Location = new System.Drawing.Point(6, 160);
            this.ltbBlue.Maximum = 255;
            this.ltbBlue.Name = "ltbBlue";
            this.ltbBlue.Size = new System.Drawing.Size(330, 45);
            this.ltbBlue.TabIndex = 4;
            this.ltbBlue.TickFrequency = 16;
            this.ltbBlue.Value = 255;
            this.ltbBlue.Scroll += new System.EventHandler(this.Color_ValueChanged);
            this.ltbBlue.ValueChanged += new System.EventHandler(this.Color_ValueChanged);
            //
            // ntbBlue
            //
            this.ntbBlue.AllowNegative = true;
            this.ntbBlue.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ntbBlue.Location = new System.Drawing.Point(342, 160);
            this.ntbBlue.MaxLength = 4;
            this.ntbBlue.Name = "ntbBlue";
            this.ntbBlue.Size = new System.Drawing.Size(40, 20);
            this.ntbBlue.TabIndex = 5;
            this.ntbBlue.Text = "255";
            this.ntbBlue.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.ntbBlue.Value = 255;
            //
            // lblGreen
            //
            this.lblGreen.AutoSize = true;
            this.lblGreen.Location = new System.Drawing.Point(6, 80);
            this.lblGreen.Name = "lblGreen";
            this.lblGreen.Size = new System.Drawing.Size(39, 13);
            this.lblGreen.TabIndex = 0;
            this.lblGreen.Text = "Green:";
            //
            // ltbGreen
            //
            this.ltbGreen.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ltbGreen.IntegerComponent = this.ntbGreen;
            this.ltbGreen.LargeChange = 16;
            this.ltbGreen.Location = new System.Drawing.Point(6, 96);
            this.ltbGreen.Maximum = 255;
            this.ltbGreen.Name = "ltbGreen";
            this.ltbGreen.Size = new System.Drawing.Size(330, 45);
            this.ltbGreen.TabIndex = 2;
            this.ltbGreen.TickFrequency = 16;
            this.ltbGreen.Value = 255;
            this.ltbGreen.Scroll += new System.EventHandler(this.Color_ValueChanged);
            this.ltbGreen.ValueChanged += new System.EventHandler(this.Color_ValueChanged);
            //
            // ntbGreen
            //
            this.ntbGreen.AllowNegative = true;
            this.ntbGreen.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ntbGreen.Location = new System.Drawing.Point(342, 96);
            this.ntbGreen.MaxLength = 4;
            this.ntbGreen.Name = "ntbGreen";
            this.ntbGreen.Size = new System.Drawing.Size(40, 20);
            this.ntbGreen.TabIndex = 3;
            this.ntbGreen.Text = "255";
            this.ntbGreen.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.ntbGreen.Value = 255;
            //
            // lblRed
            //
            this.lblRed.AutoSize = true;
            this.lblRed.Location = new System.Drawing.Point(6, 16);
            this.lblRed.Name = "lblRed";
            this.lblRed.Size = new System.Drawing.Size(30, 13);
            this.lblRed.TabIndex = 0;
            this.lblRed.Text = "Red:";
            //
            // ltbRed
            //
            this.ltbRed.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ltbRed.IntegerComponent = this.ntbRed;
            this.ltbRed.LargeChange = 16;
            this.ltbRed.Location = new System.Drawing.Point(6, 32);
            this.ltbRed.Maximum = 255;
            this.ltbRed.Name = "ltbRed";
            this.ltbRed.Size = new System.Drawing.Size(330, 45);
            this.ltbRed.TabIndex = 0;
            this.ltbRed.TickFrequency = 16;
            this.ltbRed.Value = 255;
            this.ltbRed.Scroll += new System.EventHandler(this.Color_ValueChanged);
            this.ltbRed.ValueChanged += new System.EventHandler(this.Color_ValueChanged);
            //
            // ntbRed
            //
            this.ntbRed.AllowNegative = true;
            this.ntbRed.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ntbRed.Location = new System.Drawing.Point(342, 32);
            this.ntbRed.MaxLength = 4;
            this.ntbRed.Name = "ntbRed";
            this.ntbRed.Size = new System.Drawing.Size(40, 20);
            this.ntbRed.TabIndex = 1;
            this.ntbRed.Text = "255";
            this.ntbRed.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.ntbRed.Value = 255;
            //
            // chkPreview
            //
            this.chkPreview.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.chkPreview.AutoSize = true;
            this.chkPreview.Checked = true;
            this.chkPreview.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkPreview.Location = new System.Drawing.Point(12, 233);
            this.chkPreview.Name = "chkPreview";
            this.chkPreview.Size = new System.Drawing.Size(64, 17);
            this.chkPreview.TabIndex = 1;
            this.chkPreview.Text = "Preview";
            this.chkPreview.UseVisualStyleBackColor = true;
            this.chkPreview.CheckedChanged += new System.EventHandler(this.Preview_CheckedChanged);
            //
            // btnCancel
            //
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(325, 229);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            //
            // btnOK
            //
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Location = new System.Drawing.Point(244, 229);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 2;
            this.btnOK.Text = "&OK";
            this.btnOK.UseVisualStyleBackColor = true;
            //
            // btnEven
            //
            this.btnEven.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnEven.Location = new System.Drawing.Point(82, 229);
            this.btnEven.Name = "btnEven";
            this.btnEven.Size = new System.Drawing.Size(75, 23);
            this.btnEven.TabIndex = 4;
            this.btnEven.Text = "&Even";
            this.btnEven.UseVisualStyleBackColor = true;
            //
            // btnLuma
            //
            this.btnLuma.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnLuma.Location = new System.Drawing.Point(163, 229);
            this.btnLuma.Name = "btnLuma";
            this.btnLuma.Size = new System.Drawing.Size(75, 23);
            this.btnLuma.TabIndex = 5;
            this.btnLuma.Text = "&Luma";
            this.btnLuma.UseVisualStyleBackColor = true;
            //
            // GrayscaleForm
            //
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(412, 264);
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
            this.Text = "Custom Grayscale";
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