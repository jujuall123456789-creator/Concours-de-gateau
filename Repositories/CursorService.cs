using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace DuelDeGateaux.Services
{
    internal static class CursorService
    {
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
                string cursorPath = Path.Combine(Application.StartupPath, "Assets", cursorFileName);

                if (File.Exists(cursorPath))
                {
                    IntPtr colorCursorHandle = LoadCursorFromFile(cursorPath);

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
