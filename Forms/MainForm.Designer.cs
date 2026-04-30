using DuelDeGateaux.Services;

namespace DuelDeGateaux.Forms
{
    partial class MainForm
    {
        /// <summary>
        /// Zone de groupe contenant tous les champs liés au concours.
        /// Ce groupe contient les informations de base sur le concours comme la date,
        /// l'heure, le lieu, le thème, les règles, le prix et le message de participation.
        /// </summary>
        private GroupBox grpContest;

        /// <summary>
        /// Sélecteur de date pour la date du concours.
        /// Ce contrôle permet à l'utilisateur de sélectionner la date du concours
        /// de manière interactive.
        /// </summary>
        private DateTimePicker datePicker;

        /// <summary>
        /// Sélecteur de temps pour l'heure du concours.
        /// Ce contrôle permet à l'utilisateur de sélectionner l'heure du concours
        /// de manière interactive.
        /// </summary>
        private DateTimePicker timePicker;

        /// <summary>
        /// Champ salle
        /// </summary>
        private TextBox txtRoom;

        /// <summary>
        /// Champ thème
        /// </summary>
        private TextBox txtTheme;

        /// <summary>
        /// Champ règles
        /// </summary>
        private TextBox txtRules;

        /// <summary>
        /// Champ prix
        /// </summary>
        private TextBox txtPrice;

        /// <summary>
        /// Message obligatoire
        /// </summary>
        private TextBox txtParticipation;

        /// <summary>
        /// Liste des titres challengers
        /// </summary>
        private TextBox txtTitles;
        /// <summary>
        /// Radio bouton permettant la sélection de deux participants
        /// </summary>
        private RadioButton rb2Challengers;
        /// <summary>
        ///Radio bouton permettant la sélection de trois participants
        /// </summary>
        private RadioButton rb3Challengers;
        /// <summary>
        /// Groupe affichage
        /// </summary>
        private GroupBox grpDisplay;

        /// <summary>
        /// Taille de police
        /// </summary>
        private NumericUpDown numFontSize;

        /// <summary>
        /// Chemin image header
        /// </summary>
        private TextBox txtImageHeader;

        /// <summary>
        /// Bouton sélection image header
        /// </summary>
        private Button btnBrowseHeader;

        /// <summary>
        /// Apperçu de l'image du header
        /// </summary>
        private PictureBox pictureHeaderImage;

        /// <summary>
        /// Chemin image footer
        /// </summary>
        private TextBox txtImageFooter;

        /// <summary>
        /// Bouton sélection image footer
        /// </summary>
        private Button btnBrowseFooter;

        /// <summary>
        /// Apperçu de l'image du footer
        /// </summary>
        private PictureBox pictureFooterImage;

        /// <summary>
        /// Hauteur image header
        /// </summary>
        private NumericUpDown numImageHeight;       

        /// <summary>
        /// Groupe SMTP
        /// </summary>
        private GroupBox grpSmtp;

        /// <summary>
        /// Email expéditeur
        /// </summary>
        private TextBox txtSender;

        /// <summary>
        /// Mode test
        /// </summary>
        private CheckBox chkTest;

        /// <summary>
        /// Mail de test
        /// </summary>
        private TextBox txtTestMail;

        /// <summary>
        /// Sujet du mail pour les challengers
        /// </summary>
        private TextBox txtSubjectChallenger;

        /// <summary>
        /// Sujet du mail pour les mangeurs (jury)
        /// </summary>
        private TextBox txtSubjectEater;

        /// <summary>
        /// Zone contenant le DataGrid participant
        /// </summary>
        private GroupBox grpParticipants;

        /// <summary>
        /// Liste des participants dans une DatagriedView
        /// </summary>
        private DataGridView dgvParticipants;

        /// <summary>
        /// Bouton Ajout de particpant au DatagridView
        /// </summary>
        private Button btnAddParticipants;
        /// <summary>
        /// Boutons principaux
        /// </summary>
        private Button btnSend;
        private Button btnPreview;
        private Button btnSave;
        private Button btnOpenJson;
        private Button btnHistory;
        private Button btnPrintBallot;


        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            SuspendLayout();
            this.Load += new System.EventHandler(this.MainForm_Load);
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(9F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1200, 900);
            Name = "MainForm";
            Text = "Duel de Gâteaux 🍰";
            ResumeLayout(false);
            // =============================
            // 🧾 GROUPE CONCOURS
            // =============================
            grpContest = new GroupBox();
            grpContest.Text = "📅 Informations du concours";
            grpContest.SetBounds(10, 10, 450, 320);

            //Date 
            Label lblDate = new Label()
            {
                Text = "Date du concours :",
                Left = 20,
                Top = 30,
                Width = 150
            };
            datePicker = new DateTimePicker()
            {
                Left = 180,
                Top = 30,
                Width = 100,
                Format = DateTimePickerFormat.Short
            };
            //Heure 
            Label lblHour = new Label()
            {
                Text = "Heure du concours :",
                Left = 20,
                Top = 60,
                Width = 150
            };
            timePicker = new DateTimePicker()
            {
                Left = 180,
                Top = 60,
                Width = 50,
                Format = DateTimePickerFormat.Custom,
                CustomFormat = "HH:mm",
                ShowUpDown = true// permet sélection sans calendrier
            };

            //Salle
            Label lblRoom = new Label()
            {
                Text = "Salle/Lieu :",
                Left = 20,
                Top = 90,
                Width = 150
            };
            txtRoom = new TextBox()
            {
                Left = 180,
                Top = 90,
                Width = 250
            };

            //Thème
            Label lblTheme = new Label()
            {
                Text = "Thème du concours :",
                Left = 20,
                Top = 120,
                Width = 150
            };
            txtTheme = new TextBox()
            {
                Left = 180,
                Top = 120,
                Width = 250
            };


            //Règle
            Label lblRules = new Label()
            {
                Text = "Règles du concours :",
                Left = 20,
                Top = 150,
                Width = 150
            };
            txtRules = new TextBox()
            {
                Left = 180,
                Top = 150,
                Width = 250
            };

            //Prix
            Label lblPrice = new Label()
            {
                Text = "Récompense :",
                Left = 20,
                Top = 180,
                Width = 150
            };
            txtPrice = new TextBox()
            {
                Left = 180,
                Top = 180,
                Width = 250
            };


            //Participation obligatoire
            Label lblParticipation = new Label()
            {
                Text = "Présence obligatoire :",
                Left = 20,
                Top = 210,
                Width = 150
            };
            txtParticipation = new TextBox()
            {
                Left = 180,
                Top = 210,
                Width = 250
            };

            //Libéllés des challengers
            Label lblTitlesChallengers = new Label()
            {
                Text = "Libellé des challengers :",
                Left = 20,
                Top = 240,
                Width = 150
            };
            txtTitles = new TextBox()
            {
                Left = 180,
                Top = 240,
                Width = 250
            };


            //Sélection du nombre de challengers
            Label lblChallengerNumber = new Label()
            {
                Text = "Nombre de challengers :",
                Left = 20,
                Top = 270,
                Width = 150
            };
            rb2Challengers = new RadioButton()
            {
                Text = "2",
                AutoSize = true,
                Left = 180,
                Top = 270
            };
            rb3Challengers = new RadioButton()
            {
                Text = "3",
                AutoSize = true,
                Left = 220,
                Top = 270
            };


            //Ajout au groupe CONCOURS
            grpContest.Controls.AddRange(new Control[]
            {
                lblDate, datePicker,
                lblHour, timePicker,
                lblRoom, txtRoom,
                lblTheme, txtTheme,
                lblRules, txtRules,
                lblPrice, txtPrice,
                lblParticipation, txtParticipation,
                lblTitlesChallengers,txtTitles,
                lblChallengerNumber, rb2Challengers, rb3Challengers
            });

            // =============================
            // 🎨 GROUPE AFFICHAGE
            // =============================
            grpDisplay = new GroupBox();
            grpDisplay.Text = "🎨 Custom mail";
            grpDisplay.SetBounds(480, 10, 450, 320);

            //Taille de la police du mail
            Label lblFontSize = new Label()
            {
                Text = "Taille de la police :",
                Left = 20,
                Top = 30,
                Width = 150
            };
            numFontSize = new NumericUpDown()
            {
                Left = 200,
                Top = 30,
                Width = 80,
                Minimum = 0,
                Maximum = 1000,
                
            };

            //Hauteur du header du mail
            Label lblImageHeight = new Label()
            {
                Text = "Hauteur du header en PX :",
                Left = 20,
                Top = 60,
                Width = 150
            };
            numImageHeight = new NumericUpDown()
            {
                Left = 200,
                Top = 60,
                Width = 80,
                Minimum = 100,
                Maximum = 1000,

            };

            //Image du header du mail
            Label lblheader = new Label()
            {
                Text = "Image du Header :",
                Left = 20,
                Top = 90,
                Width = 150
            };
            txtImageHeader = new TextBox()
            {
                Left = 200,
                Top = 90,
                Width = 230
            };
            btnBrowseHeader = new Button()
            {
                Left = 20,
                Top = 120,
                Width = 150,
                Text = "Cherche des photos"
            };
            btnBrowseHeader.Click += BtnBrowseHeader_Click;


            pictureHeaderImage = new PictureBox()
            {
                MaximumSize = new Size(150, 60),
                Left = 200,
                Top = 120,
                Height = 60,
                Cursor = Cursors.Default,
                AllowDrop = true
            };
            pictureHeaderImage.DragEnter += PictureBox_DragEnter;
            pictureHeaderImage.DragDrop += (s, e) => PictureBox_DragDrop(s, e, txtImageHeader);
            pictureHeaderImage.MouseEnter += PictureBox_MouseEnter;
            pictureHeaderImage.MouseLeave += PictureBox_MouseLeave;            

            //Image du footer du mail
            Label lblfooter = new Label()
            {
                Text = "Image du footer :",
                Left = 20,
                Top = 210,
                Width = 150
            };
            txtImageFooter = new TextBox()
            {
                Left = 200,
                Top = 210,
                Width = 230
            };
            btnBrowseFooter = new Button()
            {
                Left = 20,
                Top = 240,
                Width = 150,
                Text = "Cherche des photos",
            };
            btnBrowseFooter.Click += BtnBrowseFooter_Click;

            pictureFooterImage = new PictureBox()
            {
                MaximumSize = new Size(150, 60),
                Left = 200,
                Top = 240,
                Height = 60,
                AllowDrop =true
            };
            pictureFooterImage.DragEnter += PictureBox_DragEnter;
            pictureFooterImage.DragDrop += (s, e) => PictureBox_DragDrop(s, e, txtImageFooter);
            pictureFooterImage.MouseEnter += PictureBox_MouseEnter;
            pictureFooterImage.MouseLeave += PictureBox_MouseLeave;

            grpDisplay.Controls.AddRange(new Control[]
            {
                lblFontSize,numFontSize, 
                lblheader, txtImageHeader, btnBrowseHeader,pictureHeaderImage,
                lblImageHeight, numImageHeight,
                lblfooter, txtImageFooter, btnBrowseFooter, pictureFooterImage
            });

            // =============================
            // 📧 GROUPE MAIL
            // =============================
            grpSmtp = new GroupBox();
            grpSmtp.Text = "📧 Mail";
            grpSmtp.SetBounds(10, 350, 450, 250); // Hauteur ajustée si besoin, 200 est parfait

            //Adresse mail de l'expéditeur
            Label lblSender = new Label()
            {
                Text = "Mail d'envoi :",
                Left = 20,
                Top = 30,
                Width = 150
            };
            txtSender = new TextBox()
            {
                Left = 180,
                Top = 30,
                Width = 250
            };
            
            //Checkbox du test
            chkTest = new CheckBox() 
            { 
                Text = "Mode Test", 
                Left = 20, 
                Top = 60
            };

            //Adresse mail du testeur
            Label lblTestMail = new Label()
            {
                Text = "Mail de test :",
                Left = 20,
                Top = 90,
                Width = 150
            };
            txtTestMail = new TextBox()
            {
                Left = 180,
                Top = 90,
                Width = 250
            };

            //Sujet mail Challenger
            Label lblSubjectChallenger = new Label()
            {
                Text = "Sujet Challenger :",
                Left = 20,
                Top = 120,
                Width = 150
            };
            txtSubjectChallenger = new TextBox()
            {
                Left = 180,
                Top = 120,
                Width = 250
            };

            //Sujet mail Mangeur
            Label lblSubjectEater = new Label()
            {
                Text = "Sujet Mangeur :",
                Left = 20,
                Top = 150,
                Width = 150
            };
            txtSubjectEater = new TextBox()
            {
                Left = 180,
                Top = 150,
                Width = 250
            };
           
            // Ajout de tous les contrôles au groupe
            grpSmtp.Controls.AddRange(new Control[]
            {
                lblSender, txtSender,
                chkTest, 
                lblTestMail, txtTestMail,
                lblSubjectChallenger, txtSubjectChallenger,
                lblSubjectEater, txtSubjectEater
            });



            // =============================
            // 👥 PARTICIPANTS
            // =============================
            grpParticipants = new GroupBox();
            grpParticipants.Text = "👥 Participants";
            grpParticipants.SetBounds(480, 350, 450, 250);

            dgvParticipants = new DataGridView ();
            dgvParticipants.SetBounds(15, 15, 440, 200);
            dgvParticipants.AutoGenerateColumns = false;
            dgvParticipants.Columns.Clear();
            dgvParticipants.RowHeadersVisible = false;
            dgvParticipants.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            ///Nom
            dgvParticipants.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Nom",
                DataPropertyName = "Name",
                Width = 100
            });
            /// Email
            dgvParticipants.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Email",
                DataPropertyName = "Email",
                Width = 190
            });
            // Email
            dgvParticipants.Columns.Add(new DataGridViewCheckBoxColumn
            {
                HeaderText = "Challenger ?",
                DataPropertyName = "IsEligible",
                Width = 90
            });
            //Colonne Suppression
            dgvParticipants.Columns.Add(new DataGridViewButtonColumn
            {
                HeaderText = "🗑️",
                Text = "🗑️",
                UseColumnTextForButtonValue = true,
                Width = 40
            });
            dgvParticipants.CellClick += new DataGridViewCellEventHandler(this.dgvParticipants_CellClick);
            dgvParticipants.Columns[dgvParticipants.ColumnCount-1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            //➕ Bouton Ajouter
            btnAddParticipants = new Button();
            btnAddParticipants.Text = "➕ Ajouter un participant";
            btnAddParticipants.SetBounds(5, 222, 120, 22);
            btnAddParticipants.Click += BtnAddParticipantsList_Click;
            btnAddParticipants.BackColor = Color.Transparent;
            btnAddParticipants.Font = new Font("Segoe UI Emoji", 6, FontStyle.Bold);

            //Ajout au control 
            grpParticipants.Controls.AddRange(new Control[]
           {
                dgvParticipants,
                btnAddParticipants
           });

            dgvParticipants.AllowUserToAddRows = false;
            dgvParticipants.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            // =============================
            // 🚀 BOUTONS PRINCIPAUX
            // =============================
            btnSend = new Button();
            btnSend.Text = "🚀 Envoyer";
            btnSend.SetBounds(10, 600, 140, 50); 
            btnSend.Click += BtnRun_Click;
            btnSend.BackColor = Color.LightSeaGreen;
            btnSend.Font = new Font("Segoe UI Emoji", 9, FontStyle.Bold);

            btnPreview = new Button();
            btnPreview.Text = "👁️Aperçu Mails";
            btnPreview.SetBounds(150, 600, 140, 50);
            btnPreview.Click += BtnPreview_Click;
            btnPreview.BackColor = Color.LightGoldenrodYellow;
            btnPreview.Font = new Font("Segoe UI Emoji", 9, FontStyle.Bold);

            btnSave = new Button();
            btnSave.Text = "💾 Sauvegarder";
            btnSave.SetBounds(300, 600, 140, 50);
            btnSave.Click += BtnSave_Click;
            btnSave.BackColor = Color.LightGreen;
            btnSave.Font = new Font("Segoe UI Emoji", 9, FontStyle.Bold);
           
            btnPrintBallot = new Button();
            btnPrintBallot.Text = "🖨️Bulletins";
            btnPrintBallot.SetBounds(450, 600, 140, 50);
            btnPrintBallot.Click += BtnPrintBallot_Click;
            btnPrintBallot.BackColor = Color.LightYellow;
            btnPrintBallot.Font = new Font("Segoe UI Emoji", 9, FontStyle.Bold);

            btnOpenJson = new Button();
            btnOpenJson.Text = "📂 Ouvrir JSON";
            btnOpenJson.SetBounds(600, 600, 140, 50);
            btnOpenJson.Click += BtnOpenjson_Click;
            btnOpenJson.BackColor = Color.LightBlue;
            btnOpenJson.Font = new Font("Segoe UI Emoji", 9, FontStyle.Bold);

            btnHistory = new Button();
            btnHistory.Text = "📖 Historique";
            btnHistory.SetBounds(750, 600, 140, 50);
            btnHistory.Click  += BtnHistory_Click;
            btnHistory.BackColor = Color.LightGray;
            btnHistory.Font = new Font("Segoe UI Emoji", 9, FontStyle.Bold);

            // =============================
            // AJOUT FINAL
            // =============================
            this.Controls.AddRange(new Control[]
            {
                grpContest, grpDisplay, grpSmtp,
                grpParticipants,
                btnSend, btnPreview, btnSave, btnPrintBallot, btnOpenJson, btnHistory
            });
            //Initlisation des tooltips
            InitTooltips();
        }

        #endregion
    }
}
