using DuelDeGateaux.Models;
using DuelDeGateaux.ViewModels;
using System.IO;

namespace DuelDeGateaux.Services
{
    /// <summary>
    /// Service chargé de valider les données du formulaire.
    /// Retourne un objet contenant les erreurs détectées.
    /// </summary>
    public static class FormValidationService
    {
        /// <summary>
        /// Valide le contenu du ViewModel principal.
        /// </summary>
        public static ValidationResult Validate(MainFormViewModel vm)
        {
            var result = new ValidationResult();

            // =============================
            // VALIDATION TEXTE
            // =============================

            if (string.IsNullOrWhiteSpace(vm.ChallengeTheme))
                result.Errors[nameof(vm.ChallengeTheme)] = "Le thème est obligatoire.";

            if (string.IsNullOrWhiteSpace(vm.ChallengeRoom))
                result.Errors[nameof(vm.ChallengeRoom)] = "Le lieu est obligatoire.";

            if (string.IsNullOrWhiteSpace(vm.ChallengeRules))
                result.Errors[nameof(vm.ChallengeRules)] = "Les règles sont obligatoires.";

            if (string.IsNullOrWhiteSpace(vm.ChallengePrice))
                result.Errors[nameof(vm.ChallengePrice)] = "Le prix est obligatoire.";

            if (string.IsNullOrWhiteSpace(vm.ChallengeParticipationMessage))
                result.Errors[nameof(vm.ChallengeParticipationMessage)] = "Le message de participation est obligatoire.";

            if (string.IsNullOrWhiteSpace(vm.ChallengersTitlesRaw))
                result.Errors[nameof(vm.ChallengersTitlesRaw)] = "Les titres challengers sont obligatoires.";

            // =============================
            // DATE / HEURE
            // =============================

            if (vm.ChallengeDate == default)
                result.Errors[nameof(vm.ChallengeDate)] = "La date du concours est invalide.";

            if (string.IsNullOrWhiteSpace(vm.ChallengeHour))
                result.Errors[nameof(vm.ChallengeHour)] = "L'heure du concours est obligatoire.";

            // =============================
            // VALIDATION AFFICHAGE
            // =============================

            if (!string.IsNullOrWhiteSpace(vm.PathImageHeading) &&
                !File.Exists(vm.PathImageHeading))
                result.Errors[nameof(vm.PathImageHeading)] = "Image d'en-tête introuvable.";

            if (!string.IsNullOrWhiteSpace(vm.PathImageFooter) &&
                !File.Exists(vm.PathImageFooter))
                result.Errors[nameof(vm.PathImageFooter)] = "Image de pied introuvable.";

            // =============================
            // VALIDATION EMAIL
            // =============================

            if (!IsValidEmail(vm.SenderEmail))
                result.Errors[nameof(vm.SenderEmail)] = "L'adresse expéditeur est invalide.";

            if (vm.IsTest && !IsValidEmail(vm.TesterEmail))
                result.Errors[nameof(vm.TesterEmail)] = "L'adresse email de test est invalide.";

            if (string.IsNullOrWhiteSpace(vm.SubjectMailChallenger))
                result.Errors[nameof(vm.SubjectMailChallenger)] = "Le sujet challenger est obligatoire.";

            if (string.IsNullOrWhiteSpace(vm.SubjectMailEater))
                result.Errors[nameof(vm.SubjectMailEater)] = "Le sujet jury est obligatoire.";

            // =============================
            // VALIDATION PARTICIPANTS
            // =============================

            int requiredChallengers = vm.ChallengerNumber;
            int eligibleCount = vm.Participants.Count(p => p.IsEligible);

            if (!ParticipantService.HasEnoughEligible(vm.Participants.ToList(), requiredChallengers))
            {
                result.Errors[nameof(vm.Participants)] =
                    $"Participants insuffisants : {eligibleCount}/{requiredChallengers} challengers cochés. Où sont les cuisiniers ?";
            }

            return result;
        }

        /// <summary>
        /// Vérifie qu'une adresse email respecte un format valide.
        /// </summary>
        public static bool IsValidEmail(string email)
        {
            try
            {
                return new System.Net.Mail.MailAddress(email).Address == email;
            }
            catch
            {
                return false;
            }
        }
    }
}