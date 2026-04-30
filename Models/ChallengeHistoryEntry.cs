using System;
using System.Collections.Generic;

namespace DuelDeGateaux.Models
{
    public class ChallengeHistoryEntry
    {
        // 🆔 Identifiant unique du match (généré tout seul)
        public string MatchId { get; set; } = Guid.NewGuid().ToString();

        /// <summary>
        /// Date de l'entrée dans l'Historique
        /// </summary>
        public required string Date { get; set; }
        
        /// <summary>
        /// Thème du concours
        /// </summary>
        public required string Theme { get; set; }
        
        /// <summary>
        /// Liste des challengers
        /// </summary>
        public List<string> ChallengersList { get; set; } = new();

        /// <summary>
        /// Liste des challengers Formatés
        /// </summary>
        public string Challengers => string.Join(" vs ", ChallengersList);

       /// <summary>
        /// Nom du tournoi (ex: "Saison 1", "Tournoi Printemps 2026")
        /// </summary>
        public string TournamentName { get; set; } = "Saison 1";
        /// <summary>
        /// Index de la phase (Ex: 0 = Huitième, 1 = Quart, 2 = Demi, 3 = Finale)
        /// Permet au générateur de l'arbre de savoir dans quelle colonne dessiner ce match.
        /// </summary>
        public int PhaseIndex { get; set; } 

        /// <summary>
        /// Nom lisible de la phase (Ex: "Quart de finale")
        /// </summary>
        public string PhaseName { get; set; } = string.Empty;

        /// <summary>
        /// Le gagnant du duel. Nullable (?) car vide au moment de l'envoi du mail !
        /// </summary>
        public string? Winner { get; set; } 
    }
}