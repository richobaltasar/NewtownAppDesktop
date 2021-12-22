namespace Ewats_App.Page
{
    partial class AccountLog
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.label2 = new System.Windows.Forms.Label();
            this.panel12 = new System.Windows.Forms.Panel();
            this.TxtBacaKartu = new System.Windows.Forms.RichTextBox();
            this.button19 = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.dt_grid = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.panel12.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dt_grid)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox2
            // 
            this.pictureBox2.Image = global::Ewats_App.Properties.Resources.Login1;
            this.pictureBox2.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.pictureBox2.Location = new System.Drawing.Point(11, 7);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(29, 32);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox2.TabIndex = 53;
            this.pictureBox2.TabStop = false;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Font = new System.Drawing.Font("Calibri", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.label2.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label2.Location = new System.Drawing.Point(44, 4);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(202, 36);
            this.label2.TabIndex = 52;
            this.label2.Text = "History Account";
            this.label2.Click += new System.EventHandler(this.label2_Click);
            // 
            // panel12
            // 
            this.panel12.BackColor = System.Drawing.Color.Gray;
            this.panel12.Controls.Add(this.TxtBacaKartu);
            this.panel12.Controls.Add(this.button19);
            this.panel12.Controls.Add(this.label5);
            this.panel12.Location = new System.Drawing.Point(11, 61);
            this.panel12.Name = "panel12";
            this.panel12.Size = new System.Drawing.Size(236, 250);
            this.panel12.TabIndex = 54;
            // 
            // TxtBacaKartu
            // 
            this.TxtBacaKartu.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.TxtBacaKartu.Enabled = false;
            this.TxtBacaKartu.Location = new System.Drawing.Point(5, 50);
            this.TxtBacaKartu.Name = "TxtBacaKartu";
            this.TxtBacaKartu.Size = new System.Drawing.Size(226, 133);
            this.TxtBacaKartu.TabIndex = 13;
            this.TxtBacaKartu.Text = "";
            // 
            // button19
            // 
            this.button19.BackColor = System.Drawing.Color.DarkOrange;
            this.button19.FlatAppearance.BorderSize = 0;
            this.button19.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button19.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button19.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.button19.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.button19.Location = new System.Drawing.Point(5, 189);
            this.button19.Name = "button19";
            this.button19.Size = new System.Drawing.Size(226, 51);
            this.button19.TabIndex = 18;
            this.button19.Text = "Baca Kartu";
            this.button19.UseVisualStyleBackColor = false;
            this.button19.Click += new System.EventHandler(this.button19_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.label5.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label5.Location = new System.Drawing.Point(5, 21);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(117, 19);
            this.label5.TabIndex = 11;
            this.label5.Text = "Tempelkan Kartu";
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.Highlight;
            this.panel1.Location = new System.Drawing.Point(-2, 48);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1200, 2);
            this.panel1.TabIndex = 55;
            // 
            // dt_grid
            // 
            this.dt_grid.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dt_grid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dt_grid.Location = new System.Drawing.Point(253, 61);
            this.dt_grid.Name = "dt_grid";
            this.dt_grid.Size = new System.Drawing.Size(698, 584);
            this.dt_grid.TabIndex = 56;
            // 
            // AccountLog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.DimGray;
            this.Controls.Add(this.dt_grid);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panel12);
            this.Controls.Add(this.pictureBox2);
            this.Controls.Add(this.label2);
            this.Name = "AccountLog";
            this.Size = new System.Drawing.Size(1102, 671);
            this.Load += new System.EventHandler(this.AccountLog_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.panel12.ResumeLayout(false);
            this.panel12.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dt_grid)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Panel panel12;
        public System.Windows.Forms.RichTextBox TxtBacaKartu;
        private System.Windows.Forms.Button button19;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Panel panel1;
        public System.Windows.Forms.DataGridView dt_grid;
    }
}
