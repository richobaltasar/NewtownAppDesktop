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
using System.Drawing.Printing;

namespace Ewats_App.Page
{
    public partial class HistoryFoodCourt : Form
    {
        GlobalFunc f = new GlobalFunc();

        public HistoryFoodCourt()
        {
            InitializeComponent();
        }

        private void HistoryFoodCourt_Load(object sender, EventArgs e)
        {
            load_datagrid("");
        }
        public void atur_grid()
        {
            dt_grid.Rows.Clear();
            dt_grid.ColumnCount = 7;
            dt_grid.Columns[0].Name = "No.";
            dt_grid.Columns[1].Name = "Nama Tenant";
            dt_grid.Columns[2].Name = "Nama Item";
            dt_grid.Columns[3].Name = "Harga Satuan";
            dt_grid.Columns[4].Name = "Qty";
            dt_grid.Columns[5].Name = "Total Jual";
            dt_grid.Columns[6].Name = "Sisa Stok";
            

            dt_grid.RowHeadersVisible = false;
            dt_grid.ColumnHeadersVisible = true;
            DataGridViewColumn column1 = dt_grid.Columns[0];
            DataGridViewColumn column2 = dt_grid.Columns[1];
            DataGridViewColumn column3 = dt_grid.Columns[2];
            DataGridViewColumn column4 = dt_grid.Columns[3];
            DataGridViewColumn column5 = dt_grid.Columns[4];
            DataGridViewColumn column6 = dt_grid.Columns[5];
            DataGridViewColumn column7 = dt_grid.Columns[6];


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
            var data = f.GetLogFoodCourt(search,f.GetComputerName());
            int a = 0;
            foreach (var r in data)
            {
                a++;
                string[] row = new string[] { a.ToString(),r.NamaTenant, r.NamaItem,f.ConvertToRupiah(r.HargaSatuan),
                    r.Qtx.ToString(), f.ConvertToRupiah(r.HargaTotal),r.Stok.ToString() };
                this.dt_grid.Rows.Add(row);
                DataGridViewRow d = dt_grid.Rows[a - 1];
                d.MinimumHeight = 40;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            var data = f.GetLogFoodCourt(txtSearch.Text, f.GetComputerName());
            if (data != null)
            {
                var print = PrintFoodCourtHistory(data);
            }
        }

        public ReturnResult PrintFoodCourtHistory(List<KeranjangPosTotal> data)
        {
            var res = new ReturnResult();
            try
            {
                string s = "Merchant ID \t: " + f.GetComputerName() + Environment.NewLine;
                s += "Datetime \t: " + f.GetDatetime() + Environment.NewLine;
                s += "Nama Petugas \t: " + f.GetNamaUser(General.IDUser) + Environment.NewLine;
                s += "------------------------------------------------------------" + Environment.NewLine;
                s += "Penjualan Foodcourt " + Environment.NewLine;
                decimal d = 0;
                decimal TotalPenjualan = 0;
                s += d + "No. \t Nama Tenant - Nama Item - Qty - Harga Total - Stok " + Environment.NewLine;
                foreach (var Items in data)
                {
                    d++;
                    s += d + ". "+ Items.NamaTenant+" - " + Items.NamaItem + " - " + Items.Qtx + " - " + f.ConvertToRupiah(Items.HargaTotal)+" - "+ Items.Stok  + Environment.NewLine;
                    TotalPenjualan = TotalPenjualan + Items.HargaTotal;
                }

                s += "------------------------------------------------------------" + Environment.NewLine;
                s += "Total Penjualan \t\t: " + f.ConvertToRupiah(TotalPenjualan) + Environment.NewLine;
                s += "------------------------------------------------------------" + Environment.NewLine;

                foreach (string pfoot in f.GetFooterPrint())
                {
                    s += pfoot + Environment.NewLine;
                }

                PrintDocument p = new PrintDocument();
                p.PrintPage += delegate (object sender1, PrintPageEventArgs e1)
                {
                    int HeadreX = 0;
                    int startY = 0;
                    int Offset = 0;
                    e1.Graphics.DrawString(f.GetFooterPrint(1), new Font("verdana", 8, FontStyle.Bold), new SolidBrush(Color.Black), HeadreX, startY + Offset);
                    Offset = Offset + 18;
                    e1.Graphics.DrawString(f.GetFooterPrint(2), new Font("calibri", 7, FontStyle.Bold), new SolidBrush(Color.Black), HeadreX, startY + Offset);
                    Offset = Offset + 18;
                    e1.Graphics.DrawString(f.GetFooterPrint(3), new Font("calibri", 7, FontStyle.Bold), new SolidBrush(Color.Black), HeadreX, startY + Offset);
                    Offset = Offset + 18;
                    string underLine = "======================================";
                    e1.Graphics.DrawString(underLine, new Font("calibri", 7), new SolidBrush(Color.Black), 0, startY + Offset);
                    Offset = Offset + 10;
                    e1.Graphics.DrawString(s, new Font("calibri", 7), new SolidBrush(Color.Black), new RectangleF(0, startY + Offset, p.DefaultPageSettings.PrintableArea.Width, p.DefaultPageSettings.PrintableArea.Height));
                };
                p.Print();
                res.Success = true;
                res.Message = "Print Success";
            }
            catch (Exception ex)
            {
                res.Success = false;
                res.Message = "Exception Occured While Printing " + ex.Message;
            }
            return res;
        }
    }
}
