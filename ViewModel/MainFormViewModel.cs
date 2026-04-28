using DuelDeGateaux.Models;

namespace DuelDeGateaux.ViewModels
{
    /// <summary>
    /// Représente toutes les données manipulées dans le formulaire principal.
    ///
    /// Cette classe sert d'intermédiaire entre :
    /// - l'interface utilisateur (MainForm)
    /// - le modèle de configuration (AppConfig)
    ///
    /// Pourquoi ?
    /// Pour éviter que le formulaire manipule directement la configuration.
    /// Cela permet d'avoir un code plus clair et plus structuré.
    /// </summary>
    public class MainFormViewModel
    {
        #region Partie concours

        /// <summary>
        /// Date du concours.
        /// Utilise DateTime pour simplifier la manipulation dans l'interface.
        /// </summary>
        public DateTime ChallengeDate { get; set; }

        /// <summary>
        /// Heure du concours au format texte (ex : 14:30).
        /// </summary>
        public string ChallengeHour { get; set; } = "";

        /// <summary>
        /// Salle du concours.
        /// </summary>
        public string ChallengeRoom { get; set; } = "";

        /// <summary>
        /// Thème du concours.
        /// </summary>
        public string ChallengeTheme { get; set; } = "";

        /// <summary>
        /// Règles du concours.
        /// </summary>
        public string ChallengeRules { get; set; } = "";

        /// <summary>
        /// Prix à gagner.
        /// </summary>
        public string ChallengePrice { get; set; } = "";

        /// <summary>
        /// Message de participation envoyé aux participants.
        /// </summary>
        public string ChallengeParticipationMessage { get; set; } = "";

        /// <summary>
        /// Liste brute des titres des challengers.
        /// Exemple : "Chef Sucré, Roi du Gâteau"
        /// </summary>
        public string ChallengersTitlesRaw { get; set; } = "";

        /// <summary>
        /// Nombre de challengers attendus.
        /// </summary>
        public int ChallengerNumber { get; set; }

        #endregion

        #region Partie affichage

        /// <summary>
        /// Taille de police utilisée dans les mails.
        /// </summary>
        public int FontSize { get; set; }

        /// <summary>
        /// Chemin vers l'image d'en-tête.
        /// </summary>
        public string PathImageHeading { get; set; } = "";

        /// <summary>
        /// Hauteur de l'image d'en-tête.
        /// </summary>
        public int ImageHeadingHeight { get; set; }

        /// <summary>
        /// Chemin vers l'image de pied de page.
        /// </summary>
        public string PathImageFooter { get; set; } = "";

        #endregion

        #region Partie email

        /// <summary>
        /// Adresse email de l'expéditeur.
        /// </summary>
        public string SenderEmail { get; set; } = "";

        /// <summary>
        /// Active ou non le mode test.
        /// </summary>
        public bool IsTest { get; set; }

        /// <summary>
        /// Adresse email de test.
        /// </summary>
        public string TesterEmail { get; set; } = "";

        /// <summary>
        /// Sujet des emails envoyés aux challengers.
        /// </summary>
        public string SubjectMailChallenger { get; set; } = "";

        /// <summary>
        /// Sujet des emails envoyés aux dégustateurs.
        /// </summary>
        public string SubjectMailEater { get; set; } = "";

        #endregion

        #region Partie participants

        /// <summary>
        /// Liste des participants inscrits.
        /// </summary>
        public List<Participant> Participants { get; set; } = new();

        #endregion

        #region Conversion AppConfig <-> ViewModel

        /// <summary>
        /// Crée un ViewModel à partir d'un objet AppConfig.
        ///
        /// Cette méthode est utilisée au chargement du programme.
        /// Elle transforme les données du fichier JSON
        /// en données adaptées à l'interface.
        /// </summary>
        public static MainFormViewModel FromConfig(AppConfig config)
        {
            return new MainFormViewModel
            {
                ChallengeDate = DateTime.TryParse(config.ChallengeDate, out var date)
                    ? date
                    : DateTime.Today,

                ChallengeHour = config.ChallengeHour,
                ChallengeRoom = config.ChallengeRoom,
                ChallengeTheme = config.ChallengeTheme,
                ChallengeRules = config.ChallengeRules,
                ChallengePrice = config.ChallengePrice,
                ChallengeParticipationMessage = config.ChallengeParticipationMessage,

                // Transforme la liste en texte séparé par des virgules
                ChallengersTitlesRaw = string.Join(", ", config.ChallengersTitles ?? new()),

                ChallengerNumber = config.ChallengerNumber,

                FontSize = config.FontSize,
                PathImageHeading = config.PathImageHeading,
                ImageHeadingHeight = config.ImageHeadingHeight,
                PathImageFooter = config.PathImageFooter,

                SenderEmail = config.SenderEmail,
                IsTest = config.IsTest,
                TesterEmail = config.TesterEmail,
                SubjectMailChallenger = config.SubjectMailChallenger,
                SubjectMailEater = config.SubjectMailEater,

                Participants = config.Participants ?? new()
            };
        }

        /// <summary>
        /// Transforme le ViewModel en AppConfig.
        ///
        /// Cette méthode est utilisée lors de la sauvegarde.
        /// Elle prépare les données pour l'écriture dans le JSON.
        /// </summary>
        public AppConfig ToConfig()
        {
            return new AppConfig
            {
                ChallengeDate = ChallengeDate.ToString("dd-MM-yyyy"),
                ChallengeHour = ChallengeHour,
                ChallengeRoom = ChallengeRoom,
                ChallengeTheme = ChallengeTheme,
                ChallengeRules = ChallengeRules,
                ChallengePrice = ChallengePrice,
                ChallengeParticipationMessage = ChallengeParticipationMessage,

                // Reconvertit le texte en liste
                ChallengersTitles = ChallengersTitlesRaw
                    .Split(',', StringSplitOptions.RemoveEmptyEntries)
                    .Select(t => t.Trim())
                    .ToList(),

                ChallengerNumber = ChallengerNumber,

                FontSize = FontSize,
                PathImageHeading = PathImageHeading,
                ImageHeadingHeight = ImageHeadingHeight,
                PathImageFooter = PathImageFooter,

                SenderEmail = SenderEmail,
                IsTest = IsTest,
                TesterEmail = TesterEmail,
                SubjectMailChallenger = SubjectMailChallenger,
                SubjectMailEater = SubjectMailEater,

                Participants = Participants
            };
        }

        #endregion
    }
}
