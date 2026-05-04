using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DuelDeGateaux.Models;
using DuelDeGateaux.Repositories;
using DuelDeGateaux.Services;
using Microsoft.Web.WebView2.Core;
using Microsoft.Web.WebView2.WinForms;

namespace DuelDeGateaux.Forms
{
    /// <summary>
    /// Fenêtre affichant l'arbre de tournoi dynamique via WebView2.
    /// </summary>
    public class TournamentForm : Form
    {
        private readonly AppConfig _config;

        // Contrôles UI
        private Panel pnlTop;
        private Button btnNextSeason;
        private ComboBox cmbSeasons;
        private string _displayedSeason; // Retient la saison en cours de visionnage
        private WebView2 webViewTournament;

        public TournamentForm(AppConfig config)
        {
            _config = config;
            _displayedSeason = _config.CurrentTournamentName; // Par défaut, on affiche le présent

            InitializeUI();
            LoadSeasonsList(); // On charge les anciennes saisons
            InitializeWebViewAsync();

            // 🪄 Application des curseurs personnalisés WinForms
            this.Cursor = CursorService.LoadCustomCursor() ?? Cursors.Default;
            btnNextSeason.Cursor = CursorService.LoadCustomButtonCursor() ?? Cursors.Hand;
        }

        /// <summary>
        /// Construit l'interface utilisateur 100% par le code !
        /// </summary>
        private void InitializeUI()
        {
            this.Text = "🏆 Arbre du Tournoi";
            this.Size = new Size(1100, 750);
            this.StartPosition = FormStartPosition.CenterParent;
            this.BackColor = Color.White;
            this.ShowIcon = false;

            // 1. Le bandeau du haut
            pnlTop = new Panel
            {
                Dock = DockStyle.Top,
                Height = 60,
                BackColor = Color.FromArgb(255, 253, 240) // Un joli blanc cassé "crème"
            };

            Label lblSeasonDesc = new Label
            {
                Text = "Voir la saison :",
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                ForeColor = Color.FromArgb(60, 30, 10),
                AutoSize = true,
                Location = new Point(20, 8)
            };

            cmbSeasons = new ComboBox
            {
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = new Font("Segoe UI", 11),
                Width = 200,
                Location = new Point(160, 15),
                Cursor = this.Cursor
            };
            cmbSeasons.SelectedIndexChanged += (s, e) =>
            {
                if (cmbSeasons.SelectedItem != null)
                {
                    _displayedSeason = cmbSeasons.SelectedItem.ToString();
                    RefreshTournamentTree(); // Redessine l'arbre au changement !
                }
            };

            btnNextSeason = new Button
            {
                Text = "🏁 Clôturer la saison",
                Font = new Font("Segoe UI Emoji", 10, FontStyle.Bold),
                BackColor = Color.FromArgb(255, 182, 193),
                FlatStyle = FlatStyle.Flat,
                Width = 200,
                Cursor = CursorService.LoadCustomButtonCursor() ?? Cursors.Hand,
                Dock = DockStyle.Right
            };
            btnNextSeason.FlatAppearance.BorderSize = 0;
            btnNextSeason.Click += BtnNextSeason_Click;
            btnNextSeason.FlatAppearance.MouseOverBackColor = Color.SeaGreen;

            pnlTop.Controls.Add(lblSeasonDesc);
            pnlTop.Controls.Add(cmbSeasons);
            pnlTop.Controls.Add(btnNextSeason);
            pnlTop.Padding = new Padding(10);

            // 2. Le composant WebView2
            webViewTournament = new WebView2
            {
                Dock = DockStyle.Fill
            };

            // On abonne le C# aux messages envoyés par le Javascript (HTML) !
            webViewTournament.WebMessageReceived += WebViewTournament_WebMessageReceived;

            // 3. Ajout à la fenêtre
            this.Controls.Add(webViewTournament);
            this.Controls.Add(pnlTop);
        }

        /// <summary>
        /// Charge l'historique des saisons pour la liste déroulante
        /// </summary>
        private void LoadSeasonsList()
        {
            var allMatches = HistoryService.Load();
            var distinctSeasons = allMatches.Select(m => m.TournamentName).Distinct().ToList();

            if (!distinctSeasons.Contains(_config.CurrentTournamentName))
            {
                distinctSeasons.Add(_config.CurrentTournamentName);
            }

            cmbSeasons.Items.Clear();
            foreach (var season in distinctSeasons.OrderBy(s => s))
            {
                cmbSeasons.Items.Add(season);
            }

            if (cmbSeasons.Items.Contains(_displayedSeason))
                cmbSeasons.SelectedItem = _displayedSeason;
        }

        /// <summary>
        /// Initialise le moteur Edge Chromium (WebView2)
        /// </summary>
        private async void InitializeWebViewAsync()
        {
            // IMPORTANT : S'assure que WebView2 est bien installé sur la machine
            await webViewTournament.EnsureCoreWebView2Async(null);

            // Une fois prêt, on dessine l'arbre !
            RefreshTournamentTree();
        }

        /// <summary>
        /// Génère le HTML/CSS de l'arbre et l'envoie au navigateur.
        /// </summary>
        private void RefreshTournamentTree()
        {
            if (webViewTournament.CoreWebView2 == null)
            {
                return;
            }
            List<ChallengeHistoryEntry> allMatches = HistoryService.Load();
            var currentSeasonMatches = allMatches
                .Where(m => m.TournamentName == _displayedSeason) // ⬅️ Filtre dynamique !
                .ToList();

            if (!currentSeasonMatches.Any())
            {
                webViewTournament.NavigateToString($@"
                    <html><body style='background-color:#FFFDF0; font-family:""Segoe UI"", sans-serif; display:flex; flex-direction:column; align-items:center; justify-content:center; height:100vh; margin:0;'>
                        <h1 style='color:#3C1E0A;'>{_displayedSeason}</h1>
                        <h3 style='color:#E5D3B3;'>L'arbre est vide... Il faut envoyer un duel ! ⚔️</h3>
                    </body></html>");
                return;
            }

            int maxPhase = currentSeasonMatches.Max(m => m.PhaseIndex);
            StringBuilder html = new StringBuilder();

            // --- HTML & CSS ---
            html.AppendLine("<html><head><style>");
            html.AppendLine("body { font-family: 'Segoe UI', sans-serif; background-color: #FFFDF0; margin: 0; padding: 40px; color: #3C1E0A; }");
            html.AppendLine(".bracket { display: flex; justify-content: center; gap: 60px; margin-top: 30px; }");
            html.AppendLine(".column { display: flex; flex-direction: column; justify-content: space-around; gap: 30px; }");

            // Le style des boîtes
            html.AppendLine(".match { background: white; border: 3px solid #F4E8D1; border-radius: 12px; padding: 15px; min-width: 200px; box-shadow: 0 8px 16px rgba(60,30,10,0.05); transition: transform 0.2s; cursor: pointer; }");
            html.AppendLine(".match:hover { transform: scale(1.05); border-color: #FFB6C1; }");
            html.AppendLine(".match-title { font-size: 12px; font-weight: bold; color: #A8957A; text-transform: uppercase; margin: 0 0 10px 0; border-bottom: 1px dashed #F4E8D1; padding-bottom: 5px; text-align: center;}");

            // Joueurs et Gagnants
            html.AppendLine(".player { padding: 8px 0; font-size: 16px; display: flex; justify-content: space-between; align-items: center; }");
            html.AppendLine(".winner { font-weight: bold; color: #4CAF50; }");
            html.AppendLine(".loser { color: #CCC; text-decoration: line-through; }"); // Barre le perdant
            html.AppendLine(".crown { font-size: 20px; }");
            html.AppendLine("</style>");

            // --- JAVASCRIPT ---
            // C'est ici que le HTML "parle" au C# quand on clique sur un match !
            html.AppendLine("<script>");
            html.AppendLine("function selectWinner(matchId, isFinished) {");
            html.AppendLine("  if(!isFinished) { window.chrome.webview.postMessage(matchId); }");
            html.AppendLine("}");
            html.AppendLine("</script>");
            html.AppendLine("</head><body>");

            html.AppendLine($"<h1 style='text-align: center;'>🏆 {_displayedSeason} 🏆</h1>");
            html.AppendLine("<div class='bracket'>");

            // --- CONSTRUCTION DES COLONNES ---
            for (int p = 0; p <= maxPhase; p++)
            {
                var matchesInThisPhase = currentSeasonMatches.Where(m => m.PhaseIndex == p).ToList();
                html.AppendLine("<div class='column'>");

                foreach (var match in matchesInThisPhase)
                {
                    bool isFinished = !string.IsNullOrEmpty(match.Winner);
                    string jsFlag = isFinished ? "true" : "false";

                    // On passe l'ID du match à la fonction Javascript au clic
                    html.AppendLine($"<div class='match' onclick='selectWinner(\"{match.MatchId}\", {jsFlag})'>");
                    html.AppendLine($"<div class='match-title'>{match.PhaseName}</div>");

                    foreach (var player in match.ChallengersList)
                    {
                        bool isWinner = (match.Winner == player);
                        string cssClass = "player";
                        if (isFinished) cssClass += isWinner ? " winner" : " loser";

                        string icon = isWinner ? "<span class='crown'>👑</span>" : "";
                        html.AppendLine($"<div class='{cssClass}'><span>{player}</span> {icon}</div>");
                    }
                    html.AppendLine("</div>");
                }
                html.AppendLine("</div>");
            }

            html.AppendLine("</div></body></html>");
            webViewTournament.NavigateToString(html.ToString());
        }

        /// <summary>
        /// Événement déclenché quand le Javascript envoie un message (le MatchId cliqué).
        /// </summary>
        private void WebViewTournament_WebMessageReceived(object sender, CoreWebView2WebMessageReceivedEventArgs e)
        {
            string matchId = e.TryGetWebMessageAsString();

            var allMatches = HistoryService.Load();
            var match = allMatches.FirstOrDefault(m => m.MatchId == matchId);

            if (match != null && string.IsNullOrEmpty(match.Winner))
            {
                // On calcule quelle est la colonne la plus avancée du tournoi actuel
                int maxGlobalPhase = allMatches.Where(m => m.TournamentName == match.TournamentName).Max(m => m.PhaseIndex);

                using (var winnerForm = new WinnerSelectionForm(match.ChallengersList, match.PhaseIndex, maxGlobalPhase))
                {
                    if (winnerForm.ShowDialog() == DialogResult.OK)
                    {
                        match.Winner = winnerForm.SelectedWinner;

                        // 🛠️ Si l'utilisateur a changé la colonne dans le menu déroulant !
                        if (match.PhaseIndex != winnerForm.SelectedPhaseIndex)
                        {
                            match.PhaseIndex = winnerForm.SelectedPhaseIndex;
                            match.PhaseName = $"Tour {match.PhaseIndex + 1}";
                        }

                        HistoryService.Save(allMatches);
                        AudioService.PlaySaveSound();
                        RefreshTournamentTree();
                    }
                }
            }
        }

        /// <summary>
        /// Action pour forcer le passage à la saison suivante.
        /// </summary>
        private void BtnNextSeason_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show(
                $"Es-tu sûr de vouloir clôturer la {_config.CurrentTournamentName} ?\n\nLe prochain mail envoyé démarrera automatiquement une nouvelle saison.",
                "Nouvelle Saison", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                string currentName = _config.CurrentTournamentName;
                int seasonNum = 1;

                if (currentName.StartsWith("Saison "))
                {
                    int.TryParse(currentName.Substring(7), out seasonNum);
                }

                _config.CurrentTournamentName = $"Saison {seasonNum + 1}";

                // IMPORTANT : Il faut appeler ton ConfigService pour sauvegarder le changement !
                ConfigService.Save(_config);

                // ⬅️ On actualise la liste et l'affichage !
                _displayedSeason = _config.CurrentTournamentName;
                LoadSeasonsList();

                RefreshTournamentTree();
            }
        }
    }

    /// <summary>
    /// Petite fenêtre utilitaire interne pour choisir le gagnant d'un duel.
    /// (Générée 100% par le code pour éviter de créer plein de fichiers !)
    /// </summary>
    internal class WinnerSelectionForm : Form
    {
        public string SelectedWinner { get; private set; }
        public int SelectedPhaseIndex { get; private set; }

        public WinnerSelectionForm(List<string> challengers, int currentPhase, int maxGlobalPhase)
        {
            this.Text = "Résultat du match 🏆";
            this.ShowIcon = false;
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.BackColor = Color.FromArgb(255, 253, 240);

            // 🔄 MAGIE : La fenêtre s'agrandit toute seule pour ne cacher personne !
            this.AutoSize = true;
            this.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            this.Padding = new Padding(10);
            this.Cursor = CursorService.LoadCustomCursor() ?? Cursors.Default;
            Cursor buttonCursor = CursorService.LoadCustomButtonCursor() ?? Cursors.Hand;

            FlowLayoutPanel flowPanel = new FlowLayoutPanel
            {
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false,
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                Padding = new Padding(20)
            };

            Label lblInfo = new Label
            {
                Text = "Choisissez le grand vainqueur :",
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                AutoSize = true,
                Margin = new Padding(0, 0, 0, 15)
            };
            flowPanel.Controls.Add(lblInfo);

            foreach (var challenger in challengers)
            {
                Button btn = new Button
                {
                    Text = challenger,
                    Size = new Size(250, 45),
                    // 🦍 Support des Emojis complexes !
                    Font = new Font("Segoe UI Emoji", 10, FontStyle.Bold),
                    BackColor = Color.White,
                    FlatStyle = FlatStyle.Flat,
                    Cursor = buttonCursor,
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

            // --- 🛠️ CORRECTEUR DE COLONNE (MANCHE) ---
            Label lblPhase = new Label
            {
                Text = "Si besoin, corriger la colonne du match :",
                Font = new Font("Segoe UI", 9, FontStyle.Italic),
                AutoSize = true,
                Margin = new Padding(0, 15, 0, 5)
            };
            flowPanel.Controls.Add(lblPhase);

            ComboBox cmbPhase = new ComboBox
            {
                DropDownStyle = ComboBoxStyle.DropDownList,
                Width = 250,
                Font = new Font("Segoe UI", 9),
                Cursor = this.Cursor
            };

            // On propose les colonnes existantes + 1 au cas où on veut créer le tour suivant
            for (int i = 0; i <= maxGlobalPhase + 1; i++)
            {
                cmbPhase.Items.Add($"Tour {i + 1}");
            }
            cmbPhase.SelectedIndex = currentPhase;

            SelectedPhaseIndex = currentPhase;
            cmbPhase.SelectedIndexChanged += (s, e) => { SelectedPhaseIndex = cmbPhase.SelectedIndex; };

            flowPanel.Controls.Add(cmbPhase);
            // ------------------------------------------

            this.Controls.Add(flowPanel);
        }
    }
}
