using DuelDeGateaux.Contracts;
using DuelDeGateaux.Helpers;
using DuelDeGateaux.ViewModels;

namespace DuelDeGateaux.Mappers
{
    /// <summary>
    /// Cette classe fait le lien entre :
    /// - la vue (le formulaire affiché à l'utilisateur)
    /// - le ViewModel (les données manipulées par l'application)
    ///
    /// Son rôle est de copier les données dans les deux sens.
    ///
    /// Pourquoi ?
    /// Pour éviter que le formulaire gère lui-même toute la logique.
    /// Cela rend le code plus propre, plus lisible et plus facile à maintenir.
    /// </summary>
    internal static class MainFormMapper
    {
        #region Méthodes publiques

        /// <summary>
        /// Remplit l'interface utilisateur avec les données du ViewModel.
        ///
        /// Exemple :
        /// vm.ChallengeTheme → champ texte du formulaire
        /// </summary>
        public static void PopulateView(IMainFormView view, MainFormViewModel vm)
        {
            MapContestToView(view, vm);
            MapDisplayToView(view, vm);
            MapMailToView(view, vm);
            MapParticipantsToView(view, vm);
        }

        /// <summary>
        /// Récupère les valeurs saisies dans l'interface
        /// pour mettre à jour le ViewModel.
        ///
        /// Exemple :
        /// champ texte du formulaire → vm.ChallengeTheme
        /// </summary>
        public static void UpdateViewModel(IMainFormView view, MainFormViewModel vm)
        {
            MapContestFromView(view, vm);
            MapDisplayFromView(view, vm);
            MapMailFromView(view, vm);
            MapParticipantsFromView(view, vm);
        }

        #endregion

        #region Partie concours

        /// <summary>
        /// Copie les données du concours du ViewModel vers la vue.
        /// </summary>
        private static void MapContestToView(IMainFormView view, MainFormViewModel vm)
        {
            view.ChallengeDate = vm.ChallengeDate;
            view.ChallengeHour = vm.ChallengeHour;
            view.ChallengeRoom = vm.ChallengeRoom;
            view.ChallengeTheme = vm.ChallengeTheme;
            view.ChallengeRules = vm.ChallengeRules;
            view.ChallengePrice = vm.ChallengePrice;
            view.ChallengeParticipationMessage = vm.ChallengeParticipationMessage;
            view.ChallengersTitlesRaw = vm.ChallengersTitlesRaw;
            view.ChallengerNumber = vm.ChallengerNumber;
        }

        /// <summary>
        /// Copie les données du formulaire vers le ViewModel.
        /// </summary>
        private static void MapContestFromView(IMainFormView view, MainFormViewModel vm)
        {
            vm.ChallengeDate = view.ChallengeDate;
            vm.ChallengeHour = view.ChallengeHour;
            vm.ChallengeRoom = view.ChallengeRoom;
            vm.ChallengeTheme = view.ChallengeTheme;
            vm.ChallengeRules = view.ChallengeRules;
            vm.ChallengePrice = view.ChallengePrice;
            vm.ChallengeParticipationMessage = view.ChallengeParticipationMessage;
            vm.ChallengersTitlesRaw = view.ChallengersTitlesRaw;
            vm.ChallengerNumber = view.ChallengerNumber;
        }

        #endregion

        #region Partie affichage

        /// <summary>
        /// Copie les paramètres visuels du ViewModel vers la vue.
        /// </summary>
        private static void MapDisplayToView(IMainFormView view, MainFormViewModel vm)
        {
            view.PathImageHeading = vm.PathImageHeading;
            view.PathImageFooter = vm.PathImageFooter;
            view.FontSize = (int)MathHelper.ClampNumeric(vm.FontSize, view.FontSizeMinimum, view.FontSizeMaximum);
            view.ImageHeadingHeight = (int)MathHelper.ClampNumeric(vm.ImageHeadingHeight, view.ImageHeightMinimum, view.ImageHeightMaximum);
            view.SetHeaderPreview(vm.PathImageHeading);
            view.SetFooterPreview(vm.PathImageFooter);
        }

        /// <summary>
        /// Copie les paramètres visuels de la vue vers le ViewModel.
        /// </summary>
        private static void MapDisplayFromView(IMainFormView view, MainFormViewModel vm)
        {
            vm.FontSize = view.FontSize;
            vm.PathImageHeading = view.PathImageHeading;
            vm.ImageHeadingHeight = view.ImageHeadingHeight;
            vm.PathImageFooter = view.PathImageFooter;
        }

        #endregion

        #region Partie email

        /// <summary>
        /// Copie les paramètres email du ViewModel vers la vue.
        /// </summary>
        private static void MapMailToView(IMainFormView view, MainFormViewModel vm)
        {
            view.SenderEmail = vm.SenderEmail;
            view.IsTest = vm.IsTest;
            view.TesterEmail = vm.TesterEmail;
            view.SubjectMailChallenger = vm.SubjectMailChallenger;
            view.SubjectMailEater = vm.SubjectMailEater;
        }

        /// <summary>
        /// Copie les paramètres email de la vue vers le ViewModel.
        /// </summary>
        private static void MapMailFromView(IMainFormView view, MainFormViewModel vm)
        {
            vm.SenderEmail = view.SenderEmail;
            vm.IsTest = view.IsTest;
            vm.TesterEmail = view.TesterEmail;
            vm.SubjectMailChallenger = view.SubjectMailChallenger;
            vm.SubjectMailEater = view.SubjectMailEater;
        }

        #endregion

        #region Partie participants

        /// <summary>
        /// Charge la liste des participants dans la vue.
        /// </summary>
        private static void MapParticipantsToView(IMainFormView view, MainFormViewModel vm)
        {
            view.Participants = vm.Participants;
        }

        /// <summary>
        /// Récupère la liste des participants depuis la vue.
        /// </summary>
        private static void MapParticipantsFromView(IMainFormView view, MainFormViewModel vm)
        {
            vm.Participants = view.Participants;
        }

        #endregion
    }
}
