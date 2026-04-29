namespace DuelDeGateaux.Helpers
{
    /// <summary>
    /// Outils utilitaires pour l'interface utilisateur.
    /// </summary>
    public static class UiHelper
    {
        /// <summary>
        /// Méthode commune pour gérer les erreurs lors de l'exécution des actions
        /// </summary>
        /// <param name="action">Action à exécuter</param>
        /// <param name="successMessage">Message de succès</param>
        public static bool ExecuteWithErrorHandling(Action action, string? successMessage = null)
        {
            try
            {
                action();
                if (!string.IsNullOrWhiteSpace(successMessage))
                    MessageBox.Show(successMessage);

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Une erreur est survenue : {ex.Message}", "Erreur",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }
    }
}