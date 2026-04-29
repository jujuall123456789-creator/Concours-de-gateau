using DuelDeGateaux.Services;
using System.Runtime.InteropServices;

namespace DuelDeGateaux.Repositories
{
    internal static class CursorService
    {
        // 📁 Chemin du fichier .cur (dans dossier exe)
        private const string FileName = "rollingPin.cur";

        /// <summary>
        /// Chemin du fichier de configuration JSON
        /// Gère le décalage de dossier en mode Debug pour Visual Studio.
        /// </summary>
        private static readonly string ConfigPath = FileSelectionService.FilePathAssets(FileName);

        // Importation de l'API Windows pour garder les couleurs d'origine du curseur
        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        private static extern IntPtr LoadCursorFromFile(string path);

        /// <summary>
        /// Tente de charger un curseur personnalisé en couleurs depuis le dossier Assets.
        /// </summary>
        /// <param name="cursorFileName">Le nom du fichier curseur (ex: "rouleau.cur")</param>
        /// <returns>Le Cursor personnalisé, ou null si introuvable/erreur.</returns>
        public static Cursor? LoadCustomCursor(string cursorFileName)
        {
            try
            {

                if (File.Exists(ConfigPath))
                {
                    IntPtr colorCursorHandle = LoadCursorFromFile(ConfigPath);

                    if (colorCursorHandle != IntPtr.Zero)
                    {
                        return new Cursor(colorCursorHandle);
                    }
                }
            }
            catch
            {
                // Si ça plante, on attrape l'erreur silencieusement.
            }

            // Si on arrive ici, c'est que quelque chose a échoué. On renvoie null.
            return null;
        }
    }
}
