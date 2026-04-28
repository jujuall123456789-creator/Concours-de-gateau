using DuelDeGateaux.Models;
using System.Net.Mail;

namespace DuelDeGateaux.Services
{
    /// <summary>
    /// Service de validation métier.
    /// </summary>
    public static class FormValidationService
    {
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