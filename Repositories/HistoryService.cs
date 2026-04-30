using DuelDeGateaux.Models;
using System.Text.Json;
using System.Linq;

namespace DuelDeGateaux.Services
{
    internal static class HistoryService
    {
        // 📁 Chemin du fichier JSON (dans dossier exe)
        private const string FileName = "history.json";

        /// <summary>
        /// Chemin du fichier utilisé pour l'historique au format JSON
        /// </summary>
        private static readonly string Path = FileSelectionService.FilePathData(FileName);
        
        /// <summary>
        /// Ajoute un concours dans l'historique avec AUTO-DÉTECTION de la phase ! 🧠
        /// </summary>
        public static void Add(AppConfig config, List<Participant> challengers) 
        {
            var list = Load();
            
            // --- 🤖 ALGORITHME D'AUTO-DÉTECTION DE LA MANCHE ---
            int maxPastPhase = -1; // -1 signifie "Aucun historique"
            
            foreach (var challenger in challengers)
            {
                var pastMatches = list.Where(m => 
                    m.TournamentName == config.CurrentTournamentName && 
                    m.ChallengersList.Contains(challenger.Name));
                
                if (pastMatches.Any())
                {
                    int maxPhaseForThisChallenger = pastMatches.Max(m => m.PhaseIndex);
                    if (maxPhaseForThisChallenger > maxPastPhase)
                    {
                        maxPastPhase = maxPhaseForThisChallenger;
                    }
                }
            }

            int newPhaseIndex = maxPastPhase + 1;
            string newPhaseName = $"Tour {newPhaseIndex + 1}"; 

            // --- 💾 SAUVEGARDE ---
            list.Add(new ChallengeHistoryEntry
            {
                Date = config.ChallengeDate,
                Theme = config.ChallengeTheme,
                ChallengersList = challengers.Select(c => c.Name).ToList(),
                
                // On enregistre le nom du tournoi !
                TournamentName = config.CurrentTournamentName, 
                
                PhaseIndex = newPhaseIndex,
                PhaseName = newPhaseName,
                Winner = null
            });
            
            Save(list);
        }

        /// <summary>
        /// Charge le fichier de l'historique
        /// </summary>
        public static List<ChallengeHistoryEntry> Load()
        {
            if (!File.Exists(Path))
            {
                return new List<ChallengeHistoryEntry>();
            }
            string json = File.ReadAllText(Path);
            if(!string.IsNullOrEmpty(json.Trim()))
            {
                List<ChallengeHistoryEntry> challengeHistoryEntry = JsonSerializer.Deserialize<List<ChallengeHistoryEntry>>(json) ?? new List<ChallengeHistoryEntry>();
                return challengeHistoryEntry;
            }
            else return new List<ChallengeHistoryEntry>();
        }

        /// <summary>
        /// Sauvegarde dans le fichier d'historique
        /// </summary>
        public static void Save(List<ChallengeHistoryEntry> list)
        {
            File.WriteAllText(Path, JsonSerializer.Serialize(list, new JsonSerializerOptions { WriteIndented = true, }));
        }
    }
}