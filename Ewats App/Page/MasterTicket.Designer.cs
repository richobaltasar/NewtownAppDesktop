namespace Ewats_App.Page
{
    partial class MasterTicket
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MasterTicket));
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.button1 = new System.Windows.Forms.Button();
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.dt_grid = new System.Windows.Forms.DataGridView();
            this.panel1 = new System.Windows.Forms.Panel();
            this.lblDiskon = new System.Windows.Forms.Label();
            this.lblNamaDiskon = new System.Windows.Forms.Label();
            this.lblDiskonId = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.label17 = new System.Windows.Forms.Label();
            this.label18 = new System.Windows.Forms.Label();
            this.LblHarga = new System.Windows.Forms.Label();
            this.lblNama = new System.Windows.Forms.Label();
            this.lblId = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.button14 = new System.Windows.Forms.Button();
            this.panel10 = new System.Windows.Forms.Panel();
            this.label5 = new System.Windows.Forms.Label();
            this.TxtQtxTicket = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.button19 = new System.Windows.Forms.Button();
            this.button18 = new System.Windows.Forms.Button();
            this.txtControl = new System.Windows.Forms.Label();
            this.button15 = new System.Windows.Forms.Button();
            this.button16 = new System.Windows.Forms.Button();
            this.button17 = new System.Windows.Forms.Button();
            this.button7 = new System.Windows.Forms.Button();
            this.button8 = new System.Windows.Forms.Button();
            this.button9 = new System.Windows.Forms.Button();
            this.button10 = new System.Windows.Forms.Button();
            this.button11 = new System.Windows.Forms.Button();
            this.button12 = new System.Windows.Forms.Button();
            this.button6 = new System.Windows.Forms.Button();
            this.button5 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button13 = new System.Windows.Forms.Button();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.dt_grid)).BeginInit();
            this.panel1.SuspendLayout();
            this.panel10.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.SuspendLayout();
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "business-color_payment_icon-icons.com_53442.png");
            this.imageList1.Images.SetKeyName(1, "images.jpg");
            this.imageList1.Images.SetKeyName(2, "JD-11-512.png");
            this.imageList1.Images.SetKeyName(3, "key_icon-01.png");
            this.imageList1.Images.SetKeyName(4, "splash-jungle-water-park-phuket.jpg.jpg");
            this.imageList1.Images.SetKeyName(5, "ticket-512.png");
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.DodgerBlue;
            this.button1.FlatAppearance.BorderSize = 0;
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.button1.Location = new System.Drawing.Point(324, 66);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(87, 48);
            this.button1.TabIndex = 6;
            this.button1.Text = "Cari";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // txtSearch
            // 
            this.txtSearch.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSearch.Location = new System.Drawing.Point(7, 81);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(307, 26);
            this.txtSearch.TabIndex = 5;
            this.txtSearch.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtSearch.TextChanged += new System.EventHandler(this.txtSearch_TextChanged);
            // 
            // dt_grid
            // 
            this.dt_grid.BackgroundColor = System.Drawing.Color.Gray;
            this.dt_grid.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dt_grid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dt_grid.Location = new System.Drawing.Point(7, 122);
            this.dt_grid.Name = "dt_grid";
            this.dt_grid.Size = new System.Drawing.Size(404, 437);
            this.dt_grid.TabIndex = 4;
            this.dt_grid.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dt_grid_CellClick);
            this.dt_grid.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dt_grid_CellContentClick);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Gray;
            this.panel1.Controls.Add(this.lblDiskon);
            this.panel1.Controls.Add(this.lblNamaDiskon);
            this.panel1.Controls.Add(this.lblDiskonId);
            this.panel1.Controls.Add(this.label8);
            this.panel1.Controls.Add(this.label12);
            this.panel1.Controls.Add(this.label15);
            this.panel1.Controls.Add(this.label16);
            this.panel1.Controls.Add(this.label17);
            this.panel1.Controls.Add(this.label18);
            this.panel1.Controls.Add(this.LblHarga);
            this.panel1.Controls.Add(this.lblNama);
            this.panel1.Controls.Add(this.lblId);
            this.panel1.Controls.Add(this.label9);
            this.panel1.Controls.Add(this.label10);
            this.panel1.Controls.Add(this.label11);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.panel1.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.panel1.Location = new System.Drawing.Point(417, 15);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(551, 101);
            this.panel1.TabIndex = 8;
            // 
            // lblDiskon
            // 
            this.lblDiskon.AutoSize = true;
            this.lblDiskon.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDiskon.Location = new System.Drawing.Point(377, 67);
            this.lblDiskon.Name = "lblDiskon";
            this.lblDiskon.Size = new System.Drawing.Size(14, 19);
            this.lblDiskon.TabIndex = 41;
            this.lblDiskon.Text = "-";
            // 
            // lblNamaDiskon
            // 
            this.lblNamaDiskon.AutoSize = true;
            this.lblNamaDiskon.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNamaDiskon.Location = new System.Drawing.Point(377, 46);
            this.lblNamaDiskon.Name = "lblNamaDiskon";
            this.lblNamaDiskon.Size = new System.Drawing.Size(14, 19);
            this.lblNamaDiskon.TabIndex = 40;
            this.lblNamaDiskon.Text = "-";
            // 
            // lblDiskonId
            // 
            this.lblDiskonId.AutoSize = true;
            this.lblDiskonId.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDiskonId.Location = new System.Drawing.Point(377, 25);
            this.lblDiskonId.Name = "lblDiskonId";
            this.lblDiskonId.Size = new System.Drawing.Size(14, 19);
            this.lblDiskonId.TabIndex = 39;
            this.lblDiskonId.Text = "-";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(361, 67);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(13, 19);
            this.label8.TabIndex = 38;
            this.label8.Text = ":";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label12.Location = new System.Drawing.Point(361, 46);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(13, 19);
            this.label12.TabIndex = 37;
            this.label12.Text = ":";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label15.Location = new System.Drawing.Point(361, 25);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(13, 19);
            this.label15.TabIndex = 36;
            this.label15.Text = ":";
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label16.Location = new System.Drawing.Point(258, 67);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(63, 19);
            this.label16.TabIndex = 35;
            this.label16.Text = "%Diskon";
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label17.Location = new System.Drawing.Point(258, 46);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(94, 19);
            this.label17.TabIndex = 34;
            this.label17.Text = "Nama Diskon";
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label18.Location = new System.Drawing.Point(258, 25);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(70, 19);
            this.label18.TabIndex = 33;
            this.label18.Text = "ID Diskon";
            // 
            // LblHarga
            // 
            this.LblHarga.AutoSize = true;
            this.LblHarga.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LblHarga.Location = new System.Drawing.Point(131, 62);
            this.LblHarga.Name = "LblHarga";
            this.LblHarga.Size = new System.Drawing.Size(14, 19);
            this.LblHarga.TabIndex = 28;
            this.LblHarga.Text = "-";
            // 
            // lblNama
            // 
            this.lblNama.AutoSize = true;
            this.lblNama.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNama.Location = new System.Drawing.Point(131, 41);
            this.lblNama.Name = "lblNama";
            this.lblNama.Size = new System.Drawing.Size(14, 19);
            this.lblNama.TabIndex = 27;
            this.lblNama.Text = "-";
            // 
            // lblId
            // 
            this.lblId.AutoSize = true;
            this.lblId.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblId.Location = new System.Drawing.Point(131, 20);
            this.lblId.Name = "lblId";
            this.lblId.Size = new System.Drawing.Size(14, 19);
            this.lblId.TabIndex = 26;
            this.lblId.Text = "-";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(115, 66);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(13, 19);
            this.label9.TabIndex = 23;
            this.label9.Text = ":";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.Location = new System.Drawing.Point(115, 43);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(13, 19);
            this.label10.TabIndex = 22;
            this.label10.Text = ":";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.Location = new System.Drawing.Point(115, 20);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(13, 19);
            this.label11.TabIndex = 21;
            this.label11.Text = ":";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(12, 66);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(48, 19);
            this.label4.TabIndex = 18;
            this.label4.Text = "Harga";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(12, 43);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(89, 19);
            this.label3.TabIndex = 17;
            this.label3.Text = "Nama Ticket";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(12, 20);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(23, 19);
            this.label2.TabIndex = 16;
            this.label2.Text = "ID";
            // 
            // button14
            // 
            this.button14.BackColor = System.Drawing.Color.DodgerBlue;
            this.button14.FlatAppearance.BorderSize = 0;
            this.button14.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button14.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button14.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.button14.Location = new System.Drawing.Point(341, 193);
            this.button14.Name = "button14";
            this.button14.Size = new System.Drawing.Size(199, 100);
            this.button14.TabIndex = 15;
            this.button14.Text = "Add";
            this.button14.UseVisualStyleBackColor = false;
            this.button14.Click += new System.EventHandler(this.button14_Click);
            // 
            // panel10
            // 
            this.panel10.BackColor = System.Drawing.Color.Gray;
            this.panel10.Controls.Add(this.label5);
            this.panel10.Controls.Add(this.TxtQtxTicket);
            this.panel10.Controls.Add(this.label6);
            this.panel10.Controls.Add(this.button19);
            this.panel10.Controls.Add(this.button18);
            this.panel10.Controls.Add(this.txtControl);
            this.panel10.Controls.Add(this.button15);
            this.panel10.Controls.Add(this.button16);
            this.panel10.Controls.Add(this.button17);
            this.panel10.Controls.Add(this.button7);
            this.panel10.Controls.Add(this.button14);
            this.panel10.Controls.Add(this.button8);
            this.panel10.Controls.Add(this.button9);
            this.panel10.Controls.Add(this.button10);
            this.panel10.Controls.Add(this.button11);
            this.panel10.Controls.Add(this.button12);
            this.panel10.Controls.Add(this.button6);
            this.panel10.Controls.Add(this.button5);
            this.panel10.Controls.Add(this.button4);
            this.panel10.Controls.Add(this.button3);
            this.panel10.Controls.Add(this.button2);
            this.panel10.Controls.Add(this.button13);
            this.panel10.Location = new System.Drawing.Point(417, 122);
            this.panel10.Name = "panel10";
            this.panel10.Size = new System.Drawing.Size(551, 437);
            this.panel10.TabIndex = 14;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.label5.Location = new System.Drawing.Point(115, 18);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(13, 20);
            this.label5.TabIndex = 43;
            this.label5.Text = ":";
            // 
            // TxtQtxTicket
            // 
            this.TxtQtxTicket.BackColor = System.Drawing.Color.Gray;
            this.TxtQtxTicket.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TxtQtxTicket.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.TxtQtxTicket.Location = new System.Drawing.Point(135, 15);
            this.TxtQtxTicket.Name = "TxtQtxTicket";
            this.TxtQtxTicket.Size = new System.Drawing.Size(405, 27);
            this.TxtQtxTicket.TabIndex = 33;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.label6.Location = new System.Drawing.Point(12, 18);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(98, 19);
            this.label6.TabIndex = 42;
            this.label6.Text = "Banyak Ticket";
            // 
            // button19
            // 
            this.button19.BackColor = System.Drawing.Color.Tomato;
            this.button19.FlatAppearance.BorderSize = 0;
            this.button19.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button19.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button19.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.button19.Location = new System.Drawing.Point(341, 324);
            this.button19.Name = "button19";
            this.button19.Size = new System.Drawing.Size(199, 100);
            this.button19.TabIndex = 32;
            this.button19.Text = "Batal";
            this.button19.UseVisualStyleBackColor = false;
            this.button19.Click += new System.EventHandler(this.button19_Click);
            // 
            // button18
            // 
            this.button18.BackColor = System.Drawing.Color.DarkViolet;
            this.button18.FlatAppearance.BorderSize = 0;
            this.button18.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button18.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button18.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.button18.Location = new System.Drawing.Point(341, 64);
            this.button18.Name = "button18";
            this.button18.Size = new System.Drawing.Size(199, 96);
            this.button18.TabIndex = 31;
            this.button18.Text = "Diskon";
            this.button18.UseVisualStyleBackColor = false;
            this.button18.Click += new System.EventHandler(this.button18_Click);
            // 
            // txtControl
            // 
            this.txtControl.AutoSize = true;
            this.txtControl.BackColor = System.Drawing.Color.Gray;
            this.txtControl.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.txtControl.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.txtControl.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.txtControl.Location = new System.Drawing.Point(52, 48);
            this.txtControl.Name = "txtControl";
            this.txtControl.Size = new System.Drawing.Size(10, 13);
            this.txtControl.TabIndex = 28;
            this.txtControl.Text = "-";
            this.txtControl.Visible = false;
            // 
            // button15
            // 
            this.button15.BackColor = System.Drawing.Color.Gray;
            this.button15.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button15.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold);
            this.button15.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.button15.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.button15.Location = new System.Drawing.Point(241, 351);
            this.button15.Name = "button15";
            this.button15.Size = new System.Drawing.Size(90, 70);
            this.button15.TabIndex = 27;
            this.button15.Text = "Enter";
            this.button15.UseVisualStyleBackColor = false;
            this.button15.Click += new System.EventHandler(this.button15_Click);
            // 
            // button16
            // 
            this.button16.BackColor = System.Drawing.Color.Gray;
            this.button16.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button16.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold);
            this.button16.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.button16.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.button16.Location = new System.Drawing.Point(148, 351);
            this.button16.Name = "button16";
            this.button16.Size = new System.Drawing.Size(90, 70);
            this.button16.TabIndex = 26;
            this.button16.Text = "Reset";
            this.button16.UseVisualStyleBackColor = false;
            this.button16.Click += new System.EventHandler(this.button16_Click);
            // 
            // button17
            // 
            this.button17.BackColor = System.Drawing.Color.Gray;
            this.button17.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button17.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold);
            this.button17.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.button17.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.button17.Location = new System.Drawing.Point(55, 351);
            this.button17.Name = "button17";
            this.button17.Size = new System.Drawing.Size(90, 70);
            this.button17.TabIndex = 25;
            this.button17.Text = "<-";
            this.button17.UseVisualStyleBackColor = false;
            this.button17.Click += new System.EventHandler(this.button17_Click);
            // 
            // button7
            // 
            this.button7.BackColor = System.Drawing.Color.Gray;
            this.button7.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button7.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold);
            this.button7.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.button7.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.button7.Location = new System.Drawing.Point(241, 279);
            this.button7.Name = "button7";
            this.button7.Size = new System.Drawing.Size(90, 70);
            this.button7.TabIndex = 24;
            this.button7.Text = "000";
            this.button7.UseVisualStyleBackColor = false;
            this.button7.Click += new System.EventHandler(this.button7_Click);
            // 
            // button8
            // 
            this.button8.BackColor = System.Drawing.Color.Gray;
            this.button8.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button8.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold);
            this.button8.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.button8.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.button8.Location = new System.Drawing.Point(148, 279);
            this.button8.Name = "button8";
            this.button8.Size = new System.Drawing.Size(90, 70);
            this.button8.TabIndex = 23;
            this.button8.Text = "00";
            this.button8.UseVisualStyleBackColor = false;
            this.button8.Click += new System.EventHandler(this.button8_Click);
            // 
            // button9
            // 
            this.button9.BackColor = System.Drawing.Color.Gray;
            this.button9.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button9.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold);
            this.button9.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.button9.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.button9.Location = new System.Drawing.Point(241, 208);
            this.button9.Name = "button9";
            this.button9.Size = new System.Drawing.Size(90, 70);
            this.button9.TabIndex = 22;
            this.button9.Text = "9";
            this.button9.UseVisualStyleBackColor = false;
            this.button9.Click += new System.EventHandler(this.button9_Click);
            // 
            // button10
            // 
            this.button10.BackColor = System.Drawing.Color.Gray;
            this.button10.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button10.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold);
            this.button10.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.button10.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.button10.Location = new System.Drawing.Point(148, 208);
            this.button10.Name = "button10";
            this.button10.Size = new System.Drawing.Size(90, 70);
            this.button10.TabIndex = 21;
            this.button10.Text = "8";
            this.button10.UseVisualStyleBackColor = false;
            this.button10.Click += new System.EventHandler(this.button10_Click);
            // 
            // button11
            // 
            this.button11.BackColor = System.Drawing.Color.Gray;
            this.button11.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button11.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold);
            this.button11.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.button11.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.button11.Location = new System.Drawing.Point(55, 279);
            this.button11.Name = "button11";
            this.button11.Size = new System.Drawing.Size(90, 70);
            this.button11.TabIndex = 20;
            this.button11.Text = "0";
            this.button11.UseVisualStyleBackColor = false;
            this.button11.Click += new System.EventHandler(this.button11_Click);
            // 
            // button12
            // 
            this.button12.BackColor = System.Drawing.Color.Gray;
            this.button12.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button12.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold);
            this.button12.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.button12.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.button12.Location = new System.Drawing.Point(55, 208);
            this.button12.Name = "button12";
            this.button12.Size = new System.Drawing.Size(90, 70);
            this.button12.TabIndex = 19;
            this.button12.Text = "7";
            this.button12.UseVisualStyleBackColor = false;
            this.button12.Click += new System.EventHandler(this.button12_Click);
            // 
            // button6
            // 
            this.button6.BackColor = System.Drawing.Color.Gray;
            this.button6.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button6.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold);
            this.button6.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.button6.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.button6.Location = new System.Drawing.Point(241, 136);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(90, 70);
            this.button6.TabIndex = 18;
            this.button6.Text = "6";
            this.button6.UseVisualStyleBackColor = false;
            this.button6.Click += new System.EventHandler(this.button6_Click);
            // 
            // button5
            // 
            this.button5.BackColor = System.Drawing.Color.Gray;
            this.button5.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button5.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold);
            this.button5.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.button5.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.button5.Location = new System.Drawing.Point(148, 136);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(90, 70);
            this.button5.TabIndex = 17;
            this.button5.Text = "5";
            this.button5.UseVisualStyleBackColor = false;
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // button4
            // 
            this.button4.BackColor = System.Drawing.Color.Gray;
            this.button4.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button4.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold);
            this.button4.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.button4.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.button4.Location = new System.Drawing.Point(241, 64);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(90, 70);
            this.button4.TabIndex = 16;
            this.button4.Text = "3";
            this.button4.UseVisualStyleBackColor = false;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // button3
            // 
            this.button3.BackColor = System.Drawing.Color.Gray;
            this.button3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button3.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold);
            this.button3.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.button3.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.button3.Location = new System.Drawing.Point(148, 64);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(90, 70);
            this.button3.TabIndex = 15;
            this.button3.Text = "2";
            this.button3.UseVisualStyleBackColor = false;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button2
            // 
            this.button2.BackColor = System.Drawing.Color.Gray;
            this.button2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold);
            this.button2.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.button2.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.button2.Location = new System.Drawing.Point(55, 136);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(90, 70);
            this.button2.TabIndex = 14;
            this.button2.Text = "4";
            this.button2.UseVisualStyleBackColor = false;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button13
            // 
            this.button13.BackColor = System.Drawing.Color.Gray;
            this.button13.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button13.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold);
            this.button13.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.button13.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.button13.Location = new System.Drawing.Point(55, 64);
            this.button13.Name = "button13";
            this.button13.Size = new System.Drawing.Size(90, 70);
            this.button13.TabIndex = 13;
            this.button13.Text = "1";
            this.button13.UseVisualStyleBackColor = false;
            this.button13.Click += new System.EventHandler(this.button13_Click);
            // 
            // pictureBox2
            // 
            this.pictureBox2.Image = global::Ewats_App.Properties.Resources.Login1;
            this.pictureBox2.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.pictureBox2.Location = new System.Drawing.Point(12, 27);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(29, 32);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox2.TabIndex = 68;
            this.pictureBox2.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Calibri", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.label1.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label1.Location = new System.Drawing.Point(42, 27);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(123, 29);
            this.label1.TabIndex = 67;
            this.label1.Text = "Data Ticket";
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.SystemColors.MenuHighlight;
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.ForeColor = System.Drawing.SystemColors.MenuHighlight;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(975, 5);
            this.panel2.TabIndex = 66;
            // 
            // MasterTicket
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.DimGray;
            this.ClientSize = new System.Drawing.Size(975, 595);
            this.Controls.Add(this.pictureBox2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panel10);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.txtSearch);
            this.Controls.Add(this.dt_grid);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "MasterTicket";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Load += new System.EventHandler(this.MasterTicket_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dt_grid)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel10.ResumeLayout(false);
            this.panel10.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.DataGridView dt_grid;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label LblHarga;
        private System.Windows.Forms.Label lblNama;
        private System.Windows.Forms.Label lblId;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button button14;
        private System.Windows.Forms.Panel panel10;
        private System.Windows.Forms.Label txtControl;
        private System.Windows.Forms.Button button15;
        private System.Windows.Forms.Button button16;
        private System.Windows.Forms.Button button17;
        private System.Windows.Forms.Button button7;
        private System.Windows.Forms.Button button8;
        private System.Windows.Forms.Button button9;
        private System.Windows.Forms.Button button10;
        private System.Windows.Forms.Button button11;
        private System.Windows.Forms.Button button12;
        private System.Windows.Forms.Button button6;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button13;
        private System.Windows.Forms.Button button18;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Label label18;
        public System.Windows.Forms.Label lblDiskon;
        public System.Windows.Forms.Label lblNamaDiskon;
        public System.Windows.Forms.Label lblDiskonId;
        private System.Windows.Forms.Button button19;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox TxtQtxTicket;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel2;
    }
}