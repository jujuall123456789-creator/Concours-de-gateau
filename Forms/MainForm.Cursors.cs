using DuelDeGateaux.Repositories;

namespace DuelDeGateaux.Forms
{
    /// <summary>
    /// Partie dédiée à la personnalisation des curseurs.
    /// </summary>
    public partial class MainForm
    {

        private Cursor? mainCursor;
        private Cursor? buttonCursor;
        private Cursor? textCursor;
        /// <summary>
        /// Remplace le curseur par défaut par un magnifique rouleau à pâtisserie.
        /// </summary>
        private void SetCustomCursor()
        {
            mainCursor = CursorService.LoadCustomCursor();
            if (mainCursor != null)
            {
                Cursor = mainCursor;
            }
        }
        
        /// <summary>
        /// Définit le curseur au survol pour les boutons principaux.
        /// </summary>
        private void SetButtonCursors()
        {            
            buttonCursor = CursorService.LoadCustomButtonCursor();
            Cursor actionCursor = buttonCursor ?? Cursors.Default;
            // Boutons principaux
            btnSend.Cursor = btnPreview.Cursor = btnSave.Cursor = btnPrintBallot.Cursor = btnHistory.Cursor = btnOpenJson.Cursor = actionCursor;
            // Boutons d'images et d'ajout
            btnBrowseHeader.Cursor = btnBrowseFooter.Cursor = btnAddParticipants.Cursor = actionCursor;
        }

        /// <summary>
        /// Définit le curseur personnalisé pour toutes les zones de texte.
        /// </summary>
        private void SetTextBoxCursors()
        {
            textCursor = CursorService.LoadCustomTextCursor(); 
            // On récupère le curseur principal (rouleau) pour l'appliquer sur les boutons fléchés
            Cursor fallbackMainCursor = mainCursor ?? Cursors.Default;

            if (textCursor != null)
            {
                // On passe les deux curseurs à notre méthode de scan
                ApplyCustomCursors(this, textCursor, mainCursor);
            }
        }

        /// <summary>
        /// Fonction récursive qui fouille dans tous les conteneurs pour appliquer les curseurs
        /// textuels, en gérant spécifiquement les contrôles complexes.
        /// </summary>
        private void ApplyCustomCursors(Control parent, Cursor textCursor, Cursor mainCursor)
        {
            foreach (Control ctrl in parent.Controls)
            {
                // Cas 1 : Zone de texte classique
                if (ctrl is TextBox txt)
                {
                    txt.Cursor = textCursor;
                }
                // Cas 2 : NumericUpDown (Composite : 1 TextBox + 1 bloc de flèches)
                else if (ctrl is NumericUpDown num)
                {
                    num.Cursor = mainCursor; // Curseur de base du contrôle
                    
                    // On fouille à l'intérieur du NumericUpDown
                    foreach (Control child in num.Controls)
                    {
                        if (child is TextBox)
                        {
                            child.Cursor = textCursor; // Pour la partie où on tape le texte
                        }
                        else
                        {
                            child.Cursor = mainCursor; // Pour les petites flèches (haut/bas)
                        }
                    }
                }
                // Cas 3 : DateTimePicker (Monolithique natif)
                else if (ctrl is DateTimePicker dtp)
                {
                    // Impossible de séparer la zone de texte des flèches en WinForms standard.
                    // On applique donc le rouleau (mainCursor) sur tout le composant.
                    dtp.Cursor = mainCursor; 
                }
                // Cas 4 : Conteneur générique (Panel, GroupBox...)
                else if (ctrl.HasChildren)
                {
                    ApplyCustomCursors(ctrl, textCursor, mainCursor);
                }
            }
        }
    }
}