using System.Collections.Generic;

namespace DuelDeGateaux.Models
{
    /// <summary>
    /// Réprésente toute la configuration du concours
    /// </summary>
    internal class AppConfig
    {
        //EMAIL
        public string SenderEmail { get; set; } = string.Empty;
        public string TesterEmail { get; set; } = string.Empty;
        public bool IsTest { get; set; }

        //CONCOURS
        public string ChallengeDate { get; set; } = string.Empty;
        public string ChallengeHour { get; set; } = string.Empty;
        public string ChallengeRoom { get; set; } = string.Empty;
        public string ChallengeTheme { get; set; } = string.Empty;

        public string ChallengeRules { get; set; } = string.Empty;
        public string ChallengeParticipationMessage { get; set; } = string.Empty;
        public string ChallengePrice { get; set; } = string.Empty;

        //PARAMETRE
        public int ChallengerNumber { get; set; } = 2;
        public int FontSize { get; set; } = 12;

        //IMAGE
        public string PathImageHeading { get; set; } = string.Empty;
        public int ImageHeadingHeight { get; set; } = 100;
        public string PathImageFooter { get; set; } = string.Empty;

        //TITRE DES CHALLENGERS
        public List<string> ChallengersTitles { get; set; } = new();

        //LISTE DES PARTICIPANTS
        public List<Participant> Participants { get; set; } = new();
    }
}