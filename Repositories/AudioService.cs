using DuelDeGateaux.Services;
using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace DuelDeGateaux.Repositories
{
    internal static class AudioService
    {
        // 📁 Chemin du son SendSoundName 
        private const string SendSoundFileName = "OvenSound.mp3";

        // 📁 Chemin du son d'ambiance 
        private const string MusicName = "ShortChillMusic.mp3";

        // Importation de la fonction magique de Windows pour lire les MP3
        [DllImport("winmm.dll")]
        private static extern long mciSendString(string command, StringBuilder returnValue, int returnLength, nint winHandle);

        /// <summary>
        /// Lance la musique de fond en boucle.
        /// </summary>
        public static void PlayBackgroundMusic()
        {
            try
            {
                string path = FileSelectionService.FilePathAssets(MusicName);
                if (File.Exists(path))
                {
                    // Ouvre le fichier et lui donne le pseudo "bgmusic"
                    mciSendString($"open \"{path}\" type mpegvideo alias bgmusic", null, 0, nint.Zero);
                    // Joue la musique avec l'option "repeat" pour boucler Ã  l'infini
                    mciSendString("play bgmusic repeat", null, 0, nint.Zero);
                }
            }
            catch { /* On ignore si pas de carte son ou fichier manquant */ }
        }

        /// <summary>
        /// Joue un effet sonore ponctuel (se superpose Ã  la musique).
        /// </summary>
        public static void PlaySendSound()
        {
            try
            {
                string path = FileSelectionService.FilePathAssets(SendSoundFileName);
                if (File.Exists(path))
                {
                    // Ferme le son s'il Ã©tait dÃ©jÃ  en train d'Ãªtre jouÃ©
                    mciSendString("close sfx", null, 0, nint.Zero);
                    // Ouvre le fichier
                    mciSendString($"open \"{path}\" type mpegvideo alias sfx", null, 0, nint.Zero);
                    // Joue le son une fois
                    mciSendString("play sfx", null, 0, nint.Zero);
                }
            }
            catch { }
        }

        /// <summary>
        /// ArrÃªte la musique de fond (utile si tu veux un bouton Mute un jour).
        /// </summary>
        public static void StopBackgroundMusic()
        {
            mciSendString("close bgmusic", null, 0, nint.Zero);
        }
    }
}