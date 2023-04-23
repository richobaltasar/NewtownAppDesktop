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
using System.IO;
using System.Runtime.InteropServices;
using System.Drawing.Drawing2D;
using System.Drawing.Printing;
using System.Management;
using Ewats_App.Function;

namespace Ewats_App.Page
{
    public partial class TunaiPayment : Form
    {
        Function.GlobalFunc f = new Function.GlobalFunc();

        public int retCode, hContext, hCard, Protocol;
        public bool connActive = false;
        public bool autoDet;

        public byte[] SendBuff = new byte[263];
        public byte[] RecvBuff = new byte[263];

        public int SendLen, RecvLen, nBytesRet, reqType, Aprotocol, dwProtocol, cbPciLength;

        string readername = "ACS ACR122 0";
        public card_function.Card.SCARD_READERSTATE RdrState;
        public card_function.Card.SCARD_IO_REQUEST pioSendRequest;

        public TunaiPayment()
        {
            InitializeComponent();
        }

        private void button13_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button13_Click_1(object sender, EventArgs e)
        {
            this.Close();
        }

        private void TunaiPayment_Load(object sender, EventArgs e)
        {
            if (CashPayment.JenisTransaksi != null)
            {
                if (CashPayment.JenisTransaksi == "TOPUP")
                {
                    txtTotalBelanja.Text = f.ConvertToRupiah(TopupCashPayment.NominalTopup);
                    if (Model.ConfigurationFileStatic.VFDPort != "")
                    {
                        Function.VFDPort.send("Total Pembayaran :", f.ConvertToRupiah(f.ConvertDecimal(txtTotalBelanja.Text)), Function.VFDPort.sp.PortName);
                    }
                    
                }
                else if (CashPayment.JenisTransaksi == "FoodCourt")
                {
                    txtTotalBelanja.Text = "" + f.ConvertToRupiah(FoodCourtPayment.Pay.TotalBayar);
                    if (ConfigurationFileStatic.VFDPort != "")
                    {
                        Function.VFDPort.send("Total Pembayaran :", f.ConvertToRupiah(FoodCourtPayment.Pay.TotalBayar), Function.VFDPort.sp.PortName);
                    }
                }
                else if(CashPayment.JenisTransaksi == "Registrasi")
                {
                    txtTotalBelanja.Text = f.ConvertToRupiah(RegisCashPayment.Payment.PayCash);
                    if (ConfigurationFileStatic.VFDPort != "")
                    {
                        Function.VFDPort.send("Total Pembayaran :", f.ConvertToRupiah(RegisCashPayment.Payment.PayCash), Function.VFDPort.sp.PortName);
                    }
                }
                txtControl.Text = TxtUangTerima.Name;
                txtControl.Visible = false;
                TxtUangTerima.Focus();
            }
            else
            {
                this.Close();
            }
        }

        #region Printing
        public ReturnResult PrintRegis(SaveFoodCourtPayment Data,string Datetime)
        {
            var res = new ReturnResult();
            try
            {
                string s = "Datetime \t: " + Datetime + Environment.NewLine;
                s += "ID Transaction\t: TRX" + Datetime.Replace("/", "").Replace(":", "").Replace(" ", "") + Environment.NewLine;
                s += "Merchant ID \t: " + f.GetComputerName() + Environment.NewLine;
                s += "Nama Petugas \t: " + f.GetNamaUser(General.IDUser) + Environment.NewLine;
                s += "-------------------------------------------------------" + Environment.NewLine;
                s += "Transaksi Registrasi " + Environment.NewLine ;
                foreach (var ticket in RegisCashPayment.tiket)
                {
                    s += "Nama Tiket \t: " + ticket.NamaTicket + Environment.NewLine;
                    s += "Harga Satuan \t: " + f.ConvertToRupiah(ticket.Harga) + Environment.NewLine;
                    s += "Qty \t\t: " + ticket.Qty + Environment.NewLine;
                    s += "Total \t\t: " + f.ConvertToRupiah(ticket.Total) + Environment.NewLine;
                    s += "Nama Diskon \t: " + ticket.NamaDiskon + Environment.NewLine;
                    s += "Diskon \t\t: " + ticket.Diskon + "% - "+ f.ConvertToRupiah(ticket.TotalDiskon) + Environment.NewLine;
                    s += "Total - Diskon \t: " + f.ConvertToRupiah(ticket.TotalAfterDiskon) + Environment.NewLine ;
                }
                s += "-------------------------------------------------------" + Environment.NewLine;
                s += "Total Beli Tiket \t: " + f.ConvertToRupiah(RegisCashPayment.TotalBeliTiket) + Environment.NewLine;

                if (Data != null)
                {
                    s += "-------------------------------------------------------" + Environment.NewLine;
                    s += "Transaksi Persewaan " + Environment.NewLine;

                    decimal d = 0;
                    foreach (var Items in Data.Keranjang)
                    {
                        d++;
                        s += d + ". " + Items.NamaItem + " - " + Items.Qtx + "\t : " + f.ConvertToRupiah((Items.Harga * Items.Qtx)) + Environment.NewLine;
                    }
                    s += "-------------------------------------------------------" + Environment.NewLine;
                    s += "Total Belanja \t\t: " + f.ConvertToRupiah(Data.Pay.TotalBayar) + Environment.NewLine;
                }                

                if (RegisCashPayment.Asuransi > 0)
                {
                    s += "Asuransi " + RegisCashPayment.QtyTotalTiket + " Org \t: " + f.ConvertToRupiah(RegisCashPayment.Asuransi) + Environment.NewLine;
                }

                if (RegisCashPayment.Card.SaldoJaminan == 0)
                {
                    if (RegisCashPayment.Card.SaldoJaminanAfter > 0)
                    {
                        s += "Saldo Jaminan \t: " + f.ConvertToRupiah(RegisCashPayment.Card.SaldoJaminanAfter) + Environment.NewLine;
                    }
                }

                if (RegisCashPayment.Cashback > 0)
                {
                    s += "-------------------------------------------------------" + Environment.NewLine;
                    s += "Cashback \t: - " + f.ConvertToRupiah(RegisCashPayment.Cashback) + Environment.NewLine;
                }

                if (RegisCashPayment.Topup > 0)
                {
                    s += "-------------------------------------------------------" + Environment.NewLine;
                    s += "Topup Emoney \t: " + f.ConvertToRupiah(RegisCashPayment.Topup) + Environment.NewLine;
                }

                s += "-------------------------------------------------------" + Environment.NewLine;
                s += "Total \t\t: " + f.ConvertToRupiah(RegisCashPayment.TotalAll) + Environment.NewLine;

                if (RegisCashPayment.Payment != null)
                {
                    s += "-------------------------------------------------------" + Environment.NewLine;
                    s += "Payment \t: " + RegisCashPayment.Payment.JenisTransaksi + Environment.NewLine;
                    if (RegisCashPayment.Payment.PayEmoney > 0)
                    {
                        s += "Use eMoney \t: " + f.ConvertToRupiah(RegisCashPayment.Payment.PayEmoney) + Environment.NewLine;
                    }
                    if (RegisCashPayment.Payment.PayCash > 0)
                    {
                        s += "Total Cash \t: " + f.ConvertToRupiah(RegisCashPayment.Payment.PayCash) + Environment.NewLine;
                    }

                    if (RegisCashPayment.Payment.TerimaUang > 0)
                    {
                        s += "Dibayarkan \t: " + f.ConvertToRupiah(RegisCashPayment.Payment.TerimaUang) + Environment.NewLine;
                        s += "Kembalian \t: " + f.ConvertToRupiah(RegisCashPayment.Payment.Kembalian) + Environment.NewLine;
                    }
                    if (RegisCashPayment.Payment.PayEmoney > 0)
                    {
                        s += "-------------------------------------------------------" + Environment.NewLine;
                        s += "Account Number \t: " + RegisCashPayment.Card.IdCard + "-" + f.ConvertDecimal(RegisCashPayment.Card.CodeIdAfter).ToString() + Environment.NewLine;
                        s += "Emoney Before \t: " + f.ConvertToRupiah(RegisCashPayment.Card.SaldoEmoney) + Environment.NewLine;
                        s += "Emoney Current \t: " + f.ConvertToRupiah(RegisCashPayment.Card.SaldoEmoneyAfter) + Environment.NewLine;
                        s += "Saldo Jaminan \t: " + f.ConvertToRupiah(RegisCashPayment.Card.SaldoJaminanAfter) + Environment.NewLine;
                    }
                    else
                    {
                        s += "-------------------------------------------------------" + Environment.NewLine;
                        s += "Account Number \t: " + RegisCashPayment.Card.IdCard + "-" + f.ConvertDecimal(RegisCashPayment.Card.CodeIdAfter).ToString() + Environment.NewLine;
                        s += "Previous Balance : " + f.ConvertToRupiah(RegisCashPayment.Card.SaldoEmoney) + Environment.NewLine;
                        s += "Current Balance \t: " + f.ConvertToRupiah(RegisCashPayment.Card.SaldoEmoneyAfter) + Environment.NewLine;
                        s += "Saldo Jaminan \t: " + f.ConvertToRupiah(RegisCashPayment.Card.SaldoJaminanAfter) + Environment.NewLine;
                    }
                }

                s += "-------------------------------------------------------" + Environment.NewLine;

                foreach (string pfoot in f.GetFooterPrint())
                {
                    s += pfoot + Environment.NewLine;
                }
                var printkolom = f.GetPrintKolomVisitor();
                if (printkolom.Visible == "1")
                {
                    printkolom.NoTicket = "ID" + Datetime.Replace("/", "").Replace(":", "").Replace(" ", "");
                    s += "-------------------------------------------------------" + Environment.NewLine;
                    s += printkolom.Title + Environment.NewLine;
                    s += Environment.NewLine;
                    s += "ID Ticket : " + printkolom.NoTicket + Environment.NewLine;
                    s += "-------------------------------------------------------" + Environment.NewLine;
                    s += printkolom.Nama + Environment.NewLine;
                    s += "-------------------------------------------------------" + Environment.NewLine;
                    s += printkolom.MoKtp + Environment.NewLine;
                    s += "-------------------------------------------------------" + Environment.NewLine;
                    s += printkolom.NoTelp + Environment.NewLine;
                    s += "-------------------------------------------------------" + Environment.NewLine;
                    s += printkolom.Alamat + Environment.NewLine+ Environment.NewLine;

                    s += "-------------------------------------------------------" + Environment.NewLine;
                    s += "ID Ticket : " + printkolom.NoTicket + Environment.NewLine;
                    s += "-------------------------------------------------------" + Environment.NewLine;
                    s += printkolom.Nama + Environment.NewLine;
                    s += "-------------------------------------------------------" + Environment.NewLine;
                    s += printkolom.MoKtp + Environment.NewLine;
                    s += "-------------------------------------------------------" + Environment.NewLine;
                    s += printkolom.NoTelp + Environment.NewLine;
                    s += "-------------------------------------------------------" + Environment.NewLine;
                    s += printkolom.Alamat + Environment.NewLine + Environment.NewLine;
                    
                }

                PrintDocument p = new PrintDocument();
                p.PrintPage += delegate (object sender1, PrintPageEventArgs e1)
                {
                    int HeadreX = 0;
                    int startY = 0;
                    int Offset = 0;
                    string underLine1 = "=============================================================";
                    e1.Graphics.DrawString(underLine1, new Font("calibri", 7), new SolidBrush(Color.Black), 0, startY + Offset);
                    Offset = Offset + 10;
                    e1.Graphics.DrawString(f.GetFooterPrint(1), new Font("Arial", 8, FontStyle.Bold), new SolidBrush(Color.Black), HeadreX, startY + Offset);
                    Offset = Offset + 15;
                    e1.Graphics.DrawString(f.GetFooterPrint(2), new Font("Arial", 7, FontStyle.Bold), new SolidBrush(Color.Black), new RectangleF(0, startY + Offset, p.DefaultPageSettings.PrintableArea.Width, p.DefaultPageSettings.PrintableArea.Height));
                    Offset = Offset + 15;
                    e1.Graphics.DrawString(f.GetFooterPrint(3), new Font("Arial", 5, FontStyle.Italic), new SolidBrush(Color.Black), new RectangleF(0, startY + Offset, p.DefaultPageSettings.PrintableArea.Width, p.DefaultPageSettings.PrintableArea.Height));
                    Offset = Offset + 15;
                    e1.Graphics.DrawString(underLine1, new Font("Arial", 7), new SolidBrush(Color.Black), 0, startY + Offset);
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

        public ReturnResult PrintTopup(string datetime)
        {
            var res = new ReturnResult();
            try
            {
                string s = "Datetime \t: " + datetime + Environment.NewLine;
                s += "ID Transaction\t: TRX" + datetime.Replace("/", "").Replace(":", "").Replace(" ", "") + Environment.NewLine;
                s += "Merchant ID \t: " + f.GetComputerName() + Environment.NewLine;
                s += "Nama Petugas \t: " + f.GetNamaUser(General.IDUser) + Environment.NewLine;
                s += "-------------------------------------------------------" + Environment.NewLine;
                s += "Transaksi Topup " + Environment.NewLine;
                s += "Nominal Topup \t: " + f.ConvertToRupiah(TopupCashPayment.NominalTopup) + Environment.NewLine;
                s += "Uang dibayarkan  : " + f.ConvertToRupiah(TopupCashPayment.Pay.TerimaUang) + Environment.NewLine;
                s += "Uang kembalian \t: "+f.ConvertToRupiah(TopupCashPayment.Pay.Kembalian) + Environment.NewLine;
                s += "-------------------------------------------------------" + Environment.NewLine;
                s += "Account Number \t: " + TopupCashPayment.Card.IdCard +"-"+f.ConvertDecimal(TopupCashPayment.Card.CodeId).ToString()+ Environment.NewLine;
                s += "Previous Balance : "+f.ConvertToRupiah(TopupCashPayment.Card.SaldoEmoney) + Environment.NewLine;
                s += "Current Balance   : "+f.ConvertToRupiah(TopupCashPayment.Card.SaldoEmoneyAfter) + Environment.NewLine;

                s += "-------------------------------------------------------" + Environment.NewLine;
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
                    string underLine1 = "=============================================================";
                    e1.Graphics.DrawString(underLine1, new Font("calibri", 7), new SolidBrush(Color.Black), 0, startY + Offset);
                    Offset = Offset + 10;
                    e1.Graphics.DrawString(f.GetFooterPrint(1), new Font("Arial", 8, FontStyle.Bold), new SolidBrush(Color.Black), HeadreX, startY + Offset);
                    Offset = Offset + 15;
                    e1.Graphics.DrawString(f.GetFooterPrint(2), new Font("Arial", 7, FontStyle.Bold), new SolidBrush(Color.Black), new RectangleF(0, startY + Offset, p.DefaultPageSettings.PrintableArea.Width, p.DefaultPageSettings.PrintableArea.Height));
                    Offset = Offset + 15;
                    e1.Graphics.DrawString(f.GetFooterPrint(3), new Font("Arial", 5, FontStyle.Italic), new SolidBrush(Color.Black), new RectangleF(0, startY + Offset, p.DefaultPageSettings.PrintableArea.Width, p.DefaultPageSettings.PrintableArea.Height));
                    Offset = Offset + 15;
                    e1.Graphics.DrawString(underLine1, new Font("Arial", 6), new SolidBrush(Color.Black), 0, startY + Offset);
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

        public ReturnResult PrintFoodCourt(SaveFoodCourtPayment Data,string datetime)
        {
            var res = new ReturnResult();
            try
            {
                string s = "Datetime \t: " + datetime + Environment.NewLine;
                s += "ID Transaction\t: TRX" + datetime.Replace("/", "").Replace(":", "").Replace(" ", "") + Environment.NewLine;
                s += "Merchant ID \t: " + f.GetComputerName() + Environment.NewLine;
                s += "Nama Petugas \t: " + f.GetNamaUser(General.IDUser) + Environment.NewLine;
                s += "-------------------------------------------------------" + Environment.NewLine;
                s += "Transaksi Foodcourt " + Environment.NewLine;

                decimal d = 0;

                foreach (var Items in Data.Keranjang)
                {
                    d++;
                    s += d + ". " + Items.NamaItem + " - " + Items.Qtx + "\t : " + f.ConvertToRupiah((Items.Harga * Items.Qtx)) + Environment.NewLine;
                }
                s += "-------------------------------------------------------" + Environment.NewLine;
                s += "Total \t\t: "+f.ConvertToRupiah(Data.Pay.TotalBayar) + Environment.NewLine;

                if (Data.Pay != null)
                {
                    s += "-------------------------------------------------------" + Environment.NewLine;
                    s += "Payment \t: " + Data.Pay.JenisTransaksi + Environment.NewLine;
                    s += "Uang dibayarkan  : " + f.ConvertToRupiah(Data.Pay.TerimaUang) + Environment.NewLine;
                    s += "Uang kembalian \t: " + f.ConvertToRupiah(Data.Pay.Kembalian) + Environment.NewLine;
                }

                s += "-------------------------------------------------------" + Environment.NewLine;
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
                    string underLine1 = "=============================================================";
                    e1.Graphics.DrawString(underLine1, new Font("calibri", 7), new SolidBrush(Color.Black), 0, startY + Offset);
                    Offset = Offset + 10;
                    e1.Graphics.DrawString(f.GetFooterPrint(1), new Font("Arial", 8, FontStyle.Bold), new SolidBrush(Color.Black), HeadreX, startY + Offset);
                    Offset = Offset + 15;
                    e1.Graphics.DrawString(f.GetFooterPrint(2), new Font("Arial", 7, FontStyle.Bold), new SolidBrush(Color.Black), new RectangleF(0, startY + Offset, p.DefaultPageSettings.PrintableArea.Width, p.DefaultPageSettings.PrintableArea.Height));
                    Offset = Offset + 15;
                    e1.Graphics.DrawString(f.GetFooterPrint(3), new Font("Arial", 5, FontStyle.Italic), new SolidBrush(Color.Black), new RectangleF(0, startY + Offset, p.DefaultPageSettings.PrintableArea.Width, p.DefaultPageSettings.PrintableArea.Height));
                    Offset = Offset + 15;
                    e1.Graphics.DrawString(underLine1, new Font("Arial", 7), new SolidBrush(Color.Black), 0, startY + Offset);
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
        #endregion

        private void button20_Click(object sender, EventArgs e)
        {
            ulang:
            if (TxtUangKembalian.Text != "")
            {
                decimal kembalian = f.ConvertDecimal(TxtUangKembalian.Text);
                if (kembalian < 100000)
                {
                    decimal UangTerima = f.ConvertDecimal(TxtUangTerima.Text);
                    if (CashPayment.JenisTransaksi == "TOPUP")
                    {
                    ulangTopup:
                        var card = ReadCardDataKey();
                        if (card.Success == true)
                        {
                            f.UpdatAccountData(card);
                            card.SaldoEmoneyAfter = card.SaldoEmoney + TopupCashPayment.NominalTopup;
                            var pay = new PaymentMethod();
                            pay.JenisTransaksi = CashPayment.JenisTransaksi;
                            pay.PayCash = UangTerima;
                            pay.TotalBayar = TopupCashPayment.NominalTopup;
                            pay.TerimaUang = UangTerima;
                            pay.Kembalian = pay.TerimaUang - pay.TotalBayar;
                            TopupCashPayment.Pay = pay;
                            TopupCashPayment.Card = card;
                            var Topup = UpdateBlok("04", "04", card.SaldoEmoneyAfter.ToString());
                            if (Topup.Success == true)
                            {
                                var DataSaveTopup = new SaveTopupTrx();
                                DataSaveTopup.Card = TopupCashPayment.Card;
                                DataSaveTopup.NominalTopup = TopupCashPayment.NominalTopup;
                                DataSaveTopup.Pay = TopupCashPayment.Pay;
                                DataSaveTopup.NamaUser = f.GetNamaUser(General.IDUser);
                                DataSaveTopup.ComputerName = f.GetComputerName();
                                var save = f.SaveTransaksiTopup(DataSaveTopup);
                                if (save.Message.Contains('~') == true && save.Success == true)
                                {
                                    var dtTrx = save.Message.Split('~');
                                    var ReadUpdate = ReadCardDataKey();
                                    f.UpdatAccountData(ReadUpdate);
                                ulangPrint:
                                    var print = PrintTopup(dtTrx[1].Trim());
                                    if (print.Success == true)
                                    {
                                        f.RefreshDashboard();
                                        Form frm = Application.OpenForms["Main"];
                                        if (frm != null)
                                        {
                                            Panel tbx = frm.Controls.Find("PagePanel", true).FirstOrDefault() as Panel;
                                            UserControl fc = tbx.Controls.Find("Topup", true).FirstOrDefault() as UserControl;
                                            if (fc != null)
                                            {
                                                fc.Show();
                                                fc.BringToFront();
                                                RichTextBox TxtBacaKartu = fc.Controls.Find("TxtBacaKartu", true).FirstOrDefault() as RichTextBox;
                                                if (TxtBacaKartu != null)
                                                {
                                                    TxtBacaKartu.Text = "";
                                                }
                                                TextBox txtBoxInput = fc.Controls.Find("txtBoxInput", true).FirstOrDefault() as TextBox;
                                                if (txtBoxInput != null)
                                                {
                                                    txtBoxInput.Text = "";
                                                }

                                            }
                                        }

                                        Print fp = new Print();
                                        fp.Show();
                                        fp.BringToFront();
                                        fp.StartPosition = FormStartPosition.CenterScreen;
                                        fp.txtMessageBox.Text = "Topup Berhasil sebesar " + f.ConvertToRupiah(CashPayment.TotalPembayaran);
                                        TopupCashPayment.Card = null;
                                        TopupCashPayment.NominalTopup = 0;
                                        TopupCashPayment.Pay = null;
                                        this.Close();
                                    }
                                    else
                                    {
                                        var res = MessageBox.Show(print.Message, "Print Fail", MessageBoxButtons.RetryCancel, MessageBoxIcon.Warning);
                                        if (res == DialogResult.Retry)
                                        {
                                            goto ulangPrint;
                                        }
                                    }
                                }
                                else
                                {
                                    var res = MessageBox.Show("Error : " + save.Message, "SaveTransaksiTopup failed", MessageBoxButtons.RetryCancel, MessageBoxIcon.Warning);
                                    if (res == DialogResult.Retry)
                                    {
                                        goto ulangTopup;
                                    }
                                }
                            }
                            else
                            {
                                var res = MessageBox.Show("Topup Kartu Gagal", "Update data Fail", MessageBoxButtons.RetryCancel, MessageBoxIcon.Warning);
                                if (res == DialogResult.Retry)
                                {
                                    goto ulangTopup;
                                }
                            }
                            if (Model.ConfigurationFileStatic.VFDPort != null && Model.ConfigurationFileStatic.VFDPort != "")
                            {
                                VFDPort.send("Terima Kasih", "Selamat Menikmati", VFDPort.sp.PortName);
                            }
                        }
                        else
                        {
                            var res = MessageBox.Show("Read Data Kartu Gagal", "Read data Fail", MessageBoxButtons.RetryCancel, MessageBoxIcon.Warning);
                            if (res == DialogResult.Retry)
                            {
                                goto ulangTopup;
                            }
                        }
                    }
                    else if (CashPayment.JenisTransaksi == "Registrasi")
                    {
                        ulangRegis:
                        var card = ReadCardDataKey();
                        if (card.Success == true)
                        {
                            if (RegisCashPayment.Topup > 0)
                            {
                                RegisCashPayment.Card.SaldoEmoneyAfter = RegisCashPayment.Card.SaldoEmoney + RegisCashPayment.Topup;
                            }
                            if (card != null)
                            {
                                var loadKey = LoaAuthoKey();
                                if (loadKey.Success == true)
                                {
                                    if (f.ConvertDecimal(RegisCashPayment.Card.CodeId).ToString().Length < 10)
                                    {
                                        string CodeIDAfter = f.GenCodeID();
                                        RegisCashPayment.Card.CodeIdAfter = CodeIDAfter;
                                    }
                                    else
                                    {
                                        RegisCashPayment.Card.CodeIdAfter = f.ConvertDecimal(RegisCashPayment.Card.CodeId.ToString()).ToString();
                                    }

                                    var SaldoEmoney = UpdateBlok("04", "04", RegisCashPayment.Card.SaldoEmoneyAfter.ToString());
                                    var TicketWeekDayAfter = UpdateBlok("05", "04", RegisCashPayment.Card.TicketWeekDayAfter.ToString());
                                    var TicketWeekEndAfter = UpdateBlok("06", "04", RegisCashPayment.Card.TicketWeekEndAfter.ToString());
                                    var SaldoJaminanAfter = UpdateBlok("08", "08", RegisCashPayment.Card.SaldoJaminanAfter.ToString());
                                    var CodeId = UpdateBlok("09", "08", f.ConvertDecimal(RegisCashPayment.Card.CodeIdAfter.ToString()).ToString());
                                    if (SaldoEmoney.Success == true &&
                                        TicketWeekDayAfter.Success == true &&
                                        TicketWeekEndAfter.Success == true &&
                                        SaldoJaminanAfter.Success == true &&
                                        CodeId.Success == true)
                                    {
                                        var DataSave = new SaveRegisTrx();
                                        DataSave.Asuransi = RegisCashPayment.Asuransi;
                                        DataSave.Card = RegisCashPayment.Card;
                                        DataSave.Cashback = RegisCashPayment.Cashback;
                                        DataSave.Payment = RegisCashPayment.Payment;
                                        DataSave.QtyTotalTiket = RegisCashPayment.QtyTotalTiket;
                                        DataSave.SaldoJaminan = RegisCashPayment.SaldoJaminan;
                                        DataSave.tiket = RegisCashPayment.tiket;
                                        DataSave.Topup = RegisCashPayment.Topup;
                                        DataSave.TotalAll = RegisCashPayment.TotalAll;
                                        DataSave.TotalBeliTiket = RegisCashPayment.TotalBeliTiket;
                                        DataSave.TotalSewa = RegisCashPayment.TotalSewa;
                                        DataSave.Sewa = RegisCashPayment.Sewa;

                                        string IdTicket = f.GetIdTiket();
                                        string IdItemSewa = f.GetIdTrx();
                                        string ChasierBy = f.GetNamaUser(General.IDUser);
                                        string ComputerName = f.GetComputerName();

                                        foreach (var Tiket in RegisCashPayment.tiket)
                                        {
                                            var savetiket = f.SaveTicket(Tiket, DataSave.Card.IdCard + "-" + f.ConvertDecimal(DataSave.Card.CodeIdAfter).ToString(), IdTicket, ChasierBy, ComputerName);
                                        }

                                        var save = f.SaveTransaksiRegistrasi(DataSave, IdTicket, ComputerName, ChasierBy);
                                        var dtTrx = save.Message.Split('~');
                                        var UpdateAccount = ReadCardDataKey();
                                        f.UpdatAccountData(UpdateAccount);

                                        if (RegisCashPayment.Sewa.Count() > 0)
                                        {
                                            foreach (var s in RegisCashPayment.Sewa)
                                            {
                                                var saveSewa = f.SaveItemsFB(s, DataSave.Card.IdCard + "-" + f.ConvertDecimal(DataSave.Card.CodeIdAfter).ToString(), IdItemSewa, ChasierBy, ComputerName);
                                            }

                                            var SavePOS = new SaveFoodCourtPayment();
                                            SavePOS.Card = DataSave.Card;
                                            SavePOS.Keranjang = DataSave.Sewa;
                                            var PayPos = new PaymentMethod();
                                            PayPos.JenisTransaksi = DataSave.Payment.JenisTransaksi;
                                            PayPos.PayCash = DataSave.TotalSewa;
                                            PayPos.Kembalian = 0;
                                            PayPos.PayEmoney = 0;
                                            PayPos.TerimaUang = DataSave.TotalSewa;
                                            PayPos.TotalBayar = DataSave.TotalSewa;
                                            SavePOS.Pay = PayPos;
                                            var ResSavePOS = f.SaveFoodCourtPayment(SavePOS, IdItemSewa, ChasierBy, ComputerName);
                                        Printulang:
                                            
                                            var print = PrintRegis(SavePOS,dtTrx[1].Trim());
                                            
                                            if (print.Success == true)
                                            {
                                                TxtBacaKartu.Text = "";
                                                TxtUangTerima.Text = "";
                                                TxtUangKembalian.Text = "";
                                                txtTotalBelanja.Text = "";
                                                f.RefreshDashboard();
                                                Form frm = Application.OpenForms["Main"];
                                                if (frm != null)
                                                {
                                                    Panel tbx = frm.Controls.Find("PagePanel", true).FirstOrDefault() as Panel;
                                                    UserControl fc = tbx.Controls.Find("Registrasi", true).FirstOrDefault() as UserControl;
                                                    if (fc != null)
                                                    {
                                                        fc.Show();
                                                        fc.BringToFront();

                                                        TextBox txtBanyakTicket = fc.Controls.Find("txtBanyakTicket", true).FirstOrDefault() as TextBox;
                                                        TextBox txtTotalBayarSewa = fc.Controls.Find("txtTotalBayarSewa", true).FirstOrDefault() as TextBox;

                                                        RichTextBox TxtBacaKartu = fc.Controls.Find("TxtBacaKartu", true).FirstOrDefault() as RichTextBox;
                                                        RichTextBox TxtPromoDiskon = fc.Controls.Find("TxtPromoDiskon", true).FirstOrDefault() as RichTextBox;
                                                        RichTextBox txtNota = fc.Controls.Find("txtNota", true).FirstOrDefault() as RichTextBox;

                                                        CheckBox cbTicketRegular = fc.Controls.Find("cbTicketRegular", true).FirstOrDefault() as CheckBox;
                                                        CheckBox cbTicketRombongan = fc.Controls.Find("cbTicketRombongan", true).FirstOrDefault() as CheckBox;
                                                        CheckBox cbTicketMember = fc.Controls.Find("cbTicketMember", true).FirstOrDefault() as CheckBox;
                                                        CheckBox cbTicketSekolah = fc.Controls.Find("cbTicketSekolah", true).FirstOrDefault() as CheckBox;
                                                        CheckBox CbUseEmoney = fc.Controls.Find("CbUseEmoney", true).FirstOrDefault() as CheckBox;

                                                        Panel PanelEmoney = fc.Controls.Find("PanelEmoney", true).FirstOrDefault() as Panel;
                                                        Panel PanelHitung = fc.Controls.Find("PanelHitung", true).FirstOrDefault() as Panel;

                                                        Button btnSelesai = fc.Controls.Find("btnSelesai", true).FirstOrDefault() as Button;

                                                        Panel panelCardType = fc.Controls.Find("panelCardType", true).FirstOrDefault() as Panel;
                                                        Panel panelTicket = fc.Controls.Find("panelTicket", true).FirstOrDefault() as Panel;
                                                        Panel panelAsuransi = fc.Controls.Find("panelAsuransi", true).FirstOrDefault() as Panel;
                                                        Panel panelPromo = fc.Controls.Find("panelPromo", true).FirstOrDefault() as Panel;
                                                        Panel PanelReader = fc.Controls.Find("PanelReader", true).FirstOrDefault() as Panel;
                                                        Panel PanelJaminan = fc.Controls.Find("PanelJaminan", true).FirstOrDefault() as Panel;
                                                        Panel PanelAdditional = fc.Controls.Find("PanelAdditional", true).FirstOrDefault() as Panel;
                                                        Panel panel2 = fc.Controls.Find("panel2", true).FirstOrDefault() as Panel;
                                                        DataGridView dt_grid = fc.Controls.Find("dt_grid", true).FirstOrDefault() as DataGridView;
                                                        DataGridView dt_grid2 = fc.Controls.Find("dt_grid2", true).FirstOrDefault() as DataGridView;

                                                        if (txtTotalBayarSewa != null)
                                                        {
                                                            txtTotalBayarSewa.Text = "Total : Rp 0";
                                                        }
                                                        if (dt_grid != null)
                                                        {
                                                            dt_grid.Rows.Clear();
                                                        }

                                                        if (dt_grid2 != null)
                                                        {
                                                            dt_grid2.Rows.Clear();
                                                        }

                                                        if (panel2 != null)
                                                        {
                                                            panel2.Visible = true;
                                                            panel2.Enabled = true;
                                                        }

                                                        if (PanelAdditional != null)
                                                        {
                                                            PanelAdditional.Visible = true;
                                                            PanelAdditional.Enabled = true;
                                                        }

                                                        if (panelPromo != null)
                                                        {
                                                            panelPromo.Visible = true;
                                                            panelPromo.Enabled = true;
                                                        }

                                                        if (PanelReader != null)
                                                        {
                                                            PanelReader.Visible = true;
                                                            PanelReader.Enabled = true;
                                                        }

                                                        if (PanelJaminan != null)
                                                        {
                                                            PanelJaminan.Visible = true;
                                                            PanelJaminan.Enabled = true;
                                                        }

                                                        if (TxtBacaKartu != null)
                                                        {
                                                            TxtBacaKartu.Text = "";
                                                        }

                                                        if (txtBanyakTicket != null)
                                                        {
                                                            txtBanyakTicket.Text = "";
                                                        }

                                                        if (TxtPromoDiskon != null)
                                                        {
                                                            TxtPromoDiskon.Text = "";
                                                        }

                                                        if (txtNota != null)
                                                        {
                                                            txtNota.Text = "";
                                                        }

                                                        if (cbTicketRegular != null)
                                                        {
                                                            cbTicketRegular.Checked = false;
                                                        }

                                                        if (cbTicketRombongan != null)
                                                        {
                                                            cbTicketRombongan.Checked = false;
                                                        }

                                                        if (cbTicketSekolah != null)
                                                        {
                                                            cbTicketSekolah.Checked = false;
                                                        }

                                                        if (cbTicketMember != null)
                                                        {
                                                            cbTicketMember.Checked = false;
                                                        }

                                                        if (PanelEmoney != null)
                                                        {
                                                            PanelEmoney.Visible = false;
                                                        }

                                                        if (PanelHitung != null)
                                                        {
                                                            PanelHitung.Visible = false;
                                                        }

                                                        if (panelCardType != null)
                                                        {
                                                            panelCardType.Visible = true;
                                                            panelCardType.Enabled = true;
                                                        }
                                                        if (panelTicket != null)
                                                        {
                                                            panelTicket.Visible = true;
                                                            panelTicket.Enabled = true;
                                                        }
                                                        if (panelAsuransi != null)
                                                        {
                                                            panelAsuransi.Visible = true;
                                                            panelAsuransi.Enabled = true;
                                                        }

                                                        Print f = new Print();
                                                        f.Show();
                                                        f.BringToFront();

                                                        f.StartPosition = FormStartPosition.CenterScreen;
                                                        f.txtMessageBox.Text = "Registrasi Berhasil transaksi sebesar " + (RegisCashPayment.TotalAll);
                                                        this.Close();
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                var res = MessageBox.Show(print.Message + ", tekan Print ulang", "Reader not connected", MessageBoxButtons.RetryCancel, MessageBoxIcon.Warning);
                                                if (res == DialogResult.Retry)
                                                {
                                                    goto Printulang;
                                                }
                                            }
                                        }
                                        else
                                        {
                                        Printulang:
                                            var print = PrintRegis(null,dtTrx[1].Trim());
                                            if (print.Success == true)
                                            {
                                                TxtBacaKartu.Text = "";
                                                TxtUangTerima.Text = "";
                                                TxtUangKembalian.Text = "";
                                                txtTotalBelanja.Text = "";
                                                f.RefreshDashboard();
                                                Form frm = Application.OpenForms["Main"];
                                                if (frm != null)
                                                {
                                                    Panel tbx = frm.Controls.Find("PagePanel", true).FirstOrDefault() as Panel;
                                                    UserControl fc = tbx.Controls.Find("Registrasi", true).FirstOrDefault() as UserControl;
                                                    if (fc != null)
                                                    {
                                                        fc.Show();
                                                        fc.BringToFront();

                                                        TextBox txtBanyakTicket = fc.Controls.Find("txtBanyakTicket", true).FirstOrDefault() as TextBox;
                                                        TextBox txtTotalBayarSewa = fc.Controls.Find("txtTotalBayarSewa", true).FirstOrDefault() as TextBox;

                                                        RichTextBox TxtBacaKartu = fc.Controls.Find("TxtBacaKartu", true).FirstOrDefault() as RichTextBox;
                                                        RichTextBox TxtPromoDiskon = fc.Controls.Find("TxtPromoDiskon", true).FirstOrDefault() as RichTextBox;
                                                        RichTextBox txtNota = fc.Controls.Find("txtNota", true).FirstOrDefault() as RichTextBox;

                                                        CheckBox cbTicketRegular = fc.Controls.Find("cbTicketRegular", true).FirstOrDefault() as CheckBox;
                                                        CheckBox cbTicketRombongan = fc.Controls.Find("cbTicketRombongan", true).FirstOrDefault() as CheckBox;
                                                        CheckBox cbTicketMember = fc.Controls.Find("cbTicketMember", true).FirstOrDefault() as CheckBox;
                                                        CheckBox cbTicketSekolah = fc.Controls.Find("cbTicketSekolah", true).FirstOrDefault() as CheckBox;
                                                        CheckBox CbUseEmoney = fc.Controls.Find("CbUseEmoney", true).FirstOrDefault() as CheckBox;

                                                        Panel PanelEmoney = fc.Controls.Find("PanelEmoney", true).FirstOrDefault() as Panel;
                                                        Panel PanelHitung = fc.Controls.Find("PanelHitung", true).FirstOrDefault() as Panel;

                                                        Button btnSelesai = fc.Controls.Find("btnSelesai", true).FirstOrDefault() as Button;

                                                        Panel panelCardType = fc.Controls.Find("panelCardType", true).FirstOrDefault() as Panel;
                                                        Panel panelTicket = fc.Controls.Find("panelTicket", true).FirstOrDefault() as Panel;
                                                        Panel panelAsuransi = fc.Controls.Find("panelAsuransi", true).FirstOrDefault() as Panel;
                                                        Panel panelPromo = fc.Controls.Find("panelPromo", true).FirstOrDefault() as Panel;
                                                        Panel PanelReader = fc.Controls.Find("PanelReader", true).FirstOrDefault() as Panel;
                                                        Panel PanelJaminan = fc.Controls.Find("PanelJaminan", true).FirstOrDefault() as Panel;
                                                        Panel PanelAdditional = fc.Controls.Find("PanelAdditional", true).FirstOrDefault() as Panel;
                                                        Panel panel2 = fc.Controls.Find("panel2", true).FirstOrDefault() as Panel;
                                                        DataGridView dt_grid = fc.Controls.Find("dt_grid", true).FirstOrDefault() as DataGridView;
                                                        DataGridView dt_grid2 = fc.Controls.Find("dt_grid2", true).FirstOrDefault() as DataGridView;

                                                        if (txtTotalBayarSewa != null)
                                                        {
                                                            txtTotalBayarSewa.Text = "Total : Rp 0";
                                                        }
                                                        if (dt_grid != null)
                                                        {
                                                            dt_grid.Rows.Clear();
                                                        }

                                                        if (dt_grid2 != null)
                                                        {
                                                            dt_grid2.Rows.Clear();
                                                        }

                                                        if (panel2 != null)
                                                        {
                                                            panel2.Visible = true;
                                                            panel2.Enabled = true;
                                                        }

                                                        if (PanelAdditional != null)
                                                        {
                                                            PanelAdditional.Visible = true;
                                                            PanelAdditional.Enabled = true;
                                                        }

                                                        if (panelPromo != null)
                                                        {
                                                            panelPromo.Visible = true;
                                                            panelPromo.Enabled = true;
                                                        }

                                                        if (PanelReader != null)
                                                        {
                                                            PanelReader.Visible = true;
                                                            PanelReader.Enabled = true;
                                                        }

                                                        if (PanelJaminan != null)
                                                        {
                                                            PanelJaminan.Visible = true;
                                                            PanelJaminan.Enabled = true;
                                                        }

                                                        if (TxtBacaKartu != null)
                                                        {
                                                            TxtBacaKartu.Text = "";
                                                        }

                                                        if (txtBanyakTicket != null)
                                                        {
                                                            txtBanyakTicket.Text = "";
                                                        }

                                                        if (TxtPromoDiskon != null)
                                                        {
                                                            TxtPromoDiskon.Text = "";
                                                        }

                                                        if (txtNota != null)
                                                        {
                                                            txtNota.Text = "";
                                                        }

                                                        if (cbTicketRegular != null)
                                                        {
                                                            cbTicketRegular.Checked = false;
                                                        }

                                                        if (cbTicketRombongan != null)
                                                        {
                                                            cbTicketRombongan.Checked = false;
                                                        }

                                                        if (cbTicketSekolah != null)
                                                        {
                                                            cbTicketSekolah.Checked = false;
                                                        }

                                                        if (cbTicketMember != null)
                                                        {
                                                            cbTicketMember.Checked = false;
                                                        }

                                                        if (PanelEmoney != null)
                                                        {
                                                            PanelEmoney.Visible = false;
                                                        }

                                                        if (PanelHitung != null)
                                                        {
                                                            PanelHitung.Visible = false;
                                                        }

                                                        if (panelCardType != null)
                                                        {
                                                            panelCardType.Visible = true;
                                                            panelCardType.Enabled = true;
                                                        }
                                                        if (panelTicket != null)
                                                        {
                                                            panelTicket.Visible = true;
                                                            panelTicket.Enabled = true;
                                                        }
                                                        if (panelAsuransi != null)
                                                        {
                                                            panelAsuransi.Visible = true;
                                                            panelAsuransi.Enabled = true;
                                                        }

                                                        Print frint = new Print();
                                                        frint.Show();
                                                        frint.BringToFront();
                                                        frint.StartPosition = FormStartPosition.CenterScreen;
                                                        frint.txtMessageBox.Text = "Registrasi Berhasil transaksi sebesar " + f.ConvertToRupiah(RegisCashPayment.TotalAll);
                                                        this.Close();
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                var res = MessageBox.Show(print.Message + ", tekan Print ulang", "Reader not connected", MessageBoxButtons.RetryCancel, MessageBoxIcon.Warning);
                                                if (res == DialogResult.Retry)
                                                {
                                                    goto Printulang;
                                                }
                                            }
                                        }
                                        if (Model.ConfigurationFileStatic.VFDPort != null && Model.ConfigurationFileStatic.VFDPort != "")
                                        {
                                            VFDPort.send("Terima Kasih", "Selamat Menikmati", VFDPort.sp.PortName);
                                        }
                                    }
                                    else
                                    {
                                        var res = MessageBox.Show("Error : " + SaldoJaminanAfter, "Reader not connected", MessageBoxButtons.RetryCancel, MessageBoxIcon.Warning);
                                        if (res == DialogResult.Retry)
                                        {
                                            goto ulang;
                                        }
                                    }
                                }
                                else
                                {
                                    var res = MessageBox.Show("Error : Cannot Akses to Key Card", "Reader Fail", MessageBoxButtons.RetryCancel, MessageBoxIcon.Warning);
                                    if (res == DialogResult.Retry)
                                    {
                                        goto ulang;
                                    }
                                }
                            }
                            if (Model.ConfigurationFileStatic.VFDPort != null && Model.ConfigurationFileStatic.VFDPort != "")
                            {
                                VFDPort.send("Terima Kasih", "Selamat Menikmati", VFDPort.sp.PortName);
                            }
                        }
                        else
                        {
                            TxtBacaKartu.Text = "Smart Card tidak terdeteksi \n";
                            var res = MessageBox.Show(TxtBacaKartu.Text + "Silahkan tempelkan Kartu pada reader, lalu tekan Retry?", "Reader not connected", MessageBoxButtons.RetryCancel, MessageBoxIcon.Warning);
                            if (res == DialogResult.Retry)
                            {
                                goto ulangRegis;
                            }
                        }
                    }
                    else if (CashPayment.JenisTransaksi == "FoodCourt")
                    {
                        if (FoodCourtPayment.Pay != null)
                        {
                        SaveFoodCourtPaymentLagi:
                            var DataSave = new SaveFoodCourtPayment();
                            DataSave.Card = FoodCourtPayment.Card;
                            DataSave.Keranjang = FoodCourtPayment.Keranjang;
                            DataSave.Pay = FoodCourtPayment.Pay;

                            string IdTrx = f.GetIdTrx();
                            string Chasier = f.GetNamaUser(General.IDUser);
                            string ComputerName = f.GetComputerName();

                            var HasilSave = f.SaveFoodCourtPayment(DataSave, IdTrx, Chasier, ComputerName);
                            var dtTrx = HasilSave.Message.Split('~');
                            if (HasilSave.Success == true)
                            {
                                foreach (var dataItem in DataSave.Keranjang)
                                {
                                    var SaveItems = f.SaveItemsFB(dataItem, "", IdTrx, Chasier, ComputerName);
                                }
                            PrintLagi:
                                var print = PrintFoodCourt(DataSave,dtTrx[1].Trim());
                                if (print.Success == true)
                                {
                                    f.RefreshDashboard();
                                    this.Close();
                                    Print fp = new Print();
                                    fp.Show();
                                    fp.BringToFront();
                                    fp.StartPosition = FormStartPosition.CenterScreen;
                                    fp.txtMessageBox.Text = "belanja Berhasil sebesar " + f.ConvertToRupiah(FoodCourtPayment.Pay.TotalBayar);
                                }
                                else
                                {
                                    var res3 = MessageBox.Show("Print Gagal", "Printing Fail", MessageBoxButtons.RetryCancel, MessageBoxIcon.Warning);
                                    if (res3 == DialogResult.Retry)
                                    {
                                        goto PrintLagi;
                                    }
                                }
                            }
                            else
                            {
                                var res3 = MessageBox.Show("SaveFoodCourtPayment Gagal", "SaveFoodCourtPayment Fail", MessageBoxButtons.RetryCancel, MessageBoxIcon.Warning);
                                if (res3 == DialogResult.Retry)
                                {
                                    goto SaveFoodCourtPaymentLagi;
                                }
                            }
                        }
                        else
                        {
                            var res = MessageBox.Show("SaveFoodCourtPayment is null, silahkan tekan OK untuk kembali?", "Reader not connected", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            if (res == DialogResult.OK)
                            {
                                this.Close();
                            }
                        }

                        if (Model.ConfigurationFileStatic.VFDPort != null && Model.ConfigurationFileStatic.VFDPort != "")
                        {
                            VFDPort.send("Terima Kasih", "Selamat Menikmati", VFDPort.sp.PortName);
                        }
                    }
                }

            }
        }
        
        #region CardFunction
        internal void establishContext()
        {
            retCode = card_function.Card.SCardEstablishContext(card_function.Card.SCARD_SCOPE_SYSTEM, 0, 0, ref hContext);
            if (retCode != card_function.Card.SCARD_S_SUCCESS)
            {
                MessageBox.Show("Check your device and please restart again", "Reader not connected", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                connActive = false;
                return;
            }
        }
        public void SelectDevice()
        {
            try
            {
                List<string> availableReaders = this.ListReaders();
                this.RdrState = new card_function.Card.SCARD_READERSTATE();
                readername = availableReaders[0].ToString();//selecting first device
                this.RdrState.RdrName = readername;
            }

            catch (Exception e)
            {
                Console.WriteLine("{0} Exception caught.", e);
                f.ShowMessagebox("Error :" + e.Message, "Reader Error", MessageBoxButtons.OK);
            }


        }
        public bool connectCard()
        {
            connActive = true;

            retCode = card_function.Card.SCardConnect(hContext, readername, card_function.Card.SCARD_SHARE_SHARED,
                      card_function.Card.SCARD_PROTOCOL_T0 | card_function.Card.SCARD_PROTOCOL_T1, ref hCard, ref Protocol);

            if (retCode != card_function.Card.SCARD_S_SUCCESS)
            {
                connActive = false;
                return false;
            }
            return true;
        }
        public List<string> ListReaders()
        {
            int ReaderCount = 0;
            List<string> AvailableReaderList = new List<string>();

            //Make sure a context has been established before 
            //retrieving the list of smartcard readers.
            retCode = card_function.Card.SCardListReaders(hContext, null, null, ref ReaderCount);
            if (retCode != card_function.Card.SCARD_S_SUCCESS)
            {
                MessageBox.Show("Silahkan Pasangkan RFID Reader", card_function.Card.GetScardErrMsg(retCode));
            }

            byte[] ReadersList = new byte[ReaderCount];

            retCode = card_function.Card.SCardListReaders(hContext, null, ReadersList, ref ReaderCount);
            if (retCode != card_function.Card.SCARD_S_SUCCESS)
            {
                MessageBox.Show("Silahkan Pasangkan RFID Reader", card_function.Card.GetScardErrMsg(retCode));
            }

            string rName = "";
            int indx = 0;
            if (ReaderCount > 0)
            {
                // Convert reader buffer to string
                while (ReadersList[indx] != 0)
                {

                    while (ReadersList[indx] != 0)
                    {
                        rName = rName + (char)ReadersList[indx];
                        indx = indx + 1;
                    }

                    //Add reader name to list
                    AvailableReaderList.Add(rName);
                    rName = "";
                    indx = indx + 1;

                }
            }
            return AvailableReaderList;
        }
        private string getcardUID()//only for mifare 1k cards
        {
            string cardUID = "";
            byte[] receivedUID = new byte[256];
            card_function.Card.SCARD_IO_REQUEST request = new card_function.Card.SCARD_IO_REQUEST();
            request.dwProtocol = card_function.Card.SCARD_PROTOCOL_T1;
            request.cbPciLength = System.Runtime.InteropServices.Marshal.SizeOf(typeof(card_function.Card.SCARD_IO_REQUEST));
            byte[] sendBytes = new byte[] { 0xFF, 0xCA, 0x00, 0x00, 0x00 }; //get UID command      for Mifare cards
            int outBytes = receivedUID.Length;
            int status = card_function.Card.SCardTransmit(hCard, ref request, ref sendBytes[0], sendBytes.Length, ref request, ref receivedUID[0], ref outBytes);

            if (status != card_function.Card.SCARD_S_SUCCESS)
            {
                cardUID = "Error";
            }
            else
            {
                cardUID = BitConverter.ToString(receivedUID.Take(4).ToArray()).Replace("-", string.Empty).ToUpper();
            }

            return cardUID;
        }
        private void ClearBuffers()
        {

            long indx;

            for (indx = 0; indx <= 262; indx++)
            {

                RecvBuff[indx] = 0;
                SendBuff[indx] = 0;

            }

        }
        public int SendAPDU()
        {
            int indx;
            string tmpStr;

            pioSendRequest.dwProtocol = Aprotocol;
            pioSendRequest.cbPciLength = 8;

            // Display Apdu In
            tmpStr = "";
            for (indx = 0; indx <= SendLen - 1; indx++)
            {

                tmpStr = tmpStr + " " + string.Format("{0:X2}", SendBuff[indx]);

            }
            displayOut(2, 0, tmpStr);
            retCode = card_function.Card.SCardTransmit(hCard, ref pioSendRequest, ref SendBuff[0], SendLen, ref pioSendRequest, ref RecvBuff[0], ref RecvLen);

            if (retCode != card_function.Card.SCARD_S_SUCCESS)
            {

                displayOut(1, retCode, "");
                return retCode;

            }

            tmpStr = "";
            for (indx = 0; indx <= RecvLen - 1; indx++)
            {

                tmpStr = tmpStr + " " + string.Format("{0:X2}", RecvBuff[indx]);

            }

            displayOut(3, 0, tmpStr);
            return retCode;

        }
        private void displayOut(int errType, int retVal, string PrintText)
        {

            switch (errType)
            {

                case 0:
                    TxtBacaKartu.SelectionColor = Color.Green;
                    break;
                case 1:
                    TxtBacaKartu.SelectionColor = Color.Red;
                    PrintText = card_function.Card.GetScardErrMsg(retVal);
                    break;
                case 2:
                    TxtBacaKartu.SelectionColor = Color.Black;
                    PrintText = "<" + PrintText;
                    break;
                case 3:
                    TxtBacaKartu.SelectionColor = Color.Black;
                    PrintText = ">" + PrintText;
                    break;
                case 4:
                    TxtBacaKartu.SelectionColor = Color.Red;
                    break;

            }

            TxtBacaKartu.AppendText(PrintText);
            TxtBacaKartu.AppendText("\n");
            TxtBacaKartu.SelectionColor = Color.Black;
            TxtBacaKartu.Focus();

        }
        private ReturnResult LoaAuthoKey()
        {
            var data = new ReturnResult();
            try
            {
                string tmpStr = "";
                ClearBuffers();
                SendBuff[0] = 0xFF;                                                                        // Class
                SendBuff[1] = 0x82;                                                                        // INS
                SendBuff[2] = 0x00;                                                                        // P1 : Key Structure
                SendBuff[3] = 0x00;
                SendBuff[4] = 0x06;                                                                        // P3 : Lc
                SendBuff[5] = 0xFF;        // Key 1 value
                SendBuff[6] = 0xFF;        // Key 2 value
                SendBuff[7] = 0xFF;        // Key 3 value
                SendBuff[8] = 0xFF;        // Key 4 value
                SendBuff[9] = 0xFF;        // Key 5 value
                SendBuff[10] = 0xFF;       // Key 6 value

                SendLen = 16;
                RecvLen = 2;

                retCode = SendAPDU();

                if (retCode != card_function.Card.SCARD_S_SUCCESS)
                {
                    data.RetCode = retCode;
                    data.Success = false;
                    data.Message = "LoaAuthoKey Failed";
                }
                else
                {
                    tmpStr = "";
                    for (int indx = RecvLen - 2; indx <= RecvLen - 1; indx++)
                    {
                        tmpStr = tmpStr + " " + string.Format("{0:X2}", RecvBuff[indx]);
                    }
                }
                if (tmpStr.Trim() != "90 00")
                {
                    data.RetCode = retCode;
                    data.Success = true;
                    data.Message = "LoaAuthoKey Succes";
                }
            }
            catch (Exception ex)
            {
                data.RetCode = retCode;
                data.Success = false;
                data.Message = "Load authentication keys error!" + ex.Message;
            }
            return data;
        }
        private ReturnResult Authenticate(string Blocknumber)
        {
            var res = new ReturnResult();
            int indx;
            string tmpStr = "";

            ClearBuffers();

            SendBuff[0] = 0xFF;                             // Class
            SendBuff[1] = 0x86;                             // INS
            SendBuff[2] = 0x00;                             // P1
            SendBuff[3] = 0x00;                             // P2
            SendBuff[4] = 0x05;                             // Lc
            SendBuff[5] = 0x01;                             // Byte 1 : Version number
            SendBuff[6] = 0x00;                             // Byte 2
            SendBuff[7] = (byte)int.Parse("" + Blocknumber + "");     // Byte 3 : Block number
            SendBuff[8] = 0x60;

            SendBuff[9] = byte.Parse("00", System.Globalization.NumberStyles.HexNumber);        // Key 5 value

            SendLen = 10;
            RecvLen = 2;

            retCode = SendAPDU();

            if (retCode != card_function.Card.SCARD_S_SUCCESS)
            {
                res.Message = "card_function error :" + retCode;
                res.RetCode = retCode;
                res.Success = false;
            }
            else
            {
                tmpStr = "";
                for (indx = 0; indx <= RecvLen - 1; indx++)
                {
                    tmpStr = tmpStr + " " + string.Format("{0:X2}", RecvBuff[indx]);
                }
            }
            if (tmpStr.Trim() == "90 00")
            {
                displayOut(0, 0, "Authentication success!");
                res.Message = "Authentication success";
                res.Success = true;
            }
            else
            {
                displayOut(4, 0, "Authentication failed!");
                res.Message = "Authentication failed";
                res.Success = false;
            }

            return res;
        }
        private ReturnResult ReadBlock(string BinBlk, string Autho)
        {
            var data = new ReturnResult();
            string tmpStr = "";
            try
            {
                Authenticate(Autho);
                int indx;
                ClearBuffers();
                SendBuff[0] = 0xFF;
                SendBuff[1] = 0xB0;
                SendBuff[2] = 0x00;
                SendBuff[3] = (byte)int.Parse(BinBlk);
                SendBuff[4] = (byte)int.Parse("16");

                SendLen = 5;
                RecvLen = SendBuff[4] + 2;

                retCode = SendAPDU();

                if (retCode != card_function.Card.SCARD_S_SUCCESS)
                {
                    data.RetCode = retCode;
                    data.Success = false;
                    data.Message = "Send Request Error";
                }
                else
                {
                    tmpStr = "";
                    for (indx = RecvLen - 2; indx <= RecvLen - 1; indx++)
                    {
                        tmpStr = tmpStr + " " + string.Format("{0:X2}", RecvBuff[indx]);
                    }

                }
                if (tmpStr.Trim() == "90 00")
                {
                    tmpStr = "";
                    for (indx = 0; indx <= RecvLen - 3; indx++)
                    {

                        tmpStr = tmpStr + Convert.ToChar(RecvBuff[indx]);
                    }

                    data.RetCode = retCode;
                    data.Success = true;
                    data.Message = tmpStr;
                }
                else
                {
                    data.RetCode = retCode;
                    data.Success = false;
                    data.Message = "Read block error!";
                }
            }
            catch (Exception ex)
            {
                data.RetCode = retCode;
                data.Success = false;
                data.Message = "Read block error! - " + ex.Message;
            }
            return data;
        }
        private ReturnResult UpdateBlok(string BinBlk, string AuthoBin, string BinData)
        {
            var res = new ReturnResult();
            var Authorize = Authenticate(AuthoBin);
            if (Authorize.Success == true)
            {
                string tmpStr;
                int indx;
                tmpStr = BinData;
                ClearBuffers();
                SendBuff[0] = 0xFF;                                     // CLA
                SendBuff[1] = 0xD6;                                     // INS
                SendBuff[2] = 0x00;                                     // P1
                SendBuff[3] = (byte)int.Parse(BinBlk);            // P2 : Starting Block No.
                SendBuff[4] = (byte)int.Parse("16");            // P3 : Data length

                for (indx = 0; indx <= (tmpStr).Length - 1; indx++)
                {

                    SendBuff[indx + 5] = (byte)tmpStr[indx];

                }
                SendLen = SendBuff[4] + 5;
                RecvLen = 0x02;

                retCode = SendAPDU();

                if (retCode != card_function.Card.SCARD_S_SUCCESS)
                {
                    res.Success = false;
                    res.Message = "Card Function Fail : " + retCode;
                }
                else
                {
                    tmpStr = "";
                    for (indx = 0; indx <= RecvLen - 1; indx++)
                    {

                        tmpStr = tmpStr + " " + string.Format("{0:X2}", RecvBuff[indx]);

                    }

                }
                if (tmpStr.Trim() == "90 00")
                {
                    res.Success = true;
                    res.Message = "Encode Berhasil";
                }
                else
                {
                    displayOut(2, 0, "");
                    res.Success = false;
                    res.Message = "Encode Gagal";
                }
            }
            else
            {
                res.Success = false;
                res.Message = "Encode Gagal";
            }
            return res;
        }
        public ReaderCard ReadCardDataKey()
        {
            var res = new ReaderCard();
            SelectDevice();
            establishContext();
            if (connectCard())
            {
                string cardUID = getcardUID();
                if (cardUID != "Error" || cardUID != "99")
                {
                    int count = (cardUID.Length / 2) - 1;
                    string[] array_data = new string[cardUID.Length / 2];
                    int itung = 0;
                    for (int a = 0; a < cardUID.Length; a++)
                    {
                        int c = a % 2;
                        if (c == 0)
                        {
                            array_data[itung] = cardUID.Substring(a, 2);
                            itung++;
                        }
                    }
                    string id_card = "";
                    for (int a = array_data.Count() - 1; a >= 0; a--)
                    {
                        if (a == array_data.Count() - 1)
                        {
                            id_card = id_card + array_data[a];
                        }
                        else
                        {
                            id_card = id_card + array_data[a];
                        }

                    }
                    uint num = uint.Parse(id_card, System.Globalization.NumberStyles.AllowHexSpecifier);
                    if (num != 0)
                    {
                        res.IdCard = num.ToString();
                        var loadKey = LoaAuthoKey();
                        if (loadKey.Success == true)
                        {
                            var SaldoEmoney = ReadBlock("04", "04");
                            var tiketWeekDay = ReadBlock("05", "04");
                            var tiketWeekEnd = ReadBlock("06", "04");
                            var JaminanSaldo = ReadBlock("08", "08");
                            var CodeId = ReadBlock("09", "08");
                            if (SaldoEmoney.Success == true && tiketWeekDay.Success == true 
                                && tiketWeekEnd.Success == true && JaminanSaldo.Success == true 
                                && CodeId.Success == true)
                            {
                                res.SaldoEmoney = f.ConvertDecimal(SaldoEmoney.Message);
                                res.TicketWeekDay = f.ConvertDecimal(tiketWeekDay.Message);
                                res.TicketWeekEnd = f.ConvertDecimal(tiketWeekEnd.Message);
                                res.SaldoJaminan = f.ConvertDecimal(JaminanSaldo.Message);
                                res.CodeId = f.ConvertDecimal(CodeId.Message).ToString();
                                res.Success = true;
                                res.Message = "Reading Card success";
                            }
                            else
                            {
                                res.Success = false;
                                res.Message = "Reading Card fail";
                            }
                        }
                        else
                        {
                            res.Success = false;
                            res.Message = "loadKey Card fail";
                        }
                    }
                    else
                    {
                        res.Success = false;
                        res.Message = "Smart Card UID tidak terdeteksi";
                    }
                }
                else
                {
                    res.Success = false;
                    res.Message = "Smart Card UID tidak terdeteksi";
                }
            }
            else
            {
                res.Success = false;
                res.Message = "Smart Card UID tidak terdeteksi";
            }
            return res;
        }
        #endregion

        #region KeyboardFunc
        public void OpenDrawer()
        {
            PrintDialog pd = new PrintDialog();
            pd.PrinterSettings = new PrinterSettings();
            byte[] codeOpenCashDrawer = new byte[] { 27, 112, 48, 55, 121 };
            IntPtr pUnmanagedBytes = new IntPtr(0);
            pUnmanagedBytes = Marshal.AllocCoTaskMem(5);
            Marshal.Copy(codeOpenCashDrawer, 0, pUnmanagedBytes, 5);
            Function.RawPrinterHelper.SendBytesToPrinter(pd.PrinterSettings.PrinterName, pUnmanagedBytes, 5);
            Marshal.FreeCoTaskMem(pUnmanagedBytes);
        }
        public void pd_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            string eDrawer = (char)27 + "";
            e.Graphics.DrawString(eDrawer, new Font("Arial", 1), new SolidBrush(Color.Red), new PointF(0, 0));
        }
        void pd_EndPrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {

        }

        void pd_BeginPrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {

        }
        public void input_keyPad(string key, string Object)
        {
            TextBox txt = this.Controls.Find(Object, true).FirstOrDefault() as TextBox;
            if (txt != null)
            {
                if (key == "<-")
                {
                    if (txt.Text.Length > 0)
                    {
                        txt.Text = txt.Text.Remove(txt.Text.Length - 1, 1);
                        string data = txt.Text.Replace(".", "").Replace(",", "");
                        if (data != "")
                        {
                            decimal t = Convert.ToDecimal(data);
                            txt.Text = string.Format("{0:n0}", t);
                        }
                        else
                        {
                            txt.Text = "0";
                        }
                    }
                }
                else if (key == "Reset")
                {
                    txt.Text = "0";
                }
                else if (key == "Enter")
                {
                }
                else
                {
                    string data = (txt.Text + key).Replace(".", "").Replace(",", "");
                    if (data != "")
                    {
                        decimal t = Convert.ToDecimal(data);
                        txt.Text = string.Format("{0:n0}", t);
                    }
                    else
                    {
                        txt.Text = txt.Text + key;
                    }
                }

                if (TxtUangTerima.Text != "0" && TxtUangTerima.Text != "")
                {
                    string txtTerima = TxtUangTerima.Text.Replace(".", "").Replace(",", "");
                    if (txtTerima.All(char.IsDigit) == true)
                    {
                        decimal Terima = Convert.ToDecimal(txtTerima);
                        if (CashPayment.JenisTransaksi == "Registrasi")
                        {
                            if (Terima >= RegisCashPayment.Payment.PayCash)
                            {
                                decimal sisa = Terima - RegisCashPayment.Payment.PayCash;
                                CashPayment.Kembalian = sisa;
                                TxtUangKembalian.Text = f.ConvertToRupiah(sisa);
                                RegisCashPayment.Payment.TerimaUang = Terima;
                                RegisCashPayment.Payment.Kembalian = sisa;
                                OpenDrawer();
                            }
                            else
                            {
                                TxtUangKembalian.Text = "";
                            }
                        }
                        else if (CashPayment.JenisTransaksi == "TOPUP")
                        {
                            if (Terima >= TopupCashPayment.Pay.TotalBayar)
                            {
                                decimal sisa = Terima - TopupCashPayment.Pay.TotalBayar;
                                TopupCashPayment.Pay.TerimaUang = Terima;
                                TopupCashPayment.Pay.Kembalian = sisa;
                                TxtUangKembalian.Text = f.ConvertToRupiah(sisa);
                                OpenDrawer();
                            }
                            else
                            {
                                TxtUangKembalian.Text = "";
                            }
                        }
                        if (CashPayment.JenisTransaksi == "FoodCourt")
                        {
                            if (Terima >= FoodCourtPayment.Pay.TotalBayar)
                            {
                                decimal sisa = Terima - FoodCourtPayment.Pay.TotalBayar;
                                CashPayment.Kembalian = sisa;
                                TxtUangKembalian.Text = f.ConvertToRupiah(sisa);
                                FoodCourtPayment.Pay.TerimaUang = Terima;
                                FoodCourtPayment.Pay.Kembalian = sisa;
                                OpenDrawer();
                            }
                            else
                            {
                                TxtUangKembalian.Text = "";
                            }
                        }
                    }
                }
                else
                {
                    TxtUangKembalian.Text = "";
                }
            }
        }

        private void button18_Click(object sender, EventArgs e)
        {
            input_keyPad((sender as Button).Text, txtControl.Text);
        }

        private void button16_Click(object sender, EventArgs e)
        {
            input_keyPad((sender as Button).Text, txtControl.Text);
        }

        private void button15_Click(object sender, EventArgs e)
        {
            input_keyPad((sender as Button).Text, txtControl.Text);
        }

        private void button17_Click(object sender, EventArgs e)
        {
            input_keyPad((sender as Button).Text, txtControl.Text);
        }

        private void button14_Click(object sender, EventArgs e)
        {
            input_keyPad((sender as Button).Text, txtControl.Text);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            input_keyPad((sender as Button).Text, txtControl.Text);
        }

        private void button12_Click(object sender, EventArgs e)
        {
            input_keyPad((sender as Button).Text, txtControl.Text);
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void TxtUangTerima_Click(object sender, EventArgs e)
        {
            
        }

        private void panel3_Paint(object sender, PaintEventArgs e)
        {
            panel3.Location = new Point(
    this.ClientSize.Width / 2 - panel3.Size.Width / 2,
    this.ClientSize.Height / 2 - panel3.Size.Height / 2);
            panel3.Anchor = AnchorStyles.None;
        }

        private void button10_Click(object sender, EventArgs e)
        {
            input_keyPad((sender as Button).Text, txtControl.Text);
        }

        private void button9_Click(object sender, EventArgs e)
        {
            input_keyPad((sender as Button).Text, txtControl.Text);
        }

        private void button11_Click(object sender, EventArgs e)
        {
            input_keyPad((sender as Button).Text, txtControl.Text);
        }

        private void button8_Click(object sender, EventArgs e)
        {
            input_keyPad((sender as Button).Text, txtControl.Text);
        }

        private void button7_Click(object sender, EventArgs e)
        {
            input_keyPad((sender as Button).Text, txtControl.Text);
        }

        private void button21_Click(object sender, EventArgs e)
        {
            input_keyPad((sender as Button).Text, txtControl.Text);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            input_keyPad((sender as Button).Text, txtControl.Text);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            
        }


        #endregion

        public void UpdateDashBoardTopup()
        {
            Form frm = Application.OpenForms["Main"];
            if (frm != null)
            {
                Panel tbx = frm.Controls.Find("PagePanel", true).FirstOrDefault() as Panel;
                UserControl fc = tbx.Controls.Find("Dashboard", true).FirstOrDefault() as UserControl;
                if (fc != null)
                {
                    TextBox TxtTotalTopup = fc.Controls.Find("TxtTotalTopup", true).FirstOrDefault() as TextBox;
                    if (TxtTotalTopup != null)
                    {
                        decimal TotalTopup = f.ConvertDecimal(f.GetTotalTopup(f.GetComputerName()));
                        TxtTotalTopup.Text = f.ConvertToRupiah(TotalTopup);
                    }
                }
            }
        }

    }
}
