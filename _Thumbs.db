using DuelDeGateaux.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuelDeGateaux.Services
{
    internal static class DrawService
    {
        /// <summary>
        /// Tire aléatoirement les challengers parmi les partcipants éligibbles
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        public static Dictionary<string, string> AssignChallengers(AppConfig config)
        {
            Random random = new Random();

            var eligible = config.Participants
                .Where(p => p.IsEligible)
                .ToList();

            return eligible
                .OrderBy(x => random.Next())
                .Take(config.ChallengerNumber)
                .ToDictionary(p => p.Name, p => p.Email);
        }

    }
}
