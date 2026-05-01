using DuelDeGateaux.Forms;

namespace DuelDeGateaux.UI
{
    public static class TooltipDefinitions
    {
        public static Dictionary<Control, string> Build(MainForm form)
        {
            return new()
            {
                // INFOS CONCOURS
                [form.DatePickerControl] = "Sélectionnez la date du concours.",
                [form.TimePickerControl] = "Sélectionnez l'heure du concours.",
                [form.RoomControl] = "Indiquez le lieu du concours.",
                [form.ThemeControl] = "Définissez le thème du concours.",
                [form.RulesControl] = "Décrivez les règles à respecter.",
                [form.PriceControl] = "Précisez la récompense du concours.",
                [form.ParticipationControl] = "Message envoyé aux participants.",
                [form.TitlesControl] =
                    "Liste des titres séparés par des virgules.\n" +
                    "Ex : Incroyable, Légendaire.\n" +
                    "Prévoyez assez de titres pour le nombre de challengers.",

                // AFFICHAGE
                [form.FontSizeControl] = "Taille du texte dans les emails.",
                [form.ImageHeightControl] = "Hauteur de l'image d'en-tête.",
                [form.HeaderImageControl] = "Glissez-déposez une image ici pour l'en-tête.",
                [form.FooterImageControl] = "Glissez-déposez une image ici pour le pied de mail.",

                // EMAIL
                [form.SenderControl] = "Adresse email expéditeur.",
                [form.TestModeControl] =
                    "Active le mode test.\nTous les emails seront envoyés vers l'adresse test.",
                [form.TestMailControl] = "Adresse email de réception test.",
                [form.SubjectChallengerControl] = "Sujet du mail destiné aux challengers.",
                [form.SubjectEaterControl] = "Sujet du mail destiné aux dégustateurs.",

                // PARTICIPANTS
                [form.AddParticipantControl] = "Ajoute un nouveau participant.",
                [form.ParticipantsGridControl] = "Liste des participants inscrits.",

                // CHALLENGERS
                [form.TwoChallengersControl] = "Sélectionnez un duel classique avec 2 challengers.",
                [form.ThreeChallengersControl] = "Sélectionnez un affrontement royal avec 3 challengers.",

                // BOUTONS
                [form.SendControl] = "Lance l'envoi des emails.",
                [form.PreviewControl] = "Prévisualisation des mails.",
                [form.SaveControl] = "Sauvegarde la configuration.",
                [form.PrintBallotControl] = "Imprime les bulletins.",
                [form.HistoryControl] = "Ouvre l'historique.",
                [form.OpenJsonControl] = "Ouvre le fichier JSON."
            };
        }
    }
}