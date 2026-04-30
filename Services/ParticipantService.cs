using DuelDeGateaux.Models;
using System.ComponentModel;

namespace DuelDeGateaux.Services
{
    /// <summary>
    /// Service métier lié aux participants.
    /// </summary>
    public static class ParticipantService
    {
        public static void AddDefaultParticipant(BindingList<Participant> participants, string email)
        {
            participants.Add(new Participant("👴PNJ👴", email));
        }

        public static bool HasEnoughEligible(List<Participant> participants, int required)
        {
            return participants.Count(p => p.IsEligible) >= required;
        }
    }
}