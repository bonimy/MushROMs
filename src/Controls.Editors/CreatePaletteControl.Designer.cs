// <copyright file="CreatePaletteControl.Designer.cs" company="Public Domain">
//     Copyright (c) 2018 Nelson Garcia.
// </copyright>

namespace Controls.Editors
{
    partial class CreatePaletteControl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CreatePaletteControl));
            this.gbxNumColors = new System.Windows.Forms.GroupBox();
            this.nudNumColors = new System.Windows.Forms.NumericUpDown();
            this.chkFromCopy = new System.Windows.Forms.CheckBox();
            this.gbxNumColors.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudNumColors)).BeginInit();
            this.SuspendLayout();
            // 
            // gbxNumColors
            // 
            resources.ApplyResources(this.gbxNumColors, "gbxNumColors");
            this.gbxNumColors.Controls.Add(this.nudNumColors);
            this.gbxNumColors.Name = "gbxNumColors";
            this.gbxNumColors.TabStop = false;
            // 
            // nudNumColors
            // 
            resources.ApplyResources(this.nudNumColors, "nudNumColors");
            this.nudNumColors.Hexadecimal = true;
            this.nudNumColors.Maximum = new decimal(new int[] {
            8388608,
            0,
            0,
            0});
            this.nudNumColors.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudNumColors.Name = "nudNumColors";
            this.nudNumColors.Value = new decimal(new int[] {
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
            // CreatePaletteControl
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.gbxNumColors);
            this.Controls.Add(this.chkFromCopy);
            this.Name = "CreatePaletteControl";
            this.gbxNumColors.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.nudNumColors)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox gbxNumColors;
        private System.Windows.Forms.NumericUpDown nudNumColors;
        private System.Windows.Forms.CheckBox chkFromCopy;
    }
}
