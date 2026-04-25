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
#if DEBUG
        private static readonly string path = Path.Combine("../../../", FileName);
#else
        private static readonly string path=Path.Combine(AppDomain.CurrentDomain.BaseDirectory,FileName);
#endif
        /// <summary>
        /// Ajoute un concours dans l'historique
        /// </summary>
        /// <param name="config"></param>
        /// <param name="assignments"></param>
        public static void Add(AppConfig config, Dictionary<string, string> assignments) 
        {
            var list = Load();
            list.Add(new ChallengeHistoryEntry
            {
                Date = config.ChallengeDate,
                Theme = config.ChallengeTheme,
                ChallengersList = assignments.Keys.ToList()
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
            if (!File.Exists(path))
            {
                return new List<ChallengeHistoryEntry>();
            }
            string json = File.ReadAllText(path);
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
            File.WriteAllText(path, JsonSerializer.Serialize(list, new JsonSerializerOptions { WriteIndented = true, }));
        }

    }
}
