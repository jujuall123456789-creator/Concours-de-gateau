using DuelDeGateaux.UI;
namespace DuelDeGateaux.Forms
{
    /// <summary>
    /// Partie dédiée à la validation du formulaire.
    /// </summary>
    public partial class MainForm
    {
        private readonly Dictionary<string, Control> validationControls;

        /// <summary>
        /// Valide tous les champs du formulaire et signale les erreurs.
        /// Cette méthode vérifie que tous les champs obligatoires sont remplis
        /// et que les adresses e-mail sont valides. Les champs invalides sont
        /// mis en évidence en les coloriant en rouge, et un message d'erreur est
        /// affiché si des erreurs sont trouvées.
        /// </summary>
        /// <returns>True si tous les champs sont valides, False sinon.</returns>
        private bool ValidateFields()
        {
            EndEditParticipants();
            var result = FormValidationService.Validate(viewModel);

            ValidationUiService.ResetFieldColors(validationControls);

            if (!result.IsValid)
            {
                ValidationUiService.ApplyFieldErrors(result, validationControls);
                 string message = ValidationUiService.BuildValidationMessage(result,_messageRandomizer); 
                CustomMessageBox.Show(message, "Validation impossible", MessageBoxButtons.OK,MessageBoxIcon.Warning);
                return false;
            }
            return true;
        }
    }
}