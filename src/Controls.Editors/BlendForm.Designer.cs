// <copyright file="BlendForm.Designer.cs" company="Public Domain">
//     Copyright (c) 2018 Nelson Garcia.
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BlendForm));
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
            resources.ApplyResources(this.gbxColor, "gbxColor");
            this.gbxColor.Controls.Add(this.lblBlue);
            this.gbxColor.Controls.Add(this.ltbBlue);
            this.gbxColor.Controls.Add(this.ntbBlue);
            this.gbxColor.Controls.Add(this.lblGreen);
            this.gbxColor.Controls.Add(this.ltbGreen);
            this.gbxColor.Controls.Add(this.ntbGreen);
            this.gbxColor.Controls.Add(this.lblRed);
            this.gbxColor.Controls.Add(this.ltbRed);
            this.gbxColor.Controls.Add(this.ntbRed);
            this.gbxColor.Name = "gbxColor";
            this.gbxColor.TabStop = false;
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
            this.ltbBlue.Scroll += new System.EventHandler(this.Object_ValueChanged);
            this.ltbBlue.ValueChanged += new System.EventHandler(this.Object_ValueChanged);
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
            this.ltbGreen.Scroll += new System.EventHandler(this.Object_ValueChanged);
            this.ltbGreen.ValueChanged += new System.EventHandler(this.Object_ValueChanged);
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
            this.ltbRed.Scroll += new System.EventHandler(this.Object_ValueChanged);
            this.ltbRed.ValueChanged += new System.EventHandler(this.Object_ValueChanged);
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
            this.chkPreview.CheckedChanged += new System.EventHandler(this.Object_ValueChanged);
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
            // gbxBlendMode
            // 
            this.gbxBlendMode.Controls.Add(this.cbxBlendMode);
            resources.ApplyResources(this.gbxBlendMode, "gbxBlendMode");
            this.gbxBlendMode.Name = "gbxBlendMode";
            this.gbxBlendMode.TabStop = false;
            // 
            // cbxBlendMode
            // 
            resources.ApplyResources(this.cbxBlendMode, "cbxBlendMode");
            this.cbxBlendMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxBlendMode.FormattingEnabled = true;
            this.cbxBlendMode.Items.AddRange(new object[] {
            resources.GetString("cbxBlendMode.Items"),
            resources.GetString("cbxBlendMode.Items1"),
            resources.GetString("cbxBlendMode.Items2"),
            resources.GetString("cbxBlendMode.Items3"),
            resources.GetString("cbxBlendMode.Items4"),
            resources.GetString("cbxBlendMode.Items5"),
            resources.GetString("cbxBlendMode.Items6"),
            resources.GetString("cbxBlendMode.Items7"),
            resources.GetString("cbxBlendMode.Items8"),
            resources.GetString("cbxBlendMode.Items9"),
            resources.GetString("cbxBlendMode.Items10"),
            resources.GetString("cbxBlendMode.Items11"),
            resources.GetString("cbxBlendMode.Items12"),
            resources.GetString("cbxBlendMode.Items13"),
            resources.GetString("cbxBlendMode.Items14"),
            resources.GetString("cbxBlendMode.Items15"),
            resources.GetString("cbxBlendMode.Items16"),
            resources.GetString("cbxBlendMode.Items17"),
            resources.GetString("cbxBlendMode.Items18"),
            resources.GetString("cbxBlendMode.Items19"),
            resources.GetString("cbxBlendMode.Items20")});
            this.cbxBlendMode.Name = "cbxBlendMode";
            this.cbxBlendMode.SelectedIndexChanged += new System.EventHandler(this.Object_ValueChanged);
            // 
            // BlendForm
            // 
            this.AcceptButton = this.btnOK;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
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
            this.Shown += new System.EventHandler(this.Object_ValueChanged);
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