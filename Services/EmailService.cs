using System.Net.Mail;
using DuelDeGateaux;
using DuelDeGateaux.Models;
using UTIL_SMTPLib;


namespace DuelDeGateaux
{

	internal static class Program
	{
		/// <summary>
		/// Point d'entrée de l'application
		/// </summary>
		[STAThread]
		static void Main()
		{
			Application.EnableVisualStyles();
			Application.Run(new MainForm());
		}

	}
}
//    class Program
//	{
//        #region Constante
//        //Adresse email à partir de laquelle est envoyé le mail
//        const string senderEmail = "Papy.Brossard@e-i.com";
//		//Date du concours
//		const string challengeDate = "28/04/2026";
//		//Objet du mail pour les deux challengeurs
//		const string subjectMailChallenger = "🥚🍰🥮🍮🍪 Duel de Gâteau - Félicitation vous êtes le Challenger du "+ challengeDate + " ! 🥚🍰🥮🍮🍪";
//		//Objet du mail pour les mangeurs
//		const string subjectMailEater = "🥚🍰🥮🍮🍪 Duel de Gâteau - Les challengers ont été sélectionnés pour le concours du "+ challengeDate + " ! 🥚🍰🥮🍮🍪";
//		//Activer ou non le mode test
//		const bool isTest = true;
//		//Mail qui va recevoir les tests
//		const string testerMail = "julien.allanic@e-i.com";
//		//heure du concours
//		const string challengeHour = "09h00";		
//		//Lieux du concours
//		const string challengeRoom = "21 Rue de Rieux Salle Lu";
//		//Thème du concours
//		const string challengeTheme = "🍋🍋‍ Gâteau moelleux au citron 🍋🍋‍";
//		//Message de participation obligatoire
//		const string challengeParticipationMessage = "Votre participation est obligatoire (sinon, la porte vous attend ☠️).";
//		//Règle pour le gâteau.
//		const string challengeRules = " Préparez le meilleur moelleux au citron si vous souhaitez avoir le respect de Tianning ou sinon il tuera vos plantes 💀";
//		//Annonce de la récompense du concours
//		const string challengePrice = " Un prix fabuleux (ou presque) est à gagner ! (Votre paricipation au prochain restaurant d'équipe vous est offerte, faites-vous plaisir !";
//        //Nombre de challengers pour le prochain concours
//		const int challengerNumber = 2;
//        // Liste des participants avec leurs e-mails (True veut dire challengers)
//        public static List<Participant> participants = new List<Participant>
//        {
//            new Participant("Julien 👑", "julien.allanic@e-i.com", false),
//            new Participant("Maxime R 🦘", "maxime.rivalin@e-i.com", false),
//			new Participant("Mathilde 🦉", "mathilde.bescond@e-i.com",true),
//			new Participant("Tianning 🦫","tianning.ma@externe.e-i.com",false),
//			new Participant("Benoît 🐗","benoit.kersual@e-i.com",false),
//			new Participant("Vincent 👮‍♂️","vincent.donze@e-i.com",true),
//			new Participant("Nicolas 🦄", "nicolas.charpentier@e-i.com", true),
//			new Participant("Dimitri 🦒", "dimitri.leurs@e-i.com",true ),
//            new Participant("Maxime Y 🐖", "maxime.you@e-i.com",true),
//			new Participant("Léa 🐈‍", "lea.helleboid@externe.e-i.com",true)
//		};		
//		//ImageHeading
//		const string pathImageHeading = "..\\..\\..\\Seconde manche - Duel de Gateau.png";
//        const int imageHeadingHeight = 750;
		
//		//BackGroundImage
//        //const string pathImageBackGround = "..\\..\\..\\BdayCakePromoWIDE4_1.jpg";

//        //BackGroundImage
//        const string pathImageFooter = "..\\..\\..\\Pole animation Q560.jpg";
//		//Taille de la police d'écriture de l'annonce
//		const int fontSize = 15;
//		//Liste des adjectifs utilisés pour qualifier les challengers dans le mail des eaters
//		private static readonly IReadOnlyList<string> challengersTitles = new[]
//		{
//			"L'incroyable",
//			"l'émérite",
//			"l'extraordinaire"
//		};
//        #endregion
//        //Liste encore vide des mangeurs
//        public static List<Participant> eaters;

//		static void Main(string[] args)
//		{

//            // Attribtuion du rôle de pâtissier et de mangeur parmis les participants
//            var assignments = AssignRoles();

//			string base64Image = ConvertImageToBase64(pathImageHeading);
//            // Envoi des e-mails aux challengers
//            NotifyChallengers(assignments, base64Image);

//            // Envoi des e-mails aux mangeurs
//            NotifyEaters(assignments, base64Image);

//			// Envoi des e-mails au mangeurs
//			Console.WriteLine("Cake challenge emails have been sent!");
//		}
//		static Dictionary<string, string> AssignRoles()
//		{
//			var assignments = new Dictionary<string, string>();
//			var names = participants.Where(p => p.IsChallenger).ToList();
//			var random = new Random();
//            for (int i = 0; i < challengerNumber; i++)
//            {
//				var fighter1 = names[random.Next(names.Count)];
//				assignments.Add(fighter1.Name, fighter1.Email);
//				names.Remove(fighter1);
//				Console.WriteLine($" Fighter ! {fighter1.Name} ");
//			}
//            eaters = participants.Where(p => !assignments.ContainsKey(p.Name)).ToList();
//			return assignments;
//		}

//		static void NotifyEaters(Dictionary<string, string> assignments, string base64Image)
//		{
//			string subject = subjectMailEater;
//			foreach (Participant participant in eaters)
//			{
//				string challengersAnnouncement = BuildChallengersAnnoucement(assignments);

//                string eater = participant.Name;
//				string emailTo = participant.Email;
//				string emailBody = @"
//                    <!DOCTYPE html><html xmlns:v=""urn:schemas-microsoft-com:vml"" xmlns:o=""urn:schemas-microsoft-com:office:office"" xmlns:w=""urn:schemas-microsoft-com:office:word"" xmlns:m=""http://schemas.microsoft.com/office/2004/12/omml"" xmlns=""http://www.w3.org/TR/REC-html40""><head>
//					<meta http-equiv=""Content-Type"" content=""text/html; charset=utf-8""><meta name=""Generator"" content=""Microsoft Word 15 (filtered medium)""><!--[if !mso]><style>v\:* {behavior:url(#default#VML);}
//					o\:* {behavior:url(#default#VML);}
//					w\:* {behavior:url(#default#VML);}
//					.shape {behavior:url(#default#VML);}
//					</style><![endif]--><style><!--
//					/* Font Definitions */
//					@font-face
//						{font-family:""Cambria Math"";
//						panose-1:2 4 5 3 5 4 6 3 2 4;}
//					@font-face
//						{font-family:Calibri;
//						panose-1:2 15 5 2 2 2 4 3 2 4;}
//					@font-face
//						{font-family:Verdana;
//						panose-1:2 11 6 4 3 5 4 4 2 4;}
//					@font-face
//						{font-family:""Segoe UI Emoji"";
//						panose-1:2 11 5 2 4 2 4 2 2 3;}
//					@font-face
//						{font-family:Tahoma;
//						panose-1:2 11 6 4 3 5 4 4 2 4;}
//					@font-face
//						{font-family:""Trebuchet MS"";
//						panose-1:2 11 6 3 2 2 2 2 2 4;}
//					/* Style Definitions */
//					p.MsoNormal, li.MsoNormal, div.MsoNormal
//						{margin:0cm;
//						font-size:11.0pt;
//						font-family:""Calibri"",sans-serif;}
//					span.EmailStyle22
//						{mso-style-type:personal-reply;
//						font-family:""Calibri"",sans-serif;
//						color:windowtext;}
//					.MsoChpDefault
//						{mso-style-type:export-only;
//						font-size:10.0pt;}
//					@page WordSection1
//						{size:612.0pt 792.0pt;
//						margin:70.85pt 70.85pt 70.85pt 70.85pt;}
//					div.WordSection1
//						{page:WordSection1;}
//					--></style><!--[if gte mso 9]><xml>
//					<o:shapedefaults v:ext=""edit"" spidmax=""1027"" />
//					</xml><![endif]--><!--[if gte mso 9]><xml>
//					<o:shapelayout v:ext=""edit"">
//					<o:idmap v:ext=""edit"" data=""1"" />
//					</o:shapelayout></xml><![endif]--></head><body lang=""FR"" link=""blue"" vlink=""purple"" style=""word-wrap:break-word""><div class=""WordSection1""><p class=""MsoNormal""><!--[if gte vml 1]><v:shapetype id=""_x0000_t75"" coordsize=""21600,21600"" o:spt=""75"" o:preferrelative=""t"" path=""m@4@5l@4@11@9@11@9@5xe"" filled=""f"" stroked=""f"">
//					<v:stroke joinstyle=""miter"" />
//					<v:formulas>
//					<v:f eqn=""if lineDrawn pixelLineWidth 0"" />
//					<v:f eqn=""sum @0 1 0"" />
//					<v:f eqn=""sum 0 0 @1"" />
//					<v:f eqn=""prod @2 1 2"" />
//					<v:f eqn=""prod @3 21600 pixelWidth"" />
//					<v:f eqn=""prod @3 21600 pixelHeight"" />
//					<v:f eqn=""sum @0 0 1"" />
//					<v:f eqn=""prod @6 1 2"" />
//					</v:formulas>
//					<v:path o:extrusionok=""f"" gradientshapeok=""t"" o:connecttype=""rect"" />
//					<o:lock v:ext=""edit"" aspectratio=""t"" />
//					</v:shapetype><v:shape id=""Image_x0020_1"" o:spid=""_x0000_s1026"" type=""#_x0000_t75"" alt=""Gros gâteau"" style='position:absolute;margin-left:0;margin-top:0;width:747.95pt;height:1684.3pt;z-index:-251658240;visibility:visible;mso-wrap-style:square;mso-width-percent:0;mso-height-percent:0;mso-left-percent:20;mso-top-percent:20;mso-wrap-distance-left:9pt;mso-wrap-distance-top:0;mso-wrap-distance-right:9pt;mso-wrap-distance-bottom:0;mso-position-horizontal-relative:page;mso-position-vertical-relative:page;mso-width-percent:0;mso-height-percent:0;mso-left-percent:20;mso-top-percent:20;mso-width-relative:page;mso-height-relative:page'>
//					<o:lock v:ext=""edit"" aspectratio=""f"" />
//					<w:wrap anchorx=""page"" anchory=""page""/>
//					</v:shape><![endif]--></p><div><p class=""MsoNormal""><span lang=""EN""><o:p>&nbsp;</o:p></span></p></div><table class=""MsoNormalTable"" border=""0"" cellspacing=""0"" cellpadding=""0"" width=""100%"" style=""width:100.0%;z-index:2""><tr><td style=""padding:0cm 0cm 0cm 0cm""><div align=""center""><table class=""MsoNormalTable"" border=""0"" cellspacing=""0"" cellpadding=""0"" width=""100%"" style=""width:100.0%;background-position-x:50%;background-position-y:0%""><tr><td style=""padding:0cm 0cm 0cm 0cm""><div align=""center""><table class=""MsoNormalTable"" border=""0"" cellspacing=""0"" cellpadding=""0"" width=""640"" style=""width:480.0pt;border-radius: 0""><tr><td width=""100%"" valign=""top"" style=""width:100.0%;padding:3.75pt 0cm 3.75pt 0cm""><table class=""MsoNormalTable"" border=""0"" cellspacing=""0"" cellpadding=""0"" width=""100%"" style=""width:100.0%""><tr><td width=""100%"" style=""width:100.0%;padding:0cm 0cm 0cm 0cm""><div><p class=""MsoNormal"" align=""center"" style=""text-align:center;mso-line-height-alt:10pt""><img width=""512"" height="""+ imageHeadingHeight +@""" style=""width:5.3333in"" id=""_x0000_i1028"" src=""data:image/jpeg;base64," + base64Image+@"""><o:p></o:p></p></div></td></tr></table><p class=""MsoNormal""><span style=""color:black;display:none""><o:p>&nbsp;</o:p></span></p><table class=""MsoNormalTable"" border=""0"" cellspacing=""0"" cellpadding=""0"" width=""100%"" style=""width:100.0%;word-break:break-word""><tr><td style=""padding:0cm 0cm 0cm 0cm""><p style=""mso-margin-top-alt:0cm;margin-right:0cm;margin-bottom:28.5pt;margin-left:0cm;line-height:32.65pt""><b><span style=""font-size:22.0pt;font-family:&quot;Trebuchet MS&quot;,sans-serif;color:#444BAD;letter-spacing:.75pt"">Cher/Chère</span></b><b><span style=""font-size:22.0pt;font-family:&quot;Trebuchet MS&quot;,sans-serif;color:#DB1616;letter-spacing:.75pt""> " + eater.ToUpperInvariant() + @"&nbsp;</span></b><b><span style=""font-size:22.0pt;font-family:&quot;Trebuchet MS&quot;,sans-serif;color:#444BAD;letter-spacing:.75pt"">,<o:p></o:p></span></b></p><p style=""mso-margin-top-alt:0cm;margin-right:0cm;margin-bottom:28.5pt;margin-left:0cm;line-height:32.65pt""><b><span style=""font-size:"+fontSize+@"pt;font-family:&quot;Trebuchet MS&quot;,sans-serif;color:#444BAD;letter-spacing:.75pt"">Grande nouvelle (et pas moyen d’y échapper) : "+ challengersAnnouncement + " ont été sélectionnés comme Challengers à notre grand concours de gâteaux sur le thème « "+challengeTheme+ @" » !<o:p></o:p></span></b></p><p style=""margin:0cm;line-height:32.65pt""><b><span style=""font-size:22.0pt;font-family:&quot;Segoe UI Emoji&quot;,sans-serif;color:#444BAD;letter-spacing:.75pt"">&#128104;</span></b><b><span style=""font-size:22.0pt;font-family:&quot;Arial&quot;,sans-serif;color:#444BAD;letter-spacing:.75pt"">‍</span></b><b><span style=""font-size:22.0pt;font-family:&quot;Segoe UI Emoji&quot;,sans-serif;color:#444BAD;letter-spacing:.75pt"">&#127859;</span></b><b><span style=""font-size:"+fontSize+@"pt;font-family:&quot;Trebuchet MS&quot;,sans-serif;color:#444BAD;letter-spacing:.75pt""> Ils ont la consigne suivante : "+challengeRules+@"<o:p></o:p></span></b></p></td></tr></table><p class=""MsoNormal""><span style=""color:black;display:none""><o:p>&nbsp;</o:p></span></p><table class=""MsoNormalTable"" border=""0"" cellspacing=""0"" cellpadding=""0"" width=""100%"" style=""width:100.0%;word-break:break-word""><tr><td style=""padding:15.0pt 15.0pt 15.0pt 15.0pt""><p align=""center"" style=""margin:0cm;text-align:center;line-height:26.1pt""><b><span style=""font-size:22.0pt;font-family:&quot;Verdana&quot;,sans-serif;color:#FF0041"">"+challengeParticipationMessage+@"</span></b><b><span style=""font-size:22.0pt;font-family:&quot;Verdana&quot;,sans-serif;color:#444A5B""><o:p></o:p></span></b></p></td></tr></table><p class=""MsoNormal""><span style=""color:black""><o:p>&nbsp;</o:p></span></p><table class=""MsoNormalTable"" border=""0"" cellspacing=""0"" cellpadding=""0"" width=""100%"" style=""width:100.0%""><tr><td style=""padding:15.0pt 7.5pt 11.25pt 7.5pt""><div align=""center""><table class=""MsoNormalTable"" border=""0"" cellspacing=""0"" cellpadding=""0"" width=""90%"" style=""width:90.0%""><tr><td style=""border:none;border-top:dotted white 4.5pt;padding:0cm 0cm 0cm 0cm""><p class=""MsoNormal"" style=""mso-line-height-alt:.75pt""><span style=""font-size:1.0pt""> <o:p></o:p></span></p></td></tr></table></div></td></tr></table><p class=""MsoNormal""><span style=""color:black;display:none""><o:p>&nbsp;</o:p></span></p><table class=""MsoNormalTable"" border=""0"" cellspacing=""0"" cellpadding=""0"" width=""100%"" style=""width:100.0%""><tr><td width=""100%"" style=""width:100.0%;padding:0cm 0cm 45.0pt 0cm""><div><p class=""MsoNormal"" align=""center"" style=""text-align:center;mso-line-height-alt:7.5pt""><img width=""320"" height=""155"" style=""width:3.3333in;height:1.6145in"" id=""_x0000_i1027"" src=""https://d1oco4z2z1fhwp.cloudfront.net/templates/default/1391/birthday-arrows.gif"" alt=""Alternate text""><o:p></o:p></p></div></td></tr></table><div><p class=""MsoNormal"" style=""line-height:60.0pt""><span style=""font-size:1.0pt;color:black""> <o:p></o:p></span></p></div><table class=""MsoNormalTable"" border=""0"" cellspacing=""0"" cellpadding=""0"" width=""100%"" style=""width:100.0%;word-break:break-word""><tr><td style=""padding:0cm 15.0pt 30.0pt 15.0pt""><p align=""center"" style=""margin:0cm;text-align:center;line-height:27.0pt;word-break:break-word""><strong><span style=""font-size:22.5pt;font-family:&quot;Arial&quot;,sans-serif;color:#333333;background:#FF99CC"">&nbsp;</span></strong><strong><span style=""font-size:22.5pt;font-family:&quot;Segoe UI Emoji&quot;,sans-serif;color:#333333;background:#FF99CC"">&#128197;</span></strong><strong><span style=""font-size:22.5pt;font-family:&quot;Arial&quot;,sans-serif;color:#333333;background:#FF99CC""> Quand? " + challengeDate + @"</span></strong><b><span style=""font-size:22.5pt;font-family:&quot;Arial&quot;,sans-serif;color:#333333;background:#FF99CC""><br></span></b><strong><span style=""font-size:22.5pt;font-family:&quot;Segoe UI Emoji&quot;,sans-serif;color:#333333;background:#FF99CC"">&#128205;</span></strong><strong><span style=""font-size:22.5pt;font-family:&quot;Arial&quot;,sans-serif;color:#333333;background:#FF99CC""> Où? " + challengeRoom + @"</span></strong><b><span style=""font-size:22.5pt;font-family:&quot;Arial&quot;,sans-serif;color:#333333;background:#FF99CC""><br></span></b><strong><span style=""font-size:22.5pt;font-family:&quot;Segoe UI Emoji&quot;,sans-serif;color:#333333;background:#FF99CC"">⏰</span></strong><strong><span style=""font-size:22.5pt;font-family:&quot;Arial&quot;,sans-serif;color:#333333;background:#FF99CC""> À quelle heure? " + challengeHour+ @"</span></strong><b><span style=""font-size:22.5pt;font-family:&quot;Arial&quot;,sans-serif;color:#333333;background:#FF99CC""><br><br></span></b><span style=""font-size:22.5pt;font-family:&quot;Arial&quot;,sans-serif;color:black""><o:p></o:p></span></p></td></tr></table><p class=""MsoNormal""><span style=""color:black;display:none""><o:p>&nbsp;</o:p></span></p><table class=""MsoNormalTable"" border=""0"" cellspacing=""0"" cellpadding=""0"" width=""100%"" style=""width:100.0%;word-break:break-word""><tr><td style=""padding:0cm 15.0pt 7.5pt 15.0pt""><p align=""center"" style=""margin:0cm;text-align:center;line-height:27.0pt;word-break:break-word""><strong><span style=""font-size:22.5pt;font-family:&quot;Arial&quot;,sans-serif;color:black;background:#FF99CC"">Les règles sont simples : ils pâtissent, on déguste, et on élit le meilleur gâteau." + challengePrice + @"</span></strong><strong><span style=""font-size:22.5pt;font-family:&quot;Segoe UI Emoji&quot;,sans-serif;color:black;background:#FF99CC"">&#128539;</span></strong><strong><span style=""font-size:22.5pt;font-family:&quot;Arial&quot;,sans-serif;color:black;background:#FF99CC"">).</span></strong><span style=""font-size:22.5pt;font-family:&quot;Arial&quot;,sans-serif;color:#555555""><o:p></o:p></span></p></td></tr></table><p class=""MsoNormal""><span style=""color:black;display:none""><o:p>&nbsp;</o:p></span></p><table class=""MsoNormalTable"" border=""0"" cellspacing=""0"" cellpadding=""0"" width=""100%"" style=""width:100.0%;word-break:break-word""><tr><td style=""padding:7.5pt 3.75pt 7.5pt 3.75pt""><p align=""center"" style=""margin:0cm;text-align:center;line-height:23.4pt;word-break:break-word""><strong><span style=""font-size:19.5pt;font-family:&quot;Tahoma&quot;,sans-serif;color:white;background:black"">&nbsp; Préparez vos bavoirs, on a hâte de voir ce qu'ils vont nous concocter !</span></strong><span style=""font-size:19.5pt;font-family:&quot;Tahoma&quot;,sans-serif;color:#434040""><o:p></o:p></span></p></td></tr></table><p class=""MsoNormal""><span style=""color:black;display:none""><o:p>&nbsp;</o:p></span></p><table class=""MsoNormalTable"" border=""0"" cellspacing=""0"" cellpadding=""0"" width=""100%"" style=""width:100.0%""><tr><td style=""padding:11.25pt 7.5pt 45.0pt 7.5pt""><p class=""MsoNormal"" align=""center"" style=""text-align:center""><a href=""https://wlib-SI.cm-cic.fr/?mnc=SRVIPX&typ=VIEWDOC&par=CodeObjet:9800118454;CodeSitePixis:99;Fede:11;Banque:10278;Guichet:00111"" target=""_blank""><span style=""color:white;text-decoration:none""><img border=""0"" width=""140"" height=""54"" style=""width:1.4583in;height:.5625in"" id=""_x0000_i1026"" src=""https://cdn.textstudio.com/output/sample/normal/4/6/5/7/see-you-soon-logo-600-17564.png"" alt=""À bientôt""></span></a><o:p></o:p></p></td></tr></table><p class=""MsoNormal""><span style=""color:black""><o:p>&nbsp;</o:p></span></p><table class=""MsoNormalTable"" border=""0"" cellspacing=""0"" cellpadding=""0"" width=""100%"" style=""width:100.0%""><tr><td style=""padding:45.0pt 7.5pt 30.0pt 7.5pt""><div align=""center""><table class=""MsoNormalTable"" border=""0"" cellspacing=""0"" cellpadding=""0"" width=""100%"" style=""width:100.0%""><tr><td style=""border:none;border-top:solid #BBBBBB 1.0pt;padding:0cm 0cm 0cm 0cm""><p class=""MsoNormal"" style=""mso-line-height-alt:.75pt""><span style=""font-size:1.0pt""> <o:p></o:p></span></p></td></tr></table></div></td></tr></table><div><p class=""MsoNormal"" style=""line-height:116.25pt""><span style=""font-size:1.0pt;color:black""> <o:p></o:p></span></p></div></td></tr></table></div></td></tr></table></div><p class=""MsoNormal""><o:p>&nbsp;</o:p></p><div align=""center""><table class=""MsoNormalTable"" border=""0"" cellspacing=""0"" cellpadding=""0"" width=""100%"" style=""width:100.0%""><tr><td style=""padding:0cm 0cm 0cm 0cm""><div align=""center""><table class=""MsoNormalTable"" border=""0"" cellspacing=""0"" cellpadding=""0"" width=""640"" style=""width:480.0pt""><tr><td width=""100%"" valign=""top"" style=""width:100.0%;padding:3.75pt 0cm 3.75pt 0cm""><table class=""MsoNormalTable"" border=""0"" cellspacing=""0"" cellpadding=""0"" width=""100%"" style=""width:100.0%""><tr><td width=""100%"" style=""width:100.0%;padding:0cm 0cm 0cm 0cm""><div><p class=""MsoNormal"" align=""center"" style=""text-align:center;mso-line-height-alt:7.5pt"">
//					<o:p></o:p></p><p class=MsoNormal style='mso-line-height-alt:7.5pt'><o:p>&nbsp;</o:p></p><p class=MsoNormal style='mso-line-height-alt:7.5pt'><o:p>&nbsp;</o:p></p><p class=MsoNormal align=center style='text-align:center;mso-line-height-alt:7.5pt'><img border=0 width=288 height=288 style='width:3.0in;height:3.0in' id=""_x0000_i1025"" src=""data:image/jpeg;base64," + ConvertImageToBase64(pathImageFooter)+@"""><o:p></o:p></p></div></td></tr></table><p class=MsoNormal><span style='color:black;display:none'><o:p>&nbsp;</o:p></span></p><table class=MsoNormalTable border=0 cellspacing=0 cellpadding=0 width=""100%"" style='width:100.0%;word-break:break-word'><tr><td style='padding:18.75pt 7.5pt 18.75pt 7.5pt'><p align=center style='margin:0cm;text-align:center;line-height:12.15pt'><span style='font-size:7.0pt;font-family:""Tahoma"",sans-serif;color:#A1A1A1'>You recieved this email because you suck.<o:p></o:p></span></p></td></tr></table><p class=MsoNormal><o:p></o:p></p></td></tr></table></div></td></tr></table></div><p class=MsoNormal align=center style='text-align:center'><o:p></o:p></p></td></tr></table><p class=MsoNormal><o:p>&nbsp;</o:p></p></div></body>
//					</html>";
//				var mailMessage = new MailMessage
//				{
//					From = new MailAddress(senderEmail),
//					Subject = subject,
//					Body = emailBody,
//					IsBodyHtml = true
//				};
//				mailMessage.To.Add(SetMailTo(emailTo));


//				try
//				{

//					SendMail(mailMessage);
//					Console.WriteLine($" Mail envoyé au mangeur {eater} à l'adresse mail {emailTo}");
//				}
//				catch (Exception ex)
//				{
//					Console.WriteLine($"Failed to send email to eater {eater}: {ex.Message}");
//				}
//			}

//		}

//		static void NotifyChallengers(Dictionary<string, string> assignments, string base64Image)
//		{		
//			// Subject
//			string subject = subjectMailChallenger;

//			foreach (var pair in assignments)
//			{				
//				string fighter = pair.Key;
//				string emailTo = pair.Value;
//				string emailBody = @"
//<!DOCTYPE html><html xmlns:v=""urn:schemas-microsoft-com:vml"" xmlns:o=""urn:schemas-microsoft-com:office:office"" xmlns:w=""urn:schemas-microsoft-com:office:word"" xmlns:m=""http://schemas.microsoft.com/office/2004/12/omml"" xmlns=""http://www.w3.org/TR/REC-html40""><head>
//<meta http-equiv=""Content-Type"" content=""text/html; charset=utf-8""><meta name=""Generator"" content=""Microsoft Word 15 (filtered medium)""><!--[if !mso]><style>v\:* {behavior:url(#default#VML);}
//o\:* {behavior:url(#default#VML);}
//w\:* {behavior:url(#default#VML);}
//.shape {behavior:url(#default#VML);}
//</style><![endif]--><style><!--
///* Font Definitions */
//@font-face
//	{font-family:""Cambria Math"";
//	panose-1:2 4 5 3 5 4 6 3 2 4;}
//@font-face
//	{font-family:Calibri;
//	panose-1:2 15 5 2 2 2 4 3 2 4;}
//@font-face
//	{font-family:Verdana;
//	panose-1:2 11 6 4 3 5 4 4 2 4;}
//@font-face
//	{font-family:""Segoe UI Emoji"";
//	panose-1:2 11 5 2 4 2 4 2 2 3;}
//@font-face
//	{font-family:Tahoma;
//	panose-1:2 11 6 4 3 5 4 4 2 4;}
//@font-face
//	{font-family:""Trebuchet MS"";
//	panose-1:2 11 6 3 2 2 2 2 2 4;}
///* Style Definitions */
//p.MsoNormal, li.MsoNormal, div.MsoNormal
//	{margin:0cm;
//	font-size:11.0pt;
//	font-family:""Calibri"",sans-serif;}
//span.EmailStyle22
//	{mso-style-type:personal-reply;
//	font-family:""Calibri"",sans-serif;
//	color:windowtext;}
//.MsoChpDefault
//	{mso-style-type:export-only;
//	font-size:10.0pt;}
//@page WordSection1
//	{size:612.0pt 792.0pt;
//	margin:70.85pt 70.85pt 70.85pt 70.85pt;}
//div.WordSection1
//	{page:WordSection1;}
//--></style><!--[if gte mso 9]><xml>
//<o:shapedefaults v:ext=""edit"" spidmax=""1027"" />
//</xml><![endif]--><!--[if gte mso 9]><xml>
//<o:shapelayout v:ext=""edit"">
//<o:idmap v:ext=""edit"" data=""1"" />
//</o:shapelayout></xml><![endif]--></head><body lang=""FR"" link=""blue"" vlink=""purple"" style=""word-wrap:break-word""><div class=""WordSection1""><p class=""MsoNormal""><!--[if gte vml 1]><v:shapetype id=""_x0000_t75"" coordsize=""21600,21600"" o:spt=""75"" o:preferrelative=""t"" path=""m@4@5l@4@11@9@11@9@5xe"" filled=""f"" stroked=""f"">
//<v:stroke joinstyle=""miter"" />
//<v:formulas>
//<v:f eqn=""if lineDrawn pixelLineWidth 0"" />
//<v:f eqn=""sum @0 1 0"" />
//<v:f eqn=""sum 0 0 @1"" />
//<v:f eqn=""prod @2 1 2"" />
//<v:f eqn=""prod @3 21600 pixelWidth"" />
//<v:f eqn=""prod @3 21600 pixelHeight"" />
//<v:f eqn=""sum @0 0 1"" />
//<v:f eqn=""prod @6 1 2"" />
//</v:formulas>
//<v:path o:extrusionok=""f"" gradientshapeok=""t"" o:connecttype=""rect"" />
//<o:lock v:ext=""edit"" aspectratio=""t"" />
//</v:shapetype><v:shape id=""Image_x0020_1"" o:spid=""_x0000_s1026"" type=""#_x0000_t75"" alt=""Gros gâteau"" style='position:absolute;margin-left:0;margin-top:0;width:747.95pt;height:1684.3pt;z-index:-251658240;visibility:visible;mso-wrap-style:square;mso-width-percent:0;mso-height-percent:0;mso-left-percent:20;mso-top-percent:20;mso-wrap-distance-left:9pt;mso-wrap-distance-top:0;mso-wrap-distance-right:9pt;mso-wrap-distance-bottom:0;mso-position-horizontal-relative:page;mso-position-vertical-relative:page;mso-width-percent:0;mso-height-percent:0;mso-left-percent:20;mso-top-percent:20;mso-width-relative:page;mso-height-relative:page'>
//<o:lock v:ext=""edit"" aspectratio=""f"" />
//<w:wrap anchorx=""page"" anchory=""page""/>
//</v:shape><![endif]--></p><div><p class=""MsoNormal""><span lang=""EN""><o:p>&nbsp;</o:p></span></p></div><table class=""MsoNormalTable"" border=""0"" cellspacing=""0"" cellpadding=""0"" width=""100%"" style=""width:100.0%;z-index:2""><tr><td style=""padding:0cm 0cm 0cm 0cm""><div align=""center""><table class=""MsoNormalTable"" border=""0"" cellspacing=""0"" cellpadding=""0"" width=""100%"" style=""width:100.0%;background-position-x:50%;background-position-y:0%""><tr><td style=""padding:0cm 0cm 0cm 0cm""><div align=""center""><table class=""MsoNormalTable"" border=""0"" cellspacing=""0"" cellpadding=""0"" width=""640"" style=""width:480.0pt;border-radius: 0""><tr><td width=""100%"" valign=""top"" style=""width:100.0%;padding:3.75pt 0cm 3.75pt 0cm""><table class=""MsoNormalTable"" border=""0"" cellspacing=""0"" cellpadding=""0"" width=""100%"" style=""width:100.0%""><tr><td width=""100%"" style=""width:100.0%;padding:0cm 0cm 0cm 0cm""><div><p class=""MsoNormal"" align=""center"" style=""text-align:center;mso-line-height-alt:10pt""><img width=""512"" height="""+ imageHeadingHeight +@""" style=""width:5.3333in;"" id=""_x0000_i1028"" src=""data:image/jpeg;base64," + base64Image+@"""><o:p></o:p></p></div></td></tr></table><p class=""MsoNormal""><span style=""color:black;display:none""><o:p>&nbsp;</o:p></span></p><table class=""MsoNormalTable"" border=""0"" cellspacing=""0"" cellpadding=""0"" width=""100%"" style=""width:100.0%;word-break:break-word""><tr><td style=""padding:0cm 0cm 0cm 0cm""><p style=""mso-margin-top-alt:0cm;margin-right:0cm;margin-bottom:28.5pt;margin-left:0cm;line-height:32.65pt""><b><span style=""font-size:22.0pt;font-family:&quot;Trebuchet MS&quot;,sans-serif;color:#444BAD;letter-spacing:.75pt"">Cher/Chère</span></b><b><span style=""font-size:22.0pt;font-family:&quot;Trebuchet MS&quot;,sans-serif;color:#DB1616;letter-spacing:.75pt""> " + fighter.ToUpperInvariant()+ @"&nbsp;</span></b><b><span style=""font-size:22.0pt;font-family:&quot;Trebuchet MS&quot;,sans-serif;color:#444BAD;letter-spacing:.75pt"">,<o:p></o:p></span></b></p><p style=""mso-margin-top-alt:0cm;margin-right:0cm;margin-bottom:28.5pt;margin-left:0cm;line-height:32.65pt""><b><span style=""font-size:"+fontSize+@"pt;font-family:&quot;Trebuchet MS&quot;,sans-serif;color:#444BAD;letter-spacing:.75pt"">Grande nouvelle (et pas moyen d’y échapper) : vous avez été sélectionné(e) pour participer à notre grand concours de gâteaux sur le thème « " + challengeTheme + @" » !<o:p></o:p></span></b></p><p style=""margin:0cm;line-height:32.65pt""><b><span style=""font-size:22.0pt;font-family:&quot;Segoe UI Emoji&quot;,sans-serif;color:#444BAD;letter-spacing:.75pt"">&#128104;</span></b><b><span style=""font-size:22.0pt;font-family:&quot;Arial&quot;,sans-serif;color:#444BAD;letter-spacing:.75pt"">‍</span></b><b><span style=""font-size:22.0pt;font-family:&quot;Segoe UI Emoji&quot;,sans-serif;color:#444BAD;letter-spacing:.75pt"">&#127859;</span></b><b><span style=""font-size:" + fontSize + @"pt;font-family:&quot;Trebuchet MS&quot;,sans-serif;color:#444BAD;letter-spacing:.75pt""> Vous devez : " + challengeRules + @"<o:p></o:p></span></b></p></td></tr></table><p class=""MsoNormal""><span style=""color:black;display:none""><o:p>&nbsp;</o:p></span></p><table class=""MsoNormalTable"" border=""0"" cellspacing=""0"" cellpadding=""0"" width=""100%"" style=""width:100.0%;word-break:break-word""><tr><td style=""padding:15.0pt 15.0pt 15.0pt 15.0pt""><p align=""center"" style=""margin:0cm;text-align:center;line-height:26.1pt""><b><span style=""font-size:22.0pt;font-family:&quot;Verdana&quot;,sans-serif;color:#FF0041"">" + challengeParticipationMessage + @"</span></b><b><span style=""font-size:22.0pt;font-family:&quot;Verdana&quot;,sans-serif;color:#444A5B""><o:p></o:p></span></b></p></td></tr></table><p class=""MsoNormal""><span style=""color:black""><o:p>&nbsp;</o:p></span></p><table class=""MsoNormalTable"" border=""0"" cellspacing=""0"" cellpadding=""0"" width=""100%"" style=""width:100.0%""><tr><td style=""padding:15.0pt 7.5pt 11.25pt 7.5pt""><div align=""center""><table class=""MsoNormalTable"" border=""0"" cellspacing=""0"" cellpadding=""0"" width=""90%"" style=""width:90.0%""><tr><td style=""border:none;border-top:dotted white 4.5pt;padding:0cm 0cm 0cm 0cm""><p class=""MsoNormal"" style=""mso-line-height-alt:.75pt""><span style=""font-size:1.0pt""> <o:p></o:p></span></p></td></tr></table></div></td></tr></table><p class=""MsoNormal""><span style=""color:black;display:none""><o:p>&nbsp;</o:p></span></p><table class=""MsoNormalTable"" border=""0"" cellspacing=""0"" cellpadding=""0"" width=""100%"" style=""width:100.0%""><tr><td width=""100%"" style=""width:100.0%;padding:0cm 0cm 45.0pt 0cm""><div><p class=""MsoNormal"" align=""center"" style=""text-align:center;mso-line-height-alt:7.5pt""><img width=""320"" height=""155"" style=""width:3.3333in;height:1.6145in"" id=""_x0000_i1027"" src=""https://d1oco4z2z1fhwp.cloudfront.net/templates/default/1391/birthday-arrows.gif"" alt=""Alternate text""><o:p></o:p></p></div></td></tr></table><div><p class=""MsoNormal"" style=""line-height:60.0pt""><span style=""font-size:1.0pt;color:black""> <o:p></o:p></span></p></div><table class=""MsoNormalTable"" border=""0"" cellspacing=""0"" cellpadding=""0"" width=""100%"" style=""width:100.0%;word-break:break-word""><tr><td style=""padding:0cm 15.0pt 30.0pt 15.0pt""><p align=""center"" style=""margin:0cm;text-align:center;line-height:27.0pt;word-break:break-word""><strong><span style=""font-size:22.5pt;font-family:&quot;Arial&quot;,sans-serif;color:#333333;background:#FF99CC"">&nbsp;</span></strong><strong><span style=""font-size:22.5pt;font-family:&quot;Segoe UI Emoji&quot;,sans-serif;color:#333333;background:#FF99CC"">&#128197;</span></strong><strong><span style=""font-size:22.5pt;font-family:&quot;Arial&quot;,sans-serif;color:#333333;background:#FF99CC""> Quand? " + challengeDate + @"</span></strong><b><span style=""font-size:22.5pt;font-family:&quot;Arial&quot;,sans-serif;color:#333333;background:#FF99CC""><br></span></b><strong><span style=""font-size:22.5pt;font-family:&quot;Segoe UI Emoji&quot;,sans-serif;color:#333333;background:#FF99CC"">&#128205;</span></strong><strong><span style=""font-size:22.5pt;font-family:&quot;Arial&quot;,sans-serif;color:#333333;background:#FF99CC""> Où? " + challengeRoom + @"</span></strong><b><span style=""font-size:22.5pt;font-family:&quot;Arial&quot;,sans-serif;color:#333333;background:#FF99CC""><br></span></b><strong><span style=""font-size:22.5pt;font-family:&quot;Segoe UI Emoji&quot;,sans-serif;color:#333333;background:#FF99CC"">⏰</span></strong><strong><span style=""font-size:22.5pt;font-family:&quot;Arial&quot;,sans-serif;color:#333333;background:#FF99CC""> À quelle heure? " + challengeHour + @"</span></strong><b><span style=""font-size:22.5pt;font-family:&quot;Arial&quot;,sans-serif;color:#333333;background:#FF99CC""><br><br></span></b><span style=""font-size:22.5pt;font-family:&quot;Arial&quot;,sans-serif;color:black""><o:p></o:p></span></p></td></tr></table><p class=""MsoNormal""><span style=""color:black;display:none""><o:p>&nbsp;</o:p></span></p><table class=""MsoNormalTable"" border=""0"" cellspacing=""0"" cellpadding=""0"" width=""100%"" style=""width:100.0%;word-break:break-word""><tr><td style=""padding:0cm 15.0pt 7.5pt 15.0pt""><p align=""center"" style=""margin:0cm;text-align:center;line-height:27.0pt;word-break:break-word""><strong><span style=""font-size:22.5pt;font-family:&quot;Arial&quot;,sans-serif;color:black;background:#FF99CC"">Les règles sont simples : vous pâtissez, on déguste, et on élit le meilleur gâteau."+challengePrice+@"  </span></strong><strong><span style=""font-size:22.5pt;font-family:&quot;Segoe UI Emoji&quot;,sans-serif;color:black;background:#FF99CC"">&#128539;</span></strong><strong><span style=""font-size:22.5pt;font-family:&quot;Arial&quot;,sans-serif;color:black;background:#FF99CC"">).</span></strong><span style=""font-size:22.5pt;font-family:&quot;Arial&quot;,sans-serif;color:#555555""><o:p></o:p></span></p></td></tr></table><p class=""MsoNormal""><span style=""color:black;display:none""><o:p>&nbsp;</o:p></span></p><table class=""MsoNormalTable"" border=""0"" cellspacing=""0"" cellpadding=""0"" width=""100%"" style=""width:100.0%;word-break:break-word""><tr><td style=""padding:7.5pt 3.75pt 7.5pt 3.75pt""><p align=""center"" style=""margin:0cm;text-align:center;line-height:23.4pt;word-break:break-word""><strong><span style=""font-size:19.5pt;font-family:&quot;Tahoma&quot;,sans-serif;color:white;background:black"">&nbsp;  À vos fouets et tabliers, on a hâte de voir ce que vous allez nous concocter !</span></strong><span style=""font-size:19.5pt;font-family:&quot;Tahoma&quot;,sans-serif;color:#434040""><o:p></o:p></span></p></td></tr></table><p class=""MsoNormal""><span style=""color:black;display:none""><o:p>&nbsp;</o:p></span></p><table class=""MsoNormalTable"" border=""0"" cellspacing=""0"" cellpadding=""0"" width=""100%"" style=""width:100.0%""><tr><td style=""padding:11.25pt 7.5pt 45.0pt 7.5pt""><p class=""MsoNormal"" align=""center"" style=""text-align:center""><a href=""https://wlib-SI.cm-cic.fr/?mnc=SRVIPX&typ=VIEWDOC&par=CodeObjet:9800118454;CodeSitePixis:99;Fede:11;Banque:10278;Guichet:00111"" target=""_blank""><span style=""color:white;text-decoration:none""><img border=""0"" width=""140"" height=""54"" style=""width:1.4583in;height:.5625in"" id=""_x0000_i1026"" src=""https://cdn.textstudio.com/output/sample/normal/4/6/5/7/see-you-soon-logo-600-17564.png"" alt=""À bientôt""></span></a><o:p></o:p></p></td></tr></table><p class=""MsoNormal""><span style=""color:black""><o:p>&nbsp;</o:p></span></p><table class=""MsoNormalTable"" border=""0"" cellspacing=""0"" cellpadding=""0"" width=""100%"" style=""width:100.0%""><tr><td style=""padding:45.0pt 7.5pt 30.0pt 7.5pt""><div align=""center""><table class=""MsoNormalTable"" border=""0"" cellspacing=""0"" cellpadding=""0"" width=""100%"" style=""width:100.0%""><tr><td style=""border:none;border-top:solid #BBBBBB 1.0pt;padding:0cm 0cm 0cm 0cm""><p class=""MsoNormal"" style=""mso-line-height-alt:.75pt""><span style=""font-size:1.0pt""> <o:p></o:p></span></p></td></tr></table></div></td></tr></table><div><p class=""MsoNormal"" style=""line-height:116.25pt""><span style=""font-size:1.0pt;color:black""> <o:p></o:p></span></p></div></td></tr></table></div></td></tr></table></div><p class=""MsoNormal""><o:p>&nbsp;</o:p></p><div align=""center""><table class=""MsoNormalTable"" border=""0"" cellspacing=""0"" cellpadding=""0"" width=""100%"" style=""width:100.0%""><tr><td style=""padding:0cm 0cm 0cm 0cm""><div align=""center""><table class=""MsoNormalTable"" border=""0"" cellspacing=""0"" cellpadding=""0"" width=""640"" style=""width:480.0pt""><tr><td width=""100%"" valign=""top"" style=""width:100.0%;padding:3.75pt 0cm 3.75pt 0cm""><table class=""MsoNormalTable"" border=""0"" cellspacing=""0"" cellpadding=""0"" width=""100%"" style=""width:100.0%""><tr><td width=""100%"" style=""width:100.0%;padding:0cm 0cm 0cm 0cm""><div><p class=""MsoNormal"" align=""center"" style=""text-align:center;mso-line-height-alt:7.5pt"">
//<o:p></o:p></p><p class=MsoNormal style='mso-line-height-alt:7.5pt'><o:p>&nbsp;</o:p></p><p class=MsoNormal style='mso-line-height-alt:7.5pt'><o:p>&nbsp;</o:p></p><p class=MsoNormal align=center style='text-align:center;mso-line-height-alt:7.5pt'><img border=0 width=288 height=288 style='width:3.0in;height:3.0in' id=""_x0000_i1025"" src=""data:image/jpeg;base64," + ConvertImageToBase64(pathImageFooter)+@"""><o:p></o:p></p></div></td></tr></table><p class=MsoNormal><span style='color:black;display:none'><o:p>&nbsp;</o:p></span></p><table class=MsoNormalTable border=0 cellspacing=0 cellpadding=0 width=""100%"" style='width:100.0%;word-break:break-word'><tr><td style='padding:18.75pt 7.5pt 18.75pt 7.5pt'><p align=center style='margin:0cm;text-align:center;line-height:12.15pt'><span style='font-size:7.0pt;font-family:""Tahoma"",sans-serif;color:#A1A1A1'>You recieved this email because you suck.<o:p></o:p></span></p></td></tr></table><p class=MsoNormal><o:p></o:p></p></td></tr></table></div></td></tr></table></div><p class=MsoNormal align=center style='text-align:center'><o:p></o:p></p></td></tr></table><p class=MsoNormal><o:p>&nbsp;</o:p></p></div></body>
//</html>";

//				var mailMessage = new MailMessage
//				{
//					From = new MailAddress(senderEmail),
//					Subject = subject,
//					Body = emailBody,
//					IsBodyHtml = true
//				};
				
//				mailMessage.To.Add(SetMailTo(emailTo));				

//				try
//				{
//					SendMail(mailMessage);
//					Console.WriteLine($" Mail envoyé au challenger {fighter} à l'adresse mail {emailTo}");
//				}
//				catch (Exception ex)
//				{
//					Console.WriteLine($"Failed to send email to  challenger {fighter}: {ex.Message}");
//				}
//			}

//		}
//		public static void SendMail(MailMessage message)
//		{
//			IUTIL_SMTPClient oSmtp = new UTIL_SMTPClient();

//			// From
//			oSmtp.From(senderEmail);

//			// Subject
//			oSmtp.Subject(message.Subject);

//			// Body
//			oSmtp.AddHtmlBody(message.Body);

//			// To
//			oSmtp.To(message.To.First().Address) ;
//			// Send
//			oSmtp.Send();

//		}
//		static private string SetMailTo(string mailto)
//        {
//			if (isTest)
//			{
//				return testerMail;
//			}
//			else
//			{
//				return mailto;
//			}

//		}
		
//        public static string ConvertImageToBase64(string imagePath)
//        {
//			byte[] imageBytes = File.ReadAllBytes(imagePath);
//			string base64String = Convert.ToBase64String(imageBytes);
//			return base64String;
//        }

//		private static string BuildChallengersAnnoucement(IReadOnlyDictionary<string, string> assignments)
//		{

//			if (assignments.Count < 2 || assignments.Count > 3)
//			{
//				throw new ArgumentException("assignments doit contenir 2 ou 3 pâtisser !", nameof(assignments));
//			}
//            List<string> parts = assignments.Keys.Select((name, index) => $"{challengersTitles[index]} {name}").ToList();
//            return parts.Count == 2 ? string.Join(" et ", parts) : $"{parts[0]} , {parts[1]} et {parts[2]}";
//		}
//    }

//}
