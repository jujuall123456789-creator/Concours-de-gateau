namespace DuelDeGateaux.Models
{
    /// <summary>
    /// Représente le résultat d'une validation.
    /// </summary>
    public class ValidationResult
    {
        public bool IsValid => Errors.Count == 0;
        public Dictionary<string, string> Errors { get; set; } = new();
    }
}