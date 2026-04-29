using DuelDeGateaux.Models;
using System.Text.Json;

namespace DuelDeGateaux.Services
{
    internal static class HistoryService
    {
       
        // 📁 Chemin du fichier JSON (dans dossier exe)
        private const string FileName = "history.json";

        /// <summary>
        /// Chemin du fichier utilsié pour l'historique au format JSON
        /// </summary>
        private static readonly string Path =  FileSelectionService.FilePathData(FileName);
        /// <summary>
        /// Ajoute un concours dans l'historique
        /// </summary>
        /// <param name="config"></param>
        /// <param name="assignments"></param>
        public static void Add(AppConfig config, List<Participant> challengers) 
        {
            var list = Load();
            list.Add(new ChallengeHistoryEntry
            {
                Date = config.ChallengeDate,
                Theme = config.ChallengeTheme,
                ChallengersList = challengers.Select(c => c.Name).ToList()
            });
            Save(list);
        }

        /// <summary>
        /// Charge le fichier de l'historique
        /// </summary>
        /// <returns></returns>
        public static List<ChallengeHistoryEntry> Load()
        {
            // Si le fichier n'exite pas, on retourne une liste vide
            if (!File.Exists(Path))
            {
                return new List<ChallengeHistoryEntry>();
            }
            string json = File.ReadAllText(Path);
            if(!String.IsNullOrEmpty(json.Trim()))
            {
                List<ChallengeHistoryEntry> challengeHsitoryEntry = JsonSerializer.Deserialize<List<ChallengeHistoryEntry>>(json) ?? new List<ChallengeHistoryEntry>();
                return challengeHsitoryEntry;
            }
            else return new List<ChallengeHistoryEntry>();
        }
        /// <summary>
        /// Sauvegarde dans le fichier d'historique les challengers du concours après l'envoi du mail
        /// </summary>
        /// <param name="config"></param>
        public static void Save(List<ChallengeHistoryEntry> list)
        {
            File.WriteAllText(Path, JsonSerializer.Serialize(list, new JsonSerializerOptions { WriteIndented = true, }));
        }

    }
}
