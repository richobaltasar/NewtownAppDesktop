namespace Ewats_App.Page
{
    partial class StockOpname
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
            this.label1 = new System.Windows.Forms.Label();
            this.dt_grid = new System.Windows.Forms.DataGridView();
            this.dt_grid2 = new System.Windows.Forms.DataGridView();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.label3 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dt_grid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dt_grid2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(0, 13);
            this.label1.TabIndex = 6;
            // 
            // dt_grid
            // 
            this.dt_grid.BackgroundColor = System.Drawing.Color.Gray;
            this.dt_grid.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dt_grid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dt_grid.Location = new System.Drawing.Point(12, 66);
            this.dt_grid.Name = "dt_grid";
            this.dt_grid.Size = new System.Drawing.Size(610, 499);
            this.dt_grid.TabIndex = 5;
            this.dt_grid.CellMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dt_grid_CellMouseClick);
            // 
            // dt_grid2
            // 
            this.dt_grid2.BackgroundColor = System.Drawing.Color.Gray;
            this.dt_grid2.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dt_grid2.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dt_grid2.Location = new System.Drawing.Point(628, 66);
            this.dt_grid2.Name = "dt_grid2";
            this.dt_grid2.Size = new System.Drawing.Size(677, 499);
            this.dt_grid2.TabIndex = 8;
            this.dt_grid2.CellMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dt_grid2_CellMouseClick);
            this.dt_grid2.RowsAdded += new System.Windows.Forms.DataGridViewRowsAddedEventHandler(this.dt_grid2_RowsAdded);
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.LightSeaGreen;
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F);
            this.button1.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.button1.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.button1.Location = new System.Drawing.Point(411, 573);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(211, 57);
            this.button1.TabIndex = 38;
            this.button1.Text = "Update Stok";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.BackColor = System.Drawing.Color.Crimson;
            this.button2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button2.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F);
            this.button2.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.button2.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.button2.Location = new System.Drawing.Point(628, 573);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(211, 57);
            this.button2.TabIndex = 39;
            this.button2.Text = "Cancel";
            this.button2.UseVisualStyleBackColor = false;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // pictureBox2
            // 
            this.pictureBox2.Image = global::Ewats_App.Properties.Resources.Login1;
            this.pictureBox2.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.pictureBox2.Location = new System.Drawing.Point(12, 19);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(29, 32);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox2.TabIndex = 58;
            this.pictureBox2.TabStop = false;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Font = new System.Drawing.Font("Calibri", 21.75F);
            this.label3.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.label3.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label3.Location = new System.Drawing.Point(47, 17);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(206, 36);
            this.label3.TabIndex = 57;
            this.label3.Text = "STOCK OPNAME";
            // 
            // StockOpname
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.DimGray;
            this.ClientSize = new System.Drawing.Size(1317, 639);
            this.Controls.Add(this.pictureBox2);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.dt_grid2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.dt_grid);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "StockOpname";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "StockOpname";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.StockOpname_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dt_grid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dt_grid2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridView dt_grid;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        public System.Windows.Forms.DataGridView dt_grid2;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.Label label3;
    }
}