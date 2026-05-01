using DuelDeGateaux.Forms;
using DuelDeGateaux.ViewModels;
namespace DuelDeGateaux.UI
{
    /// <summary>
    /// Définit la correspondance entre clés de validation et contrôles.
    /// </summary>
    public static class ValidationDefinitions
    {
        public static Dictionary<string, Control> Build(MainForm form)
        {
            return new()
            {
                [nameof(MainFormViewModel.ChallengeTheme)] = form.ThemeControl,
                [nameof(MainFormViewModel.ChallengeRoom)] = form.RoomControl,
                [nameof(MainFormViewModel.ChallengeRules)] = form.RulesControl,
                [nameof(MainFormViewModel.ChallengePrice)] = form.PriceControl,
                [nameof(MainFormViewModel.ChallengeParticipationMessage)] = form.ParticipationControl,
                [nameof(MainFormViewModel.ChallengersTitlesRaw)] = form.TitlesControl,
                [nameof(MainFormViewModel.ChallengeDate)] = form.DatePickerControl,
                [nameof(MainFormViewModel.ChallengeHour)] = form.TimePickerControl,
                [nameof(MainFormViewModel.SenderEmail)] = form.SenderControl,
                [nameof(MainFormViewModel.TesterEmail)] = form.TestMailControl,
                [nameof(MainFormViewModel.SubjectMailChallenger)] = form.SubjectChallengerControl,
                [nameof(MainFormViewModel.SubjectMailEater)] = form.SubjectEaterControl,
                [nameof(MainFormViewModel.Participants)] = form.ParticipantsGridControl,
                [nameof(MainFormViewModel.PathImageHeading)] = form.HeaderImageControl,
                [nameof(MainFormViewModel.PathImageFooter)] = form.FooterImageControl
            };
        }
    }
}