// <copyright file="BlendForm.Designer.cs" company="Public Domain">
//     Copyright (c) 2017 Nelson Garcia.
// </copyright>

namespace Controls.Editors
{
    partial class BlendForm
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
            this.gbxColor = new System.Windows.Forms.GroupBox();
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
            this.gbxBlendMode = new System.Windows.Forms.GroupBox();
            this.cbxBlendMode = new System.Windows.Forms.ComboBox();
            this.gbxColor.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ltbBlue)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ltbGreen)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ltbRed)).BeginInit();
            this.gbxBlendMode.SuspendLayout();
            this.SuspendLayout();
            //
            // gbxColor
            //
            this.gbxColor.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gbxColor.Controls.Add(this.lblBlue);
            this.gbxColor.Controls.Add(this.ltbBlue);
            this.gbxColor.Controls.Add(this.ntbBlue);
            this.gbxColor.Controls.Add(this.lblGreen);
            this.gbxColor.Controls.Add(this.ltbGreen);
            this.gbxColor.Controls.Add(this.ntbGreen);
            this.gbxColor.Controls.Add(this.lblRed);
            this.gbxColor.Controls.Add(this.ltbRed);
            this.gbxColor.Controls.Add(this.ntbRed);
            this.gbxColor.Location = new System.Drawing.Point(12, 12);
            this.gbxColor.Name = "gbxColor";
            this.gbxColor.Size = new System.Drawing.Size(403, 211);
            this.gbxColor.TabIndex = 0;
            this.gbxColor.TabStop = false;
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
            this.ltbBlue.Size = new System.Drawing.Size(345, 45);
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
            this.ntbBlue.Location = new System.Drawing.Point(357, 160);
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
            this.ltbGreen.Size = new System.Drawing.Size(345, 45);
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
            this.ntbGreen.Location = new System.Drawing.Point(357, 96);
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
            this.ltbRed.Size = new System.Drawing.Size(345, 45);
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
            this.ntbRed.Location = new System.Drawing.Point(357, 32);
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
            this.chkPreview.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.chkPreview.AutoSize = true;
            this.chkPreview.Checked = true;
            this.chkPreview.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkPreview.Location = new System.Drawing.Point(189, 250);
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
            this.btnCancel.Location = new System.Drawing.Point(340, 246);
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
            this.btnOK.Location = new System.Drawing.Point(259, 246);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 2;
            this.btnOK.Text = "&OK";
            this.btnOK.UseVisualStyleBackColor = true;
            //
            // gbxBlendMode
            //
            this.gbxBlendMode.Controls.Add(this.cbxBlendMode);
            this.gbxBlendMode.Location = new System.Drawing.Point(12, 229);
            this.gbxBlendMode.Name = "gbxBlendMode";
            this.gbxBlendMode.Size = new System.Drawing.Size(168, 46);
            this.gbxBlendMode.TabIndex = 7;
            this.gbxBlendMode.TabStop = false;
            this.gbxBlendMode.Text = "Blend Mode";
            //
            // cbxBlendMode
            //
            this.cbxBlendMode.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cbxBlendMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxBlendMode.FormattingEnabled = true;
            this.cbxBlendMode.Items.AddRange(new object[] {
            "Grayscale",
            "Multiply",
            "Screen",
            "Overlay",
            "Hard Light",
            "Soft Light",
            "Color Dodge",
            "Linear Dodge",
            "Color Burn",
            "Linear Burn",
            "Vivid Light",
            "Linear Light",
            "Difference",
            "Darken",
            "Lighten",
            "Darker Color",
            "Lighter Color",
            "Hue",
            "Saturation",
            "Luminosity",
            "Divide"});
            this.cbxBlendMode.Location = new System.Drawing.Point(6, 19);
            this.cbxBlendMode.Name = "cbxBlendMode";
            this.cbxBlendMode.Size = new System.Drawing.Size(156, 21);
            this.cbxBlendMode.TabIndex = 0;
            this.cbxBlendMode.SelectedIndexChanged += new System.EventHandler(this.BlendMode_SelectedIndexChanged);
            //
            // BlendForm
            //
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(427, 287);
            this.Controls.Add(this.gbxBlendMode);
            this.Controls.Add(this.gbxColor);
            this.Controls.Add(this.chkPreview);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "BlendForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "RGBForm";
            this.Shown += new System.EventHandler(this.Form_Shown);
            this.gbxColor.ResumeLayout(false);
            this.gbxColor.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ltbBlue)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ltbGreen)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ltbRed)).EndInit();
            this.gbxBlendMode.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox gbxColor;
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
        private System.Windows.Forms.GroupBox gbxBlendMode;
        private System.Windows.Forms.ComboBox cbxBlendMode;
    }
}