using System.Net.Mail;

namespace DuelDeGateaux.Services
{
    /// <summary>
    /// Service de validation métier.
    /// </summary>
    public static class FormValidationService
    {
        /// <summary>
        /// vérification de l'adresse mail
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public static bool IsValidEmail(string email)
        {
            try
            {
                return new MailAddress(email).Address == email;
            }
            catch
            {
                return false;
            }
        }
    }
}