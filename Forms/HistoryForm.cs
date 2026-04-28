using DuelDeGateaux.Models;
using DuelDeGateaux.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DuelDeGateaux
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
            //📅 COLONNE DATE
            //==============================
            dgvHistory.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Date",
                HeaderText = "Date du concours",
                DataPropertyName = "Date", //correspond à la propriété du modèle
                Width = 120
            });
            //==============================
            //📅 COLONNE THEME
            //==============================
            dgvHistory.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Theme",
                HeaderText = "Thème",
                DataPropertyName = "Theme", //correspond à la propriété du modèle
                Width = 200
            });
            //==============================
            //📅 COLONNE CHALLENGERS
            //==============================
            dgvHistory.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Challengers",
                HeaderText = "Challengers",
                DataPropertyName = "Challengers", //correspond à la propriété du modèle
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
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
