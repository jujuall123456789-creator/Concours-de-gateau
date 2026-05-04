using DuelDeGateaux.Models;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace DuelDeGateaux.ViewModels
{
    /// <summary>
    /// ViewModel principal du formulaire.
    ///
    /// Il représente l'état complet de l'interface.
    /// Toutes les données affichées ou modifiées dans le formulaire
    /// passent par cette classe.
    ///
    /// Cette version implémente INotifyPropertyChanged
    /// afin que l'interface puisse se synchroniser automatiquement.
    /// </summary>
    public class MainFormViewModel : INotifyPropertyChanged
    {
        #region Notification automatique UI

        /// <summary>
        /// Événement déclenché lorsqu'une propriété change.
        /// Permet aux contrôles liés (DataBinding) de se mettre à jour.
        /// </summary>
        public event PropertyChangedEventHandler? PropertyChanged;

        /// <summary>
        /// Méthode utilitaire pour modifier une propriété
        /// et notifier automatiquement l'interface.
        /// </summary>
        protected bool SetProperty<T>(ref T field, T value, [CallerMemberName] string propertyName = "")
        {
            if (EqualityComparer<T>.Default.Equals(field, value))
                return false;

            field = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            return true;
        }

        #endregion

        #region Partie concours

        private DateTime challengeDate = DateTime.Today;
        public DateTime ChallengeDate
        {
            get => challengeDate;
            set => SetProperty(ref challengeDate, value);
        }

        private string challengeHour = "09:00";
        public string ChallengeHour
        {
            get => challengeHour;
            set => SetProperty(ref challengeHour, value);
        }

        public DateTime ChallengeTime
        {
            get
            {
                if (TimeSpan.TryParse(ChallengeHour, out var ts))
                    return DateTime.Today.Add(ts);

                return DateTime.Today;
            }
            set => ChallengeHour = value.ToString("HH:mm");
        }

        private string challengeRoom = "";
        public string ChallengeRoom
        {
            get => challengeRoom;
            set => SetProperty(ref challengeRoom, value);
        }

        private string challengeTheme = "";
        public string ChallengeTheme
        {
            get => challengeTheme;
            set => SetProperty(ref challengeTheme, value);
        }

        private string challengeRules = "";
        public string ChallengeRules
        {
            get => challengeRules;
            set => SetProperty(ref challengeRules, value);
        }

        private string challengePrice = "";
        public string ChallengePrice
        {
            get => challengePrice;
            set => SetProperty(ref challengePrice, value);
        }

        private string challengeParticipationMessage = "";
        public string ChallengeParticipationMessage
        {
            get => challengeParticipationMessage;
            set => SetProperty(ref challengeParticipationMessage, value);
        }

        private string challengersTitlesRaw = "";
        public string ChallengersTitlesRaw
        {
            get => challengersTitlesRaw;
            set => SetProperty(ref challengersTitlesRaw, value);
        }

        private int challengerNumber = 2;
        public int ChallengerNumber
        {
            get => challengerNumber;
            set => SetProperty(ref challengerNumber, value);
        }

        private string currentTournamentName = "Saison 1";
        public string CurrentTournamentName
        {
            get => currentTournamentName;
            set => SetProperty(ref currentTournamentName, value);
        }

        #endregion

        #region Partie affichage

        private int fontSize = 14;
        public int FontSize
        {
            get => fontSize;
            set => SetProperty(ref fontSize, value);
        }

        private string _pathImageHeading = "";
        public string PathImageHeading
        {
            get => _pathImageHeading;
            set => SetProperty(ref _pathImageHeading, value);
        }

        private int imageHeadingHeight = 650;
        public int ImageHeadingHeight
        {
            get => imageHeadingHeight;
            set => SetProperty(ref imageHeadingHeight, value);
        }

        private string pathImageFooter = "";
        public string PathImageFooter
        {
            get => pathImageFooter;
            set => SetProperty(ref pathImageFooter, value);
        }

        #endregion

        #region Partie email

        private string senderEmail = "";
        public string SenderEmail
        {
            get => senderEmail;
            set => SetProperty(ref senderEmail, value);
        }

        private bool isTest;
        public bool IsTest
        {
            get => isTest;
            set => SetProperty(ref isTest, value);
        }

        private string testerEmail = "";
        public string TesterEmail
        {
            get => testerEmail;
            set => SetProperty(ref testerEmail, value);
        }

        private string subjectMailChallenger = "";
        public string SubjectMailChallenger
        {
            get => subjectMailChallenger;
            set => SetProperty(ref subjectMailChallenger, value);
        }

        private string subjectMailEater = "";
        public string SubjectMailEater
        {
            get => subjectMailEater;
            set => SetProperty(ref subjectMailEater, value);
        }

        #endregion

        #region Participants

        /// <summary>
        /// Liste observable des participants.
        /// Le DataGridView peut s'y connecter directement.
        /// </summary>
        private BindingList<Participant> participants = new();

        public BindingList<Participant> Participants
        {
            get => participants;
            set => SetProperty(ref participants, value);
        }

        #endregion

        #region Conversion Config

        public static MainFormViewModel FromConfig(AppConfig config)
        {
            return new MainFormViewModel
            {
                ChallengeDate = DateTime.TryParse(config.ChallengeDate, out var date)
                    ? date
                    : DateTime.Today,

                ChallengeHour = string.IsNullOrWhiteSpace(config.ChallengeHour)? "09:00": config.ChallengeHour,
                ChallengeRoom = config.ChallengeRoom,
                ChallengeTheme = config.ChallengeTheme,
                ChallengeRules = config.ChallengeRules,
                ChallengePrice = config.ChallengePrice,
                ChallengeParticipationMessage = config.ChallengeParticipationMessage,
                CurrentTournamentName = string.IsNullOrWhiteSpace(config.CurrentTournamentName) ? "Saison 1" : config.CurrentTournamentName,
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

                Participants = new BindingList<Participant>(config.Participants ?? new())
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
                CurrentTournamentName = CurrentTournamentName,
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

                Participants = Participants.ToList()
            };
        }

        #endregion
        public void LoadFrom(MainFormViewModel source)
        {
            ChallengeDate = source.ChallengeDate;
            ChallengeHour = source.ChallengeHour;
            ChallengeRoom = source.ChallengeRoom;
            ChallengeTheme = source.ChallengeTheme;
            ChallengeRules = source.ChallengeRules;
            ChallengePrice = source.ChallengePrice;
            ChallengeParticipationMessage = source.ChallengeParticipationMessage;
            CurrentTournamentName = source.CurrentTournamentName;
            ChallengersTitlesRaw = source.ChallengersTitlesRaw;
            ChallengerNumber = source.ChallengerNumber;

            FontSize = source.FontSize;
            PathImageHeading = source.PathImageHeading;
            ImageHeadingHeight = source.ImageHeadingHeight;
            PathImageFooter = source.PathImageFooter;

            SenderEmail = source.SenderEmail;
            IsTest = source.IsTest;
            TesterEmail = source.TesterEmail;
            SubjectMailChallenger = source.SubjectMailChallenger;
            SubjectMailEater = source.SubjectMailEater;

            Participants.Clear();
            foreach (var p in source.Participants)
            {
                Participants.Add(new Participant
                {
                    Name = p.Name,
                    Email = p.Email,
                    IsEligible = p.IsEligible
                });
            }
        }
    }
}