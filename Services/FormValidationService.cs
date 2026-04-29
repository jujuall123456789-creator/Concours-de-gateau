using DuelDeGateaux.Models;

namespace DuelDeGateaux.Services
{
    public static class FormValidationService
    {
        public static ValidationResult Validate(AppConfig config)
        {
            var result = new ValidationResult();

            // =============================
            // VALIDATION TEXTE
            // =============================

            if (string.IsNullOrWhiteSpace(config.ChallengeTheme))
                result.Errors["Theme"] = "Le thème est obligatoire.";

            if (string.IsNullOrWhiteSpace(config.ChallengeRoom))
                result.Errors["Room"] = "Le lieu est obligatoire.";

            if (string.IsNullOrWhiteSpace(config.ChallengeRules))
                result.Errors["Rules"] = "Les règles sont obligatoires.";

            if (string.IsNullOrWhiteSpace(config.ChallengePrice))
                result.Errors["Price"] = "Le prix est obligatoire.";

            if (string.IsNullOrWhiteSpace(config.ChallengeParticipationMessage))
                result.Errors["Participation"] = "Le message de participation est obligatoire.";

            if (string.IsNullOrWhiteSpace(config.ChallengersTitlesRaw))
                result.Errors["Titles"] = "Les titres challengers sont obligatoires.";

            // =============================
            // DATE / HEURE
            // =============================

            if (config.ChallengeDate == default)
                result.Errors["Date"] = "La date du concours est invalide.";

            if (string.IsNullOrWhiteSpace(config.ChallengeHour))
                result.Errors["Hour"] = "L'heure du concours est obligatoire.";

            // =============================
            // VALIDATION AFFICHAGE
            // =============================

            if (!string.IsNullOrWhiteSpace(config.PathImageHeading) &&
                !File.Exists(config.PathImageHeading))
                result.Errors["ImageHeader"] = "Image d'en-tête introuvable.";

            if (!string.IsNullOrWhiteSpace(config.PathImageFooter) &&
                !File.Exists(config.PathImageFooter))
                result.Errors["ImageFooter"] = "Image de pied introuvable.";

            // =============================
            // VALIDATION EMAIL
            // =============================

            if (!IsValidEmail(config.SenderEmail))
                result.Errors["Sender"] = "L'adresse expéditeur est invalide.";

            if (config.IsTest && !IsValidEmail(config.TesterEmail))
                result.Errors["Tester"] = "L'adresse email de test est invalide.";

            if (string.IsNullOrWhiteSpace(config.SubjectMailChallenger))
                result.Errors["SubjectChallenger"] = "Le sujet challenger est obligatoire.";

            if (string.IsNullOrWhiteSpace(config.SubjectMailEater))
                result.Errors["SubjectEater"] = "Le sujet jury est obligatoire.";

            // =============================
            // VALIDATION PARTICIPANTS
            // =============================

            int requiredChallengers = config.ChallengerNumber;
            int eligibleCount = config.Participants.Count(p => p.IsEligible);

            if (!ParticipantService.HasEnoughEligible(config.Participants, requiredChallengers))
            {
                result.Errors["Participants"] =
                    $"Participants insuffisants : {eligibleCount}/{requiredChallengers} challengers cochés. Où sont les cuisiniers ?";
            }

            return result;
        }

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