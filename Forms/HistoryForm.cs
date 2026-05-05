using DuelDeGateaux.Models;
using DuelDeGateaux.Services;

namespace DuelDeGateaux.Forms
{
    /// <summary>
    /// Formulaire permettant d'afficher l'historique des concours sous la forme d'un tableau lisible.
    /// </summary>
    public partial class HistoryForm : Form
    {
        /// <summary>
        /// COnstrcteur du formulaire 
        /// </summary>
        public HistoryForm()
        {
            InitializeComponent();
            //configuration du DataGridView au démarrage
            ConfigureGrid();
            // 🍰 Ajout de l'icône
            IconService.ApplyIconToForm(this);

        }
        /// <summary>
        /// Evénement déclenché au chargement du formulaire
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HistoryForm_Load(object sender, EventArgs e)
        {
            LoadHistory();
        }

        /*
====================================================================================================================
         🧱 Configuration du tableau
====================================================================================================================
        */

        /// <summary>
        /// Configure les colonnes du DataGridView
        /// </summary>
        private void ConfigureGrid() 
        { 
            //IMPORTANT : on désactive l'auto génération
            dgvHistory.AutoGenerateColumns = false;

            //Nettoyage
            dgvHistory.Columns.Clear();

            //==============================
            //🏆 COLONNE SAISON (NOUVEAU)
            //==============================
            dgvHistory.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Tournament",
                HeaderText = "Saison",
                DataPropertyName = "TournamentName", // Propriété ajoutée tout à l'heure
                Width = 100
            });

            //==============================
            //📌 COLONNE PHASE (NOUVEAU)
            //==============================
            dgvHistory.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Phase",
                HeaderText = "Phase",
                DataPropertyName = "PhaseName", // Propriété ajoutée tout à l'heure
                Width = 100
            });

            //==============================
            //📅 COLONNE DATE
            //==============================
            dgvHistory.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Date",
                HeaderText = "Date du concours",
                DataPropertyName = "Date", 
                Width = 110
            });

            //==============================
            //🎨 COLONNE THEME
            //==============================
            dgvHistory.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Theme",
                HeaderText = "Thème",
                DataPropertyName = "Theme", 
                Width = 180
            });

            //==============================
            //⚔️ COLONNE CHALLENGERS
            //==============================
            dgvHistory.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Challengers",
                HeaderText = "Challengers",
                DataPropertyName = "Challengers", 
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            });

            //==============================
            //👑 COLONNE GAGNANT (NOUVEAU)
            //==============================
            dgvHistory.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Winner",
                HeaderText = "Gagnant 👑",
                DataPropertyName = "Winner", 
                Width = 120
            });

            //Options Visuelles
            dgvHistory.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvHistory.ReadOnly = true;
            dgvHistory.AllowUserToAddRows = false;
        }
        /*
====================================================================================================================
         🧱 Chargement des données
====================================================================================================================
        */

        /// <summary>
        /// Charge les données de l'historique et les affiche
        /// </summary>
        private void LoadHistory()
        {
            List<ChallengeHistoryEntry> history = HistoryService.Load();
            dgvHistory.DataSource = history;

        }

    }
}
