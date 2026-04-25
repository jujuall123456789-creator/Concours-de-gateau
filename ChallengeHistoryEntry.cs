using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace DuelDeGateaux.Models
{
    internal class ChallengeHistoryEntry
    {
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
    }
}
