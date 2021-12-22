using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Ewats_App.Function;

namespace Ewats_App.Page
{
    public partial class MasterDiskon : Form
    {
        GlobalFunc f = new GlobalFunc();

        public MasterDiskon()
        {
            InitializeComponent();
        }

        private void MasterDiskon_Load(object sender, EventArgs e)
        {
            load_datagrid("");
        }
        public void atur_grid()
        {
            dt_grid.Rows.Clear();
            dt_grid.ColumnCount = 4;
            dt_grid.Columns[0].Name = "ID Promo";
            dt_grid.Columns[1].Name = "Diskon";
            dt_grid.Columns[2].Name = "Nama Event";
            dt_grid.Columns[3].Name = "Category Promo";

            dt_grid.RowHeadersVisible = false;
            dt_grid.ColumnHeadersVisible = true;
            DataGridViewColumn column1 = dt_grid.Columns[0];
            DataGridViewColumn column2 = dt_grid.Columns[1];
            DataGridViewColumn column3 = dt_grid.Columns[2];
            DataGridViewColumn column4 = dt_grid.Columns[3];


            // Initialize basic DataGridView properties.
            dt_grid.Dock = DockStyle.None;
            dt_grid.BackgroundColor = SystemColors.GradientInactiveCaption;
            dt_grid.BorderStyle = BorderStyle.Fixed3D;
            // Set property values appropriate for read-only display and 
            // limited interactivity. 
            dt_grid.AllowUserToAddRows = false;
            dt_grid.AllowUserToDeleteRows = false;
            dt_grid.AllowUserToOrderColumns = true;
            dt_grid.ReadOnly = true;
            dt_grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dt_grid.MultiSelect = true;
            dt_grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dt_grid.AllowUserToResizeColumns = true;
            dt_grid.ColumnHeadersHeightSizeMode =
                DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dt_grid.AllowUserToResizeRows = false;
            dt_grid.RowHeadersWidthSizeMode =
                DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            // Set the selection background color for all the cells.
            dt_grid.DefaultCellStyle.SelectionBackColor = Color.LightBlue;
            dt_grid.DefaultCellStyle.SelectionForeColor = Color.Black;
            // Set RowHeadersDefaultCellStyle.SelectionBackColor so that its default
            // value won't override DataGridView.DefaultCellStyle.SelectionBackColor.
            dt_grid.RowHeadersDefaultCellStyle.SelectionBackColor = Color.Empty;
            // Set the background color for all rows and for alternating rows. 
            // The value for alternating rows overrides the value for all rows. 
            dt_grid.RowsDefaultCellStyle.BackColor = Color.White;
            dt_grid.AlternatingRowsDefaultCellStyle.BackColor = Color.White;
            // Set the row and column header styles.
            dt_grid.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dt_grid.ColumnHeadersDefaultCellStyle.BackColor = Color.Black;
            dt_grid.RowHeadersDefaultCellStyle.BackColor = Color.Black;
            dt_grid.DefaultCellStyle.Font = new Font("Tahoma", 12);
            dt_grid.Columns[0].Width = 40;

        }

        public void load_datagrid(string search)
        {
            atur_grid();
            var data = f.GetPromo(search);
            int a = 0;
            foreach (var r in data)
            {
                a++;
                string[] row = new string[] {r.idPromo , (r.Diskon + " %"), r.NamaPromo, r.CatPromo };
                this.dt_grid.Rows.Add(row);
                DataGridViewRow d = dt_grid.Rows[a-1];
                d.MinimumHeight = 40;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            load_datagrid(txtSearch.Text);
        }

        private void dt_grid_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = this.dt_grid.Rows[e.RowIndex];
                Form frm = Application.OpenForms["MasterTicket"];
                if (frm != null)
                {
                    Label lblDiskonId = frm.Controls.Find("lblDiskonId", true).FirstOrDefault() as Label;
                    if (lblDiskonId != null)
                    {
                        lblDiskonId.Text = row.Cells["ID Promo"].Value.ToString();
                    }
                    Label lblNamaDiskon = frm.Controls.Find("lblNamaDiskon", true).FirstOrDefault() as Label;
                    if (lblNamaDiskon != null)
                    {
                        lblNamaDiskon.Text =  row.Cells["Nama Event"].Value.ToString();
                    }
                    
                    Label lblDiskon = frm.Controls.Find("lblDiskon", true).FirstOrDefault() as Label;
                    if (lblDiskon != null)
                    {
                        lblDiskon.Text = row.Cells["Diskon"].Value.ToString();
                    }

                    this.Close();
                }
            }
        }

        private void dt_grid_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
