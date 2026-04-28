namespace DuelDeGateaux.Services
{
    /// <summary>
    /// Service d'initialisation des tooltips.
    /// </summary>
    public static class TooltipService
    {
        public static ToolTip BuildDefault()
        {
            return new ToolTip
            {
                AutoPopDelay = 50000,// durée d'affichage
                InitialDelay = 500, // délai avant affichage
                ReshowDelay = 200, // délai entre deux affichages
                ShowAlways = true // s'affiche même si la fenêtre n'est pas active
            };
        }
    }
}