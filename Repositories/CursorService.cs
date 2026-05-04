using DuelDeGateaux.Services;
using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace DuelDeGateaux.Services // (Modifié de Repositories à Services pour la propreté)
{
    public static class CursorService
    {
        // ==========================================
        // 📁 CONSTANTES : NOMS DES FICHIERS
        // ==========================================
        private const string MainCursorFileName = "rollingPin.cur";
        private const string ButtonCursorFileName = "muffin.cur";
        private const string TextCursorFileName = "gousse.cur";

        // Importation de l'API Windows
        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        private static extern IntPtr LoadCursorFromFile(string path);

        // ==========================================
        // 🧠 MISE EN CACHE (Pour éviter les fuites de mémoire !)
        // ==========================================
        private static Cursor? _mainCursor;
        private static Cursor? _buttonCursor;
        private static Cursor? _textCursor;

        // Propriétés publiques qui chargent le curseur à la volée, puis le gardent en mémoire.
        public static Cursor MainCursor => _mainCursor ??= (LoadCursor(MainCursorFileName) ?? Cursors.Default);
        public static Cursor ButtonCursor => _buttonCursor ??= (LoadCursor(ButtonCursorFileName) ?? Cursors.Hand);
        public static Cursor TextCursor => _textCursor ??= (LoadCursor(TextCursorFileName) ?? Cursors.IBeam);

        // ==========================================
        // 🛠️ MÉTHODES INTERNES
        // ==========================================
        private static Cursor? LoadCursor(string fileName)
        {
            try
            {
                var path = FileSelectionService.FilePathAssets(fileName);
                if (File.Exists(path))
                {
                    IntPtr colorCursorHandle = LoadCursorFromFile(path);
                    if (colorCursorHandle != IntPtr.Zero)
                        return new Cursor(colorCursorHandle);
                }
            }
            catch { }
            return null;
        }

        // ==========================================
        // 🌐 MÉTHODES POUR LE WEBVIEW2 (HTML/CSS)
        // ==========================================
        public static string GetCssMainCursor() => GetCssCursorBase64(MainCursorFileName, "auto");
        public static string GetCssButtonCursor() => GetCssCursorBase64(ButtonCursorFileName, "pointer");

        private static string GetCssCursorBase64(string fileName, string fallbackCss)
        {
            try
            {
                string path = FileSelectionService.FilePathAssets(fileName);
                if (File.Exists(path))
                {
                    byte[] bytes = File.ReadAllBytes(path);
                    string base64 = Convert.ToBase64String(bytes);
                    return $"url(data:image/x-icon;base64,{base64}), {fallbackCss}";
                }
            }
            catch { }
            return fallbackCss;
        }

        // ==========================================
        // 🪄 APPLICATION AUTOMATIQUE AUX FORMULAIRES
        // ==========================================
        /// <summary>
        /// Fonction récursive qui scanne toute une fenêtre et applique le bon curseur 
        /// (Muffin, Gousse, Rouleau) selon le type de contrôle automatiquement.
        /// </summary>
        public static void ApplyCursorsToForm(Control parent)
        {
            // Si c'est la fenêtre principale elle-même
            if (parent is Form f) f.Cursor = MainCursor;

            foreach (Control ctrl in parent.Controls)
            {
                // Attribution selon le type
                if (ctrl is Button)
                {
                    ctrl.Cursor = ButtonCursor;
                }
                else if (ctrl is TextBox)
                {
                    ctrl.Cursor = TextCursor;
                }
                else if (ctrl is ComboBox || ctrl is DateTimePicker)
                {
                    ctrl.Cursor = MainCursor;
                }
                else if (ctrl is NumericUpDown num)
                {
                    num.Cursor = MainCursor;
                    foreach (Control child in num.Controls)
                    {
                        child.Cursor = child is TextBox ? TextCursor : MainCursor;
                    }
                }
                
                // Si le contrôle contient d'autres contrôles (Panel, GroupBox...), on continue la fouille
                if (ctrl.HasChildren)
                {
                    ApplyCursorsToForm(ctrl);
                }
            }
        }
    }
}