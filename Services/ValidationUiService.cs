using DuelDeGateaux.Models;

namespace DuelDeGateaux.Services
{
    /// <summary>
    /// Service chargé d'afficher visuellement les erreurs
    /// de validation dans l'interface utilisateur.
    /// </summary>
    public static class ValidationUiService
    {
        /// <summary>
        /// Réinitialise les couleurs des champs contrôlés.
        /// </summary>
        public static void ResetFieldColors(Dictionary<string, Control> controls)
        {
            foreach (var ctrl in controls.Values)
            {
                if (ctrl is TextBox or NumericUpDown)
                    ctrl.BackColor = SystemColors.Window;
                else
                    ctrl.BackColor = SystemColors.Control;
            }
        }

        /// <summary>
        /// Applique une coloration visuelle aux champs invalides
        /// en fonction des erreurs retournées par la validation métier.
        /// </summary>
        public static void ApplyFieldErrors(
            ValidationResult result,
            Dictionary<string, Control> controls)
        {
            foreach (var key in result.Errors.Keys)
            {
                if (controls.TryGetValue(key, out var ctrl))
                    ctrl.BackColor = Color.LightPink;
            }
        }

        /// <summary>
        /// Génère un message lisible regroupant toutes les erreurs
        /// afin d'informer l'utilisateur des corrections à effectuer.
        /// </summary>
        public static string BuildValidationMessage(
            ValidationResult result,
            Random randomizer)
        {
            string message = "Le formulaire contient des erreurs :\n\n";

            foreach (var error in result.Errors.Values)
            {
                message += "• " + error + "\n";
            }

            string[] funMessages =
            {
                "⚠️ Oups, il manque des infos ! On se réveille ☕",
                "⚠️ Faut remplir les cases en rouge, NEUNEU 😤",
                "⚠️ Un gâteau sans farine, ça ne marche pas. Un formulaire vide non plus 🍰",
                "⚠️ Allez, on se concentre et on corrige les cases rouges 🎯"
            };

            message += "\n" + funMessages[randomizer.Next(funMessages.Length)];

            return message;
        }
    }
}