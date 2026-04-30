using DuelDeGateaux.Services;
using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace DuelDeGateaux.Repositories
{
    internal static class CursorService
    {
        // ==========================================
        // 📁 CONSTANTES : NOMS DES FICHIERS
        // ==========================================
        private const string MainCursorFileName = "rollingPin.cur";
        private const string ButtonCursorFileName = "muffin.cur";
        private const string TextCursorFileName = "gousse.cur";

        // Importation de l'API Windows pour garder les couleurs d'origine du curseur
        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        private static extern IntPtr LoadCursorFromFile(string path);

        // ==========================================
        // 🛠️ MÉTHODE CENTRALE 
        // ==========================================

        /// <summary>
        /// Méthode interne pour charger un fichier curseur spécifique en qualité originale.
        /// </summary>
        /// <param name="fileName">Le nom du fichier .cur</param>
        /// <returns>Le Cursor personnalisé, ou null si introuvable/erreur.</returns>
        private static Cursor? LoadCursor(string fileName)
        {
            try
            {
                var path = FileSelectionService.FilePathAssets(fileName);
                if (File.Exists(path))
                {
                    IntPtr colorCursorHandle = LoadCursorFromFile(path);

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

            // Si on arrive ici, c'est que le fichier manque ou est corrompu.
            return null;
        }

        // ==========================================
        // 🎨 ACCÈS PUBLICS
        // ==========================================

        /// <summary>
        /// Charge le curseur principal de l'application (Le Rouleau à Pâtisserie).
        /// </summary>
        public static Cursor? LoadCustomCursor() => LoadCursor(MainCursorFileName);

        /// <summary>
        /// Charge le curseur de survol pour les boutons (Le Muffin).
        /// </summary>
        public static Cursor? LoadCustomButtonCursor() => LoadCursor(ButtonCursorFileName);

        /// <summary>
        /// Charge le curseur de survol pour l'édition de texte (La gousse de vanille).
        /// </summary>
        public static Cursor? LoadCustomTextCursor() => LoadCursor(TextCursorFileName);
    }
}