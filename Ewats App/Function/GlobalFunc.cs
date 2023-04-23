using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using Ewats_App.Model;
using System.Configuration;
using System.Net.Http;
using Newtonsoft.Json;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.IO;
using System.Security.Cryptography;
using System.IO.Ports;
using System.Threading;

namespace Ewats_App.Function
{
    public static class StringExtensions
    {
        public static string Left(this string value, int maxLength)
        {
            if (string.IsNullOrEmpty(value)) return value;
            maxLength = Math.Abs(maxLength);

            return (value.Length <= maxLength
                   ? value
                   : value.Substring(0, maxLength)
                   );
        }
        public static string Right(this string value, int length)
        {
            return value.Substring(value.Length - length);
        }
    }

    public static class Encrypt
    {
        // This size of the IV (in bytes) must = (keysize / 8).  Default keysize is 256, so the IV must be
        // 32 bytes long.  Using a 16 character string here gives us 32 bytes when converted to a byte array.
        private const string initVector = "pemgail9uzpgzl88";
        // This constant is used to determine the keysize of the encryption algorithm
        private const int keysize = 256;
        //Encrypt
        public static string EncryptString(string plainText, string passPhrase)
        {
            byte[] initVectorBytes = Encoding.UTF8.GetBytes(initVector);
            byte[] plainTextBytes = Encoding.UTF8.GetBytes(plainText);
            PasswordDeriveBytes password = new PasswordDeriveBytes(passPhrase, null);
            byte[] keyBytes = password.GetBytes(keysize / 8);
            RijndaelManaged symmetricKey = new RijndaelManaged();
            symmetricKey.Mode = CipherMode.CBC;
            ICryptoTransform encryptor = symmetricKey.CreateEncryptor(keyBytes, initVectorBytes);
            MemoryStream memoryStream = new MemoryStream();
            CryptoStream cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write);
            cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
            cryptoStream.FlushFinalBlock();
            byte[] cipherTextBytes = memoryStream.ToArray();
            memoryStream.Close();
            cryptoStream.Close();
            return Convert.ToBase64String(cipherTextBytes);
        }
        //Decrypt
        public static string DecryptString(string cipherText, string passPhrase)
        {
            string res = "";
            try
            {
                byte[] initVectorBytes = Encoding.UTF8.GetBytes(initVector);
                byte[] cipherTextBytes = Convert.FromBase64String(cipherText);
                PasswordDeriveBytes password = new PasswordDeriveBytes(passPhrase, null);
                byte[] keyBytes = password.GetBytes(keysize / 8);
                RijndaelManaged symmetricKey = new RijndaelManaged();
                symmetricKey.Mode = CipherMode.CBC;
                ICryptoTransform decryptor = symmetricKey.CreateDecryptor(keyBytes, initVectorBytes);
                MemoryStream memoryStream = new MemoryStream(cipherTextBytes);
                CryptoStream cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);
                byte[] plainTextBytes = new byte[cipherTextBytes.Length];
                int decryptedByteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);
                memoryStream.Close();
                cryptoStream.Close();
                res = Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount);
            }
            catch (Exception ex)
            {
                res = "Error:"+ ex.Message;
            }
            return res;
        }
    }

    public static class VFDPort
    {
        public static GlobalFunc f = new GlobalFunc();
        public static SerialPort sp = new SerialPort();
        internal static bool HasOpenPort(string portName)
        {
            bool portState = false;

            if (portName != string.Empty)
            {
                foreach (var itm in SerialPort.GetPortNames())
                {
                    if (itm.Contains(portName))
                    {
                        if (VFDPort.sp.IsOpen) { portState = true; }
                        else { portState = false; }
                    }
                }
            }

            else { System.Windows.Forms.MessageBox.Show("Error: No Port Specified."); }

            return portState;
        }
        internal static bool KonekPort(string portName)
        {
            bool portState = false;
            try
            {
                if (sp.IsOpen)
                {
                    sp.Close();
                    sp.Dispose();
                    sp = null;
                }
                sp.PortName = portName;
                sp.BaudRate = 9600;
                sp.Parity = Parity.None;
                sp.DataBits = 8;
                sp.StopBits = StopBits.One;
                sp.Open();
                if (HasOpenPort(portName) == true)
                {
                    portState = true;
                }
            }
            catch (Exception ex)
            {
                var res = f.ShowMessagebox("Serial port tidak ditemukan, silahkan tekan yes untuk melakukan settingan PORT VFD \n "+ex.Message, "Warning", MessageBoxButtons.YesNo);
                if (res == DialogResult.Yes)
                {
                    f.PageControl("VFDConfig");
                }

            }
            return portState;
        }
        public static bool send(string Line1, string Line2, string Port)
        {
            bool res = false;
            ulang:
            if (HasOpenPort(Port) == false)
            {
                if (KonekPort(Port) == true)
                {
                    goto ulang;
                }
            }
            else
            {
                string title = Line1;
                byte[] bytesToSend = new byte[1] { 0x0C };
                VFDPort.sp.Write(bytesToSend, 0, 1);
                int d = title.Length;
                if (title.Length < 20)
                {
                    int sisa = 20 - d;
                    for (int a = 0; a < sisa; a++)
                    {
                        title = title + " ";
                    }
                }
                byte[] asciiBytes = Encoding.ASCII.GetBytes(title);
                VFDPort.sp.Write(asciiBytes, 0, asciiBytes.Length);
                byte[] enter = new byte[2] { 0x1F, 0x42 };
                VFDPort.sp.WriteLine(Line2);
                VFDPort.sp.Write(enter, 0, enter.Length);
                res = true;
            }
            
            return res;
        }
    }

    public class GlobalFunc
    {
        string server = ConfigurationFileStatic.ConnStrLog;
        string ImgPath = ConfigurationFileStatic.PathImgWeb;

        public SqlConnection conn = new SqlConnection();

        public SqlCommand cmd = new SqlCommand();

        #region keyboard_virtual
        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("user32.dll", SetLastError = true)]
        static extern bool PostMessage(IntPtr hWnd, uint Msg, UIntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        static extern IntPtr FindWindow(String sClassName, String sAppName);

        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        static extern IntPtr FindWindowEx(IntPtr hwndParent, IntPtr hwndChildAfter, String lpszClass, String lpszWindow);

        public void ShowOnScreenKeyboard()
        {
            ProcessStartInfo startInfo = new ProcessStartInfo(@"C:\Program Files\Common Files\Microsoft Shared\ink\TabTip.exe");
            Process.Start(startInfo);
        }

        public void HideOnScreenKeyboard()
        {
            Process[] oskProcessArray = Process.GetProcessesByName("TabTip");
            foreach (Process onscreenProcess in oskProcessArray)
            {
                onscreenProcess.Kill();
            }

        }

        #endregion

        public bool CheckDBConnApp(string ConnStr)
        {
            bool res = false;
            try
            {
                string con = ConnStr;
                using (SqlConnection connection = new SqlConnection(con))
                {
                    connection.Open();
                    connection.Close();
                    res = true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                res = false;
            }
            return res;
        }
        public string CheckDBLocal(string Ipserver, string DBname, string username, string password)
        {
            string res = "";
            try
            {
                string con = "Data Source = " + Ipserver + "; Initial Catalog = " + DBname + "; User ID = " + username + "; Password = " + password + "";
                using (SqlConnection connection = new SqlConnection(con))
                {
                    connection.Open();
                    connection.Close();
                    res = con;
                }
            }
            catch (Exception ex)
            {
                res = "error:" + ex.Message;
            }
            return res;
        }
        public bool CheckDbAlreadyExists(string Connstring, string DbName)
        {
            bool res = false;
            try
            {
                conn.ConnectionString = Connstring;
                using (var connection = conn)
                {
                    connection.Open();
                    string sql = "SELECT name FROM master.dbo.sysdatabases WHERE name = N'" + DbName + "'";
                    using (var command = new SqlCommand(sql, connection))
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            int count = 0;
                            while (reader.Read())
                            {
                                count++;
                            }
                            if (count > 0)
                            {
                                res = true;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                string message = ex.Message;
            }
            return res;
        }
        public bool CreateDB(string Connstring, string DBName)
        {
            bool res = false;
            try
            {
                conn.ConnectionString = Connstring;
                using (var connection = conn)
                {
                    connection.Open();
                    string sql = "CREATE DATABASE " + DBName + "";
                    using (var command = new SqlCommand(sql, connection))
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            res = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                string message = ex.Message;
            }
            return res;
        }

        public void PageControl(string name)
        {

            switch (name)
            {
                case "Login":
                    #region LoginPage
                    Form fc1 = Application.OpenForms["Login"];
                    if (fc1 != null)
                    {
                        fc1.Show();
                        fc1.BringToFront();
                    }
                    else
                    {
                        Login frm = new Login();
                        frm.StartPosition = FormStartPosition.CenterScreen;
                        frm.BringToFront();
                        frm.MaximizeBox = false;
                        frm.MinimizeBox = false;
                        frm.Show();
                    }
                    break;
                #endregion
                case "Main":
                    #region MainPage
                    Form fc2 = Application.OpenForms["Main"];
                    if (fc2 != null)
                    {
                        fc2.Show();
                        fc2.BringToFront();
                    }
                    else
                    {
                        Page.Main frm = new Page.Main();
                        frm.StartPosition = FormStartPosition.CenterScreen;
                        frm.BringToFront();
                        frm.MaximizeBox = false;
                        frm.MinimizeBox = false;
                        frm.Show();
                    }
                    break;
                #endregion
                case "InsertLicense":
                    #region Insert License
                    Form fc3 = Application.OpenForms["InsertLicense"];
                    if (fc3 != null)
                    {
                        fc3.Show();
                        fc3.BringToFront();
                    }
                    else
                    {
                        InsertLicense frm = new InsertLicense();
                        frm.StartPosition = FormStartPosition.CenterScreen;
                        frm.BringToFront();
                        frm.MaximizeBox = false;
                        frm.MinimizeBox = false;
                        frm.Show();
                    }
                    break;
                #endregion
                case "EwatsConfig":
                    #region Ewats Config
                    Form fc4 = Application.OpenForms["EwatsConfig"];
                    if (fc4 != null)
                    {
                        fc4.Show();
                        fc4.BringToFront();
                    }
                    else
                    {
                        EwatsConfig frm = new EwatsConfig();
                        frm.StartPosition = FormStartPosition.CenterScreen;
                        frm.BringToFront();
                        frm.MaximizeBox = false;
                        frm.MinimizeBox = false;
                        frm.Show();
                    }
                    break;
                #endregion
                case "InitPage":
                    #region InitPage
                    Form fc5 = Application.OpenForms["InitPage"];
                    if (fc5 != null)
                    {
                        fc5.Show();
                        fc5.BringToFront();
                    }
                    else
                    {
                        InitPage frm = new InitPage();
                        frm.StartPosition = FormStartPosition.CenterScreen;
                        frm.BringToFront();
                        frm.MaximizeBox = false;
                        frm.MinimizeBox = false;
                        frm.Show();
                    }
                    break;
                #endregion
                case "VFDConfig":
                    #region VFDConfig
                    Form fc6 = Application.OpenForms["VFDConfig"];
                    if (fc6 != null)
                    {
                        fc6.Show();
                        fc6.BringToFront();
                        fc6.TopMost = true;
                    }
                    else
                    {
                        VFDConfig frm = new VFDConfig();
                        frm.StartPosition = FormStartPosition.CenterScreen;
                        frm.BringToFront();
                        frm.MaximizeBox = false;
                        frm.MinimizeBox = false;
                        frm.Show();
                    }
                    break;
                #endregion
                case "ChangePassword":
                    #region ChangePassword
                    Form fc7 = Application.OpenForms["ChangePassword"];
                    if (fc7 != null)
                    {
                        fc7.Show();
                        fc7.BringToFront();
                        fc7.TopMost = true;
                    }
                    else
                    {
                        Page.ChangePassword frm = new Page.ChangePassword();
                        frm.StartPosition = FormStartPosition.CenterScreen;
                        frm.BringToFront();
                        frm.MaximizeBox = false;
                        frm.MinimizeBox = false;
                        frm.Show();
                    }
                    break;
                    #endregion
            }


        }

        public void GetTerimnalPos()
        {

        }

        public OLogin LoginProc(string username, string password)
        {
            ulang:
            var res = new OLogin(); 
            try
            {
                conn.ConnectionString = server;
                using (var connection = conn)
                {
                    connection.Open();
                    string sql = "exec SP_Login @Username='"+username+"', @Password='"+password+"'";
                    using (var command = new SqlCommand(sql, connection))
                    {
                        command.CommandTimeout = 0;
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                res.ID = reader["id"].ToString();
                                res.HakAkses = reader["hakakses"].ToString();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                var result = messageboxError(ex.Message);
                if (result == DialogResult.Retry)
                {
                    goto ulang;
                }
                else
                {
                    Application.Exit();
                }
            }
            return res;
        }

        public void UpdatAccountData(ReaderCard ReadUpdate)
        {
            ReadUpdate.CodeId = ConvertDecimal(ReadUpdate.CodeId).ToString();
            if (ReadUpdate.CodeId.Length == 14)
            {
                var AccountUpdate = new DataAccount();
                AccountUpdate.AccountNumber = ReadUpdate.IdCard + "-" + ConvertDecimal(ReadUpdate.CodeId).ToString();
                AccountUpdate.BalancedSesudah = ReadUpdate.SaldoEmoney;
                AccountUpdate.Ticket = (ReadUpdate.TicketWeekDay + ReadUpdate.TicketWeekEnd);
                AccountUpdate.JaminanGelangYgTerbaca = ReadUpdate.SaldoJaminan;
                ReadUpdateAccountData(AccountUpdate);
            }
            
        }

        public List<Ticket> GetHarga()
        {
            ulang:
            var data = new List<Ticket>();
            try
            {
                conn.ConnectionString = server;
                using (var connection = conn)
                {
                    connection.Open();
                    string sql = "exec SP_GETTICKETPRICE";
                    using (var command = new SqlCommand(sql, connection))
                    {
                        command.CommandTimeout = 0;
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var d = new Ticket();
                                d.namaticket = reader["namaticket"].ToString();
                                if (reader["Harga"].ToString() != "")
                                {
                                    d.harga = Convert.ToDecimal(reader["Harga"].ToString());
                                }
                                data.Add(d);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                var result = messageboxError(ex.Message);
                if (result == DialogResult.Retry)
                {
                    goto ulang;
                }
                else
                {
                    Application.Exit();
                }
            }
            return data;
        }

        public List<DataTenant> GetTenant()
        {
            ulang:
            var data = new List<DataTenant>();
            try
            {
                conn.ConnectionString = server;
                using (var connection = conn)
                {
                    connection.Open();
                    string sql = "exec SP_GetTenant";
                    using (var command = new SqlCommand(sql, connection))
                    {
                        command.CommandTimeout = 0;
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var d = new DataTenant();
                                d.Id = reader["idTenant"].ToString();
                                d.NamaTenant = reader["NamaTenant"].ToString();
                                data.Add(d);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                var result = messageboxError(ex.Message);
                if (result == DialogResult.Retry)
                {
                    goto ulang;
                }
                else
                {
                    Application.Exit();
                }
            }
            return data;
        }

        public ReturnResult CheckExpired(string AccountNum, string CodeId)
        {
            ulang:
            var res = new ReturnResult();
            try
            {
                CodeId = CodeId.Replace("\0", "");
                conn.ConnectionString = server;
                using (var connection = conn)
                {
                    connection.Open();
                    string sql = "exec SP_CheckExpired " +
                        "@CodeId='"+ AccountNum +"-"+ CodeId+"'";
                    using (var command = new SqlCommand(sql, connection))
                    {
                        command.CommandTimeout = 0;
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                res.Message = reader["Status"].ToString();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                var result = messageboxError(ex.Message);
                if (result == DialogResult.Retry)
                {
                    goto ulang;
                }
                else
                {
                    Application.Exit();
                }
            }
            return res;
        }

        public string GetNamaUser(string IdUser)
        {
            ulang:
            string nama = "";
            try
            {
                conn.ConnectionString = server;
                using (var connection = conn)
                {
                    connection.Open();
                    string sql = "exec SP_GetNamaUser "+IdUser+"";
                    using (var command = new SqlCommand(sql, connection))
                    {
                        command.CommandTimeout = 0;
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                nama = reader["NamaLengkap"].ToString();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                var result = messageboxError(ex.Message);
                if (result == DialogResult.Retry)
                {
                    goto ulang;
                }
                else
                {
                    Application.Exit();
                }
            }
            return nama;
        }

        public DataKolomPrintInputVisitor GetPrintKolomVisitor()
        {
        ulang:
            var res = new DataKolomPrintInputVisitor();
            try
            {
                conn.ConnectionString = server;
                using (var connection = conn)
                {
                    connection.Open();
                    string sql = "exec SP_GetPrintKolomVisitor";
                    using (var command = new SqlCommand(sql, connection))
                    {
                        command.CommandTimeout = 0;
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                res.Visible = reader["Visible"].ToString();
                                res.Title = reader["Title"].ToString();
                                res.Nama = reader["Nama"].ToString();
                                res.MoKtp = reader["MoKtp"].ToString();
                                res.Alamat = reader["Alamat"].ToString();
                                res.NoTelp = reader["NoTelp"].ToString();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                var result = messageboxError(ex.Message);
                if (result == DialogResult.Retry)
                {
                    goto ulang;
                }
                else
                {
                    Application.Exit();
                }
            }
            return res;
        }

        public string GetComputerName()
        {
            try
            {
                return Environment.MachineName;
            }
            catch (Exception ex)
            {
                return "Error :" + ex.Message;
            }
        }

        public decimal ConvertDecimal(string data)
        {
            decimal res = 0;
            if (data != "" && data!= null)
            {
                string filter = data.Replace("Rp", "").Replace(".", "").Replace(",", "").Replace("\0", "").Replace("\n","").Replace("ÿ","").Trim().Replace(":","").Replace("/","").Replace(" ", "");
                if (filter != "")
                {
                    if (filter.All(char.IsDigit) == true)
                    {
                        res = Convert.ToDecimal(filter);
                    }
                }
            }
            return res;
        }

        public ReturnResult SaveDataTambahModal(TambahModalCashbox data)
        {
            ulang:
            var res = new ReturnResult();
            try
            {
                conn.ConnectionString = server;
                using (var connection = conn)
                {
                    connection.Open();
                    string sql = "exec SP_SaveDataTambahModal " +
                        "@ComputerName='" + data.ComputerName + "'," +
                        "@NamaUser='" + data.NamaUser + "'," +
                        "@Nominal=" + data.Nominal + "";
                    using (var command = new SqlCommand(sql, connection))
                    {
                        command.CommandTimeout = 0;
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {

                                res.Message = reader["_Message"].ToString();
                                if (reader["Success"].ToString().Contains("TRUE") == true)
                                {
                                    res.Success = true;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                var result = messageboxError(ex.Message);
                if (result == DialogResult.Retry)
                {
                    goto ulang;
                }
                else
                {
                    Application.Exit();
                }
            }
            return res;
        }

        public ReturnResult SaveClosing(DashboardModel data, string ComputerName,string Username, decimal nominalCashierInput)
        {
            ulang:
            string sql = "";
            var res = new ReturnResult();
            try
            {
                conn.ConnectionString = server;
                using (var connection = conn)
                {
                    connection.Open();
                    sql = "exec  SP_Closing " +
                        "@TotalTransaksi=" + data.TotalTransaksi+"," +
                        "@TotalTopup=" + data.TotalTopup+"," +
                        "@TotalRegis=" + data.TotalRegis+"," +
                        "@TotalRefund=" + data.TotalRefund+"," +
                        "@TotalFoodcourt=" + (ConvertDecimal(data.TotalFoodcourtCash)+ ConvertDecimal(data.TotalFoodcourtEmoney))+"," +
                        "@TotalDanaModal=" + data.TotalDanaModal+"," +
                        "@TotalCashOut=" + data.TotalCashOut+"," +
                        "@TotalCashIn=" + data.TotalCashIn+"," +
                        "@TotalCashBox=" + data.TotalCashBox+"," +
                        "@TotalAllTicket=" + data.TotalAllTicket+"," +

                        "@TotalTrxEdc=" + data.TotalTrxEdc+"," +
                        "@TotalNominalDebit=" + ConvertDecimal(data.TotalNominalDebit)+"," +
                        "@TotalTrxEmoney=" + data.TotalTrxEmoney+"," +
                        "@TotalNominalDebitEmoney=" + ConvertDecimal(data.TotalNominalDebitEmoney)+"," +

                        "@TotalCashirInputMoneyCashbox="+nominalCashierInput+"," +
                        "@ComputerName='"+ComputerName+ "',@NamaUser='" + Username+"'";

                    using (var command = new SqlCommand(sql, connection))
                    {
                        command.CommandTimeout = 0;
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                res.Message = reader["_Message"].ToString();
                                if (reader["Success"].ToString().Contains("TRUE") == true)
                                {
                                    res.Success = true;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                var result = messageboxError(ex.Message+": "+sql);

                if (result == DialogResult.Retry)
                {
                    goto ulang;
                }
                else
                {
                    Application.Exit();
                }
            }
            return res;
        }

        public ReturnResult SaveTransaksiTopup(SaveTopupTrx Data)
        {
            ulang:
            var res = new ReturnResult();
            try
            {
                conn.ConnectionString = server;
                using (var connection = conn)
                {
                    connection.Open();
                    string sql = "exec SaveTransaksiTopup " +
                        "@AccountNumber='" + Data.Card.IdCard +"-"+ConvertDecimal(Data.Card.CodeId).ToString()+"', " +
                        "@NominalTopup = " + Data.NominalTopup + ", " +
                        "@JenisPayment='" + Data.Pay.JenisTransaksi + "' ," +
                        "@PayCash= " + Data.Pay.PayCash + "," +
                        "@TotalBayar= " + Data.Pay.TotalBayar + "," +
                        "@TerimaUang= " + Data.Pay.TerimaUang + "," +
                        "@Kembalian= " + Data.Pay.Kembalian + "," +
                        "@SaldoSebelum= " + Data.Card.SaldoEmoney + "," +
                        "@SaldoSetelah= " + Data.Card.SaldoEmoneyAfter + "," +
                        "@Chasierby= '" + Data.NamaUser + "'," +
                        "@ComputerName= '" + Data.ComputerName + "'";
                    using (var command = new SqlCommand(sql, connection))
                    {
                        command.CommandTimeout = 0;
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                
                                res.Message = reader["_Message"].ToString();
                                if (reader["Success"].ToString().Contains("TRUE") == true)
                                {
                                    res.Success = true;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                var result = messageboxError(ex.Message);
                if (result == DialogResult.Retry)
                {
                    goto ulang;
                }
                else
                {
                    Application.Exit();
                }
            }
            return res;
        }

        public ReturnResult SaveTransaksiDebitTopup(SaveDebitTopupTrx Data)
        {
            ulang:
            var res = new ReturnResult();
            try
            {
                conn.ConnectionString = server;
                using (var connection = conn)
                {
                    connection.Open();
                    string sql = "exec SaveDebitTransaksiTopup " +
                        "@AccountNumber='" + Data.Card.IdCard +"-"+ConvertDecimal(Data.Card.CodeId).ToString()+"'," +
                        "@NominalTopup = " + Data.NominalTopup + "," +
                        "@JenisPayment='" + Data.Pay.JenisTransaksi + "'," +
                        "@TotalBayar=" + Data.Pay.TotalBayar + "," +
                        "@KodeBank='" + Data.Pay.KodeBank + "'," +
                        "@NamaBank='" + Data.Pay.NamaBank + "'," +

                        "@DiskonBank=" + Data.Pay.DiskonBank + "," +
                        "@NominalDiskonBank=" + Data.Pay.NominalDiskonBank + "," +
                        "@AdminCharges=" + Data.Pay.AdminCharges + "," +
                        "@DebitNominal=" + Data.Pay.DebitNominal + "," +

                        "@NoATM ='" + Data.Pay.NoATM + "' ," +
                        "@NoReff='" + Data.Pay.NoReff + "'," +
                        "@SaldoSebelum=" + Data.Card.SaldoEmoney + "," +
                        "@SaldoSetelah=" + Data.Card.SaldoEmoneyAfter + "," +
                        "@Chasierby= '" + Data.NamaUser + "'," +
                        "@ComputerName= '" + Data.ComputerName + "'";
                    using (var command = new SqlCommand(sql, connection))
                    {
                        command.CommandTimeout = 0;
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                res.Message = reader["_Message"].ToString();
                                if (reader["Success"].ToString().Contains("TRUE") == true)
                                {
                                    res.Success = true;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                var result = messageboxError(ex.Message);
                if (result == DialogResult.Retry)
                {
                    goto ulang;
                }
                else
                {
                    Application.Exit();
                }
            }
            return res;
        }

        public ReturnResult SaveTransaksiRefund(SaveRefundCash Data)
        {
            ulang:
            var res = new ReturnResult();
            try
            {
                conn.ConnectionString = server;
                using (var connection = conn)
                {
                    connection.Open();
                    string sql = "exec SP_SaveTransaksiRefund " +
                        "@AccountNumber='" + Data.Card.IdCard + "-"+ConvertDecimal(Data.Card.CodeId).ToString()+"', " +
                        "@SaldoEmoney = " + Data.Card.SaldoEmoney + ", " +
                        "@SaldoJaminan=" + Data.Card.SaldoJaminan + " ," +
                        "@TicketWeekDay= " + Data.Card.TicketWeekDay + "," +
                        "@TicketWeekEnd= " + Data.Card.TicketWeekEnd + "," +
                        "@TotalNominalRefund= " + Data.NominalRefund + "," +
                        "@ChasierBy= '" + Data.NamaUser + "'," +
                        "@ComputerName= '" + Data.ComputerName + "'";

                    using (var command = new SqlCommand(sql, connection))
                    {
                        command.CommandTimeout = 0;
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                res.Message = reader["_Message"].ToString();
                                if (reader["Success"].ToString().Contains("TRUE") == true)
                                {
                                    res.Success = true;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                var result = messageboxError(ex.Message);
                if (result == DialogResult.Retry)
                {
                    goto ulang;
                }
                else
                {
                    Application.Exit();
                }
            }
            return res;
        }
        public ReturnResult SaveUpdateChangePassword(string UserId, string Password)
        {            
            ulang:
            var res = new ReturnResult();
            try
            {
                conn.ConnectionString = server;
                using (var connection = conn)
                {
                    connection.Open();
                    string sql = "exec SP_SaveUpdateChangePassword " +
                        "@UserId=" + UserId + "," +
                        "@Password='" + Password + "'" +
                        "";
                    using (var command = new SqlCommand(sql, connection))
                    {
                        command.CommandTimeout = 0;
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                res.Message = reader["_Message"].ToString();
                                if (reader["Success"].ToString().Contains("TRUE") == true)
                                {
                                    res.Success = true;
                                }
                            }
                        }
                    }
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                var result = messageboxError(ex.Message);
                if (result == DialogResult.Retry)
                {
                    goto ulang;
                }
                else
                {
                    Application.Exit();
                }
            }
            return res;
        }

        public ReturnResult SaveUpdateStockOpname(KeranjangStockOpnameModel Data)
        {
            string namaUser = GetNamaUser(General.IDUser);
            ulang:
            var res = new ReturnResult();
            try
            {
                conn.ConnectionString = server;
                using (var connection = conn)
                {
                    connection.Open();
                    string sql = "exec SP_SaveUpdateStockOpname " +
                        "@idItem=" + Data.idItem + "," +
                        "@NamaTenant='" + Data.NamaTenant + "'," +
                        "@NamaItem='" + Data.NamaItem + "'," +
                        "@BykStok=" + Data.BykStok + "," +
                        "@BykStokUpdate=" + Data.BykStokUpdate+"," +
                        "@NamaUser='"+ namaUser + "'" +
                        "";
                    using (var command = new SqlCommand(sql, connection))
                    {
                        command.CommandTimeout = 0;
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                res.Message = reader["_Message"].ToString();
                                if (reader["Success"].ToString().Contains("TRUE") == true)
                                {
                                    res.Success = true;
                                }
                            }
                        }
                    }
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                var result = messageboxError(ex.Message);
                if (result == DialogResult.Retry)
                {
                    goto ulang;
                }
                else
                {
                    Application.Exit();
                }
            }
            return res;
        }

        public ReturnResult SaveTransaksiRegistrasi(SaveRegisTrx Data,string TicketId,string ComputerName,string Chasier)
        {
            ulang:
            var res = new ReturnResult();
            try
            {
                conn.ConnectionString = server;
                using (var connection = conn)
                {
                    connection.Open();

                    string sql = "exec SP_SaveTransaksiRegistrasi " +
                        "@AccountNumber ='" + Data.Card.IdCard+"-"+ConvertDecimal(Data.Card.CodeIdAfter).ToString() + "'," +
                        "@SaldoEmoney = " + Data.Card.SaldoEmoney + "," +
                        "@SaldoEmoneyAfter = " + Data.Card.SaldoEmoneyAfter + "," +
                        "@TicketWeekDay = " + Data.Card.TicketWeekDay + "," +
                        "@TicketWeekDayAfter = " + Data.Card.TicketWeekDayAfter + "," +
                        "@TicketWeekEnd = " + Data.Card.TicketWeekEnd + "," +
                        "@TicketWeekEndAfter = " + Data.Card.TicketWeekEndAfter + "," +
                        "@SaldoJaminan = " + Data.Card.SaldoJaminan + "," +
                        "@SaldoJaminanAfter = " + Data.Card.SaldoJaminanAfter + "," +
                        "@IdTicketTrx = "+TicketId+"," +
                        "@Cashback = " + Data.Cashback + "," +
                        "@Topup = " + Data.Topup + "," +
                        "@Asuransi = " + Data.Asuransi + "," +
                        "@QtyTotalTiket = " + Data.QtyTotalTiket + "," +
                        "@TotalBeliTiket = " + Data.TotalBeliTiket + "," +
                        "@TotalAll=" + Data.TotalAll + "," +
                        "@JenisTransaksi='" + Data.Payment.JenisTransaksi + "'," +
                        "@TotalBayar=" + Data.Payment.TotalBayar + "," +
                        "@PayEmoney=" + Data.Payment.PayEmoney + "," +
                        "@PayCash=" + Data.Payment.PayCash + "," +
                        "@TerimaUang=" + Data.Payment.TerimaUang + "," +
                        "@Kembalian=" + Data.Payment.Kembalian + "," +
                        "@ComputerName='"+ComputerName+"'," +
                        "@Chasier='"+Chasier+"'";
                    
                    using (var command = new SqlCommand(sql, connection))
                    {
                        command.CommandTimeout = 0;
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                res.Message = reader["_Message"].ToString();
                                if (reader["Success"].ToString().Contains("TRUE") == true)
                                {
                                    res.Success = true;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                var result = messageboxError(ex.Message);
                if (result == DialogResult.Retry)
                {
                    goto ulang;
                }
                else
                {
                    Application.Exit();
                }
            }
            return res;
        }

        public ReturnResult SaveDebitTransaksiRegistrasi(SaveRegisDebitTrx Data, string TicketId, string ComputerName, string Chasier)
        {
            ulang:
            var res = new ReturnResult();
            try
            {
                conn.ConnectionString = server;
                using (var connection = conn)
                {
                    connection.Open();

                    string sql = "exec SP_SaveDebitTransaksiRegistrasi " +
                        "@AccountNumber ='" + Data.Card.IdCard +"-"+ ConvertDecimal(Data.Card.CodeIdAfter).ToString()+ "'," +
                        "@SaldoEmoney = " + Data.Card.SaldoEmoney + "," +
                        "@SaldoEmoneyAfter = " + Data.Card.SaldoEmoneyAfter + "," +
                        "@TicketWeekDay = " + Data.Card.TicketWeekDay + "," +
                        "@TicketWeekDayAfter = " + Data.Card.TicketWeekDayAfter + "," +
                        "@TicketWeekEnd = " + Data.Card.TicketWeekEnd + "," +
                        "@TicketWeekEndAfter = " + Data.Card.TicketWeekEndAfter + "," +
                        "@SaldoJaminan = " + Data.Card.SaldoJaminan + "," +
                        "@SaldoJaminanAfter = " + Data.Card.SaldoJaminanAfter + "," +
                        "@IdTicketTrx = " + TicketId + "," +
                        "@Cashback = " + Data.Cashback + "," +
                        "@Topup = " + Data.Topup + "," +
                        "@Asuransi = " + Data.Asuransi + "," +
                        "@QtyTotalTiket = " + Data.QtyTotalTiket + "," +
                        "@TotalBeliTiket = " + Data.TotalBeliTiket + "," +
                        "@TotalAll=" + Data.TotalAll + "," +
                        "@JenisTransaksi='" + Data.Payment.JenisTransaksi + "'," +
                        "@TotalBayar=" + Data.Payment.TotalBayar + "," +
                        "@PayEmoney=" + Data.Payment.PayEmoney + "," +

                        "@KodeBank='" + Data.Payment.KodeBank + "'," +
                        "@NamaBank='" + Data.Payment.NamaBank + "'," +
                        "@DiskonBank=" + Data.Payment.DiskonBank + "," +
                        "@NominalDiskonBank=" + Data.Payment.NominalDiskonBank + "," +
                        "@AdminCharges=" + Data.Payment.AdminCharges + "," +
                        "@NoATM='" + Data.Payment.NoATM + "'," +
                        "@NoReff='" + Data.Payment.NoReff + "'," +
                        "@DebitNominal=" + Data.Payment.DebitNominal + "," +

                        "@ComputerName='" + ComputerName + "'," +
                        "@Chasier='" + Chasier + "'";

                    using (var command = new SqlCommand(sql, connection))
                    {
                        command.CommandTimeout = 0;
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                res.Message = reader["_Message"].ToString();
                                if (reader["Success"].ToString().Contains("TRUE") == true)
                                {
                                    res.Success = true;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                var result = messageboxError(ex.Message);
                if (result == DialogResult.Retry)
                {
                    goto ulang;
                }
                else
                {
                    Application.Exit();
                }
            }
            return res;
        }

        public ReturnResult SaveTicket(KeranjangTicket data,string AccountNumber,string IdTicket,string Chasier, string ComputerName)
        {
            ulang:
            var res = new ReturnResult();
            try
            {
                conn.ConnectionString = server;
                using (var connection = conn)
                {
                    connection.Open();
                    if (data.IdDiskon == "-")
                    {
                        data.IdDiskon = "0";
                    }
                    string sql = "exec SP_SaveTicketKeranjang " +
                        "@AccountNumber ='" + AccountNumber + "'," +
                        "@IdTicket =" + IdTicket + "," +
                        "@NamaTicket='" + data.NamaTicket + "'," +
                        "@Harga=" + data.Harga + "," +
                        "@Qty=" + data.Qty + "," +
                        "@Total=" + data.Total + "," +
                        "@IdDiskon=" + data.IdDiskon + "," +
                        "@NamaDiskon='" + data.NamaDiskon + "'," +
                        "@Diskon=" + data.Diskon.ToString().Replace(",",".") + "," +
                        "@TotalDiskon=" + data.TotalDiskon + "," +
                        "@TotalAfterDiskon=" + data.TotalAfterDiskon + "," +
                        "@ChasierBy='"+Chasier+"'," +
                        "@ComputerName='"+ComputerName+"'";

                    using (var command = new SqlCommand(sql, connection))
                    {
                        command.CommandTimeout = 0;
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                res.Message = reader["_Message"].ToString();
                                if (reader["Success"].ToString().Contains("TRUE") == true)
                                {
                                    res.Success = true;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                var result = messageboxError(ex.Message);
                if (result == DialogResult.Retry)
                {
                    goto ulang;
                }
                else
                {
                    Application.Exit();
                }
            }
            return res;
        }

        public ReturnResult SaveSewa(KeranjangFoodcourt data, string AccountNumber, string IdTicket, string Chasier, string ComputerName)
        {
            ulang:
            var res = new ReturnResult();
            try
            {
                conn.ConnectionString = server;
                using (var connection = conn)
                {
                    connection.Open();
                    string sql = "";

                    using (var command = new SqlCommand(sql, connection))
                    {
                        command.CommandTimeout = 0;
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                res.Message = reader["_Message"].ToString();
                                if (reader["Success"].ToString().Contains("TRUE") == true)
                                {
                                    res.Success = true;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                var result = messageboxError(ex.Message);
                if (result == DialogResult.Retry)
                {
                    goto ulang;
                }
                else
                {
                    Application.Exit();
                }
            }
            return res;
        }

        public ReturnResult SaveItemsFB(KeranjangFoodcourt data, 
            string AccountNumber, string IdItems,
            string ChasierBy, string ComputerName)
        {
            ulang:
            var res = new ReturnResult();
            try
            {
                conn.ConnectionString = server;
                using (var connection = conn)
                {
                    connection.Open();
                    string sql = "exec SP_SaveItemsFB " +
                        "@IdItemsKeranjang =" + IdItems + "," +
                        "@KodeBarang =" + data.IdTrx + "," +
                        "@NamaItem='" + data.NamaItem + "'," +
                        "@Harga=" + data.Harga + "," +
                        "@Qtx=" + data.Qtx + "," +
                        "@AccountNumber='"+AccountNumber+"'," +
                        "@Chasierby='"+ ChasierBy + "'," +
                        "@ComputerName='"+ ComputerName + "'";

                    using (var command = new SqlCommand(sql, connection))
                    {
                        command.CommandTimeout = 0;
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                res.Message = reader["_Message"].ToString();
                                if (reader["Success"].ToString().Contains("TRUE") == true)
                                {
                                    res.Success = true;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                var result = messageboxError(ex.Message);
                if (result == DialogResult.Retry)
                {
                    goto ulang;
                }
                else
                {
                    Application.Exit();
                }
            }
            return res;
        }

        public ReturnResult SaveFoodCourtPayment(SaveFoodCourtPayment Data,string IdItemsKeranjang, string Chasier, string ComputerName)
        {
            ulang:
            var res = new ReturnResult();
            try
            {
                string CodeId = GenCodeID();
                conn.ConnectionString = server;
                using (var connection = conn)
                {
                    connection.Open();
                    string sql = "";
                    if (Data.Card == null)
                    {
                        sql = "exec SP_SaveFoodCourtPayment " +
                        "@AccountNumber='"+CodeId+"'," +
                        "@SaldoEmoney=0," +
                        "@SaldoEmoneyAfter=0," +
                        "@IdItemsKeranjang=" + IdItemsKeranjang + "," +
                        "@JenisTransaksi='" + Data.Pay.JenisTransaksi + "'," +
                        "@TotalBayar=" + Data.Pay.TotalBayar + "," +
                        "@PayEmoney=" + Data.Pay.PayEmoney + "," +
                        "@PayCash=" + Data.Pay.PayCash + "," +
                        "@TerimaUang=" + Data.Pay.TerimaUang + "," +
                        "@Kembalian=" + Data.Pay.Kembalian + "," +
                        "@ComputerName='" + ComputerName + "'," +
                        "@CashierBy='" + Chasier + "'";
                    }
                    else
                    {
                        sql = "exec SP_SaveFoodCourtPayment " +
                        "@AccountNumber='" + Data.Card.IdCard +"-"+CodeId+ "'," +
                        "@SaldoEmoney=" + Data.Card.SaldoEmoney + "," +
                        "@SaldoEmoneyAfter=" + Data.Card.SaldoEmoneyAfter + "," +
                        "@IdItemsKeranjang=" + IdItemsKeranjang + "," +
                        "@JenisTransaksi='" + Data.Pay.JenisTransaksi + "'," +
                        "@TotalBayar=" + Data.Pay.TotalBayar + "," +
                        "@PayEmoney=" + Data.Pay.PayEmoney + "," +
                        "@PayCash=" + Data.Pay.PayCash + "," +
                        "@TerimaUang=" + Data.Pay.TerimaUang + "," +
                        "@Kembalian=" + Data.Pay.Kembalian + "," +
                        "@ComputerName='" + ComputerName + "'," +
                        "@CashierBy='" + Chasier + "'";
                    }
                    

                    using (var command = new SqlCommand(sql, connection))
                    {
                        command.CommandTimeout = 0;
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                res.Message = reader["_Message"].ToString();
                                if (reader["Success"].ToString().Contains("TRUE") == true)
                                {
                                    res.Success = true;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                var result = messageboxError(ex.Message);
                if (result == DialogResult.Retry)
                {
                    goto ulang;
                }
                else
                {
                    Application.Exit();
                }
            }
            return res;
        }

        public decimal GetAsuransi()
        {
            ulang:
            decimal data = 0;
            try
            {
                conn.ConnectionString = server;
                using (var connection = conn)
                {
                    connection.Open();
                    string sql = "exec SP_GetAsuransi";
                    using (var command = new SqlCommand(sql, connection))
                    {
                        command.CommandTimeout = 0;
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var d = new Ticket();
                                if (reader["Harga"].ToString() != "")
                                {
                                    data = Convert.ToDecimal(reader["Harga"].ToString());
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                var result = messageboxError(ex.Message);
                if (result == DialogResult.Retry)
                {
                    goto ulang;
                }
                else
                {
                    Application.Exit();
                }  
            }
            return data;
        }

        public bool CheckNowWeekend()
        {
            ulang:
            bool res = true;
            try
            {
                conn.ConnectionString = server;
                using (var connection = conn)
                {
                    connection.Open();
                    string sql = "exec SP_CheckNowWeekend";
                    using (var command = new SqlCommand(sql, connection))
                    {
                        command.CommandTimeout = 0;
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                string hr = reader["HariNow"].ToString();
                                if (hr != "" && hr!= "Sunday" && hr!= "Saturday")
                                {
                                    res = false;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                var result = messageboxError(ex.Message);
                if (result == DialogResult.Retry)
                {
                    goto ulang;
                }
                else
                {
                    Application.Exit();
                }
            }
            return res;
        }

        public bool CheckPasswordCurrentValid(string Password, string User)
        {
            ulang:
            bool res = false;
            try
            {
                conn.ConnectionString = server;
                using (var connection = conn)
                {
                    connection.Open();
                    string sql = "exec SP_CheckPassword " +
                        "@User="+User+"," +
                        "@Password='"+Password+"'";

                    using (var command = new SqlCommand(sql, connection))
                    {
                        command.CommandTimeout = 0;
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                if (reader["Result"].ToString().Contains("TRUE") == true)
                                {
                                    res = true;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                var result = messageboxError(ex.Message);
                if (result == DialogResult.Retry)
                {
                    goto ulang;
                }
                else
                {
                    Application.Exit();
                }
            }
            return res;
        }

        public ReturnResult CheckOpenCashier()
        {
            string ComputerName = GetComputerName();
            ulang:
            var res = new ReturnResult();
            try
            {
                conn.ConnectionString = server;
                using (var connection = conn)
                {
                    connection.Open();
                    string sql = "exec SP_CheckOpenCashier '"+ComputerName+"'";
                    using (var command = new SqlCommand(sql, connection))
                    {
                        command.CommandTimeout = 0;
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                res.Message = reader["_Message"].ToString();
                                if (reader["Success"].ToString().Contains("TRUE") == true)
                                {
                                    res.Success = true;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                var result = messageboxError(ex.Message);
                if (result == DialogResult.Retry)
                {
                    goto ulang;
                }
                else
                {
                    Application.Exit();
                }
            }
            return res;
        }

        public ReturnResult CheckClosingCashier(string NamaUser)
        {
            string ComputerName = GetComputerName();
            ulang:
            var res = new ReturnResult();
            try
            {
                conn.ConnectionString = server;
                using (var connection = conn)
                {
                    connection.Open();
                    string sql = "exec SP_CheckClosingMerchant '" + ComputerName + "','"+NamaUser+"'";
                    using (var command = new SqlCommand(sql, connection))
                    {
                        command.CommandTimeout = 0;
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                res.Message = reader["_Message"].ToString();
                                if (reader["Success"].ToString().Contains("TRUE") == true)
                                {
                                    res.Success = true;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                var result = messageboxError(ex.Message);
                if (result == DialogResult.Retry)
                {
                    goto ulang;
                }
                else
                {
                    Application.Exit();
                }
            }
            return res;
        }

        public string GetIdTiket()
        {
            ulang:
            string Res = "";
            try
            {
                conn.ConnectionString = server;
                using (var connection = conn)
                {
                    connection.Open();
                    string sql = "exec SP_GedtIdTiket";
                    using (var command = new SqlCommand(sql, connection))
                    {
                        command.CommandTimeout = 0;
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Res = reader["IdTicket"].ToString();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                var result = messageboxError(ex.Message);
                if (result == DialogResult.Retry)
                {
                    goto ulang;
                }
                else
                {
                    Application.Exit();
                }
            }
            return Res;
        }

        public string GetIdTrx()
        {
            ulang:
            string Res = "";
            try
            {
                conn.ConnectionString = server;
                using (var connection = conn)
                {
                    connection.Open();
                    string sql = "exec [dbo].[SP_GedtIdTrxF&B]";
                    using (var command = new SqlCommand(sql, connection))
                    {
                        command.CommandTimeout = 0;
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Res = reader["IdItems"].ToString();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                var result = messageboxError(ex.Message);
                if (result == DialogResult.Retry)
                {
                    goto ulang;
                }
                else
                {
                    Application.Exit();
                }
            }
            return Res;
        }

        public List<Ticket> GetTicket(string Seacrh)
        {
            ulang:
            var data = new List<Ticket>();
            try
            {
                conn.ConnectionString = server;
                using (var connection = conn)
                {
                    connection.Open();
                    string sql = "exec SP_GETTICKETPRICE @search='" + Seacrh+"'";
                    using (var command = new SqlCommand(sql, connection))
                    {
                        command.CommandTimeout = 0;
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var d = new Ticket();
                                if (reader["Harga"].ToString() != "")
                                {
                                    d.harga = Convert.ToDecimal(reader["Harga"].ToString());
                                }
                                d.IdTicket = reader["id"].ToString();
                                d.namaticket = reader["NamaTicket"].ToString();
                                data.Add(d);

                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                var result = messageboxError(ex.Message);
                if (result == DialogResult.Retry)
                {
                    goto ulang;
                }
                else
                {
                    Application.Exit();
                }
            }
            return data;
        }

        public GetDataTransaksiRegistrasiModel GetDataTransaksiRegistrasi(AllTransaksiModel data)
        {
            ulang:
            var res = new GetDataTransaksiRegistrasiModel();
            try
            {
                conn.ConnectionString = server;
                using (var connection = conn)
                {
                    connection.Open();
                    string sql = "exec SP_GetDataTransaksiRegistrasi " +
                        "@IdTrx=" + data.IdTrx+"," +
                        "@Datetime='" + data.Datetime + "'," +
                        "@JenisTransaksi='" + data.JenisTransaksi + "'," +
                        "@Nominal=" + data.Nominal + "," +
                        "@CashierBy='" + data.CashierBy + "'";
                    using (var command = new SqlCommand(sql, connection))
                    {
                        command.CommandTimeout = 0;
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                res.AccountNumber = reader["AccountNumber"].ToString();
                                res.Asuransi =  ConvertDecimal(reader["Asuransi"].ToString());
                                res.Cashback = ConvertDecimal(reader["Cashback"].ToString());
                                res.CashierBy = reader["CashierBy"].ToString();
                                res.ComputerName = reader["ComputerName"].ToString();
                                res.Datetime = reader["Datetime"].ToString();
                                res.IdTicketTrx = reader["IdTicketTrx"].ToString();
                                res.idTrx = "TRX"+reader["Datetime"].ToString().Replace("/", "").Replace(":", "").Replace(" ", "");
                                res.JenisTransaksi = reader["JenisTransaksi"].ToString();
                                res.Kembalian = ConvertDecimal(reader["Kembalian"].ToString());
                                res.PayCash = ConvertDecimal(reader["PayCash"].ToString());
                                res.PayEmoney = ConvertDecimal((reader["PayEmoney"].ToString()));
                                res.QtyTotalTiket = ConvertDecimal(reader["QtyTotalTiket"].ToString());
                                res.SaldoJaminan = ConvertDecimal(reader["SaldoJaminan"].ToString());
                                res.SaldoEmoneyBefore = ConvertDecimal(reader["SaldoEmoney"].ToString());
                                res.SaldoEmoneyAfter = ConvertDecimal(reader["SaldoEmoneyAfter"].ToString());
                                res.TerimaUang = ConvertDecimal(reader["TerimaUang"].ToString());
                                res.topup = ConvertDecimal(reader["topup"].ToString());
                                res.TotalAll = ConvertDecimal(reader["TotalAll"].ToString());
                                res.TotalBayar = ConvertDecimal(reader["TotalBayar"].ToString());
                                res.TotalBeliTiket = ConvertDecimal(reader["TotalBeliTiket"].ToString());
                                res.BankCode = reader["BankCode"].ToString();
                                res.NamaBank = reader["NamaBank"].ToString();
                                res.DiskonBank = ConvertDecimal(reader["DiskonBank"].ToString());
                                res.NominalDiskon = ConvertDecimal(reader["NominalDiskon"].ToString());
                                res.AdminCharges = ConvertDecimal(reader["AdminCharges"].ToString());
                                res.TotalDebit = ConvertDecimal(reader["TotalDebit"].ToString());
                                res.NoATM = reader["NoATM"].ToString();
                                res.NoReffEddPrint = reader["NoReffEddPrint"].ToString();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                var result = messageboxError(ex.Message);
                if (result == DialogResult.Retry)
                {
                    goto ulang;
                }
                else
                {
                    Application.Exit();
                }
            }
            return res;
        }

        public GetDataTransaksiTopupModel GetDataTransaksiTopupReprint(AllTransaksiModel data)
        {
            ulang:
            var res = new GetDataTransaksiTopupModel();
            try
            {
                conn.ConnectionString = server;
                using (var connection = conn)
                {
                    connection.Open();
                    string sql = "exec SP_GetDataTransaksiTopup " +
                        "@IdTrx=" + data.IdTrx + "," +
                        "@Datetime='" + data.Datetime + "'," +
                        "@Nominal=" + data.Nominal.ToString() + "," +
                        "@CashierBy='" + data.CashierBy + "'";
                    using (var command = new SqlCommand(sql, connection))
                    {
                        command.CommandTimeout = 0;
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                res.AccountNumber = reader["AccountNumber"].ToString();
                                res.Datetime = reader["Datetime"].ToString();
                                res.IdTransaction = "TRX"+reader["Datetime"].ToString().Replace("/", "").Replace(":", "").Replace(" ", "");
                                res.Kembalian = ConvertToRupiah(ConvertDecimal(reader["Kembalian"].ToString()));

                                res.MerchantName = reader["ComputerName"].ToString();
                                res.NamaKasir = reader["Chasierby"].ToString();
                                res.NominalTopup = ConvertToRupiah(ConvertDecimal(reader["NominalTopup"].ToString()));

                                res.SaldoSebelumnya = ConvertToRupiah(ConvertDecimal(reader["SaldoSebelum"].ToString()));
                                res.SaldoSetelahnya = ConvertToRupiah(ConvertDecimal(reader["SaldoSetelah"].ToString()));
                                res.UangDibayarkan = ConvertToRupiah(ConvertDecimal(reader["TerimaUang"].ToString()));

                                res.PaymentMethod = reader["PaymentMethod"].ToString();
                                res.IdLogEDCTransaksi = reader["IdLogEDCTransaksi"].ToString();
                                res.BankCode = reader["BankCode"].ToString();
                                res.NamaBank = reader["NamaBank"].ToString();
                                res.DiskonBank = reader["DiskonBank"].ToString();
                                res.NominalDiskon = reader["NominalDiskon"].ToString();
                                res.AdminCharges = reader["AdminCharges"].ToString();
                                res.TotalDebit = reader["TotalDebit"].ToString();
                                res.NoATM = reader["NoATM"].ToString();
                                res.NoReffEddPrint = reader["NoReffEddPrint"].ToString();


                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                var result = messageboxError(ex.Message);
                if (result == DialogResult.Retry)
                {
                    goto ulang;
                }
                else
                {
                    Application.Exit();
                }
            }
            return res;
        }

        public GetDataTransaksiRefundReprintModel GetDataTransaksiRefundReprint(AllTransaksiModel data)
        {
            ulang:
            var res = new GetDataTransaksiRefundReprintModel();
            try
            {
                conn.ConnectionString = server;
                using (var connection = conn)
                {
                    connection.Open();
                    string sql = "exec SP_GetDataTransaksiRefund " +
                        "@IdTrx=" + data.IdTrx + "," +
                        "@Datetime='" + data.Datetime + "'," +
                        "@Nominal=" + data.Nominal.ToString() + "," +
                        "@CashierBy='" + data.CashierBy + "'";
                    using (var command = new SqlCommand(sql, connection))
                    {
                        command.CommandTimeout = 0;
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                res.IdTransaction = "TRX"+reader["Datetime"].ToString().Replace("/", "").Replace(":", "").Replace(" ", "");
                                res.AccounNumber = reader["AccountNumber"].ToString();
                                res.Datetime = reader["Datetime"].ToString();
                                res.MerchantName = reader["ComputerName"].ToString();
                                res.NamaKasir = reader["ChasierBy"].ToString();
                                res.SaldoEmoney = ConvertToRupiah(ConvertDecimal(reader["SaldoEmoney"].ToString()));
                                res.SaldoJaminan = ConvertToRupiah(ConvertDecimal(reader["SaldoJaminan"].ToString()));
                                res.TotalRefund = ConvertToRupiah(ConvertDecimal(reader["TotalNominalRefund"].ToString()));
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                var result = messageboxError(ex.Message);
                if (result == DialogResult.Retry)
                {
                    goto ulang;
                }
                else
                {
                    Application.Exit();
                }
            }
            return res;
        }

        public GetDataTransaksiFoodCourtReprintModel GetDataTransaksiFoodCourtReprint(AllTransaksiModel data)
        {
            ulang:
            var res = new GetDataTransaksiFoodCourtReprintModel();
            try
            {
                conn.ConnectionString = server;
                using (var connection = conn)
                {
                    connection.Open();
                    string sql = "exec SP_GetDataTransaksiFoodCourtReprint " +
                        "@IdTrx=" + data.IdTrx + "," +
                        "@Datetime='" + data.Datetime + "'," +
                        "@Nominal=" + data.Nominal.ToString() + "," +
                        "@CashierBy='" + data.CashierBy + "'";
                    using (var command = new SqlCommand(sql, connection))
                    {
                        command.CommandTimeout = 0;
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                res.IdTransaction = "TRX"+reader["Datetime"].ToString().Replace("/", "").Replace(":", "").Replace(" ", "");
                                res.Datetime = reader["Datetime"].ToString();
                                res.MerchantName = reader["ComputerName"].ToString();
                                res.NamaKasir = reader["CashierBy"].ToString();
                                res.TotalBelanja = ConvertToRupiah(ConvertDecimal(reader["TotalBayar"].ToString()));
                                res.PaymentMethod = reader["JenisTransaksi"].ToString();
                                res.UseEmoney = ConvertToRupiah(ConvertDecimal(reader["PayEmoney"].ToString()));
                                res.AccountNumber = reader["AccountNumber"].ToString();
                                res.SaldoSebelum = ConvertToRupiah(ConvertDecimal(reader["SaldoEmoney"].ToString()));
                                res.SaldoSetelah = ConvertToRupiah(ConvertDecimal(reader["SaldoEmoneyAfter"].ToString()));
                                res.IdItemKeranjang = reader["IdItemsKeranjang"].ToString();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                var result = messageboxError(ex.Message);
                if (result == DialogResult.Retry)
                {
                    goto ulang;
                }
                else
                {
                    Application.Exit();
                }
            }
            return res;
        }

        public List<GetGridTicketModel> GetGridTicket(string IdTicketTrx)
        {
            ulang:
            var res = new List<GetGridTicketModel>();
            try
            {
                conn.ConnectionString = server;
                using (var connection = conn)
                {
                    connection.Open();
                    string sql = "exec SP_GetGridTicket " +
                        "@IdTicket="+IdTicketTrx+"";
                    using (var command = new SqlCommand(sql, connection))
                    {
                        command.CommandTimeout = 0;
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var d = new GetGridTicketModel();
                                d.NamaTicket = reader["NamaTicket"].ToString();
                                d.BesarDiskon = reader["Diskon"].ToString() + " %";
                                d.HargaSatuan = ConvertToRupiah(ConvertDecimal(reader["Harga"].ToString()));
                                d.NamaDiskon = reader["NamaDiskon"].ToString();
                                d.Qty = reader["Qty"].ToString();
                                d.Total = ConvertToRupiah(ConvertDecimal(reader["Total"].ToString()));
                                d.TotalAkhir = ConvertToRupiah(ConvertDecimal(reader["TotalAfterDiskon"].ToString()));
                                d.TotalDiskon = ConvertToRupiah(ConvertDecimal(reader["TotalDiskon"].ToString()));
                                res.Add(d);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                var result = messageboxError(ex.Message);
                if (result == DialogResult.Retry)
                {
                    goto ulang;
                }
                else
                {
                    Application.Exit();
                }
            }
            return res;
        }

        public List<GetGridKeranjangModel> GetGridKeranjangFoodCourtReprint(string ItemKeranjang)
        {
            ulang:
            var res = new List<GetGridKeranjangModel>();
            try
            {
                conn.ConnectionString = server;
                using (var connection = conn)
                {
                    connection.Open();
                    string sql = "exec SP_GetGridKeranjangFoodCourtReprint " +
                        "@ItemKeranjang=" + ItemKeranjang + "";
                    using (var command = new SqlCommand(sql, connection))
                    {
                        command.CommandTimeout = 0;
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var d = new GetGridKeranjangModel();
                                d.HargaSatuan = ConvertToRupiah(ConvertDecimal(reader["Harga"].ToString()));
                                d.NamaItem = reader["NamaItem"].ToString();
                                d.Qty = reader["Qtx"].ToString();
                                d.Total = ConvertToRupiah(ConvertDecimal(reader["Total"].ToString()));
                                res.Add(d);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                var result = messageboxError(ex.Message);
                if (result == DialogResult.Retry)
                {
                    goto ulang;
                }
                else
                {
                    Application.Exit();
                }
            }
            return res;
        }

        public string GetDataTransaksiTopup(AllTransaksiModel data)
        {
            ulang:
            try
            {
                conn.ConnectionString = server;
                using (var connection = conn)
                {
                    connection.Open();
                    string sql = "exec SP_GetDataTransaksiTopup ";
                    using (var command = new SqlCommand(sql, connection))
                    {
                        command.CommandTimeout = 0;
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {

                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                var result = messageboxError(ex.Message);
                if (result == DialogResult.Retry)
                {
                    goto ulang;
                }
                else
                {
                    Application.Exit();
                }
            }
            return "";
        }
        public string GetDataTransaksiRefund(AllTransaksiModel data)
        {
            ulang:
            try
            {
                conn.ConnectionString = server;
                using (var connection = conn)
                {
                    connection.Open();
                    string sql = "exec SP_GetDataTransaksiRefund ";
                    using (var command = new SqlCommand(sql, connection))
                    {
                        command.CommandTimeout = 0;
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {

                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                var result = messageboxError(ex.Message);
                if (result == DialogResult.Retry)
                {
                    goto ulang;
                }
                else
                {
                    Application.Exit();
                }
            }
            return "";
        }

        public string GetDataTransaksiFoodcourt(AllTransaksiModel data)
        {
            ulang:
            try
            {
                conn.ConnectionString = server;
                using (var connection = conn)
                {
                    connection.Open();
                    string sql = "exec SP_GetDataTransaksiRefund ";
                    using (var command = new SqlCommand(sql, connection))
                    {
                        command.CommandTimeout = 0;
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {

                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                var result = messageboxError(ex.Message);
                if (result == DialogResult.Retry)
                {
                    goto ulang;
                }
                else
                {
                    Application.Exit();
                }
            }
            return "";
        }

        public decimal GetJaminan()
        {
            ulang:
            decimal data = 0;
            try
            {
                conn.ConnectionString = server;
                using (var connection = conn)
                {
                    connection.Open();
                    string sql = "exec SP_GetJaminan";
                    using (var command = new SqlCommand(sql, connection))
                    {
                        command.CommandTimeout = 0;
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var d = new Ticket();
                                if (reader["Harga"].ToString() != "")
                                {
                                    data = Convert.ToDecimal(reader["Harga"].ToString());
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                var result = messageboxError(ex.Message);
                if (result == DialogResult.Retry)
                {
                    goto ulang;
                }
                else
                {
                    Application.Exit();
                }
            }
            return data;
        }

        public string GetDatetime()
        {
            ulang:
            string data = "";
            try
            {
                conn.ConnectionString = server;
                using (var connection = conn)
                {
                    connection.Open();
                    string sql = "exec SP_GetDateTime ";
                    using (var command = new SqlCommand(sql, connection))
                    {
                        command.CommandTimeout = 0;
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                data = reader["tanggal"].ToString();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                var result = messageboxError(ex.Message);
                if (result == DialogResult.Retry)
                {
                    goto ulang;
                }
                else
                {
                    Application.Exit();
                }
            }
            return data;
        }

        public string GenCodeID()
        {
            ulang:
            string data = "";
            try
            {
                conn.ConnectionString = server;
                using (var connection = conn)
                {
                    connection.Open();
                    string sql = "exec SP_GenCodeID ";
                    using (var command = new SqlCommand(sql, connection))
                    {
                        command.CommandTimeout = 0;
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                data = reader["tanggal"].ToString();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                var result = messageboxError(ex.Message);
                if (result == DialogResult.Retry)
                {
                    goto ulang;
                }
                else
                {
                    Application.Exit();
                }
            }
            return data;
        }

        public List<string> GetFooterPrint()
        {
            ulang:
            var data = new List<string>();
            try
            {
                conn.ConnectionString = server;
                using (var connection = conn)
                {
                    connection.Open();
                    string sql = "exec SP_GetFooterPrint ";
                    using (var command = new SqlCommand(sql, connection))
                    {
                        command.CommandTimeout = 0;
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                string d = reader["val3"].ToString();
                                data.Add(d);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                var result = messageboxError(ex.Message);
                if (result == DialogResult.Retry)
                {
                    goto ulang;
                }
                else
                {
                    Application.Exit();
                }
            }
            return data;
        }

        public string GetFooterPrint(int line)
        {
            ulang:
            string d = "";
            try
            {
                conn.ConnectionString = server;
                using (var connection = conn)
                {
                    connection.Open();
                    string sql = "exec SP_GetHeaderPrint '"+line+"'";
                    using (var command = new SqlCommand(sql, connection))
                    {
                        command.CommandTimeout = 0;
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                d = reader["val3"].ToString();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                var result = messageboxError(ex.Message);
                if (result == DialogResult.Retry)
                {
                    goto ulang;
                }
                else
                {
                    Application.Exit();
                }
            }
            return d;
        }

        public List<DataPromo> GetPromo(string search)
        {
            ulang:
            var data = new List<DataPromo>();
            try
            {
                conn.ConnectionString = server;
                using (var connection = conn)
                {
                    connection.Open();
                    string sql = "exec SP_GetPromo '"+search+"'";
                    using (var command = new SqlCommand(sql, connection))
                    {
                        command.CommandTimeout = 0;
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                //idPromo NamaPromo   CategoryPromo Diskon  Status BerlakuDari BerlakuSampai
                                var d = new DataPromo();
                                d.idPromo = reader["idPromo"].ToString();
                                d.NamaPromo = reader["NamaPromo"].ToString();
                                d.CatPromo = reader["CategoryPromo"].ToString();
                                d.Diskon = reader["Diskon"].ToString();
                                data.Add(d);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                var result = messageboxError(ex.Message);
                if (result == DialogResult.Retry)
                {
                    goto ulang;
                }
                else
                {
                    Application.Exit();
                }
            }
            return data;
        }

        public List<KeranjangPosTotal> GetLogFoodCourt(string search, string ComputerName)
        {
            ulang:
            var res = new List<KeranjangPosTotal>();
            try
            {
                conn.ConnectionString = server;
                using (var connection = conn)
                {
                    connection.Open();
                    string sql = "exec SP_GETFoodCourtSalesLog " +
                        "@Search='" + search + "'," +
                        "@ComputerName='"+ ComputerName + "'";
                    using (var command = new SqlCommand(sql, connection))
                    {
                        command.CommandTimeout = 0;
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var d = new KeranjangPosTotal();
                                d.NamaTenant = reader["NamaTenant"].ToString();
                                d.NamaItem = reader["NamaItem"].ToString();
                                d.Qtx = ConvertDecimal(reader["Qty"].ToString());
                                d.HargaTotal = ConvertDecimal(reader["Total"].ToString());
                                d.Stok = ConvertDecimal(reader["Stok"].ToString());
                                d.HargaSatuan = ConvertDecimal(reader["Harga"].ToString());
                                res.Add(d);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                var result = messageboxError(ex.Message);
                if (result == DialogResult.Retry)
                {
                    goto ulang;
                }
                else
                {
                    Application.Exit();
                }
            }
            return res;
        }

        public string ConvertToRupiah(decimal Nominal)
        {
            string res = "Rp " + string.Format("{0:n0}", Nominal);
            return res;
        }

        public void RefreshDashboard()
        {
            string ComputerName = GetComputerName();
            string NamaUser = GetNamaUser(General.IDUser);
            var data = GetDashboard(ComputerName,NamaUser);
            Form frm = Application.OpenForms["Main"];
            if (frm != null)
            {
                Panel tbx = frm.Controls.Find("PagePanel", true).FirstOrDefault() as Panel;
                UserControl fc = tbx.Controls.Find("Dashboard", true).FirstOrDefault() as UserControl;
                if (fc != null)
                {
                    TextBox TxtTotalTopup = fc.Controls.Find("TxtTotalTopup", true).FirstOrDefault() as TextBox;
                    TextBox TxtTotalRefund = fc.Controls.Find("TxtTotalRefund", true).FirstOrDefault() as TextBox;
                    TextBox TxtRegisTotal = fc.Controls.Find("TxtRegisTotal", true).FirstOrDefault() as TextBox;
                    TextBox TxtFoodCourt = fc.Controls.Find("TxtFoodCourt", true).FirstOrDefault() as TextBox;
                    TextBox txtDanaModal = fc.Controls.Find("txtDanaModal", true).FirstOrDefault() as TextBox;
                    TextBox txtTotalCashin = fc.Controls.Find("txtTotalCashin", true).FirstOrDefault() as TextBox;
                    TextBox txtTotalCashOut = fc.Controls.Find("txtTotalCashOut", true).FirstOrDefault() as TextBox;
                    TextBox txtTotalCashBox = fc.Controls.Find("txtTotalCashBox", true).FirstOrDefault() as TextBox;

                    TextBox txtTotalEDC = fc.Controls.Find("txtTotalEDC", true).FirstOrDefault() as TextBox;
                    TextBox txtTotalEmoney = fc.Controls.Find("txtTotalEmoney", true).FirstOrDefault() as TextBox;

                    TextBox txtAllTiket = fc.Controls.Find("txtAllTiket", true).FirstOrDefault() as TextBox;

                    TextBox txtRegistCount = fc.Controls.Find("txtRegistCount", true).FirstOrDefault() as TextBox;
                    TextBox txtRefundCount = fc.Controls.Find("txtRefundCount", true).FirstOrDefault() as TextBox;

                    Button btnClosing = fc.Controls.Find("btnClosing", true).FirstOrDefault() as Button;

                    DataGridView dt_grid = fc.Controls.Find("dt_grid", true).FirstOrDefault() as DataGridView;
                    DataGridView dt_grid2 = fc.Controls.Find("dt_grid2", true).FirstOrDefault() as DataGridView;
                    if (TxtTotalTopup != null)
                    {
                        decimal TotalTopup = ConvertDecimal(data.TotalTopup);
                        TxtTotalTopup.Text = ConvertToRupiah(TotalTopup);
                    }
                    if (TxtTotalRefund != null)
                    {
                        decimal TotalRefund = ConvertDecimal(data.TotalRefund);
                        TxtTotalRefund.Text = ConvertToRupiah(TotalRefund);
                    }
                    if (TxtRegisTotal != null)
                    {
                        decimal RegisTotal = ConvertDecimal(data.TotalRegis);
                        TxtRegisTotal.Text = ConvertToRupiah(RegisTotal);
                    }
                    if (TxtFoodCourt != null)
                    {
                        decimal FoodCourt = ConvertDecimal(data.TotalFoodcourtCash)+ConvertDecimal(data.TotalFoodcourtEmoney);
                        TxtFoodCourt.Text = ConvertToRupiah(FoodCourt);
                    }
                    if (txtDanaModal != null)
                    {
                        decimal DanaModal = ConvertDecimal(data.TotalDanaModal);
                        txtDanaModal.Text = ConvertToRupiah(DanaModal);
                    }
                    if (txtTotalCashin != null)
                    {
                        decimal TotalCashin = ConvertDecimal(data.TotalCashIn);
                        txtTotalCashin.Text = ConvertToRupiah((TotalCashin - ConvertDecimal(data.TotalDanaModal)));
                    }
                    if (txtTotalCashOut != null)
                    {
                        decimal TotalCashOut = ConvertDecimal(data.TotalCashOut);
                        txtTotalCashOut.Text = ConvertToRupiah(TotalCashOut);
                    }

                    if (txtTotalCashBox != null)
                    {
                        if (data.TotalCashBox.Contains('-') == false)
                        {
                            decimal TotalCashBox = ConvertDecimal(data.TotalCashBox);
                            txtTotalCashBox.Text = ConvertToRupiah(TotalCashBox);
                        }
                        else
                        {
                            decimal TotalCashBox = ConvertDecimal(data.TotalCashBox.Replace("-",""));
                            txtTotalCashBox.Text = "- "+ConvertToRupiah(TotalCashBox);
                        }
                    }

                    if (txtTotalEDC != null)
                    {
                        txtTotalEDC.Text = ConvertToRupiah(ConvertDecimal(data.TotalNominalDebit));
                    }

                    if (txtTotalEmoney != null)
                    {
                        txtTotalEmoney.Text = ConvertToRupiah(ConvertDecimal(data.TotalNominalDebitEmoney));
                    }

                    if (txtAllTiket != null)
                    {
                        decimal TotalAllTicket = ConvertDecimal(data.TotalAllTicket);
                        txtAllTiket.Text = "" + string.Format("{0:n0}", TotalAllTicket);
                    }

                    if (txtRegistCount != null)
                    {
                        decimal TotalRegistCount = ConvertDecimal(data.TotalRegistCount);
                        txtRegistCount.Text = "" + TotalRegistCount.ToString();
                    }

                    if (txtRefundCount != null)
                    {
                        decimal TotalRefundCount = ConvertDecimal(data.TotalRefundCount);
                        txtRefundCount.Text = "" + TotalRefundCount.ToString();

                    }

                    if (dt_grid != null)
                    {
                        loadGrid(dt_grid);
                    }

                    if (dt_grid2 != null)
                    {
                        loadGrid2(dt_grid2);
                    }

                    if (btnClosing != null)
                    {
                        var Open = CheckOpenCashier();
                        if (Open.Success == true)
                        {
                            btnClosing.Enabled = true;
                        }
                        else
                        {
                            btnClosing.Enabled = false;
                        }
                    }
                }
            }
        }

        public void loadGrid(DataGridView dt_grid)
        {
            dt_grid.Rows.Clear();
            atur_grid(dt_grid);
            var data = GetDashTicketCount(GetComputerName(),GetNamaUser(General.IDUser));
            int a = 0;
            foreach (var r in data)
            {
                a++;
                string[] row = new string[] { r.JenisTicket, r.Count};
                dt_grid.Rows.Add(row);
            }
        }

        public void loadGrid2(DataGridView dt_grid)
        {
            dt_grid.Rows.Clear();
            atur_grid2(dt_grid);
            var data = GetDataAllTransaksi(GetComputerName(),GetNamaUser(General.IDUser));
            int a = 0;
            foreach (var r in data)
            {
                a++;
                string[] row = new string[] {"x", r.IdTrx,r.Datetime, r.JenisTransaksi, ConvertToRupiah(r.Nominal),r.CashierBy };
                dt_grid.Rows.Add(row);
            }
        }

        public void atur_grid2(DataGridView dt_grid)
        {
            dt_grid.Rows.Clear();
            dt_grid.ColumnCount = 6;
            dt_grid.Columns[0].Name = "X";
            dt_grid.Columns[1].Name = "Id Trx";
            dt_grid.Columns[2].Name = "Datetime";
            dt_grid.Columns[3].Name = "Nama Transaksi";
            dt_grid.Columns[4].Name = "Total Belanja";
            dt_grid.Columns[5].Name = "Cashier by";

            dt_grid.RowHeadersVisible = false;
            dt_grid.ColumnHeadersVisible = true;
            DataGridViewColumn column1 = dt_grid.Columns[0];
            DataGridViewColumn column2 = dt_grid.Columns[1];
            DataGridViewColumn column3 = dt_grid.Columns[2];
            DataGridViewColumn column4 = dt_grid.Columns[3];
            DataGridViewColumn column5 = dt_grid.Columns[4];
            DataGridViewColumn column6 = dt_grid.Columns[5];

            // Initialize basic DataGridView properties.
            dt_grid.Dock = DockStyle.Fill;
            dt_grid.BorderStyle = BorderStyle.None;
            dt_grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dt_grid.ScrollBars = ScrollBars.Both;
            dt_grid.Columns[4].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dt_grid.DefaultCellStyle.Font = new Font("Tahoma", 9);
            dt_grid.DefaultCellStyle.ForeColor = Color.Black;
            dt_grid.Columns[0].Width = 40;
        }

        public void atur_grid(DataGridView dt_grid)
        {
            dt_grid.Rows.Clear();
            dt_grid.ColumnCount = 2;
            dt_grid.Columns[0].Name = "Nama Ticket";
            dt_grid.Columns[1].Name = "Total";

            dt_grid.RowHeadersVisible = false;
            dt_grid.ColumnHeadersVisible = true;
            DataGridViewColumn column1 = dt_grid.Columns[0];
            DataGridViewColumn column2 = dt_grid.Columns[1];

            // Initialize basic DataGridView properties.
            dt_grid.Dock = DockStyle.Fill;
            dt_grid.BorderStyle = BorderStyle.None;
            dt_grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dt_grid.RowsDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dt_grid.DefaultCellStyle.Font = new Font("Tahoma", 14);
            dt_grid.DefaultCellStyle.ForeColor = Color.Black;
            dt_grid.Columns[0].Width = 200;
        }

        public string GetTotalTopup(string ComputerName)
        {
            ulang:
            string Res = "";
            try
            {
                conn.ConnectionString = server;
                using (var connection = conn)
                {
                    connection.Open();
                    string sql = "exec SP_GetDashTotalToup '" + ComputerName+"'";
                    using (var command = new SqlCommand(sql, connection))
                    {
                        command.CommandTimeout = 0;
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Res = reader["TotalTopup"].ToString();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                var result = messageboxError(ex.Message);
                if (result == DialogResult.Retry)
                {
                    goto ulang;
                }
                else
                {
                    Application.Exit();
                }
            }
            return Res;
        }

        public string GetTotalRefund(string ComputerName)
        {
            ulang:
            string Res = "";
            try
            {
                conn.ConnectionString = server;
                using (var connection = conn)
                {
                    connection.Open();
                    string sql = "exec SP_GetDashTotalRefund '" + ComputerName + "'";
                    using (var command = new SqlCommand(sql, connection))
                    {
                        command.CommandTimeout = 0;
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Res = reader["TotalRefund"].ToString();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                var result = messageboxError(ex.Message);
                if (result == DialogResult.Retry)
                {
                    goto ulang;
                }
                else
                {
                    Application.Exit();
                }
            }
            return Res;
        }

        public List<DataDashTicket> GetDashTicketCount(string ComputerName, string NamaUser)
        {
            ulang:
            var data = new List<DataDashTicket>();
            try
            {
                conn.ConnectionString = server;
                using (var connection = conn)
                {
                    connection.Open();
                    string sql = "exec SP_GetDashTicketCount '" + ComputerName + "','"+NamaUser+"'";
                    using (var command = new SqlCommand(sql, connection))
                    {
                        command.CommandTimeout = 0;
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var ls = new DataDashTicket();
                                ls.JenisTicket = reader["NamaTicket"].ToString();
                                ls.Count = reader["Qty"].ToString();
                                data.Add(ls);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                var result = messageboxError(ex.Message);
                if (result == DialogResult.Retry)
                {
                    goto ulang;
                }
                else
                {
                    Application.Exit();
                }
            }
            return data;
        }

        public DashboardModel GetDashboard(string computername,string Username)
        {
            ulang:
            var res = new DashboardModel();
            try
            {
                conn.ConnectionString = server;
                using (var connection = conn)
                {
                    connection.Open();
                    string sql = "exec SP_Dashboard @ComputerName='"+computername+"',@Username='"+Username+"' ";
                    using (var command = new SqlCommand(sql, connection))
                    {
                        command.CommandTimeout = 0;
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                res.TotalTopup = reader["TotalTopup"].ToString();
                                res.TotalRefund = reader["TotalRefund"].ToString();
                                res.TotalRegis = reader["TotalRegis"].ToString();
                                res.TotalFoodcourtCash = reader["TotalFoodcourtCash"].ToString();
                                res.TotalFoodcourtEmoney = reader["TotalFoodcourtEmoney"].ToString();
                                res.TotalTicketPayEmoney = reader["TotalTicketPayEmoney"].ToString();
                                res.TotalTransaksi = reader["TotalTransaksi"].ToString();

                                res.TotalDanaModal = reader["TotalDanaModal"].ToString();
                                res.TotalCashIn = reader["TotalCashIn"].ToString();
                                res.TotalCashOut = reader["TotalCashOut"].ToString();
                                res.TotalCashBox = reader["TotalCashBox"].ToString();
                                res.TotalAllTicket = reader["TotalAllTicket"].ToString();

                                res.TotalNominalEdcRegis = reader["TotalNominalEdcRegis"].ToString();
                                res.TotalNominalEdcTopup = reader["TotalNominalEdcTopup"].ToString();
                                res.TotalTrxEdc = reader["TotalTrxEDC"].ToString();
                                res.TotalNominalDebit = reader["TotalNominalDebit"].ToString();


                                res.TotalTrxEmoney = reader["TotalTrxEmoney"].ToString();
                                res.TotalNominalDebitEmoney = reader["TotalNominalDebitEmoney"].ToString();

                                res.TotalRegistCount = reader["TotalRegistCount"].ToString();
                                res.TotalRefundCount = reader["TotalRefundCount"].ToString();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                var result = messageboxError(ex.Message);
                if (result == DialogResult.Retry)
                {
                    goto ulang;
                }
                else
                {
                    Application.Exit();
                }
            }
            return res;
        }

        public List<DataBarang> GetBarang(string Tenant)
        {
            ulang:
            var data = new List<DataBarang>();
            try
            {
                conn.ConnectionString = server;
                using (var connection = conn)
                {
                    connection.Open();
                    string sql = "";
                    if (Tenant == "PERSEWAAN")
                    {
                        sql = "exec SP_GetListSewabyName";
                    }
                    else
                    {
                       sql = "exec SP_GetBarang " + Tenant;
                    }
                     
                    using (var command = new SqlCommand(sql, connection))
                    {
                        command.CommandTimeout = 0;
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                //idPromo NamaPromo   CategoryPromo Diskon  Status BerlakuDari BerlakuSampai
                                var d = new DataBarang();
                                d.IdMenu = reader["idMenu"].ToString();
                                d.NamaBarang = reader["NamaMenu"].ToString();
                                d.Harga = ConvertDecimal(reader["HargaJual"].ToString());
                                d.LinkPic = ImgPath+reader["ImgLink"].ToString();
                                d.Stok = ConvertDecimal(reader["Stok"].ToString());   
                                data.Add(d);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                var result = messageboxError(ex.Message);
                if (result == DialogResult.Retry)
                {
                    goto ulang;
                }
                else
                {
                    Application.Exit();
                }
            }
            return data;
        }

        public List<LogHistoryAccModel> GetHistoryAcc(string AccountNumber)
        {
            ulang:
            var data = new List<LogHistoryAccModel>();
            try
            {
                conn.ConnectionString = server;
                using (var connection = conn)
                {
                    connection.Open();
                    string sql = "exec SP_GetHistoryAcc @AccountNumber='" + AccountNumber+"'";
                    using (var command = new SqlCommand(sql, connection))
                    {
                        command.CommandTimeout = 0;
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var d = new LogHistoryAccModel();
                                d.idlog = reader["idlog"].ToString();
                                d.Datetime = reader["Datetime"].ToString();
                                d.JenisTransaksi = reader["JenisTransaksi"].ToString();
                                d.Uraian = reader["Uraian"].ToString();
                                d.Nominal = ConvertToRupiah(ConvertDecimal(reader["Nominal"].ToString()));
                                data.Add(d);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                var result = messageboxError(ex.Message);
                if (result == DialogResult.Retry)
                {
                    goto ulang;
                }
                else
                {
                    Application.Exit();
                }
            }
            return data;
        }

        public List<DataCashback> GetCashbacks(string search)
        {
            ulang:
            var data = new List<DataCashback>();
            try
            {
                conn.ConnectionString = server;
                using (var connection = conn)
                {
                    connection.Open();
                    string sql = "exec SP_GetCashback '" + search + "'";
                    using (var command = new SqlCommand(sql, connection))
                    {
                        command.CommandTimeout = 0;
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                //idPromo NamaPromo   CategoryPromo Diskon  Status BerlakuDari BerlakuSampai
                                var d = new DataCashback();
                                d.id = reader["IdCashback"].ToString();
                                d.NamaCashback = reader["NamaCashback"].ToString();
                                if (reader["NominalCashback"].ToString() != "")
                                {
                                    d.Nominal = Convert.ToDecimal(reader["NominalCashback"].ToString());
                                }
                                data.Add(d);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                var result = messageboxError(ex.Message);
                if (result == DialogResult.Retry)
                {
                    goto ulang;
                }
                else
                {
                    Application.Exit();
                }
            }
            return data;
        }

        public List<DataBank> GetDataBank(string search)
        {
            ulang:
            var data = new List<DataBank>();
            try
            {
                conn.ConnectionString = server;
                using (var connection = conn)
                {
                    connection.Open();
                    string sql = "exec SP_GetDataBank '" + search + "'";
                    using (var command = new SqlCommand(sql, connection))
                    {
                        command.CommandTimeout = 0;
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var d = new DataBank();
                                d.idLog = reader["idLog"].ToString();
                                d.AdminCharges = reader["AdminCharges"].ToString();
                                d.DiskonBank = reader["DiskonBank"].ToString();
                                d.KodeBank = reader["KodeBank"].ToString();
                                d.NamaBank = reader["NamaBank"].ToString();
                                d.status = reader["status"].ToString();
                                data.Add(d);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                var result = messageboxError(ex.Message);
                if (result == DialogResult.Retry)
                {
                    goto ulang;
                }
                else
                {
                    Application.Exit();
                }
            }
            return data;
        }

        public List<DataBank> GetLogAccount(string search)
        {
            ulang:
            var data = new List<DataBank>();
            try
            {
                conn.ConnectionString = server;
                using (var connection = conn)
                {
                    connection.Open();
                    string sql = "exec SP_GetDataBank '" + search + "'";
                    using (var command = new SqlCommand(sql, connection))
                    {
                        command.CommandTimeout = 0;
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var d = new DataBank();
                                d.idLog = reader["idLog"].ToString();
                                d.AdminCharges = reader["AdminCharges"].ToString();
                                d.DiskonBank = reader["DiskonBank"].ToString();
                                d.KodeBank = reader["KodeBank"].ToString();
                                d.NamaBank = reader["NamaBank"].ToString();
                                d.status = reader["status"].ToString();
                                data.Add(d);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                var result = messageboxError(ex.Message);
                if (result == DialogResult.Retry)
                {
                    goto ulang;
                }
                else
                {
                    Application.Exit();
                }
            }
            return data;
        }

        public List<StockOpnameModel> GetDataStok(string search)
        {
            ulang:
            var data = new List<StockOpnameModel>();
            try
            {
                conn.ConnectionString = server;
                using (var connection = conn)
                {
                    connection.Open();
                    string sql = "exec SP_GetDataStok '" + search + "'";
                    using (var command = new SqlCommand(sql, connection))
                    {
                        command.CommandTimeout = 0;
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var d = new StockOpnameModel();
                                d.idItem = reader["idMenu"].ToString();
                                d.NamaTenant = reader["NamaTenant"].ToString();
                                d.NamaItem = reader["NamaMenu"].ToString();
                                d.BykStok = ConvertDecimal(reader["Stok"].ToString());
                                data.Add(d);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                var result = messageboxError(ex.Message);
                if (result == DialogResult.Retry)
                {
                    goto ulang;
                }
                else
                {
                    Application.Exit();
                }
            }
            return data;
        }

        public List<AllTransaksiModel> GetDataAllTransaksi(string ComputerName,string NamaUser)
        {
            ulang:
            var data = new List<AllTransaksiModel>();
            try
            {
                conn.ConnectionString = server;
                using (var connection = conn)
                {
                    connection.Open();
                    string sql = "exec SP_GetDataAllTransaksi @ComputerName='" + ComputerName + "',@NamaUser='"+NamaUser+"'";
                    using (var command = new SqlCommand(sql, connection))
                    {
                        command.CommandTimeout = 0;
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var d = new AllTransaksiModel();
                                d.IdTrx = reader["IdTrx"].ToString();
                                d.Datetime = reader["Datetime"].ToString();
                                d.JenisTransaksi = reader["JenisTransaksi"].ToString();
                                d.Nominal = ConvertDecimal(reader["Nominal"].ToString());
                                d.CashierBy = reader["CashierBy"].ToString();
                                data.Add(d);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                var result = messageboxError(ex.Message);
                if (result == DialogResult.Retry)
                {
                    goto ulang;
                }
                else
                {
                    Application.Exit();
                }
            }
            return data;
        }

        public List<TambahModalCashbox> GetDanaModalLog(string ComputerName,string NamaUser)
        {
            ulang:
            var data = new List<TambahModalCashbox>();
            try
            {
                conn.ConnectionString = server;
                using (var connection = conn)
                {
                    connection.Open();
                    string sql = "exec SP_GetDanaModalLog @ComputerName='"+ ComputerName + "',@NamaUser='"+NamaUser+"'";
                    using (var command = new SqlCommand(sql, connection))
                    {
                        command.CommandTimeout = 0;
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var d = new TambahModalCashbox();
                                d.Id = reader["idLog"].ToString();
                                d.ComputerName = reader["NamaComputer"].ToString();
                                d.NamaUser = reader["NamaUser"].ToString();
                                d.Nominal = ConvertDecimal(reader["NominalTambahModal"].ToString());
                                d.Datetime = reader["Datetime"].ToString();
                                data.Add(d);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                var result = messageboxError(ex.Message);
                if (result == DialogResult.Retry)
                {
                    goto ulang;
                }
                else
                {
                    Application.Exit();
                }
            }
            return data;
        }

        private async Task<List<Ticket>> GetTicket2(string URI)
        {
            var data = new List<Ticket>();
            try
            {
                using (var client = new HttpClient())
                {
                    using (var response = await client.GetAsync(URI))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            var productJsonString = await response.Content.ReadAsStringAsync();
                            data = JsonConvert.DeserializeObject<List<Ticket>>(productJsonString);
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return data;
        }

        public DialogResult messageboxError(string content)
        {
            string message = "Do you want to abort this operation? \n"+ content;
            string title = "Close Window";
            MessageBoxButtons buttons = MessageBoxButtons.RetryCancel;
            DialogResult result = MessageBox.Show(message, title, buttons, MessageBoxIcon.Warning);
            return result;
        }

        public DialogResult ShowMessagebox(string message,string title,MessageBoxButtons btn)
        {
            DialogResult result = MessageBox.Show(message, title, btn, MessageBoxIcon.Warning);
            return result;
        }

        public string ReadUpdateAccountData(DataAccount data)
        {
            ulang:
            string result ="";
            try
            {
                conn.ConnectionString = server;
                using (var connection = conn)
                {
                    connection.Open();
                    string sql = "exec SP_UpdateAccountDataRead " +
                        "@AccountNumber='" + data.AccountNumber+ "'," +
                        "@Balanced=" + data.BalancedSesudah+ "," +
                        "@Ticket=" + data.Ticket+"," +
                        "@JaminanGelang ="+data.JaminanGelangYgTerbaca;
                    using (var command = new SqlCommand(sql, connection))
                    {
                        command.CommandTimeout = 0;
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                result = reader["Result"].ToString();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                result = ex.Message;
                var msg = messageboxError(ex.Message);
                if (msg == DialogResult.Retry)
                {
                    goto ulang;
                }
            }
            return result;
        }
    }

    public class ReaderFunction
    {
        public int retCode, hContext, hCard, Protocol;
        public bool connActive = false;
        public bool autoDet;
        public byte[] SendBuff = new byte[263];
        public byte[] RecvBuff = new byte[263];
        public int SendLen, RecvLen, nBytesRet, reqType, Aprotocol, dwProtocol, cbPciLength;
        public card_function.Card.SCARD_READERSTATE RdrState;
        public card_function.Card.SCARD_IO_REQUEST pioSendRequest;

        public void ClearBuffers()
        {

            long indx;

            for (indx = 0; indx <= 262; indx++)
            {

                RecvBuff[indx] = 0;
                SendBuff[indx] = 0;

            }

        }

        public ReturnResult Authenticate(string BlokNum,string KeyVal)
        {
            var data = new ReturnResult();
            int indx;
            string tmpStr = "";

            ClearBuffers();
            if (int.Parse(BlokNum) > 319)
            {
                BlokNum = "319";
            }

            if (int.Parse(KeyVal) > 1)
            {
                KeyVal = "1";
            }
            SendBuff[0] = 0xFF;                             // Class
            SendBuff[1] = 0x86;                             // INS
            SendBuff[2] = 0x00;                             // P1
            SendBuff[3] = 0x00;                             // P2
            SendBuff[4] = 0x05;                             // Lc
            SendBuff[5] = 0x01;                             // Byte 1 : Version number
            SendBuff[6] = 0x00;                             // Byte 2
            SendBuff[7] = (byte)int.Parse(BlokNum);     // Byte 3 : Block number
            SendBuff[8] = 0x60;

            SendBuff[9] = byte.Parse(KeyVal, System.Globalization.NumberStyles.HexNumber);        // Key 5 value

            SendLen = 10;
            RecvLen = 2;

            retCode = SendAPDU().RetCode;

            if (retCode != card_function.Card.SCARD_S_SUCCESS)
            {
                data.Success = false;
                data.Message = "Authenticate Failed";
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
                data.Success = true;
                data.Message = "Authenticate Success";
            }
            else
            {
                data.Success = false;
                data.Message = "Authenticate Failed";
            }

            return data;
        }
        public ReturnResult ReadBlock(string BinBlk)
        {
            var data = new ReturnResult();
            string tmpStr = "";
            int indx;
            ClearBuffers();
            SendBuff[0] = 0xFF;
            SendBuff[1] = 0xB0;
            SendBuff[2] = 0x00;
            SendBuff[3] = (byte)int.Parse(BinBlk);
            SendBuff[4] = (byte)int.Parse("16");

            SendLen = 5;
            RecvLen = SendBuff[4] + 2;

            retCode = SendAPDU().RetCode;

            if (retCode != card_function.Card.SCARD_S_SUCCESS)
            {
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

                data.Success = true;
                data.Message = tmpStr;
            }
            else
            {
                data.Success = false;
                data.Message = "Read block error!";
            }
            return data;
        }
        public ReturnResult SendAPDU()
        {
            var data = new ReturnResult();
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

            retCode = card_function.Card.SCardTransmit(hCard, ref pioSendRequest, ref SendBuff[0], SendLen, ref pioSendRequest, ref RecvBuff[0], ref RecvLen);

            if (retCode != card_function.Card.SCARD_S_SUCCESS)
            {
                data.Success = false;
                
            }
            else
            {
                data.Success = true;
            }

            tmpStr = "";
            for (indx = 0; indx <= RecvLen - 1; indx++)
            {

                tmpStr = tmpStr + " " + string.Format("{0:X2}", RecvBuff[indx]);

            }
            data.Message = tmpStr;
            data.RetCode = retCode;
            return data;

        }
    }

    public class RawPrinterHelper
    {
        // Structure and API declarions:
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        public class DOCINFOA
        {
            [MarshalAs(UnmanagedType.LPStr)]
            public string pDocName;
            [MarshalAs(UnmanagedType.LPStr)]
            public string pOutputFile;
            [MarshalAs(UnmanagedType.LPStr)]
            public string pDataType;
        }
        [DllImport("winspool.Drv", EntryPoint = "OpenPrinterA", SetLastError = true, CharSet = CharSet.Unicode, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern bool OpenPrinter([MarshalAs(UnmanagedType.LPStr)] string szPrinter, out IntPtr hPrinter, IntPtr pd);

        [DllImport("winspool.Drv", EntryPoint = "ClosePrinter", SetLastError = true, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern bool ClosePrinter(IntPtr hPrinter);

        [DllImport("winspool.Drv", EntryPoint = "StartDocPrinterA", SetLastError = true, CharSet = CharSet.Unicode, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern bool StartDocPrinter(IntPtr hPrinter, Int32 level, [In, MarshalAs(UnmanagedType.LPStruct)] DOCINFOA di);

        [DllImport("winspool.Drv", EntryPoint = "EndDocPrinter", SetLastError = true, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern bool EndDocPrinter(IntPtr hPrinter);

        [DllImport("winspool.Drv", EntryPoint = "StartPagePrinter", SetLastError = true, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern bool StartPagePrinter(IntPtr hPrinter);

        [DllImport("winspool.Drv", EntryPoint = "EndPagePrinter", SetLastError = true, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern bool EndPagePrinter(IntPtr hPrinter);

        [DllImport("winspool.Drv", EntryPoint = "WritePrinter", SetLastError = true, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern bool WritePrinter(IntPtr hPrinter, IntPtr pBytes, Int32 dwCount, out Int32 dwWritten);

        // SendBytesToPrinter()
        // When the function is given a printer name and an unmanaged array
        // of bytes, the function sends those bytes to the print queue.
        // Returns true on success, false on failure.
        public static bool SendBytesToPrinter(string szPrinterName, IntPtr pBytes, Int32 dwCount)
        {
            Int32 dwError = 0, dwWritten = 0;
            IntPtr hPrinter = new IntPtr(0);
            DOCINFOA di = new DOCINFOA();
            bool bSuccess = false; // Assume failure unless you specifically succeed.

            di.pDocName = "RAW Document";
            // Win7
            di.pDataType = "RAW";

            // Win8+
            // di.pDataType = "XPS_PASS";

            // Open the printer.
            if (OpenPrinter(szPrinterName.Normalize(), out hPrinter, IntPtr.Zero))
            {
                // Start a document.
                if (StartDocPrinter(hPrinter, 1, di))
                {
                    // Start a page.
                    if (StartPagePrinter(hPrinter))
                    {
                        // Write your bytes.
                        bSuccess = WritePrinter(hPrinter, pBytes, dwCount, out dwWritten);
                        EndPagePrinter(hPrinter);
                    }
                    EndDocPrinter(hPrinter);
                }
                ClosePrinter(hPrinter);
            }
            // If you did not succeed, GetLastError may give more information
            // about why not.
            if (bSuccess == false)
            {
                dwError = Marshal.GetLastWin32Error();
            }
            return bSuccess;
        }

        public static bool SendFileToPrinter(string szPrinterName, string szFileName)
        {
            // Open the file.
            FileStream fs = new FileStream(szFileName, FileMode.Open);
            // Create a BinaryReader on the file.
            BinaryReader br = new BinaryReader(fs);
            // Dim an array of bytes big enough to hold the file's contents.
            Byte[] bytes = new Byte[fs.Length];
            bool bSuccess = false;
            // Your unmanaged pointer.
            IntPtr pUnmanagedBytes = new IntPtr(0);
            int nLength;

            nLength = Convert.ToInt32(fs.Length);
            // Read the contents of the file into the array.
            bytes = br.ReadBytes(nLength);
            // Allocate some unmanaged memory for those bytes.
            pUnmanagedBytes = Marshal.AllocCoTaskMem(nLength);
            // Copy the managed byte array into the unmanaged array.
            Marshal.Copy(bytes, 0, pUnmanagedBytes, nLength);
            // Send the unmanaged bytes to the printer.
            bSuccess = SendBytesToPrinter(szPrinterName, pUnmanagedBytes, nLength);
            // Free the unmanaged memory that you allocated earlier.
            Marshal.FreeCoTaskMem(pUnmanagedBytes);
            fs.Close();
            fs.Dispose();
            fs = null;
            return bSuccess;
        }

        public static bool SendStringToPrinter(string szPrinterName, string szString)
        {
            IntPtr pBytes;
            Int32 dwCount;
            // How many characters are in the string?
            dwCount = szString.Length;
            // Assume that the printer is expecting ANSI text, and then convert
            // the string to ANSI text.
            pBytes = Marshal.StringToCoTaskMemAnsi(szString);
            // Send the converted ANSI string to the printer.
            SendBytesToPrinter(szPrinterName, pBytes, dwCount);
            Marshal.FreeCoTaskMem(pBytes);
            return true;
        }
    }

    public class ReadFromFile
    {
        public List<string> readfile(string path)
        {
            string[] lines = System.IO.File.ReadAllLines(path);
            foreach (string line in lines)
            {
                // Use a tab to indent each line of the file.
                Console.WriteLine("\t" + line);
            }
            return lines.ToList();
        }
        public void lineChanger(string newText, string fileName, int line_to_edit)
        {
            var arrLine = new List<string>(File.ReadAllLines(fileName));
            arrLine[line_to_edit - 1] = newText;
            File.WriteAllLines(fileName, arrLine);
        }
        public bool CheckFileKey()
        {
            bool res = false;
            var appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            var path0 = Path.Combine(appDataPath, @"Tentakel\");
            if (!Directory.Exists(path0))
            {
                Directory.CreateDirectory(path0);
            }
            string curFile = Path.Combine(path0, @"keyEwats.ttl");
            if (File.Exists(curFile))
            {
                res = true;
            }
            return res;
        }
        public bool CreateFileKey(string Key)
        {
            bool res = false;
            try
            {
                var appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                var path0 = Path.Combine(appDataPath, @"Tentakel\");
                string curFile = Path.Combine(path0, @"keyEwats.ttl");
                using (StreamWriter writer = new StreamWriter(curFile))
                {
                    string line1 = Key;
                    writer.Write(Encrypt.EncryptString(line1, "BISMILLAH"));
                }
                res = true;
            }
            catch (Exception ex)
            {
                res = false;
                Console.WriteLine(ex.Message);
            }
            return res;
        }
        public string ReadFileKey()
        {
            string res = "";
            StringCipher Cripto = new StringCipher();
            var appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            var path0 = Path.Combine(appDataPath, @"Tentakel\");
            string curFile = Path.Combine(path0, @"keyEwats.ttl");
            var lines = File.ReadAllLines(curFile);
            if (lines.Length > 0)
            {
                res = lines[0];
            }
            return res;
        }

        public bool CheckFileConfig()
        {
            bool res = false;
            var appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            var path0 = Path.Combine(appDataPath, @"Tentakel\");
            if (!Directory.Exists(path0))
            {
                Directory.CreateDirectory(path0);
            }
            string curFile = Path.Combine(path0, @"configEwats.ttl");
            if (File.Exists(curFile))
            {
                res = true;
            }
            return res;
        }
        public ConfigurationFile ReadFileConfig()
        {
            var res = new ConfigurationFile();
            StringCipher Cripto = new StringCipher();
            var appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            var path0 = Path.Combine(appDataPath, @"Tentakel\");
            string curFile = Path.Combine(path0, @"configEwats.ttl");
            var lines = File.ReadAllLines(curFile);
            if (lines.Length > 0)
            {
                res.ConnStrLog = lines[0];
                res.IpServer = lines[1];
                res.DBServer = lines[2];
                res.UsernameServer = lines[3];
                res.PasswordServer = lines[4];
                res.PathImgWeb = lines[5];
                if (lines.Length > 6)
                {
                    if (lines[6] != "")
                    {
                        res.VFDPort = lines[6];
                    }
                }
                
            }
            return res;
        }
        public bool CreateFileConfig(ConfigurationFile data)
        {
            bool res = false;
            try
            {
                var appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                var path0 = Path.Combine(appDataPath, @"Tentakel\");
                string curFile = Path.Combine(path0, @"configEwats.ttl");

                using (StreamWriter writer = new StreamWriter(curFile))
                {
                    string line4 = data.ConnStrLog;
                    writer.WriteLine(Encrypt.EncryptString(line4, "BISMILLAH"));
                    string line5 = data.IpServer;
                    writer.WriteLine(Encrypt.EncryptString(line5, "BISMILLAH"));

                    string line6 = data.DBServer;
                    writer.WriteLine(Encrypt.EncryptString(line6, "BISMILLAH"));

                    string line7 = data.UsernameServer;
                    writer.WriteLine(Encrypt.EncryptString(line7, "BISMILLAH"));

                    string line8 = data.PasswordServer;
                    writer.WriteLine(Encrypt.EncryptString(line8, "BISMILLAH"));

                    string line9 = data.PathImgWeb;
                    writer.WriteLine(Encrypt.EncryptString(line9, "BISMILLAH"));

                    string line10 = data.VFDPort;
                    writer.WriteLine(Encrypt.EncryptString(line10, "BISMILLAH"));
                }

                res = true;
            }
            catch (Exception ex)
            {
                res = false;
                Console.WriteLine(ex.Message);
            }
            return res;
        }
    }

    public class StringCipher
    {
        public string key = "TENTAKEL GROUP";
        private const int Keysize = 256;
        private const int DerivationIterations = 1000;

        public string Encrypt(string plainText, string passPhrase)
        {
            var saltStringBytes = Generate256BitsOfRandomEntropy();
            var ivStringBytes = Generate256BitsOfRandomEntropy();
            var plainTextBytes = Encoding.UTF8.GetBytes(plainText);
            using (var password = new Rfc2898DeriveBytes(passPhrase, saltStringBytes, DerivationIterations))
            {
                var keyBytes = password.GetBytes(Keysize / 8);
                using (var symmetricKey = new RijndaelManaged())
                {
                    symmetricKey.BlockSize = 256;
                    symmetricKey.Mode = CipherMode.CBC;
                    symmetricKey.Padding = PaddingMode.PKCS7;
                    using (var encryptor = symmetricKey.CreateEncryptor(keyBytes, ivStringBytes))
                    {
                        using (var memoryStream = new MemoryStream())
                        {
                            using (var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                            {
                                cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
                                cryptoStream.FlushFinalBlock();
                                // Create the final bytes as a concatenation of the random salt bytes, the random iv bytes and the cipher bytes.
                                var cipherTextBytes = saltStringBytes;
                                cipherTextBytes = cipherTextBytes.Concat(ivStringBytes).ToArray();
                                cipherTextBytes = cipherTextBytes.Concat(memoryStream.ToArray()).ToArray();
                                memoryStream.Close();
                                cryptoStream.Close();
                                return Convert.ToBase64String(cipherTextBytes);
                            }
                        }
                    }
                }
            }
        }

        public string Decrypt(string cipherText, string passPhrase)
        {
            // Get the complete stream of bytes that represent:
            // [32 bytes of Salt] + [32 bytes of IV] + [n bytes of CipherText]
            var cipherTextBytesWithSaltAndIv = Convert.FromBase64String(cipherText);
            // Get the saltbytes by extracting the first 32 bytes from the supplied cipherText bytes.
            var saltStringBytes = cipherTextBytesWithSaltAndIv.Take(Keysize / 8).ToArray();
            // Get the IV bytes by extracting the next 32 bytes from the supplied cipherText bytes.
            var ivStringBytes = cipherTextBytesWithSaltAndIv.Skip(Keysize / 8).Take(Keysize / 8).ToArray();
            // Get the actual cipher text bytes by removing the first 64 bytes from the cipherText string.
            var cipherTextBytes = cipherTextBytesWithSaltAndIv.Skip((Keysize / 8) * 2).Take(cipherTextBytesWithSaltAndIv.Length - ((Keysize / 8) * 2)).ToArray();

            using (var password = new Rfc2898DeriveBytes(passPhrase, saltStringBytes, DerivationIterations))
            {
                var keyBytes = password.GetBytes(Keysize / 8);
                using (var symmetricKey = new RijndaelManaged())
                {
                    symmetricKey.BlockSize = 256;
                    symmetricKey.Mode = CipherMode.CBC;
                    symmetricKey.Padding = PaddingMode.PKCS7;
                    using (var decryptor = symmetricKey.CreateDecryptor(keyBytes, ivStringBytes))
                    {
                        using (var memoryStream = new MemoryStream(cipherTextBytes))
                        {
                            using (var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
                            {
                                var plainTextBytes = new byte[cipherTextBytes.Length];
                                var decryptedByteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);
                                memoryStream.Close();
                                cryptoStream.Close();
                                return Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount);
                            }
                        }
                    }
                }
            }
        }

        private static byte[] Generate256BitsOfRandomEntropy()
        {
            var randomBytes = new byte[32]; // 32 Bytes will give us 256 bits.
            using (var rngCsp = new RNGCryptoServiceProvider())
            {
                // Fill the array with cryptographically secure random bytes.
                rngCsp.GetBytes(randomBytes);
            }
            return randomBytes;
        }
    }
}
