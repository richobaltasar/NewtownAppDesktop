using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Ewats_App.Model;
using Ewats_App.Function;
using System.Drawing.Printing;

namespace Ewats_App.Page
{
    public partial class TrxFoodCourt : Form
    {
        GlobalFunc f = new GlobalFunc();
        public TrxFoodCourt()
        {
            InitializeComponent();
        }

        private void TrxFoodCourt_Load(object sender, EventArgs e)
        {
            if (TempAllTransaksiModel.IdTrx != "")
            {
                var data = new AllTransaksiModel();
                data.IdTrx = TempAllTransaksiModel.IdTrx;
                data.CashierBy = TempAllTransaksiModel.CashierBy;
                data.Datetime = TempAllTransaksiModel.Datetime;
                data.JenisTransaksi = TempAllTransaksiModel.JenisTransaksi;
                data.Nominal = TempAllTransaksiModel.Nominal;

                var res = f.GetDataTransaksiFoodCourtReprint(data);
                if (res != null)
                {
                    IdTransaction.Text = res.IdTransaction;
                    Datetime.Text = res.Datetime;
                    MerchantName.Text = res.MerchantName;
                    NamaKasir.Text = res.NamaKasir;
                    TotalBelanja.Text = res.TotalBelanja;
                    PaymentMethod.Text = res.PaymentMethod;
                    UseEmoney.Text = res.UseEmoney;
                    AccountNumber.Text = res.AccountNumber;
                    SaldoSebelum.Text = res.SaldoSebelum;
                    SaldoSetelah.Text = res.SaldoSetelah;
                    loadGrid(res.IdItemKeranjang);
                }
            }
        }

        public void loadGrid(string ItemKeranjang)
        {
            dt_grid.Rows.Clear();
            atur_grid();
            var data = f.GetGridKeranjangFoodCourtReprint(ItemKeranjang);
            int a = 0;
            foreach (var r in data)
            {
                a++;
                string[] row = new string[] { a.ToString(), r.NamaItem,
                            r.HargaSatuan, r.Qty, r.Total};
                dt_grid.Rows.Add(row);
            }
        }

        public void atur_grid()
        {
            dt_grid.Rows.Clear();
            dt_grid.ColumnCount = 5;
            dt_grid.Columns[0].Name = "No";
            dt_grid.Columns[1].Name = "Nama Item";
            dt_grid.Columns[2].Name = "Harga Satuan";
            dt_grid.Columns[3].Name = "Qty";
            dt_grid.Columns[4].Name = "Total";

            dt_grid.RowHeadersVisible = false;
            dt_grid.ColumnHeadersVisible = true;
            DataGridViewColumn column1 = dt_grid.Columns[0];
            DataGridViewColumn column2 = dt_grid.Columns[1];
            DataGridViewColumn column3 = dt_grid.Columns[2];
            DataGridViewColumn column4 = dt_grid.Columns[3];
            DataGridViewColumn column5 = dt_grid.Columns[4];
            
            // Initialize basic DataGridView properties.
            dt_grid.Dock = DockStyle.None;
            //dt_grid.BackgroundColor = SystemColors.Highlight;
            dt_grid.BorderStyle = BorderStyle.None;
            dt_grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dt_grid.ScrollBars = ScrollBars.Both;
            //// Set property values appropriate for read-only display and 
            //// limited interactivity. 
            //dt_grid.AllowUserToAddRows = false;
            //dt_grid.AllowUserToDeleteRows = false;
            //dt_grid.AllowUserToOrderColumns = true;
            //dt_grid.ReadOnly = true;
            //dt_grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            //dt_grid.MultiSelect = true;

            //dt_grid.AllowUserToResizeColumns = true;
            //dt_grid.ColumnHeadersHeightSizeMode =
            //    DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            //dt_grid.AllowUserToResizeRows = false;
            //dt_grid.RowHeadersWidthSizeMode =
            //    DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            //// Set the selection background color for all the cells.
            ////dt_grid.DefaultCellStyle.SelectionBackColor = Color.LightBlue;
            ////dt_grid.DefaultCellStyle.SelectionForeColor = Color.Black;
            //// Set RowHeadersDefaultCellStyle.SelectionBackColor so that its default
            //// value won't override DataGridView.DefaultCellStyle.SelectionBackColor.
            //dt_grid.RowHeadersDefaultCellStyle.SelectionBackColor = Color.Empty;
            //// Set the background color for all rows and for alternating rows. 
            //// The value for alternating rows overrides the value for all rows. 

            //dt_grid.RowsDefaultCellStyle.BackColor = SystemColors.Highlight;
            //dt_grid.AlternatingRowsDefaultCellStyle.BackColor = Color.White;
            //// Set the row and column header styles.
            ////dt_grid.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            //dt_grid.ColumnHeadersDefaultCellStyle.BackColor = SystemColors.Highlight;
            //dt_grid.RowHeadersDefaultCellStyle.BackColor = SystemColors.Highlight;
            //dt_grid.RowsDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dt_grid.Columns[4].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dt_grid.DefaultCellStyle.Font = new Font("Tahoma", 9);
            dt_grid.DefaultCellStyle.ForeColor = Color.Black;
            dt_grid.Columns[0].Width = 40;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button19_Click(object sender, EventArgs e)
        {
            var data = new AllTransaksiModel();
            data.IdTrx = TempAllTransaksiModel.IdTrx;
            data.CashierBy = TempAllTransaksiModel.CashierBy;
            data.Datetime = TempAllTransaksiModel.Datetime;
            data.JenisTransaksi = TempAllTransaksiModel.JenisTransaksi;
            data.Nominal = TempAllTransaksiModel.Nominal;

            var res = f.GetDataTransaksiFoodCourtReprint(data);
            if (res != null)
            {
                var print = PrintFoodCourt(res);
            }
        }

        public ReturnResult PrintFoodCourt(GetDataTransaksiFoodCourtReprintModel data)
        {
            var res = new ReturnResult();
            try
            {
                string s = "Datetime \t: " + data.Datetime + Environment.NewLine;
                s += "ID Transaction\t: " + data.IdTransaction + Environment.NewLine;
                s += "Merchant ID \t: " + data.MerchantName + Environment.NewLine;
                s += "Nama Petugas \t: " + data.NamaKasir + Environment.NewLine;
                s += "-------------------------------------------------------" + Environment.NewLine;
                s += "Transaksi Foodcourt " + Environment.NewLine;
                decimal d = 0;
                var keranjang = f.GetGridKeranjangFoodCourtReprint(data.IdItemKeranjang);
                foreach (var Items in keranjang)
                {
                    d++;
                    s += d + ". " + Items.NamaItem + " - " + Items.Qty + "\t : " + (Items.Total) + Environment.NewLine;
                }
                s += "-------------------------------------------------------" + Environment.NewLine;
                s += "Total \t\t: " + data.TotalBelanja + Environment.NewLine;

                s += "-------------------------------------------------------" + Environment.NewLine;
                s += "Payment \t: " + data.PaymentMethod + Environment.NewLine;
                if (f.ConvertDecimal(data.UseEmoney) > 0)
                {
                    s += "Use eMoney \t: " + data.UseEmoney + Environment.NewLine;
                    s += "-------------------------------------------------------" + Environment.NewLine;
                    s += "Account Number \t: " + data.AccountNumber + Environment.NewLine;
                    s += "Previous Balance \t: " + f.ConvertToRupiah(f.ConvertDecimal(data.SaldoSebelum)) + Environment.NewLine;
                    s += "Current Balance \t: " + f.ConvertToRupiah(f.ConvertDecimal(data.SaldoSetelah)) + Environment.NewLine;
                }
                else
                {
                    //s += "Uang dibayarkan \t: " + f.ConvertToRupiah(data..Pay.TerimaUang) + Environment.NewLine;
                    //s += "Uang kembalian \t: " + f.ConvertToRupiah(Data.Pay.Kembalian) + Environment.NewLine;
                }
                s += "-------------------------------------------------------" + Environment.NewLine;
                foreach (string pfoot in f.GetFooterPrint())
                {
                    s += pfoot + Environment.NewLine;
                }
                s += "-------------------------------------------------------" + Environment.NewLine;
                s += "*****   RECEIPT COPY   *****" + Environment.NewLine;

                PrintDocument p = new PrintDocument();
                p.PrintPage += delegate (object sender1, PrintPageEventArgs e1)
                {
                    int HeadreX = 0;
                    int startY = 0;
                    int Offset = 0;
                    e1.Graphics.DrawString("*****   RECEIPT COPY   *****", new Font("Arial", 8, FontStyle.Bold), new SolidBrush(Color.Black), HeadreX, startY + Offset);
                    Offset = Offset + 15;
                    string underLine1 = "======================================";
                    e1.Graphics.DrawString(underLine1, new Font("calibri", 7), new SolidBrush(Color.Black), 0, startY + Offset);
                    Offset = Offset + 10;
                    e1.Graphics.DrawString(f.GetFooterPrint(1), new Font("Arial", 8, FontStyle.Bold), new SolidBrush(Color.Black), HeadreX, startY + Offset);
                    Offset = Offset + 15;
                    //e1.Graphics.DrawString(f.GetFooterPrint(2), new Font("Arial", 10, FontStyle.Bold), new SolidBrush(Color.Black), HeadreX, startY + Offset);
                    e1.Graphics.DrawString(f.GetFooterPrint(2), new Font("Arial", 7, FontStyle.Bold), new SolidBrush(Color.Black), new RectangleF(0, startY + Offset, p.DefaultPageSettings.PrintableArea.Width, p.DefaultPageSettings.PrintableArea.Height));
                    Offset = Offset + 15;
                    e1.Graphics.DrawString(f.GetFooterPrint(3), new Font("Arial", 5, FontStyle.Italic), new SolidBrush(Color.Black), new RectangleF(0, startY + Offset, p.DefaultPageSettings.PrintableArea.Width, p.DefaultPageSettings.PrintableArea.Height));
                    Offset = Offset + 15;
                    string underLine = "======================================";
                    e1.Graphics.DrawString(underLine, new Font("Arial", 6), new SolidBrush(Color.Black), 0, startY + Offset);
                    Offset = Offset + 10;
                    e1.Graphics.DrawString(s, new Font("Arial", 7), new SolidBrush(Color.Black), new RectangleF(0, startY + Offset, p.DefaultPageSettings.PrintableArea.Width, p.DefaultPageSettings.PrintableArea.Height));

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
