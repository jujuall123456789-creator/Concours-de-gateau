namespace DuelDeGateaux.Services
{
    /// <summary>
    /// Service chargé de configurer les tooltips
    /// de l'interface utilisateur.
    /// </summary>
    public static class TooltipUiService
    {
        /// <summary>
        /// Applique toutes les définitions de tooltips
        /// sur les contrôles fournis.
        /// </summary>
        /// <param name="toolTip">Composant ToolTip utilisé par le formulaire.</param>
        /// <param name="definitions">
        /// Association entre un contrôle et le texte d'aide à afficher.
        /// </param>
        public static void Configure(
            ToolTip toolTip,
            Dictionary<Control, string> definitions)
        {
            foreach (var pair in definitions)
            {
                toolTip.SetToolTip(pair.Key, pair.Value);
            }
        }
    }
}