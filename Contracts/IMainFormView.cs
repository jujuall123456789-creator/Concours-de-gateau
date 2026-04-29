using DuelDeGateaux.Models;

namespace DuelDeGateaux.Contracts
{
    /// <summary>
    /// Définit le contrat entre le formulaire principal et le Mapper.
    ///
    /// Une interface décrit ce qu'une classe doit exposer,
    /// sans préciser comment cela est implémenté.
    ///
    /// Ici, elle permet au MainFormMapper de manipuler la vue
    /// sans dépendre directement de WinForms.
    ///
    /// Avantage :
    /// - code plus propre
    /// - meilleure séparation des responsabilités
    /// - plus facile à tester
    /// </summary>
    public interface IMainFormView
    {
        #region Partie concours

        /// <summary>
        /// Date du concours.
        /// </summary>
        DateTime ChallengeDate { get; set; }

        /// <summary>
        /// Heure du concours.
        /// </summary>
        string ChallengeHour { get; set; }

        /// <summary>
        /// Salle du concours.
        /// </summary>
        string ChallengeRoom { get; set; }

        /// <summary>
        /// Thème du concours.
        /// </summary>
        string ChallengeTheme { get; set; }

        /// <summary>
        /// Règles du concours.
        /// </summary>
        string ChallengeRules { get; set; }

        /// <summary>
        /// Prix à gagner.
        /// </summary>
        string ChallengePrice { get; set; }

        /// <summary>
        /// Message de participation.
        /// </summary>
        string ChallengeParticipationMessage { get; set; }

        /// <summary>
        /// Titres des challengers au format texte.
        /// </summary>
        string ChallengersTitlesRaw { get; set; }

        /// <summary>
        /// Nombre de challengers.
        /// </summary>
        int ChallengerNumber { get; set; }

        #endregion

        #region Partie affichage

        /// <summary>
        /// Taille de police.
        /// </summary>
        int FontSize { get; set; }

        /// <summary>
        /// Chemin de l'image d'en-tête.
        /// </summary>
        string PathImageHeading { get; set; }

        /// <summary>
        /// Hauteur de l'image d'en-tête.
        /// </summary>
        int ImageHeadingHeight { get; set; }

        /// <summary>
        /// Chemin de l'image de pied de page.
        /// </summary>
        string PathImageFooter { get; set; }

        void SetHeaderPreview(string path);

        void SetFooterPreview(string path);

        /// <summary>
        /// Valeur minimale autorisée pour la taille de police
        /// </summary>
        decimal FontSizeMinimum { get; }

        /// <summary>
        /// Valeur maximale autorisée pour la taille de police
        /// </summary>
        decimal FontSizeMaximum { get; }

        /// <summary>
        /// Valeur minimale autorisée pour la hauteur d’image
        /// </summary>
        decimal ImageHeightMinimum { get; }

        /// <summary>
        /// Valeur maximale autorisée pour la hauteur d’image
        /// </summary>
        decimal ImageHeightMaximum { get; }

        #endregion

        #region Partie email

        /// <summary>
        /// Adresse email expéditeur.
        /// </summary>
        string SenderEmail { get; set; }

        /// <summary>
        /// Indique si le mode test est activé.
        /// </summary>
        bool IsTest { get; set; }

        /// <summary>
        /// Adresse email de test.
        /// </summary>
        string TesterEmail { get; set; }

        /// <summary>
        /// Sujet email challenger.
        /// </summary>
        string SubjectMailChallenger { get; set; }

        /// <summary>
        /// Sujet email dégustateur.
        /// </summary>
        string SubjectMailEater { get; set; }

        #endregion

        #region Partie participants

        /// <summary>
        /// Liste des participants.
        /// </summary>
        List<Participant> Participants { get; set; }

        #endregion
    }
}