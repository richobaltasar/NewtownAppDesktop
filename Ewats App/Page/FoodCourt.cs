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

namespace Ewats_App.Page
{
    public partial class FoodCourt : UserControl
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
        public List<KeranjangFoodcourt> DataKeranjang = new List<KeranjangFoodcourt>();

        public FoodCourt()
        {
            InitializeComponent();
        }
        private void FoodCourt_Load(object sender, EventArgs e)
        {
            LoadComboTenant();
            atur_grid();
        }
        private void button19_Click(object sender, EventArgs e)
        {
            var Card = ReadCardDataKey();
            if (Card.Success == true)
            {
                f.UpdatAccountData(Card);
                if (Card.SaldoJaminan > 0)
                {
                    RefundCash.Card = Card;
                    TxtBacaKartu.Text = "======================";
                    TxtBacaKartu.Text = TxtBacaKartu.Text + "\nACCOUNT DETAIL";
                    TxtBacaKartu.Text = TxtBacaKartu.Text + "\n======================";
                    TxtBacaKartu.Text = TxtBacaKartu.Text + "\n Account Number : " + Card.IdCard;
                    TxtBacaKartu.Text = TxtBacaKartu.Text + "\n Code ID \t\t: " + f.ConvertDecimal(Card.CodeId).ToString();
                    TxtBacaKartu.Text = TxtBacaKartu.Text + "\n Saldo Emoney \t: Rp " + string.Format("{0:n0}", Card.SaldoEmoney);
                    TxtBacaKartu.Text = TxtBacaKartu.Text + "\n Ticket WeekDay \t: " + string.Format("{0:n0}", Card.TicketWeekDay);
                    TxtBacaKartu.Text = TxtBacaKartu.Text + "\n Ticket WeekEnd \t: " + string.Format("{0:n0}", Card.TicketWeekEnd);
                    TxtBacaKartu.Text = TxtBacaKartu.Text + "\n SaldoJaminan \t: Rp " + string.Format("{0:n0}", Card.SaldoJaminan);
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
            //displayOut(2, 0, tmpStr);
            retCode = card_function.Card.SCardTransmit(hCard, ref pioSendRequest, ref SendBuff[0], SendLen, ref pioSendRequest, ref RecvBuff[0], ref RecvLen);

            if (retCode != card_function.Card.SCARD_S_SUCCESS)
            {

                //displayOut(1, retCode, "");
                return retCode;

            }

            tmpStr = "";
            for (indx = 0; indx <= RecvLen - 1; indx++)
            {

                tmpStr = tmpStr + " " + string.Format("{0:X2}", RecvBuff[indx]);

            }

            //displayOut(3, 0, tmpStr);
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

        private void dt_grid_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.ColumnIndex == 0)// created column index (delete button)
            {
                if (e.RowIndex >= 0)
                {
                    dt_grid.Rows.Remove(dt_grid.Rows[e.RowIndex]);
                    DataKeranjang.RemoveAt(e.RowIndex);
                    decimal total = 0;
                    foreach (var tt in DataKeranjang)
                    {
                        total = total + (tt.Harga * tt.Qtx);
                    }
                    txtTotalBayar.Text = "Total : Rp " + string.Format("{0:n0}", total);

                    if (DataKeranjang.Count > 0)
                    {
                        PanelCheckOut.Visible = true;
                    }
                    else
                    {
                        PanelCheckOut.Visible = false;
                    }
                }
            }
        }
        private void dt_grid_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = this.dt_grid.Rows[e.RowIndex];
                if (row.Cells.Count > 1)
                {
                    var d = new KeranjangFoodcourt();
                    d.IdTrx = row.Cells["Id Trx"].Value.ToString();
                    d.NamaItem = row.Cells["Nama Item"].Value.ToString();
                    d.Harga = f.ConvertDecimal(row.Cells["Harga"].Value.ToString());
                    d.Qtx = f.ConvertDecimal(row.Cells["Qtx"].Value.ToString());
                    if (txtTotalBayar.Text == "Total : Rp 0")
                    { DataKeranjang.Clear(); }
                    DataKeranjang.Add(d);
                    if (Model.ConfigurationFileStatic.VFDPort != null && Model.ConfigurationFileStatic.VFDPort != "")
                    {
                        Function.VFDPort.send(d.IdTrx + "." + d.NamaItem + " - " + d.Qtx, "Total: " + f.ConvertToRupiah(d.Harga * d.Qtx), Function.VFDPort.sp.PortName);
                    }
                    decimal total = 0;
                    foreach (var tt in DataKeranjang)
                    {
                        total = total + (tt.Harga * tt.Qtx);
                    }
                    txtTotalBayar.Text = "Total : Rp "+string.Format("{0:n0}", total);
                    if (DataKeranjang.Count > 0)
                    {
                        PanelCheckOut.Visible = true;
                    }
                    else
                    {
                        PanelCheckOut.Visible = false;
                    }

                    
                }
            }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            reset();
        }

        public void reset()
        {
            DataKeranjang.Clear();
            FoodCourtPayment.Card = null;
            FoodCourtPayment.Pay = null;
            TxtBacaKartu.Text = "";
            txtTotalBayar.Text = "Total : Rp 0";
            ListMenu.Items.Clear();
            dt_grid.Rows.Clear();
            PanelCheckOut.Visible = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Bacaulang:
            var card = ReadCardDataKey();
            if (card.Success == true)
            {
                f.UpdatAccountData(card);
                FoodCourtPayment.Card = card;
                if (FoodCourtPayment.Card.SaldoEmoney > 0)
                {
                    FoodCourtPayment.Keranjang = DataKeranjang;
                    if (FoodCourtPayment.Keranjang.Count > 0)
                    {
                        decimal total = 0;
                        foreach (var list in FoodCourtPayment.Keranjang)
                        {
                            total = total + (list.Harga * list.Qtx);
                        }
                        var pay = new PaymentMethod();
                        pay.TotalBayar = total;
                        if (FoodCourtPayment.Card.SaldoEmoney >= pay.TotalBayar)
                        {
                            pay.JenisTransaksi = "eMoney";
                            pay.PayEmoney = total;
                            FoodCourtPayment.Card.SaldoEmoneyAfter = FoodCourtPayment.Card.SaldoEmoney - total;
                            FoodCourtPayment.Pay = pay;
                            if (Model.ConfigurationFileStatic.VFDPort != null && Model.ConfigurationFileStatic.VFDPort != "")
                            {
                                Function.VFDPort.send("Saldo terpakai : ", f.ConvertToRupiah(pay.TotalBayar), Function.VFDPort.sp.PortName);
                            }
                            var res = MessageBox.Show("Apakah Anda yakin untuk melakukan pembayaran sebesar "+ f.ConvertToRupiah(pay.TotalBayar) + " ?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                            if (res == DialogResult.Yes)
                            {
                                EncodeLagi:
                                var Saldo = UpdateBlok("04", "04", FoodCourtPayment.Card.SaldoEmoneyAfter.ToString());
                                if (Saldo.Success == true)
                                {
                                    var DataSave = new SaveFoodCourtPayment();
                                    DataSave.Card = FoodCourtPayment.Card;
                                    DataSave.Keranjang = FoodCourtPayment.Keranjang;
                                    DataSave.Pay = FoodCourtPayment.Pay;

                                    string IdTrx = f.GetIdTrx();
                                    string Chasier = f.GetNamaUser(General.IDUser);
                                    string ComputerName = f.GetComputerName();

                                    var HasilSave = f.SaveFoodCourtPayment(DataSave,IdTrx, Chasier,ComputerName);
                                    var dtTrx = HasilSave.Message.Split('~');
                                    foreach (var dataItem in DataSave.Keranjang)
                                    {
                                        var SaveItems = f.SaveItemsFB(dataItem,DataSave.Card.IdCard+"-"+f.ConvertDecimal(DataSave.Card.CodeId).ToString(),IdTrx,Chasier,ComputerName);
                                    }

                                    var UpdateAccount = ReadCardDataKey();
                                    f.UpdatAccountData(UpdateAccount);

                                    PrintLagi:
                                    var print = PrintFoodCourt(dtTrx[1].Trim());
                                    if (print.Success == true)
                                    {
                                        f.RefreshDashboard();
                                        General.Page = "Foodcourt";
                                        Print fp = new Print();
                                        fp.Show();
                                        fp.BringToFront();
                                        fp.StartPosition = FormStartPosition.CenterScreen;
                                        fp.txtMessageBox.Text = "belanja Berhasil sebesar " + string.Format("{0:n0}", FoodCourtPayment.Pay.TotalBayar);
                                        reset();
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
                                    var res2 = MessageBox.Show("Update Saldo Gagal", "Encode Fail", MessageBoxButtons.RetryCancel, MessageBoxIcon.Warning);
                                    if (res2 == DialogResult.Retry)
                                    {
                                        goto EncodeLagi;
                                    }
                                }
                            }
                            
                        }
                        else
                        {
                            decimal SisaTopup = pay.TotalBayar - FoodCourtPayment.Card.SaldoEmoney;
                            if (Model.ConfigurationFileStatic.VFDPort != null && Model.ConfigurationFileStatic.VFDPort != "")
                            {
                                Function.VFDPort.send("Saldo kurang sebesar", f.ConvertToRupiah(SisaTopup), Function.VFDPort.sp.PortName);
                            }
                            var res = MessageBox.Show("Saldo tidak cukup,lakukan Topup : " + f.ConvertToRupiah(SisaTopup) + ", Apakah Anda ingin melakukan Topup ?", "reading Fail", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                            if (res == DialogResult.Yes)
                            {
                                this.Hide();
                                GotoPage("Topup");
                            }
                        }
                    }
                    else
                    {
                        var res = MessageBox.Show("Keranjang belanjaan masih kosong", "reading Fail", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                else
                {
                    FoodCourtPayment.Keranjang = DataKeranjang;
                    if (FoodCourtPayment.Keranjang.Count > 0)
                    {
                        decimal total = 0;
                        foreach (var list in FoodCourtPayment.Keranjang)
                        {
                            total = total + (list.Harga * list.Qtx);
                        }
                        var pay = new PaymentMethod();
                        pay.TotalBayar = total;
                        decimal SisaTopup = pay.TotalBayar - FoodCourtPayment.Card.SaldoEmoney;
                        var res = MessageBox.Show("Saldo tidak cukup,lakukan Topup : " + f.ConvertToRupiah(SisaTopup) + ", Apakah Anda ingin melakukan Topup ?", "reading Fail", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                        if (res == DialogResult.Yes)
                        {
                            this.Hide();
                            GotoPage("Topup");
                        }
                    }
                }

            }
            else
            {
                var res = MessageBox.Show("Error : Reading data Card fail", "reading Fail", MessageBoxButtons.RetryCancel, MessageBoxIcon.Warning);
                if (res == DialogResult.Retry)
                {
                    goto Bacaulang;
                }
            }
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
                //displayOut(0, 0, "Authentication success!");
                res.Message = "Authentication success";
                res.Success = true;
            }
            else
            {
                //displayOut(4, 0, "Authentication failed!");
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

        private void button3_Click(object sender, EventArgs e)
        {
            f.ShowOnScreenKeyboard();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            CashPayment.JenisTransaksi = "FoodCourt";
            FoodCourtPayment.Keranjang = DataKeranjang;
            General.Page = "Foodcourt";
            if (FoodCourtPayment.Keranjang.Count > 0)
            {
                decimal total = 0;
                foreach (var list in FoodCourtPayment.Keranjang)
                {
                    total = total + (list.Harga * list.Qtx);
                }
                var pay = new PaymentMethod();
                pay.TotalBayar = total;
                pay.PayCash = total;
                pay.PayEmoney = 0;
                pay.JenisTransaksi = "Cash";
                FoodCourtPayment.Pay = pay;
                TunaiPayment f = new TunaiPayment();
                Form fc = Application.OpenForms["TunaiPayment"];
                if (fc != null)
                {
                    fc.Show();
                    fc.BringToFront();
                }
                else
                {
                    TunaiPayment frm = new TunaiPayment();
                    frm.StartPosition = FormStartPosition.CenterScreen;
                    frm.BringToFront();
                    frm.MaximizeBox = false;
                    frm.MinimizeBox = false;
                    frm.Show();
                }
            }
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
                    //displayOut(2, 0, "");
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

        #region Function
        public void LoadComboTenant()
        {
            cbTenant.Items.Clear();
            var data = f.GetTenant();
            foreach (var d in data)
            {
                ComboboxItem item = new ComboboxItem();
                item.Text = d.NamaTenant;
                item.Value = d.Id;
                cbTenant.Items.Add(item);
            }
        }
        public void GotoPage(string PageA)
        {
            Form frm = Application.OpenForms["Main"];
            if (frm != null)
            {
                Panel tbx = frm.Controls.Find("PagePanel", true).FirstOrDefault() as Panel;
                UserControl fc = tbx.Controls.Find(PageA, true).FirstOrDefault() as UserControl;
                if (fc != null)
                {
                    fc.Show();
                    fc.BringToFront();
                }
                else
                {
                    if (PageA == "Topup")
                    {
                        var Page = new Page.Topup();
                        Page.Width = tbx.Width;
                        Page.Height = tbx.Height;
                        tbx.Controls.Add(Page);
                        Page.BringToFront();
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
        }
        #endregion
        #region Print
        public ReturnResult PrintFoodCourt(string datetime)
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
                foreach (var Items in FoodCourtPayment.Keranjang)
                {
                    d++;
                    s += d +". " + Items.NamaItem +" - "+Items.Qtx +"\t : "+ f.ConvertToRupiah(Items.Harga*Items.Qtx) +Environment.NewLine;

                }
                s += "-------------------------------------------------------" + Environment.NewLine;
                s += "Total Belanja \t\t: " + f.ConvertToRupiah(FoodCourtPayment.Pay.TotalBayar) + Environment.NewLine;

                if (FoodCourtPayment.Pay != null)
                {
                    s += "-------------------------------------------------------" + Environment.NewLine;
                    s += "Payment \t\t: " + FoodCourtPayment.Pay.JenisTransaksi + Environment.NewLine;
                    if (FoodCourtPayment.Pay.PayEmoney > 0)
                    {
                        s += "Use eMoney \t\t: Rp " + string.Format("{0:n0}", (FoodCourtPayment.Pay.PayEmoney)) + Environment.NewLine;
                    }
                    if (FoodCourtPayment.Pay.PayEmoney > 0)
                    {
                        s += "-------------------------------------------------------" + Environment.NewLine;
                        s += "Account Number \t:" +Environment.NewLine +FoodCourtPayment.Card.IdCard+"-"+f.ConvertDecimal(FoodCourtPayment.Card.CodeId).ToString() + Environment.NewLine;
                        s += "Emoney Before \t: " + f.ConvertToRupiah(FoodCourtPayment.Card.SaldoEmoney) + Environment.NewLine;
                        s += "Emoney Current \t: " + f.ConvertToRupiah(FoodCourtPayment.Card.SaldoEmoneyAfter) + Environment.NewLine;
                        s += "Saldo Jaminan \t: " + f.ConvertToRupiah(FoodCourtPayment.Card.SaldoJaminanAfter) + Environment.NewLine;
                    }
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
        private void cbTenant_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox cmb = (ComboBox)sender;
            ComboboxItem d = (ComboboxItem)cmb.SelectedItem;
            GetMenu(d.Value.ToString());
        }

        public void GetMenu(string Tenant)
        {
            var dataMenu = f.GetBarang(Tenant);
            ImageList il = new ImageList();
            int count = 0;
            ListMenu.Clear();
            foreach (var img in dataMenu)
            {
                try
                {
                    System.Net.WebRequest request = System.Net.WebRequest.Create(img.LinkPic);
                    System.Net.WebResponse resp = request.GetResponse();
                    System.IO.Stream respStream = resp.GetResponseStream();
                    Bitmap bmp = new Bitmap(respStream);
                    respStream.Dispose();

                    il.ImageSize = new Size(80, 80);
                    il.Images.Add(bmp);
                }
                catch(Exception ex)
                {
                    //throw ex;
                }
            }
            ListMenu.LargeImageList = il;
            foreach (var data in dataMenu)
            {
                ListViewItem lst = new ListViewItem();
                lst.Text = data.NamaBarang;
                lst.Name = data.IdMenu+"~"+data.NamaBarang+"~"+data.Harga+"~"+data.Stok;
                lst.ImageIndex = count++;
                ListMenu.Items.Add(lst);
            }
            
        }
        public void atur_grid()
        {
            dt_grid.Rows.Clear();
            dt_grid.ColumnCount = 6;
            dt_grid.Columns[0].Name = "X";
            dt_grid.Columns[1].Name = "Id Trx";
            dt_grid.Columns[2].Name = "Nama Item";
            dt_grid.Columns[3].Name = "Harga";
            dt_grid.Columns[4].Name = "Qtx";
            dt_grid.Columns[5].Name = "Total Harga";

            dt_grid.RowHeadersVisible = false;
            dt_grid.ColumnHeadersVisible = true;
            DataGridViewColumn column1 = dt_grid.Columns[0];
            DataGridViewColumn column2 = dt_grid.Columns[1];
            DataGridViewColumn column3 = dt_grid.Columns[2];
            DataGridViewColumn column4 = dt_grid.Columns[3];
            DataGridViewColumn column5 = dt_grid.Columns[4];
            DataGridViewColumn column6 = dt_grid.Columns[5];

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
            dt_grid.RowsDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dt_grid.DefaultCellStyle.Font = new Font("Tahoma", 8);
            dt_grid.Columns[0].Width = 10;
        }

        private void ListMenu_Click(object sender, EventArgs e)
        {
            var data = ListMenu.SelectedItems[0];
            if (data.Name != "")
            {
                OrderMenu frm = new OrderMenu();
                frm.Show();
                frm.BringToFront();
                frm.StartPosition = FormStartPosition.CenterScreen;
                Label lblKodeBarang = frm.Controls.Find("lblKodeBarang", true).FirstOrDefault() as Label;
                Label lblNamaProduk = frm.Controls.Find("lblNamaProduk", true).FirstOrDefault() as Label;
                Label lblHarga = frm.Controls.Find("lblHarga", true).FirstOrDefault() as Label;
                Label lblSisa = frm.Controls.Find("lblSisa", true).FirstOrDefault() as Label;
                if (lblKodeBarang != null)
                {
                    lblNamaProduk.Text = data.Text;
                    var param = data.Name.Split('~');
                    lblKodeBarang.Text = param[0];
                    lblNamaProduk.Text = param[1];
                    lblHarga.Text = "Rp "+ string.Format("{0:n0}", f.ConvertDecimal(param[2]));
                    lblSisa.Text = param[3];
                }
            }
        }
    }
}
