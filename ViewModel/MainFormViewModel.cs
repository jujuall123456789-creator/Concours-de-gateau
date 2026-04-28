using DuelDeGateaux.Models;

namespace DuelDeGateaux.ViewModels
{
    /// <summary>
    /// Représente les données manipulées dans le formulaire principal.
    /// Sert d'intermédiaire entre l'UI et AppConfig.
    /// </summary>
    public class MainFormViewModel
    {
        #region Contest

        public DateTime ChallengeDate { get; set; }
        public string ChallengeHour { get; set; } = "";
        public string ChallengeRoom { get; set; } = "";
        public string ChallengeTheme { get; set; } = "";
        public string ChallengeRules { get; set; } = "";
        public string ChallengePrice { get; set; } = "";
        public string ChallengeParticipationMessage { get; set; } = "";
        public string ChallengersTitlesRaw { get; set; } = "";
        public int ChallengerNumber { get; set; }

        #endregion

        #region Display

        public int FontSize { get; set; }
        public string PathImageHeading { get; set; } = "";
        public int ImageHeadingHeight { get; set; }
        public string PathImageFooter { get; set; } = "";

        #endregion

        #region Mail

        public string SenderEmail { get; set; } = "";
        public bool IsTest { get; set; }
        public string TesterEmail { get; set; } = "";
        public string SubjectMailChallenger { get; set; } = "";
        public string SubjectMailEater { get; set; } = "";

        #endregion

        #region Participants

        public List<Participant> Participants { get; set; } = new();

        #endregion

        #region Mapping

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
                ChallengersTitlesRaw = string.Join(",", config.ChallengersTitles ?? new()),
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

                Participants = config.Participants
            };
        }

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
