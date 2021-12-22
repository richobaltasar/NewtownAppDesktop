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
using Ewats_App.Model;

namespace Ewats_App.Page
{
    public partial class StockOpname : Form
    {
        GlobalFunc f = new GlobalFunc();
        List<Model.KeranjangStockOpnameModel> Keranjang = new List<Model.KeranjangStockOpnameModel>();
        public StockOpname()
        {
            InitializeComponent();
        }

        private void StockOpname_Load(object sender, EventArgs e)
        {
            load_datagrid("");
            atur_grid2();
        }

        public void atur_grid()
        {
            dt_grid.Rows.Clear();
            dt_grid.ColumnCount = 4;
            dt_grid.Columns[0].Name = "Kode Item";
            dt_grid.Columns[1].Name = "Nama Tenant";
            dt_grid.Columns[2].Name = "Nama Item";
            dt_grid.Columns[3].Name = "Stok";

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
        public void atur_grid2()
        {
            dt_grid2.Rows.Clear();
            dt_grid2.ColumnCount = 6;
            dt_grid2.Columns[0].Name = "X";
            dt_grid2.Columns[1].Name = "Kode Item";
            dt_grid2.Columns[2].Name = "Nama Tenant";
            dt_grid2.Columns[3].Name = "Nama Item";
            dt_grid2.Columns[4].Name = "Stok Sebelumnya";
            dt_grid2.Columns[5].Name = "Stok Terbaru";

            dt_grid2.RowHeadersVisible = false;
            dt_grid2.ColumnHeadersVisible = true;
            DataGridViewColumn column1 = dt_grid2.Columns[0];
            DataGridViewColumn column2 = dt_grid2.Columns[1];
            DataGridViewColumn column3 = dt_grid2.Columns[2];
            DataGridViewColumn column4 = dt_grid2.Columns[3];
            DataGridViewColumn column5 = dt_grid2.Columns[4];
            DataGridViewColumn column6 = dt_grid2.Columns[5];

            // Initialize basic DataGridView properties.
            dt_grid2.Dock = DockStyle.None;
            dt_grid2.BackgroundColor = SystemColors.GradientInactiveCaption;
            dt_grid2.BorderStyle = BorderStyle.Fixed3D;
            // Set property values appropriate for read-only display and 
            // limited interactivity. 
            dt_grid2.AllowUserToAddRows = false;
            dt_grid2.AllowUserToDeleteRows = false;
            dt_grid2.AllowUserToOrderColumns = true;
            dt_grid2.ReadOnly = true;
            dt_grid2.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dt_grid2.MultiSelect = true;
            dt_grid2.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dt_grid2.AllowUserToResizeColumns = true;
            dt_grid2.ColumnHeadersHeightSizeMode =
                DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dt_grid2.AllowUserToResizeRows = false;
            dt_grid2.RowHeadersWidthSizeMode =
                DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            // Set the selection background color for all the cells.
            dt_grid2.DefaultCellStyle.SelectionBackColor = Color.LightBlue;
            dt_grid2.DefaultCellStyle.SelectionForeColor = Color.Black;
            // Set RowHeadersDefaultCellStyle.SelectionBackColor so that its default
            // value won't override DataGridView.DefaultCellStyle.SelectionBackColor.
            dt_grid2.RowHeadersDefaultCellStyle.SelectionBackColor = Color.Empty;
            // Set the background color for all rows and for alternating rows. 
            // The value for alternating rows overrides the value for all rows. 
            dt_grid2.RowsDefaultCellStyle.BackColor = Color.White;
            dt_grid2.AlternatingRowsDefaultCellStyle.BackColor = Color.White;
            // Set the row and column header styles.
            dt_grid2.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dt_grid2.ColumnHeadersDefaultCellStyle.BackColor = Color.Black;
            dt_grid2.RowHeadersDefaultCellStyle.BackColor = Color.Black;
            dt_grid2.DefaultCellStyle.Font = new Font("Tahoma", 12);
            dt_grid2.Columns[0].Width = 40;
        }

        public void load_datagrid(string search)
        {
            atur_grid();
            var data = f.GetDataStok(search);
            int a = 0;
            foreach (var r in data)
            {
                a++;
                string[] row = new string[] { r.idItem, r.NamaTenant, r.NamaItem, r.BykStok.ToString()};
                this.dt_grid.Rows.Add(row);
                DataGridViewRow d = dt_grid.Rows[a - 1];
                d.MinimumHeight = 40;
            }
        }

        private void dt_grid_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.ColumnIndex == 0)// created column index (delete button)
            {
                if (e.RowIndex >= 0)
                {
                    int rowIndex = e.RowIndex;
                    DataGridViewRow row = dt_grid.Rows[rowIndex];
                    var data = new StockOpnameModel();
                    data.idItem = dt_grid.Rows[rowIndex].Cells[0].Value.ToString();
                    data.NamaTenant = dt_grid.Rows[rowIndex].Cells[1].Value.ToString();
                    data.NamaItem = dt_grid.Rows[rowIndex].Cells[2].Value.ToString();
                    data.BykStok = f.ConvertDecimal(dt_grid.Rows[rowIndex].Cells[3].Value.ToString());

                    TambahStockBarang frm = new TambahStockBarang();
                    frm.Show();
                    frm.BringToFront();
                    frm.StartPosition = FormStartPosition.CenterScreen;
                    Label lblKodeBarang = frm.Controls.Find("lblKodeBarang", true).FirstOrDefault() as Label;
                    Label lblNamaProduk = frm.Controls.Find("lblNamaProduk", true).FirstOrDefault() as Label;
                    Label lblNamaTenant = frm.Controls.Find("lblNamaTenant", true).FirstOrDefault() as Label;
                    Label lblSisaStok = frm.Controls.Find("lblSisaStok", true).FirstOrDefault() as Label;

                    if (lblKodeBarang != null)
                    {
                        lblKodeBarang.Text = data.idItem;
                    }
                    if (lblNamaProduk != null)
                    {
                        lblNamaProduk.Text = data.NamaItem;
                    }
                    if (lblNamaTenant != null)
                    {
                        lblNamaTenant.Text = data.NamaTenant;
                    }
                    if (lblSisaStok != null)
                    {
                        lblSisaStok.Text = data.BykStok.ToString();
                    }
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Form fc = Application.OpenForms["TambahStockBarang"];
            if (fc != null)
            {
                fc.Close();
            }
            this.Close();
        }

        private void dt_grid2_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.ColumnIndex == 0)// created column index (delete button)
            {
                if (e.RowIndex >= 0)
                {
                    dt_grid2.Rows.Remove(dt_grid2.Rows[e.RowIndex]);
                    Keranjang.RemoveAt(e.RowIndex);
                }
            }
        }

        private void dt_grid2_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = this.dt_grid2.Rows[e.RowIndex];
                if (row.Cells.Count > 1)
                {
                    var d = new KeranjangStockOpnameModel();
                    d.idItem = row.Cells["Kode Item"].Value.ToString();
                    d.NamaTenant = row.Cells["Nama Tenant"].Value.ToString();
                    d.NamaItem = row.Cells["Nama Item"].Value.ToString();
                    d.BykStok = f.ConvertDecimal(row.Cells["Stok Sebelumnya"].Value.ToString());
                    d.BykStokUpdate = f.ConvertDecimal(row.Cells["Stok Terbaru"].Value.ToString());
                    Keranjang.Add(d);
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (Keranjang.Count > 0)
            {
                var dlg = MessageBox.Show("Apakah Anda yakin untuk mengupdate Stock Item?", "Information", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
                if (dlg == DialogResult.OK)
                {
                    foreach(var data in Keranjang)
                    {
                        var save = f.SaveUpdateStockOpname(data);
                    }
                    Form fc = Application.OpenForms["TambahStockBarang"];
                    if (fc != null)
                    {
                        fc.Close();
                    }
                    this.Close();
                }
            }
        }
    }
}
