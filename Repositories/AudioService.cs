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
        // ==========================================
        // 📁 CONSTANTES : NOMS DES FICHIERS
        // ==========================================
        private const string SendSoundFileName = "ovenSound.mp3";
        private const string SaveSoundFileName = "success.mp3";
        private const string OpenJsonSoundFileName = "openDoor.mp3";
        private const string HistorySoundFileName = "slurpLick.mp3";
        private const string PrintBallotSoundFileName = "cartoonPoof.mp3";
        private const string PreviewSoundFileName = "bubble.mp3";
        private const string MusicName = "shortChillMusic.mp3";


        // Importation de la fonction magique de Windows pour lire les MP3
        [DllImport("winmm.dll", CharSet = CharSet.Unicode)]
        private static extern long mciSendString(string command, StringBuilder? returnValue, int returnLength, nint winHandle);

        // ==========================================
        // 🛠️ MÉTHODE CENTRALE
        // ==========================================

        /// <summary>
        /// Gère la lecture de n'importe quel fichier audio de manière sécurisée.
        /// </summary>
        /// <param name="fileName">Le nom du fichier à jouer (ex: "bubble.mp3")</param>
        /// <param name="alias">Le pseudo donné au lecteur Windows (ex: "sfx" ou "bgmusic")</param>
        /// <param name="loop">Si true, la musique tourne en boucle</param>
        private static void PlayAudio(string fileName, string alias, bool loop = false)
        {
            try
            {
                string path = FileSelectionService.FilePathAssets(fileName);
                if (File.Exists(path))
                {
                    // 1. Ferme le son s'il était déjà en cours de lecture sur cet alias
                    mciSendString($"close {alias}", null, 0, nint.Zero);
                    
                    // 2. Ouvre le fichier avec l'alias spécifié
                    mciSendString($"open \"{path}\" type mpegvideo alias {alias}", null, 0, nint.Zero);
                    
                    // 3. Joue le son (avec ou sans répétition)
                    string playCommand = loop ? $"play {alias} repeat" : $"play {alias}";
                    mciSendString(playCommand, null, 0, nint.Zero);
                }
            }
            catch 
            { 
                // On ignore silencieusement si la carte son n'est pas dispo ou le fichier manquant
            }
        }

        // ==========================================
        // 🎵 LECTURE DE LA MUSIQUE D'AMBIANCE
        // ==========================================
        
        /// <summary>
        /// Lance la musique de fond en boucle (utilise l'alias "bgmusic").
        /// </summary>
        public static void PlayBackgroundMusic() => PlayAudio(MusicName, "bgmusic", true);

        /// <summary>
        /// Arrête la musique de fond.
        /// </summary>
        public static void StopBackgroundMusic() => mciSendString("close bgmusic", null, 0, nint.Zero);


        // ==========================================
        // 💥 EFFETS SONORES (SFX)
        // ==========================================
        // Note : On utilise le même alias "sfx" pour tous les effets ponctuels.
        // Cela permet de couper proprement un son en cours si l'utilisateur clique très vite.

        /// <summary>Joue le son de l'envoi d'emails (four).</summary>
        public static void PlaySendSound() => PlayAudio(SendSoundFileName, "sfx");

        /// <summary>Joue le son de sauvegarde (succès).</summary>
        public static void PlaySaveSound() => PlayAudio(SaveSoundFileName, "sfx");

        /// <summary>Joue le son d'ouverture de fichier JSON (porte).</summary>
        public static void PlayOpenJsonSound() => PlayAudio(OpenJsonSoundFileName, "sfx");

        /// <summary>Joue le son d'ouverture de l'historique (slurp).</summary>
        public static void PlayHistorySound() => PlayAudio(HistorySoundFileName, "sfx");

        /// <summary>Joue le son de l'impression des bulletins (poof).</summary>
        public static void PlayPrintBallotSound() => PlayAudio(PrintBallotSoundFileName, "sfx");

        /// <summary>Joue le son de la prévisualisation (bulle).</summary>
        public static void PlayPreviewSound() => PlayAudio(PreviewSoundFileName, "sfx");
    }
}