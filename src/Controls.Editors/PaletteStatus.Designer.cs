// <copyright file="PaletteStatus.Designer.cs" company="Public Domain">
//     Copyright (c) 2018 Nelson Garcia.
// </copyright>

namespace Controls.Editors
{
    partial class PaletteStatus
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PaletteStatus));
            this.gbxSelectedColor = new System.Windows.Forms.GroupBox();
            this.lblPcValueText = new System.Windows.Forms.Label();
            this.lblPcValue = new System.Windows.Forms.Label();
            this.lblSnesValueText = new System.Windows.Forms.Label();
            this.lblSnesValue = new System.Windows.Forms.Label();
            this.lblGreenValue = new System.Windows.Forms.Label();
            this.lblRedValue = new System.Windows.Forms.Label();
            this.lblBlueValue = new System.Windows.Forms.Label();
            this.gbxZoom = new System.Windows.Forms.GroupBox();
            this.cbxZoom = new System.Windows.Forms.ComboBox();
            this.gbxROMViewing = new System.Windows.Forms.GroupBox();
            this.btnNextByte = new System.Windows.Forms.Button();
            this.btnLastByte = new System.Windows.Forms.Button();
            this.gbxSelectedColor.SuspendLayout();
            this.gbxZoom.SuspendLayout();
            this.gbxROMViewing.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbxSelectedColor
            // 
            this.gbxSelectedColor.Controls.Add(this.lblPcValueText);
            this.gbxSelectedColor.Controls.Add(this.lblPcValue);
            this.gbxSelectedColor.Controls.Add(this.lblSnesValueText);
            this.gbxSelectedColor.Controls.Add(this.lblSnesValue);
            this.gbxSelectedColor.Controls.Add(this.lblGreenValue);
            this.gbxSelectedColor.Controls.Add(this.lblRedValue);
            this.gbxSelectedColor.Controls.Add(this.lblBlueValue);
            resources.ApplyResources(this.gbxSelectedColor, "gbxSelectedColor");
            this.gbxSelectedColor.Name = "gbxSelectedColor";
            this.gbxSelectedColor.TabStop = false;
            // 
            // lblPcValueText
            // 
            resources.ApplyResources(this.lblPcValueText, "lblPcValueText");
            this.lblPcValueText.Name = "lblPcValueText";
            // 
            // lblPcValue
            // 
            resources.ApplyResources(this.lblPcValue, "lblPcValue");
            this.lblPcValue.Name = "lblPcValue";
            // 
            // lblSnesValueText
            // 
            resources.ApplyResources(this.lblSnesValueText, "lblSnesValueText");
            this.lblSnesValueText.Name = "lblSnesValueText";
            // 
            // lblSnesValue
            // 
            resources.ApplyResources(this.lblSnesValue, "lblSnesValue");
            this.lblSnesValue.Name = "lblSnesValue";
            // 
            // lblGreenValue
            // 
            resources.ApplyResources(this.lblGreenValue, "lblGreenValue");
            this.lblGreenValue.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.lblGreenValue.Name = "lblGreenValue";
            // 
            // lblRedValue
            // 
            resources.ApplyResources(this.lblRedValue, "lblRedValue");
            this.lblRedValue.ForeColor = System.Drawing.Color.Red;
            this.lblRedValue.Name = "lblRedValue";
            // 
            // lblBlueValue
            // 
            resources.ApplyResources(this.lblBlueValue, "lblBlueValue");
            this.lblBlueValue.ForeColor = System.Drawing.Color.Blue;
            this.lblBlueValue.Name = "lblBlueValue";
            // 
            // gbxZoom
            // 
            this.gbxZoom.Controls.Add(this.cbxZoom);
            resources.ApplyResources(this.gbxZoom, "gbxZoom");
            this.gbxZoom.Name = "gbxZoom";
            this.gbxZoom.TabStop = false;
            // 
            // cbxZoom
            // 
            this.cbxZoom.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            resources.ApplyResources(this.cbxZoom, "cbxZoom");
            this.cbxZoom.FormattingEnabled = true;
            this.cbxZoom.Items.AddRange(new object[] {
            resources.GetString("cbxZoom.Items"),
            resources.GetString("cbxZoom.Items1"),
            resources.GetString("cbxZoom.Items2"),
            resources.GetString("cbxZoom.Items3")});
            this.cbxZoom.Name = "cbxZoom";
            // 
            // gbxROMViewing
            // 
            this.gbxROMViewing.Controls.Add(this.btnNextByte);
            this.gbxROMViewing.Controls.Add(this.btnLastByte);
            resources.ApplyResources(this.gbxROMViewing, "gbxROMViewing");
            this.gbxROMViewing.Name = "gbxROMViewing";
            this.gbxROMViewing.TabStop = false;
            // 
            // btnNextByte
            // 
            resources.ApplyResources(this.btnNextByte, "btnNextByte");
            this.btnNextByte.Name = "btnNextByte";
            this.btnNextByte.UseVisualStyleBackColor = true;
            // 
            // btnLastByte
            // 
            resources.ApplyResources(this.btnLastByte, "btnLastByte");
            this.btnLastByte.Name = "btnLastByte";
            this.btnLastByte.UseVisualStyleBackColor = true;
            // 
            // PaletteStatus
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.gbxSelectedColor);
            this.Controls.Add(this.gbxZoom);
            this.Controls.Add(this.gbxROMViewing);
            this.Name = "PaletteStatus";
            this.gbxSelectedColor.ResumeLayout(false);
            this.gbxSelectedColor.PerformLayout();
            this.gbxZoom.ResumeLayout(false);
            this.gbxROMViewing.ResumeLayout(false);
            this.gbxROMViewing.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gbxSelectedColor;
        private System.Windows.Forms.Label lblPcValueText;
        private System.Windows.Forms.Label lblPcValue;
        private System.Windows.Forms.Label lblSnesValueText;
        private System.Windows.Forms.Label lblSnesValue;
        private System.Windows.Forms.Label lblGreenValue;
        private System.Windows.Forms.Label lblRedValue;
        private System.Windows.Forms.Label lblBlueValue;
        private System.Windows.Forms.GroupBox gbxZoom;
        private System.Windows.Forms.ComboBox cbxZoom;
        private System.Windows.Forms.GroupBox gbxROMViewing;
        private System.Windows.Forms.Button btnNextByte;
        private System.Windows.Forms.Button btnLastByte;
    }
}
