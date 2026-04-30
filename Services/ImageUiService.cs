using DuelDeGateaux.Services;

namespace DuelDeGateaux.Services
{
    /// <summary>
    /// Service utilitaire dédié à la gestion des images côté interface utilisateur.
    /// Fournit des méthodes pour charger des aperçus, valider les formats
    /// et sécuriser les opérations liées aux fichiers image.
    /// </summary>
    public static class ImageUiService
    {
        /// <summary>
        /// Charge une miniature d'image depuis un chemin enregistré en configuration.
        /// Si le fichier est absent ou invalide, retourne null sans lever d'exception.
        /// </summary>
        /// <param name="path">Chemin du fichier image.</param>
        /// <param name="thumbnailSize">Taille maximale de la miniature.</param>
        /// <returns>Une miniature de l'image ou null si impossible.</returns>
        public static Image? LoadPreviewFromConfig(string path, int thumbnailSize)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(path) || !File.Exists(path))
                    return null;

                return ImagePreviewService.LoadPreview(path, thumbnailSize);
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Charge une miniature d'image choisie par l'utilisateur.
        /// Lève une exception si le fichier est introuvable.
        /// </summary>
        /// <param name="path">Chemin du fichier image.</param>
        /// <param name="thumbnailSize">Taille maximale de la miniature.</param>
        /// <returns>Une miniature prête à afficher.</returns>
        /// <exception cref="FileNotFoundException">
        /// Levée si le fichier n'existe pas.
        /// </exception>
        public static Image? LoadPreviewFromUserInput(string path, int thumbnailSize)
        {
            if (string.IsNullOrWhiteSpace(path) || !File.Exists(path))
                throw new FileNotFoundException("Image introuvable.");

            return ImagePreviewService.LoadPreview(path, thumbnailSize);
        }

        /// <summary>
        /// Vérifie si un fichier correspond à un format d'image autorisé.
        /// </summary>
        /// <param name="path">Chemin du fichier à tester.</param>
        /// <param name="allowedExtensions">Liste des extensions autorisées.</param>
        /// <returns>True si le fichier est supporté, sinon false.</returns>
        public static bool IsSupportedImage(string path, string[] allowedExtensions)
        {
            string ext = Path.GetExtension(path).ToLowerInvariant();
            return allowedExtensions.Contains(ext);
        }
    }
}