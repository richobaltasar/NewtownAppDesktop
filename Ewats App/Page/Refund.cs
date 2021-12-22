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
using System.Drawing.Printing;
using System.Runtime.InteropServices;

namespace Ewats_App.Page
{
    public partial class Refund : UserControl
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

        public Refund()
        {
            InitializeComponent();
        }

        private void Refund_Load(object sender, EventArgs e)
        {
            btnRefund.Visible = false;
        }

        private void button19_Click(object sender, EventArgs e)
        {
            var Card = ReadCardDataKey();
            if (Card.Success == true)
            {
                f.UpdatAccountData(Card);
                if (Card.SaldoJaminan > 0)
                {
                    var Aktif = f.CheckExpired(Card.IdCard, Card.CodeId);
                    if (Aktif.Message == "Aktif")
                    {
                        RefundCash.Card = Card;
                        TxtBacaKartu.Text = "ACCOUNT DETAIL";
                        TxtBacaKartu.Text = TxtBacaKartu.Text + "\n================";
                        TxtBacaKartu.Text = TxtBacaKartu.Text + "\n Account Number : " + Card.IdCard;
                        TxtBacaKartu.Text = TxtBacaKartu.Text + "\n Code ID \t \t : " + f.ConvertDecimal(Card.CodeId).ToString();
                        TxtBacaKartu.Text = TxtBacaKartu.Text + "\n Saldo Emoney \t: Rp " + string.Format("{0:n0}", Card.SaldoEmoney);
                        TxtBacaKartu.Text = TxtBacaKartu.Text + "\n Ticket WeekDay \t: " + string.Format("{0:n0}", Card.TicketWeekDay);
                        TxtBacaKartu.Text = TxtBacaKartu.Text + "\n Ticket WeekEnd \t: " + string.Format("{0:n0}", Card.TicketWeekEnd);
                        TxtBacaKartu.Text = TxtBacaKartu.Text + "\n SaldoJaminan \t: Rp " + string.Format("{0:n0}", Card.SaldoJaminan);
                        TxtBacaKartu.Text = TxtBacaKartu.Text + "\n================";
                        TxtBacaKartu.Text = TxtBacaKartu.Text + "\n Total Refund \t: Rp " + string.Format("{0:n0}", (Card.SaldoJaminan + Card.SaldoEmoney));
                        btnRefund.Visible = true;
                        if (Model.ConfigurationFileStatic.VFDPort != null && Model.ConfigurationFileStatic.VFDPort != "")
                        {
                            Function.VFDPort.send("Total Refund :", f.ConvertToRupiah((Card.SaldoJaminan + Card.SaldoEmoney)), Function.VFDPort.sp.PortName);
                        }
                    }
                    else
                    {
                        TxtBacaKartu.Text = "ACCOUNT DETAIL";
                        TxtBacaKartu.Text = TxtBacaKartu.Text + "\n================";
                        TxtBacaKartu.Text = TxtBacaKartu.Text + "\n Account Number : " + Card.IdCard;
                        TxtBacaKartu.Text = TxtBacaKartu.Text + "\n Code ID \t: " + f.ConvertDecimal(Card.CodeId).ToString();
                        TxtBacaKartu.Text = TxtBacaKartu.Text + "\n Account telah Expired";
                        btnRefund.Visible = false;
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
                btnRefund.Visible = false;
            }
        }

        private void btnRefund_Click(object sender, EventArgs e)
        {
            var data = MessageBox.Show("Apakah Anda yakin untuk melakukan Refund sebesar " + f.ConvertToRupiah(RefundCash.Card.SaldoEmoney+RefundCash.Card.SaldoJaminan), "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (data == DialogResult.Yes)
            {
                ulangBaca:
                var card = ReadCardDataKey();
                if (card.Success == true)
                {
                    f.UpdatAccountData(card);
                    card.SaldoEmoneyAfter = 0;
                    card.SaldoJaminanAfter = 0;
                    card.TicketWeekDayAfter = 0;
                    card.TicketWeekEndAfter = 0;
                    card.CodeIdAfter = "0";

                    RefundCash.Card = card;
                    RefundCash.NominalRefund = card.SaldoEmoney + card.SaldoJaminan;
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
                        var DataSaveRefund = new SaveRefundCash();
                        DataSaveRefund.Card = RefundCash.Card;
                        DataSaveRefund.NominalRefund = RefundCash.NominalRefund;
                        DataSaveRefund.NamaUser = f.GetNamaUser(General.IDUser);
                        DataSaveRefund.ComputerName = f.GetComputerName();

                        var save = f.SaveTransaksiRefund(DataSaveRefund);
                        if (save.Message.Contains('~') == true)
                        {
                            var dtTrx = save.Message.Split('~');
                            if (save.Success == true)
                            {
                                var cardUpdate = ReadCardDataKey();
                                f.UpdatAccountData(cardUpdate);
                                OpenDrawer();
                            }
                        Printulang:
                            var print = PrintRefund(dtTrx[1].Trim());
                            if (print.Success == true)
                            {
                                f.RefreshDashboard();
                                TxtBacaKartu.Text = "";
                                btnRefund.Visible = false;
                                Print fp = new Print();
                                fp.Show();
                                fp.BringToFront();
                                fp.StartPosition = FormStartPosition.CenterScreen;
                                fp.txtMessageBox.Text = "Refund Berhasil sebesar " + string.Format("{0:n0}", RefundCash.NominalRefund);

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
                            var res = MessageBox.Show("SaveTransaksiRefund error,karena tidak bisa generate ID Trx nya, error :" + save.Message, "Read data Fail", MessageBoxButtons.RetryCancel, MessageBoxIcon.Warning);
                            if (res == DialogResult.Retry)
                            {
                                goto ulangBaca;
                            }
                        }
                    
                    }
                    else
                    {
                        goto ulangBaca;
                    }
                }
                else
                {
                    var res = MessageBox.Show("Read Data Kartu Gagal, "+card.Message, "Read data Fail", MessageBoxButtons.RetryCancel, MessageBoxIcon.Warning);
                    if (res == DialogResult.Retry)
                    {
                        goto ulangBaca;
                    }
                }
                if (Model.ConfigurationFileStatic.VFDPort != null && Model.ConfigurationFileStatic.VFDPort != "")
                {
                    Function.VFDPort.send("Terima Kasih,Selamat datang kembali", "", Function.VFDPort.sp.PortName);
                }
            }
        }

        #region Printing
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
        public ReturnResult PrintRefund(string datetime)
        {
            var res = new ReturnResult();
            try
            {
                string s = "Datetime \t: " + datetime + Environment.NewLine;
                s += "ID Transaction\t: TRX" + datetime.Replace("/", "").Replace(":", "").Replace(" ", "") + Environment.NewLine;
                s += "Merchant ID \t: " + f.GetComputerName() + Environment.NewLine;
                s += "Nama Petugas \t: " + f.GetNamaUser(General.IDUser) + Environment.NewLine;
                s += "-------------------------------------------------------" + Environment.NewLine;
                s += "Transaksi Refund " + Environment.NewLine;
                s += "Saldo Emoney \t: Rp " + string.Format("{0:n0}", RefundCash.Card.SaldoEmoney) + Environment.NewLine;
                s += "Saldo Jaminan \t: Rp " + string.Format("{0:n0}", RefundCash.Card.SaldoJaminan) + Environment.NewLine;
                if ((RefundCash.Card.TicketWeekDay + RefundCash.Card.TicketWeekEnd) > 0)
                {
                    s += "*Ticket Hangus " + Environment.NewLine;
                }
                s += "-------------------------------------------------------" + Environment.NewLine;
                s += "Total Refund \t: Rp " + string.Format("{0:n0}", RefundCash.NominalRefund) + Environment.NewLine;
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
            try
            {
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
            }
            catch (Exception ex)
            {
                res.Success = false;
                res.Message = "Smart Card UID tidak terdeteksi, error : "+ex.Message;
            }
            
            return res;
        }
        #endregion

    }
}
