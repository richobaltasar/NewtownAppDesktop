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
    public partial class AccountLog : UserControl
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

        public AccountLog()
        {
            InitializeComponent();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void button19_Click(object sender, EventArgs e)
        {
            var Card = ReadCardDataKey();
            if (Card.Success == true)
            {
                f.UpdatAccountData(Card);
                if (Card.SaldoJaminan > 0)
                {
                    TxtBacaKartu.Text = "ACCOUNT DETAIL";
                    TxtBacaKartu.Text = TxtBacaKartu.Text + "\n================";
                    TxtBacaKartu.Text = TxtBacaKartu.Text + "\n Account Number : " + Card.IdCard;
                    TxtBacaKartu.Text = TxtBacaKartu.Text + "\n Code ID \t\t: " + f.ConvertDecimal(Card.CodeId).ToString();
                    TxtBacaKartu.Text = TxtBacaKartu.Text + "\n Saldo Emoney \t: Rp " + string.Format("{0:n0}", Card.SaldoEmoney);
                    TxtBacaKartu.Text = TxtBacaKartu.Text + "\n Ticket WeekDay \t: " + string.Format("{0:n0}", Card.TicketWeekDay);
                    TxtBacaKartu.Text = TxtBacaKartu.Text + "\n Ticket WeekEnd \t: " + string.Format("{0:n0}", Card.TicketWeekEnd);
                    TxtBacaKartu.Text = TxtBacaKartu.Text + "\n SaldoJaminan \t: Rp " + string.Format("{0:n0}", Card.SaldoJaminan);
                    load_datagrid(Card.IdCard+"-"+ f.ConvertDecimal(Card.CodeId).ToString());
                    if (Model.ConfigurationFileStatic.VFDPort != null && Model.ConfigurationFileStatic.VFDPort != "")
                    {
                        VFDPort.send("Saldo Anda sebesar :", f.ConvertToRupiah(Card.SaldoEmoney), VFDPort.sp.PortName);
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
            }
        }

        public void atur_grid()
        {
            dt_grid.Rows.Clear();
            dt_grid.ColumnCount = 5;
            dt_grid.Columns[0].Name = "Id Log";
            dt_grid.Columns[1].Name = "Datetime";
            dt_grid.Columns[2].Name = "Jenis Transaksi";
            dt_grid.Columns[3].Name = "Uraian Transaksi";
            dt_grid.Columns[4].Name = "Nominal";

            dt_grid.RowHeadersVisible = false;
            dt_grid.ColumnHeadersVisible = true;
            DataGridViewColumn column1 = dt_grid.Columns[0];
            DataGridViewColumn column2 = dt_grid.Columns[1];
            DataGridViewColumn column3 = dt_grid.Columns[2];
            DataGridViewColumn column4 = dt_grid.Columns[3];
            DataGridViewColumn column5 = dt_grid.Columns[4];

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
            dt_grid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dt_grid.AllowUserToResizeRows = false;
            dt_grid.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            dt_grid.DefaultCellStyle.SelectionBackColor = Color.LightBlue;
            dt_grid.DefaultCellStyle.SelectionForeColor = Color.Black;
            dt_grid.RowHeadersDefaultCellStyle.SelectionBackColor = Color.Empty;
            dt_grid.RowsDefaultCellStyle.BackColor = Color.White;
            dt_grid.AlternatingRowsDefaultCellStyle.BackColor = Color.White;
            dt_grid.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dt_grid.ColumnHeadersDefaultCellStyle.BackColor = Color.Black;
            dt_grid.RowHeadersDefaultCellStyle.BackColor = Color.Black;
            dt_grid.DefaultCellStyle.Font = new Font("Calibri", 12);
            dt_grid.Columns[0].Width = 40;
        }

        public void load_datagrid(string AccountNumber)
        {
            dt_grid.Rows.Clear();
            atur_grid();
            var data = f.GetHistoryAcc(AccountNumber);
            int a = 0;
            foreach (var r in data)
            {
                a++;
                string[] row = new string[] { r.idlog, r.Datetime, r.JenisTransaksi, r.Uraian, r.Nominal };
                this.dt_grid.Rows.Add(row);
                DataGridViewRow d = dt_grid.Rows[a - 1];
                d.MinimumHeight = 40;
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

        private void AccountLog_Load(object sender, EventArgs e)
        {

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
                            if (SaldoEmoney.Success == true && tiketWeekDay.Success == true && tiketWeekEnd.Success == true && JaminanSaldo.Success == true)
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
    }
}
