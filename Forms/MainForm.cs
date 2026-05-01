using DuelDeGateaux.Helpers;
using DuelDeGateaux.Models;
using DuelDeGateaux.Repositories;
using DuelDeGateaux.Services;
using DuelDeGateaux.UI;
using DuelDeGateaux.ViewModels;

namespace DuelDeGateaux.Forms
{
    /// <summary>
    /// Fenêtre principale de l'application de gestion du concours.
    /// </summary>
    public partial class MainForm : Form
    {
        /// <summary>
        /// ViewModel représentant l'état actuel du formulaire
        /// </summary>
        private readonly MainFormViewModel viewModel = new();
        /// <summary>
        /// Source de données intermédiaire entre la liste des participants
        /// et l'affichage dans la grille.
        /// Permet de rafraîchir facilement l'interface sans rebinder manuellement.
        /// </summary>
        private readonly BindingSource participantBindingSource = new();

        /// <summary>
        /// Composant permettant d'afficher des bulles d'aide quand l'utilisateur survole un champ avec la souris
        /// </summary>
        private readonly ToolTip toolTip;
        
        /// <summary>
        /// Random utilisé pour randomiser la sélection de certaines string
        /// </summary>
        private static readonly Random _messageRandomizer = new();
        
        /// <summary>
        /// Taille fixe utilisée pour les miniatures affichées dans l'interface.
        /// </summary>
        private const int _thumbnailSize = 55;

        /// <summary>
        /// Extensions de fichiers autorisées pour les images importées.
        /// </summary>
        private static readonly string[] _allowedExtensions = [".jpg", ".jpeg", ".png"];
        
        #region UI Exposed Controls
        // INFOS CONCOURS
        public DateTimePicker DatePickerControl => datePicker;
        public DateTimePicker TimePickerControl => timePicker;

        public TextBox RoomControl => txtRoom;
        public TextBox ThemeControl => txtTheme;
        public TextBox RulesControl => txtRules;
        public TextBox PriceControl => txtPrice;
        public TextBox ParticipationControl => txtParticipation;
        public TextBox TitlesControl => txtTitles;
        // AFFICHAGE
        public NumericUpDown FontSizeControl => numFontSize;
        public NumericUpDown ImageHeightControl => numImageHeight;

        public PictureBox HeaderImageControl => pictureHeaderImage;
        public PictureBox FooterImageControl => pictureFooterImage;
        // EMAIL
        public TextBox SenderControl => txtSender;
        public CheckBox TestModeControl => chkTest;
        public TextBox TestMailControl => txtTestMail;
        public TextBox SubjectChallengerControl => txtSubjectChallenger;
        public TextBox SubjectEaterControl => txtSubjectEater;
        // PARTICIPANTS
        public Button AddParticipantControl => btnAddParticipants;
        public DataGridView ParticipantsGridControl => dgvParticipants;
        // CHALLENGERS
        public RadioButton TwoChallengersControl => rb2Challengers;
        public RadioButton ThreeChallengersControl => rb3Challengers;
        // BOUTONS
        public Button SendControl => btnSend;
        public Button PreviewControl => btnPreview;
        public Button SaveControl => btnSave;
        public Button PrintBallotControl => btnPrintBallot;
        public Button HistoryControl => btnHistory;
        public Button OpenJsonControl => btnOpenJson;

        #endregion

        /// <summary>
        /// Constructeur du formulaire principal.
        /// Initialise les composants du formulaire et charge la configuration.
        /// </summary>
        public MainForm()
        {
            // Initialise les composants du Designer
            InitializeComponent();
            //On initialise les champs contrôlés
            validationControls = ValidationDefinitions.Build(this);
            // Configure les aides à la saisie (bulles d'aide)
            toolTip = TooltipService.BuildDefault();
            TooltipUiService.Configure(toolTip, TooltipDefinitions.Build(this));
            // 🪄 MAGIE DU CURSEUR PERSONNALISÉ
            SetCustomCursor();
            // 👆 MAGIE DU SURVOL DES BOUTONS
            SetButtonCursors();
            // ✍️ MAGIE DU SURVOL DU TEXTE
            SetTextBoxCursors();
            //Assignation des valeurs du viewmodel aux composants
            rb2Challengers.Checked = viewModel.ChallengerNumber == 2;
            rb3Challengers.Checked = viewModel.ChallengerNumber == 3;
            pictureHeaderImage.DragDrop += (s, e) => PictureBox_DragDrop(sender: s, e: e, true);
            pictureFooterImage.DragDrop += (s, e) => PictureBox_DragDrop(sender: s, e: e, false);
        }

        /// <summary>
        /// Se déclenche au chargement de la fenêtre.
        /// </summary>
        private void MainForm_Load(object sender, EventArgs e)
        {
            try
            {                
                 // 1) Préparer les bindings une seule fois
                SetupBindings();

                // 2) Charger la configuration
                var config = ConfigService.Load();
                var loadedVm = MainFormViewModel.FromConfig(config);
                
                // 3) Connecter la grille
                SetupParticipantsGrid();
                
                // 4) Copier les données dans l'instance existante
                viewModel.LoadFrom(loadedVm);

                // 5) Charger les aperçus images
                RefreshImagePreviews();
                // 6) 🎵 Lancement de la musique de fond !
                AudioService.PlayBackgroundMusic();
            }
            catch (Exception ex)
            {
                // Gestion de l'erreur si le fichier JSON est introuvable ou mal formé
                CustomMessageBox.Show($"Erreur lors du chargement de la configuration :\n{ex.Message}", "Erreur au démarrage", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void RefreshImagePreviews()
        {
            SetHeaderPreview(viewModel.PathImageHeading);
            SetFooterPreview(viewModel.PathImageFooter);
        }
        
        private void SetHeaderPreview(string path)
        {
            pictureHeaderImage.Image?.Dispose();
            pictureHeaderImage.Image = ImageUiService.LoadPreviewFromConfig(path, _thumbnailSize);
        }

        private void SetFooterPreview(string path)
        {
            pictureFooterImage.Image?.Dispose();
            pictureFooterImage.Image = ImageUiService.LoadPreviewFromConfig(path, _thumbnailSize);
        }

        /// <summary>
        /// Charge une image depuis un chemin de fichier spécifié.
        /// Cette méthode tente de charger une image depuis le chemin de fichier donné,
        /// et retourne une miniature de l'image si elle est chargée avec succès.
        /// Si le chemin de fichier est invalide ou que l'image ne peut pas être chargée,
        /// un message d'erreur est affiché à l'utilisateur.
        /// </summary>
        /// <param name="path">Le chemin de fichier de l'image à charger.</param>
        /// <returns>Une miniature de l'image chargée, ou null si l'image ne peut pas être chargée.</returns>
        private void LoadImageUserInput(string path, Action<string> updatePath, Action<Image> updatePreview)
        {
            try
            {
                var preview = ImageUiService.LoadPreviewFromUserInput(path, _thumbnailSize);

                if (preview != null)
                {
                    updatePath(path);
                    updatePreview(preview);
                }
            }
            catch (FileNotFoundException)
            {
                CustomMessageBox.Show("Image introuvable...\nT'as mangé le fichier ? 🍰","Erreur image", MessageBoxButtons.OK,MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                CustomMessageBox.Show($"Une erreur est survenue lors du chargement de l'image : {ex.Message}", "Erreur de chargement", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        

        /// <summary>
        /// Sauvegarde les entrées de l'utilisateur dans le fichier de config
        /// </summary>
        private void SyncAndSaveConfig()
        {
            if (!ValidateFields())
                return;
            // 👥 GROUPE PARTICIPANTS
            EndEditParticipants();
            ConfigService.Save(viewModel.ToConfig());
        } 
        #region Bouton
        #region Actions principales
        /// <summary>
        /// Action de lancement du mailling du concours
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnRun_Click( object sender, EventArgs e)
        {
            if (!ValidateFields())
            {
                return;
            }
            this.Cursor = Cursors.WaitCursor;
            try
            {
                UiHelper.ExecuteWithErrorHandling(() =>
                {
                    SyncAndSaveConfig();                
                    var currentConfig = viewModel.ToConfig();
                    List<Participant> assignments = DrawService.AssignChallengers(currentConfig);
                    EmailService.SendDuelEmails(currentConfig, assignments);
                    if (!currentConfig.IsTest)
                    {
                        HistoryService.Add(currentConfig, assignments);
                    }

                    /// 💥 Joue le son d'effet spécial d'envoi !
                    AudioService.PlaySendSound();
                    
                }, "🎉 Emails envoyés et challengers désignés ! Préparez les fourchettes 🍴");
            }
            finally
            {
                this.Cursor = mainCursor ?? Cursors.Default;
            }
        }
        /// <summary>
        /// Action utilisateur pour prévisualiser les templates d'e-mail
        /// </summary>
        private void BtnPreview_Click(object sender, EventArgs e)
        {
            // Création du menu déroulant
            ContextMenuStrip previewMenu = new ContextMenuStrip();
            previewMenu.Cursor = Cursors.Hand; // Pour garder un bel aspect au survol
            previewMenu.Font = new Font("Segoe UI Emoji", 10, FontStyle.Regular);

            // Option 1 : Les Challengers
            ToolStripMenuItem itemChallenger = new ToolStripMenuItem("⚔️ Aperçu Mail Challengers");
            itemChallenger.Click += (s, args) => LaunchPreviewWindow(true);
            
            // Option 2 : Le Jury (Mangeurs)
            ToolStripMenuItem itemJury = new ToolStripMenuItem("🤤 Aperçu Mail Jury");
            itemJury.Click += (s, args) => LaunchPreviewWindow(false);

            // Ajout des options au menu
            previewMenu.Items.Add(itemChallenger);
            previewMenu.Items.Add(new ToolStripSeparator()); // Une jolie petite ligne de séparation
            previewMenu.Items.Add(itemJury);

            // On affiche le menu juste en dessous du bouton cliqué
            Button btn = (Button)sender;
            previewMenu.Show(btn, new Point(0, btn.Height));
            
        }

        /// <summary>
        /// Génère et affiche la fenêtre de prévisualisation du mail.
        /// </summary>
        /// <param name="isChallenger">True pour le mail Challenger, False pour le mail Jury.</param>
        private void LaunchPreviewWindow(bool isChallenger)
        {
            if (!ValidateFields())
            {
                return;
            }

            UiHelper.ExecuteWithErrorHandling(() =>
            {
                // On joue le petit son de bulle qu'on a configuré !
                AudioService.PlayPreviewSound();

                // On met à jour la configuration en mémoire
                var currentConfig = viewModel.ToConfig();

                string title = isChallenger ? "Mail Challengers ⚔️" : "Mail Jury (Mangeurs) 🤤";
                
                // On récupère le HTML
                string html = EmailService.GetPreviewHtml(currentConfig, isChallenger);

                // On ouvre la belle fenêtre WebView2
                using (var previewWindow = new PreviewForm(html, title))
                {
                    previewWindow.ShowDialog();
                }
            }, null);
        }

        /// <summary>
        /// Action utilisateur pour sauvegarder la configuration.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnSave_Click(object sender, EventArgs e)
        {
            UiHelper.ExecuteWithErrorHandling(() =>
            {                
                SyncAndSaveConfig();
                AudioService.PlaySaveSound();
            },"Configuration sauvegardée! ");
        }

        /// <summary>
        /// Action utilisateur pour accéder à l'historique
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnHistory_Click(object sender, EventArgs e)
        {            
           AudioService.PlayHistorySound();
           // Création du menu déroulant (comme pour la preview !)
           ContextMenuStrip historyMenu = new ContextMenuStrip();
           historyMenu.Cursor = Cursors.Hand; 
           historyMenu.Font = new Font("Segoe UI Emoji", 10, FontStyle.Regular);

           // Option 1 : Le tableau brut
           ToolStripMenuItem itemTable = new ToolStripMenuItem("📖 Tableau brut");
           itemTable.Click += btnMenuHistoryTable_Click;
           
           // Option 2 : L'arbre du tournoi
           ToolStripMenuItem itemTree = new ToolStripMenuItem("🏆 Arbre du Tournoi");
           itemTree.Click += btnMenuHistoryTree_Click; 

           // Ajout des options au menu
           historyMenu.Items.Add(itemTable);
           historyMenu.Items.Add(new ToolStripSeparator());
           historyMenu.Items.Add(itemTree);

           // On affiche le menu juste en dessous du bouton cliqué
           Button btn = (Button)sender;
           historyMenu.Show(btn, new Point(0, btn.Height));
        }
        /// <summary>
        /// Ouvre l'historique brut (le tableau classique)
        /// </summary>
        private void btnMenuHistoryTable_Click(object sender, EventArgs e)
        {
            var historyForm = new HistoryForm();
            historyForm.ShowDialog();
        }

        /// <summary>
        /// Ouvre le nouvel arbre du tournoi WebView2 !
        /// </summary>
        private void btnMenuHistoryTree_Click(object sender, EventArgs e)
        {
            // On récupère la config à jour (qui contient la Saison actuelle)
            var currentConfig = viewModel.ToConfig();
            
            using var tournamentForm = new TournamentForm(currentConfig);
            tournamentForm.ShowDialog();
        }

        /// <summary>
        /// Ouverture du fichier de config au clic sur le bouton
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnOpenjson_Click(object sender, EventArgs e)
        {
            AudioService.PlayOpenJsonSound();
            ConfigService.OpenConfigFile();
        }

        /// <summary>
        /// Action utilisateur pour générer et imprimer les bulletins de vote
        /// </summary>
        private void BtnPrintBallot_Click(object sender, EventArgs e)
        {
            AudioService.PlayPrintBallotSound();
            UiHelper.ExecuteWithErrorHandling(() =>
            {
                // On sauvegarde d'abord pour être sûr d'avoir la bonne date et le bon nombre de gâteaux (2 ou 3)
                SyncAndSaveConfig(); 
                
                // On lance la génération HTML et l'impression !
                BallotService.GenerateAndPrintBallots(viewModel.ToConfig());
                
            }, null); // Pas de message de succès, l'ouverture du navigateur suffit
        }
        #endregion Actions principales
        #region Participants
        /// <summary>
        /// Ajoute un participant dans la liste 
        /// Modification conservée en mémoire uniquement.
        /// L'utilisateur doit cliquer sur "Sauvegarder" pour l'enregistrer dans le fichier JSON.
        /// </summary>
        private void BtnAddParticipantsList_Click(object sender, EventArgs e)
        {
            // Ajoute une ligne exemple que l'utilisateur pourra modifier dans la grille
            ParticipantService.AddDefaultParticipant(viewModel.Participants, viewModel.SenderEmail.Trim());
            AudioService.PlayPreviewSound();
            CustomMessageBox.Show("Participant ajouté. Pensez à sauvegarder pour enregistrer les modifications.");
        }

        /// <summary>
        /// Gère les click sur la grille des participants dont la suppression
        /// </summary>
        private void dgvParticipants_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;
            //Index de la colonne "corbeille" dernière colonne
            int deleteColumnindex = dgvParticipants.Columns.GetLastColumn(DataGridViewElementStates.None, DataGridViewElementStates.None).Index;
            if(deleteColumnindex == e.ColumnIndex)
            {
                //Demande une confirmation de l'utilisateur 
                var result = CustomMessageBox.Show("Êtes-vous sûr de vouloir supprimer ce gentil participant ?",
                    "Confirmer",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning
                 );
                if(result == DialogResult.Yes)
                {
                    viewModel.Participants.RemoveAt(e.RowIndex);
                    AudioService.PlayPreviewSound();
                }
            }
        }
        /// <summary>
        /// Change le curseur en Muffin quand on survole la colonne de suppression.
        /// </summary>
        private void dgvParticipants_CellMouseEnter(object sender, DataGridViewCellEventArgs e)
        {
            // Si on survole la colonne de suppression (la dernière)
            int deleteColumnIndex = dgvParticipants.Columns.GetLastColumn(DataGridViewElementStates.None, DataGridViewElementStates.None).Index;
            
            if (e.RowIndex >= 0 && e.ColumnIndex == deleteColumnIndex)
            {
                dgvParticipants.Cursor = CursorService.LoadCustomButtonCursor() ?? Cursors.Default;
            }
            else
            {
                // Sinon on remet le curseur par défaut (ton rouleau)
                dgvParticipants.Cursor = this.Cursor; 
            }
        }
        #endregion Participants
        #region Images
        /// <summary>
        /// Événement permettant de sélectionner une image pour l'en-tête.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnBrowseHeader_Click(object sender, EventArgs e)
        {
            string path = FileSelectionService.SelectImage(viewModel.PathImageHeading);
            if (!string.IsNullOrEmpty(path))
            {
                UpdateHeaderImage(path);
            }
        }
        
        /// <summary>
        /// Événement permettant de sélectionner une image pour le header.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnBrowseFooter_Click(object sender, EventArgs e)
        {
            string path = FileSelectionService.SelectImage(viewModel.PathImageFooter);
            if (!string.IsNullOrEmpty(path))
            {
                UpdateFooterImage(path);
            }
        }

        private void PictureBox_DragEnter(object sender, DragEventArgs e)
        {
            // On vérifie si ce qu'on survole est bien un fichier
            if (e.Data?.GetDataPresent(DataFormats.FileDrop) == true)
                e.Effect = DragDropEffects.Copy;
        }

        private void PictureBox_DragDrop(object sender, DragEventArgs e, bool isHeader)
        {
            if (e.Data?.GetData(DataFormats.FileDrop) is not string[] files || files.Length == 0)
                return;

            string path = files[0];
            if (!ImageUiService.IsSupportedImage(path, _allowedExtensions))
            {
                CustomMessageBox.Show("Hé ! On a dit une image, pas un PDF ! 😤", "Erreur de cuisine");
                return;
            }

            if (isHeader)
            {
                UpdateHeaderImage(path);
            }
            else
            {
                UpdateFooterImage(path);
            }
        }

        private void UpdateHeaderImage(string path)
        {
            LoadImageUserInput(
                path,
                p => viewModel.PathImageHeading = p,
                img =>
                {
                    pictureHeaderImage.Image?.Dispose();
                    pictureHeaderImage.Image = img;
                });
        }

        private void UpdateFooterImage(string path)
        {
            LoadImageUserInput(
                path,
                p => viewModel.PathImageFooter = p,
                img =>
                {
                    pictureFooterImage.Image?.Dispose();
                    pictureFooterImage.Image = img;
                });
        }

        private void PictureBox_MouseEnter(object sender, EventArgs e)
        {
            PictureBox pb = (PictureBox)sender;
            pb.Cursor = Cursors.Hand;
            pb.BackColor = Color.LightYellow; // Petit flash au survol
            // Optionnel : tu peux aussi changer la BorderStyle
            pb.BorderStyle = BorderStyle.FixedSingle;
        }

        private void PictureBox_MouseLeave(object sender, EventArgs e)
        {
            PictureBox pb = (PictureBox)sender;
            pb.BackColor = Color.Transparent;
            pb.BorderStyle = BorderStyle.None;
        }
        #endregion Images
        #region Sélection challengers
        private void rb2Challengers_CheckedChanged(object sender, EventArgs e)
        {
            if (rb2Challengers.Checked)
                viewModel.ChallengerNumber = 2;
        }

        private void rb3Challengers_CheckedChanged(object sender, EventArgs e)
        {
            if (rb3Challengers.Checked)
                viewModel.ChallengerNumber = 3;
        }
        #endregion Sélection challengers
        #endregion Bouton
    }
}
