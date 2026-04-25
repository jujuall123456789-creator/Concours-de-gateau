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
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Adresse email du participant
        /// </summary>
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// Indique si le participant est éligible pour être challenger
        /// </summary>
        public bool IsEligible { get; set; }

        /// <summary>
        /// Constructeur vide requis pour la sérialisation.
        /// </summary>
        public Participant()
        {
        }

        public Participant(string name, string email, bool isEligible = false)
        {
            Name = name;
            Email = email;
            IsEligible = isEligible;
        }
    }
}