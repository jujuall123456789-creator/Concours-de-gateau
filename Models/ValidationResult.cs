namespace DuelDeGateaux.Models
{
    /// <summary>
    /// Représente le résultat d'une validation.
    /// </summary>
    public class ValidationResult
    {
        public bool IsValid { get; set; }
        public string Message { get; set; } = string.Empty;
    }
}