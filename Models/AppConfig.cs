using System.Collections.Generic;

namespace DuelDeGateaux.Models
{
    /// <summary>
    /// Réprésente toute la configuration du concours
    /// </summary>
    public class AppConfig
    {
        //EMAIL
        public string SenderEmail { get; set; } = string.Empty;
        public string TesterEmail { get; set; } = string.Empty;
        public bool IsTest { get; set; }
        public string SubjectMailChallenger { get; set; } = string.Empty;
        public string SubjectMailEater { get; set; } = string.Empty;

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
        private List<string> _challengersTitles = new();
        public List<string> ChallengersTitles
        {
            get => _challengersTitles;
            // Si le JSON essaie d'injecter "null", on le rejette et on met une liste vide à la place !
            set => _challengersTitles = value ?? new List<string>(); 
        }

        //LISTE DES PARTICIPANTS
        private List<Participant> _participants = new();
        public List<Participant> Participants
        {
            get => _participants;
            // Si le JSON essaie d'injecter "null", on le rejette et on met une liste vide à la place !
            set => _participants = value ?? new List<Participant>(); 
        }
    }
}