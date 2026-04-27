using System.Diagnostics;
using System.IO;
using System.Text;
using DuelDeGateaux.Models;

namespace DuelDeGateaux.Services
{
    internal static class BallotService
    {
        public static void GenerateAndPrintBallots(AppConfig config)
        {
            int nbCakes = config.ChallengerNumber;
            // On récupère juste la date (on peut ajouter le thème si on veut)
            string date = string.IsNullOrWhiteSpace(config.ChallengeDate) ? "..." : config.ChallengeDate;

            StringBuilder sb = new StringBuilder();
            sb.AppendLine("<!DOCTYPE html><html lang='fr'><head><meta charset='utf-8'><title>Bulletins de vote</title>");
            sb.AppendLine("<style>");
            // CSS général
            sb.AppendLine("body { font-family: 'Segoe UI', Arial, sans-serif; margin: 0; padding: 20px; color: #333; }");
            // Grille pour avoir 4 bulletins par page (2 colonnes, 2 lignes)
            sb.AppendLine(".page { display: grid; grid-template-columns: 1fr 1fr; gap: 40px; margin-bottom: 40px; }");
            sb.AppendLine(".ballot { padding: 15px; }");
            sb.AppendLine(".title { font-size: 12pt; font-weight: bold; color: #D35400; margin-bottom: 20px; text-align: center; border-bottom: 2px dashed #D35400; padding-bottom: 10px; }");
            // Le style des blocs de notation (calqué sur ta photo)
            sb.AppendLine(".criterion { display: flex; justify-content: space-between; align-items: center; border: 1px solid #555; padding: 10px; margin-bottom: 12px; font-size: 10pt; }");
            sb.AppendLine(".crit-name { font-weight: bold; width: 40%; }");
            sb.AppendLine(".cakes { width: 60%; text-align: right; }");
            sb.AppendLine(".cake-line { margin-bottom: 6px; display: flex; justify-content: flex-end; align-items: center; }");
            sb.AppendLine(".cake-line:last-child { margin-bottom: 0; }");
            sb.AppendLine(".checkbox { width: 14px; height: 14px; border: 1px solid #000; display: inline-block; margin-left: 10px; }");
            // CSS Spécial pour l'imprimante (Format A4 paysage ou portrait)
            sb.AppendLine("@media print { ");
            sb.AppendLine("  @page { size: A4 portrait; margin: 15mm; }");
            sb.AppendLine("  body { padding: 0; }");
            sb.AppendLine("  .ballot { page-break-inside: avoid; }");
            sb.AppendLine("}");
            sb.AppendLine("</style></head>");
            
            // Le script onload ouvre directement la fenêtre d'impression
            sb.AppendLine("<body onload='window.print()'>");
            sb.AppendLine("<div class='page'>");

            // Les critères basés sur ta photo
            string[] criteria = { "👁️ Aspect Visuel", "🧱 Texture", "👅 Goût", "✨ Topping" };

            // On génère 4 bulletins pour remplir une feuille A4
            for (int i = 0; i < 4; i++)
            {
                sb.AppendLine("<div class='ballot'>");
                sb.AppendLine($"<div class='title'>🍪 Concours de Gâteau du {date} 🍰</div>");

                foreach (var crit in criteria)
                {
                    sb.AppendLine("<div class='criterion'>");
                    sb.AppendLine($"  <div class='crit-name'>{crit} :</div>");
                    sb.AppendLine("  <div class='cakes'>");
                    
                    // On boucle selon le nombre de challengers (2 ou 3)
                    for (int c = 1; c <= nbCakes; c++)
                    {
                        sb.AppendLine($"    <div class='cake-line'><span>Gâteau {c}</span> <div class='checkbox'></div></div>");
                    }
                    
                    sb.AppendLine("  </div>");
                    sb.AppendLine("</div>");
                }
                sb.AppendLine("</div>");
            }

            sb.AppendLine("</div></body></html>");

            // Sauvegarde dans un fichier temporaire
            string tempPath = Path.Combine(Path.GetTempPath(), "BulletinsDeVote_DuelDeGateaux.html");
            File.WriteAllText(tempPath, sb.ToString(), Encoding.UTF8);

            // Ouverture dans le navigateur par défaut
            Process.Start(new ProcessStartInfo
            {
                FileName = tempPath,
                UseShellExecute = true
            });
        }
    }
}
