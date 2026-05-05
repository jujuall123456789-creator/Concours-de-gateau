using System.IO;

namespace DuelDeGateaux.Services
{
    internal static class FileSelectionService
    {
        /// <summary>
        /// Ouvre un sélecteur de fichier Image au format PNG, JPG
        /// </summary>
        /// <returns> Le nom du fichier sélectionné l'utilisateur </returns>
        public static string SelectImage(string existingPath)
        {
            using OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "Images (*.png; *.jpg)|*.jpg; *.png";
            dialog.Title = "Sélectionner une image pour le mail";
            if (!string.IsNullOrEmpty(existingPath))
            {
                dialog.InitialDirectory = Path.GetDirectoryName(existingPath);
            }
            string fileName = dialog.ShowDialog() == DialogResult.OK ? dialog.FileName : "";
            return fileName;
        }

        public static bool FileExists(string path)
        {
            return File.Exists(path);
        }

        public static string FilePathData(string fileName)
        {
#if DEBUG
            return Path.Combine("../../../Data", fileName);
#else
            return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName);
#endif
        }

        public static string FilePathAssets(string fileName)
        {
#if DEBUG
            return Path.Combine("../../../Assets", fileName);
#else
            return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName);
#endif
        }
    }
}
