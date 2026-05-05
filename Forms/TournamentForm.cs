using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using DuelDeGateaux.Models;
using DuelDeGateaux.Repositories;
using DuelDeGateaux.Services;
using DuelDeGateaux.Helpers;
using Microsoft.Web.WebView2.Core;
using Microsoft.Web.WebView2.WinForms;

namespace DuelDeGateaux.Forms
{
    public class TournamentForm : Form
    {
        private readonly AppConfig _config;
        private string _displayedSeason;

        // UI Constants
        private readonly Color _bgColor = Color.FromArgb(255, 253, 240);
        private readonly Color _btnColor = Color.FromArgb(255, 182, 193);

        // UI Controls
        private Panel pnlTop = null!;
        private Button btnNextSeason = null!;
        private ComboBox cmbSeasons = null!;
        private WebView2 webViewTournament = null!;

        public TournamentForm(AppConfig config)
        {
            _config = config;
            _displayedSeason = _config.CurrentTournamentName;

            InitializeUI();
            LoadSeasonsList();
            InitializeWebViewAsync();

            // 🪄 MAGIE : On appelle le service après avoir créé l'interface UI
            CursorService.ApplyCursorsToForm(this);
            // 🍰 Ajout de l'icône
            IconService.ApplyIconToForm(this);
        }

        private void InitializeUI()
        {
            this.Text = "🏆 Arbre du Tournoi";
            this.Size = new Size(1100, 750);
            this.StartPosition = FormStartPosition.CenterParent;
            this.BackColor = Color.White;

            pnlTop = new Panel { Dock = DockStyle.Top, Height = 60, BackColor = _bgColor, Padding = new Padding(10) };

            var lblSeasonDesc = new Label { Text = "Voir la saison :", Font = new Font("Segoe UI", 12, FontStyle.Bold), ForeColor = Color.FromArgb(60, 30, 10), AutoSize = true, Location = new Point(20, 8) };

            cmbSeasons = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList, Font = new Font("Segoe UI", 11), Width = 200, Location = new Point(160, 15) };
            cmbSeasons.SelectedIndexChanged += CmbSeasons_SelectedIndexChanged;

            btnNextSeason = new Button { Text = "🏁 Clôturer la saison", Font = new Font("Segoe UI Emoji", 10, FontStyle.Bold), BackColor = _btnColor, FlatStyle = FlatStyle.Flat, Width = 200, Dock = DockStyle.Right };
            btnNextSeason.FlatAppearance.BorderSize = 0;
            btnNextSeason.FlatAppearance.MouseOverBackColor = Color.SeaGreen;
            btnNextSeason.Click += BtnNextSeason_Click;

            pnlTop.Controls.Add(lblSeasonDesc);
            pnlTop.Controls.Add(cmbSeasons);
            pnlTop.Controls.Add(btnNextSeason);

            webViewTournament = new WebView2 { Dock = DockStyle.Fill };
            webViewTournament.WebMessageReceived += WebViewTournament_WebMessageReceived;

            this.Controls.Add(webViewTournament);
            this.Controls.Add(pnlTop);
        }

        private void CmbSeasons_SelectedIndexChanged(object? sender, EventArgs e)
        {
            if (cmbSeasons.SelectedItem != null)
            {
                _displayedSeason = cmbSeasons.SelectedItem?.ToString() ?? string.Empty;
                RefreshTournamentTree();
            }
        }

        private void LoadSeasonsList()
        {
            var allMatches = HistoryService.Load();
            var distinctSeasons = allMatches.Select(m => m.TournamentName).Distinct().ToList();

            if (!distinctSeasons.Contains(_config.CurrentTournamentName))
                distinctSeasons.Add(_config.CurrentTournamentName);

            cmbSeasons.Items.Clear();
            foreach (var season in distinctSeasons.OrderBy(s => s))
                cmbSeasons.Items.Add(season);

            if (cmbSeasons.Items.Contains(_displayedSeason))
                cmbSeasons.SelectedItem = _displayedSeason;
        }

        private async void InitializeWebViewAsync()
        {
            try
            {
                await webViewTournament.EnsureCoreWebView2Async(null);
                
                // Si l'utilisateur a déjà fermé la fenêtre, on ne fait rien !
                if (!this.IsDisposed)
                {
                    RefreshTournamentTree();
                }
            }
            catch (System.Runtime.InteropServices.COMException)
            {
                // L'utilisateur a fermé la fenêtre super vite, on ignore l'erreur silencieusement 🥷
            }
            catch (Exception)
            {
                // Capture de sécurité pour toute autre erreur de démarrage
            }
        }

        private void RefreshTournamentTree()
        {
            if (webViewTournament.CoreWebView2 == null) return;

            var currentSeasonMatches = HistoryService.Load()
                .Where(m => m.TournamentName == _displayedSeason)
                .ToList();

            string cssDefaultCursor = CursorService.GetCssMainCursor();
            string cssPointerCursor = CursorService.GetCssButtonCursor();

            string html = TournamentHtmlBuilder.BuildTree(_displayedSeason, currentSeasonMatches, cssDefaultCursor, cssPointerCursor);
            
            webViewTournament.NavigateToString(html);
        }

        private void WebViewTournament_WebMessageReceived(object? sender, CoreWebView2WebMessageReceivedEventArgs e)
        {
            string matchId = e.TryGetWebMessageAsString();
            var allMatches = HistoryService.Load();
            var match = allMatches.FirstOrDefault(m => m.MatchId == matchId);

            if (match != null)
            {
                int maxGlobalPhase = allMatches.Where(m => m.TournamentName == match.TournamentName).Max(m => m.PhaseIndex);

                using (var winnerForm = new WinnerSelectionForm(match.ChallengersList, match.PhaseIndex, maxGlobalPhase))
                {
                    if (winnerForm.ShowDialog() == DialogResult.OK)
                    {
                        match.Winner = winnerForm.SelectedWinner;
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

        private void BtnNextSeason_Click(object? sender, EventArgs e)
        {
            var result = MessageBox.Show(
                $"Es-tu sûr de vouloir clôturer la {_config.CurrentTournamentName} ?\n\nLe prochain mail envoyé démarrera automatiquement une nouvelle saison.",
                "Nouvelle Saison", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                int seasonNum = 1;
                if (_config.CurrentTournamentName.StartsWith("Saison "))
                    int.TryParse(_config.CurrentTournamentName.Substring(7), out seasonNum);

                _config.CurrentTournamentName = $"Saison {seasonNum + 1}";
                ConfigService.Save(_config);

                _displayedSeason = _config.CurrentTournamentName;
                LoadSeasonsList();
                RefreshTournamentTree();
            }
        }
    }
}