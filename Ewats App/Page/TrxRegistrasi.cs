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
    public partial class TrxRegistrasi : Form
    {
        GlobalFunc f = new GlobalFunc();
        public TrxRegistrasi()
        {
            InitializeComponent();
        }

        private void TrxRegistrasi_Load(object sender, EventArgs e)
        {
            if (TempAllTransaksiModel.IdTrx != "")
            {
                var data = new AllTransaksiModel();
                data.IdTrx = TempAllTransaksiModel.IdTrx;
                data.CashierBy = TempAllTransaksiModel.CashierBy;
                data.Datetime = TempAllTransaksiModel.Datetime;
                data.JenisTransaksi = TempAllTransaksiModel.JenisTransaksi;
                data.Nominal = TempAllTransaksiModel.Nominal;                
                
                var res = f.GetDataTransaksiRegistrasi(data);
                if (res != null)
                {
                    IdTransaction.Text = res.idTrx;
                    Datetime.Text = res.Datetime;
                    MerchantName.Text = res.ComputerName;
                    NamaKasir.Text = res.CashierBy;
                    AccountNumber.Text = res.AccountNumber;
                    SaldoJaminan.Text =  f.ConvertToRupiah(res.SaldoJaminan);
                    IdTicket.Text = res.IdTicketTrx;
                    CashBack.Text = f.ConvertToRupiah(res.Cashback);
                    Topup.Text = f.ConvertToRupiah(res.topup);
                    Asuransi.Text = f.ConvertToRupiah(res.Asuransi);
                    TotalBeliTicket.Text = f.ConvertToRupiah(res.TotalBeliTiket);
                    TotalAll.Text = f.ConvertToRupiah(res.TotalAll);
                    JenisTransaksi.Text = res.JenisTransaksi;
                    TotalBayar.Text = f.ConvertToRupiah(res.TotalBayar);
                    PayEmoney.Text = f.ConvertToRupiah(res.PayEmoney);
                    if (res.JenisTransaksi != "EDC")
                    {
                        panelCash.Visible = true;
                        PanelDebit.Visible = false;
                        PayCash.Text = f.ConvertToRupiah(res.PayCash);
                        TerimaUang.Text = f.ConvertToRupiah(res.TerimaUang);
                        Kembalian.Text = f.ConvertToRupiah(res.Kembalian);
                    }
                    else
                    {
                        panelCash.Visible = false;
                        PanelDebit.Visible = true;
                        txtKodeBank.Text = res.BankCode;
                        TxtNamaBank.Text = res.NamaBank;
                        txtDiskon.Text = res.DiskonBank.ToString() + " %";
                        txtNominalDiskon.Text = f.ConvertToRupiah(res.NominalDiskon);
                        txtAdminCharges.Text = f.ConvertToRupiah(res.AdminCharges);
                        txtAtmNumber.Text = res.NoATM;
                        txtNoReff.Text = res.NoReffEddPrint;
                        TxtTotalDebit.Text = f.ConvertToRupiah(res.TotalDebit);
                    }
                    
                    loadGrid(res.IdTicketTrx);
                }
            }
            
        }

        public void loadGrid(string IdTicketTrx)
        {
            dt_grid.Rows.Clear();
            atur_grid();
            var data = f.GetGridTicket(IdTicketTrx);
            int a = 0;
            foreach (var r in data)
            {
                a++;
                string[] row = new string[] { a.ToString(), r.NamaTicket,
                            r.HargaSatuan, r.Qty, r.Total, r.NamaDiskon,
                            r.BesarDiskon,r.TotalDiskon, r.TotalAkhir};
                dt_grid.Rows.Add(row);
            }
        }

        public void atur_grid()
        {
            dt_grid.Rows.Clear();
            dt_grid.ColumnCount = 9;
            dt_grid.Columns[0].Name = "No";
            dt_grid.Columns[1].Name = "NamaTicket";
            dt_grid.Columns[2].Name = "Harga Satuan";
            dt_grid.Columns[3].Name = "Qty";
            dt_grid.Columns[4].Name = "Total";
            dt_grid.Columns[5].Name = "Nama Diskon";
            dt_grid.Columns[6].Name = "Besar Diskon";
            dt_grid.Columns[7].Name = "Total Diskon";
            dt_grid.Columns[8].Name = "Total Akhir";

            dt_grid.RowHeadersVisible = false;
            dt_grid.ColumnHeadersVisible = true;
            DataGridViewColumn column1 = dt_grid.Columns[0];
            DataGridViewColumn column2 = dt_grid.Columns[1];
            DataGridViewColumn column3 = dt_grid.Columns[2];
            DataGridViewColumn column4 = dt_grid.Columns[3];
            DataGridViewColumn column5 = dt_grid.Columns[4];
            DataGridViewColumn column6 = dt_grid.Columns[5];
            DataGridViewColumn column7 = dt_grid.Columns[6];
            DataGridViewColumn column8 = dt_grid.Columns[7];
            DataGridViewColumn column9 = dt_grid.Columns[8];

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
            if (TempAllTransaksiModel.IdTrx != "")
            {
                var data = new AllTransaksiModel();
                data.IdTrx = TempAllTransaksiModel.IdTrx;
                data.CashierBy = TempAllTransaksiModel.CashierBy;
                data.Datetime = TempAllTransaksiModel.Datetime;
                data.JenisTransaksi = TempAllTransaksiModel.JenisTransaksi;
                data.Nominal = TempAllTransaksiModel.Nominal;

                var res = f.GetDataTransaksiRegistrasi(data);
                var res1 = PrintRegis(res);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            
        }
        public ReturnResult PrintRegis(GetDataTransaksiRegistrasiModel data)
        {
            var res = new ReturnResult();
            try
            {
                string s = "";

                s += "Datetime \t: " + data.Datetime + Environment.NewLine;
                s += "ID Transaction\t: TRX" + data.Datetime.Replace("/", "").Replace(":", "").Replace(" ", "") + Environment.NewLine;
                s += "Merchant ID \t: " + data.ComputerName + Environment.NewLine;
                s += "Nama Petugas \t: " + data.CashierBy + Environment.NewLine;
                s += "-------------------------------------------------------" + Environment.NewLine;
                s += "Transaksi Registrasi " + Environment.NewLine;
                var dT = f.GetGridTicket(data.IdTicketTrx);
                foreach (var ticket in dT)
                {
                    s += "Nama Tiket \t: " + ticket.NamaTicket + Environment.NewLine;
                    s += "Harga Satuan \t: " + ticket.HargaSatuan + Environment.NewLine;
                    s += "Qty \t\t: " + ticket.Qty + Environment.NewLine;
                    s += "Total \t\t: " + ticket.Total + Environment.NewLine;
                    s += "Nama Diskon \t: " + ticket.NamaDiskon + Environment.NewLine;
                    s += "Diskon \t\t: " + ticket.BesarDiskon +" - " + ticket.TotalDiskon + Environment.NewLine;
                    s += "Total - Diskon \t: " + ticket.TotalAkhir + Environment.NewLine;
                }
                s += "-------------------------------------------------------" + Environment.NewLine;
                s += "Total Beli Tiket \t: " + f.ConvertToRupiah(data.TotalBeliTiket) + Environment.NewLine;

                if (data.Asuransi > 0)
                {
                    s += "Asuransi " + data.QtyTotalTiket + " Org \t: " + f.ConvertToRupiah(data.Asuransi) + Environment.NewLine;
                }
                
                if (data.SaldoJaminan > 0)
                {
                        s += "Saldo Jaminan \t: " + f.ConvertToRupiah(data.SaldoJaminan) + Environment.NewLine;
                }

                if (data.Cashback > 0)
                {
                    s += "-------------------------------------------------------" + Environment.NewLine;
                    s += "Cashback \t: - " + f.ConvertToRupiah(data.Cashback) + Environment.NewLine;
                }
                if (data.topup > 0)
                {
                    s += "-------------------------------------------------------" + Environment.NewLine;
                    s += "Topup Emoney \t: " + f.ConvertToRupiah(data.topup) + Environment.NewLine;
                }
                s += "-------------------------------------------------------" + Environment.NewLine;
                s += "Total \t\t: " + f.ConvertToRupiah(data.TotalBayar) + Environment.NewLine;

                if (data.JenisTransaksi != null)
                {
                    s += "-------------------------------------------------------" + Environment.NewLine;
                    s += "Payment \t: " + data.JenisTransaksi + Environment.NewLine;
                    if (data.PayEmoney > 0)
                    {
                        s += "Use eMoney \t: " + f.ConvertToRupiah(data.PayEmoney) + Environment.NewLine;
                    }
                    if (data.PayCash > 0)
                    {
                        s += "Total Cash \t: " + f.ConvertToRupiah(data.PayCash) + Environment.NewLine;
                    }

                    if (data.TerimaUang > 0)
                    {
                        s += "Dibayarkan \t: " + f.ConvertToRupiah(data.TerimaUang) + Environment.NewLine;
                        s += "Kembalian \t: " + f.ConvertToRupiah(data.Kembalian) + Environment.NewLine;
                    }
                    if (data.PayEmoney > 0)
                    {
                        s += "-------------------------------------------------------" + Environment.NewLine;
                        s += "Account Number \t: " + data.AccountNumber + Environment.NewLine;
                        s += "Use eMoney \t: " + f.ConvertToRupiah(data.PayEmoney) + Environment.NewLine;
                    }
                    else
                    {
                        s += "-------------------------------------------------------" + Environment.NewLine;
                        s += "Account Number \t: " +  data.AccountNumber + Environment.NewLine;
                        s += "Emoney Before \t: " + f.ConvertToRupiah(data.SaldoEmoneyBefore) + Environment.NewLine;
                        s += "Emoney Current \t: " + f.ConvertToRupiah(data.SaldoEmoneyAfter) + Environment.NewLine;
                        s += "Saldo Jaminan \t: " + f.ConvertToRupiah(data.SaldoJaminan) + Environment.NewLine;
                    }
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
                    e1.Graphics.DrawString(f.GetFooterPrint(3), new Font("Arial", 5,FontStyle.Italic), new SolidBrush(Color.Black), new RectangleF(0, startY + Offset, p.DefaultPageSettings.PrintableArea.Width, p.DefaultPageSettings.PrintableArea.Height));
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
