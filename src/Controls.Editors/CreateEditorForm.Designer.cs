// <copyright file="CreateEditorForm.Designer.cs" company="Public Domain">
//     Copyright (c) 2018 Nelson Garcia.
// </copyright>

namespace Controls.Editors
{
    partial class CreateEditorForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CreateEditorForm));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.lblDescription = new System.Windows.Forms.Label();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOpen = new System.Windows.Forms.Button();
            this.dgvNewFileList = new Controls.BufferedDataGridView();
            this.dgvIcon = new System.Windows.Forms.DataGridViewImageColumn();
            this.dgvName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pnlOptions = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.dgvNewFileList)).BeginInit();
            this.SuspendLayout();
            // 
            // lblDescription
            // 
            resources.ApplyResources(this.lblDescription, "lblDescription");
            this.lblDescription.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.lblDescription.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblDescription.Name = "lblDescription";
            // 
            // btnCancel
            // 
            resources.ApplyResources(this.btnCancel, "btnCancel");
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnOpen
            // 
            resources.ApplyResources(this.btnOpen, "btnOpen");
            this.btnOpen.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOpen.Name = "btnOpen";
            this.btnOpen.UseVisualStyleBackColor = true;
            // 
            // dgvNewFileList
            // 
            this.dgvNewFileList.AllowUserToAddRows = false;
            this.dgvNewFileList.AllowUserToDeleteRows = false;
            this.dgvNewFileList.AllowUserToResizeColumns = false;
            this.dgvNewFileList.AllowUserToResizeRows = false;
            resources.ApplyResources(this.dgvNewFileList, "dgvNewFileList");
            this.dgvNewFileList.BackgroundColor = System.Drawing.SystemColors.ControlLightLight;
            this.dgvNewFileList.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.dgvNewFileList.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.Disable;
            this.dgvNewFileList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvNewFileList.ColumnHeadersVisible = false;
            this.dgvNewFileList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dgvIcon,
            this.dgvName,
            this.dgvType});
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvNewFileList.DefaultCellStyle = dataGridViewCellStyle3;
            this.dgvNewFileList.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dgvNewFileList.MultiSelect = false;
            this.dgvNewFileList.Name = "dgvNewFileList";
            this.dgvNewFileList.ReadOnly = true;
            this.dgvNewFileList.RowHeadersVisible = false;
            this.dgvNewFileList.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dgvNewFileList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvNewFileList.CellMouseEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.NewFileList_CellMouseEnter);
            this.dgvNewFileList.CellMouseLeave += new System.Windows.Forms.DataGridViewCellEventHandler(this.NewFileList_CellMouseLeave);
            this.dgvNewFileList.CurrentCellChanged += new System.EventHandler(this.CurrentCellChanged);
            // 
            // dgvIcon
            // 
            resources.ApplyResources(this.dgvIcon, "dgvIcon");
            this.dgvIcon.Name = "dgvIcon";
            this.dgvIcon.ReadOnly = true;
            // 
            // dgvName
            // 
            this.dgvName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            this.dgvName.DefaultCellStyle = dataGridViewCellStyle1;
            resources.ApplyResources(this.dgvName, "dgvName");
            this.dgvName.Name = "dgvName";
            this.dgvName.ReadOnly = true;
            // 
            // dgvType
            // 
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.dgvType.DefaultCellStyle = dataGridViewCellStyle2;
            resources.ApplyResources(this.dgvType, "dgvType");
            this.dgvType.Name = "dgvType";
            this.dgvType.ReadOnly = true;
            // 
            // pnlOptions
            // 
            resources.ApplyResources(this.pnlOptions, "pnlOptions");
            this.pnlOptions.BackColor = System.Drawing.SystemColors.Info;
            this.pnlOptions.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlOptions.Name = "pnlOptions";
            this.pnlOptions.ControlAdded += new System.Windows.Forms.ControlEventHandler(this.Options_ControlAdded);
            // 
            // CreateEditorForm
            // 
            this.AcceptButton = this.btnOpen;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.Controls.Add(this.pnlOptions);
            this.Controls.Add(this.btnOpen);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.lblDescription);
            this.Controls.Add(this.dgvNewFileList);
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "CreateEditorForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.TopMost = true;
            ((System.ComponentModel.ISupportInitialize)(this.dgvNewFileList)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Controls.BufferedDataGridView dgvNewFileList;
        private System.Windows.Forms.DataGridViewImageColumn dgvIcon;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvName;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvType;
        private System.Windows.Forms.Label lblDescription;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOpen;
        private System.Windows.Forms.Panel pnlOptions;
    }
}