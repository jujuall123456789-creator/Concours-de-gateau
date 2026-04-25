namespace DuelDeGateaux.Models
{
    /// <summary>
    /// Réprésente un particpant du concours de gâteau
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
        /// Indique si le particpant est éligible pour être challanger
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