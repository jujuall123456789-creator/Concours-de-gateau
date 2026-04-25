using DuelDeGateaux.Models;
using DuelDeGateaux.Services;
using DuelDeGateaux.Tools;
using System.Net.Mail;

namespace DuelDeGateaux
{
    public partial class MainForm : Form
    {
        /// <summary>
        /// Configuration de l'application chargée depuis le fichier JSON.
        /// Cette propriété contient toutes les informations nécessaires pour configurer
        /// et lancer un concours de gâteaux.
        /// </summary>
        private AppConfig config;
        /// <summary>
        /// Zone de groupe contenant tous les champs liés au concours.
        /// Ce groupe contient les informations de base sur le concours comme la date,
        /// l'heure, le lieu, le thème, les règles, le prix et le message de participation.
        /// </summary>
        private GroupBox grpContest;

        /// <summary>
        /// Sélecteur de date pour la date du concours.
        /// Ce contrôle permet à l'utilisateur de sélectionner la date du concours
        /// de manière interactive.
        /// </summary>
        private DateTimePicker datePicker;

        /// <summary>
        /// Sélecteur de temps pour l'heure du concours.
        /// Ce contrôle permet à l'utilisateur de sélectionner l'heure du concours
        /// de manière interactive.
        /// </summary>
        private DateTimePicker timePicker;

        /// <summary>
        /// Champ salle
        /// </summary>
        private TextBox txtRoom;

        /// <summary>
        /// Champ thème
        /// </summary>
        private TextBox txtTheme;

        /// <summary>
        /// Champ règles
        /// </summary>
        private TextBox txtRules;

        /// <summary>
        /// Champ prix
        /// </summary>
        private TextBox txtPrice;

        /// <summary>
        /// Message obligatoire
        /// </summary>
        private TextBox txtParticipation;

        /// <summary>
        /// Liste des titres challengers
        /// </summary>
        private TextBox txtTitles;
        /// <summary>
        /// Radio bouton permettant la sélection de deux participants
        /// </summary>
        private RadioButton rb2Challengers;
        /// <summary>
        ///Radio bouton permettant la sélection de trois participants
        /// </summary>
        private RadioButton rb3Challengers;
        /// <summary>
        /// GroupeBox regroupant les radio boutons
        /// </summary>
        private GroupBox grpChallengersCount;
        /// <summary>
        /// Groupe affichage
        /// </summary>
        private GroupBox grpDisplay;

        /// <summary>
        /// Taille de police
        /// </summary>
        private NumericUpDown numFontSize;

        /// <summary>
        /// Chemin image header
        /// </summary>
        private TextBox txtImageHeader;

        /// <summary>
        /// Bouton sélection image header
        /// </summary>
        private Button btnBrowseHeader;

        /// <summary>
        /// Apperçu de l'image du header
        /// </summary>
        private PictureBox pictureHeaderImage;

        /// <summary>
        /// Chemin image footer
        /// </summary>
        private TextBox txtImageFooter;

        /// <summary>
        /// Bouton sélection image footer
        /// </summary>
        private Button btnBrowseFooter;

        /// <summary>
        /// Apperçu de l'image du footer
        /// </summary>
        private PictureBox pictureFooterImage;

        /// <summary>
        /// Hauteur image header
        /// </summary>
        private NumericUpDown numImageHeight;       

        /// <summary>
        /// Groupe SMTP
        /// </summary>
        private GroupBox grpSmtp;

        /// <summary>
        /// Email expéditeur
        /// </summary>
        private TextBox txtSender;

        /// <summary>
        /// Mode test
        /// </summary>
        private CheckBox chkTest;

        /// <summary>
        /// Mail de test
        /// </summary>
        private TextBox txtTestMail;

        /// <summary>
        /// Zone contenant le DataGrid participant
        /// </summary>
        private GroupBox grpParticipants;

        /// <summary>
        /// Liste des participants dans une DatagriedView
        /// </summary>
        private DataGridView dgvParticipants;

        /// <summary>
        /// Bouton Ajout de particpant au DatagridView
        /// </summary>
        private Button btnAddParticipants;
        /// <summary>
        /// Boutons principaux
        /// </summary>
        private Button btnSend;
        private Button btnSave;
        private Button btnOpenJson;
        private Button btnHistory;

        /// <summary>
        /// Composant permettant d'afficher des bulles d'aide quand l'utilisateur survole un champ avec la souris
        /// </summary>
        private ToolTip toolTip;

        /// <summary>
        /// Constructeur du formulaire principal.
        /// Initialise les composants du formulaire et charge la configuration.
        /// </summary>
        public MainForm()
        {
            InitializeComponent();
            LoadConfig();
        }
        /// <summary>
        /// Charge la configuration de l'application depuis le fichier JSON.
        /// Cette méthode lit le fichier de configuration et initialise les champs
        /// du formulaire avec les valeurs de configuration.
        /// </summary>
        private void LoadConfig()
        {
            try
            {
                //🧾 GROUPE CONCOURS
                config = ConfigService.Load();
                if (DateTime.TryParse(config.ChallengeDate, out DateTime date))
                {
                    datePicker.Value = date;
                }
                if (DateTime.TryParseExact(config.ChallengeHour, "HH:mm", null, System.Globalization.DateTimeStyles.None, out DateTime time));
                {
                    timePicker.Value = time;
                }
                txtRoom.Text = config.ChallengeRoom;
                txtTheme.Text = config.ChallengeTheme;
                txtRules.Text = config.ChallengeRules;
                txtPrice.Text = config.ChallengePrice;
                txtParticipation.Text = config.ChallengeParticipationMessage;
                txtTitles.Text = string.Join(",", config.ChallengersTitles);
                if (config.ChallengerNumber == 3)
                {
                    rb3Challengers.Checked = true;
                }
                else
                {
                    rb2Challengers.Checked = true;
                }

                // 🎨 GROUPE AFFICHAGE
                numFontSize.Value = config.FontSize;
                txtImageHeader.Text = config.PathImageHeading;
                pictureHeaderImage.Image = LoadImage(config.PathImageHeading);
                numImageHeight.Value = config.ImageHeadingHeight;
                txtImageFooter.Text = config.PathImageFooter;
                pictureFooterImage.Image = LoadImage(config.PathImageFooter);
                // 📧 GROUPE MAIL
                txtSender.Text = config.SenderEmail;
                chkTest.Checked = config.IsTest;
                txtTestMail.Text = config.TesterEmail;
                // 👥 PARTICIPANTS
                RefeshParticipantDataGrid();
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
        private Image LoadImage(string path)
        {
            try
            {
                if (string.IsNullOrEmpty(path) || !File.Exists(path))
                {
                    MessageBox.Show("Le fichier image est introuvable ou le chemin est invalide.", "Erreur de chargement", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return null;
                }
                using (Image image = Image.FromFile(path))
                {
                    Image.GetThumbnailImageAbort myCallback = new Image.GetThumbnailImageAbort(ThumbnailCallBack);
                    return image.GetThumbnailImage(55, 55, myCallback, IntPtr.Zero);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Une erreur est survenue lors du chargement de l'image : {ex.Message}", "Erreur de chargement", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }

        }
        /// <summary>
        /// Charge la liste des participants dans l'IHM
        /// </summary>
        private void RefeshParticipantDataGrid()
        {
            dgvParticipants.DataSource = null;
            dgvParticipants.DataSource = config.Participants;
        }

        /// <summary>
        /// Sauvegarde les entrées de l'utilisateurs dans le fichier de config
        /// </summary>
        private void SaveConfig()
        {
            //🧾 GROUPE CONCOURS
            config.ChallengeDate = datePicker.Value.ToShortDateString();
            config.ChallengeHour = timePicker.Value.ToShortTimeString();
            config.ChallengeRoom = txtRoom.Text;
            config.ChallengeTheme = txtTheme.Text;
            config.ChallengeRules = txtRules.Text;
            config.ChallengePrice = txtPrice.Text;
            config.ChallengeParticipationMessage = txtParticipation.Text;
            config.ChallengersTitles = txtTitles.Text.Split(',').ToList();
            config.ChallengerNumber = rb2Challengers.Checked ? 2 : 3;
            // 🎨 GROUPE AFFICHAGE
            config.FontSize = (int) numFontSize.Value;
            config.PathImageHeading = txtImageHeader.Text;
            config.ImageHeadingHeight = (int)numImageHeight.Value;
            config.PathImageFooter = txtImageFooter.Text;
            // 📧 GROUPE MAIL
            config.SenderEmail = txtSender.Text;
            config.IsTest = chkTest.Checked;
            config.TesterEmail = txtTestMail.Text;

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
            // VALIDATION EMAIL
            // =============================

            if (!IsValidEmail(txtSender.Text))
            {
                SetError(txtSender, true);
                isValid = false;
            }
            else SetError(txtSender, false);

            if (chkTest.Checked && !IsValidEmail(txtTestMail.Text))
            {
                SetError(txtTestMail, true);
                isValid = false;
            }
            else SetError(txtTestMail, false);

            // =============================
            // RESULTAT GLOBAL
            // =============================

            if (!isValid)
            {
                MessageBox.Show("⚠️ Merci de corriger les champs en rouge ! NEUNEU 😤");
            }

            return isValid;
        }
        /// <summary>
        /// vérification de l'adresse mail
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        private bool IsValidEmail( string email)
        {
            try
            {
                return new MailAddress(email).Address == email;
            }
            catch 
            {
                return false;
            }
        }
        #region Bouton

        /// <summary>
        /// Méthode commune pour gérer les erreurs lors de l'exécution des actions
        /// </summary>
        /// <param name="action">Action à exécuter</param>
        /// <param name="successMessage">Message de succès</param>
        private void ExecuteWithErrorHandling(Action action, string successMessage)
        {
            try
            {
                action();
                MessageBox.Show(successMessage);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Une erreur est survenue : {ex.Message}", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Action de lancement du mailling du concours
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnRun_Click( object sender, EventArgs e)
        {
            if (!ValidateFields())
            {
                MessageBox.Show("Erreur de validation");
                return;
            }
            ExecuteWithErrorHandling(() =>
            {
                SaveConfig();
                var assignments = DrawService.AssignChallengers(config);
                EmailService.toto();
                if (config.IsTest)
                {
                    HistoryService.Add(config, assignments);
                }
            },"Emails envoyées! ");
        }
        /// <summary>
        /// Action utilsiateur de sauvgarde de l'historique
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnSave_Click(object sender, EventArgs e)
        {
            ExecuteWithErrorHandling(() =>
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
        /// Ajoute un partcipant dans la liste 
        /// </summary>
        private void btnAddParticipantsList_Click(object sender, EventArgs e)
        {
            config.Participants.Add(new Participant("👴PNJ👴", txtSender.Text));
            RefeshParticipantDataGrid();
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
                    "Confimer",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning,
                    MessageBoxDefaultButton.Button2,
                    MessageBoxOptions.ServiceNotification
                 );
                if(result == DialogResult.Yes)
                {
                    config.Participants.RemoveAt(e.RowIndex);
                    RefeshParticipantDataGrid();
                }
            }
        }
        /// <summary>
        /// Evénement de chercher un fichier image pour le header
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnBrowseHeader_Click(object sender, EventArgs e)
        {
            BrowseImage_Click(sender, e, txtImageHeader, pictureHeaderImage);
        }
        
        /// <summary>
        /// Evénement de chercher un fichier image pour le header
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnBrowseFooter_Click(object sender, EventArgs e)
        {
            BrowseImage_Click(sender, e, txtImageFooter, pictureFooterImage);
        }

        private bool ThumbnailCallBack()
        {
            return false;
        }
        /// <summary>
        /// Méthode commune pour sélectionner une image
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <param name="textBox"></param>
        /// <param name="pictureBox"></param
        private void BrowseImage_Click(object sender, EventArgs e, TextBox textBox, PictureBox pictureBox)
        {
            string path = FileHelper.SelectImage(textBox.Text);
            if (!String.IsNullOrEmpty(path))
            {
                textBox.Text = path;
                Image image = Image.FromFile(path);
                Image.GetThumbnailImageAbort myCallback = new Image.GetThumbnailImageAbort(ThumbnailCallBack);
                pictureBox.Image = image.GetThumbnailImage(55, 55, ThumbnailCallBack, IntPtr.Zero);
            }
        }
        #endregion Bouton

        #region Tooltip
        /// <summary>
        /// Initilisation des tooltips
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void InitTooltips()
        {
            toolTip = new ToolTip();
            toolTip.AutoPopDelay = 50000; // durée d'affichage
            toolTip.InitialDelay = 500; // délai avant affichage
            toolTip.ReshowDelay = 200; // délai entre deux affichages
            toolTip.ShowAlways = true; // s'affiche même si la fenêtre n'est pas active

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
