using DuelDeGateaux.Helpers;
using DuelDeGateaux.Models;
using DuelDeGateaux.Services;
using DuelDeGateaux.Tools;
using System.Net.Mail;

namespace DuelDeGateaux.Forms
{
    /// <summary>
    /// Fenêtre principale de l'application de gestion du concours.
    /// </summary>
    public partial class MainForm : Form
    {
        /// <summary>
        /// Configuration de l'application chargée depuis le fichier JSON.
        /// Cette propriété contient toutes les informations nécessaires pour configurer
        /// et lancer un concours de gâteaux.
        /// </summary>
        private AppConfig config = new();        
        /// <summary>
        /// Source de données intermédiaire entre la liste des participants
        /// et l'affichage dans la grille.
        /// Permet de rafraîchir facilement l'interface sans rebinder manuellement.
        /// </summary>
        private BindingSource participantBindingSource = new();

        /// <summary>
        /// Composant permettant d'afficher des bulles d'aide quand l'utilisateur survole un champ avec la souris
        /// </summary>
        private ToolTip toolTip;
        
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
        private readonly string[] allowedExtensions = [".jpg", ".jpeg", ".png"];

        /// <summary>
        /// Constructeur du formulaire principal.
        /// Initialise les composants du formulaire et charge la configuration.
        /// </summary>
        public MainForm()
        {
            // Initialise les composants du Designer
            InitializeComponent();
            // Configure les aides à la saisie (bulles d'aide)
            InitTooltips();
        }

        /// <summary>
        /// Se déclenche au chargement de la fenêtre.
        /// </summary>
        private void MainForm_Load(object sender, EventArgs e)
        {
            try
            {
                // Chargement des données JSON
                config = ConfigService.Load();
                // Remplissage des champs de l'écran
                LoadConfigToUI();
            }
            catch (Exception ex)
            {
                // Gestion de l'erreur si le fichier JSON est introuvable ou mal formé
                MessageBox.Show($"Erreur lors du chargement de la configuration :\n{ex.Message}", "Erreur au démarrage", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        /// <summary>
        /// Charge la configuration de l'application depuis le fichier JSON.
        /// Cette méthode lit le fichier de configuration et initialise les champs
        /// du formulaire avec les valeurs de configuration.
        /// </summary>
        private void LoadConfigToUI()
        {
            try
            {
                //🧾 GROUPE CONCOURS
                if (DateTime.TryParse(config.ChallengeDate, out DateTime date))
                {
                    datePicker.Value = date;
                }
                if (DateTime.TryParseExact(config.ChallengeHour, "HH:mm", null, System.Globalization.DateTimeStyles.None, out DateTime time))
                {
                    timePicker.Value = time;
                }
                txtRoom.Text = config.ChallengeRoom;
                txtTheme.Text = config.ChallengeTheme;
                txtRules.Text = config.ChallengeRules;
                txtPrice.Text = config.ChallengePrice;
                txtParticipation.Text = config.ChallengeParticipationMessage;
                txtTitles.Text = string.Join(",", config.ChallengersTitles ?? new());
                if (config.ChallengerNumber == 3)
                {
                    rb3Challengers.Checked = true;
                }
                else
                {
                    rb2Challengers.Checked = true;
                }

                // 🎨 GROUPE AFFICHAGE
                numFontSize.Value = UiHelper.ClampNumeric(config.FontSize, numFontSize.Minimum, numFontSize.Maximum);
                txtImageHeader.Text = config.PathImageHeading;
                pictureHeaderImage.Image?.Dispose();
                pictureHeaderImage.Image = LoadImageFromConfig(config.PathImageHeading);
                numImageHeight.Value = UiHelper.ClampNumeric(config.ImageHeadingHeight, numImageHeight.Minimum, numImageHeight.Maximum);
                txtImageFooter.Text = config.PathImageFooter;
                pictureFooterImage.Image?.Dispose();
                pictureFooterImage.Image = LoadImageFromConfig(config.PathImageFooter);
                // 📧 GROUPE MAIL
                txtSender.Text = config.SenderEmail;
                chkTest.Checked = config.IsTest;
                txtTestMail.Text = config.TesterEmail;
                txtSubjectChallenger.Text = config.SubjectMailChallenger;
                txtSubjectEater.Text = config.SubjectMailEater;
                // 👥 PARTICIPANTS                
                participantBindingSource.DataSource = config.Participants;
                dgvParticipants.DataSource = participantBindingSource;
                RefreshParticipantDataGrid();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Une erreur est survenue lors du chargement de la configuration : {ex.Message}", "Erreur de chargement", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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
        /// Recharge la liste des participants dans l'IHM
        /// </summary>
        private void RefreshParticipantDataGrid()
        {
            participantBindingSource.ResetBindings(false);
        }

        /// <summary>
        /// Sauvegarde les entrées de l'utilisateur dans le fichier de config
        /// </summary>
        private void SaveConfig()
        {
            //🧾 GROUPE CONCOURS
            config.ChallengeDate = datePicker.Value.ToString("dd-MM-yyyy");
            config.ChallengeHour = timePicker.Value.ToString("HH:mm");
            config.ChallengeRoom = txtRoom.Text.Trim();
            config.ChallengeTheme = txtTheme.Text.Trim();
            config.ChallengeRules = txtRules.Text.Trim();
            config.ChallengePrice = txtPrice.Text.Trim();
            config.ChallengeParticipationMessage = txtParticipation.Text.Trim();
            config.ChallengersTitles = txtTitles.Text.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(t => t.Trim()).Where(t => !string.IsNullOrWhiteSpace(t)).ToList();
            config.ChallengerNumber = rb2Challengers.Checked ? 2 : 3;
            // 🎨 GROUPE AFFICHAGE
            config.FontSize = (int) numFontSize.Value;
            config.PathImageHeading = txtImageHeader.Text.Trim();
            config.ImageHeadingHeight = (int)numImageHeight.Value;
            config.PathImageFooter = txtImageFooter.Text.Trim();
            // 📧 GROUPE MAIL
            config.SenderEmail = txtSender.Text.Trim();
            config.IsTest = chkTest.Checked;
            config.TesterEmail = txtTestMail.Text.Trim();
            config.SubjectMailChallenger = txtSubjectChallenger.Text.Trim();
            config.SubjectMailEater = txtSubjectEater.Text.Trim();
            // 👥 GROUPE PARTICIPANTS
            EndEditParticipants();

            ConfigService.Save(config);
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
            bool isValid = true;

            // Fonction locale pour marquer un champ en erreur
            void SetError(Control ctrl, bool error)
            {
                ctrl.BackColor = error ? Color.LightPink : Color.White;
            }
            // =============================
            // VALIDATION TEXTE
            // =============================

            if (string.IsNullOrWhiteSpace(txtTheme.Text))
            {
                SetError(txtTheme, true);
                isValid = false;
            }
            else SetError(txtTheme, false);

            if (string.IsNullOrWhiteSpace(txtRoom.Text))
            {
                SetError(txtRoom, true);
                isValid = false;
            }
            else SetError(txtRoom, false);

            if (string.IsNullOrWhiteSpace(txtRules.Text))
            {
                SetError(txtRules, true);
                isValid = false;
            }
            else SetError(txtRules, false);

            if (string.IsNullOrWhiteSpace(txtPrice.Text))
            {
                SetError(txtPrice, true);
                isValid = false;
            }
            else SetError(txtPrice, false);

            if (string.IsNullOrWhiteSpace(txtParticipation.Text))
            {
                SetError(txtParticipation, true);
                isValid = false;
            }
            else SetError(txtParticipation, false);

            // =============================
            // VALIDATION AFFICHAGE
            // =============================
            if (!string.IsNullOrWhiteSpace(txtImageHeader.Text) && !File.Exists(txtImageHeader.Text))
            {
                SetError(txtImageHeader, true);
                isValid = false;
            }
            else SetError(txtImageHeader, false);
            if (!string.IsNullOrWhiteSpace(txtImageFooter.Text) && !File.Exists(txtImageFooter.Text))
            {
                SetError(txtImageFooter, true);
                isValid = false;
            } else SetError(txtImageFooter, false);

            // =============================
            // VALIDATION EMAIL
            // =============================

            if (!FormValidationService.IsValidEmail(txtSender.Text))
            {
                SetError(txtSender, true);
                isValid = false;
            }
            else SetError(txtSender, false);

            if (chkTest.Checked && !FormValidationService.IsValidEmail(txtTestMail.Text))
            {
                SetError(txtTestMail, true);
                isValid = false;
            }
            else SetError(txtTestMail, false);

            if (string.IsNullOrWhiteSpace(txtSubjectChallenger.Text))
            {
                SetError(txtSubjectChallenger, true);
                isValid = false;
            }
            else SetError(txtSubjectChallenger, false);

            if (string.IsNullOrWhiteSpace(txtSubjectEater.Text))
            {
                SetError(txtSubjectEater, true);
                isValid = false;
            }
            else SetError(txtSubjectEater, false);           

            // =============================
            // RESULTAT GLOBAL
            // =============================

            if (!isValid)
            {
                string[] funInsults = {
                    "⚠️ Oups, il manque des infos ! On se réveille ☕",
                    "⚠️ Faut remplir les cases en rouge, NEUNEU 😤",
                    "⚠️ Un gâteau sans farine, ça ne marche pas. Un formulaire vide non plus 🍰",
                    "⚠️ Allez, on se concentre et on corrige les cases rouges 🎯"
                };
                string randomMessage = funInsults[rng.Next(funInsults.Length)];
                // Petit son d'erreur système pour marquer le coup
                System.Media.SystemSounds.Hand.Play();
                MessageBox.Show(randomMessage, "Formulaire incomplet", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

             // =============================
            // VALIDATION PARTICIPANTS
            // =============================
            int requiredChallengers = rb2Challengers.Checked ? 2 : 3;
            EndEditParticipants();
            participantBindingSource.EndEdit();
            if (!ParticipantService.HasEnoughEligible(config.Participants, requiredChallengers))
            {
                int eligibleCount = config.Participants.Count(p => p.IsEligible);
                System.Media.SystemSounds.Exclamation.Play();
                MessageBox.Show($"Impossible de lancer le tirage !\nIl te faut au moins {requiredChallengers} participants cochés comme 'Challenger' dans la liste. Actuellement, tu n'en as que {eligibleCount}.", 
                    "Où sont les cuisiniers ?", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            return true;
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
                    SaveConfig();                
                    List<Participant> assignments = DrawService.AssignChallengers(config);
                    // TODO: remplacer par l'envoi réel des emails
                    EmailService.TestSend();
                    // TODO : enregistre les tests dans l'historique pour validation à changer lors de la mise en prod
                    if (config.IsTest)
                    {
                        HistoryService.Add(config, assignments);
                    }                
                    // Son de succès 
                    System.Media.SystemSounds.Asterisk.Play();
                    
                }, "🎉 Emails envoyés et challengers désignés ! Préparez les fourchettes 🍴");
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
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
                SaveConfig();
            },"Configuration sauvegardée! ");
        }

        /// <summary>
        /// Action utilisateur pour accéder à l'historique
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnHistory_Click(object sender, EventArgs e)
        {
           new HistoryForm().ShowDialog();
        }

        /// <summary>
        /// Ouverture du fichier de config au clic sur le bouton
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnOpenjson_Click(object sender, EventArgs e)
        {
            ConfigService.OpenConfigFile();
        }

        /// <summary>
        /// Action utilisateur pour générer et imprimer les bulletins de vote
        /// </summary>
        private void BtnPrintBallot_Click(object sender, EventArgs e)
        {
            UiHelper.ExecuteWithErrorHandling(() =>
            {
                // On sauvegarde d'abord pour être sûr d'avoir la bonne date et le bon nombre de gâteaux (2 ou 3)
                SaveConfig(); 
                
                // On lance la génération HTML et l'impression !
                BallotService.GenerateAndPrintBallots(config);
                
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
            ParticipantService.AddDefaultParticipant(config.Participants, txtSender.Text.Trim());
            //RefreshParticipantDataGrid();
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
                    config.Participants.RemoveAt(e.RowIndex);
                    RefreshParticipantDataGrid();
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
            string path = FileHelper.SelectImage(textBox.Text);
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

            toolTip.SetToolTip(btnSave,
                "Sauvegarde la configuration dans le fichier JSON.");

            toolTip.SetToolTip(btnHistory,
                "Ouvre la page d'historique des derniers concours.");

            toolTip.SetToolTip(btnOpenJson,
                "Ouvre le fichier de configuration dans l'explorateur.");

        }
        #endregion Tooltip

    }
}