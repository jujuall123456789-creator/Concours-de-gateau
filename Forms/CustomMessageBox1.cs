using DuelDeGateaux.Repositories;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace DuelDeGateaux.Forms
{
    /// <summary>
    /// Remplace le MessageBox standard de Windows pour éviter les sons systèmes
    /// et appliquer nos curseurs personnalisés.
    /// </summary>
    public class CustomMessageBox : Form
    {
        private DialogResult _result = DialogResult.None;

        // Constructeur privé : on passera par la méthode statique Show()
        private CustomMessageBox(string message, string title, MessageBoxButtons buttons, MessageBoxIcon icon)
        {
            // --- Configuration de la fenêtre ---
            this.Text = title;
            this.Size = new Size(450, 220);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.StartPosition = FormStartPosition.CenterParent;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.BackColor = Color.White;

            // 🪄 Application du curseur Rouleau à pâtisserie sur le fond
            this.Cursor = CursorService.LoadCustomCursor() ?? Cursors.Default;

            // --- Construction de l'interface ---
            Panel iconPanel = new Panel { Dock = DockStyle.Left, Width = 80, Padding = new Padding(20) };
            PictureBox picIcon = new PictureBox { Size = new Size(40, 40), SizeMode = PictureBoxSizeMode.Zoom };
            
            // Récupération de l'icône système SANS déclencher le son !
            switch (icon)
            {
                case MessageBoxIcon.Warning: picIcon.Image = SystemIcons.Warning.ToBitmap(); break;
                case MessageBoxIcon.Error: picIcon.Image = SystemIcons.Error.ToBitmap(); break;
                case MessageBoxIcon.Information: picIcon.Image = SystemIcons.Information.ToBitmap(); break;
                case MessageBoxIcon.Question: picIcon.Image = SystemIcons.Question.ToBitmap(); break;
            }
            iconPanel.Controls.Add(picIcon);

            Label lblMessage = new Label
            {
                Text = message,
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleLeft,
                Font = new Font("Segoe UI", 10),
                Padding = new Padding(10)
            };

            FlowLayoutPanel buttonPanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Bottom,
                Height = 50,
                FlowDirection = FlowDirection.RightToLeft,
                Padding = new Padding(10),
                BackColor = Color.WhiteSmoke
            };

            // --- Ajout des boutons selon le choix ---
            Cursor buttonCursor = CursorService.LoadCustomButtonCursor() ?? Cursors.Hand;

            if (buttons == MessageBoxButtons.YesNo || buttons == MessageBoxButtons.YesNoCancel)
            {
                Button btnNo = CreateButton("Non", DialogResult.No, buttonCursor);
                Button btnYes = CreateButton("Oui", DialogResult.Yes, buttonCursor);
                buttonPanel.Controls.Add(btnNo);
                buttonPanel.Controls.Add(btnYes);
                this.CancelButton = btnNo;
            }
            else // Par défaut, on met juste un bouton OK
            {
                Button btnOk = CreateButton("OK", DialogResult.OK, buttonCursor);
                buttonPanel.Controls.Add(btnOk);
                this.AcceptButton = btnOk;
                this.CancelButton = btnOk;
            }

            // Assemblage
            this.Controls.Add(lblMessage);
            this.Controls.Add(iconPanel);
            this.Controls.Add(buttonPanel);
        }

        private Button CreateButton(string text, DialogResult dialogResult, Cursor cursor)
        {
            Button btn = new Button
            {
                Text = text,
                Width = 100,
                Height = 35,
                Margin = new Padding(10, 0, 0, 0),
                Cursor = cursor, // 👆 Le fameux curseur Muffin !
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.White
            };
            btn.FlatAppearance.BorderColor = Color.LightGray;
            
            btn.Click += (s, e) =>
            {
                _result = dialogResult;
                this.Close();
            };
            return btn;
        }

        /// <summary>
        /// Charge l'icône demandée, avec notre exception pour le gâteau brûlé !
        /// </summary>
        private void LoadCustomIcon(MessageBoxIcon icon)
        {
            if (icon == MessageBoxIcon.Warning)
            {
                // 🎂 On tente de charger notre image de gâteau brûlé
                string burntCakePath = FileSelectionService.FilePathAssets("forbiddenCake.png");
                
                if (File.Exists(burntCakePath))
                {
                    picIcon.Image = Image.FromFile(burntCakePath);
                }
                else
                {
                    // Secours si l'image a disparu
                    picIcon.Image = SystemIcons.Warning.ToBitmap();
                }
            }
            else if (icon == MessageBoxIcon.Error) picIcon.Image = SystemIcons.Error.ToBitmap();
            else if (icon == MessageBoxIcon.Information) picIcon.Image = SystemIcons.Information.ToBitmap();
            else if (icon == MessageBoxIcon.Question) picIcon.Image = SystemIcons.Question.ToBitmap();
            else picIcon.Visible = false; // Pas d'icône du tout
        }

        /// <summary>
        /// Affiche la boîte de dialogue personnalisée (Drop-in replacement pour MessageBox.Show)
        /// </summary>
        public static DialogResult Show(string message, string title = "", MessageBoxButtons buttons = MessageBoxButtons.OK, MessageBoxIcon icon = MessageBoxIcon.None)
        {
            using (var customBox = new CustomMessageBox(message, title, buttons, icon))
            {
                customBox.ShowDialog();
                return customBox._result;
            }
        }
    }
}