using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace DuelDeGateaux.Services
{
    public static class IconService
    {
        // 🧠 Mise en cache de l'icône pour ne la charger qu'une seule fois
        private static Icon? _appIcon;

        public static Icon AppIcon
        {
            get
            {
                // Si on a déjà chargé l'icône, on la renvoie directement
                if (_appIcon != null) return _appIcon;

                string cakeIconPath = FileSelectionService.FilePathAssets("cakeIcon.ico");
                if (File.Exists(cakeIconPath))
                {
                    // L'instanciation directe avec "new Icon" est plus performante pour un vrai fichier .ico 
                    // que ExtractAssociatedIcon (qui sert plutôt à extraire l'icône d'un .exe)
                    _appIcon = new Icon(cakeIconPath);
                }
                else
                {
                    // Secours : l'icône par défaut de l'application si le gâteau a disparu
                    _appIcon = SystemIcons.Application;
                }

                return _appIcon;
            }
        }

        /// <summary>
        /// Applique l'icône du gâteau au formulaire spécifié.
        /// </summary>
        public static void ApplyIconToForm(Form form)
        {
            if (form != null)
            {
                form.Icon = AppIcon;
            }
        }
    }
}