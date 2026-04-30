using DuelDeGateaux.Models;
using DuelDeGateaux.Services;

namespace DuelDeGateaux.Forms
{
    partial class HistoryForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// DataGridView pour le tableau
        /// </summary>
        private DataGridView dgvHistory;

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
            dgvHistory = new DataGridView();
            ((System.ComponentModel.ISupportInitialize)dgvHistory).BeginInit();
            SuspendLayout();
            // 
            // dgvHistory
            // 
            dgvHistory.Location = new Point(12, 12);
            dgvHistory.Name = "dgvHistory";
            dgvHistory.Size = new Size(760, 400);
            dgvHistory.TabIndex = 0;
            // 
            // HistoryForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 501);
            Controls.Add(dgvHistory);
            Name = "HistoryForm";
            ShowIcon = false;
            Text = "📖 Historique des manches";
            Load += HistoryForm_Load;
            ((System.ComponentModel.ISupportInitialize)dgvHistory).EndInit();
            ResumeLayout(false);
        }

        #endregion
    }
}