using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Ewats_App.Function;
using Ewats_App.Model;

namespace Ewats_App.Page
{
    public partial class Dashboard : UserControl
    {
        GlobalFunc f = new GlobalFunc();

        public Dashboard()
        {
            InitializeComponent();
        }

        public void atur_grid()
        {
            dt_grid.Rows.Clear();
            dt_grid.ColumnCount = 2;
            dt_grid.Columns[0].Name = "Nama Ticket";
            dt_grid.Columns[1].Name = "Total";

            // Initialize basic DataGridView properties.
            dt_grid.Dock = DockStyle.Fill;
            dt_grid.BackgroundColor = SystemColors.GradientInactiveCaption;
            dt_grid.BorderStyle = BorderStyle.Fixed3D;
            // Set property values appropriate for read-only display and 
            // limited interactivity. 
            dt_grid.AllowUserToAddRows = false;
            dt_grid.AllowUserToDeleteRows = false;
            dt_grid.AllowUserToOrderColumns = true;
            dt_grid.ReadOnly = true;
            dt_grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dt_grid.MultiSelect = false;
            dt_grid.AllowUserToResizeColumns = false;
            dt_grid.AllowUserToResizeRows = false;
            dt_grid.DefaultCellStyle.SelectionBackColor = Color.LightBlue;
            dt_grid.DefaultCellStyle.SelectionForeColor = Color.Black;
            dt_grid.RowHeadersDefaultCellStyle.SelectionBackColor = Color.Empty;
            dt_grid.RowsDefaultCellStyle.BackColor = Color.White;
            dt_grid.AlternatingRowsDefaultCellStyle.BackColor = Color.White;
            dt_grid.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dt_grid.ColumnHeadersDefaultCellStyle.BackColor = Color.Black;
            dt_grid.RowHeadersDefaultCellStyle.BackColor = Color.Black;

            dt_grid.ColumnHeadersDefaultCellStyle.Font = new Font("Calibri", 12, FontStyle.Bold);
            dt_grid.DefaultCellStyle.Font = new Font("Calibri", 14);

            for (int i = 0; i < dt_grid.Columns.Count - 1; i++)
            {
                dt_grid.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            }

            dt_grid.Columns[dt_grid.Columns.Count - 1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            dt_grid.Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dt_grid.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            
        }

        private void Dashboard_Load(object sender, EventArgs e)
        {
            var data = f.CheckOpenCashier();
            if (data.Success == true)
            {
                btnClosing.Enabled = true;
            }
            else
            {
                btnClosing.Enabled = false;
            }

            atur_grid();
            atur_grid2();

        }

        private void button1_Click(object sender, EventArgs e)
        {
            TambahDanaModal f = new TambahDanaModal();
            f.Show();
            f.BringToFront();
            f.StartPosition = FormStartPosition.CenterScreen;
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        public void atur_grid2()
        {
            dt_grid2.Rows.Clear();
            dt_grid2.ColumnCount = 6;
            dt_grid2.Columns[0].Name = "X";
            dt_grid2.Columns[1].Name = "Id Trx";
            dt_grid2.Columns[2].Name = "Datetime";
            dt_grid2.Columns[3].Name = "Nama Transaksi";
            dt_grid2.Columns[4].Name = "Total Belanja";
            dt_grid2.Columns[5].Name = "Cashier by";

            dt_grid2.Columns[0].HeaderText = "X";
            dt_grid2.Columns[1].HeaderText = "ID TRX";
            dt_grid2.Columns[2].HeaderText = "DATETIME";
            dt_grid2.Columns[3].HeaderText = "NAMA TRANSAKSI";
            dt_grid2.Columns[4].HeaderText = "TOTAL";
            dt_grid2.Columns[5].HeaderText = "KASIR";

            dt_grid2.RowHeadersVisible = false;
            dt_grid2.ColumnHeadersVisible = true;

            // Initialize basic DataGridView properties.
            dt_grid2.Dock = DockStyle.Fill;
            dt_grid2.BackgroundColor = SystemColors.GradientInactiveCaption;
            dt_grid2.BorderStyle = BorderStyle.Fixed3D;
            // Set property values appropriate for read-only display and 
            // limited interactivity. 
            dt_grid2.AllowUserToAddRows = false;
            dt_grid2.AllowUserToDeleteRows = false;
            dt_grid2.AllowUserToOrderColumns = true;
            dt_grid2.ReadOnly = true;
            dt_grid2.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dt_grid2.MultiSelect = false;
            dt_grid2.AllowUserToResizeColumns = false;
            dt_grid2.AllowUserToResizeRows = false;
            dt_grid2.DefaultCellStyle.SelectionBackColor = Color.LightBlue;
            dt_grid2.DefaultCellStyle.SelectionForeColor = Color.Black;
            dt_grid2.RowHeadersDefaultCellStyle.SelectionBackColor = Color.Empty;
            dt_grid2.RowsDefaultCellStyle.BackColor = Color.White;
            dt_grid2.AlternatingRowsDefaultCellStyle.BackColor = Color.White;
            dt_grid2.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dt_grid2.ColumnHeadersDefaultCellStyle.BackColor = Color.Black;
            dt_grid2.RowHeadersDefaultCellStyle.BackColor = Color.Black;

            dt_grid2.ColumnHeadersDefaultCellStyle.Font = new Font("Calibri", 12, FontStyle.Bold);
            dt_grid2.DefaultCellStyle.Font = new Font("Calibri", 16);

            for (int i = 0; i < dt_grid2.Columns.Count - 1; i++)
            {
                dt_grid2.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            }
            dt_grid2.Columns[dt_grid2.Columns.Count - 1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            dt_grid2.Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dt_grid2.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dt_grid2.Columns[2].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dt_grid2.Columns[3].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dt_grid2.Columns[4].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dt_grid2.Columns[5].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
        }

        private void btnClosing_Click(object sender, EventArgs e)
        {
            Form fc = Application.OpenForms["ClosingCashier"];
            if (fc != null)
            {
                fc.Show();
                fc.BringToFront();
            }
            else
            {
                ClosingCashier frm = new ClosingCashier();
                frm.StartPosition = FormStartPosition.CenterScreen;
                frm.BringToFront();
                frm.MaximizeBox = false;
                frm.MinimizeBox = false;
                frm.Show();
            }
        }

        private void dt_grid2_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.ColumnIndex == 0)// created column index (delete button)
            {
                if (e.RowIndex >= 0)
                {
                    int rowIndex = e.RowIndex;
                    DataGridViewRow row = dt_grid2.Rows[rowIndex];
                    var data = new AllTransaksiModel();
                    
                    data.IdTrx = dt_grid2.Rows[rowIndex].Cells[1].Value == null ? "" : dt_grid2.Rows[rowIndex].Cells[1].Value.ToString();
                    data.Datetime = dt_grid2.Rows[rowIndex].Cells[2].Value ==null? "" : dt_grid2.Rows[rowIndex].Cells[2].Value.ToString();
                    data.JenisTransaksi = dt_grid2.Rows[rowIndex].Cells[3].Value == null? "" : dt_grid2.Rows[rowIndex].Cells[3].Value.ToString();
                    data.Nominal = f.ConvertDecimal(dt_grid2.Rows[rowIndex].Cells[4].Value == null?"": dt_grid2.Rows[rowIndex].Cells[4].Value.ToString());
                    data.CashierBy = dt_grid2.Rows[rowIndex].Cells[5].Value ==null ? "":dt_grid2.Rows[rowIndex].Cells[5].Value.ToString();
                     
                    if (data.IdTrx.Contains("REG") == true)
                    {
                        data.IdTrx = data.IdTrx.Replace("REG", "").Trim();
                        if (data.IdTrx != null && data.IdTrx != "")
                        {
                            TempAllTransaksiModel.IdTrx = data.IdTrx;
                            TempAllTransaksiModel.Datetime = data.Datetime;
                            TempAllTransaksiModel.CashierBy = data.CashierBy;
                            TempAllTransaksiModel.JenisTransaksi = data.JenisTransaksi;
                            TempAllTransaksiModel.Nominal = data.Nominal;
                            
                            Form fc = Application.OpenForms["TrxRegistrasi"];
                            if (fc != null)
                            {
                                fc.Show();
                                fc.BringToFront();
                            }
                            else
                            {
                                Page.TrxRegistrasi frm = new Page.TrxRegistrasi();
                                frm.StartPosition = FormStartPosition.CenterScreen;
                                frm.BringToFront();
                                frm.MaximizeBox = false;
                                frm.MinimizeBox = false;
                                frm.Show();
                            }

                        }
                        
                    }
                    else if (data.IdTrx.Contains("TOPUP") == true)
                    {
                        data.IdTrx = data.IdTrx.Replace("TOPUP", "").Trim();
                        if (data.IdTrx != null && data.IdTrx != "")
                        {
                            TempAllTransaksiModel.IdTrx = data.IdTrx;
                            TempAllTransaksiModel.Datetime = data.Datetime;
                            TempAllTransaksiModel.CashierBy = data.CashierBy;
                            TempAllTransaksiModel.JenisTransaksi = data.JenisTransaksi;
                            TempAllTransaksiModel.Nominal = data.Nominal;

                            Form fc = Application.OpenForms["TrxTopup"];
                            if (fc != null)
                            {
                                fc.Show();
                                fc.BringToFront();
                            }
                            else
                            {
                                Page.TrxTopup frm = new Page.TrxTopup();
                                frm.StartPosition = FormStartPosition.CenterScreen;
                                frm.BringToFront();
                                frm.MaximizeBox = false;
                                frm.MinimizeBox = false;
                                frm.Show();
                            }

                        }
                    }
                    else if (data.IdTrx.Contains("REFUND") == true)
                    {
                        data.IdTrx = data.IdTrx.Replace("REFUND", "").Trim();
                        if (data.IdTrx != null && data.IdTrx != "")
                        {
                            TempAllTransaksiModel.IdTrx = data.IdTrx;
                            TempAllTransaksiModel.Datetime = data.Datetime;
                            TempAllTransaksiModel.CashierBy = data.CashierBy;
                            TempAllTransaksiModel.JenisTransaksi = data.JenisTransaksi;
                            TempAllTransaksiModel.Nominal = data.Nominal;

                            Form fc = Application.OpenForms["TrxRefund"];
                            if (fc != null)
                            {
                                fc.Show();
                                fc.BringToFront();
                            }
                            else
                            {
                                Page.TrxRefund frm = new Page.TrxRefund();
                                frm.StartPosition = FormStartPosition.CenterScreen;
                                frm.BringToFront();
                                frm.MaximizeBox = false;
                                frm.MinimizeBox = false;
                                frm.Show();
                            }

                        }
                    }
                    else if (data.IdTrx.Contains("FOODCOURT") == true)
                    {
                        data.IdTrx = data.IdTrx.Replace("FOODCOURT", "").Trim();
                        if (data.IdTrx != null && data.IdTrx != "")
                        {
                            TempAllTransaksiModel.IdTrx = data.IdTrx;
                            TempAllTransaksiModel.Datetime = data.Datetime;
                            TempAllTransaksiModel.CashierBy = data.CashierBy;
                            TempAllTransaksiModel.JenisTransaksi = data.JenisTransaksi;
                            TempAllTransaksiModel.Nominal = data.Nominal;

                            Form fc = Application.OpenForms["TrxFoodCourt"];
                            if (fc != null)
                            {
                                fc.Show();
                                fc.BringToFront();
                            }
                            else
                            {
                                TrxFoodCourt frm = new TrxFoodCourt();
                                frm.StartPosition = FormStartPosition.CenterScreen;
                                frm.BringToFront();
                                frm.MaximizeBox = false;
                                frm.MinimizeBox = false;
                                frm.Show();
                            }

                        }
                    }
                }
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Form fc = Application.OpenForms["StockOpname"];
            if (fc != null)
            {
                fc.Show();
                fc.BringToFront();
            }
            else
            {
                StockOpname frm = new StockOpname();
                frm.StartPosition = FormStartPosition.CenterScreen;
                frm.BringToFront();
                frm.MaximizeBox = false;
                frm.MinimizeBox = false;
                frm.Show();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Form fc = Application.OpenForms["HistoryFoodCourt"];
            if (fc != null)
            {
                fc.Show();
                fc.BringToFront();
            }
            else
            {
                HistoryFoodCourt frm = new HistoryFoodCourt();
                frm.StartPosition = FormStartPosition.CenterScreen;
                frm.BringToFront();
                frm.MaximizeBox = false;
                frm.MinimizeBox = false;
                frm.Show();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void txtAllTiket_TextChanged(object sender, EventArgs e)
        {

        }

        private void tableLayoutPanel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void dt_grid2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void tableLayoutPanel3_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
