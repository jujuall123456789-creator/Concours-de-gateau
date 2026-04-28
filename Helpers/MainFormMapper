using DuelDeGateaux.Forms;
using DuelDeGateaux.Models;

namespace DuelDeGateaux.Helpers
{
    /// <summary>
    /// Gère le mapping entre l'interface utilisateur et la configuration métier.
    /// </summary>
    public static class MainFormMapper
    {
        #region Public API

        public static void FillForm(MainForm form, AppConfig config)
        {
            FillContestSection(form, config);
            FillDisplaySection(form, config);
            FillMailSection(form, config);
            FillParticipantsSection(form, config);
        }

        public static void FillConfig(MainForm form, AppConfig config)
        {
            FillContestConfig(form, config);
            FillDisplayConfig(form, config);
            FillMailConfig(form, config);
            FillParticipantsConfig(form, config);
        }

        #endregion

        #region Contest

        private static void FillContestSection(MainForm form, AppConfig config)
        {
            if (DateTime.TryParse(config.ChallengeDate, out DateTime date))
                form.datePicker.Value = date;

            if (DateTime.TryParseExact(config.ChallengeHour, "HH:mm", null,
                System.Globalization.DateTimeStyles.None, out DateTime time))
                form.timePicker.Value = time;

            form.txtRoom.Text = config.ChallengeRoom;
            form.txtTheme.Text = config.ChallengeTheme;
            form.txtRules.Text = config.ChallengeRules;
            form.txtPrice.Text = config.ChallengePrice;
            form.txtParticipation.Text = config.ChallengeParticipationMessage;
            form.txtTitles.Text = string.Join(",", config.ChallengersTitles ?? new());

            form.rb2Challengers.Checked = config.ChallengerNumber == 2;
            form.rb3Challengers.Checked = config.ChallengerNumber == 3;
        }

        private static void FillContestConfig(MainForm form, AppConfig config)
        {
            config.ChallengeDate = form.datePicker.Value.ToString("dd-MM-yyyy");
            config.ChallengeHour = form.timePicker.Value.ToString("HH:mm");

            config.ChallengeRoom = form.txtRoom.Text.Trim();
            config.ChallengeTheme = form.txtTheme.Text.Trim();
            config.ChallengeRules = form.txtRules.Text.Trim();
            config.ChallengePrice = form.txtPrice.Text.Trim();
            config.ChallengeParticipationMessage = form.txtParticipation.Text.Trim();

            config.ChallengersTitles = form.txtTitles.Text
                .Split(',', StringSplitOptions.RemoveEmptyEntries)
                .Select(t => t.Trim())
                .Where(t => !string.IsNullOrWhiteSpace(t))
                .ToList();

            config.ChallengerNumber = form.rb2Challengers.Checked ? 2 : 3;
        }

        #endregion

        #region Display

        private static void FillDisplaySection(MainForm form, AppConfig config)
        {
            form.numFontSize.Value = UiHelper.ClampNumeric(
                config.FontSize,
                form.numFontSize.Minimum,
                form.numFontSize.Maximum);

            form.txtImageHeader.Text = config.PathImageHeading;
            form.numImageHeight.Value = UiHelper.ClampNumeric(
                config.ImageHeadingHeight,
                form.numImageHeight.Minimum,
                form.numImageHeight.Maximum);

            form.txtImageFooter.Text = config.PathImageFooter;
        }

        private static void FillDisplayConfig(MainForm form, AppConfig config)
        {
            config.FontSize = (int)form.numFontSize.Value;
            config.PathImageHeading = form.txtImageHeader.Text.Trim();
            config.ImageHeadingHeight = (int)form.numImageHeight.Value;
            config.PathImageFooter = form.txtImageFooter.Text.Trim();
        }

        #endregion

        #region Mail

        private static void FillMailSection(MainForm form, AppConfig config)
        {
            form.txtSender.Text = config.SenderEmail;
            form.chkTest.Checked = config.IsTest;
            form.txtTestMail.Text = config.TesterEmail;
            form.txtSubjectChallenger.Text = config.SubjectMailChallenger;
            form.txtSubjectEater.Text = config.SubjectMailEater;
        }

        private static void FillMailConfig(MainForm form, AppConfig config)
        {
            config.SenderEmail = form.txtSender.Text.Trim();
            config.IsTest = form.chkTest.Checked;
            config.TesterEmail = form.txtTestMail.Text.Trim();
            config.SubjectMailChallenger = form.txtSubjectChallenger.Text.Trim();
            config.SubjectMailEater = form.txtSubjectEater.Text.Trim();
        }

        #endregion

        #region Participants

        private static void FillParticipantsSection(MainForm form, AppConfig config)
        {
            form.participantBindingSource.DataSource = config.Participants;
            form.dgvParticipants.DataSource = form.participantBindingSource;
            form.participantBindingSource.ResetBindings(false);
        }

        private static void FillParticipantsConfig(MainForm form, AppConfig config)
        {
            form.dgvParticipants.EndEdit();
            form.participantBindingSource.EndEdit();
        }

        #endregion
    }
}
