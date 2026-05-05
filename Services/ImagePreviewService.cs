using System.IO;

namespace DuelDeGateaux.Services
{
    /// <summary>
    /// Service de gestion des aperçus d'images.
    /// </summary>
    public static class ImagePreviewService
    {
        /// <summary>
        /// Méthode commune de chargement de miniature
        /// </summary>
        /// <param name="path">Le chemin de fichier de l'image à charger.</param>
        /// <returns>Une miniature de l'image chargée, ou null si l'image ne peut pas être chargée.</returns>
        public static Image? LoadPreview(string path, int size)
        {
            if (string.IsNullOrWhiteSpace(path) || !File.Exists(path))
                return null;

            using (Image image = Image.FromFile(path))
            {
                return new Bitmap(image, new Size(size, size));
            }
        }
    }
}