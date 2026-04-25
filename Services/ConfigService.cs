using DuelDeGateaux.Models;
using DuelDeGateaux.Tools;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

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
#if DEBUG
        private static readonly string path = Path.Combine("../../../", FileName);
#else
        private static readonly string path=Path.Combine(AppDomain.CurrentDomain.BaseDirectory,FileName);
#endif
        /// <summary>
        /// Charge la configuration depuis le fichier JSON.
        /// </summary>
        /// <returns>Un objet AppConfig rempli ou une nouvelle instance si échec.</returns>
        public static AppConfig Load()
        {
            try
            {
                if (!File.Exists(path))
                {
                    // Si le fichier est absent, on informe l'utilisateur
                    MessageBox.Show($"Fichier de configuration introuvable à l'emplacement : {path}\nUne configuration par défaut sera utilisée.", 
                        "Avertissement", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return new AppConfig();
                }

                var json = File.ReadAllText(path);
                return JsonSerializer.Deserialize<AppConfig>(json) ?? new AppConfig();
            }
            catch (Exception ex)
            {
                throw new Exception($"Erreur lors de la lecture du fichier JSON : {ex.Message}");
            }
        }
       // <summary>
        /// Sauvegarde la configuration actuelle dans le fichier JSON.
        /// </summary>
        /// <param name="config">L'objet de configuration à sérialiser.</param>
        public static void Save(AppConfig config)
        {
            try
            {
                var options = new JsonSerializerOptions { WriteIndented = true };
                var json = JsonSerializer.Serialize(config, options);
                File.WriteAllText(path, json);
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
            if (!File.Exists(path))
            {
                MessageBox.Show("Le fichier JSON n'existe pas encore sur le disque.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            Process.Start("notepad.exe", path);
        }

    }
}
