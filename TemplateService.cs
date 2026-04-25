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
    internal static class ConfigService
    {
        // 📁 Chemin du fichier JSON (dans dossier exe)
        private const string FileName = "appsettings.json";

        /// <summary>
        /// Chemin du fichier de configuration JSON
        /// </summary>
#if DEBUG
        private static readonly string path = Path.Combine("../../../", FileName);
#else
        private static readonly string path=Path.Combine(AppDomain.CurrentDomain.BaseDirectory,FileName);
#endif
        public static AppConfig Load()
        {
            /// <summary>
            /// Charge la configuration depuis le fichier JSON
            /// </summary>
            if (!FileHelper.FileExist(path))
            {
                //Si le fichier est absent retour d'un message d'erreur
                MessageBox.Show($"Config introuvable : {path}");
                Environment.Exit(1);
            }

            var json = File.ReadAllText(path);
            return JsonSerializer.Deserialize<AppConfig>(json) ?? new AppConfig();
        }
        /// <summary>
        /// Sauvegarde la configuration faite par l'utilisateur dans le fichier appsetting.json
        /// </summary>
        /// <param name="config"></param>
        public static void Save(AppConfig config)
        {
            File.WriteAllText(path, JsonSerializer.Serialize(config, new JsonSerializerOptions { WriteIndented = true, }));
        }

        /// <summary>
        /// Ouvre le fichier de configuration JSON dans l'éditeur notepad.exe
        /// </summary>
        public static void OpenConfigFile()
        {
            if (!File.Exists(path))
            {
                MessageBox.Show("Fichier config introuvable");
                return;
            }

            Process.Start("notepad.exe", path);
        }

    }
}
