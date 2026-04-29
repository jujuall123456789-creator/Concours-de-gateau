using DuelDeGateaux.Models;
using System.Diagnostics;
using System.Text.Json;

namespace DuelDeGateaux.Services
{
    /// <summary>
    /// Service gérant la persistance de la configuration de l'application (JSON).
    /// </summary>
    internal static class ConfigService
    {
        // 📁 Chemin du fichier JSON (dans dossier exe)
        private const string FileName = "appsettings.json";

        /// <summary>
        /// Chemin du fichier de configuration JSON
        /// Gère le décalage de dossier en mode Debug pour Visual Studio.
        /// </summary>
        private static readonly string ConfigPath = FileSelectionService.FilePathData(FileName);
        /// <summary>
        /// Charge la configuration. 
        /// Retourne une config par défaut si le fichier n'existe pas.
        /// </summary>
        public static AppConfig Load()
        {
            try
            {
                if (!File.Exists(ConfigPath))
                {
                    // Si le fichier est absent, on informe l'utilisateur
                    MessageBox.Show($"Fichier de configuration introuvable à l'emplacement : {ConfigPath}\nUne configuration par défaut sera utilisée.", 
                        "Avertissement", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return new AppConfig();
                }

                var json = File.ReadAllText(ConfigPath);
                // Si le fichier est vide, on retourne une nouvelle instance
                if (string.IsNullOrWhiteSpace(json)) return new AppConfig();
                var config = JsonSerializer.Deserialize<AppConfig>(json) ?? new AppConfig();
                config.Participants ??= new List<Participant>();
                return config;
            }
            catch (JsonException ex)
            {
                // Erreur de formatage JSON
                throw new Exception($"Le fichier de configuration est corrompu : {ex.Message}");
            }
            catch (IOException ex)
            {
                // Erreur d'accès au fichier (fichier ouvert par un autre processus, etc.)
                throw new Exception($"Impossible d'accéder au fichier : {ex.Message}");
            }
        }
        /// <summary>
        /// Sauvegarde la configuration actuelle dans le fichier JSON.
        /// </summary>
        /// <param name="config">L'objet de configuration à sérialiser.</param>
        public static void Save(AppConfig config)
        {
            try
            {
                var options = new JsonSerializerOptions { WriteIndented = true };
                var json = JsonSerializer.Serialize(config, options);
                File.WriteAllText(ConfigPath, json);
            }
            catch (Exception ex)
            {
                throw new Exception($"Impossible de sauvegarder la configuration : {ex.Message}");
            }
        }

        /// <summary>
        /// Ouvre le fichier de configuration JSON dans le Bloc-notes de Windows.
        /// </summary>
        public static void OpenConfigFile()
        {
            if (!File.Exists(ConfigPath))
            {
                MessageBox.Show("Le fichier JSON n'existe pas encore sur le disque.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            Process.Start(new ProcessStartInfo
            {
                FileName = "notepad.exe",
                Arguments = ConfigPath,
                UseShellExecute = true
            });
        }

    }
}
