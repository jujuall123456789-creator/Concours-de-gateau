namespace DuelDeGateaux.Services
{
    /// <summary>
    /// Outils utilitaires pour l'interface utilisateur.
    /// </summary>
    public static class UiHelperService
    {
        public static decimal ClampNumeric(decimal value, decimal min, decimal max)
        {
            return Math.Max(min, Math.Min(max, value));
        }

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