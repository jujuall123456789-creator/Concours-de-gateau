using System.Collections.Generic;
using System.Linq;
using System.Text;
using DuelDeGateaux.Models;

namespace DuelDeGateaux.Helpers
{
    public static class TournamentHtmlBuilder
    {
        public static string BuildTree(string seasonName, List<ChallengeHistoryEntry> matches, string defaultCursorCss, string pointerCursorCss)
        {
            if (!matches.Any())
            {
                return $@"
                    <html><body style='background-color:#FFFDF0; font-family:""Segoe UI"", sans-serif; display:flex; flex-direction:column; align-items:center; justify-content:center; height:100vh; margin:0; cursor: {defaultCursorCss};'>
                        <h1 style='color:#3C1E0A;'>{seasonName}</h1>
                        <h3 style='color:#E5D3B3;'>L'arbre est vide... Il faut envoyer un duel ! ⚔️</h3>
                    </body></html>";
            }

            int maxPhase = matches.Max(m => m.PhaseIndex);
            var html = new StringBuilder();

            AppendHeadAndStyles(html, defaultCursorCss, pointerCursorCss);
            AppendBodyAndScript(html, seasonName);
            AppendBracket(html, matches, maxPhase);

            html.AppendLine("</body></html>");
            return html.ToString();
        }

        private static void AppendHeadAndStyles(StringBuilder html, string defaultCursor, string pointerCursor)
        {
            html.AppendLine("<html><head><style>");
            html.AppendLine($"body {{ font-family: 'Segoe UI', sans-serif; background-color: #FFFDF0; margin: 0; padding: 40px; color: #3C1E0A; cursor: {defaultCursor}; }}");
            html.AppendLine(".bracket { display: flex; justify-content: center; gap: 60px; margin-top: 30px; }");
            html.AppendLine(".column { display: flex; flex-direction: column; justify-content: space-around; gap: 30px; }");
            html.AppendLine($".match {{ background: white; border: 3px solid #F4E8D1; border-radius: 12px; padding: 15px; min-width: 200px; box-shadow: 0 8px 16px rgba(60,30,10,0.05); transition: transform 0.2s; cursor: {pointerCursor}; }}");
            html.AppendLine($".match:hover {{ transform: scale(1.05); border-color: #FFB6C1; cursor: {pointerCursor}; }}");
            html.AppendLine(".match-title { font-size: 12px; font-weight: bold; color: #A8957A; text-transform: uppercase; margin: 0 0 10px 0; border-bottom: 1px dashed #F4E8D1; padding-bottom: 5px; text-align: center;}");
            html.AppendLine(".player { padding: 8px 0; font-size: 16px; display: flex; justify-content: space-between; align-items: center; }");
            html.AppendLine(".winner { font-weight: bold; color: #4CAF50; }");
            html.AppendLine(".loser { color: #CCC; text-decoration: line-through; }");
            html.AppendLine(".crown { font-size: 20px; }");
            html.AppendLine("</style>");
        }

        private static void AppendBodyAndScript(StringBuilder html, string seasonName)
        {
            html.AppendLine("<script>");
            html.AppendLine("function selectWinner(matchId) { window.chrome.webview.postMessage(matchId); }");
            html.AppendLine("</script></head><body>");
            html.AppendLine($"<h1 style='text-align: center;'>🏆 {seasonName} 🏆</h1>");
        }

        private static void AppendBracket(StringBuilder html, List<ChallengeHistoryEntry> matches, int maxPhase)
        {
            html.AppendLine("<div class='bracket'>");
            for (int p = 0; p <= maxPhase; p++)
            {
                var phaseMatches = matches.Where(m => m.PhaseIndex == p).ToList();
                html.AppendLine("<div class='column'>");

                foreach (var match in phaseMatches)
                {
                    bool isFinished = !string.IsNullOrEmpty(match.Winner);
                    html.AppendLine($"<div class='match' onclick='selectWinner(\"{match.MatchId}\")'>");
                    html.AppendLine($"<div class='match-title'>{match.PhaseName}</div>");

                    foreach (var player in match.ChallengersList)
                    {
                        bool isWinner = match.Winner == player;
                        string cssClass = "player" + (isFinished ? (isWinner ? " winner" : " loser") : "");
                        string icon = isWinner ? "<span class='crown'>👑</span>" : "";
                        
                        html.AppendLine($"<div class='{cssClass}'><span>{player}</span> {icon}</div>");
                    }
                    html.AppendLine("</div>");
                }
                html.AppendLine("</div>");
            }
            html.AppendLine("</div>");
        }
    }
}