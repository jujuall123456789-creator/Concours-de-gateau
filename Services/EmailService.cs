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
            // On extrait les emails des challengers pour la comparaison
            var challengerEmails = challengers.Select(c => c.Email.ToLower()).ToList();

            // On gère le mode Test
            var recipients = config.IsTest
                ? new List<Participant> { new Participant("Testeur", config.TesterEmail) }
                : config.Participants;

            // Remplacement de la balise {{Date}} pour les sujets
            string finalSubjectChallenger = config.SubjectMailChallenger.Replace("{{Date}}", config.ChallengeDate);
            string finalSubjectEater = config.SubjectMailEater.Replace("{{Date}}", config.ChallengeDate);

            // Conversion des images en Base64
            string headerBase64 = ConvertImageToBase64(config.PathImageHeading);
            string footerBase64 = ConvertImageToBase64(config.PathImageFooter);

            // On prépare la phrase "L'incroyable X et le redoutable Y" une seule fois pour tout le monde
            string challengersAnnouncement = GetFormattedChallengers(config, challengers);

            foreach (var p in recipients)
            {
                if (string.IsNullOrWhiteSpace(p.Email)) continue;

                bool isChallenger = challengerEmails.Contains(p.Email.ToLower());

                // Note: Assure-toi que cette classe correspond à ton client SMTP réel (ex: SmtpClient)
                IUTIL_SMTPClient oSmtp = new UTIL_SMTPClient();
                oSmtp.From(config.SenderEmail);
                oSmtp.To(p.Email);

                if (isChallenger)
                {
                    oSmtp.Subject(finalSubjectChallenger);
                    
                    // Pour le challenger, on veut lui dire contre qui il se bat (on l'exclut de la liste)
                    var opponents = challengers.Where(c => !c.Email.Equals(p.Email, StringComparison.OrdinalIgnoreCase)).ToList();
                    string opponentsPhrase = GetFormattedChallengers(config, opponents);
                    
                    oSmtp.AddHtmlBody(GenerateChallengerHtml(config, p.Name, opponentsPhrase, headerBase64, footerBase64));
                }
                else
                {
                    oSmtp.Subject(finalSubjectEater);
                    
                    // Pour les mangeurs, on affiche tout le monde avec les titres
                    oSmtp.AddHtmlBody(GenerateEaterHtml(config, p.Name, challengersAnnouncement, headerBase64, footerBase64));
                }

                oSmtp.Send();
            }
        }

        // ==========================================
        // 2. LOGIQUE DES TITRES ALÉATOIRES
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
                    availableTitles.RemoveAt(index); // On l'enlève pour ne pas avoir 2 fois le même titre
                }
                namedChallengers.Add($"<strong>{title}{c.Name}</strong>");
            }

            if (namedChallengers.Count == 1) return namedChallengers[0];
            
            string last = namedChallengers.Last();
            namedChallengers.RemoveAt(namedChallengers.Count - 1);
            
            return string.Join(", ", namedChallengers) + " et " + last;
        }

        // ==========================================
        // 3. TEMPLATE : LES CHALLENGERS
        // ==========================================
        private static string GenerateChallengerHtml(AppConfig config, string participantName, string opponents, string headerBase64, string footerBase64)
        {
            string headerImageHtml = string.IsNullOrEmpty(headerBase64) ? "" : 
                $@"<tr><td align='center' bgcolor='#FBEEE6'><img src='data:image/jpeg;base64,{headerBase64}' height='{config.ImageHeadingHeight}' style='display:block; border:none; max-width:100%;' alt='Header'></td></tr>";
                
            string footerImageHtml = string.IsNullOrEmpty(footerBase64) ? "" : 
                $@"<tr><td align='center' style='padding-top:20px;'><img src='data:image/jpeg;base64,{footerBase64}' style='display:block; border:none; max-width:150px;' alt='Footer'></td></tr>";

            return $@"
            <table role='presentation' width='100%' cellspacing='0' cellpadding='0' border='0' bgcolor='#f4f4f4'>
              <tr>
                <td align='center' style='padding: 20px 0;'>
                  <table role='presentation' width='600' cellspacing='0' cellpadding='0' border='0' bgcolor='#FBEEE6' style='border: 1px solid #dddddd; font-family: ""Segoe UI"", Arial, sans-serif;'>
                    {headerImageHtml}
                    <tr>
                      <td bgcolor='#D35400' align='center' style='padding: 20px; color: #ffffff;'>
                        <h1 style='margin: 0; font-size: 24px; letter-spacing: 2px;'>⚔️ LE DESTIN T'A CHOISI ⚔️</h1>
                      </td>
                    </tr>
                    <tr>
                      <td style='padding: 30px; color: #2C3E50; font-size: {config.FontSize}px; line-height: 1.6;'>
                        <p style='margin-top: 0;'>Félicitations <strong>{participantName}</strong> !</p>
                        <p>Tu as été tiré(e) au sort pour notre grand concours de pâtisserie. Prépare ton meilleur fouet et fais chauffer le four, car la bataille s'annonce épique.</p>
                        
                        <table role='presentation' width='100%' cellspacing='0' cellpadding='0' border='0' style='margin: 20px 0;'>
                          <tr>
                            <td bgcolor='#ffffff' style='padding: 15px; border-left: 4px solid #D35400;'>
                              <p style='margin: 0;'><strong>🎯 Thème :</strong> {config.ChallengeTheme}</p>
                              <p style='margin: 5px 0 0 0;'><strong>🥊 Tes adversaires :</strong> {opponents}</p>
                            </td>
                          </tr>
                        </table>

                        <p><strong>📅 Rendez-vous le :</strong> {config.ChallengeDate} à {config.ChallengeHour}</p>
                        <p><strong>📍 Lieu du combat :</strong> {config.ChallengeRoom}</p>
                        <p><strong>📜 Règles à respecter :</strong> {config.ChallengeRules}</p>
                        <p><strong>🏆 À gagner :</strong> {config.ChallengePrice}</p>

                        <p style='margin-top: 25px; color: #D35400; font-weight: bold; text-align: center;'>{config.ChallengeParticipationMessage}</p>
                        
                        {footerImageHtml}
                        
                        <p style='font-size: 12px; color: #7f8c8d; text-align: center; margin-top: 30px;'>
                          Cet email a été envoyé automatiquement par le Maître des Gâteaux.
                        </p>
                      </td>
                    </tr>
                  </table>
                </td>
              </tr>
            </table>";
        }

        // ==========================================
        // 4. TEMPLATE : LE JURY (MANGEURS)
        // ==========================================
       private static string GenerateEaterHtml(AppConfig config, string eaterName, string challengersAnnouncement, string headerBase64, string footerBase64)
{
    string headerRow = string.IsNullOrEmpty(headerBase64) ? "" : 
        $@"<tr><td align='center' style='padding-bottom:20px;'><img src='data:image/jpeg;base64,{headerBase64}' height='{config.ImageHeadingHeight}' style='display:block; border:none; max-width:100%;'></td></tr>";
        
    string footerRow = string.IsNullOrEmpty(footerBase64) ? "" : 
        $@"<tr><td align='center' style='padding-top:40px;'><img src='data:image/jpeg;base64,{footerBase64}' style='display:block; border:none; max-width:250px;'></td></tr>";

    return $@"
    <!DOCTYPE html>
    <html>
    <body style='margin:0; padding:0; background-color:#ffffff;'>
        <table role='presentation' width='100%' cellspacing='0' cellpadding='0' border='0'>
          <tr>
            <td align='center' style='padding: 20px 0;'>
              <table role='presentation' width='600' cellspacing='0' cellpadding='0' border='0' style='font-family: ""Trebuchet MS"", sans-serif; line-height: 1.8;'>
                
                {headerRow}

                <tr>
                  <td style='padding: 10px 20px;'>
                    
                    <h2 style='font-size: 22px; color: #444BAD; margin-bottom: 25px;'>
                      Cher/Chère <span style='color: #DB1616;'>{eaterName.ToUpper()}</span> 👮,
                    </h2>
                    
                    <p style='color: #444BAD; font-weight: bold; font-size: {config.FontSize}px;'>
                      Grande nouvelle (et pas moyen d'y échapper) : {challengersAnnouncement} ont été sélectionnés comme Challengers à notre grand concours de gâteaux sur le thème « 🍫 {config.ChallengeTheme} 🍫 » !
                    </p>
                    
                    <p style='color: #444BAD; font-size: {config.FontSize}px;'>
                      🧑‍🍳 <strong>Ils ont la consigne suivante :</strong> {config.ChallengeRules}
                    </p>
                    
                    <p style='color: #DB1616; font-size: 20px; font-weight: bold; text-align: center; margin: 35px 0;'>
                      {config.ChallengeParticipationMessage}
                    </p>
                    
                    <table role='presentation' width='100%' cellspacing='0' cellpadding='0' border='0' style='margin: 30px 0;'>
                      <tr>
                        <td bgcolor='#FCE4EC' style='padding: 30px; border-radius: 15px; border: 1px solid #F8BBD0;'>
                          <table role='presentation' width='100%' cellspacing='0' cellpadding='0' border='0'>
                            <tr>
                              <td width='60' valign='top' style='font-size: 28px; padding-bottom: 20px;'>🗓️</td>
                              <td style='padding-bottom: 20px; border-bottom: 1px solid #F8BBD0;'>
                                <span style='color: #444BAD; font-weight: bold; text-transform: uppercase; font-size: 11px; letter-spacing: 2px;'>Quand ?</span><br>
                                <span style='font-size: 19px; color: #333333; font-weight: bold;'>{config.ChallengeDate}</span>
                              </td>
                            </tr>
                            <tr>
                              <td width='60' valign='top' style='font-size: 28px; padding: 20px 0;'>📍</td>
                              <td style='padding: 20px 0; border-bottom: 1px solid #F8BBD0;'>
                                <span style='color: #444BAD; font-weight: bold; text-transform: uppercase; font-size: 11px; letter-spacing: 2px;'>Où se situe le combat ?</span><br>
                                <span style='font-size: 19px; color: #333333; font-weight: bold;'>{config.ChallengeRoom}</span>
                              </td>
                            </tr>
                            <tr>
                              <td width='60' valign='top' style='font-size: 28px; padding-top: 20px;'>⏰</td>
                              <td style='padding-top: 20px;'>
                                <span style='color: #444BAD; font-weight: bold; text-transform: uppercase; font-size: 11px; letter-spacing: 2px;'>À quelle heure ?</span><br>
                                <span style='font-size: 19px; color: #333333; font-weight: bold;'>{config.ChallengeHour}</span>
                              </td>
                            </tr>
                          </table>
                        </td>
                      </tr>
                    </table>

                    <table role='presentation' width='100%' cellspacing='0' cellpadding='0' border='0' bgcolor='#ffffff' style='border: 2px dashed #F8BBD0; border-radius: 15px; margin-bottom: 30px;'>
                      <tr>
                        <td align='center' style='padding: 20px; font-size: 16px; color: #333333;'>
                          Les règles sont simples : ils pâtissent, on déguste, et on élit le meilleur gâteau.<br>
                          Un prix fabuleux (ou presque) est à gagner :<br>
                          <span style='font-size: 18px; color: #DB1616; font-weight: bold;'>{config.ChallengePrice} 😋</span>
                        </td>
                      </tr>
                    </table>
                    
                    <table role='presentation' width='100%' cellspacing='0' cellpadding='15' border='0' bgcolor='#1A1A1A' style='border-radius: 8px;'>
                      <tr>
                        <td align='center' style='color: #ffffff; font-weight: bold; font-size: 16px; letter-spacing: 1.5px; text-transform: uppercase;'>
                          Préparez vos bavoirs, on a hâte !
                        </td>
                      </tr>
                    </table>

                    {footerRow}
                    
                    <p style='font-size: 10px; color: #A1A1A1; text-align: center; margin-top: 30px; border-top: 1px solid #eeeeee; padding-top: 15px;'>
                      You received this email because you suck.
                    </p>

                  </td>
                </tr>
              </table>
            </td>
          </tr>
        </table>
    </body>
    </html>";
}



        // ==========================================
        // 5. OUTIL : CONVERSION IMAGE EN BASE 64
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
        // 6. OUTIL : PRÉVISUALISATION
        // ==========================================
        public static string GetPreviewHtml(AppConfig config, bool isChallengerTemplate)
        {
            // On simule des données fictives pour l'aperçu
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
