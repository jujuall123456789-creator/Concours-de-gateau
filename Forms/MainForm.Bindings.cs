namespace DuelDeGateaux.Forms
{
    /// <summary>
    /// Partie dédiée aux bindings et synchronisation UI ↔ ViewModel.
    /// </summary>
    public partial class MainForm
    {
        private bool bindingsInitialized;
        /// <summary>
        /// Configure les liaisons entre les contrôles UI et le ViewModel.
        /// </summary>
        private void SetupBindings()
        {
            if (bindingsInitialized)
                return;

            bindingsInitialized = true;
            txtTheme.DataBindings.Add("Text", viewModel, nameof(viewModel.ChallengeTheme));
            txtRoom.DataBindings.Add("Text", viewModel, nameof(viewModel.ChallengeRoom));
            txtRules.DataBindings.Add("Text", viewModel, nameof(viewModel.ChallengeRules));
            txtPrice.DataBindings.Add("Text", viewModel, nameof(viewModel.ChallengePrice));
            txtParticipation.DataBindings.Add("Text", viewModel, nameof(viewModel.ChallengeParticipationMessage));
            txtTitles.DataBindings.Add("Text", viewModel, nameof(viewModel.ChallengersTitlesRaw));

            timePicker.DataBindings.Add("Value", viewModel, nameof(viewModel.ChallengeTime));
            datePicker.DataBindings.Add("Value", viewModel, nameof(viewModel.ChallengeDate));

            txtSender.DataBindings.Add("Text", viewModel, nameof(viewModel.SenderEmail));
            txtTestMail.DataBindings.Add("Text", viewModel, nameof(viewModel.TesterEmail));

            txtSubjectChallenger.DataBindings.Add("Text", viewModel, nameof(viewModel.SubjectMailChallenger));
            txtSubjectEater.DataBindings.Add("Text", viewModel, nameof(viewModel.SubjectMailEater));

            chkTest.DataBindings.Add("Checked", viewModel, nameof(viewModel.IsTest));

            numFontSize.DataBindings.Add("Value", viewModel, nameof(viewModel.FontSize));
            numImageHeight.DataBindings.Add("Value", viewModel, nameof(viewModel.ImageHeadingHeight));
            txtImageHeader.DataBindings.Add("Text", viewModel, nameof(viewModel.PathImageHeading));
            txtImageFooter.DataBindings.Add("Text", viewModel, nameof(viewModel.PathImageFooter));
        }        

        /// <summary>
        /// Configure la grille des participants.
        /// </summary>
        private void SetupParticipantsGrid()
        {
            if (dgvParticipants.DataSource != null)
                return;
            participantBindingSource.DataSource = viewModel.Participants;
            dgvParticipants.DataSource = participantBindingSource;
        }

        /// <summary>
        /// Force la fin d'édition de l'utilisateur afin de récupérer sa dernière saisie
        /// </summary>
        private void EndEditParticipants()
        {
            dgvParticipants.EndEdit();
            participantBindingSource.EndEdit();
        }
    }
}