namespace DuelDeGateaux.Forms
{
    partial class CustomMessageBox
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
            picIcon = new PictureBox();
            pnlButtons = new FlowLayoutPanel();
            rtbMessage = new RichTextBox();
            ((System.ComponentModel.ISupportInitialize)picIcon).BeginInit();
            SuspendLayout();
            // 
            // picIcon
            // 
            picIcon.BackColor = Color.OldLace;
            picIcon.Dock = DockStyle.Left;
            picIcon.Location = new Point(0, 0);
            picIcon.Name = "picIcon";
            picIcon.Padding = new Padding(10);
            picIcon.Size = new Size(80, 150);
            picIcon.SizeMode = PictureBoxSizeMode.Zoom;
            picIcon.TabIndex = 0;
            picIcon.TabStop = false;
            // 
            // pnlButtons
            // 
            pnlButtons.BackColor = Color.OldLace;
            pnlButtons.Dock = DockStyle.Bottom;
            pnlButtons.FlowDirection = FlowDirection.RightToLeft;
            pnlButtons.Location = new Point(80, 116);
            pnlButtons.Name = "pnlButtons";
            pnlButtons.Padding = new Padding(10, 0, 10, 0);
            pnlButtons.Size = new Size(404, 34);
            pnlButtons.TabIndex = 1;
            pnlButtons.Paint += pnlButtons_Paint;
            // 
            // rtbMessage
            // 
            rtbMessage.BackColor = Color.OldLace;
            rtbMessage.BorderStyle = BorderStyle.None;
            rtbMessage.Dock = DockStyle.Fill;
            rtbMessage.Font = new Font("Segoe UI Emoji", 10.25F);
            rtbMessage.Location = new Point(80, 0);
            rtbMessage.Margin = new Padding(3, 100, 3, 3);
            rtbMessage.Name = "rtbMessage";
            rtbMessage.ReadOnly = true;
            rtbMessage.Size = new Size(404, 116);
            rtbMessage.TabIndex = 2;
            rtbMessage.Text = "";
            rtbMessage.TextChanged += rtbMessage_TextChanged;
            // 
            // CustomMessageBox
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.OldLace;
            ClientSize = new Size(484, 150);
            ControlBox = false;
            Controls.Add(rtbMessage);
            Controls.Add(pnlButtons);
            Controls.Add(picIcon);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "CustomMessageBox";
            ShowIcon = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "CustomMessageBox";
            Load += CustomMessageBox_Load;
            ((System.ComponentModel.ISupportInitialize)picIcon).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private PictureBox picIcon;
        private FlowLayoutPanel pnlButtons;
        private RichTextBox rtbMessage;
    }
}