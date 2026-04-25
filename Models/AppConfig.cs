using DuelDeGateaux.Services;

namespace DuelDeGateaux
{
    partial class MainForm
    {
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
                Text = "Récompnse :",
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

            //Image du header du mail
            Label lblheader = new Label()
            {
                Text = "Image du Header :",
                Left = 20,
                Top = 60,
                Width = 150
            };
            txtImageHeader = new TextBox()
            {
                Left = 200,
                Top = 60,
                Width = 200
            };
            btnBrowseHeader = new Button()
            {
                Left = 20,
                Top = 90,
                Width = 150,
                Text = "Cherche des photos",
                Cursor = Cursors.SizeAll
            };
            btnBrowseHeader.Click += btnBrowseHeader_Click;


            pictureHeaderImage = new PictureBox()
            {
                MaximumSize = new Size(150, 60),
                Left = 200,
                Top = 90,
                Height = 60,
                Cursor = Cursors.No
            };

            //Hauteur du header du mail
            Label lblImageHeight = new Label()
            {
                Text = "Hauteur du header en PX :",
                Left = 20,
                Top = 180,
                Width = 150
            };
            numImageHeight = new NumericUpDown()
            {
                Left = 200,
                Top = 180,
                Width = 80,
                Minimum = 0,
                Maximum = 1000,

            };

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
                Width = 200
            };
            btnBrowseFooter = new Button()
            {
                Left = 20,
                Top = 240,
                Width = 150,
                Text = "Cherche des photos",
                Cursor = Cursors.AppStarting
            };
            btnBrowseFooter.Click += btnBrowseFooter_Click;

            pictureFooterImage = new PictureBox()
            {
                MaximumSize = new Size(150, 60),
                Left = 200,
                Top = 240,
                Height = 60,
                Cursor = Cursors.No
            };

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
            grpSmtp.SetBounds(10, 350, 450, 250);

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
                Width = 200
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
                Width = 200
            };
           
            grpSmtp.Controls.AddRange(new Control[]
            {
                lblSender,txtSender,
                chkTest, 
                lblTestMail, txtTestMail
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
            btnAddParticipants.Click += btnAddParticipantsList_Click;
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
            btnSend.SetBounds(10, 600, 200, 50);
            btnSend.Click += BtnRun_Click;
            btnSend.BackColor = Color.LightSeaGreen;
            btnSend.Font = new Font("Segoe UI Emoji", 9, FontStyle.Bold);

            btnSave = new Button();
            btnSave.Text = "💾 Sauvegarder";
            btnSave.SetBounds(220, 600, 200, 50);
            btnSave.Click += BtnSave_Click;
            btnSave.BackColor = Color.LightGreen;
            btnSave.Font = new Font("Segoe UI Emoji", 9, FontStyle.Bold);

            btnOpenJson = new Button();
            btnOpenJson.Text = "📂 Ouvrir JSON";
            btnOpenJson.SetBounds(430, 600, 200, 50);
            btnOpenJson.Click += BtnOpenjson_Click;
            btnOpenJson.BackColor = Color.LightBlue;
            btnOpenJson.Font = new Font("Segoe UI Emoji", 9, FontStyle.Bold);


            btnHistory = new Button();
            btnHistory.Text = "📖 Historique";
            btnHistory.SetBounds(640, 600, 200, 50);
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
                btnSend, btnSave, btnOpenJson, btnHistory
            });
            //Initlisation des tooltips
            InitTooltips();
        }

        #endregion
    }
}