using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using DuelDeGateaux.Services; // ⚠️ Attention : Assure-toi d'utiliser le bon namespace !

namespace DuelDeGateaux.Forms
{
    public class WinnerSelectionForm : Form
    {
        public string SelectedWinner { get; private set; }
        public int SelectedPhaseIndex { get; private set; }

        private readonly Color _bgColor = Color.FromArgb(255, 253, 240);

        public WinnerSelectionForm(List<string> challengers, int currentPhase, int maxGlobalPhase)
        {
            InitializeComponent();
            SetupContent(challengers, currentPhase, maxGlobalPhase);
            
            // 🪄 MAGIE : On appelle le service une seule fois à la toute fin !
            // Il va scanner toute la fenêtre et appliquer le Muffin aux boutons et le Rouleau au reste.
            CursorService.ApplyCursorsToForm(this);
        }

        private void InitializeComponent()
        {
            this.Text = "Résultat du match 🏆";
            this.ShowIcon = false;
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.BackColor = _bgColor;
            this.AutoSize = true;
            this.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            this.Padding = new Padding(10);
        }

        private void SetupContent(List<string> challengers, int currentPhase, int maxGlobalPhase)
        {
            var flowPanel = new FlowLayoutPanel
            {
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false,
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                Padding = new Padding(20)
            };

            flowPanel.Controls.Add(new Label
            {
                Text = "Choisissez le grand vainqueur :",
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                AutoSize = true,
                Margin = new Padding(0, 0, 0, 15)
            });

            foreach (var challenger in challengers)
            {
                var btn = new Button
                {
                    Text = challenger,
                    Size = new Size(250, 45),
                    Font = new Font("Segoe UI Emoji", 10, FontStyle.Bold),
                    BackColor = Color.White,
                    FlatStyle = FlatStyle.Flat,
                    Margin = new Padding(0, 0, 0, 10)
                };
                btn.Click += (s, e) =>
                {
                    SelectedWinner = challenger;
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                };
                flowPanel.Controls.Add(btn);
            }

            flowPanel.Controls.Add(new Label
            {
                Text = "Si besoin, corriger la colonne du match :",
                Font = new Font("Segoe UI", 9, FontStyle.Italic),
                AutoSize = true,
                Margin = new Padding(0, 15, 0, 5)
            });

            var cmbPhase = new ComboBox
            {
                DropDownStyle = ComboBoxStyle.DropDownList,
                Width = 250,
                Font = new Font("Segoe UI", 9)
            };

            for (int i = 0; i <= maxGlobalPhase + 1; i++)
            {
                cmbPhase.Items.Add($"Tour {i + 1}");
            }
            cmbPhase.SelectedIndex = currentPhase;
            
            SelectedPhaseIndex = currentPhase;
            cmbPhase.SelectedIndexChanged += (s, e) => { SelectedPhaseIndex = cmbPhase.SelectedIndex; };

            flowPanel.Controls.Add(cmbPhase);
            this.Controls.Add(flowPanel);
        }
    }
}