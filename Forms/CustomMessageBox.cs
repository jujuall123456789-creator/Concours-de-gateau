using DuelDeGateaux.Repositories;
using DuelDeGateaux.Services;
using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace DuelDeGateaux.Forms
{
    public partial class CustomMessageBox : Form
    {
        private DialogResult _result = DialogResult.None;

        // Le constructeur est privé, on passe par la méthode statique Show()
        private CustomMessageBox(string message, string title, MessageBoxButtons buttons, MessageBoxIcon icon)
        {
            // Initialise les composants dessinés dans le Designer
            InitializeComponent();

            this.Text = title;
            rtbMessage.Text = message;

            // 🪄 Application du curseur Rouleau à pâtisserie sur le fond
            this.Cursor = CursorService.LoadCustomCursor() ?? Cursors.Default;
            rtbMessage.Cursor = this.Cursor;
            LoadCustomIcon(icon);
            SetupButtons(buttons);
        }

        /// <summary>
        /// Charge l'icône demandée, avec notre exception pour le gâteau brûlé !
        /// </summary>
        private void LoadCustomIcon(MessageBoxIcon icon)
        {
            if (icon == MessageBoxIcon.Warning)
            {
                // 🎂 On tente de charger notre image de gâteau brûlé
                string forbiddenCakePath = FileSelectionService.FilePathAssets("forbiddenCake.png");

                if (File.Exists(forbiddenCakePath))
                {
                    picIcon.Image = Image.FromFile(forbiddenCakePath);
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
        /// Génère dynamiquement les boutons à l'intérieur du FlowLayoutPanel (pnlButtons)
        /// </summary>
        private void SetupButtons(MessageBoxButtons buttons)
        {
            Cursor buttonCursor = CursorService.LoadCustomButtonCursor() ?? Cursors.Hand;

            if (buttons == MessageBoxButtons.YesNo || buttons == MessageBoxButtons.YesNoCancel)
            {
                Button btnNo = CreateButton("Non", DialogResult.No, buttonCursor);
                Button btnYes = CreateButton("Oui", DialogResult.Yes, buttonCursor);

                // L'ordre d'ajout est inversé car le FlowLayoutPanel est en "RightToLeft"
                pnlButtons.Controls.Add(btnNo);
                pnlButtons.Controls.Add(btnYes);

                this.CancelButton = btnNo;
            }
            else // Par défaut, on met juste un bouton OK
            {
                Button btnOk = CreateButton("OK", DialogResult.OK, buttonCursor);
                pnlButtons.Controls.Add(btnOk);

                this.AcceptButton = btnOk;
                this.CancelButton = btnOk;
            }
        }

        private Button CreateButton(string text, DialogResult dialogResult, Cursor cursor)
        {
            Button btn = new Button
            {
                Text = text.ToUpper(),
                Width = 110,
                Height = 25,
                Margin = new Padding(0, 0, 0, 5),
                Cursor = cursor, // 👆 Le fameux curseur Muffin !
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.LightPink,
                Font = new Font("Segoe UI Emoji", 10F, FontStyle.Bold)


            };
            btn.FlatAppearance.BorderSize = 0;
            btn.FlatAppearance.MouseOverBackColor = Color.SeaGreen;

            btn.Click += (s, e) =>
            {
                _result = dialogResult;
                this.Close();
            };
            return btn;
        }

        /// <summary>
        /// Affiche la boîte de dialogue personnalisée
        /// </summary>
        public static DialogResult Show(string message, string title = "", MessageBoxButtons buttons = MessageBoxButtons.OK, MessageBoxIcon icon = MessageBoxIcon.None)
        {
            using (var customBox = new CustomMessageBox(message, title, buttons, icon))
            {
                customBox.ShowDialog();
                return customBox._result;
            }
        }

        private void CustomMessageBox_Load(object sender, EventArgs e)
        {

        }

        private void lblMessage_Click(object sender, EventArgs e)
        {

        }

        private void rtbMessage_TextChanged(object sender, EventArgs e)
        {

        }

        private void pnlButtons_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
