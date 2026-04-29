using System;
using System.Windows.Forms;
using Microsoft.Web.WebView2.WinForms;

namespace DuelDeGateaux.Forms
{
    public partial class PreviewForm : Form
    {
        private WebView2 webView;
        private string htmlContent;

        public PreviewForm(string html, string title)
        {
            htmlContent = html;
            
            // Configuration de base de la fenêtre
            Text = "👁️ Aperçu : " + title;
            Width = 850;
            Height = 900;
            StartPosition = FormStartPosition.CenterParent;
            ShowIcon = false;

            // Création dynamique du composant WebView2
            webView = new WebView2
            {
                Dock = DockStyle.Fill
            };
            Controls.Add(webView);

            // Le composant WebView2 doit être initialisé de manière asynchrone
            InitializeWebView();
        }

        private async void InitializeWebView()
        {
            // Attend que le moteur Edge soit prêt
            await webView.EnsureCoreWebView2Async(null);
            
            // Injecte notre code HTML directement dans le navigateur intégré
            webView.NavigateToString(htmlContent);
        }
    }
}
