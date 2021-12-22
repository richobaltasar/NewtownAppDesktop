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
    public partial class DebitCard : Form
    {
        GlobalFunc f = new GlobalFunc();

        public int retCode, hContext, hCard, Protocol;
        public bool connActive = false;
        public bool autoDet;

        public byte[] SendBuff = new byte[263];
        public byte[] RecvBuff = new byte[263];

        public int SendLen, RecvLen, nBytesRet, reqType, Aprotocol, dwProtocol, cbPciLength;

        string readername = "ACS ACR122 0";
        public card_function.Card.SCARD_READERSTATE RdrState;
        public card_function.Card.SCARD_IO_REQUEST pioSendRequest;

        public DebitCard()
        {
            InitializeComponent();
        }

        private void DebitCard_Load(object sender, EventArgs e)
        {
            if (DebitPayment.JenisTransaksi != null)
            {
                if (DebitPayment.JenisTransaksi == "TOPUP")
                {
                    txtTotalBelanja.Text = f.ConvertToRupiah(TopupDebitPayment.NominalTopup);
                }
                else if (DebitPayment.JenisTransaksi == "FoodCourt")
                {
                    txtTotalBelanja.Text = "" + f.ConvertToRupiah(FoodCourtPayment.Pay.TotalBayar);
                }
                else if (DebitPayment.JenisTransaksi == "Registrasi")
                {
                    txtTotalBelanja.Text = f.ConvertToRupiah(RegisDebitPayment.Payment.TotalBayar);
                }
                txtControl.Text = txtATMNumber.Name;
                txtControl.Visible = false;
                txtATMNumber.Focus();
            }
            else
            {
                this.Close();
            }
        }

        private void button13_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtATMNumber_Click(object sender, EventArgs e)
        {
            txtControl.Text = txtATMNumber.Name;
        }

        private void txtNoReff_Click(object sender, EventArgs e)
        {
            txtControl.Text = txtNoReff.Name;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            input_keyPad((sender as Button).Text, txtControl.Text);
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
                        string data = txt.Text.Replace(".", "").Replace(",", "").Replace("-","");
                        if (data != "")
                        {
                            decimal t = Convert.ToDecimal(data);
                            //txt.Text = string.Format("{0:n0}", t);
                            const int separateOnLength = 4;
                            txt.Text = new string(
                                t.ToString().Select((x, i) => i > 0 && i % separateOnLength == 0 ? new[] { '-', x } : new[] { x })
                                    .SelectMany(x => x)
                                    .ToArray()
                                );
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
                    string data = (txt.Text + key).Replace(".", "").Replace(",", "").Replace("-", "");
                    if (data != "")
                    {
                        decimal t = Convert.ToDecimal(data);
                        //txt.Text = string.Format("{0:n0}", t);
                        const int separateOnLength = 4;
                        txt.Text = new string(
                            t.ToString().Select((x, i) => i > 0 && i % separateOnLength == 0 ? new[] { '-', x } : new[] { x })
                                .SelectMany(x => x)
                                .ToArray()
                            );
                    }
                    else
                    {
                        txt.Text = txt.Text + key;
                    }
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            input_keyPad((sender as Button).Text, txtControl.Text);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            input_keyPad((sender as Button).Text, txtControl.Text);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            input_keyPad((sender as Button).Text, txtControl.Text);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            input_keyPad((sender as Button).Text, txtControl.Text);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            input_keyPad((sender as Button).Text, txtControl.Text);
        }

        private void button12_Click(object sender, EventArgs e)
        {
            input_keyPad((sender as Button).Text, txtControl.Text);
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

        private void button17_Click(object sender, EventArgs e)
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

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            Form fc = Application.OpenForms["MasterBank"];
            if (fc != null)
            {
                fc.Show();
                fc.BringToFront();
                Label LblPage = fc.Controls.Find("LblPage", true).FirstOrDefault() as Label;
                if (LblPage != null)
                {
                    LblPage.Text = this.Name;
                }
            }
            else
            {
                MasterBank frm = new MasterBank();
                frm.StartPosition = FormStartPosition.CenterScreen;
                frm.BringToFront();
                frm.MaximizeBox = false;
                frm.MinimizeBox = false;
                Label LblPage = frm.Controls.Find("LblPage", true).FirstOrDefault() as Label;
                if (LblPage != null)
                {
                    LblPage.Text = this.Name;
                }
                frm.Show();
            }
        }

        private void button20_Click(object sender, EventArgs e)
        {
            if (txtBankCode.Text != ""
                && txtATMNumber.Text != ""
                && txtNoReff.Text != "")
            {
                if (DebitPayment.JenisTransaksi == "TOPUP")
                {
                    var pay = TopupDebitPayment.Pay;
                    if (pay != null)
                    {
                        if (txtBankCode.Text != "")
                        {
                            pay.KodeBank = txtBankCode.Text;
                        }
                        if (txtNamaBank.Text != "")
                        {
                            pay.NamaBank = txtNamaBank.Text;
                        }
                        if (txtDiskon.Text != "")
                        {
                            pay.DiskonBank = f.ConvertDecimal(txtDiskon.Text);
                        }
                        if (TxtNominalDiskon.Text != "")
                        {
                            pay.NominalDiskonBank = f.ConvertDecimal(TxtNominalDiskon.Text);
                        }
                        if (txtAdminCharges.Text != "")
                        {
                            pay.AdminCharges = f.ConvertDecimal(txtAdminCharges.Text);
                        }
                        if (txtTotalDebit.Text != "")
                        {
                            pay.DebitNominal = f.ConvertDecimal(txtTotalDebit.Text);
                        }
                        if (txtATMNumber.Text != null)
                        {
                            pay.NoATM = txtATMNumber.Text;
                        }
                        if (txtNoReff.Text != null)
                        {
                            pay.NoReff = txtNoReff.Text;
                        }
                        UlangReadCardDataKey:
                        var card = ReadCardDataKey();
                        if (card.Success == true)
                        {
                            f.UpdatAccountData(card);
                            card.SaldoEmoneyAfter = card.SaldoEmoney + TopupDebitPayment.NominalTopup;
                            var Topup = UpdateBlok("04", "04", card.SaldoEmoneyAfter.ToString());
                            if (Topup.Success == true)
                            {
                                var SaveData = new SaveDebitTopupTrx();
                                SaveData.Card = card;
                                SaveData.Pay = TopupDebitPayment.Pay;
                                SaveData.NominalTopup = TopupDebitPayment.NominalTopup;
                                SaveData.ComputerName = f.GetComputerName();
                                SaveData.NamaUser = f.GetNamaUser(General.IDUser);

                            UlangSaveTransaksiDebitTopup:
                                var save = f.SaveTransaksiDebitTopup(SaveData);
                                if (save.Success == true && save.Message.Contains('~') == true)
                                {
                                    var dtTrx = save.Message.Split('~');
                                    var UpdateAccount = ReadCardDataKey();
                                    f.UpdatAccountData(UpdateAccount);
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
                                        fp.txtMessageBox.Text = "Topup Berhasil sebesar " + f.ConvertToRupiah(TopupDebitPayment.NominalTopup);
                                        TopupDebitPayment.Card = null;
                                        TopupDebitPayment.NominalTopup = 0;
                                        TopupDebitPayment.Pay = null;
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
                                    var res = MessageBox.Show("Save Transaksi Debit Topup Gagal", "Update data Fail", MessageBoxButtons.RetryCancel, MessageBoxIcon.Warning);
                                    if (res == DialogResult.Retry)
                                    {
                                        goto UlangSaveTransaksiDebitTopup;
                                    }
                                }
                            }
                            else
                            {
                                var res = MessageBox.Show("Topup Kartu Gagal", "Update data Fail", MessageBoxButtons.RetryCancel, MessageBoxIcon.Warning);
                                if (res == DialogResult.Retry)
                                {
                                    goto UlangReadCardDataKey;
                                }
                            }
                        }
                        else
                        {
                            var res = MessageBox.Show("Baca Kartu Gagal,Silahkan tempelkan Kartu lalu tekan 'Retry' ", "Baca data Fail", MessageBoxButtons.RetryCancel, MessageBoxIcon.Warning);
                            if (res == DialogResult.Retry)
                            {
                                goto UlangReadCardDataKey;
                            }
                        }
                    }
                    else
                    {
                        this.Close();
                    }
                }
                else if (DebitPayment.JenisTransaksi == "Registrasi")
                {
                    var kartu = RegisDebitPayment.Card;
                    var pay = RegisDebitPayment.Payment;
                    if (pay != null)
                    {
                        if (txtBankCode.Text != "")
                        {
                            pay.KodeBank = txtBankCode.Text;
                        }
                        if (txtNamaBank.Text != "")
                        {
                            pay.NamaBank = txtNamaBank.Text;
                        }
                        if (txtDiskon.Text != "")
                        {
                            pay.DiskonBank = f.ConvertDecimal(txtDiskon.Text);
                        }
                        if (TxtNominalDiskon.Text != "")
                        {
                            pay.NominalDiskonBank = f.ConvertDecimal(TxtNominalDiskon.Text);
                        }
                        if (txtAdminCharges.Text != "")
                        {
                            pay.AdminCharges = f.ConvertDecimal(txtAdminCharges.Text);
                        }
                        if (txtTotalDebit.Text != "")
                        {
                            pay.DebitNominal = f.ConvertDecimal(txtTotalDebit.Text);
                        }
                        if (txtATMNumber.Text != null)
                        {
                            pay.NoATM = txtATMNumber.Text;
                        }
                        if (txtNoReff.Text != null)
                        {
                            pay.NoReff = txtNoReff.Text;
                        }

                        UlangReadCardDataKey:
                        var card = ReadCardDataKey();
                        if (card.Success == true)
                        {

                            //f.UpdatAccountData(card);
                            if (RegisDebitPayment.Topup > 0)
                            {
                                RegisDebitPayment.Card.SaldoEmoneyAfter = RegisDebitPayment.Card.SaldoEmoney + RegisDebitPayment.Topup;
                            }
                            if (card != null)
                            {
                                var loadKey = LoaAuthoKey();
                                if (loadKey.Success == true)
                                {
                                    if (f.ConvertDecimal(RegisDebitPayment.Card.CodeId).ToString().Length < 10)
                                    {
                                        string CodeIDAfter = f.GenCodeID();
                                        RegisDebitPayment.Card.CodeIdAfter = f.ConvertDecimal(CodeIDAfter).ToString();
                                    }
                                    else
                                    {
                                        RegisDebitPayment.Card.CodeIdAfter = f.ConvertDecimal(RegisCashPayment.Card.CodeId).ToString();
                                    }
                                    UlangReadCardDataKey1:
                                    try
                                    {
                                        var SaldoEmoney = UpdateBlok("04", "04", RegisDebitPayment.Card.SaldoEmoneyAfter.ToString());
                                        var TicketWeekDayAfter = UpdateBlok("05", "04", RegisDebitPayment.Card.TicketWeekDayAfter.ToString());
                                        var TicketWeekEndAfter = UpdateBlok("06", "04", RegisDebitPayment.Card.TicketWeekEndAfter.ToString());
                                        var SaldoJaminanAfter = UpdateBlok("08", "08", RegisDebitPayment.Card.SaldoJaminanAfter.ToString());
                                        var CodeId = UpdateBlok("09", "08", f.ConvertDecimal(RegisDebitPayment.Card.CodeIdAfter).ToString());
                                        if (SaldoEmoney.Success == true &&
                                            TicketWeekDayAfter.Success == true &&
                                            TicketWeekEndAfter.Success == true &&
                                            SaldoJaminanAfter.Success == true &&
                                            CodeId.Success == true)
                                        {
                                            var DataSave = new SaveRegisDebitTrx();
                                            DataSave.Asuransi = RegisDebitPayment.Asuransi;
                                            DataSave.Card = RegisDebitPayment.Card;
                                            DataSave.Cashback = RegisDebitPayment.Cashback;
                                            DataSave.Payment = RegisDebitPayment.Payment;
                                            DataSave.QtyTotalTiket = RegisDebitPayment.QtyTotalTiket;
                                            DataSave.SaldoJaminan = RegisDebitPayment.SaldoJaminan;
                                            DataSave.tiket = RegisDebitPayment.tiket;
                                            DataSave.Topup = RegisDebitPayment.Topup;
                                            DataSave.TotalAll = RegisDebitPayment.TotalAll;
                                            DataSave.TotalBeliTiket = RegisDebitPayment.TotalBeliTiket;
                                            DataSave.TotalSewa = RegisDebitPayment.TotalSewa;
                                            DataSave.Sewa = RegisDebitPayment.Sewa;

                                            string IdTicket = f.GetIdTiket();
                                            string IdItemSewa = f.GetIdTrx();
                                            string ChasierBy = f.GetNamaUser(General.IDUser);
                                            string ComputerName = f.GetComputerName();

                                            foreach (var Tiket in RegisDebitPayment.tiket)
                                            {
                                                var savetiket = f.SaveTicket(Tiket, DataSave.Card.IdCard + "-" + f.ConvertDecimal(DataSave.Card.CodeIdAfter).ToString(), IdTicket, ChasierBy, ComputerName);
                                            }

                                            SaveDebitTransaksiRegistrasiUlang:

                                            var save = f.SaveDebitTransaksiRegistrasi(DataSave, IdTicket, ComputerName, ChasierBy);
                                            if (save.Message.Contains('~') == true && save.Success == true)
                                            {
                                                var dtTrx = save.Message.Split('~');
                                                var UpdateAccount = ReadCardDataKey();
                                                f.UpdatAccountData(UpdateAccount);
                                                if (RegisDebitPayment.Sewa.Count() > 0)
                                                {
                                                    foreach (var s in RegisDebitPayment.Sewa)
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
                                                        txtTotalBelanja.Text = "";
                                                        txtBankCode.Text = "";

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
                                                        txtTotalBelanja.Text = "";
                                                        txtBankCode.Text = "";
                                                        txtNamaBank.Text = "";
                                                        txtDiskon.Text = "";
                                                        TxtNominalDiskon.Text = "";
                                                        txtAdminCharges.Text = "";
                                                        txtTotalDebit.Text = "";
                                                        txtATMNumber.Text = "";
                                                        txtNoReff.Text = "";
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
                                            }
                                            else
                                            {
                                                var res = MessageBox.Show("Error : " + save.Message, "SaveDebitTransaksiRegistrasi failed", MessageBoxButtons.RetryCancel, MessageBoxIcon.Warning);
                                                if (res == DialogResult.Retry)
                                                {
                                                    goto SaveDebitTransaksiRegistrasiUlang;
                                                }
                                            }
                                        }
                                        else
                                        {
                                            var res = MessageBox.Show("Error : " + SaldoJaminanAfter, "Reader not connected", MessageBoxButtons.RetryCancel, MessageBoxIcon.Warning);
                                            if (res == DialogResult.Retry)
                                            {
                                                goto UlangReadCardDataKey1;
                                            }
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        var res = MessageBox.Show("Error : " + ex.Message, "Reader not connected", MessageBoxButtons.RetryCancel, MessageBoxIcon.Warning);
                                        if (res == DialogResult.Retry)
                                        {
                                            goto UlangReadCardDataKey;
                                        }
                                    }
                                }
                                else
                                {
                                    var res = MessageBox.Show("Error : Cannot Akses to Key Card", "Reader Fail", MessageBoxButtons.RetryCancel, MessageBoxIcon.Warning);
                                    if (res == DialogResult.Retry)
                                    {
                                        goto UlangReadCardDataKey;
                                    }
                                }
                            }
                        }
                        else
                        {
                            var res = MessageBox.Show("Baca Kartu Gagal,Silahkan tempelkan Kartu lalu tekan 'Retry' ", "Baca data Fail", MessageBoxButtons.RetryCancel, MessageBoxIcon.Warning);
                            if (res == DialogResult.Retry)
                            {
                                goto UlangReadCardDataKey;
                            }
                        }
                    }
                    else
                    {
                        this.Close();
                    }

                }
                if (Model.ConfigurationFileStatic.VFDPort != null && Model.ConfigurationFileStatic.VFDPort != "")
                {
                    VFDPort.send("Terima Kasih", "Selamat Menikmati", VFDPort.sp.PortName);
                }
            }
            else
            {
                MessageBox.Show("Silahkan Isi Field yang Masih Kosong", "Empty Field", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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

        #region Printing
        public ReturnResult PrintTopup(string datetime)
        {
            var res = new ReturnResult();
            try
            {
                string now = f.GetDatetime();
                string s = "Datetime \t: " + datetime + Environment.NewLine;
                s += "ID Transaction\t: TRX" + datetime.Replace("/", "").Replace(":", "").Replace(" ", "") + Environment.NewLine;
                s += "Merchant ID \t: " + f.GetComputerName() + Environment.NewLine;
                s += "Nama Petugas \t: " + f.GetNamaUser(General.IDUser) + Environment.NewLine;
                s += "-------------------------------------------------------" + Environment.NewLine;
                s += "Transaksi Topup " + Environment.NewLine;
                s += "Nominal Topup \t: " + f.ConvertToRupiah(TopupDebitPayment.NominalTopup) + Environment.NewLine;
                s += "-------------------------------------------------------" + Environment.NewLine;
                s += "Total Bayar \t: " + f.ConvertToRupiah(TopupDebitPayment.Pay.TotalBayar) + Environment.NewLine;
                s += "Payment \t: EDC " + Environment.NewLine;
                s += "Nama Bank \t: "+ TopupDebitPayment.Pay.NamaBank + Environment.NewLine;
                s += "No.ATM \t\t: " + TopupDebitPayment.Pay.NoATM + Environment.NewLine;
                s += "Diskon \t\t: " + TopupDebitPayment.Pay.DiskonBank +"% - " + f.ConvertToRupiah(TopupDebitPayment.Pay.NominalDiskonBank) + Environment.NewLine;
                s += "Admin \t\t: " + f.ConvertToRupiah(TopupDebitPayment.Pay.AdminCharges) + Environment.NewLine;
                s += "Total Debit \t: " + f.ConvertToRupiah(TopupDebitPayment.Pay.DebitNominal) + Environment.NewLine;
                s += "-------------------------------------------------------" + Environment.NewLine;
                s += "Account Number \t: " + TopupDebitPayment.Card.IdCard+"-"+f.ConvertDecimal(TopupDebitPayment.Card.CodeId).ToString() + Environment.NewLine;
                s += "Previous Balance: " + f.ConvertToRupiah(TopupDebitPayment.Card.SaldoEmoney) + Environment.NewLine;
                s += "Current Balance \t: " + f.ConvertToRupiah(TopupDebitPayment.Card.SaldoEmoneyAfter) + Environment.NewLine;
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
                    string underLine1 = "======================================";
                    e1.Graphics.DrawString(underLine1, new Font("calibri", 7), new SolidBrush(Color.Black), 0, startY + Offset);
                    Offset = Offset + 10;
                    e1.Graphics.DrawString(f.GetFooterPrint(1), new Font("Arial", 8, FontStyle.Bold), new SolidBrush(Color.Black), HeadreX, startY + Offset);
                    Offset = Offset + 15;
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
        public ReturnResult PrintRegis(SaveFoodCourtPayment Data,string Datetime)
        {
            var res = new ReturnResult();
            try
            {
                string now = f.GetDatetime();
                string s = "Datetime \t: " + Datetime + Environment.NewLine;
                s += "ID Transaction\t: TRX" + Datetime.Replace("/", "").Replace(":", "").Replace(" ", "") + Environment.NewLine;
                s += "Merchant ID \t: " + f.GetComputerName() + Environment.NewLine;
                s += "Nama Petugas \t: " + f.GetNamaUser(General.IDUser) + Environment.NewLine;
                s += "-------------------------------------------------------" + Environment.NewLine;
                s += "Transaksi Registrasi " + Environment.NewLine;
                foreach (var ticket in RegisDebitPayment.tiket)
                {
                    s += "Nama Tiket \t: " + ticket.NamaTicket + Environment.NewLine;
                    s += "Harga Satuan \t: " + f.ConvertToRupiah(ticket.Harga) + Environment.NewLine;
                    s += "Qty \t\t: " + ticket.Qty + Environment.NewLine;
                    s += "Total \t\t: " + f.ConvertToRupiah(ticket.Total) + Environment.NewLine;
                    s += "Nama Diskon \t: " + ticket.NamaDiskon + Environment.NewLine;
                    s += "Diskon \t\t: " + ticket.Diskon + "%" + Environment.NewLine;
                    s += "Total Diskon \t: " + f.ConvertToRupiah(ticket.TotalDiskon) + Environment.NewLine;
                    s += "Total - Diskon \t: " + f.ConvertToRupiah(ticket.TotalAfterDiskon) + Environment.NewLine;
                }
                s += "-------------------------------------------------------" + Environment.NewLine;
                s += "Total Beli Tiket \t: " + f.ConvertToRupiah(RegisDebitPayment.TotalBeliTiket) + Environment.NewLine;
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

                s += "-------------------------------------------------------" + Environment.NewLine;

                if (RegisCashPayment.Asuransi > 0)
                {
                    s += "Asuransi " + RegisCashPayment.QtyTotalTiket + " Org \t: " + f.ConvertToRupiah(RegisDebitPayment.Asuransi) + Environment.NewLine;
                }

                if (RegisCashPayment.Card.SaldoJaminan == 0)
                {
                    if (RegisCashPayment.Card.SaldoJaminanAfter > 0)
                    {
                        s += "Saldo Jaminan \t: " + f.ConvertToRupiah(RegisDebitPayment.Card.SaldoJaminanAfter) + Environment.NewLine;
                    }
                }

                if (RegisDebitPayment.Cashback > 0)
                {
                    s += "-------------------------------------------------------" + Environment.NewLine;
                    s += "Cashback \t: - " + f.ConvertToRupiah(RegisDebitPayment.Cashback) + Environment.NewLine;
                }
                if (RegisDebitPayment.Topup > 0)
                {
                    s += "-------------------------------------------------------" + Environment.NewLine;
                    s += "Topup Emoney \t: " + f.ConvertToRupiah(RegisDebitPayment.Topup) + Environment.NewLine;
                }
                s += "-------------------------------------------------------" + Environment.NewLine;
                s += "Total Pembelian \t: " + f.ConvertToRupiah(RegisDebitPayment.TotalAll) + Environment.NewLine;

                if (RegisDebitPayment.Payment != null)
                {
                    s += "-------------------------------------------------------" + Environment.NewLine;
                    s += "Payment \t: " + RegisDebitPayment.Payment.JenisTransaksi + Environment.NewLine;
                    if (RegisDebitPayment.Payment.PayEmoney > 0)
                    {
                        s += "Use eMoney \t: " + f.ConvertToRupiah(RegisDebitPayment.Payment.PayEmoney) + Environment.NewLine;
                    }
                    s += "Nama Bank \t: " + RegisDebitPayment.Payment.NamaBank + Environment.NewLine;
                    s += "No.ATM \t\t: " + RegisDebitPayment.Payment.NoATM + Environment.NewLine;
                    s += "Diskon \t\t: " + RegisDebitPayment.Payment.DiskonBank + "% - " + f.ConvertToRupiah(RegisDebitPayment.Payment.NominalDiskonBank) + Environment.NewLine;
                    s += "Admin \t\t: " + f.ConvertToRupiah(RegisDebitPayment.Payment.AdminCharges) + Environment.NewLine;
                    s += "Total Debit \t: " + f.ConvertToRupiah(RegisDebitPayment.Payment.DebitNominal) + Environment.NewLine;
                    
                    if (RegisDebitPayment.Payment.PayEmoney > 0)
                    {
                        s += "-------------------------------------------------------" + Environment.NewLine;
                        s += "Account Number \t: " + RegisDebitPayment.Card.IdCard+"-"+f.ConvertDecimal(RegisDebitPayment.Card.CodeIdAfter).ToString()+ Environment.NewLine;
                        s += "Emoney Previous \t: " + f.ConvertToRupiah(RegisDebitPayment.Card.SaldoEmoney) + Environment.NewLine;
                        s += "Emoney Current \t: " + f.ConvertToRupiah(RegisDebitPayment.Card.SaldoEmoneyAfter) + Environment.NewLine;
                        s += "Saldo Jaminan \t: " + f.ConvertToRupiah(RegisCashPayment.Card.SaldoJaminanAfter) + Environment.NewLine;
                    }
                    else
                    {
                        s += "-------------------------------------------------------" + Environment.NewLine;
                        s += "Account Number \t: " + RegisCashPayment.Card.IdCard + "-" + f.ConvertDecimal(RegisCashPayment.Card.CodeIdAfter).ToString() + Environment.NewLine;
                        s += "Previous Balance \t: " + f.ConvertToRupiah(RegisCashPayment.Card.SaldoEmoney) + Environment.NewLine;
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
                if (printkolom.Title != null && printkolom.Visible == "1" && printkolom.Title != null)
                {
                    printkolom.NoTicket = "ID" + Datetime.Replace("/", "").Replace(":", "").Replace(" ", "");
                    s += "-------------------------------------------------------" + Environment.NewLine;
                    s += printkolom.Title + Environment.NewLine;
                    s += Environment.NewLine;
                    s += "ID Ticket : " + printkolom.NoTicket + Environment.NewLine;
                    s += ".................................................................." + Environment.NewLine;
                    s += printkolom.Nama + Environment.NewLine;
                    s += ".................................................................." + Environment.NewLine;
                    s += printkolom.MoKtp + Environment.NewLine;
                    s += ".................................................................." + Environment.NewLine;
                    s += printkolom.NoTelp + Environment.NewLine;
                    s += ".................................................................." + Environment.NewLine;
                    s += printkolom.Alamat + Environment.NewLine + Environment.NewLine;

                    s += "-------------------------------------------------------" + Environment.NewLine + Environment.NewLine;
                    s += "ID Ticket : " + printkolom.NoTicket + Environment.NewLine;
                    s += ".................................................................." + Environment.NewLine;
                    s += printkolom.Nama + Environment.NewLine;
                    s += ".................................................................." + Environment.NewLine;
                    s += printkolom.MoKtp + Environment.NewLine;
                    s += ".................................................................." + Environment.NewLine;
                    s += printkolom.NoTelp + Environment.NewLine;
                    s += ".................................................................." + Environment.NewLine;
                    s += printkolom.Alamat + Environment.NewLine + Environment.NewLine;

                }

                PrintDocument p = new PrintDocument();
                p.PrintPage += delegate (object sender1, PrintPageEventArgs e1)
                {
                    int HeadreX = 0;
                    int startY = 0;
                    int Offset = 0;
                    string underLine1 = "======================================";
                    e1.Graphics.DrawString(underLine1, new Font("calibri", 7), new SolidBrush(Color.Black), 0, startY + Offset);
                    Offset = Offset + 10;
                    e1.Graphics.DrawString(f.GetFooterPrint(1), new Font("Arial", 8, FontStyle.Bold), new SolidBrush(Color.Black), HeadreX, startY + Offset);
                    Offset = Offset + 15;
                    e1.Graphics.DrawString(f.GetFooterPrint(2), new Font("Arial", 7, FontStyle.Bold), new SolidBrush(Color.Black), new RectangleF(0, startY + Offset, p.DefaultPageSettings.PrintableArea.Width, p.DefaultPageSettings.PrintableArea.Height));
                    Offset = Offset + 15;
                    e1.Graphics.DrawString(f.GetFooterPrint(3), new Font("Arial", 5, FontStyle.Italic), new SolidBrush(Color.Black), new RectangleF(0, startY + Offset, p.DefaultPageSettings.PrintableArea.Width, p.DefaultPageSettings.PrintableArea.Height));
                    Offset = Offset + 15;
                    string underLine = "======================================";
                    e1.Graphics.DrawString(underLine, new Font("Arial", 7), new SolidBrush(Color.Black), 0, startY + Offset);
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
    }
}
