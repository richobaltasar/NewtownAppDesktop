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
using System.Drawing.Printing;

namespace Ewats_App.Page
{
    public partial class Registrasi : UserControl
    {
        GlobalFunc f = new GlobalFunc();
        ReaderFunction NFC = new ReaderFunction();
        
        public int retCode, hContext, hCard, Protocol;
        public bool connActive = false;
        public bool autoDet;
        public byte[] SendBuff = new byte[263];
        public byte[] RecvBuff = new byte[263];
        public int SendLen, RecvLen, nBytesRet, reqType, Aprotocol, dwProtocol, cbPciLength;

        string readername = "ACS ACR122 0";

        public card_function.Card.SCARD_READERSTATE RdrState;
        public card_function.Card.SCARD_IO_REQUEST pioSendRequest;
        public List<KeranjangTicket> data = new List<KeranjangTicket>();
        public List<KeranjangFoodcourt> dataSewa = new List<KeranjangFoodcourt>();

        public Registrasi()
        {
            InitializeComponent();
        }

        #region Event
        private void Registrasi_Load(object sender, EventArgs e)
        {
            clearAll();
            atur_grid3();
            GetAll();
        }
        private void button15_Click(object sender, EventArgs e)
        {
            f.PageControl("Payment");
        }
        private void cbTicketSekolah_CheckedChanged(object sender, EventArgs e)
        {
            bool cbTicketSekolah = true;
            if (cbTicketSekolah == true)
            {
                registrasiModel.namaticket = "Sekolah";
                string hargaTicket = "";
                if (hargaTicket != "")
                {
                    registrasiModel.harga = Convert.ToDecimal(hargaTicket);
                }
            }
        }
        private void button13_Click_1(object sender, EventArgs e)
        {
            PanelControl(true);
        }
        private void BtnCheckOut_Click(object sender, EventArgs e)
        {
            f.PageControl("Payment");
        }
        private void BtnBatal_Click(object sender, EventArgs e)
        {
            Reset();
            PanelControl(false);
            
        }
        private void button20_Click(object sender, EventArgs e)
        {
            var Hitung = new RegistrasiCheckout();
            var Card = ReadCardDataKey();
            if (Card.Message != "Reading Card fail")
            {
                if (Card.Success == true)
                {
                    Card.CodeId = f.ConvertDecimal(Card.CodeId).ToString();
                    if (Card.CodeId.Length == 14)
                    {
                        var Aktif = f.CheckExpired(Card.IdCard, Card.CodeId.ToString());
                        if (Aktif.Message == "Aktif")
                        {
                            f.UpdatAccountData(Card);
                            RegisCashPayment.Card = Card;
                            RegisDebitPayment.Card = Card;
                            TxtBacaKartu.Text = "ACCOUNT DETAIL";
                            TxtBacaKartu.Text = TxtBacaKartu.Text + "\n================";
                            TxtBacaKartu.Text = TxtBacaKartu.Text + "\n Account Number : " + Card.IdCard;
                            TxtBacaKartu.Text = TxtBacaKartu.Text + "\n Code ID \t: " + f.ConvertDecimal(Card.CodeId).ToString();
                            TxtBacaKartu.Text = TxtBacaKartu.Text + "\n Saldo Emoney \t: Rp " + string.Format("{0:n0}", Card.SaldoEmoney);
                            TxtBacaKartu.Text = TxtBacaKartu.Text + "\n Ticket WeekDay \t: " + string.Format("{0:n0}", Card.TicketWeekDay);
                            TxtBacaKartu.Text = TxtBacaKartu.Text + "\n Ticket WeekEnd \t: " + string.Format("{0:n0}", Card.TicketWeekEnd);
                            TxtBacaKartu.Text = TxtBacaKartu.Text + "\n SaldoJaminan \t: Rp " + string.Format("{0:n0}", Card.SaldoJaminan);

                            Hitung.Card = Card;
                            if (data.Count() > 0)
                            {
                                decimal totalBeliTiket = 0;
                                decimal TotalTicket = 0;

                                string Cashback = btnCashback.Text.Replace("Cashback : Rp", "").Replace(".", "").Replace(",", "").Trim();
                                string Topup = btnTopup.Text.Replace("Topup : Rp ", "").Replace(".", "").Replace(",", "");
                                if (Cashback.All(char.IsDigit) == true)
                                {
                                    Hitung.Cashback = Convert.ToDecimal(Cashback);
                                }
                                if (Topup.All(char.IsDigit) == true)
                                {
                                    Hitung.Topup = f.ConvertDecimal(Topup);
                                    Hitung.Card.SaldoEmoneyAfter = Hitung.Topup + Hitung.Card.SaldoEmoney;
                                }
                                if (cbJaminan.Checked == true)
                                {
                                    if (Hitung.Card.SaldoJaminan == 0)
                                    {
                                        if (TxtJaminan.Text != "")
                                        {
                                            string jmn = TxtJaminan.Text.Replace(".", "").Replace("Rp", "").Replace(",", "").Trim();
                                            if (jmn.All(char.IsDigit) == true)
                                            {
                                                Hitung.Card.SaldoJaminanAfter = Convert.ToDecimal(jmn.Trim());
                                            }
                                        }
                                    }
                                    else
                                    {
                                        Hitung.Card.SaldoJaminanAfter = Card.SaldoJaminan;
                                    }
                                }

                                foreach (var d in data)
                                {
                                    totalBeliTiket = totalBeliTiket + d.TotalAfterDiskon;
                                    TotalTicket = TotalTicket + d.Qty;
                                    Hitung.QtyTotalTiket = TotalTicket;

                                }

                                decimal totalSewa = 0;
                                foreach (var tt in dataSewa)
                                {
                                    totalSewa = totalSewa + (tt.Harga * tt.Qtx);
                                }


                                if (cbAsuransi.Checked == true)
                                {
                                    if (HargaAsuransi.Text != "")
                                    {
                                        string Asr = HargaAsuransi.Text.Replace(".", "").Replace("Rp", "").Replace(",", "").Trim();
                                        if (Asr.All(char.IsDigit) == true)
                                        {
                                            Hitung.Asuransi = Convert.ToDecimal(Asr.Trim()) * TotalTicket;
                                        }
                                    }
                                }

                                Hitung.Card.TicketWeekDayAfter = Card.TicketWeekDay + TotalTicket;

                                decimal sumAll = 0;

                                if (Hitung.Card.SaldoJaminan == 0)
                                {
                                    sumAll = (totalBeliTiket + Hitung.Asuransi + Hitung.Card.SaldoJaminanAfter + Hitung.Topup + totalSewa);
                                }
                                else
                                {
                                    sumAll = (totalBeliTiket + Hitung.Asuransi + Hitung.Topup + totalSewa);
                                }

                                if (Hitung.Cashback > sumAll)
                                {
                                    f.ShowMessagebox("Sorry Cashback tidak valid", "Information", MessageBoxButtons.OK);
                                }
                                else
                                {
                                    if (Card != null)
                                    {
                                        if (Card.SaldoEmoney > 0)
                                        {
                                            PanelEmoney.Visible = true;
                                            CbUseEmoney.Checked = false;
                                            lblUseEmoney.Text = f.ConvertToRupiah(Card.SaldoEmoney);
                                        }
                                        PanelHitung.Visible = true;
                                        lblTotalBayar.Text = string.Format("{0:n0}", (sumAll - RegisCashPayment.Cashback));
                                        PanelReader.Enabled = false;
                                        panelCardType.Enabled = false;
                                        panelAsuransi.Enabled = false;
                                        PanelJaminan.Enabled = false;
                                        PanelAdditional.Enabled = false;
                                        panel2.Enabled = false;

                                        RegisCashPayment.Asuransi = Hitung.Asuransi;
                                        RegisCashPayment.Card = Hitung.Card;
                                        RegisCashPayment.Cashback = Hitung.Cashback;
                                        RegisCashPayment.tiket = data;
                                        RegisCashPayment.Sewa = dataSewa;
                                        RegisCashPayment.QtyTotalTiket = TotalTicket;
                                        RegisCashPayment.SaldoJaminan = Hitung.SaldoJaminan;
                                        RegisCashPayment.Topup = Hitung.Topup;
                                        RegisCashPayment.TotalBeliTiket = Decimal.Round(totalBeliTiket, 0);
                                        RegisCashPayment.TotalSewa = Decimal.Round(totalSewa, 0);
                                        RegisCashPayment.TotalAll = Decimal.Round(sumAll - Hitung.Cashback, 0);
                                        if (Model.ConfigurationFileStatic.VFDPort != null && Model.ConfigurationFileStatic.VFDPort != "")
                                        {
                                            VFDPort.send("Total Pembayaran :", f.ConvertToRupiah(RegisCashPayment.TotalAll), VFDPort.sp.PortName);
                                        }
                                    }

                                }

                            }
                            else
                            {
                                f.ShowMessagebox("Keranjang Tiket Masih Kosong", "Information", MessageBoxButtons.OK);
                            }
                        }
                        else
                        {
                            TxtBacaKartu.Text = "ACCOUNT DETAIL";
                            TxtBacaKartu.Text = TxtBacaKartu.Text + "\n================";
                            TxtBacaKartu.Text = TxtBacaKartu.Text + "\n Account Number : " + Card.IdCard;
                            TxtBacaKartu.Text = TxtBacaKartu.Text + "\n Code ID \t: " + f.ConvertDecimal(Card.CodeId).ToString();
                            TxtBacaKartu.Text = TxtBacaKartu.Text + "\n Account telah Expired";
                        }
                    }
                    else
                    {
                        TxtBacaKartu.Text = "ACCOUNT DETAIL";
                        TxtBacaKartu.Text = TxtBacaKartu.Text + "\n================";
                        TxtBacaKartu.Text = TxtBacaKartu.Text + "\n Account Number : " + Card.IdCard;
                        TxtBacaKartu.Text = TxtBacaKartu.Text + "\n Code ID \t: -";
                        TxtBacaKartu.Text = TxtBacaKartu.Text + "\n Saldo Emoney \t: Rp " + string.Format("{0:n0}", Card.SaldoEmoney);
                        TxtBacaKartu.Text = TxtBacaKartu.Text + "\n Ticket WeekDay \t: " + string.Format("{0:n0}", Card.TicketWeekDay);
                        TxtBacaKartu.Text = TxtBacaKartu.Text + "\n Ticket WeekEnd \t: " + string.Format("{0:n0}", Card.TicketWeekEnd);
                        TxtBacaKartu.Text = TxtBacaKartu.Text + "\n SaldoJaminan \t: Rp " + string.Format("{0:n0}", Card.SaldoJaminan);

                        Hitung.Card = Card;
                        if (data.Count() > 0)
                        {
                            decimal totalBeliTiket = 0;
                            decimal TotalTicket = 0;

                            string Cashback = btnCashback.Text.Replace("Cashback : Rp", "").Replace(".", "").Replace(",", "").Trim();
                            string Topup = btnTopup.Text.Replace("Topup : Rp ", "").Replace(".", "").Replace(",", "");
                            if (Cashback.All(char.IsDigit) == true)
                            {
                                Hitung.Cashback = Convert.ToDecimal(Cashback);
                            }
                            if (Topup.All(char.IsDigit) == true)
                            {
                                Hitung.Topup = f.ConvertDecimal(Topup);
                                Hitung.Card.SaldoEmoneyAfter = Hitung.Topup + Hitung.Card.SaldoEmoney;
                            }
                            if (cbJaminan.Checked == true)
                            {
                                if (Hitung.Card.SaldoJaminan == 0)
                                {
                                    if (TxtJaminan.Text != "")
                                    {
                                        string jmn = TxtJaminan.Text.Replace(".", "").Replace("Rp", "").Replace(",", "").Trim();
                                        if (jmn.All(char.IsDigit) == true)
                                        {
                                            Hitung.Card.SaldoJaminanAfter = Convert.ToDecimal(jmn.Trim());
                                        }
                                    }
                                }
                                else
                                {
                                    Hitung.Card.SaldoJaminanAfter = Card.SaldoJaminan;
                                }
                            }

                            foreach (var d in data)
                            {
                                totalBeliTiket = totalBeliTiket + d.TotalAfterDiskon;
                                TotalTicket = TotalTicket + d.Qty;
                                Hitung.QtyTotalTiket = TotalTicket;

                            }

                            decimal totalSewa = 0;
                            foreach (var tt in dataSewa)
                            {
                                totalSewa = totalSewa + (tt.Harga * tt.Qtx);
                            }


                            if (cbAsuransi.Checked == true)
                            {
                                if (HargaAsuransi.Text != "")
                                {
                                    string Asr = HargaAsuransi.Text.Replace(".", "").Replace("Rp", "").Replace(",", "").Trim();
                                    if (Asr.All(char.IsDigit) == true)
                                    {
                                        Hitung.Asuransi = Convert.ToDecimal(Asr.Trim()) * TotalTicket;
                                    }
                                }
                            }

                            Hitung.Card.TicketWeekDayAfter = Card.TicketWeekDay + TotalTicket;

                            decimal sumAll = 0;

                            if (Hitung.Card.SaldoJaminan == 0)
                            {
                                sumAll = (totalBeliTiket + Hitung.Asuransi + Hitung.Card.SaldoJaminanAfter + Hitung.Topup + totalSewa);
                            }
                            else
                            {
                                sumAll = (totalBeliTiket + Hitung.Asuransi + Hitung.Topup + totalSewa);
                            }

                            if (Hitung.Cashback > sumAll)
                            {
                                f.ShowMessagebox("Sorry Cashback tidak valid", "Information", MessageBoxButtons.OK);
                            }
                            else
                            {
                                if (Card != null)
                                {
                                    if (Card.SaldoEmoney > 0)
                                    {
                                        PanelEmoney.Visible = true;
                                        CbUseEmoney.Checked = false;
                                        lblUseEmoney.Text = f.ConvertToRupiah(Card.SaldoEmoney);
                                    }
                                    PanelHitung.Visible = true;
                                    lblTotalBayar.Text = string.Format("{0:n0}", (sumAll - RegisCashPayment.Cashback));
                                    PanelReader.Enabled = false;
                                    panelCardType.Enabled = false;
                                    panelAsuransi.Enabled = false;
                                    PanelJaminan.Enabled = false;
                                    PanelAdditional.Enabled = false;
                                    panel2.Enabled = false;

                                    RegisCashPayment.Asuransi = Hitung.Asuransi;
                                    RegisCashPayment.Card = Hitung.Card;
                                    RegisCashPayment.Cashback = Hitung.Cashback;
                                    RegisCashPayment.tiket = data;
                                    RegisCashPayment.Sewa = dataSewa;
                                    RegisCashPayment.QtyTotalTiket = TotalTicket;
                                    RegisCashPayment.SaldoJaminan = Hitung.SaldoJaminan;
                                    RegisCashPayment.Topup = Hitung.Topup;
                                    RegisCashPayment.TotalBeliTiket = Decimal.Round(totalBeliTiket, 0);
                                    RegisCashPayment.TotalSewa = Decimal.Round(totalSewa, 0);
                                    RegisCashPayment.TotalAll = Decimal.Round((sumAll - Hitung.Cashback), 0);
                                    if (Model.ConfigurationFileStatic.VFDPort != null && Model.ConfigurationFileStatic.VFDPort != "")
                                    {
                                        VFDPort.send("Total Belanja : ", f.ConvertToRupiah(RegisCashPayment.TotalAll), VFDPort.sp.PortName);
                                    }
                                }

                            }

                        }
                        else
                        {
                            f.ShowMessagebox("Keranjang Tiket Masih Kosong", "Information", MessageBoxButtons.OK);
                        }
                    }
                }
                else
                {
                    TxtBacaKartu.Text = "ACCOUNT DETAIL";
                    TxtBacaKartu.Text = TxtBacaKartu.Text + "\n================";
                    TxtBacaKartu.Text = TxtBacaKartu.Text + "\n Account Number : " + Card.IdCard;
                    TxtBacaKartu.Text = TxtBacaKartu.Text + "\n Code ID \t: -";
                    TxtBacaKartu.Text = TxtBacaKartu.Text + "\n Saldo Emoney \t: Rp " + string.Format("{0:n0}", Card.SaldoEmoney);
                    TxtBacaKartu.Text = TxtBacaKartu.Text + "\n Ticket WeekDay \t: " + string.Format("{0:n0}", Card.TicketWeekDay);
                    TxtBacaKartu.Text = TxtBacaKartu.Text + "\n Ticket WeekEnd \t: " + string.Format("{0:n0}", Card.TicketWeekEnd);
                    TxtBacaKartu.Text = TxtBacaKartu.Text + "\n SaldoJaminan \t: Rp " + string.Format("{0:n0}", Card.SaldoJaminan);
                }

            }
            else
            {
                f.ShowMessagebox("Baca Account Gelang Gagal", "Information", MessageBoxButtons.OK);
            }
        }
        private void button22_Click(object sender, EventArgs e)
        {
            PanelHitung.Visible = false;
            PanelEmoney.Visible = false;
            btnSelesai.Visible = false;
            CbUseEmoney.Checked = false;
            PanelControl(true);
        }
        private void button23_Click(object sender, EventArgs e)
        {
            if (TxtBacaKartu.Text != "")
            {
                CashPayment.JenisTransaksi = "Registrasi";
                if (CbUseEmoney.Checked == false)
                {
                    var pay = new PaymentMethod();
                    pay.JenisTransaksi = "Cash";
                    pay.TotalBayar = RegisCashPayment.TotalAll;
                    pay.PayEmoney = 0;
                    pay.PayCash = pay.TotalBayar;
                    RegisCashPayment.Payment = pay;
                    RegisCashPayment.Card.SaldoEmoneyAfter = RegisCashPayment.Card.SaldoEmoney;
                }
                else
                {
                    var pay = new PaymentMethod();
                    pay.JenisTransaksi = "eMoney & Cash";
                    pay.TotalBayar = RegisCashPayment.TotalAll;
                    if (RegisCashPayment.Card.SaldoEmoney >= RegisCashPayment.TotalAll)
                    {
                        pay.PayEmoney = RegisCashPayment.TotalAll;
                        pay.PayCash = 0;
                        RegisCashPayment.Card.SaldoEmoneyAfter = RegisCashPayment.Card.SaldoEmoney - RegisCashPayment.TotalAll;
                    }
                    else
                    {
                        decimal sisa = RegisCashPayment.TotalAll - RegisCashPayment.Card.SaldoEmoney;
                        pay.PayEmoney = RegisCashPayment.Card.SaldoEmoney;
                        pay.PayCash = sisa;
                        RegisCashPayment.Card.SaldoEmoneyAfter = 0;
                    }

                    RegisCashPayment.Payment = pay;
                }

                TunaiPayment f = new TunaiPayment();
                Form fc = Application.OpenForms["TunaiPayment"];
                if (fc != null)
                {
                    fc.Show();
                    fc.BringToFront();
                }
                else
                {
                    Page.TunaiPayment frm = new Page.TunaiPayment();
                    frm.StartPosition = FormStartPosition.CenterScreen;
                    frm.BringToFront();
                    frm.MaximizeBox = false;
                    frm.MinimizeBox = false;
                    frm.Show();
                }
            }
        }
        private void button21_Click(object sender, EventArgs e)
        {
            data.Clear();
            clearAll();
            Reset();
            if (Model.ConfigurationFileStatic.VFDPort != null && Model.ConfigurationFileStatic.VFDPort != "")
            {
                VFDPort.send("Selamat Datang", "di KUMPAY WATERPARK", Function.VFDPort.sp.PortName);
            }
            this.Hide();
            Form frm = Application.OpenForms["Main"];
            if (frm != null)
            {
                Panel tbx = frm.Controls.Find("PagePanel", true).FirstOrDefault() as Panel;
                UserControl fc = tbx.Controls.Find("Dashboard", true).FirstOrDefault() as UserControl;
                if (fc != null)
                {
                    fc.Show();
                    fc.BringToFront();
                }
                else
                {
                    var Page = new Page.Dashboard();
                    Page.Width = tbx.Width;
                    Page.Height = tbx.Height;
                    tbx.Controls.Add(Page);
                    Page.BringToFront();
                }
            }
        }
        private void button24_Click(object sender, EventArgs e)
        {
            if (TxtBacaKartu.Text != "")
            {
                DebitPayment.JenisTransaksi = "Registrasi";
                if (CbUseEmoney.Checked == false)
                {
                    var pay = new DebitPaymentMethod();
                    pay.JenisTransaksi = "EDC";
                    pay.TotalBayar = RegisCashPayment.TotalAll;
                    pay.PayEmoney = 0;                    
                    RegisDebitPayment.Payment = pay;
                    RegisDebitPayment.Asuransi = RegisCashPayment.Asuransi;
                    RegisDebitPayment.Card = RegisCashPayment.Card;
                    RegisDebitPayment.Cashback = RegisCashPayment.Cashback;
                    RegisDebitPayment.tiket = RegisCashPayment.tiket;
                    RegisDebitPayment.Sewa = RegisCashPayment.Sewa;
                    RegisDebitPayment.QtyTotalTiket = RegisCashPayment.QtyTotalTiket;
                    RegisDebitPayment.SaldoJaminan = RegisCashPayment.SaldoJaminan;
                    RegisDebitPayment.Topup = RegisCashPayment.Topup;
                    RegisDebitPayment.TotalBeliTiket = RegisCashPayment.TotalBeliTiket;
                    RegisDebitPayment.TotalSewa = RegisCashPayment.TotalSewa;
                    RegisDebitPayment.TotalAll = RegisCashPayment.TotalAll;

                }
                else
                {
                    var pay = new DebitPaymentMethod();
                    pay.JenisTransaksi = "eMoney & EDC";
                    pay.TotalBayar = RegisCashPayment.TotalAll;
                    if (RegisCashPayment.Card.SaldoEmoney >= RegisCashPayment.TotalAll)
                    {
                        pay.PayEmoney = RegisDebitPayment.TotalAll;
                        RegisDebitPayment.Card.SaldoEmoneyAfter = RegisCashPayment.Card.SaldoEmoney - RegisCashPayment.TotalAll;
                    }
                    else
                    {
                        decimal sisa = RegisCashPayment.TotalAll - RegisCashPayment.Card.SaldoEmoney;
                        pay.PayEmoney = RegisCashPayment.Card.SaldoEmoney;
                        pay.TotalBayar = sisa;
                        RegisDebitPayment.Card.SaldoEmoneyAfter = 0;
                    }

                    RegisDebitPayment.Payment = pay;


                }

                DebitCard f = new DebitCard();
                Form fc = Application.OpenForms["TunaiPayment"];
                if (fc != null)
                {
                    fc.Show();
                    fc.BringToFront();
                }
                else
                {
                    DebitCard frm = new DebitCard();
                    frm.StartPosition = FormStartPosition.CenterScreen;
                    frm.BringToFront();
                    frm.MaximizeBox = false;
                    frm.MinimizeBox = false;
                    frm.Show();
                }
            }
        }
        private void button18_Click(object sender, EventArgs e)
        {
            MasterDiskon f = new MasterDiskon();
            f.Show();
            f.BringToFront();
            f.StartPosition = FormStartPosition.CenterParent;
        }

        private void button14_Click(object sender, EventArgs e)
        {
            clearAll();
            var Card = ReadCardDataKey();
            if (Card.Success == true)
            {                
                if (Card.CodeId.Length == 14 )
                {
                    var Aktif = f.CheckExpired(Card.IdCard, Card.CodeId);
                    if (Aktif.Message == "Aktif")
                    {
                        f.UpdatAccountData(Card);
                        RegisCashPayment.Card = Card;
                        RegisDebitPayment.Card = Card;
                        TxtBacaKartu.Text = "ACCOUNT DETAIL";
                        TxtBacaKartu.Text = TxtBacaKartu.Text + "\n================";
                        TxtBacaKartu.Text = TxtBacaKartu.Text + "\n Account Number : " + Card.IdCard;
                        TxtBacaKartu.Text = TxtBacaKartu.Text + "\n Code ID \t: " + f.ConvertDecimal(Card.CodeId).ToString();
                        TxtBacaKartu.Text = TxtBacaKartu.Text + "\n Saldo Emoney \t: Rp " + string.Format("{0:n0}", Card.SaldoEmoney);
                        TxtBacaKartu.Text = TxtBacaKartu.Text + "\n Ticket WeekDay \t: " + string.Format("{0:n0}", Card.TicketWeekDay);
                        TxtBacaKartu.Text = TxtBacaKartu.Text + "\n Ticket WeekEnd \t: " + string.Format("{0:n0}", Card.TicketWeekEnd);
                        TxtBacaKartu.Text = TxtBacaKartu.Text + "\n SaldoJaminan \t: Rp " + string.Format("{0:n0}", Card.SaldoJaminan);
                        if (Card.SaldoEmoney > 0)
                        {
                            if (Model.ConfigurationFileStatic.VFDPort != null && Model.ConfigurationFileStatic.VFDPort != "")
                            {
                                Function.VFDPort.send("Saldo Kartu Anda :", f.ConvertToRupiah(Card.SaldoEmoney), Function.VFDPort.sp.PortName);
                            }
                        }
                    }
                    else
                    {
                        TxtBacaKartu.Text = "ACCOUNT DETAIL";
                        TxtBacaKartu.Text = TxtBacaKartu.Text + "\n================";
                        TxtBacaKartu.Text = TxtBacaKartu.Text + "\n Account Number : " + Card.IdCard;
                        TxtBacaKartu.Text = TxtBacaKartu.Text + "\n Code ID \t: " + f.ConvertDecimal(Card.CodeId).ToString();
                        TxtBacaKartu.Text = TxtBacaKartu.Text + "\n Account telah Expired";
                    }
                }
                else
                {
                    TxtBacaKartu.Text = "ACCOUNT DETAIL";
                    TxtBacaKartu.Text = TxtBacaKartu.Text + "\n================";
                    TxtBacaKartu.Text = TxtBacaKartu.Text + "\n Account Number : " + Card.IdCard;
                    TxtBacaKartu.Text = TxtBacaKartu.Text + "\n Code ID \t: "+ f.ConvertDecimal(Card.CodeId).ToString();
                    TxtBacaKartu.Text = TxtBacaKartu.Text + "\n Saldo Emoney \t: Rp " + string.Format("{0:n0}", Card.SaldoEmoney);
                    TxtBacaKartu.Text = TxtBacaKartu.Text + "\n Ticket WeekDay \t: " + string.Format("{0:n0}", Card.TicketWeekDay);
                    TxtBacaKartu.Text = TxtBacaKartu.Text + "\n Ticket WeekEnd \t: " + string.Format("{0:n0}", Card.TicketWeekEnd);
                    TxtBacaKartu.Text = TxtBacaKartu.Text + "\n SaldoJaminan \t: Rp " + string.Format("{0:n0}", Card.SaldoJaminan);
                }
            }
            else
            {
                TxtBacaKartu.Text = "Gagal membaca data kartu";
            }
        }
        private void button19_Click_1(object sender, EventArgs e)
        {
            registrasiModel.Promo = null;
        }
        private void button27_Click(object sender, EventArgs e)
        {
            Form fc = Application.OpenForms["MasterTicket"];
            if (fc != null)
            {
                fc.Show();
                fc.BringToFront();
            }
            else
            {
                Page.MasterTicket frm = new Page.MasterTicket();
                frm.StartPosition = FormStartPosition.CenterScreen;
                frm.BringToFront();
                frm.MaximizeBox = false;
                frm.MinimizeBox = false;
                frm.Show();
            }
        }
        private void dt_grid_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.ColumnIndex == 0)// created column index (delete button)
            {
                if (e.RowIndex >= 0)
                {
                    dt_grid.Rows.Remove(dt_grid.Rows[e.RowIndex]);
                    data.RemoveAt(e.RowIndex);
                }
            }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            Form fc = Application.OpenForms["MasterTopupSaatRegis"];
            if (fc != null)
            {
                fc.Show();
                fc.BringToFront();
            }
            else
            {
                Page.MasterTopupSaatRegis frm = new Page.MasterTopupSaatRegis();
                frm.StartPosition = FormStartPosition.CenterScreen;
                frm.BringToFront();
                frm.MaximizeBox = false;
                frm.MinimizeBox = false;
                frm.Show();
            }

        }
        private void button1_Click_1(object sender, EventArgs e)
        {
            Form fc = Application.OpenForms["MasterCashback"];
            if (fc != null)
            {
                fc.Show();
                fc.BringToFront();
            }
            else
            { 
                Page.MasterCashback frm = new Page.MasterCashback();
                frm.StartPosition = FormStartPosition.CenterScreen;
                frm.BringToFront();
                frm.MaximizeBox = false;
                frm.MinimizeBox = false;
                frm.Show();
            }

        }
        private void dt_grid_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = this.dt_grid.Rows[e.RowIndex];

                if (row.Cells.Count > 1)
                {
                    var d = new KeranjangTicket();
                    d.IdTicket = row.Cells["Id Ticket"].Value.ToString();
                    d.NamaTicket = row.Cells["Nama Ticket"].Value.ToString();
                    d.Harga = Convert.ToDecimal(row.Cells["Harga"].Value.ToString());
                    d.Qty = Convert.ToDecimal(row.Cells["Qtx"].Value.ToString());
                    d.Total = Convert.ToDecimal(row.Cells["Total"].Value.ToString());
                    d.IdDiskon = row.Cells["Id Diskon"].Value.ToString();
                    d.NamaDiskon = row.Cells["Nama Diskon"].Value.ToString();
                    d.Diskon = Convert.ToDecimal(row.Cells["Diskon"].Value.ToString());
                    d.TotalDiskon = Convert.ToDecimal(row.Cells["Total Diskon"].Value.ToString());
                    d.TotalAfterDiskon = Convert.ToDecimal(row.Cells["TotalAfterDiskon"].Value.ToString());
                    data.Add(d);
                    if (Model.ConfigurationFileStatic.VFDPort != null && Model.ConfigurationFileStatic.VFDPort != "")
                    {
                        VFDPort.send(data.Count + "." + d.NamaTicket + " " + d.Qty + " ", f.ConvertToRupiah(d.TotalAfterDiskon), VFDPort.sp.PortName);
                    }
                }
            }
        }
        private void button25_Click(object sender, EventArgs e)
        {
            PanelHitung.Visible = false;
            PanelReader.Enabled = true;
            panelCardType.Enabled = true;
            panelAsuransi.Enabled = true;
            PanelJaminan.Enabled = true;
            PanelAdditional.Enabled = true;
            panel2.Enabled = true;
            PanelEmoney.Visible = false;
            
        }
        private void CbUseEmoney_CheckedChanged(object sender, EventArgs e)
        {
            if (CbUseEmoney.Checked == true)
            {
                var pay = new PaymentMethod();
                if (RegisCashPayment.TotalAll > RegisCashPayment.Card.SaldoEmoney)
                {
                    decimal LastPay = RegisCashPayment.TotalAll - RegisCashPayment.Card.SaldoEmoney;
                    lblTotalBayar.Text = f.ConvertToRupiah(LastPay);
                    lblUseEmoney.Text = f.ConvertToRupiah(0);
                    btnSelesai.Visible = false;
                    PanelHitung.Enabled = true;
                    pay.JenisTransaksi = "eMoney & Cash";
                    pay.PayCash = LastPay;
                    pay.PayEmoney = RegisCashPayment.Card.SaldoEmoney;
                    RegisCashPayment.Card.SaldoEmoneyAfter = 0;
                }
                else
                {
                    decimal sisa = RegisCashPayment.Card.SaldoEmoney - RegisCashPayment.TotalAll;
                    lblUseEmoney.Text = f.ConvertToRupiah(sisa);
                    lblTotalBayar.Text = f.ConvertToRupiah(0);
                    btnSelesai.Visible = true;
                    PanelHitung.Enabled = false;
                    pay.PayCash = 0;
                    pay.PayEmoney = RegisCashPayment.Card.SaldoEmoney;
                    RegisCashPayment.Card.SaldoEmoneyAfter = sisa;
                }
                RegisCashPayment.Payment = pay;
            }
            else
            {
                if (RegisCashPayment.Card != null)
                {
                    lblTotalBayar.Text = f.ConvertToRupiah(RegisCashPayment.TotalAll);
                    lblUseEmoney.Text = f.ConvertToRupiah(RegisCashPayment.Card.SaldoEmoney);
                }
                else
                {
                    lblTotalBayar.Text = "Rp 0";
                    lblUseEmoney.Text = "Rp 0";
                }
                btnSelesai.Visible = false;
                PanelHitung.Enabled = true;

            }
        }
        private void btnSelesai_Click(object sender, EventArgs e)
        {
            var dlg = MessageBox.Show("Apakah Anda yakin untuk menggunakan eMoney sebesar : Rp " + string.Format("{0:n0}", RegisCashPayment.TotalAll), "Information", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
            if (dlg == DialogResult.OK)
            {
                var card = ReadCardDataKey();
                if (card.Success == true)
                {
                    //f.UpdatAccountData(card);

                    if (RegisCashPayment.TotalAll <= card.SaldoEmoney)
                    {
                        var pay = new PaymentMethod();
                        pay.JenisTransaksi = "eMoney";
                        pay.TotalBayar = RegisCashPayment.TotalAll;
                        pay.PayEmoney = RegisCashPayment.TotalAll;
                        if (pay.PayEmoney > 0 && pay.PayCash == 0)
                        {
                            RegisCashPayment.Card.SaldoEmoneyAfter = card.SaldoEmoney - RegisCashPayment.TotalAll;
                            RegisCashPayment.Card.SaldoEmoney = card.SaldoEmoney;
                            RegisCashPayment.Payment = pay;
                            ulangLoadKey:
                            var loadKey = LoaAuthoKey();
                            if (loadKey.Success == true)
                            {
                                if (RegisCashPayment.Card.CodeId == "0")
                                {
                                    string CodeIDAfter = f.GenCodeID();
                                    RegisCashPayment.Card.CodeIdAfter = CodeIDAfter;
                                }
                                else
                                {
                                    RegisCashPayment.Card.CodeIdAfter = f.ConvertDecimal(RegisCashPayment.Card.CodeId).ToString();
                                }

                                var SaldoEmoneyAfter = UpdateBlok("04", "04", RegisCashPayment.Card.SaldoEmoneyAfter.ToString());
                                var TicketWeekDayAfter = UpdateBlok("05", "04", RegisCashPayment.Card.TicketWeekDayAfter.ToString());
                                var TicketWeekEndAfter = UpdateBlok("06", "04", RegisCashPayment.Card.TicketWeekEndAfter.ToString());
                                var SaldoJaminanAfter = UpdateBlok("08", "08", RegisCashPayment.Card.SaldoJaminanAfter.ToString());
                                var CodeId = UpdateBlok("09", "08", f.ConvertDecimal(RegisCashPayment.Card.CodeIdAfter).ToString());

                                if (SaldoEmoneyAfter.Success == true &&
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

                                    string IdTicket = f.GetIdTiket();
                                    string IdItemSewa = f.GetIdTrx();
                                    string Chasier = f.GetNamaUser(General.IDUser);
                                    string ComputerName = f.GetComputerName();
                                    foreach (var Tiket in RegisCashPayment.tiket)
                                    {
                                        var savetiket = f.SaveTicket(Tiket, DataSave.Card.IdCard +"-"+f.ConvertDecimal(DataSave.Card.CodeId).ToString(), IdTicket,Chasier,ComputerName);
                                    }

                                    var save = f.SaveTransaksiRegistrasi(DataSave, IdTicket, ComputerName, Chasier);
                                    var dtTRX = save.Message.Split('~');
                                    var UpdateAccount = ReadCardDataKey();
                                    f.UpdatAccountData(UpdateAccount);

                                    if (RegisCashPayment.Sewa.Count() > 0)
                                    {
                                        foreach (var s in RegisCashPayment.Sewa)
                                        {
                                            var saveSewa = f.SaveItemsFB(s, DataSave.Card.IdCard, IdItemSewa, Chasier, ComputerName);
                                        }

                                        var SavePOS = new SaveFoodCourtPayment();
                                        SavePOS.Card = DataSave.Card;
                                        SavePOS.Keranjang = DataSave.Sewa;
                                        var PayPos = new PaymentMethod();
                                        PayPos.JenisTransaksi = DataSave.Payment.JenisTransaksi;
                                        PayPos.PayCash = DataSave.TotalSewa;
                                        PayPos.Kembalian = 0;
                                        PayPos.PayEmoney = DataSave.TotalSewa;
                                        PayPos.TerimaUang = 0;
                                        PayPos.TotalBayar = DataSave.TotalSewa;
                                        SavePOS.Pay = PayPos;
                                        var ResSavePOS = f.SaveFoodCourtPayment(SavePOS, IdItemSewa, Chasier, ComputerName);
                                        Printulang:
                                        var print = PrintRegis(SavePOS,dtTRX[1].Trim());
                                        if (print.Success == true)
                                        {
                                            TxtBacaKartu.Text = "";
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
                                        PrintUlang:
                                        var print = PrintRegis(null,dtTRX[1].Trim());
                                        if (print.Success == true)
                                        {
                                            clearAll();
                                            Reset();
                                            Print f = new Print();
                                            f.Show();
                                            f.BringToFront();

                                            f.StartPosition = FormStartPosition.CenterScreen;
                                            f.txtMessageBox.Text = "Registrasi Berhasil transaksi sebesar " + string.Format("{0:n0}", RegisCashPayment.TotalAll);
                                        }
                                        else
                                        {
                                            var res = MessageBox.Show("Error : Print Gagal", "Printing Fail", MessageBoxButtons.RetryCancel, MessageBoxIcon.Warning);
                                            if (res == DialogResult.Retry)
                                            {
                                                goto PrintUlang;
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    var res = MessageBox.Show("Error : Cannot Update Data Card", "Reader Fail", MessageBoxButtons.RetryCancel, MessageBoxIcon.Warning);
                                    if (res == DialogResult.Retry)
                                    {
                                        goto ulangLoadKey;
                                    }
                                }
                            }
                            else
                            {
                                var res = MessageBox.Show("Error : Cannot Akses to Key Card", "Reader Fail", MessageBoxButtons.RetryCancel, MessageBoxIcon.Warning);
                                if (res == DialogResult.Retry)
                                {
                                    goto ulangLoadKey;
                                }
                            }
                        }
                        
                    }
                }
            }
            else
            {
                return;
            }
        }
        #endregion

        #region Function
        private void GetAll()
        {
            TxtJaminan.Text = f.ConvertToRupiah(f.GetJaminan());
            HargaAsuransi.Text = f.ConvertToRupiah(f.GetAsuransi());
            atur_grid();
            cbJaminan.Checked = true;
            cbAsuransi.Checked = true;
            PanelControl(true);
            PanelHitung.Visible = false;
        }
        public void atur_grid()
        {
            dt_grid.Rows.Clear();
            dt_grid.ColumnCount = 11;
            dt_grid.Columns[0].Name = "X";
            dt_grid.Columns[1].Name = "Id Ticket";
            dt_grid.Columns[2].Name = "Nama Ticket";
            dt_grid.Columns[3].Name = "Harga";
            dt_grid.Columns[4].Name = "Qtx";
            dt_grid.Columns[5].Name = "Total";
            dt_grid.Columns[6].Name = "Id Diskon";
            dt_grid.Columns[7].Name = "Nama Diskon";
            dt_grid.Columns[8].Name = "Diskon";
            dt_grid.Columns[9].Name = "Total Diskon";
            dt_grid.Columns[10].Name = "TotalAfterDiskon";

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
            DataGridViewColumn column10 = dt_grid.Columns[9];
            DataGridViewColumn column11 = dt_grid.Columns[10];

            // Initialize basic DataGridView properties.
            dt_grid.Dock = DockStyle.None;
            
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
            dt_grid.RowsDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dt_grid.DefaultCellStyle.Font = new Font("Tahoma", 8);
            dt_grid.Columns[0].Width = 10;
            column2.Width = 0;
        }
        public void PanelControl(bool enable)
        {
            panelCardType.Enabled = enable;
            panelAsuransi.Enabled = enable;
            PanelAdditional.Enabled = enable;
            PanelReader.Enabled = enable;
            PanelJaminan.Enabled = enable;
            panel2.Enabled = enable;
        }
        public void Reset()
        {
            TxtBacaKartu.Text = "";
            cbAsuransi.Checked = true;
            CbUseEmoney.Checked = false;
            PanelHitung.Visible = false;
            PanelControl(true);
        }
        public void clearAll()
        {
            RegisCashPayment.Card = null;
            RegisCashPayment.Payment = null;
            RegisCashPayment.Asuransi = 0;
            RegisCashPayment.Cashback = 0;
            RegisCashPayment.QtyTotalTiket = 0;
            RegisCashPayment.SaldoJaminan = 0;
            RegisCashPayment.Topup = 0;
            RegisCashPayment.TotalAll = 0;
            RegisCashPayment.TotalBeliTiket = 0;
            var List = new List<KeranjangTicket>();
            var ListS = new List<KeranjangFoodcourt>();
            RegisCashPayment.tiket= List;
            RegisCashPayment.Sewa = ListS;
            data.Clear();
            dataSewa.Clear();
            TxtBacaKartu.Text = "";
            PanelHitung.Visible = false;
            PanelEmoney.Visible = false;
            CbUseEmoney.Checked = false;            
            lblTotalBayar.Text = "";
            dt_grid.Rows.Clear();
            dt_grid2.Rows.Clear();
            btnCashback.Text = "Give Cashback";
            btnTopup.Text = "Isi Saldo Emoney";
            txtTotalBayarSewa.Text = "Total : Rp 0";
        }
        #endregion

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

        private void button1_Click(object sender, EventArgs e)
        {
            Form fc = Application.OpenForms["Persewaan"];
            if (fc != null)
            {
                fc.Show();
                fc.BringToFront();
            }
            else
            {
                Persewaan frm = new Persewaan();
                frm.StartPosition = FormStartPosition.CenterScreen;
                frm.BringToFront();
                frm.MaximizeBox = false;
                frm.MinimizeBox = false;
                frm.Show();
            }
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            Form fc = Application.OpenForms["Persewaan"];
            if (fc != null)
            {
                fc.Show();
                fc.BringToFront();
            }
            else
            {
                Persewaan frm = new Persewaan();
                frm.StartPosition = FormStartPosition.CenterScreen;
                frm.BringToFront();
                frm.MaximizeBox = false;
                frm.MinimizeBox = false;
                frm.Show();
            }
        }

        public void atur_grid3()
        {
            dt_grid2.Rows.Clear();

            dt_grid2.ColumnCount = 6;
            dt_grid2.Columns[0].Name = "X";
            dt_grid2.Columns[1].Name = "Id Trx";
            dt_grid2.Columns[2].Name = "Nama Item";
            dt_grid2.Columns[3].Name = "Harga";
            dt_grid2.Columns[4].Name = "Qtx";
            dt_grid2.Columns[5].Name = "Total Harga";

            dt_grid2.RowHeadersVisible = false;
            dt_grid2.ColumnHeadersVisible = true;

            DataGridViewColumn column1 = dt_grid2.Columns[0];
            DataGridViewColumn column2 = dt_grid2.Columns[1];
            DataGridViewColumn column3 = dt_grid2.Columns[2];
            DataGridViewColumn column4 = dt_grid2.Columns[3];
            DataGridViewColumn column5 = dt_grid2.Columns[4];
            DataGridViewColumn column6 = dt_grid2.Columns[5];

            dt_grid2.Dock = DockStyle.None;
            dt_grid2.BorderStyle = BorderStyle.None;
            dt_grid2.AllowUserToAddRows = false;
            dt_grid2.AllowUserToDeleteRows = false;
            dt_grid2.AllowUserToOrderColumns = true;
            dt_grid2.ReadOnly = true;
            dt_grid2.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dt_grid2.MultiSelect = true;
            dt_grid2.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dt_grid2.AllowUserToResizeColumns = true;
            dt_grid2.ColumnHeadersHeightSizeMode =
                DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dt_grid2.AllowUserToResizeRows = false;
            dt_grid2.DefaultCellStyle.SelectionBackColor = Color.LightBlue;
            dt_grid2.DefaultCellStyle.SelectionForeColor = Color.Black;
            dt_grid2.RowHeadersDefaultCellStyle.SelectionBackColor = Color.Empty;
            dt_grid2.RowsDefaultCellStyle.BackColor = Color.White;
            dt_grid2.AlternatingRowsDefaultCellStyle.BackColor = Color.White;
            dt_grid2.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dt_grid2.ColumnHeadersDefaultCellStyle.BackColor = Color.Black;
            dt_grid2.RowHeadersDefaultCellStyle.BackColor = Color.Black;
            dt_grid2.RowsDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dt_grid2.DefaultCellStyle.Font = new Font("Tahoma", 10);
            dt_grid2.Columns[0].Width = 10;
            dt_grid2.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.AutoSizeToDisplayedHeaders;
        }

        private void dt_grid2_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = this.dt_grid2.Rows[e.RowIndex];
                if (row.Cells.Count > 1)
                {
                    var d = new KeranjangFoodcourt();
                    d.IdTrx = row.Cells["Id Trx"].Value.ToString();
                    d.NamaItem = row.Cells["Nama Item"].Value.ToString();
                    d.Harga = f.ConvertDecimal(row.Cells["Harga"].Value.ToString());
                    d.Qtx = f.ConvertDecimal(row.Cells["Qtx"].Value.ToString());
                    dataSewa.Add(d);
                    decimal total = 0;
                    foreach (var tt in dataSewa)
                    {
                        total = total + (tt.Harga * tt.Qtx);
                    }
                    txtTotalBayarSewa.Text = "Total : "+f.ConvertToRupiah(total);
                }
            }
        }

        private void dt_grid2_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.ColumnIndex == 0)// created column index (delete button)
            {
                if (e.RowIndex >= 0)
                {
                    dt_grid2.Rows.Remove(dt_grid2.Rows[e.RowIndex]);
                    dataSewa.RemoveAt(e.RowIndex);
                    decimal total = 0;
                    foreach (var tt in dataSewa)
                    {
                        total = total + (tt.Harga * tt.Qtx);
                    }
                    txtTotalBayarSewa.Text = "Total : " + f.ConvertToRupiah(total);
                }
            }
        }

        private void button1_Click_2(object sender, EventArgs e)
        {
            var data = MessageBox.Show("Apakah anda yakin untuk menghapus data gelang? ini akan mengakibatkan hilangnya semua uang eMoney user yang ada pada gelang, harap berhati-hati", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (data == DialogResult.Yes)
            {
                ulangBaca:
                var card = ReadCardDataKey();
                if (card.Success == true)
                {
                    card.SaldoEmoneyAfter = 0;
                    card.SaldoJaminanAfter = 0;
                    card.TicketWeekDayAfter = 0;
                    card.TicketWeekEndAfter = 0;
                    card.CodeIdAfter = "0";
                    var SaldoEmoneyAfter = UpdateBlok("04", "04", card.SaldoEmoneyAfter.ToString());
                    var SaldoJaminanAfter = UpdateBlok("05", "04", card.SaldoJaminanAfter.ToString());
                    var TicketWeekDayAfter = UpdateBlok("06", "04", card.TicketWeekDayAfter.ToString());
                    var TicketWeekEndAfter = UpdateBlok("08", "08", card.TicketWeekEndAfter.ToString());
                    var CodeId = UpdateBlok("09", "08", f.ConvertDecimal(card.CodeIdAfter).ToString());
                    if (SaldoEmoneyAfter.Success == true &&
                        SaldoJaminanAfter.Success == true &&
                        TicketWeekDayAfter.Success == true &&
                        TicketWeekEndAfter.Success == true &&
                        CodeId.Success == true)
                    {
                        clearAll();
                        var Card = ReadCardDataKey();
                        if (Card.Success == true)
                        {
                            RegisCashPayment.Card = Card;
                            TxtBacaKartu.Text = "ACCOUNT DETAIL";
                            TxtBacaKartu.Text = TxtBacaKartu.Text + "\n================";
                            TxtBacaKartu.Text = TxtBacaKartu.Text + "\n Account Number : " + Card.IdCard;
                            TxtBacaKartu.Text = TxtBacaKartu.Text + "\n Code ID : " + f.ConvertDecimal(Card.CodeId).ToString();
                            TxtBacaKartu.Text = TxtBacaKartu.Text + "\n Saldo Emoney \t: Rp " + string.Format("{0:n0}", Card.SaldoEmoney);
                            TxtBacaKartu.Text = TxtBacaKartu.Text + "\n Ticket WeekDay \t: " + string.Format("{0:n0}", Card.TicketWeekDay);
                            TxtBacaKartu.Text = TxtBacaKartu.Text + "\n Ticket WeekEnd \t: " + string.Format("{0:n0}", Card.TicketWeekEnd);
                            TxtBacaKartu.Text = TxtBacaKartu.Text + "\n SaldoJaminan \t: Rp " + string.Format("{0:n0}", Card.SaldoJaminan);
                        }
                        else
                        {
                            TxtBacaKartu.Text = "Gagal membaca data kartu";
                        }
                    }
                }
                else
                {
                    var res = MessageBox.Show("Read Data Kartu Gagal", "Read data Fail", MessageBoxButtons.RetryCancel, MessageBoxIcon.Warning);
                    if (res == DialogResult.Retry)
                    {
                        goto ulangBaca;
                    }
                }
            }
        }

        private void lblTotalBayar_TextChanged(object sender, EventArgs e)
        {
            if (f.ConvertDecimal(lblTotalBayar.Text) > 0)
            {
                if (Model.ConfigurationFileStatic.VFDPort != null && Model.ConfigurationFileStatic.VFDPort != "")
                {
                    Function.VFDPort.send("Total Pembayaran :", f.ConvertToRupiah(f.ConvertDecimal(lblTotalBayar.Text)), Function.VFDPort.sp.PortName);
                }
            }    
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

            //TxtBacaKartu.AppendText(PrintText);
            //TxtBacaKartu.AppendText("\n");
            //TxtBacaKartu.SelectionColor = Color.Black;
            //TxtBacaKartu.Focus();

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
                            if (SaldoEmoney.Success == true && tiketWeekDay.Success == true && 
                                tiketWeekEnd.Success == true && JaminanSaldo.Success == true &&
                                CodeId.Success == true)
                            {
                                res.SaldoEmoney = f.ConvertDecimal(SaldoEmoney.Message);
                                res.TicketWeekDay = f.ConvertDecimal(tiketWeekDay.Message);
                                res.TicketWeekEnd = f.ConvertDecimal(tiketWeekEnd.Message);
                                res.SaldoJaminan = f.ConvertDecimal(JaminanSaldo.Message);
                                res.CodeId = f.ConvertDecimal(CodeId.Message).ToString();
                                res.Success = true;
                                res.Message = "Reading Card Success";
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

        #region Print
        public ReturnResult PrintRegis(SaveFoodCourtPayment Data,string Datetime)
        {
            var res = new ReturnResult();
            try
            {
                string s = "Dateime \t: " + Datetime + Environment.NewLine;
                s += "ID Transaction \t: TRX" + Datetime.Replace("/", "").Replace(":", "").Replace(" ", "") + Environment.NewLine;
                s += "Merchant ID \t: " + f.GetComputerName() + Environment.NewLine;
                s += "Nama Petugas \t: " + f.GetNamaUser(General.IDUser) + Environment.NewLine;
                s += "------------------------------------------------------------------------------------" + Environment.NewLine;
                s += "Transaksi Registrasi " + Environment.NewLine;
                foreach (var ticket in RegisCashPayment.tiket)
                {
                    s += "Nama Tiket \t: " + ticket.NamaTicket + Environment.NewLine;
                    s += "Harga Satuan \t: Rp " + string.Format("{0:n0}", ticket.Harga) + Environment.NewLine;
                    s += "Qty \t\t: " + ticket.Qty + Environment.NewLine;
                    s += "Total \t\t: Rp " + string.Format("{0:n0}", ticket.Total) + Environment.NewLine;
                    s += "Nama Diskon \t: " + ticket.NamaDiskon + Environment.NewLine;
                    s += "Diskon \t\t: " + ticket.Diskon + "%" + Environment.NewLine;
                    s += "Total Diskon \t: Rp " + string.Format("{0:n0}", ticket.TotalDiskon) + Environment.NewLine;
                    s += "Total - Diskon \t: Rp " + string.Format("{0:n0}", ticket.TotalAfterDiskon) + Environment.NewLine;
                }
                s += "------------------------------------------------------------------------------------" + Environment.NewLine;
                s += "Transaksi Sewa " + Environment.NewLine ;

                if (Data != null)
                {
                    s += "------------------------------------------------------------------------------------" + Environment.NewLine;
                    s += "Transaksi Persewaan " + Environment.NewLine ;

                    decimal d = 0;
                    foreach (var Items in Data.Keranjang)
                    {
                        d++;
                        s += d + ". " + Items.NamaItem + " - " + Items.Qtx + "\t : " + f.ConvertToRupiah((Items.Harga * Items.Qtx)) + Environment.NewLine;
                    }
                    s += "------------------------------------------------------------------------------------" + Environment.NewLine;
                    s += "Total Belanja \t\t: " + f.ConvertToRupiah(Data.Pay.TotalBayar) + Environment.NewLine;
                }

                //foreach (var Sewa in RegisCashPayment.Sewa)
                //{
                //    s += "Nama Item \t: " + Sewa.NamaItem + Environment.NewLine;
                //    s += "Harga Satuan \t: " + f.ConvertToRupiah(Sewa.Harga) + Environment.NewLine;
                //    s += "Qty \t\t: " + Sewa.Qtx + Environment.NewLine;
                //    s += "Total \t\t: " + f.ConvertToRupiah(Sewa.Qtx*Sewa.Harga) + Environment.NewLine +Environment.NewLine;
                //}

                //s += "------------------------------------------------------------" + Environment.NewLine;
                //s += "Total Sewa \t: Rp " + f.ConvertToRupiah(RegisCashPayment.TotalSewa) + Environment.NewLine;
                s += "------------------------------------------------------------------------------------" + Environment.NewLine;
                if (RegisCashPayment.Asuransi > 0)
                {
                    s += "Asuransi " + RegisCashPayment.QtyTotalTiket + " Org \t: Rp " + string.Format("{0:n0}", RegisCashPayment.Asuransi) + Environment.NewLine;
                }

                if (RegisCashPayment.Card.SaldoJaminan == 0)
                {
                    if (RegisCashPayment.Card.SaldoJaminanAfter > 0)
                    {
                        s += "Saldo Jaminan \t: Rp " + string.Format("{0:n0}", (RegisCashPayment.Card.SaldoJaminanAfter)) + Environment.NewLine;
                    }
                }

                if (RegisCashPayment.Cashback > 0)
                {
                    s += "------------------------------------------------------------------------------------" + Environment.NewLine;
                    s += "Cashback \t: - Rp " + string.Format("{0:n0}", (RegisCashPayment.Cashback)) + Environment.NewLine;
                }

                if (RegisCashPayment.Topup > 0)
                {
                    s += "------------------------------------------------------------------------------------" + Environment.NewLine;
                    s += "Topup Emoney \t: Rp " + string.Format("{0:n0}", (RegisCashPayment.Topup)) + Environment.NewLine;
                }

                s += "------------------------------------------------------------------------------------" + Environment.NewLine;
                s += "Total \t\t: Rp " + string.Format("{0:n0}", (RegisCashPayment.TotalAll)) + Environment.NewLine;

                if (RegisCashPayment.Payment != null)
                {
                    s += "------------------------------------------------------------------------------------" + Environment.NewLine;
                    s += "Payment \t: " + RegisCashPayment.Payment.JenisTransaksi + Environment.NewLine;
                    if (RegisCashPayment.Payment.PayEmoney > 0)
                    {
                        s += "Use eMoney \t: Rp " + string.Format("{0:n0}", (RegisCashPayment.Payment.PayEmoney)) + Environment.NewLine;
                    }
                    if (RegisCashPayment.Payment.PayCash > 0)
                    {
                        s += "Total Cash \t: Rp " + string.Format("{0:n0}", (RegisCashPayment.Payment.PayCash)) + Environment.NewLine;
                    }

                    if (RegisCashPayment.Payment.TerimaUang > 0)
                    {
                        s += "Uang dibayarkan \t: Rp " + string.Format("{0:n0}", (RegisCashPayment.Payment.TerimaUang)) + Environment.NewLine;
                        s += "Uang Kembalian \t: Rp " + string.Format("{0:n0}", (RegisCashPayment.Payment.Kembalian)) + Environment.NewLine;
                    }
                    if (RegisCashPayment.Payment.PayEmoney > 0)
                    {
                        s += "Account Number \t: " + RegisCashPayment.Card.IdCard+"-"+RegisCashPayment.Card.CodeId + Environment.NewLine;
                        s += "Prev Balance \t: " + f.ConvertToRupiah(RegisCashPayment.Card.SaldoEmoney) + Environment.NewLine;
                        s += "Current Balance \t: " + f.ConvertToRupiah(RegisCashPayment.Card.SaldoEmoneyAfter) + Environment.NewLine;
                        s += "Saldo Jaminan \t: " + f.ConvertToRupiah(RegisCashPayment.Card.SaldoJaminanAfter) + Environment.NewLine;
                    }
                }
                s += "------------------------------------------------------------------------------------" + Environment.NewLine;
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
                res.Success = true;
                res.Message = "Exception Occured While Printing " + ex.Message;
            }
            return res;
        }
        #endregion

    }
}

