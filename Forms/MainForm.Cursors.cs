using DuelDeGateaux.Services;
using System.Windows.Forms;

namespace DuelDeGateaux.Forms
{
    /// <summary>
    /// Partie dédiée à la personnalisation des curseurs.
    /// </summary>
    public partial class MainForm
    {
        /// <summary>
        /// Applique automatiquement tous les curseurs personnalisés (Rouleau, Muffin, Gousse)
        /// à l'ensemble de la fenêtre et de ses contrôles internes.
        /// </summary>
        private void SetupCustomCursors()
        {
            CursorService.ApplyCursorsToForm(this);
        }
    }
}