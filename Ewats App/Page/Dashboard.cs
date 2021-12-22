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

            dt_grid.RowHeadersVisible = false;
            dt_grid.ColumnHeadersVisible = true;
            DataGridViewColumn column1 = dt_grid.Columns[0];
            DataGridViewColumn column2 = dt_grid.Columns[1];

            // Initialize basic DataGridView properties.
            dt_grid.Dock = DockStyle.None;
            dt_grid.BorderStyle = BorderStyle.None;
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
            dt_grid.RowHeadersDefaultCellStyle.SelectionBackColor = Color.Empty;
            // Set the background color for all rows and for alternating rows. 
            // The value for alternating rows overrides the value for all rows. 
            dt_grid.RowsDefaultCellStyle.BackColor = Color.White;
            dt_grid.AlternatingRowsDefaultCellStyle.BackColor = Color.White;
            // Set the row and column header styles.
            dt_grid.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dt_grid.ColumnHeadersDefaultCellStyle.BackColor = Color.Black;
            dt_grid.RowHeadersDefaultCellStyle.BackColor = Color.Black;
            dt_grid.RowsDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dt_grid.DefaultCellStyle.Font = new Font("Tahoma", 8);
            dt_grid.DefaultCellStyle.ForeColor = Color.White;
            dt_grid.Columns[0].Width = 10;
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
            
            dt_grid2.BorderStyle = BorderStyle.None;
            dt_grid2.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dt_grid2.ScrollBars = ScrollBars.Both;
            
            dt_grid2.RowsDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dt_grid2.Columns[4].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dt_grid2.DefaultCellStyle.Font = new Font("Tahoma", 9);
            dt_grid2.DefaultCellStyle.ForeColor = Color.Black;
            dt_grid2.Columns[0].Width = 40;
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

    }
}
