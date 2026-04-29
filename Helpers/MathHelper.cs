namespace DuelDeGateaux.Helpers
{
    /// <summary>
    /// Outils utilitaires pour l'interface utilisateur.
    /// </summary>
    public static class MathHelper
    {
        /// <summary>
        /// Retourne une valeur bornée entre un minimum et un maximum.
        /// </summary>
        public static decimal ClampNumeric(decimal value, decimal min, decimal max)
        {
            return Math.Max(min, Math.Min(max, value));
        }
    }
}