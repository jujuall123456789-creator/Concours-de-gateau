namespace DuelDeGateaux.Models
{
    /// <summary>
    /// Représente un participant du concours de gâteau
    /// </summary>
    internal class Participant
    {
        /// <summary>
        /// Nom affiché du participant
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Adresse email du participant
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Indique si le participant est éligible pour être challenger
        /// </summary>
        public bool IsEligible { get; set; }

        public Participant(string name, string email, bool isEligible = false)
        {
            Name = name;
            Email = email;
            IsEligible = isEligible;
        }
    }
}