using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataUploader.Model
{
    public class ConfigurationFile
    {
        public string ConnStrLog { get; set; }
        public string IpServer { get; set; }
        public string DBServer { get; set; }
        public string UsernameServer { get; set; }
        public string PasswordServer { get; set; }
        public string PathImgWeb { get; set; }
    }

    public static class ConfigurationFileStatic
    {
        public static string ConnStrLog { get; set; }
        public static string IpServer { get; set; }
        public static string DBServer { get; set; }
        public static string UsernameServer { get; set; }
        public static string PasswordServer { get; set; }
        public static string PathImgWeb { get; set; }
        public static string ConnStrLogCloud { get; set; }
    }
    public static class General
    {
        public static string ConString { get; set; }
        public static string IDUser { get; set; }
        public static string Page { get; set; }

        public static string ConnStringLog { get; set; }
        public static string IpLocalServer { get; set; }
        public static string DBServer { get; set; }
        public static string UsernameServer { get; set; }
        public static string PasswordServer { get; set; }

        public static string PathImgWeb { get; set; }
    }
    #region Upload Tools
    public class UploadData
    {
        public List<LogCashierTambahModal> LogCashierTambahModal { get; set; }
        public List<LogClosing> LogClosing { get; set; }
        public List<LogDeposit> LogDeposit { get; set; }
        public List<LogEDCTransaksi> LogEDCTransaksi { get; set; }
        public List<LogEmoneyTrxAccount> LogEmoneyTrxAccount { get; set; }
        public List<LogFoodcourtTransaksi> LogFoodcourtTransaksi { get; set; }
        public List<LogItemsFBTrx> LogItemsFBTrx { get; set; }
        public List<LogRefundDetail> LogRefundDetail { get; set; }
        public List<LogRegistrasiDetail> LogRegistrasiDetail { get; set; }
        public List<LogSetoranDepositExpired> LogSetoranDepositExpired { get; set; }
        public List<LogStokOpname> LogStokOpname { get; set; }
        public List<LogTicketDetail> LogTicketDetail { get; set; }
        public List<LogTopupDetail> LogTopupDetail { get; set; }
    }

    public class ResponseUploadData
    {
        public List<string> LogCashierTambahModal { get; set; }
        public List<string> LogClosing { get; set; }
        public List<string> LogDeposit { get; set; }
        public List<string> LogEDCTransaksi { get; set; }
        public List<string> LogEmoneyTrxAccount { get; set; }
        public List<string> LogFoodcourtTransaksi { get; set; }
        public List<string> LogItemsFBTrx { get; set; }
        public List<string> LogRefundDetail { get; set; }
        public List<string> LogRegistrasiDetail { get; set; }
        public List<string> LogSetoranDepositExpired { get; set; }
        public List<string> LogStokOpname { get; set; }
        public List<string> LogTicketDetail { get; set; }
        public List<string> LogTopupDetail { get; set; }
    }

    public class LogCashierTambahModal
    {
        public string idLog { get; set; }
        public string Datetime { get; set; }
        public string NamaComputer { get; set; }
        public string NamaUser { get; set; }
        public string NominalTambahModal { get; set; }
        public string Status { get; set; }
    }

    public class LogClosing
    {
        public string IdLog { get; set; }
        public string Datetime { get; set; }
        public string NamaComputer { get; set; }
        public string NamaUser { get; set; }
        public string TotalAllTicket { get; set; }
        public string TotalTransaksi { get; set; }
        public string TotalTopup { get; set; }
        public string TotalRegis { get; set; }
        public string TotalRefund { get; set; }
        public string TotalFoodcourt { get; set; }
        public string TotalDanaModal { get; set; }
        public string TotalCashOut { get; set; }
        public string TotalCashIn { get; set; }
        public string TotalCashBox { get; set; }
        public string TotalCashirInputMoneyCashbox { get; set; }
        public string MinusIndikasiMoneyCashBox { get; set; }
        public string MatchingSucces { get; set; }
        public string StatusAcceptanceBySPV { get; set; }
        public string KeteranganAcceptance { get; set; }
        public string UangDiterimaFinnance { get; set; }
        public string TotalTrxEdc { get; set; }
        public string TotalNominalDebit { get; set; }
        public string TotalTrxEmoney { get; set; }
        public string TotalNominalDebitEmoney { get; set; }
        public string Status { get; set; }
    }

    public class LogDeposit
    {
        public string LogId { get; set; }
        public string Datetime { get; set; }
        public string AccountNumber { get; set; }
        public string TransactionType { get; set; }
        public string Nominal { get; set; }
        public string Status { get; set; }
    }

    public class LogEDCTransaksi
    {
        public string IdLog { get; set; }
        public string Datetime { get; set; }
        public string TotalBelanja { get; set; }
        public string CodeBank { get; set; }
        public string NamaBank { get; set; }
        public string DiskonBank { get; set; }
        public string NominalDiskon { get; set; }
        public string AdminCharges { get; set; }
        public string TotalDebit { get; set; }
        public string NoATM { get; set; }
        public string NoReffEddPrint { get; set; }
        public string ComputerName { get; set; }
        public string CashierBy { get; set; }
        public string status { get; set; }
    }

    public class LogEmoneyTrxAccount
    {
        public string IdLog { get; set; }
        public string AcountNumber { get; set; }
        public string Datetime { get; set; }
        public string SaldoSebelumnya { get; set; }
        public string Credit { get; set; }
        public string Debit { get; set; }
        public string SisaSaldo { get; set; }
        public string Status { get; set; }
    }

    public class LogFoodcourtTransaksi
    {
        public string IdTrx { get; set; }
        public string Datetime { get; set; }
        public string AccountNumber { get; set; }
        public string SaldoEmoney { get; set; }
        public string SaldoEmoneyAfter { get; set; }
        public string IdItemsKeranjang { get; set; }
        public string JenisTransaksi { get; set; }
        public string TotalBayar { get; set; }
        public string PayEmoney { get; set; }
        public string PayCash { get; set; }
        public string TerimaUang { get; set; }
        public string Kembalian { get; set; }
        public string ComputerName { get; set; }
        public string CashierBy { get; set; }
        public string Status { get; set; }
    }

    public class LogItemsFBTrx
    {
        public string IdItemsKeranjang { get; set; }
        public string Datetime { get; set; }
        public string NamaTenant { get; set; }
        public string KodeBarang { get; set; }
        public string NamaItem { get; set; }
        public string Harga { get; set; }
        public string Qtx { get; set; }
        public string Total { get; set; }
        public string Status { get; set; }
        public string Chasierby { get; set; }
        public string ComputerName { get; set; }
        public string AccountNumber { get; set; }
    }

    public class LogRefundDetail
    {
        public string IdRefund { get; set; }
        public string Datetime { get; set; }
        public string AccountNumber { get; set; }
        public string SaldoEmoney { get; set; }
        public string SaldoJaminan { get; set; }
        public string TicketWeekDay { get; set; }
        public string TicketWeekEnd { get; set; }
        public string TotalNominalRefund { get; set; }
        public string ChasierBy { get; set; }
        public string ComputerName { get; set; }
        public string Status { get; set; }
    }

    public class LogRegistrasiDetail
    {
        public string idTrx { get; set; }
        public string Datetime { get; set; }
        public string AccountNumber { get; set; }
        public string SaldoEmoney { get; set; }
        public string SaldoEmoneyAfter { get; set; }
        public string TicketWeekDay { get; set; }
        public string TicketWeekDayAfter { get; set; }
        public string TicketWeekEnd { get; set; }
        public string TicketWeekEndAfter { get; set; }
        public string SaldoJaminan { get; set; }
        public string SaldoJaminanAfter { get; set; }
        public string IdTicketTrx { get; set; }
        public string Cashback { get; set; }
        public string Topup { get; set; }
        public string Asuransi { get; set; }
        public string QtyTotalTiket { get; set; }
        public string TotalBeliTiket { get; set; }
        public string TotalAll { get; set; }
        public string JenisTransaksi { get; set; }
        public string TotalBayar { get; set; }
        public string PayEmoney { get; set; }
        public string PayCash { get; set; }
        public string TerimaUang { get; set; }
        public string Kembalian { get; set; }
        public string CashierBy { get; set; }
        public string ComputerName { get; set; }
        public string status { get; set; }
        public string IdLogEDCTransaksi { get; set; }
        public string BankCode { get; set; }
        public string NamaBank { get; set; }
        public string DiskonBank { get; set; }
        public string NominalDiskon { get; set; }
        public string AdminCharges { get; set; }
        public string TotalDebit { get; set; }
    }

    public class LogSetoranDepositExpired
    {
        public string LogId { get; set; }
        public string Datetime { get; set; }
        public string AccountNumber { get; set; }
        public string Saldo { get; set; }
        public string UangJaminan { get; set; }
        public string TotalDeposit { get; set; }
        public string TanggalExpired { get; set; }
        public string NamaPenyetor { get; set; }
        public string TanggalSetor { get; set; }
        public string StatusSetor { get; set; }
    }

    public class LogStokOpname
    {
        public string idLog { get; set; }
        public string Datetime { get; set; }
        public string NamaTenant { get; set; }
        public string NamaItem { get; set; }
        public string StockSebelumnya { get; set; }
        public string StockUpdate { get; set; }
        public string UpdateBy { get; set; }
        public string Status { get; set; }
    }

    public class LogTicketDetail
    {
        public string Datetime { get; set; }
        public string IdTicket { get; set; }
        public string AccountNumber { get; set; }
        public string NamaTicket { get; set; }
        public string Harga { get; set; }
        public string Qty { get; set; }
        public string Total { get; set; }
        public string IdDiskon { get; set; }
        public string NamaDiskon { get; set; }
        public string Diskon { get; set; }
        public string TotalDiskon { get; set; }
        public string TotalAfterDiskon { get; set; }
        public string Status { get; set; }
        public string ChasierBy { get; set; }
        public string ComputerName { get; set; }
    }

    public class LogTopupDetail
    {
        public string IdTopup { get; set; }
        public string Datetime { get; set; }
        public string JenisPayment { get; set; }
        public string AccountNumber { get; set; }
        public string NominalTopup { get; set; }
        public string TotalBayar { get; set; }
        public string PayCash { get; set; }
        public string TerimaUang { get; set; }
        public string Kembalian { get; set; }
        public string SaldoSebelum { get; set; }
        public string SaldoSetelah { get; set; }
        public string Chasierby { get; set; }
        public string ComputerName { get; set; }
        public string Status { get; set; }
        public string IdLogEDCTransaksi { get; set; }
        public string BankCode { get; set; }
        public string NamaBank { get; set; }
        public string DiskonBank { get; set; }
        public string NominalDiskon { get; set; }
        public string AdminCharges { get; set; }
        public string TotalDebit { get; set; }
        public string PaymentMethod { get; set; }
    }

    #endregion
}
