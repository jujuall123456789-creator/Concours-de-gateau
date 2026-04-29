using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace DuelDeGateaux.Services
{
    internal static class AudioService
    {
        // Importation de la fonction magique de Windows pour lire les MP3
        [DllImport("winmm.dll")]
        private static extern long mciSendString(string command, StringBuilder returnValue, int returnLength, IntPtr winHandle);

        /// <summary>
        /// Lance la musique de fond en boucle.
        /// </summary>
        public static void PlayBackgroundMusic()
        {
            try
            {
                string path = Path.Combine(Application.StartupPath, "Assets", "music.mp3");
                if (File.Exists(path))
                {
                    // Ouvre le fichier et lui donne le pseudo "bgmusic"
                    mciSendString($"open \"{path}\" type mpegvideo alias bgmusic", null, 0, IntPtr.Zero);
                    // Joue la musique avec l'option "repeat" pour boucler à l'infini
                    mciSendString("play bgmusic repeat", null, 0, IntPtr.Zero);
                }
            }
            catch { /* On ignore si pas de carte son ou fichier manquant */ }
        }

        /// <summary>
        /// Joue un effet sonore ponctuel (se superpose à la musique).
        /// </summary>
        public static void PlaySendSound()
        {
            try
            {
                string path = Path.Combine(Application.StartupPath, "Assets", "swoosh.mp3");
                if (File.Exists(path))
                {
                    // Ferme le son s'il était déjà en train d'être joué
                    mciSendString("close sfx", null, 0, IntPtr.Zero);
                    // Ouvre le fichier
                    mciSendString($"open \"{path}\" type mpegvideo alias sfx", null, 0, IntPtr.Zero);
                    // Joue le son une fois
                    mciSendString("play sfx", null, 0, IntPtr.Zero);
                }
            }
            catch { }
        }

        /// <summary>
        /// Arrête la musique de fond (utile si tu veux un bouton Mute un jour).
        /// </summary>
        public static void StopBackgroundMusic()
        {
            mciSendString("close bgmusic", null, 0, IntPtr.Zero);
        }
    }
}