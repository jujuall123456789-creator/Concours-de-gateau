using System;
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
            string date = string.IsNullOrWhiteSpace(config.ChallengeDate) ? "..." : config.ChallengeDate;

            StringBuilder sb = new StringBuilder();
            sb.AppendLine("<!DOCTYPE html><html lang='fr'><head><meta charset='utf-8'>");
            sb.AppendLine("<style>");
            
            // --- CONFIGURATION IMPRESSION ---
            sb.AppendLine("@media print { ");
            sb.AppendLine("  @page { size: A4 landscape; margin: 5mm; }"); // Format Paysage
            sb.AppendLine("  body { margin: 0; padding: 0; }");
            sb.AppendLine("}");

            // --- STYLE GÉNÉRAL ---
            sb.AppendLine("body { font-family: 'Segoe UI', Tahoma, sans-serif; background-color: white; }");
            
            // Conteneur 2x2 pour occuper toute la page landscape
            sb.AppendLine(".container { display: grid; grid-template-columns: 1fr 1fr; grid-template-rows: 1fr 1fr; height: 100vh; width: 100vw; box-sizing: border-box; }");
            
            // Style d'un bulletin (ballot)
            sb.AppendLine(".ballot { border: 1px dashed #ccc; padding: 15px; box-sizing: border-box; display: flex; flex-direction: column; justify-content: center; }");
            sb.AppendLine(".header { text-align: center; color: #D35400; font-weight: bold; font-size: 14pt; margin-bottom: 15px; border-bottom: 2px solid #FBEEE6; padding-bottom: 5px; }");
            
            // Style des lignes de critères (Aspect visuel, etc.)
            sb.AppendLine(".criterion-row { border: 1.5px solid #2C3E50; margin-bottom: 8px; padding: 8px; display: flex; align-items: center; justify-content: space-between; }");
            
            // "Aspect Visuel :" reste sur une seule ligne
            sb.AppendLine(".label { font-weight: bold; font-size: 11pt; white-space: nowrap; margin-right: 15px; }");
            
            // Conteneur des choix (Gâteau 1, 2...)
            sb.AppendLine(".choices { display: flex; gap: 15px; align-items: center; flex-grow: 1; justify-content: flex-end; }");
            sb.AppendLine(".cake-item { display: flex; align-items: center; white-space: nowrap; font-size: 10pt; }");
            sb.AppendLine(".checkbox { width: 16px; height: 16px; border: 1.5px solid #2C3E50; margin-left: 6px; display: inline-block; }");
            
            sb.AppendLine("</style></head>");
            
            // Script pour lancer l'impression au chargement
            sb.AppendLine("<body onload='window.print()'>");
            sb.AppendLine("<div class='container'>");

            string[] criteria = { "👁️ Aspect Visuel", "🧱 Texture", "👅 Goût", "✨ Topping" };

            // On génère exactement 4 bulletins
            for (int i = 0; i < 4; i++)
            {
                sb.AppendLine("  <div class='ballot'>");
                sb.AppendLine($"    <div class='header'>🎂 Concours de Gâteau du {date} 🍪</div>");

                foreach (var crit in criteria)
                {
                    sb.AppendLine("    <div class='criterion-row'>");
                    sb.AppendLine($"      <div class='label'>{crit} :</div>");
                    sb.AppendLine("      <div class='choices'>");
                    
                    for (int c = 1; c <= nbCakes; c++)
                    {
                        sb.AppendLine("        <div class='cake-item'>");
                        sb.AppendLine($"          <span>Gâteau {c}</span><div class='checkbox'></div>");
                        sb.AppendLine("        </div>");
                    }
                    
                    sb.AppendLine("      </div>");
                    sb.AppendLine("    </div>");
                }
                sb.AppendLine("  </div>");
            }

            sb.AppendLine("</div></body></html>");

            // --- SAUVEGARDE ET OUVERTURE ---
            try
            {
                string tempFile = Path.Combine(Path.GetTempPath(), "Bulletins_Duel.html");
                File.WriteAllText(tempFile, sb.ToString(), Encoding.UTF8);

                Process.Start(new ProcessStartInfo
                {
                    FileName = tempFile,
                    UseShellExecute = true
                });
            }
            catch (Exception ex)
            {
                throw new Exception("Impossible de générer le fichier d'impression : " + ex.Message);
            }
        }
    }
}
