using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuelDeGateaux.Tools
{
    internal static class FileHelper
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
            string fileNmae = dialog.ShowDialog() == DialogResult.OK ? dialog.FileName : "";
            return fileNmae;
        }

        public static bool FileExist(string path)
        {
            return File.Exists(path);
        }
    }
}
