using DuelDeGateaux.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UTIL_SMTPLib;

namespace DuelDeGateaux.Services
{
    internal static class EmailService
    {
        // ==========================================
        // 1. MÉTHODE D'ENVOI PRINCIPALE
        // ==========================================
        public static void SendDuelEmails(AppConfig config, List<Participant> challengers)
        {
            var challengerEmails = challengers.Select(c => c.Email.ToLower()).ToList();

            // Remplacement de la balise {{Date}} pour les sujets
            string subjectChallenger = config.SubjectMailChallenger.Replace("{{Date}}", config.ChallengeDate);
            string subjectEater = config.SubjectMailEater.Replace("{{Date}}", config.ChallengeDate);

            // Conversion des images en Base64
            string headerBase64 = ConvertImageToBase64(config.PathImageHeading);
            string footerBase64 = ConvertImageToBase64(config.PathImageFooter);

            // On prépare la phrase "L'incroyable X et le redoutable Y"
            string challengersAnnouncement = GetFormattedChallengers(config, challengers);

            // ==========================================
            // 🚀 MODE TEST : On envoie les 2 mails au testeur
            // ==========================================
            if (config.IsTest)
            {
                if (string.IsNullOrWhiteSpace(config.TesterEmail)) return;

                string opponentsPhrase = GetFormattedChallengers(config, challengers);
                
                string testChallengerHtml = GenerateChallengerHtml(config, "Testeur", opponentsPhrase, headerBase64, footerBase64);
                SendSmtpEmail(config.SenderEmail, config.TesterEmail, "[TEST] " + subjectChallenger, testChallengerHtml);

                string testEaterHtml = GenerateEaterHtml(config, "Testeur", challengersAnnouncement, headerBase64, footerBase64);
                SendSmtpEmail(config.SenderEmail, config.TesterEmail, "[TEST] " + subjectEater, testEaterHtml);
                
                return; // On sort pour ne pas envoyer aux vrais participants
            }

            // ==========================================
            // 🚀 MODE RÉEL : Boucle sur tous les participants
            // ==========================================
            foreach (var p in config.Participants)
            {
                if (string.IsNullOrWhiteSpace(p.Email)) continue;

                bool isChallenger = challengerEmails.Contains(p.Email.ToLower());
                string bodyHtml;
                string subject = isChallenger ? subjectChallenger : subjectEater;

                if (isChallenger)
                {
                    var opponents = challengers.Where(c => !c.Email.Equals(p.Email, StringComparison.OrdinalIgnoreCase)).ToList();
                    string realOpponentsPhrase = GetFormattedChallengers(config, opponents);
                    bodyHtml = GenerateChallengerHtml(config, p.Name, realOpponentsPhrase, headerBase64, footerBase64);
                }
                else
                {
                    bodyHtml = GenerateEaterHtml(config, p.Name, challengersAnnouncement, headerBase64, footerBase64);
                }

                SendSmtpEmail(config.SenderEmail, p.Email, subject, bodyHtml);
            }
        }

        // ==========================================
        // 2. MÉTHODE UTILITAIRE : ENVOI SMTP FACTORISÉ
        // ==========================================
        private static void SendSmtpEmail(string sender, string to, string subject, string htmlBody)
        {
            IUTIL_SMTPClient smtp = new UTIL_SMTPClient();
            smtp.From(sender);
            smtp.To(to);
            smtp.Subject(subject);
            smtp.AddHtmlBody(htmlBody);
            smtp.Send();
        }

        // ==========================================
        // 3. LOGIQUE DES TITRES ALÉATOIRES
        // ==========================================
        private static string GetFormattedChallengers(AppConfig config, List<Participant> challengers)
        {
            if (challengers == null || challengers.Count == 0) return "";

            Random rng = new Random();
            var availableTitles = new List<string>(config.ChallengersTitles ?? new List<string>());
            var namedChallengers = new List<string>();

            foreach (var c in challengers)
            {
                string title = "";
                if (availableTitles.Count > 0)
                {
                    int index = rng.Next(availableTitles.Count);
                    title = availableTitles[index].Trim() + " ";
                    availableTitles.RemoveAt(index);
                }
                namedChallengers.Add($"<strong>{title}{c.Name}</strong>");
            }

            if (namedChallengers.Count == 1) return namedChallengers[0];

            string last = namedChallengers.Last();
            namedChallengers.RemoveAt(namedChallengers.Count - 1);

            return string.Join(", ", namedChallengers) + " et " + last;
        }

        // ==========================================
        // 4. TEMPLATE GÉNÉRIQUE (Squelette HTML factorisé)
        // ==========================================
        private static string BuildEmailTemplate(AppConfig config, string titleText, string innerHtml, string headerBase64, string footerBase64)
        {
            string headerImageHtml = string.IsNullOrEmpty(headerBase64) ? "" :
                $@"<tr><td align='center' bgcolor='#FBEEE6'><img src='data:image/jpeg;base64,{headerBase64}' width='600' height='{config.ImageHeadingHeight}' style='display:block; border:none; max-width:600px; height:{config.ImageHeadingHeight}px;' alt='Header'></td></tr>";

            string footerImageHtml = string.IsNullOrEmpty(footerBase64) ? "" :
                $@"<table role='presentation' width='100%' cellspacing='0' cellpadding='0' border='0'>
                     <tr>
                       <td align='center' style='padding-top:20px;'>
                         <img src='data:image/jpeg;base64,{footerBase64}' width='250' height='250' style='display:block; border:none; max-width:250px;' alt='Footer'>
                       </td>
                     </tr>
                   </table>";

            return $@"
            <table role='presentation' width='100%' cellspacing='0' cellpadding='0' border='0' bgcolor='#f4f4f4'>
              <tr>
                <td align='center' style='padding: 20px 0;'>
                  <table role='presentation' width='600' cellspacing='0' cellpadding='0' border='0' bgcolor='#FBEEE6' style='border: 1px solid #dddddd; font-family: ""Segoe UI"", Arial, sans-serif;'>
                    {headerImageHtml}
                    <tr>
                      <td bgcolor='#D35400' align='center' style='padding: 20px; color: #ffffff;'>
                        <h1 style='margin: 0; font-size: 24px; letter-spacing: 2px;'>{titleText}</h1>
                      </td>
                    </tr>
                    <tr>
                      <td style='padding: 30px; color: #2C3E50; font-size: {config.FontSize}px; line-height: 1.6;'>
                        {innerHtml}
                        {footerImageHtml}
                        <p style='font-size: 12px; color: #7f8c8d; text-align: center; margin-top: 30px;'>
                          You received this email because you suck.
                        </p>
                      </td>
                    </tr>
                  </table>
                </td>
              </tr>
            </table>";
        }

        // ==========================================
        // 5. CONTENU SPÉCIFIQUE : LES CHALLENGERS
        // ==========================================
        private static string GenerateChallengerHtml(AppConfig config, string participantName, string opponents, string headerBase64, string footerBase64)
        {
            string bodyHtml = $@"
                <p style='margin-top: 0;'>Félicitations <strong>{participantName}</strong> !</p>
                <p>Tu as été tiré(e) au sort pour notre grand concours de pâtisserie. Prépare ton meilleur fouet et fais chauffer le four, car la bataille s'annonce épique.</p>
                
                <table role='presentation' width='100%' cellspacing='0' cellpadding='0' border='0'>
                  <tr><td height='20' style='font-size:0px; line-height:0px;'>&nbsp;</td></tr> <tr>
                    <td bgcolor='#ffffff' style='padding: 15px; border-left: 4px solid #D35400;'>
                      <p style='margin: 0;'><strong>🎯 Thème :</strong> {config.ChallengeTheme}</p>
                      <p style='margin: 5px 0 0 0;'><strong>🥊 Tes adversaires :</strong> {opponents}</p>
                    </td>
                  </tr>
                  <tr><td height='20' style='font-size:0px; line-height:0px;'>&nbsp;</td></tr> 
                </table>

                <p><strong>📅 Rendez-vous le :</strong> {config.ChallengeDate} à {config.ChallengeHour}</p>
                <p><strong>📍 Lieu du combat :</strong> {config.ChallengeRoom}</p>
                <p><strong>📜 Règles à respecter :</strong> {config.ChallengeRules}</p>
                <p><strong>🏆 À gagner :</strong> {config.ChallengePrice}</p>

                <p style='margin-top: 25px; color: #D35400; font-weight: bold; text-align: center;'>{config.ChallengeParticipationMessage}</p>";

            return BuildEmailTemplate(config, "🧑‍🍳 LE DESTIN T'A CHOISI 🧑‍🍳", bodyHtml, headerBase64, footerBase64);
        }

        // ==========================================
        // 6. CONTENU SPÉCIFIQUE : LE JURY (MANGEURS)
        // ==========================================
        private static string GenerateEaterHtml(AppConfig config, string eaterName, string challengersAnnouncement, string headerBase64, string footerBase64)
        {
            string bodyHtml = $@"
                <p style='margin-top: 0;'>Cher/Chère <strong>{eaterName.ToUpper()}</strong> 👮,</p>
                <p>Grande nouvelle (et pas moyen d'y échapper) : {challengersAnnouncement} ont été sélectionnés comme Challengers à notre grand concours de gâteaux !</p>
                
                <table role='presentation' width='100%' cellspacing='0' cellpadding='0' border='0'>
                  <tr><td height='20' style='font-size:0px; line-height:0px;'>&nbsp;</td></tr> 
                  <tr>
                    <td bgcolor='#ffffff' style='padding: 15px; border-left: 4px solid #D35400;'>
                      <p style='margin: 0;'><strong>🎯 Thème :</strong> {config.ChallengeTheme}</p>
                      <p style='margin: 5px 0 0 0;'><strong>🧑‍🍳 Consigne :</strong> {config.ChallengeRules}</p>
                    </td>
                  </tr>
                  <tr><td height='20' style='font-size:0px; line-height:0px;'>&nbsp;</td></tr> 
                </table>

                <p><strong>📅 Rendez-vous le :</strong> {config.ChallengeDate} à {config.ChallengeHour}</p>
                <p><strong>📍 Lieu du combat :</strong> {config.ChallengeRoom}</p>
                <p><strong>🏆 À la clé pour eux :</strong> {config.ChallengePrice}</p>

                <p style='margin-top: 15px;'><em>Les règles sont simples : ils pâtissent, on déguste, et on élit le meilleur gâteau ! Préparez vos bavoirs. 👅</em></p>

                <p style='margin-top: 25px; color: #D35400; font-weight: bold; text-align: center;'>{config.ChallengeParticipationMessage}</p>";

            return BuildEmailTemplate(config, "🧑‍⚖️ À VOS FOURCHETTES 🧑‍⚖️", bodyHtml, headerBase64, footerBase64);
        }

        // ==========================================
        // 7. OUTIL : CONVERSION IMAGE EN BASE 64
        // ==========================================
        private static string ConvertImageToBase64(string imagePath)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(imagePath) || !File.Exists(imagePath))
                    return string.Empty;

                byte[] imageBytes = File.ReadAllBytes(imagePath);
                return Convert.ToBase64String(imageBytes);
            }
            catch
            {
                return string.Empty;
            }
        }

        // ==========================================
        // 8. OUTIL : PRÉVISUALISATION
        // ==========================================
        public static string GetPreviewHtml(AppConfig config, bool isChallengerTemplate)
        {
            // Données fictives pour l'aperçu
            string dummyName = "Jean Dupont";
            string dummyMatch = "<strong>L'incroyable Jean</strong> et <strong>le redoutable Michel</strong>";

            string headerBase64 = ConvertImageToBase64(config.PathImageHeading);
            string footerBase64 = ConvertImageToBase64(config.PathImageFooter);

            if (isChallengerTemplate)
            {
                return GenerateChallengerHtml(config, dummyName, "<strong>le redoutable Michel</strong>", headerBase64, footerBase64);
            }
            else
            {
                return GenerateEaterHtml(config, dummyName, dummyMatch, headerBase64, footerBase64);
            }
        }
    }
}