using DuelDeGateaux.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DuelDeGateaux.Services
{
    internal static class DrawService
    {
        /// <summary>
        /// Tire aléatoirement les challengers parmi les participants éligibles.
        /// </summary>
        /// <param name="config">La configuration contenant la liste des participants et le nombre de challengers voulu.</param>
        /// <returns>Une liste contenant les participants tirés au sort.</returns>
        /// <exception cref="InvalidOperationException">Lancée s'il n'y a pas assez de participants éligibles.</exception>
        public static List<Participant> AssignChallengers(AppConfig config)
        {
            var random = new Random();

            // 1. On filtre pour ne garder que les participants éligibles
            var eligible = config.Participants
                .Where(p => p.IsEligible)
                .ToList();

            // 2. Vérification de sécurité CRITIQUE
            if (eligible.Count < config.ChallengerNumber)
            {
                // On lève une exception précise que tu pourras attraper dans ton MainForm
                // pour afficher un joli message d'erreur à l'utilisateur (ex: MessageBox.Show(...))
                throw new InvalidOperationException($"Impossible de lancer le tirage : Il faut {config.ChallengerNumber} challengers, mais seulement {eligible.Count} participants sont éligibles.");
            }

            // 3. Tirage au sort
            return eligible
                .OrderBy(x => random.Next())
                .Take(config.ChallengerNumber)
                .ToList(); // On retourne une List<Participant> au lieu d'un dictionnaire pour éviter les crashs sur les doublons de noms
        }
    }
}