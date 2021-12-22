namespace Ewats_App.Testing
{
    partial class TestReader
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
            this.cbReader = new System.Windows.Forms.ComboBox();
            this.button1 = new System.Windows.Forms.Button();
            this.mMsg = new System.Windows.Forms.RichTextBox();
            this.button2 = new System.Windows.Forms.Button();
            this.tKey1 = new System.Windows.Forms.TextBox();
            this.tKeyNum = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.button3 = new System.Windows.Forms.Button();
            this.tKey2 = new System.Windows.Forms.TextBox();
            this.tKey3 = new System.Windows.Forms.TextBox();
            this.tKey4 = new System.Windows.Forms.TextBox();
            this.tKey5 = new System.Windows.Forms.TextBox();
            this.tKey6 = new System.Windows.Forms.TextBox();
            this.gbAuth = new System.Windows.Forms.GroupBox();
            this.bAuth = new System.Windows.Forms.Button();
            this.tAuthenKeyNum = new System.Windows.Forms.TextBox();
            this.tBlkNo = new System.Windows.Forms.TextBox();
            this.Label5 = new System.Windows.Forms.Label();
            this.Label4 = new System.Windows.Forms.Label();
            this.gbKType = new System.Windows.Forms.GroupBox();
            this.rbKType2 = new System.Windows.Forms.RadioButton();
            this.rbKType1 = new System.Windows.Forms.RadioButton();
            this.gbBinOps = new System.Windows.Forms.GroupBox();
            this.bBinUpd = new System.Windows.Forms.Button();
            this.bBinRead = new System.Windows.Forms.Button();
            this.tBinData = new System.Windows.Forms.TextBox();
            this.Label9 = new System.Windows.Forms.Label();
            this.tBinLen = new System.Windows.Forms.TextBox();
            this.tBinBlk = new System.Windows.Forms.TextBox();
            this.Label8 = new System.Windows.Forms.Label();
            this.Label7 = new System.Windows.Forms.Label();
            this.gbValBlk = new System.Windows.Forms.GroupBox();
            this.bValRes = new System.Windows.Forms.Button();
            this.bValRead = new System.Windows.Forms.Button();
            this.bValDec = new System.Windows.Forms.Button();
            this.bValInc = new System.Windows.Forms.Button();
            this.bValStor = new System.Windows.Forms.Button();
            this.tValTar = new System.Windows.Forms.TextBox();
            this.tValSrc = new System.Windows.Forms.TextBox();
            this.tValBlk = new System.Windows.Forms.TextBox();
            this.tValAmt = new System.Windows.Forms.TextBox();
            this.Label13 = new System.Windows.Forms.Label();
            this.Label12 = new System.Windows.Forms.Label();
            this.Label11 = new System.Windows.Forms.Label();
            this.Label10 = new System.Windows.Forms.Label();
            this.gbAuth.SuspendLayout();
            this.gbKType.SuspendLayout();
            this.gbBinOps.SuspendLayout();
            this.gbValBlk.SuspendLayout();
            this.SuspendLayout();
            // 
            // cbReader
            // 
            this.cbReader.FormattingEnabled = true;
            this.cbReader.Location = new System.Drawing.Point(12, 12);
            this.cbReader.Name = "cbReader";
            this.cbReader.Size = new System.Drawing.Size(295, 21);
            this.cbReader.TabIndex = 0;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(314, 12);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 1;
            this.button1.Text = "Initialisasi";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // mMsg
            // 
            this.mMsg.Location = new System.Drawing.Point(524, 12);
            this.mMsg.Name = "mMsg";
            this.mMsg.Size = new System.Drawing.Size(264, 400);
            this.mMsg.TabIndex = 2;
            this.mMsg.Text = "";
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(12, 39);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 3;
            this.button2.Text = "Read Card";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // tKey1
            // 
            this.tKey1.Location = new System.Drawing.Point(82, 112);
            this.tKey1.MaxLength = 2;
            this.tKey1.Name = "tKey1";
            this.tKey1.Size = new System.Drawing.Size(26, 20);
            this.tKey1.TabIndex = 4;
            this.tKey1.Text = "FF";
            // 
            // tKeyNum
            // 
            this.tKeyNum.Location = new System.Drawing.Point(82, 86);
            this.tKeyNum.MaxLength = 2;
            this.tKeyNum.Name = "tKeyNum";
            this.tKeyNum.Size = new System.Drawing.Size(26, 20);
            this.tKeyNum.TabIndex = 5;
            this.tKeyNum.Text = "00";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 89);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "Key Number";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(49, 115);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(25, 13);
            this.label2.TabIndex = 7;
            this.label2.Text = "Key";
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(296, 89);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(104, 45);
            this.button3.TabIndex = 8;
            this.button3.Text = "Load Authorize Key";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // tKey2
            // 
            this.tKey2.Location = new System.Drawing.Point(114, 112);
            this.tKey2.MaxLength = 2;
            this.tKey2.Name = "tKey2";
            this.tKey2.Size = new System.Drawing.Size(26, 20);
            this.tKey2.TabIndex = 9;
            this.tKey2.Text = "FF";
            // 
            // tKey3
            // 
            this.tKey3.Location = new System.Drawing.Point(146, 112);
            this.tKey3.MaxLength = 2;
            this.tKey3.Name = "tKey3";
            this.tKey3.Size = new System.Drawing.Size(26, 20);
            this.tKey3.TabIndex = 10;
            this.tKey3.Text = "FF";
            // 
            // tKey4
            // 
            this.tKey4.Location = new System.Drawing.Point(178, 112);
            this.tKey4.MaxLength = 2;
            this.tKey4.Name = "tKey4";
            this.tKey4.Size = new System.Drawing.Size(26, 20);
            this.tKey4.TabIndex = 11;
            this.tKey4.Text = "FF";
            // 
            // tKey5
            // 
            this.tKey5.Location = new System.Drawing.Point(210, 112);
            this.tKey5.MaxLength = 2;
            this.tKey5.Name = "tKey5";
            this.tKey5.Size = new System.Drawing.Size(26, 20);
            this.tKey5.TabIndex = 12;
            this.tKey5.Text = "FF";
            // 
            // tKey6
            // 
            this.tKey6.Location = new System.Drawing.Point(242, 112);
            this.tKey6.MaxLength = 2;
            this.tKey6.Name = "tKey6";
            this.tKey6.Size = new System.Drawing.Size(26, 20);
            this.tKey6.TabIndex = 13;
            this.tKey6.Text = "FF";
            // 
            // gbAuth
            // 
            this.gbAuth.Controls.Add(this.bAuth);
            this.gbAuth.Controls.Add(this.tAuthenKeyNum);
            this.gbAuth.Controls.Add(this.tBlkNo);
            this.gbAuth.Controls.Add(this.Label5);
            this.gbAuth.Controls.Add(this.Label4);
            this.gbAuth.Controls.Add(this.gbKType);
            this.gbAuth.Location = new System.Drawing.Point(12, 135);
            this.gbAuth.Name = "gbAuth";
            this.gbAuth.Size = new System.Drawing.Size(410, 105);
            this.gbAuth.TabIndex = 45;
            this.gbAuth.TabStop = false;
            this.gbAuth.Text = "Authentication Function";
            // 
            // bAuth
            // 
            this.bAuth.Location = new System.Drawing.Point(284, 18);
            this.bAuth.Name = "bAuth";
            this.bAuth.Size = new System.Drawing.Size(104, 77);
            this.bAuth.TabIndex = 13;
            this.bAuth.Text = "Authenticate";
            this.bAuth.UseVisualStyleBackColor = true;
            this.bAuth.Click += new System.EventHandler(this.bAuth_Click);
            // 
            // tAuthenKeyNum
            // 
            this.tAuthenKeyNum.Location = new System.Drawing.Point(109, 52);
            this.tAuthenKeyNum.MaxLength = 2;
            this.tAuthenKeyNum.Name = "tAuthenKeyNum";
            this.tAuthenKeyNum.Size = new System.Drawing.Size(25, 20);
            this.tAuthenKeyNum.TabIndex = 6;
            this.tAuthenKeyNum.Text = "00";
            // 
            // tBlkNo
            // 
            this.tBlkNo.Location = new System.Drawing.Point(109, 25);
            this.tBlkNo.MaxLength = 3;
            this.tBlkNo.Name = "tBlkNo";
            this.tBlkNo.Size = new System.Drawing.Size(25, 20);
            this.tBlkNo.TabIndex = 5;
            this.tBlkNo.Text = "00";
            // 
            // Label5
            // 
            this.Label5.AutoSize = true;
            this.Label5.Location = new System.Drawing.Point(7, 52);
            this.Label5.Name = "Label5";
            this.Label5.Size = new System.Drawing.Size(93, 13);
            this.Label5.TabIndex = 3;
            this.Label5.Text = "Key Store Number";
            // 
            // Label4
            // 
            this.Label4.AutoSize = true;
            this.Label4.Location = new System.Drawing.Point(9, 28);
            this.Label4.Name = "Label4";
            this.Label4.Size = new System.Drawing.Size(80, 13);
            this.Label4.TabIndex = 2;
            this.Label4.Text = "Block No (Dec)";
            // 
            // gbKType
            // 
            this.gbKType.Controls.Add(this.rbKType2);
            this.gbKType.Controls.Add(this.rbKType1);
            this.gbKType.Location = new System.Drawing.Point(169, 9);
            this.gbKType.Name = "gbKType";
            this.gbKType.Size = new System.Drawing.Size(109, 86);
            this.gbKType.TabIndex = 1;
            this.gbKType.TabStop = false;
            this.gbKType.Text = "Key Type";
            // 
            // rbKType2
            // 
            this.rbKType2.AutoSize = true;
            this.rbKType2.Location = new System.Drawing.Point(16, 53);
            this.rbKType2.Name = "rbKType2";
            this.rbKType2.Size = new System.Drawing.Size(53, 17);
            this.rbKType2.TabIndex = 2;
            this.rbKType2.Text = "Key B";
            this.rbKType2.UseVisualStyleBackColor = true;
            // 
            // rbKType1
            // 
            this.rbKType1.AutoSize = true;
            this.rbKType1.Checked = true;
            this.rbKType1.Location = new System.Drawing.Point(16, 19);
            this.rbKType1.Name = "rbKType1";
            this.rbKType1.Size = new System.Drawing.Size(53, 17);
            this.rbKType1.TabIndex = 1;
            this.rbKType1.TabStop = true;
            this.rbKType1.Text = "Key A";
            this.rbKType1.UseVisualStyleBackColor = true;
            // 
            // gbBinOps
            // 
            this.gbBinOps.Controls.Add(this.bBinUpd);
            this.gbBinOps.Controls.Add(this.bBinRead);
            this.gbBinOps.Controls.Add(this.tBinData);
            this.gbBinOps.Controls.Add(this.Label9);
            this.gbBinOps.Controls.Add(this.tBinLen);
            this.gbBinOps.Controls.Add(this.tBinBlk);
            this.gbBinOps.Controls.Add(this.Label8);
            this.gbBinOps.Controls.Add(this.Label7);
            this.gbBinOps.Location = new System.Drawing.Point(10, 246);
            this.gbBinOps.Name = "gbBinOps";
            this.gbBinOps.Size = new System.Drawing.Size(412, 112);
            this.gbBinOps.TabIndex = 46;
            this.gbBinOps.TabStop = false;
            this.gbBinOps.Text = "Binary Block Functions";
            // 
            // bBinUpd
            // 
            this.bBinUpd.Location = new System.Drawing.Point(286, 61);
            this.bBinUpd.Name = "bBinUpd";
            this.bBinUpd.Size = new System.Drawing.Size(104, 43);
            this.bBinUpd.TabIndex = 19;
            this.bBinUpd.Text = "Update Block";
            this.bBinUpd.UseVisualStyleBackColor = true;
            this.bBinUpd.Click += new System.EventHandler(this.bBinUpd_Click);
            // 
            // bBinRead
            // 
            this.bBinRead.Location = new System.Drawing.Point(286, 12);
            this.bBinRead.Name = "bBinRead";
            this.bBinRead.Size = new System.Drawing.Size(104, 43);
            this.bBinRead.TabIndex = 18;
            this.bBinRead.Text = "Read Block";
            this.bBinRead.UseVisualStyleBackColor = true;
            this.bBinRead.Click += new System.EventHandler(this.bBinRead_Click);
            // 
            // tBinData
            // 
            this.tBinData.Location = new System.Drawing.Point(12, 78);
            this.tBinData.Name = "tBinData";
            this.tBinData.Size = new System.Drawing.Size(252, 20);
            this.tBinData.TabIndex = 17;
            // 
            // Label9
            // 
            this.Label9.AutoSize = true;
            this.Label9.Location = new System.Drawing.Point(9, 62);
            this.Label9.Name = "Label9";
            this.Label9.Size = new System.Drawing.Size(60, 13);
            this.Label9.TabIndex = 16;
            this.Label9.Text = "Data (Text)";
            // 
            // tBinLen
            // 
            this.tBinLen.Location = new System.Drawing.Point(231, 25);
            this.tBinLen.MaxLength = 2;
            this.tBinLen.Name = "tBinLen";
            this.tBinLen.Size = new System.Drawing.Size(33, 20);
            this.tBinLen.TabIndex = 15;
            this.tBinLen.Text = "16";
            this.tBinLen.TextChanged += new System.EventHandler(this.tBinLen_TextChanged);
            // 
            // tBinBlk
            // 
            this.tBinBlk.Location = new System.Drawing.Point(101, 25);
            this.tBinBlk.MaxLength = 2;
            this.tBinBlk.Name = "tBinBlk";
            this.tBinBlk.Size = new System.Drawing.Size(33, 20);
            this.tBinBlk.TabIndex = 14;
            this.tBinBlk.Text = "00";
            // 
            // Label8
            // 
            this.Label8.AutoSize = true;
            this.Label8.Location = new System.Drawing.Point(156, 28);
            this.Label8.Name = "Label8";
            this.Label8.Size = new System.Drawing.Size(69, 13);
            this.Label8.TabIndex = 1;
            this.Label8.Text = "Length (Dec)";
            // 
            // Label7
            // 
            this.Label7.AutoSize = true;
            this.Label7.Location = new System.Drawing.Point(7, 28);
            this.Label7.Name = "Label7";
            this.Label7.Size = new System.Drawing.Size(88, 13);
            this.Label7.TabIndex = 0;
            this.Label7.Text = "Start Block (Dec)";
            // 
            // gbValBlk
            // 
            this.gbValBlk.Controls.Add(this.bValRes);
            this.gbValBlk.Controls.Add(this.bValRead);
            this.gbValBlk.Controls.Add(this.bValDec);
            this.gbValBlk.Controls.Add(this.bValInc);
            this.gbValBlk.Controls.Add(this.bValStor);
            this.gbValBlk.Controls.Add(this.tValTar);
            this.gbValBlk.Controls.Add(this.tValSrc);
            this.gbValBlk.Controls.Add(this.tValBlk);
            this.gbValBlk.Controls.Add(this.tValAmt);
            this.gbValBlk.Controls.Add(this.Label13);
            this.gbValBlk.Controls.Add(this.Label12);
            this.gbValBlk.Controls.Add(this.Label11);
            this.gbValBlk.Controls.Add(this.Label10);
            this.gbValBlk.Location = new System.Drawing.Point(10, 364);
            this.gbValBlk.Name = "gbValBlk";
            this.gbValBlk.Size = new System.Drawing.Size(412, 168);
            this.gbValBlk.TabIndex = 47;
            this.gbValBlk.TabStop = false;
            this.gbValBlk.Text = "Value Block Functions";
            // 
            // bValRes
            // 
            this.bValRes.Location = new System.Drawing.Point(237, 133);
            this.bValRes.Name = "bValRes";
            this.bValRes.Size = new System.Drawing.Size(101, 23);
            this.bValRes.TabIndex = 27;
            this.bValRes.Text = "Restore Value";
            this.bValRes.UseVisualStyleBackColor = true;
            this.bValRes.Click += new System.EventHandler(this.bValRes_Click);
            // 
            // bValRead
            // 
            this.bValRead.Location = new System.Drawing.Point(237, 48);
            this.bValRead.Name = "bValRead";
            this.bValRead.Size = new System.Drawing.Size(101, 23);
            this.bValRead.TabIndex = 26;
            this.bValRead.Text = "Read Value";
            this.bValRead.UseVisualStyleBackColor = true;
            this.bValRead.Click += new System.EventHandler(this.bValRead_Click);
            // 
            // bValDec
            // 
            this.bValDec.Location = new System.Drawing.Point(237, 108);
            this.bValDec.Name = "bValDec";
            this.bValDec.Size = new System.Drawing.Size(101, 23);
            this.bValDec.TabIndex = 25;
            this.bValDec.Text = "Decrement";
            this.bValDec.UseVisualStyleBackColor = true;
            this.bValDec.Click += new System.EventHandler(this.bValDec_Click);
            // 
            // bValInc
            // 
            this.bValInc.Location = new System.Drawing.Point(237, 82);
            this.bValInc.Name = "bValInc";
            this.bValInc.Size = new System.Drawing.Size(101, 23);
            this.bValInc.TabIndex = 24;
            this.bValInc.Text = "Increment";
            this.bValInc.UseVisualStyleBackColor = true;
            this.bValInc.Click += new System.EventHandler(this.bValInc_Click);
            // 
            // bValStor
            // 
            this.bValStor.Location = new System.Drawing.Point(237, 22);
            this.bValStor.Name = "bValStor";
            this.bValStor.Size = new System.Drawing.Size(101, 23);
            this.bValStor.TabIndex = 23;
            this.bValStor.Text = "Store Value";
            this.bValStor.UseVisualStyleBackColor = true;
            this.bValStor.Click += new System.EventHandler(this.bValStor_Click);
            // 
            // tValTar
            // 
            this.tValTar.Location = new System.Drawing.Point(96, 104);
            this.tValTar.MaxLength = 2;
            this.tValTar.Name = "tValTar";
            this.tValTar.Size = new System.Drawing.Size(33, 20);
            this.tValTar.TabIndex = 22;
            // 
            // tValSrc
            // 
            this.tValSrc.Location = new System.Drawing.Point(96, 79);
            this.tValSrc.MaxLength = 2;
            this.tValSrc.Name = "tValSrc";
            this.tValSrc.Size = new System.Drawing.Size(33, 20);
            this.tValSrc.TabIndex = 21;
            // 
            // tValBlk
            // 
            this.tValBlk.Location = new System.Drawing.Point(96, 53);
            this.tValBlk.MaxLength = 2;
            this.tValBlk.Name = "tValBlk";
            this.tValBlk.Size = new System.Drawing.Size(33, 20);
            this.tValBlk.TabIndex = 20;
            this.tValBlk.Text = "00";
            // 
            // tValAmt
            // 
            this.tValAmt.Location = new System.Drawing.Point(96, 24);
            this.tValAmt.Name = "tValAmt";
            this.tValAmt.Size = new System.Drawing.Size(111, 20);
            this.tValAmt.TabIndex = 4;
            // 
            // Label13
            // 
            this.Label13.AutoSize = true;
            this.Label13.Location = new System.Drawing.Point(17, 111);
            this.Label13.Name = "Label13";
            this.Label13.Size = new System.Drawing.Size(68, 13);
            this.Label13.TabIndex = 3;
            this.Label13.Text = "Target Block";
            // 
            // Label12
            // 
            this.Label12.AutoSize = true;
            this.Label12.Location = new System.Drawing.Point(17, 82);
            this.Label12.Name = "Label12";
            this.Label12.Size = new System.Drawing.Size(71, 13);
            this.Label12.TabIndex = 2;
            this.Label12.Text = "Source Block";
            // 
            // Label11
            // 
            this.Label11.AutoSize = true;
            this.Label11.Location = new System.Drawing.Point(17, 53);
            this.Label11.Name = "Label11";
            this.Label11.Size = new System.Drawing.Size(54, 13);
            this.Label11.TabIndex = 1;
            this.Label11.Text = "Block No.";
            // 
            // Label10
            // 
            this.Label10.AutoSize = true;
            this.Label10.Location = new System.Drawing.Point(17, 27);
            this.Label10.Name = "Label10";
            this.Label10.Size = new System.Drawing.Size(73, 13);
            this.Label10.TabIndex = 0;
            this.Label10.Text = "Value Amount";
            // 
            // TestReader
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 564);
            this.Controls.Add(this.gbValBlk);
            this.Controls.Add(this.gbBinOps);
            this.Controls.Add(this.gbAuth);
            this.Controls.Add(this.tKey6);
            this.Controls.Add(this.tKey5);
            this.Controls.Add(this.tKey4);
            this.Controls.Add(this.tKey3);
            this.Controls.Add(this.tKey2);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tKeyNum);
            this.Controls.Add(this.tKey1);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.mMsg);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.cbReader);
            this.Name = "TestReader";
            this.Text = "TestReader";
            this.Load += new System.EventHandler(this.TestReader_Load);
            this.gbAuth.ResumeLayout(false);
            this.gbAuth.PerformLayout();
            this.gbKType.ResumeLayout(false);
            this.gbKType.PerformLayout();
            this.gbBinOps.ResumeLayout(false);
            this.gbBinOps.PerformLayout();
            this.gbValBlk.ResumeLayout(false);
            this.gbValBlk.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cbReader;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.RichTextBox mMsg;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.TextBox tKey1;
        private System.Windows.Forms.TextBox tKeyNum;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.TextBox tKey2;
        private System.Windows.Forms.TextBox tKey3;
        private System.Windows.Forms.TextBox tKey4;
        private System.Windows.Forms.TextBox tKey5;
        private System.Windows.Forms.TextBox tKey6;
        internal System.Windows.Forms.GroupBox gbAuth;
        internal System.Windows.Forms.Button bAuth;
        internal System.Windows.Forms.TextBox tAuthenKeyNum;
        internal System.Windows.Forms.TextBox tBlkNo;
        internal System.Windows.Forms.Label Label5;
        internal System.Windows.Forms.Label Label4;
        internal System.Windows.Forms.GroupBox gbKType;
        internal System.Windows.Forms.RadioButton rbKType2;
        internal System.Windows.Forms.RadioButton rbKType1;
        internal System.Windows.Forms.GroupBox gbBinOps;
        internal System.Windows.Forms.Button bBinUpd;
        internal System.Windows.Forms.Button bBinRead;
        internal System.Windows.Forms.TextBox tBinData;
        internal System.Windows.Forms.Label Label9;
        internal System.Windows.Forms.TextBox tBinLen;
        internal System.Windows.Forms.TextBox tBinBlk;
        internal System.Windows.Forms.Label Label8;
        internal System.Windows.Forms.Label Label7;
        internal System.Windows.Forms.GroupBox gbValBlk;
        internal System.Windows.Forms.Button bValRes;
        internal System.Windows.Forms.Button bValRead;
        internal System.Windows.Forms.Button bValDec;
        internal System.Windows.Forms.Button bValInc;
        internal System.Windows.Forms.Button bValStor;
        internal System.Windows.Forms.TextBox tValTar;
        internal System.Windows.Forms.TextBox tValSrc;
        internal System.Windows.Forms.TextBox tValBlk;
        internal System.Windows.Forms.TextBox tValAmt;
        internal System.Windows.Forms.Label Label13;
        internal System.Windows.Forms.Label Label12;
        internal System.Windows.Forms.Label Label11;
        internal System.Windows.Forms.Label Label10;
    }
}