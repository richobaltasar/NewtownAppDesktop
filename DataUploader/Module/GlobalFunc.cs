using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using DataUploader.Model;
using System.Configuration;
using System.IO;
using System.Security.Cryptography;

namespace DataUploader.Module
{
    public class GlobalFunc
    {
        public SqlConnection conn = new SqlConnection();
        public SqlCommand cmd = new SqlCommand();

        public decimal ConvertDecimal(string data)
        {
            decimal res = 0;
            if (data != "" && data != null)
            {
                string filter = data.Replace("Rp", "").Replace(",", "").Replace(".", "").Replace("\0", "").Replace("\n", "").Trim();
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

        public string GetConnStringCloudDB()
        {
            string res = "";
            try
            {
                conn.ConnectionString = ConfigurationFileStatic.ConnStrLog;
                using (var connection = conn)
                {
                    connection.Open();
                    string sql = "";
                    sql = "exec SP_GetConnStringCloudDB ";
                    using (var command = new SqlCommand(sql, connection))
                    {
                        command.CommandTimeout = 0;
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                string ServerName = reader["ServerName"].ToString();
                                string Username = reader["Username"].ToString();
                                string Password = reader["Password"].ToString();
                                string DBName = reader["DBName"].ToString();
                                res = @"Data Source = "+ServerName+"; Initial Catalog = "+DBName+"; User ID = "+ Username + "; Password = "+Password;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return res;
        }
        public UploadData GetDataYangMauDiUpload()
        {
            var res = new UploadData();
            try
            {
                res = FGetUploadData();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error :"+ex.Message);
            }
            return res;
        }

        #region Upload Data
        public UploadData FGetUploadData()
        {
            var res = new UploadData();
            res.LogCashierTambahModal = getLogCashierTambahModal();
            res.LogClosing = getLogClosing();
            res.LogDeposit = getLogDeposit();
            res.LogEDCTransaksi = getLogEDCTransaksi();
            res.LogEmoneyTrxAccount = getLogEmoneyTrxAccount();
            res.LogFoodcourtTransaksi = getLogFoodcourtTransaksi();
            res.LogItemsFBTrx = getLogItemsFBTrx();
            res.LogRefundDetail = getLogRefundDetail();
            res.LogRegistrasiDetail = getLogRegistrasiDetail();
            res.LogSetoranDepositExpired = getLogSetoranDepositExpired();
            res.LogStokOpname = getLogStokOpname();
            res.LogTicketDetail = getLogTicketDetail();
            res.LogTopupDetail = getLogTopupDetail();
            return res;
        }

        //Sudah dibuat SP nya
        public List<LogCashierTambahModal> getLogCashierTambahModal()
        {
            var ls = new List<LogCashierTambahModal>();
            try
            {
                conn.ConnectionString = ConfigurationFileStatic.ConnStrLog;
                using (var connection = conn)
                {
                    connection.Open();
                    string sql = "";
                    sql = "exec SP_getLogCashierTambahModal ";
                    using (var command = new SqlCommand(sql, connection))
                    {
                        command.CommandTimeout = 0;
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var data = new LogCashierTambahModal();
                                data.idLog = reader["idLog"].ToString();
                                data.Datetime = reader["Datetime"].ToString();
                                data.NamaComputer = reader["NamaComputer"].ToString();
                                data.NamaUser = reader["NamaUser"].ToString();
                                data.NominalTambahModal = reader["NominalTambahModal"].ToString();
                                data.Status = reader["Status"].ToString();
                                ls.Add(data);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return ls;
        }
        public List<LogClosing> getLogClosing()
        {
            var ls = new List<LogClosing>();
            try
            {
                conn.ConnectionString = ConfigurationFileStatic.ConnStrLog;
                using (var connection = conn)
                {
                    connection.Open();
                    string sql = "exec SP_getLogClosingUpload ";
                    using (var command = new SqlCommand(sql, connection))
                    {
                        command.CommandTimeout = 0;
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var data = new LogClosing();
                                data.IdLog = reader["IdLog"].ToString();
                                data.Datetime = reader["Datetime"].ToString();
                                data.NamaComputer = reader["NamaComputer"].ToString();
                                data.NamaUser = reader["NamaUser"].ToString();
                                data.TotalAllTicket = reader["TotalAllTicket"].ToString();
                                data.TotalTransaksi = reader["TotalTransaksi"].ToString();
                                data.TotalTopup = reader["TotalTopup"].ToString();
                                data.TotalRegis = reader["TotalRegis"].ToString();
                                data.TotalRefund = reader["TotalRefund"].ToString();
                                data.TotalFoodcourt = reader["TotalFoodcourt"].ToString();
                                data.TotalDanaModal = reader["TotalDanaModal"].ToString();
                                data.TotalCashOut = reader["TotalCashOut"].ToString();
                                data.TotalCashIn = reader["TotalCashIn"].ToString();
                                data.TotalCashBox = reader["TotalCashBox"].ToString();
                                data.TotalCashirInputMoneyCashbox = reader["TotalCashirInputMoneyCashbox"].ToString();
                                data.MinusIndikasiMoneyCashBox = reader["MinusIndikasiMoneyCashBox"].ToString();
                                data.MatchingSucces = reader["MatchingSucces"].ToString();
                                data.StatusAcceptanceBySPV = reader["StatusAcceptanceBySPV"].ToString();
                                data.KeteranganAcceptance = reader["KeteranganAcceptance"].ToString();
                                data.UangDiterimaFinnance = reader["UangDiterimaFinnance"].ToString();
                                data.TotalTrxEdc = reader["TotalTrxEdc"].ToString();
                                data.TotalNominalDebit = reader["TotalNominalDebit"].ToString();
                                data.TotalTrxEmoney = reader["TotalTrxEmoney"].ToString();
                                data.TotalNominalDebitEmoney = reader["TotalNominalDebitEmoney"].ToString();
                                data.Status = reader["Status"].ToString();
                                ls.Add(data);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return ls;
        }
        public List<LogDeposit> getLogDeposit()
        {
            var ls = new List<LogDeposit>();
            try
            {
                conn.ConnectionString = ConfigurationFileStatic.ConnStrLog;
                using (var connection = conn)
                {
                    connection.Open();
                    string sql = "";
                    sql = "exec SP_getLogDeposit ";
                    using (var command = new SqlCommand(sql, connection))
                    {
                        command.CommandTimeout = 0;
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var data = new LogDeposit();
                                data.LogId = reader["LogId"].ToString();
                                data.Datetime = reader["Datetime"].ToString();
                                data.AccountNumber = reader["AccountNumber"].ToString();
                                data.TransactionType = reader["TransactionType"].ToString();
                                data.Nominal = reader["Nominal"].ToString();
                                data.Status = reader["Status"].ToString();
                                ls.Add(data);
                            }
                        }
                    }


                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return ls;
        }
        public List<LogEDCTransaksi> getLogEDCTransaksi()
        {
            var ls = new List<LogEDCTransaksi>();
            try
            {
                conn.ConnectionString = ConfigurationFileStatic.ConnStrLog;
                using (var connection = conn)
                {
                    connection.Open();
                    string sql = "";
                    sql = "exec SP_getLogEDCTransaksi ";
                    using (var command = new SqlCommand(sql, connection))
                    {
                        command.CommandTimeout = 0;
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var data = new LogEDCTransaksi();
                                data.IdLog = reader["IdLog"].ToString();
                                data.Datetime = reader["Datetime"].ToString();
                                data.TotalBelanja = reader["TotalBelanja"].ToString();
                                data.CodeBank = reader["CodeBank"].ToString();
                                data.NamaBank = reader["NamaBank"].ToString();
                                data.DiskonBank = reader["DiskonBank"].ToString();
                                data.NominalDiskon = reader["NominalDiskon"].ToString();
                                data.AdminCharges = reader["AdminCharges"].ToString();
                                data.TotalDebit = reader["TotalDebit"].ToString();
                                data.NoATM = reader["NoATM"].ToString();
                                data.NoReffEddPrint = reader["NoReffEddPrint"].ToString();
                                data.ComputerName = reader["ComputerName"].ToString();
                                data.CashierBy = reader["CashierBy"].ToString();
                                data.status = reader["status"].ToString();
                                ls.Add(data);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return ls;
        }
        public List<LogEmoneyTrxAccount> getLogEmoneyTrxAccount()
        {
            var ls = new List<LogEmoneyTrxAccount>();
            try
            {
                conn.ConnectionString = ConfigurationFileStatic.ConnStrLog;
                using (var connection = conn)
                {
                    connection.Open();
                    string sql = "";
                    sql = "exec SP_getLogEmoneyTrxAccount ";
                    using (var command = new SqlCommand(sql, connection))
                    {
                        command.CommandTimeout = 0;
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var data = new LogEmoneyTrxAccount();
                                data.IdLog = reader["IdLog"].ToString();
                                data.AcountNumber = reader["AcountNumber"].ToString();
                                data.Datetime = reader["Datetime"].ToString();
                                data.SaldoSebelumnya = reader["SaldoSebelumnya"].ToString();
                                data.Credit = reader["Credit"].ToString();
                                data.Debit = reader["Debit"].ToString();
                                data.SisaSaldo = reader["SisaSaldo"].ToString();
                                data.Status = reader["Status"].ToString();
                                ls.Add(data);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return ls;
        }
        public List<LogFoodcourtTransaksi> getLogFoodcourtTransaksi()
        {
            var ls = new List<LogFoodcourtTransaksi>();
            try
            {
                conn.ConnectionString = ConfigurationFileStatic.ConnStrLog;
                using (var connection = conn)
                {
                    connection.Open();
                    string sql = "exec SP_getLogFoodcourtTransaksi ";
                    using (var command = new SqlCommand(sql, connection))
                    {
                        command.CommandTimeout = 0;
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var data = new LogFoodcourtTransaksi();
                                data.IdTrx = reader["IdTrx"].ToString();
                                data.Datetime = reader["Datetime"].ToString();
                                data.AccountNumber = reader["AccountNumber"].ToString();
                                data.SaldoEmoney = reader["SaldoEmoney"].ToString();
                                data.SaldoEmoneyAfter = reader["SaldoEmoneyAfter"].ToString();
                                data.IdItemsKeranjang = reader["IdItemsKeranjang"].ToString();
                                data.JenisTransaksi = reader["JenisTransaksi"].ToString();
                                data.TotalBayar = reader["TotalBayar"].ToString();
                                data.PayEmoney = reader["PayEmoney"].ToString();
                                data.PayCash = reader["PayCash"].ToString();
                                data.TerimaUang = reader["TerimaUang"].ToString();
                                data.Kembalian = reader["Kembalian"].ToString();
                                data.ComputerName = reader["ComputerName"].ToString();
                                data.CashierBy = reader["CashierBy"].ToString();
                                data.Status = reader["Status"].ToString();
                                ls.Add(data);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return ls;
        }
        public List<LogItemsFBTrx> getLogItemsFBTrx()
        {
            var ls = new List<LogItemsFBTrx>();
            try
            {
                conn.ConnectionString = ConfigurationFileStatic.ConnStrLog;
                using (var connection = conn)
                {
                    connection.Open();
                    string sql = "exec SP_getLogItemsFBTrx ";
                    using (var command = new SqlCommand(sql, connection))
                    {
                        command.CommandTimeout = 0;
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var data = new LogItemsFBTrx();
                                data.IdItemsKeranjang = reader["IdItemsKeranjang"].ToString();
                                data.Datetime = reader["Datetime"].ToString();
                                data.NamaTenant = reader["NamaTenant"].ToString();
                                data.KodeBarang = reader["KodeBarang"].ToString();
                                data.NamaItem = reader["NamaItem"].ToString();
                                data.Harga = reader["Harga"].ToString();
                                data.Qtx = reader["Qtx"].ToString();
                                data.Total = reader["Total"].ToString();
                                data.Status = reader["Status"].ToString();
                                data.Chasierby = reader["Chasierby"].ToString();
                                data.ComputerName = reader["ComputerName"].ToString();
                                data.AccountNumber = reader["AccountNumber"].ToString();
                                ls.Add(data);
                            }
                        }
                    }


                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return ls;
        }
        public List<LogRefundDetail> getLogRefundDetail()
        {
            var ls = new List<LogRefundDetail>();
            try
            {
                conn.ConnectionString = ConfigurationFileStatic.ConnStrLog;
                using (var connection = conn)
                {
                    connection.Open();
                    string sql = "exec SP_LogRefundDetail ";
                    using (var command = new SqlCommand(sql, connection))
                    {
                        command.CommandTimeout = 0;
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var data = new LogRefundDetail();
                                data.IdRefund = reader["IdRefund"].ToString();
                                data.Datetime = reader["Datetime"].ToString();
                                data.AccountNumber = reader["AccountNumber"].ToString();
                                data.SaldoEmoney = reader["SaldoEmoney"].ToString();
                                data.SaldoJaminan = reader["SaldoJaminan"].ToString();
                                data.TicketWeekDay = reader["TicketWeekDay"].ToString();
                                data.TicketWeekEnd = reader["TicketWeekEnd"].ToString();
                                data.TotalNominalRefund = reader["TotalNominalRefund"].ToString();
                                data.ChasierBy = reader["ChasierBy"].ToString();
                                data.ComputerName = reader["ComputerName"].ToString();
                                data.Status = reader["Status"].ToString();
                                ls.Add(data);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return ls;
        }
        public List<LogRegistrasiDetail> getLogRegistrasiDetail()
        {
            var ls = new List<LogRegistrasiDetail>();
            try
            {
                conn.ConnectionString = ConfigurationFileStatic.ConnStrLog;
                using (var connection = conn)
                {
                    connection.Open();
                    string sql = "exec SP_getLogRegistrasiDetail ";
                    using (var command = new SqlCommand(sql, connection))
                    {
                        command.CommandTimeout = 0;
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var data = new LogRegistrasiDetail();
                                data.idTrx = reader["idTrx"].ToString();
                                data.Datetime = reader["Datetime"].ToString();
                                data.AccountNumber = reader["AccountNumber"].ToString();
                                data.SaldoEmoney = reader["SaldoEmoney"].ToString();
                                data.SaldoEmoneyAfter = reader["SaldoEmoneyAfter"].ToString();
                                data.TicketWeekDay = reader["TicketWeekDay"].ToString();
                                data.TicketWeekDayAfter = reader["TicketWeekDayAfter"].ToString();
                                data.TicketWeekEnd = reader["TicketWeekEnd"].ToString();
                                data.TicketWeekEndAfter = reader["TicketWeekEndAfter"].ToString();
                                data.SaldoJaminan = reader["SaldoJaminan"].ToString();
                                data.SaldoJaminanAfter = reader["SaldoJaminanAfter"].ToString();
                                data.IdTicketTrx = reader["IdTicketTrx"].ToString();
                                data.Cashback = reader["Cashback"].ToString();
                                data.Topup = reader["Topup"].ToString();
                                data.Asuransi = reader["Asuransi"].ToString();
                                data.QtyTotalTiket = reader["QtyTotalTiket"].ToString();
                                data.TotalBeliTiket = reader["TotalBeliTiket"].ToString();
                                data.TotalAll = reader["TotalAll"].ToString();
                                data.JenisTransaksi = reader["JenisTransaksi"].ToString();
                                data.TotalBayar = reader["TotalBayar"].ToString();
                                data.PayEmoney = reader["PayEmoney"].ToString();
                                data.PayCash = reader["PayCash"].ToString();
                                data.TerimaUang = reader["TerimaUang"].ToString();
                                data.Kembalian = reader["Kembalian"].ToString();
                                data.CashierBy = reader["CashierBy"].ToString();
                                data.ComputerName = reader["ComputerName"].ToString();
                                data.status = reader["status"].ToString();
                                data.IdLogEDCTransaksi = reader["IdLogEDCTransaksi"].ToString();
                                data.BankCode = reader["BankCode"].ToString();
                                data.NamaBank = reader["NamaBank"].ToString();
                                data.DiskonBank = reader["DiskonBank"].ToString();
                                data.NominalDiskon = reader["NominalDiskon"].ToString();
                                data.AdminCharges = reader["AdminCharges"].ToString();
                                data.TotalDebit = reader["TotalDebit"].ToString();

                                ls.Add(data);

                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return ls;
        }
        public List<LogSetoranDepositExpired> getLogSetoranDepositExpired()
        {
            var ls = new List<LogSetoranDepositExpired>();
            try
            {
                conn.ConnectionString = ConfigurationFileStatic.ConnStrLog;
                using (var connection = conn)
                {
                    connection.Open();
                    string sql = "exec SP_UploadLogSetoranDepositExpired";
                    using (var command = new SqlCommand(sql, connection))
                    {
                        command.CommandTimeout = 0;
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var data = new LogSetoranDepositExpired();
                                data.LogId = reader["LogId"].ToString();
                                data.Datetime = reader["Datetime"].ToString();
                                data.AccountNumber = reader["AccountNumber"].ToString();
                                data.Saldo = reader["Saldo"].ToString();
                                data.UangJaminan = reader["UangJaminan"].ToString();
                                data.TotalDeposit = reader["TotalDeposit"].ToString();
                                data.TanggalExpired = reader["TanggalExpired"].ToString();
                                data.NamaPenyetor = reader["NamaPenyetor"].ToString();
                                data.TanggalSetor = reader["TanggalSetor"].ToString();
                                data.StatusSetor = reader["StatusSetor"].ToString();
                                ls.Add(data);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return ls;
        }
        public List<LogStokOpname> getLogStokOpname()
        {
            var ls = new List<LogStokOpname>();
            try
            {
                conn.ConnectionString = ConfigurationFileStatic.ConnStrLog;
                using (var connection = conn)
                {
                    connection.Open();
                    string sql = "exec SP_getLogStokOpname ";
                    using (var command = new SqlCommand(sql, connection))
                    {
                        command.CommandTimeout = 0;
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var data = new LogStokOpname();
                                data.idLog = reader["idLog"].ToString();
                                data.Datetime = reader["Datetime"].ToString();
                                data.NamaTenant = reader["NamaTenant"].ToString();
                                data.NamaItem = reader["NamaItem"].ToString();
                                data.StockSebelumnya = reader["StockSebelumnya"].ToString();
                                data.StockUpdate = reader["StockUpdate"].ToString();
                                data.UpdateBy = reader["UpdateBy"].ToString();
                                data.Status = reader["Status"].ToString();
                                ls.Add(data);
                            }
                        }
                    }


                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return ls;
        }
        public List<LogTicketDetail> getLogTicketDetail()
        {
            var ls = new List<LogTicketDetail>();
            try
            {
                conn.ConnectionString = ConfigurationFileStatic.ConnStrLog;
                using (var connection = conn)
                {
                    connection.Open();
                    string sql = "exec SP_getLogTicketDetail";

                    using (var command = new SqlCommand(sql, connection))
                    {
                        command.CommandTimeout = 0;
                        using (var reader = command.ExecuteReader())
                        {

                            while (reader.Read())
                            {
                                var data = new LogTicketDetail();
                                data.Datetime = reader["Datetime"].ToString();
                                data.IdTicket = reader["IdTicket"].ToString();
                                data.AccountNumber = reader["AccountNumber"].ToString();
                                data.NamaTicket = reader["NamaTicket"].ToString();
                                data.Harga = reader["Harga"].ToString();
                                data.Qty = reader["Qty"].ToString();
                                data.Total = reader["Total"].ToString();
                                data.IdDiskon = reader["IdDiskon"].ToString();
                                data.NamaDiskon = reader["NamaDiskon"].ToString();
                                data.Diskon = reader["Diskon"].ToString();
                                data.TotalDiskon = reader["TotalDiskon"].ToString();
                                data.TotalAfterDiskon = reader["TotalAfterDiskon"].ToString();
                                data.Status = reader["Status"].ToString();
                                data.ChasierBy = reader["ChasierBy"].ToString();
                                data.ComputerName = reader["ComputerName"].ToString();
                                ls.Add(data);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return ls;
        }
        public List<LogTopupDetail> getLogTopupDetail()
        {
            var ls = new List<LogTopupDetail>();
            try
            {
                conn.ConnectionString = ConfigurationFileStatic.ConnStrLog;
                using (var connection = conn)
                {
                    connection.Open();
                    string sql = "exec SP_getLogTopupDetail";

                    using (var command = new SqlCommand(sql, connection))
                    {
                        command.CommandTimeout = 0;
                        using (var reader = command.ExecuteReader())
                        {
                            
                            while (reader.Read())
                            {
                                var data = new LogTopupDetail();
                                data.AccountNumber = reader["AccountNumber"].ToString();
                                data.AdminCharges = reader["AdminCharges"].ToString();
                                data.BankCode = reader["BankCode"].ToString();
                                data.Chasierby = reader["Chasierby"].ToString();
                                data.ComputerName = reader["ComputerName"].ToString();
                                data.Datetime = reader["Datetime"].ToString();
                                data.DiskonBank = reader["DiskonBank"].ToString();
                                data.IdLogEDCTransaksi = reader["IdLogEDCTransaksi"].ToString();
                                data.IdTopup = reader["IdTopup"].ToString();
                                data.JenisPayment = reader["JenisPayment"].ToString();
                                data.Kembalian = reader["Kembalian"].ToString();
                                data.NamaBank = reader["NamaBank"].ToString();
                                data.NominalDiskon = reader["NominalDiskon"].ToString();
                                data.NominalTopup = reader["NominalTopup"].ToString();
                                data.PayCash = reader["PayCash"].ToString();
                                data.PaymentMethod = reader["PaymentMethod"].ToString();
                                data.SaldoSebelum = reader["SaldoSebelum"].ToString();
                                data.SaldoSetelah = reader["SaldoSetelah"].ToString();
                                data.Status = reader["Status"].ToString();
                                data.TerimaUang = reader["TerimaUang"].ToString();
                                data.TotalBayar = reader["TotalBayar"].ToString();
                                data.TotalDebit = reader["TotalDebit"].ToString();
                                ls.Add(data);
                            }
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return ls;
        }

        #region Upload Data cloud
        public ResponseUploadData FSaveUploadData(UploadData data)
        {
            var res = new ResponseUploadData();
            res.LogCashierTambahModal = getLogCashierTambahModal(data.LogCashierTambahModal);
            res.LogClosing = getLogClosing(data.LogClosing);
            res.LogDeposit = getLogDeposit(data.LogDeposit);
            res.LogEDCTransaksi = getLogEDCTransaksi(data.LogEDCTransaksi);
            res.LogEmoneyTrxAccount = getLogEmoneyTrxAccount(data.LogEmoneyTrxAccount);
            res.LogFoodcourtTransaksi = getLogFoodcourtTransaksi(data.LogFoodcourtTransaksi);
            res.LogItemsFBTrx = getLogItemsFBTrx(data.LogItemsFBTrx);
            res.LogRefundDetail = getLogRefundDetail(data.LogRefundDetail);
            res.LogRegistrasiDetail = getLogRegistrasiDetail(data.LogRegistrasiDetail);
            res.LogSetoranDepositExpired = getLogSetoranDepositExpired(data.LogSetoranDepositExpired);
            res.LogStokOpname = getLogStokOpname(data.LogStokOpname);
            res.LogTicketDetail = getLogTicketDetail(data.LogTicketDetail);
            res.LogTopupDetail = getLogTopupDetail(data.LogTopupDetail);
            return res;
        }
        public List<string> getLogCashierTambahModal(List<LogCashierTambahModal> data)
        {
            var ls = new List<string>();
            try
            {
                conn.ConnectionString = ConfigurationFileStatic.ConnStrLogCloud;
                using (var connection = conn)
                {
                    connection.Open();
                    string sql = "";
                    foreach (var dt in data)
                    {
                        sql = "exec SP_ApiUploadLogCashierTambahModal " +
                            "@idLog=" + dt.idLog + "," +
                            "@Datetime='" + dt.Datetime + "'," +
                            "@NamaComputer='" + dt.NamaComputer + "'," +
                            "@NamaUser='" + dt.NamaUser + "'," +
                            "@NominalTambahModal=" + dt.NominalTambahModal + "," +
                            "@Status=" + dt.Status + "";
                        using (var command = new SqlCommand(sql, connection))
                        {
                            command.CommandTimeout = 0;
                            using (var reader = command.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    ls.Add(reader["Id"].ToString());
                                }
                            }
                        }
                    }


                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return ls;
        }

        public List<string> getLogClosing(List<LogClosing> data)
        {
        ulang:
            var ls = new List<string>();
            try
            {
                conn.ConnectionString = ConfigurationFileStatic.ConnStrLogCloud;
                using (var connection = conn)
                {
                    connection.Open();
                    foreach (var dt in data)
                    {
                        string sql = "exec SP_ApiUploadLogClosing " +
                            "@IdLog= " + dt.IdLog + "," +
                            "@Datetime= '" + dt.Datetime + "'," +
                            "@NamaComputer= '" + dt.NamaComputer + "'," +
                            "@NamaUser= '" + dt.NamaUser + "'," +
                            "@TotalAllTicket= " + ConvertDecimal(dt.TotalAllTicket) + "," +
                            "@TotalTransaksi= " + ConvertDecimal(dt.TotalTransaksi) + "," +
                            "@TotalTopup= " + ConvertDecimal(dt.TotalTopup) + "," +
                            "@TotalRegis= " + ConvertDecimal(dt.TotalRegis) + "," +
                            "@TotalRefund= " + ConvertDecimal(dt.TotalRefund) + "," +
                            "@TotalFoodcourt= " + ConvertDecimal(dt.TotalFoodcourt) + "," +
                            "@TotalDanaModal= " + ConvertDecimal(dt.TotalDanaModal) + "," +
                            "@TotalCashOut= " + ConvertDecimal(dt.TotalCashOut) + "," +
                            "@TotalCashIn= " + ConvertDecimal(dt.TotalCashIn) + "," +
                            "@TotalCashBox= " + ConvertDecimal(dt.TotalCashBox) + "," +
                            "@TotalCashirInputMoneyCashbox= " + ConvertDecimal(dt.TotalCashirInputMoneyCashbox) + "," +
                            "@MinusIndikasiMoneyCashBox= " + ConvertDecimal(dt.MinusIndikasiMoneyCashBox) + "," +
                            "@MatchingSucces= '" + dt.MatchingSucces + "'," +
                            "@StatusAcceptanceBySPV= '" + dt.StatusAcceptanceBySPV + "'," +
                            "@KeteranganAcceptance= '" + dt.KeteranganAcceptance + "'," +
                            "@UangDiterimaFinnance= " + ConvertDecimal(dt.UangDiterimaFinnance) + "," +
                            "@TotalTrxEdc= " + ConvertDecimal(dt.TotalTrxEdc) + "," +
                            "@TotalNominalDebit= " + ConvertDecimal(dt.TotalNominalDebit) + "," +
                            "@TotalTrxEmoney= " + ConvertDecimal(dt.TotalTrxEmoney) + "," +
                            "@TotalNominalDebitEmoney= " + ConvertDecimal(dt.TotalNominalDebitEmoney) + "," +
                            "@Status= " + dt.Status + "";

                        using (var command = new SqlCommand(sql, connection))
                        {
                            command.CommandTimeout = 0;
                            using (var reader = command.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    ls.Add(reader["Id"].ToString());
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error :" + ex.Message);
                goto ulang;
            }
            return ls;
        }
        public List<string> getLogDeposit(List<LogDeposit> data)
        {
            var ls = new List<string>();
            try
            {
                conn.ConnectionString = ConfigurationFileStatic.ConnStrLogCloud;
                using (var connection = conn)
                {
                    connection.Open();
                    string sql = "";
                    foreach (var dt in data)
                    {
                        sql = "exec SP_ApiUploadLogDeposit " +
                            "@LogId= " + dt.LogId + "," +
                            "@Datetime= '" + dt.Datetime + "'," +
                            "@AccountNumber= '" + dt.AccountNumber + "'," +

                            "@TransactionType= '" + dt.TransactionType + "'," +
                            "@Nominal= " + ConvertDecimal(dt.Nominal) + "," +
                            "@Status= " + dt.Status + "";

                        using (var command = new SqlCommand(sql, connection))
                        {
                            command.CommandTimeout = 0;
                            using (var reader = command.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    ls.Add(reader["Id"].ToString());
                                }
                            }
                        }
                    }


                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return ls;
        }
        public List<string> getLogEDCTransaksi(List<LogEDCTransaksi> data)
        {
            var ls = new List<string>();
            try
            {
                conn.ConnectionString = ConfigurationFileStatic.ConnStrLogCloud;
                using (var connection = conn)
                {
                    connection.Open();
                    string sql = "";
                    foreach (var dt in data)
                    {
                        sql = "exec SP_ApiUploadLogEDCTransaksi " +
                            "@IdLog= " + dt.IdLog + "," +
                            "@Datetime= '" + dt.Datetime + "'," +
                            "@TotalBelanja= " + dt.TotalBelanja + "," +
                            "@CodeBank= '" + dt.CodeBank + "'," +
                            "@NamaBank= '" + dt.NamaBank + "'," +
                            "@DiskonBank= " + ConvertDecimal(dt.DiskonBank) + "," +
                            "@NominalDiskon= " + ConvertDecimal(dt.NominalDiskon) + "," +
                            "@AdminCharges= " + ConvertDecimal(dt.AdminCharges) + "," +
                            "@TotalDebit= " + ConvertDecimal(dt.TotalDebit) + "," +
                            "@NoATM= '" + ConvertDecimal(dt.NoATM) + "'," +
                            "@NoReffEddPrint= '" + ConvertDecimal(dt.NoReffEddPrint) + "'," +
                            "@ComputerName= '" + dt.ComputerName + "'," +
                            "@CashierBy= '" + dt.CashierBy + "'," +
                            "@status= " + dt.status + "";

                        using (var command = new SqlCommand(sql, connection))
                        {
                            command.CommandTimeout = 0;
                            using (var reader = command.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    ls.Add(reader["Id"].ToString());
                                }
                            }
                        }
                    }


                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return ls;
        }
        public List<string> getLogEmoneyTrxAccount(List<LogEmoneyTrxAccount> data)
        {
            var ls = new List<string>();
            try
            {
                conn.ConnectionString = ConfigurationFileStatic.ConnStrLogCloud;
                using (var connection = conn)
                {
                    connection.Open();
                    string sql = "";
                    foreach (var dt in data)
                    {
                        sql = "exec SP_getLogEmoneyTrxAccount " +
                            "@Datetime='" + dt.Datetime + "'," +
                            "@AcountNumber='" + dt.AcountNumber + "'," +
                            "@Credit=" + ConvertDecimal(dt.Credit) + "," +
                            "@Datetime='" + dt.Datetime + "'," +
                            "@Debit=" + ConvertDecimal(dt.Debit) + "," +
                            "@IdLog=" + dt.IdLog + "," +
                            "@SaldoSebelumnya=" + ConvertDecimal(dt.SaldoSebelumnya) + "," +
                            "@SisaSaldo=" + ConvertDecimal(dt.SisaSaldo) + "," +
                            "@Status=" + dt.Status + "";
                        using (var command = new SqlCommand(sql, connection))
                        {
                            command.CommandTimeout = 0;
                            using (var reader = command.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    ls.Add(reader["Id"].ToString());
                                }
                            }
                        }
                    }


                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return ls;
        }
        public List<string> getLogFoodcourtTransaksi(List<LogFoodcourtTransaksi> data)
        {
            var ls = new List<string>();
            try
            {
                conn.ConnectionString = ConfigurationFileStatic.ConnStrLogCloud;
                using (var connection = conn)
                {
                    connection.Open();
                    string sql = "";
                    foreach (var dt in data)
                    {
                        sql = "exec SP_UploadLogFoodcourtTransaksi " +
                            "@IdTrx= " + dt.IdTrx + "," +
                            "@Datetime= '" + dt.Datetime + "'," +
                            "@AccountNumber= '" + dt.AccountNumber + "'," +
                            "@SaldoEmoney= " + dt.SaldoEmoney + "," +
                            "@SaldoEmoneyAfter= " + dt.SaldoEmoneyAfter + "," +
                            "@IdItemsKeranjang= " + dt.IdItemsKeranjang + "," +
                            "@JenisTransaksi= '" + dt.JenisTransaksi + "'," +
                            "@TotalBayar= " + dt.TotalBayar + "," +
                            "@PayEmoney= " + dt.PayEmoney + "," +
                            "@PayCash= " + dt.PayCash + "," +
                            "@TerimaUang= " + dt.TerimaUang + "," +
                            "@Kembalian= " + dt.Kembalian + "," +
                            "@ComputerName= '" + dt.ComputerName + "'," +
                            "@CashierBy= '" + dt.CashierBy + "'," +
                            "@Status= " + dt.Status + "";

                        using (var command = new SqlCommand(sql, connection))
                        {
                            command.CommandTimeout = 0;
                            using (var reader = command.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    ls.Add(reader["Id"].ToString());
                                }
                            }
                        }
                    }


                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return ls;
        }
        public List<string> getLogItemsFBTrx(List<LogItemsFBTrx> data)
        {
            var ls = new List<string>();
            try
            {
                conn.ConnectionString = ConfigurationFileStatic.ConnStrLogCloud;
                using (var connection = conn)
                {
                    connection.Open();
                    string sql = "";
                    foreach (var dt in data)
                    {
                        sql = "exec SP_UploadLogItemsFBTrx " +
                            "@IdItemsKeranjang= " + dt.IdItemsKeranjang + "," +
                            "@Datetime= '" + dt.Datetime + "'," +
                            "@NamaTenant= '" + dt.NamaTenant + "'," +
                            "@KodeBarang= " + dt.KodeBarang + "," +
                            "@NamaItem= '" + dt.NamaItem + "'," +
                            "@Harga= " + dt.Harga + "," +
                            "@Qtx= " + dt.Qtx + "," +
                            "@Total= " + dt.Total + "," +
                            "@Status= " + dt.Status + "," +
                            "@Chasierby= '" + dt.Chasierby + "'," +
                            "@ComputerName= '" + dt.ComputerName + "'," +
                            "@AccountNumber= '" + dt.AccountNumber + "'";

                        using (var command = new SqlCommand(sql, connection))
                        {
                            command.CommandTimeout = 0;
                            using (var reader = command.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    ls.Add(reader["Id"].ToString());
                                }
                            }
                        }
                    }


                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return ls;
        }
        public List<string> getLogRefundDetail(List<LogRefundDetail> data)
        {
            var ls = new List<string>();
            try
            {
                conn.ConnectionString = ConfigurationFileStatic.ConnStrLogCloud;
                using (var connection = conn)
                {
                    connection.Open();
                    string sql = "";
                    foreach (var dt in data)
                    {
                        sql = "exec SP_UploadLogRefundDetail " +
                            "@IdRefund= " + dt.IdRefund + "," +
                            "@Datetime= '" + dt.Datetime + "'," +
                            "@AccountNumber= '" + dt.AccountNumber + "'," +
                            "@SaldoEmoney= " + dt.SaldoEmoney + "," +
                            "@SaldoJaminan= " + dt.SaldoJaminan + "," +
                            "@TicketWeekDay= " + dt.TicketWeekDay + "," +
                            "@TicketWeekEnd= " + dt.TicketWeekEnd + "," +
                            "@TotalNominalRefund= " + dt.TotalNominalRefund + "," +
                            "@ChasierBy= '" + dt.ChasierBy + "'," +
                            "@ComputerName= '" + dt.ComputerName + "'," +
                            "@Status= " + dt.Status + "";

                        using (var command = new SqlCommand(sql, connection))
                        {
                            command.CommandTimeout = 0;
                            using (var reader = command.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    ls.Add(reader["Id"].ToString());
                                }
                            }
                        }
                    }


                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return ls;
        }
        public List<string> getLogRegistrasiDetail(List<LogRegistrasiDetail> data)
        {
            var ls = new List<string>();
            try
            {
                conn.ConnectionString = ConfigurationFileStatic.ConnStrLogCloud;
                using (var connection = conn)
                {
                    connection.Open();
                    string sql = "";
                    foreach (var dt in data)
                    {
                        sql = "exec SP_UploadLogRegistrasiDetail " +
                            "@idTrx= " + dt.idTrx + "," +
                            "@Datetime= '" + dt.Datetime + "'," +
                            "@AccountNumber= '" + dt.AccountNumber + "'," +
                            "@SaldoEmoney= " + ConvertDecimal(dt.SaldoEmoney) + "," +
                            "@SaldoEmoneyAfter= " + ConvertDecimal(dt.SaldoEmoneyAfter) + "," +
                            "@TicketWeekDay= " + ConvertDecimal(dt.TicketWeekDay) + "," +
                            "@TicketWeekDayAfter= " + ConvertDecimal(dt.TicketWeekDayAfter) + "," +
                            "@TicketWeekEnd= " + ConvertDecimal(dt.TicketWeekEnd) + "," +
                            "@TicketWeekEndAfter= " + ConvertDecimal(dt.TicketWeekEndAfter) + "," +
                            "@SaldoJaminan= " + ConvertDecimal(dt.SaldoJaminan) + "," +
                            "@SaldoJaminanAfter= " + ConvertDecimal(dt.SaldoJaminanAfter) + "," +
                            "@IdTicketTrx= " + ConvertDecimal(dt.IdTicketTrx) + "," +
                            "@Cashback= " + ConvertDecimal(dt.Cashback) + "," +
                            "@Topup= " + ConvertDecimal(dt.Topup) + "," +
                            "@Asuransi= " + ConvertDecimal(dt.Asuransi) + "," +
                            "@QtyTotalTiket= " + ConvertDecimal(dt.QtyTotalTiket) + "," +
                            "@TotalBeliTiket= " + ConvertDecimal(dt.TotalBeliTiket) + "," +
                            "@TotalAll= " + ConvertDecimal(dt.TotalAll) + "," +
                            "@JenisTransaksi= '" + dt.JenisTransaksi + "'," +
                            "@TotalBayar= " + ConvertDecimal(dt.TotalBayar) + "," +
                            "@PayEmoney= " + ConvertDecimal(dt.PayEmoney) + "," +
                            "@PayCash= " + ConvertDecimal(dt.PayCash) + "," +
                            "@TerimaUang= " + ConvertDecimal(dt.TerimaUang) + "," +
                            "@Kembalian= " + ConvertDecimal(dt.Kembalian) + "," +
                            "@CashierBy= '" + dt.CashierBy + "'," +
                            "@ComputerName= '" + dt.ComputerName + "'," +
                            "@status= " + dt.status + "," +
                            "@IdLogEDCTransaksi= " + ConvertDecimal(dt.IdLogEDCTransaksi) + "," +
                            "@BankCode= '" + dt.BankCode + "'," +
                            "@NamaBank= '" + dt.NamaBank + "'," +
                            "@DiskonBank= " + ConvertDecimal(dt.DiskonBank) + "," +
                            "@NominalDiskon= " + ConvertDecimal(dt.NominalDiskon) + "," +
                            "@AdminCharges= " + ConvertDecimal(dt.AdminCharges) + "," +
                            "@TotalDebit= " + ConvertDecimal(dt.TotalDebit) + "";

                        using (var command = new SqlCommand(sql, connection))
                        {
                            command.CommandTimeout = 0;
                            using (var reader = command.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    ls.Add(reader["Id"].ToString());
                                }
                            }
                        }
                    }


                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return ls;
        }
        public List<string> getLogSetoranDepositExpired(List<LogSetoranDepositExpired> data)
        {
            var ls = new List<string>();
            try
            {
                conn.ConnectionString = ConfigurationFileStatic.ConnStrLogCloud;
                using (var connection = conn)
                {
                    connection.Open();
                    string sql = "";
                    foreach (var dt in data)
                    {
                        sql = "exec SP_UploadLogSetoranDepositExpired " +
                            "@LogId= " + dt.LogId + "," +
                            "@Datetime= '" + dt.Datetime + "'," +
                            "@AccountNumber= '" + dt.AccountNumber + "'," +
                            "@Saldo= " + dt.Saldo + "," +
                            "@UangJaminan= " + dt.UangJaminan + "," +
                            "@TotalDeposit= " + dt.TotalDeposit + "," +
                            "@TanggalExpired= '" + dt.TanggalExpired + "'," +
                            "@NamaPenyetor= '" + dt.NamaPenyetor + "'," +
                            "@TanggalSetor= '" + dt.TanggalSetor + "'," +
                            "@StatusSetor= " + dt.StatusSetor + "";

                        using (var command = new SqlCommand(sql, connection))
                        {
                            command.CommandTimeout = 0;
                            using (var reader = command.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    ls.Add(reader["Id"].ToString());
                                }
                            }
                        }
                    }


                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return ls;
        }
        public List<string> getLogStokOpname(List<LogStokOpname> data)
        {
            var ls = new List<string>();
            try
            {
                conn.ConnectionString = ConfigurationFileStatic.ConnStrLogCloud;
                using (var connection = conn)
                {
                    connection.Open();
                    string sql = "";
                    foreach (var dt in data)
                    {
                        sql = "exec SP_UploadLogStokOpname " +
                            "@idLog= " + dt.idLog + "," +
                            "@Datetime= '" + dt.Datetime + "'," +
                            "@NamaTenant= '" + dt.NamaTenant + "'," +
                            "@NamaItem= '" + dt.NamaItem + "'," +
                            "@StockSebelumnya= " + dt.StockSebelumnya + "," +
                            "@StockUpdate= " + dt.StockUpdate + "," +
                            "@UpdateBy= '" + dt.UpdateBy + "'," +
                            "@Status= " + dt.Status + "";

                        using (var command = new SqlCommand(sql, connection))
                        {
                            command.CommandTimeout = 0;
                            using (var reader = command.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    ls.Add(reader["Id"].ToString());
                                }
                            }
                        }
                    }


                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return ls;
        }
        public List<string> getLogTicketDetail(List<LogTicketDetail> data)
        {
            var ls = new List<string>();
            try
            {
                conn.ConnectionString = ConfigurationFileStatic.ConnStrLogCloud;
                using (var connection = conn)
                {
                    connection.Open();
                    string sql = "";
                    foreach (var dt in data)
                    {
                        sql = "exec SP_UploadLogTicketDetail " +
                            "@Datetime= '" + dt.Datetime + "'," +
                            "@IdTicket= " + dt.IdTicket + "," +
                            "@AccountNumber= '" + dt.AccountNumber + "'," +
                            "@NamaTicket= '" + dt.NamaTicket + "'," +
                            "@Harga= " + dt.Harga + "," +
                            "@Qty= " + dt.Qty + "," +
                            "@Total= " + dt.Total + "," +
                            "@IdDiskon= " + dt.IdDiskon + "," +
                            "@NamaDiskon= '" + dt.NamaDiskon + "'," +
                            "@Diskon= " + dt.Diskon + "," +
                            "@TotalDiskon= " + dt.TotalDiskon + "," +
                            "@TotalAfterDiskon= " + dt.TotalAfterDiskon + "," +
                            "@Status= " + dt.Status + "," +
                            "@ChasierBy= '" + dt.ChasierBy + "'," +
                            "@ComputerName= '" + dt.ComputerName + "'";

                        using (var command = new SqlCommand(sql, connection))
                        {
                            command.CommandTimeout = 0;
                            using (var reader = command.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    ls.Add(reader["Id"].ToString());
                                }
                            }
                        }
                    }


                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return ls;
        }
        public List<string> getLogTopupDetail(List<LogTopupDetail> data)
        {
            var ls = new List<string>();
            try
            {
                conn.ConnectionString = ConfigurationFileStatic.ConnStrLogCloud;
                using (var connection = conn)
                {
                    connection.Open();
                    string sql = "";
                    foreach (var dt in data)
                    {
                        sql = "exec SP_UploadLogTopupDetail " +
                            "@IdTopup='" + dt.IdTopup + "'," +
                            "@Datetime='" + dt.Datetime + "'," +
                            "@JenisPayment='" + dt.JenisPayment + "'," +
                            "@AccountNumber='" + dt.AccountNumber + "'," +
                            "@NominalTopup='" + dt.NominalTopup + "'," +
                            "@TotalBayar='" + dt.TotalBayar + "'," +
                            "@PayCash='" + dt.PayCash + "'," +
                            "@TerimaUang='" + dt.TerimaUang + "'," +
                            "@Kembalian='" + dt.Kembalian + "'," +
                            "@SaldoSebelum='" + dt.SaldoSebelum + "'," +
                            "@SaldoSetelah='" + dt.SaldoSetelah + "'," +
                            "@Chasierby='" + dt.Chasierby + "'," +
                            "@ComputerName='" + dt.ComputerName + "'," +
                            "@Status='" + dt.Status + "'," +
                            "@IdLogEDCTransaksi='" + dt.IdLogEDCTransaksi + "'," +
                            "@BankCode='" + dt.BankCode + "'," +
                            "@NamaBank='" + dt.NamaBank + "'," +
                            "@DiskonBank='" + dt.DiskonBank + "'," +
                            "@NominalDiskon='" + dt.NominalDiskon + "'," +
                            "@AdminCharges='" + dt.AdminCharges + "'," +
                            "@TotalDebit='" + dt.TotalDebit + "'," +
                            "@PaymentMethod='" + dt.PaymentMethod + "'";

                        using (var command = new SqlCommand(sql, connection))
                        {
                            command.CommandTimeout = 0;
                            using (var reader = command.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    ls.Add(reader["Id"].ToString());
                                }
                            }
                        }
                    }


                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return ls;
        }
        #endregion

        #region Update ID yang sudah Upload
        public void FUpdateIDUploaded(ResponseUploadData Data)
        {
            foreach (string d in Data.LogCashierTambahModal)
            {
                ULogCashierTambahModal(d);
            }
            foreach (string d in Data.LogClosing)
            {
                ULogClosing(d);
            }
            foreach (string d in Data.LogDeposit)
            {
                ULogDeposit(d);
            }
            foreach (string d in Data.LogEDCTransaksi)
            {
                ULogEDCTransaksi(d);
            }
            foreach (string d in Data.LogEmoneyTrxAccount)
            {
                ULogEmoneyTrxAccount(d);
            }
            foreach (string d in Data.LogFoodcourtTransaksi)
            {
                ULogFoodcourtTransaksi(d);
            }
            foreach (string d in Data.LogItemsFBTrx)
            {
                ULogItemsFBTrx(d);
            }
            foreach (string d in Data.LogRefundDetail)
            {
                ULogRefundDetail(d);
            }
            foreach (string d in Data.LogRegistrasiDetail)
            {
                ULogRegistrasiDetail(d);
            }
            foreach (string d in Data.LogSetoranDepositExpired)
            {
                ULogSetoranDepositExpired(d);
            }
            foreach (string d in Data.LogStokOpname)
            {
                ULogStokOpname(d);
            }
            foreach (string d in Data.LogTicketDetail)
            {
                ULogTicketDetail(d);
            }
            foreach (string d in Data.LogTopupDetail)
            {
                ULogTopupDetail(d);
            }
        }
        public bool ULogCashierTambahModal(string Id)
        {
            bool res = false;
            try
            {
                conn.ConnectionString = ConfigurationFileStatic.ConnStrLog;
                using (var connection = conn)
                {
                    connection.Open();
                    string sql = "exec SP_ULogCashierTambahModal @Id=" + Id;

                    using (var command = new SqlCommand(sql, connection))
                    {
                        command.CommandTimeout = 0;
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                res = true;
                            }
                            res = true;
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                res = false;
            }
            return res;
        }
        public bool ULogClosing(string Id)
        {
            bool res = false;
            try
            {
                conn.ConnectionString = ConfigurationFileStatic.ConnStrLog;
                using (var connection = conn)
                {
                    connection.Open();
                    string sql = "exec SP_ULogClosing @Id=" + Id;

                    using (var command = new SqlCommand(sql, connection))
                    {
                        command.CommandTimeout = 0;
                        using (var reader = command.ExecuteReader())
                        {

                            while (reader.Read())
                            {
                                res = true;
                            }
                            res = true;
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                res = false;
            }
            return res;
        }
        public bool ULogDeposit(string Id)
        {
            bool res = false;
            try
            {
                conn.ConnectionString = ConfigurationFileStatic.ConnStrLog;
                using (var connection = conn)
                {
                    connection.Open();
                    string sql = "exec SP_ULogDeposit @Id=" + Id;

                    using (var command = new SqlCommand(sql, connection))
                    {
                        command.CommandTimeout = 0;
                        using (var reader = command.ExecuteReader())
                        {

                            while (reader.Read())
                            {
                                res = true;
                            }
                            res = true;
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                res = false;
            }
            return res;
        }
        public bool ULogEDCTransaksi(string Id)
        {
            bool res = false;
            try
            {
                conn.ConnectionString = ConfigurationFileStatic.ConnStrLog;
                using (var connection = conn)
                {
                    connection.Open();
                    string sql = "exec SP_ULogEDCTransaksi @Id=" + Id;

                    using (var command = new SqlCommand(sql, connection))
                    {
                        command.CommandTimeout = 0;
                        using (var reader = command.ExecuteReader())
                        {

                            while (reader.Read())
                            {
                                res = true;
                            }
                            res = true;
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                res = false;
            }
            return res;
        }
        public bool ULogEmoneyTrxAccount(string Id)
        {
            bool res = false;
            try
            {
                conn.ConnectionString = ConfigurationFileStatic.ConnStrLog;
                using (var connection = conn)
                {
                    connection.Open();
                    string sql = "exec SP_ULogEmoneyTrxAccount @Id=" + Id;

                    using (var command = new SqlCommand(sql, connection))
                    {
                        command.CommandTimeout = 0;
                        using (var reader = command.ExecuteReader())
                        {

                            while (reader.Read())
                            {
                                res = true;
                            }
                            res = true;
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                res = false;
            }
            return res;
        }
        public bool ULogFoodcourtTransaksi(string Id)
        {
            bool res = false;
            try
            {
                conn.ConnectionString = ConfigurationFileStatic.ConnStrLog;
                using (var connection = conn)
                {
                    connection.Open();
                    string sql = "exec SP_ULogFoodcourtTransaksi @Id=" + Id;

                    using (var command = new SqlCommand(sql, connection))
                    {
                        command.CommandTimeout = 0;
                        using (var reader = command.ExecuteReader())
                        {

                            while (reader.Read())
                            {
                                res = true;
                            }
                            res = true;
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                res = false;
            }
            return res;
        }
        public bool ULogItemsFBTrx(string Id)
        {
            bool res = false;
            try
            {
                conn.ConnectionString = ConfigurationFileStatic.ConnStrLog;
                using (var connection = conn)
                {
                    connection.Open();
                    string sql = "exec SP_ULogItemsFBTrx @Id=" + Id;

                    using (var command = new SqlCommand(sql, connection))
                    {
                        command.CommandTimeout = 0;
                        using (var reader = command.ExecuteReader())
                        {

                            while (reader.Read())
                            {
                                res = true;
                            }
                            res = true;
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                res = false;
            }
            return res;
        }
        public bool ULogRefundDetail(string Id)
        {
            bool res = false;
            try
            {
                conn.ConnectionString = ConfigurationFileStatic.ConnStrLog;
                using (var connection = conn)
                {
                    connection.Open();
                    string sql = "exec SP_ULogRefundDetail @Id=" + Id;

                    using (var command = new SqlCommand(sql, connection))
                    {
                        command.CommandTimeout = 0;
                        using (var reader = command.ExecuteReader())
                        {

                            while (reader.Read())
                            {
                                res = true;
                            }
                            res = true;
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                res = false;
            }
            return res;
        }
        public bool ULogRegistrasiDetail(string Id)
        {
            bool res = false;
            try
            {
                conn.ConnectionString = ConfigurationFileStatic.ConnStrLog;
                using (var connection = conn)
                {
                    connection.Open();
                    string sql = "exec SP_ULogRegistrasiDetail @Id=" + Id;

                    using (var command = new SqlCommand(sql, connection))
                    {
                        command.CommandTimeout = 0;
                        using (var reader = command.ExecuteReader())
                        {

                            while (reader.Read())
                            {
                                res = true;
                            }
                            res = true;
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                res = false;
            }
            return res;
        }
        public bool ULogSetoranDepositExpired(string Id)
        {
            bool res = false;
            try
            {
                conn.ConnectionString = ConfigurationFileStatic.ConnStrLog;
                using (var connection = conn)
                {
                    connection.Open();
                    string sql = "exec SP_ULogSetoranDepositExpired @Id=" + Id;

                    using (var command = new SqlCommand(sql, connection))
                    {
                        command.CommandTimeout = 0;
                        using (var reader = command.ExecuteReader())
                        {

                            while (reader.Read())
                            {
                                res = true;
                            }
                            res = true;
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                res = false;
            }
            return res;
        }
        public bool ULogStokOpname(string Id)
        {
            bool res = false;
            try
            {
                conn.ConnectionString = ConfigurationFileStatic.ConnStrLog;
                using (var connection = conn)
                {
                    connection.Open();
                    string sql = "exec SP_ULogStokOpname @Id=" + Id;

                    using (var command = new SqlCommand(sql, connection))
                    {
                        command.CommandTimeout = 0;
                        using (var reader = command.ExecuteReader())
                        {

                            while (reader.Read())
                            {
                                res = true;
                            }
                            res = true;
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                res = false;
            }
            return res;
        }
        public bool ULogTicketDetail(string Id)
        {
            bool res = false;
            try
            {
                conn.ConnectionString = ConfigurationFileStatic.ConnStrLog;
                using (var connection = conn)
                {
                    connection.Open();
                    string sql = "exec SP_ULogTicketDetail @Id=" + Id;

                    using (var command = new SqlCommand(sql, connection))
                    {
                        command.CommandTimeout = 0;
                        using (var reader = command.ExecuteReader())
                        {

                            while (reader.Read())
                            {
                                res = true;
                            }
                            res = true;
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                res = false;
            }
            return res;
        }
        public bool ULogTopupDetail(string Id)
        {
            bool res = false;
            try
            {
                conn.ConnectionString = ConfigurationFileStatic.ConnStrLog;
                using (var connection = conn)
                {
                    connection.Open();
                    string sql = "exec SP_ULogTopupDetail @Id=" + Id;

                    using (var command = new SqlCommand(sql, connection))
                    {
                        command.CommandTimeout = 0;
                        using (var reader = command.ExecuteReader())
                        {

                            while (reader.Read())
                            {
                                res = true;
                            }
                            res = true;
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                res = false;
            }
            return res;
        }
        #endregion
        
        
        #endregion

        public void PageControl(string name)
        {

            switch (name)
            {
                case "FormSetting":
                    #region FormSetting
                    Form fc1 = Application.OpenForms["FormSetting"];
                    if (fc1 != null)
                    {
                        fc1.Show();
                        fc1.BringToFront();
                    }
                    else
                    {
                        FormSetting frm = new FormSetting();
                        frm.StartPosition = FormStartPosition.CenterScreen;
                        frm.BringToFront();
                        frm.MaximizeBox = false;
                        frm.MinimizeBox = false;
                        frm.Show();
                    }
                    break;
                #endregion
                case "Uploader":
                    #region Uploader
                    Form fc2 = Application.OpenForms["Uploader"];
                    if (fc2 != null)
                    {
                        fc2.Show();
                        fc2.BringToFront();
                    }
                    else
                    {
                        Uploader frm = new Uploader();
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
            string curFile = Path.Combine(path0, @"configEwatsCloud.ttl");
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
            if (lines.Length > 1)
            {
                res.ConnStrLog = lines[0];
                res.IpServer = lines[1];
                res.DBServer = lines[2];
                res.UsernameServer = lines[3];
                res.PasswordServer = lines[4];
                res.PathImgWeb = lines[5];
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
                res = "Error: " +ex.Message;
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
