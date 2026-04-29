using Microsoft.Web.WebView2.WinForms;
using System.IO;
using System.Text;

namespace DuelDeGateaux.Forms
{
    public partial class PreviewForm : Form
    {
        private WebView2 webView;
        private string htmlContent;

        public PreviewForm(string html, string title)
        {
            htmlContent = html;
            Text = "👁️ Aperçu : " + title;
            Width = 850;
            Height = 900;
            StartPosition = FormStartPosition.CenterParent;
            
            webView = new WebView2 { Dock = DockStyle.Fill };
            Controls.Add(webView);

            InitializeWebView();
        }

        private async void InitializeWebView()
        {
            await webView.EnsureCoreWebView2Async(null);

            // On crée un fichier temporaire pour éviter la limite de taille de string
            string tempFile = Path.Combine(Path.GetTempPath(), "preview_mail.html");
            File.WriteAllText(tempFile, htmlContent, Encoding.UTF8);

            // On demande à la WebView de charger le fichier
            webView.Source = new Uri(tempFile);
        }
    }
}
