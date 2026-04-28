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
                AutoPopDelay = 50000,
                InitialDelay = 500,
                ReshowDelay = 200,
                ShowAlways = true
            };
        }
    }
}