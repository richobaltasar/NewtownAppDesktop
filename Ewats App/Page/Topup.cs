using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Ewats_App.Model;

namespace Ewats_App.Page
{
    public partial class Topup : UserControl
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

        public Topup()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
            var page = new Main();
            page.PagePanel.Controls.Add(new Page.Dashboard());
            var dash = new Dashboard();
            dash.Show();
            f.PageControl("Payment");
        }

        public void controlPanel(bool Visible)
        {
            panelInputManual.Visible = Visible;
            panelNominal.Visible = Visible;
            panelKeyb.Visible = Visible;
            PanelHitung.Visible = Visible;
        }

        private void button19_Click(object sender, EventArgs e)
        {
            var Card = ReadCardDataKey();
            if (Card.Success == true)
            {
                f.UpdatAccountData(Card);
                if (Card.SaldoJaminan > 0)
                {
                    TopupCashPayment.Card = Card;
                    TxtBacaKartu.Text = "ACCOUNT DETAIL";
                    TxtBacaKartu.Text = TxtBacaKartu.Text + "\n================";
                    TxtBacaKartu.Text = TxtBacaKartu.Text + "\n Account Number : " + Card.IdCard;
                    TxtBacaKartu.Text = TxtBacaKartu.Text + "\n Code ID \t\t: " + f.ConvertDecimal(Card.CodeId).ToString();
                    TxtBacaKartu.Text = TxtBacaKartu.Text + "\n Saldo Emoney \t: Rp " + string.Format("{0:n0}", Card.SaldoEmoney);
                    TxtBacaKartu.Text = TxtBacaKartu.Text + "\n Ticket WeekDay \t: " + string.Format("{0:n0}", Card.TicketWeekDay);
                    TxtBacaKartu.Text = TxtBacaKartu.Text + "\n Ticket WeekEnd \t: " + string.Format("{0:n0}", Card.TicketWeekEnd);
                    TxtBacaKartu.Text = TxtBacaKartu.Text + "\n SaldoJaminan \t: Rp " + string.Format("{0:n0}", Card.SaldoJaminan);
                    panelInputManual.Visible = true;
                    panelNominal.Visible = true;
                    panelKeyb.Visible = true;
                    PanelHitung.Visible = true;
                    if (Model.ConfigurationFileStatic.VFDPort != null && Model.ConfigurationFileStatic.VFDPort != "")
                    {
                        Function.VFDPort.send("Saldo Kartu Anda :", f.ConvertToRupiah(Card.SaldoEmoney), Function.VFDPort.sp.PortName);
                    }
                }
                else
                {
                    TxtBacaKartu.Text = "Gelang belum registrasi, silahkan melakukan registrasi Ticket";
                }
            }
            else
            {
                TxtBacaKartu.Text = "Gagal membaca data kartu";
                panelInputManual.Visible = false;
                panelNominal.Visible = false;
                panelKeyb.Visible = false;
                PanelHitung.Visible = false;
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
            else {
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
        private void button13_Click(object sender, EventArgs e)
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
        private void button21_Click(object sender, EventArgs e)
        {
            input_keyPad((sender as Button).Text, txtControl.Text);
        }
        private void button20_Click(object sender, EventArgs e)
        {
            input_keyPad((sender as Button).Text, txtControl.Text);
        }
        private void button1_Click_1(object sender, EventArgs e)
        {
            input_keyPad((sender as Button).Text, txtControl.Text);
        }
        #endregion

        #region Even 
        private void button2_Click(object sender, EventArgs e)
        {
            txtBoxInput.Text = button2.Text;
        }
        private void txtBoxInput_Click(object sender, EventArgs e)
        {
            txtControl.Text = txtBoxInput.Name;
        }
        private void button25_Click(object sender, EventArgs e)
        {
            txtControl.Text = txtBoxInput.Name;
            controlPanel(false);
            if (Model.ConfigurationFileStatic.VFDPort != null && Model.ConfigurationFileStatic.VFDPort != "")
            {
                Function.VFDPort.send("Selamat Datang", "di KUMPAY WATERPARK", Function.VFDPort.sp.PortName);
            }
        }
        private void panelKeyb_Paint(object sender, PaintEventArgs e)
        {

        }
        private void lblTotalBayar_TextChanged(object sender, EventArgs e)
        {
            if (lblTotalBayar.ToString().Length > 0)
            {
                if (Model.ConfigurationFileStatic.VFDPort != null && Model.ConfigurationFileStatic.VFDPort != "")
                {
                    Function.VFDPort.send("Topup sebesar :", f.ConvertToRupiah(f.ConvertDecimal(lblTotalBayar.Text)), Function.VFDPort.sp.PortName);
                }
            }
        }
        private void button5_Click(object sender, EventArgs e)
        {
            txtBoxInput.Text = button5.Text;
        }
        private void button3_Click(object sender, EventArgs e)
        {
            txtBoxInput.Text = button3.Text;
        }
        private void button6_Click(object sender, EventArgs e)
        {
            txtBoxInput.Text = button6.Text;
        }
        private void button4_Click(object sender, EventArgs e)
        {
            txtBoxInput.Text = button4.Text;
        }
        private void button23_Click(object sender, EventArgs e)
        {
            txtBoxInput.Text = button23.Text;
        }
        private void button22_Click(object sender, EventArgs e)
        {
            string data = txtBoxInput.Text;
            if (data != "")
            {
                data = data.Replace(".", "").Replace(",", "");
                if (data.All(char.IsDigit) == true)
                {
                    decimal d = Convert.ToDecimal(data);
                    if (d > 0)
                    {
                        CashPayment.TotalPembayaran = d;
                        CashPayment.JenisTransaksi = "TOPUP";
                        if (TopupCashPayment.Card != null)
                        {
                            TopupCashPayment.NominalTopup = d;
                            var pay = new PaymentMethod();
                            pay.JenisTransaksi = "TOPUP";
                            pay.TotalBayar = d;
                            TopupCashPayment.Pay = pay;
                            
                            Form fc = Application.OpenForms["TunaiPayment"];
                            if (fc != null)
                            {
                                fc.Show();
                                fc.BringToFront();
                                TextBox txtTotalBelanja = fc.Controls.Find("txtTotalBelanja", true).FirstOrDefault() as TextBox;
                                if (txtTotalBelanja != null)
                                {
                                    txtTotalBelanja.Text = f.ConvertToRupiah(TopupCashPayment.NominalTopup);
                                }
                            }
                            else
                            {
                                Page.TunaiPayment frm = new Page.TunaiPayment();
                                frm.StartPosition = FormStartPosition.CenterScreen;
                                frm.BringToFront();
                                frm.MaximizeBox = false;
                                frm.MinimizeBox = false;
                                frm.Show();
                                TextBox txtTotalBelanja = frm.Controls.Find("txtTotalBelanja", true).FirstOrDefault() as TextBox;
                                if (txtTotalBelanja != null)
                                {
                                    txtTotalBelanja.Text = f.ConvertToRupiah(TopupCashPayment.NominalTopup);
                                }
                            }                            
                        }
                    }
                }
            }
        }
        private void button24_Click(object sender, EventArgs e)
        {
            string data = txtBoxInput.Text;
            if (data != "")
            {
                data = data.Replace(".", "").Replace(",", "");
                if (data.All(char.IsDigit) == true)
                {
                    decimal d = Convert.ToDecimal(data);
                    if (d > 0)
                    {
                        DebitPayment.TotalPembayaran = d;
                        DebitPayment.JenisTransaksi = "TOPUP";
                        TopupDebitPayment.Card = ReadCardDataKey();
                        if (TopupDebitPayment.Card != null)
                        {
                            TopupDebitPayment.NominalTopup = d;
                            TopupDebitPayment.Card.SaldoEmoneyAfter = TopupDebitPayment.Card.SaldoEmoney + d;

                            var pay = new DebitPaymentMethod();
                            pay.JenisTransaksi = "TOPUP";
                            pay.TotalBayar = d;
                            TopupDebitPayment.Pay = pay;

                            Form fc = Application.OpenForms["DebitCard"];
                            if (fc != null)
                            {
                                fc.Show();
                                fc.BringToFront();
                                TextBox txtTotalBelanja = fc.Controls.Find("txtTotalBelanja", true).FirstOrDefault() as TextBox;
                                if (txtTotalBelanja != null)
                                {
                                    txtTotalBelanja.Text = f.ConvertToRupiah(TopupDebitPayment.NominalTopup);
                                }
                            }
                            else
                            {
                                DebitCard frm = new DebitCard();
                                frm.StartPosition = FormStartPosition.CenterScreen;
                                frm.BringToFront();
                                frm.MaximizeBox = false;
                                frm.MinimizeBox = false;
                                frm.Show();
                                TextBox txtTotalBelanja = frm.Controls.Find("txtTotalBelanja", true).FirstOrDefault() as TextBox;
                                if (txtTotalBelanja != null)
                                {
                                    txtTotalBelanja.Text = f.ConvertToRupiah(TopupDebitPayment.NominalTopup);
                                }
                            }
                        }
                    }
                }
            }
        }
        private void txtBoxInput_TextChanged(object sender, EventArgs e)
        {
            lblTotalBayar.Text = txtBoxInput.Text;
        }
        private void Topup_Load(object sender, EventArgs e)
        {
            txtControl.Text = txtBoxInput.Name;
            controlPanel(false);
        }
        #endregion

    }
}
