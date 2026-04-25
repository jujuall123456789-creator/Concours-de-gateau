using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace DuelDeGateaux.Models
{
    /// <summary>
    /// Réprésente toute la configuration du concours
    /// </summary>
    internal class AppConfig
    {
        //EMAIL
        public string SenderEmail { get; set; }
        public string TesterEmail { get; set; }
        public bool IsTest { get; set; }

        //CONCOURS
        public string ChallengeDate { get; set; }
        public string ChallengeHour { get; set; }
        public string ChallengeRoom { get; set; }
        public string ChallengeTheme { get; set; }

        public string ChallengeRules { get; set; }
        public string ChallengeParticipationMessage { get; set; }
        public string ChallengePrice { get; set; }

        //PARAMETRE
        public int ChallengerNumber { get; set; }

        public int FontSize { get; set; }

        //IMAGE
        public string PathImageHeading { get; set; }
        public int ImageHeadingHeight { get; set; }
        public string PathImageFooter { get; set; }

        //TITRE DES CHALLENGERS
        public List<string> ChallengersTitles { get; set; }
        //LISTE DES PARTICIPANTS
        public List<Participant> Participants { get; set; }

    }
}
