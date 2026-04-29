using DuelDeGateaux.Contracts;
using DuelDeGateaux.Helpers;
using DuelDeGateaux.Mappers;
using DuelDeGateaux.Models;
using DuelDeGateaux.Repositories;
using DuelDeGateaux.Services;
using DuelDeGateaux.ViewModels;

namespace DuelDeGateaux.Forms
{
    /// <summary>
    /// Fenêtre principale de l'application de gestion du concours.
    /// </summary>
    public partial class MainForm : Form, IMainFormView
    {
        /// <summary>
        /// ViewModel représentant l'état actuel du formulaire
        /// </summary>
        private MainFormViewModel viewModel = new(); 
        /// <summary>
        /// Source de données intermédiaire entre la liste des participants
        /// et l'affichage dans la grille.
        /// Permet de rafraîchir facilement l'interface sans rebinder manuellement.
        /// </summary>
        private BindingSource participantBindingSource = new();

        /// <summary>
        /// Composant permettant d'afficher des bulles d'aide quand l'utilisateur survole un champ avec la souris
        /// </summary>
        private ToolTip toolTip = new();
        
        /// <summary>
        /// Random utilisé pour randomiser la sélection de certaines string
        /// </summary>
        private static readonly Random rng = new();
        
        /// <summary>
        /// Taille fixe utilisée pour les miniatures affichées dans l'interface.
        /// </summary>
        private const int ThumbnailSize = 55;

        /// <summary>
        /// Extensions de fichiers autorisées pour les images importées.
        /// </summary>
        private static readonly string[] allowedExtensions = [".jpg", ".jpeg", ".png"];

        /// <summary>
        /// Dictionnaire des champs contrôlés
        /// </summary>
        private readonly Dictionary<string, Control> validationControls = new();

        #region Implémentation IMainFormView

        public DateTime ChallengeDate
        {
            get => datePicker.Value;
             set
            {
                    datePicker.Value = value;
            }
        }

        public string ChallengeHour
        {
            get => timePicker.Value.ToString("HH:mm");
            set
            {
                if (DateTime.TryParseExact(value, "HH:mm", null,
                    System.Globalization.DateTimeStyles.None, out var time))
                {
                    timePicker.Value = time;
                }
            }
        }

        public string ChallengeRoom { get => txtRoom.Text; set => txtRoom.Text = value; }
        public string ChallengeTheme { get => txtTheme.Text; set => txtTheme.Text = value; }
        public string ChallengeRules { get => txtRules.Text; set => txtRules.Text = value; }
        public string ChallengePrice { get => txtPrice.Text; set => txtPrice.Text = value; }
        public string ChallengeParticipationMessage { get => txtParticipation.Text; set => txtParticipation.Text = value; }
        public string ChallengersTitlesRaw { get => txtTitles.Text; set => txtTitles.Text = value; }
        public int ChallengerNumber
        {
            get => rb2Challengers.Checked ? 2 : 3;
            set
            {
                rb2Challengers.Checked = value == 2;
                rb3Challengers.Checked = value == 3;
            }
        }

        public int FontSize { get => (int)numFontSize.Value; set => numFontSize.Value = MathHelper.ClampNumeric(value, numFontSize.Minimum, numFontSize.Maximum); }
        public string PathImageHeading { get => txtImageHeader.Text; set => txtImageHeader.Text = value; }
        public int ImageHeadingHeight { get => (int)numImageHeight.Value; set => numImageHeight.Value = MathHelper.ClampNumeric(value, numImageHeight.Minimum, numImageHeight.Maximum); }
        public string PathImageFooter { get => txtImageFooter.Text; set => txtImageFooter.Text = value; }
        public decimal FontSizeMinimum => numFontSize.Minimum;
        public decimal FontSizeMaximum => numFontSize.Maximum;
        public decimal ImageHeightMinimum => numImageHeight.Minimum;
        public decimal ImageHeightMaximum => numImageHeight.Maximum;

        public string SenderEmail { get => txtSender.Text; set => txtSender.Text = value; }
        public bool IsTest { get => chkTest.Checked; set => chkTest.Checked = value; }
        public string TesterEmail { get => txtTestMail.Text; set => txtTestMail.Text = value; }
        public string SubjectMailChallenger { get => txtSubjectChallenger.Text; set => txtSubjectChallenger.Text = value; }
        public string SubjectMailEater { get => txtSubjectEater.Text; set => txtSubjectEater.Text = value; }

        public List<Participant> Participants
        {
            get
            {
                if (participantBindingSource.DataSource is not List<Participant> list)
                {
                    list = new List<Participant>();
                    participantBindingSource.DataSource = list;
                }

                return list;
            }
            set => participantBindingSource.DataSource = value;
        }

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
            InitializeValidationControls();
            // Configure les aides à la saisie (bulles d'aide)
            InitTooltips();
            // 🪄 MAGIE DU CURSEUR PERSONNALISÉ
            SetCustomCursor();
        }

        /// <summary>
        /// Se déclenche au chargement de la fenêtre.
        /// </summary>
        private void MainForm_Load(object sender, EventArgs e)
        {
            try
            {
                // Chargement des données JSON
                var config = ConfigService.Load();
                // Remplissage des champs de l'écran
                viewModel = MainFormViewModel.FromConfig(config);
                MainFormMapper.PopulateView(this, viewModel);
                participantBindingSource.DataSource = viewModel.Participants;
                dgvParticipants.DataSource = participantBindingSource;
                // 🎵 Lancement de la musique de fond !
                AudioService.PlayBackgroundMusic();
            }
            catch (Exception ex)
            {
                // Gestion de l'erreur si le fichier JSON est introuvable ou mal formé
                MessageBox.Show($"Erreur lors du chargement de la configuration :\n{ex.Message}", "Erreur au démarrage", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        
        public void SetHeaderPreview(string path)
        {
            pictureHeaderImage.Image?.Dispose();
            pictureHeaderImage.Image = LoadImageFromConfig(path);
        }

        public void SetFooterPreview(string path)
        {
            pictureFooterImage.Image?.Dispose();
            pictureFooterImage.Image = LoadImageFromConfig(path);
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
        private void LoadImageUserInput(string path, TextBox textBox, PictureBox pictureBox)
        {
            try
            {                
                if (!File.Exists(path))
                {
                    MessageBox.Show("Image introuvable...\nT'as mangé le fichier ? 🍰.","Erreur image",MessageBoxButtons.OK,MessageBoxIcon.Warning);
                    return;
                }
                var preview = ImagePreviewService.LoadPreview(path, ThumbnailSize);
                if (preview != null)
                {
                    textBox.Text = path;
                    // On libère la mémoire de l'image précédente avant d'afficher la nouvelle
                    pictureBox.Image?.Dispose();
                    pictureBox.Image = preview;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Une erreur est survenue lors du chargement de l'image : {ex.Message}", "Erreur de chargement", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        /// <summary>
        /// Charge une image depuis un chemin de fichier spécifié au lancement du programme.
        /// Cette méthode tente de charger une image depuis le chemin de fichier donné,
        /// et retourne une miniature de l'image si elle est chargée avec succès.
        /// </summary>
        /// <param name="path">Le chemin de fichier de l'image à charger.</param>
        /// <returns>Une miniature de l'image chargée, ou null si l'image ne peut pas être chargée.</returns>
        private Image? LoadImageFromConfig(string path)
        {
            try
            {
                if (!File.Exists(path))
                    return null;
                
                return ImagePreviewService.LoadPreview(path, ThumbnailSize);
            }
            catch
            {
                return null;
            }

        }

        /// <summary>
        /// Remplace le curseur par défaut par un magnifique rouleau à pâtisserie.
        /// </summary>
        private void SetCustomCursor()
        {
            Cursor? mycustomCursor = CursorService.LoadCustomCursor();
            if (mycustomCursor != null)
            {
                Cursor = mycustomCursor;
            }
        }
        
        /// <summary>
        /// Recharge la liste des participants dans l'IHM
        /// </summary>
        private void RefreshParticipantDataGrid()
        {
            participantBindingSource.ResetBindings(false);
        }

        /// <summary>
        /// Sauvegarde les entrées de l'utilisateur dans le fichier de config
        /// </summary>
        private void SyncAndSaveConfig()
        {
            
            // 👥 GROUPE PARTICIPANTS
            EndEditParticipants();
            MainFormMapper.UpdateViewModel(this, viewModel);
            ConfigService.Save(viewModel.ToConfig());
        }
        /// <summary>
        /// Valide tous les champs du formulaire et signale les erreurs.
        /// Cette méthode vérifie que tous les champs obligatoires sont remplis
        /// et que les adresses e-mail sont valides. Les champs invalides sont
        /// mis en évidence en les coloriant en rouge, et un message d'erreur est
        /// affiché si des erreurs sont trouvées.
        /// </summary>
        /// <returns>True si tous les champs sont valides, False sinon.</returns>
        private bool ValidateFields()
        {
            EndEditParticipants();
            participantBindingSource.EndEdit();

            MainFormMapper.UpdateViewModel(this, viewModel);
            var config = viewModel.ToConfig();

            var result = FormValidationService.Validate(config);

            ResetFieldColors();

            if (!result.IsValid)
            {
                ApplyFieldErrors(result);
                ShowValidationMessage(result);
                return false;
            }

            return true;
        }

        private void ApplyFieldErrors(ValidationResult result)
        {
            foreach (var key in result.Errors.Keys)
            {
                if (validationControls.TryGetValue(key, out var ctrl))
                    ctrl.BackColor = Color.LightPink;
            }
        }

        private void ShowValidationMessage(ValidationResult result)
        {
            // Petit son d'erreur système pour marquer le coup
            System.Media.SystemSounds.Hand.Play();
            string message = "Le formulaire contient des erreurs :\n\n";
            foreach (var error in result.Errors.Values)
            {
                message += "• " + error + "\n";
            }
            string[] funInsults = {
                    "⚠️ Oups, il manque des infos ! On se réveille ☕",
                    "⚠️ Faut remplir les cases en rouge, NEUNEU 😤",
                    "⚠️ Un gâteau sans farine, ça ne marche pas. Un formulaire vide non plus 🍰",
                    "⚠️ Allez, on se concentre et on corrige les cases rouges 🎯"
                };
            string randomMessage = funInsults[rng.Next(funInsults.Length)];
            message += randomMessage + "\n";
            
            MessageBox.Show(message, "Validation impossible", MessageBoxButtons.OK,MessageBoxIcon.Warning);
        }

        private void ResetFieldColors()
        {
            foreach (var ctrl in validationControls.Values)
                ctrl.BackColor = Color.White;
        }

        private void InitializeValidationControls()
        {
            validationControls["Theme"] = txtTheme;
            validationControls["Room"] = txtRoom;
            validationControls["Rules"] = txtRules;
            validationControls["Price"] = txtPrice;
            validationControls["Participation"] = txtParticipation;
            validationControls["Titles"] = txtTitles;
            validationControls["Date"] = datePicker;
            validationControls["Hour"] = timePicker;
            validationControls["ImageHeader"] = txtImageHeader;
            validationControls["ImageFooter"] = txtImageFooter;
            validationControls["Sender"] = txtSender;
            validationControls["Tester"] = txtTestMail;
            validationControls["SubjectChallenger"] = txtSubjectChallenger;
            validationControls["SubjectEater"] = txtSubjectEater;
        }


        /// <summary>
        /// Force la fin d'édition de l'utilisateur afin de récupérer sa dernière saisie
        /// </summary>
        private void EndEditParticipants()
        {
            dgvParticipants.EndEdit();
            participantBindingSource.EndEdit();
        }

        #region Bouton
         
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
                this.Cursor = Cursors.Default;
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
            previewMenu.Font = new Font("Segoe UI", 10, FontStyle.Regular);

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
                MainFormMapper.UpdateViewModel(this, viewModel);
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
           new HistoryForm().ShowDialog();
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

        
        /// <summary>
        /// Ajoute un participant dans la liste 
        /// Modification conservée en mémoire uniquement.
        /// L'utilisateur doit cliquer sur "Sauvegarder" pour l'enregistrer dans le fichier JSON.
        /// </summary>
        private void BtnAddParticipantsList_Click(object sender, EventArgs e)
        {
            // Ajoute une ligne exemple que l'utilisateur pourra modifier dans la grille
            ParticipantService.AddDefaultParticipant(viewModel.Participants, txtSender.Text.Trim());
            RefreshParticipantDataGrid();
            AudioService.PlayPreviewSound();
            MessageBox.Show("Participant ajouté. Pensez à sauvegarder pour enregistrer les modifications.");
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
                var result = MessageBox.Show("Êtes-vous sûr de vouloir supprimer ce gentil participant ?",
                    "Confirmer",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning,
                    MessageBoxDefaultButton.Button2
                 );
                if(result == DialogResult.Yes)
                {
                    viewModel.Participants.RemoveAt(e.RowIndex);
                    RefreshParticipantDataGrid();
                    AudioService.PlayPreviewSound();
                }
            }
        }
        /// <summary>
        /// Événement permettant de sélectionner une image pour l'en-tête.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnBrowseHeader_Click(object sender, EventArgs e)
        {
            BrowseImage_Click(sender, e, txtImageHeader, pictureHeaderImage);
        }
        
        /// <summary>
        /// Événement permettant de sélectionner une image pour le header.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnBrowseFooter_Click(object sender, EventArgs e)
        {
            BrowseImage_Click(sender, e, txtImageFooter, pictureFooterImage);
        }

        /// <summary>
        /// Méthode commune pour sélectionner une image
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <param name="textBox"></param>
        /// <param name="pictureBox"></param>
        private void BrowseImage_Click(object sender, EventArgs e, TextBox textBox, PictureBox pictureBox)
        {
            string path = FileSelectionService.SelectImage(textBox.Text);
            if (!string.IsNullOrEmpty(path))
            {
                LoadImageUserInput(path, textBox, pictureBox);
            }
        }

        private void PictureBox_DragEnter(object sender, DragEventArgs e)
        {
            // On vérifie si ce qu'on survole est bien un fichier
            if (e.Data?.GetDataPresent(DataFormats.FileDrop) == true)
                e.Effect = DragDropEffects.Copy;
        }

        private void PictureBox_DragDrop(object sender, DragEventArgs e, TextBox associatedTextBox)
        {
            if (e.Data?.GetData(DataFormats.FileDrop) is not string[] files || files.Length == 0)
                return;

            string path = files[0]; // On prend le premier fichier
            string ext = Path.GetExtension(path).ToLower();
                
            if (allowedExtensions.Contains(ext))
            {
                PictureBox pb = (PictureBox)sender;
                LoadImageUserInput(path, associatedTextBox, pb);
            }
            else
            {
                MessageBox.Show("Hé ! On a dit une image, pas un PDF ! 😤", "Erreur de cuisine");
            }
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
        #endregion Bouton

        #region Tooltip
        /// <summary>
        /// Initialisation des tooltips
        /// </summary>
        private void InitTooltips()
        {
            toolTip = TooltipService.BuildDefault();

            // =============================
            // 🧾 INFOS CONCOURS
            // =============================

            toolTip.SetToolTip(datePicker,
                "Sélectionnez la date du concours.\nFormat automatique.");

            toolTip.SetToolTip(timePicker,
                "Sélectionnez l'heure du concours.\nUtilisez les flèches pour ajuster.");

            toolTip.SetToolTip(txtRoom,
                "Indiquez le lieu du concours.\nEx: Salle réunion 2 ou Open Space.");

            toolTip.SetToolTip(txtTheme,
                "Thème du concours de gâteau.\nEx: Gâteau au chocolat, citron...");

            toolTip.SetToolTip(txtRules,
                "Décrivez les règles du concours.\nEx: type de gâteau, contraintes...");

            toolTip.SetToolTip(txtPrice,
                "Décrivez la récompense du concours.\nEx: repas offert, trophée...");

            toolTip.SetToolTip(txtParticipation,
                "Message important pour les participants.\nEx: participation obligatoire.");


            toolTip.SetToolTip(txtTitles,
                "Liste des titres des challengers séparés par des virgules.\nEx: Incroyable, Légendaire, Redoutable");


            // =============================
            // 🎨 AFFICHAGE
            // =============================

            toolTip.SetToolTip(numFontSize,
                "Taille du texte dans les emails.\nEx: 14 à 18 recommandé.");

            toolTip.SetToolTip(txtImageHeader,
                "Chemin de l'image en haut du mail.\nCliquez sur ... pour sélectionner.");

            toolTip.SetToolTip(btnBrowseHeader,
                "Cliquez pour choisir une image depuis votre ordinateur.");

            toolTip.SetToolTip(txtImageFooter,
                "Chemin de l'image en bas du mail.");

            toolTip.SetToolTip(btnBrowseFooter,
                "Cliquez pour choisir une image de pied de mail.");

            toolTip.SetToolTip(numImageHeight,
                "Hauteur de l'image dans l'email.\nAjustez si elle est trop grande/petite.");


            // =============================
            // 📧 SMTP
            // =============================

            toolTip.SetToolTip(txtSender,
                "Adresse email utilisée pour envoyer les mails.");           

            toolTip.SetToolTip(chkTest,
                "Mode test activé :\nTous les mails seront envoyés à une seule adresse.");

            toolTip.SetToolTip(txtTestMail,
                "Adresse qui recevra tous les mails en mode test.");
            toolTip.SetToolTip(txtSubjectChallenger, 
                "Sujet du mail pour les challengers.\nAstuce : Laissez le mot {{Date}} pour que le programme insère la date automatiquement !");

            toolTip.SetToolTip(txtSubjectEater, 
                "Sujet du mail pour le jury.\nAstuce : Laissez le mot {{Date}} pour que le programme insère la date automatiquement !");


            // =============================
            // 👥 PARTICIPANTS
            // =============================

            toolTip.SetToolTip(dgvParticipants,
                "Liste des participants au concours.\nIls peuvent être challengers ou mangeurs.");


            // =============================
            // 🚀 BOUTONS
            // =============================

            toolTip.SetToolTip(btnSend,
                "Lance l'envoi des emails.\n⚠️ Vérifie les champs avant !");

            toolTip.SetToolTip(btnPreview,
                "Preview des mails avec un moteur de rendu HTML.");

            toolTip.SetToolTip(btnSave,
                "Sauvegarde la configuration dans le fichier JSON.");

            toolTip.SetToolTip(btnPrintBallot,
                "Ouvre la page d'impression des bulletins de vote.");

            toolTip.SetToolTip(btnHistory,
                "Ouvre la page d'historique des derniers concours.");

            toolTip.SetToolTip(btnOpenJson,
                "Ouvre le fichier de configuration dans l'explorateur.");

        }
        #endregion Tooltip

    }
}
