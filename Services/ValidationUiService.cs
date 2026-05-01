using DuelDeGateaux.Models;
using System.Text;

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
                switch (ctrl)
                {
                    case TextBox:
                    case NumericUpDown:
                        ctrl.BackColor = SystemColors.Window;
                        break;

                    case PictureBox:
                        ctrl.BackColor = Color.Transparent;
                        break;

                    default:
                        ctrl.BackColor = SystemColors.Control;
                        break;
                }
            }
        }

        /// <summary>
        /// Applique une coloration visuelle aux champs invalides
        /// en fonction des erreurs retournées par la validation métier.
        /// </summary>
        public static void ApplyFieldErrors(ValidationResult result,Dictionary<string, Control> controls)
        {
            foreach (var key in result.Errors.Keys)
            {
                if (!controls.TryGetValue(key, out var ctrl))
                    continue;

                switch (ctrl)
                {
                    case DataGridView dgv:
                        dgv.BackgroundColor = Color.LightPink;
                        break;

                    case PictureBox pb:
                        pb.BackColor = Color.MistyRose;
                        break;

                    default:
                        ctrl.BackColor = Color.LightPink;
                        break;
                }
            }
        }

        /// <summary>
        /// Génère un message lisible regroupant toutes les erreurs
        /// afin d'informer l'utilisateur des corrections à effectuer.
        /// </summary>
        public static string BuildValidationMessage(ValidationResult result,Random randomizer)
        {
            var sb = new StringBuilder();

            sb.AppendLine("Le formulaire contient des erreurs :");
            sb.AppendLine();

            foreach (var error in result.Errors.Values)
            {
                sb.AppendLine("• " + error);
            }

            string[] funMessages =
            {
                "⚠️ Oups, il manque des infos ! On se réveille ☕",
                "⚠️ Faut remplir les cases en rouge, NEUNEU 😤",
                "⚠️ Un gâteau sans farine, ça ne marche pas. Un formulaire vide non plus 🍰",
                "⚠️ Allez, on se concentre et on corrige les cases rouges 🎯"
            };
            sb.AppendLine();
            sb.AppendLine(funMessages[randomizer.Next(funMessages.Length)]);

            return sb.ToString();
        }
    }
}