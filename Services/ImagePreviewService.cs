/// <summary>
/// Service de gestion des aperçus d'images.
/// </summary>
public static class ImagePreviewService
{
    public static Image? LoadPreview(string path, int size)
    {
        if (string.IsNullOrWhiteSpace(path) || !File.Exists(path))
            return null;

        using Image image = Image.FromFile(path);
        return new Bitmap(image, new Size(size, size));
    }
}