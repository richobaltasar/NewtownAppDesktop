using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ewats_App.Model
{

    public class ConfigurationFile
    {
        public string ConnStrLog { get; set; }
        public string IpServer { get; set; }
        public string DBServer { get; set; }
        public string UsernameServer { get; set; }
        public string PasswordServer { get; set; }
        public string PathImgWeb { get; set; }
        public string VFDPort { get; set; }
    }

    public static class ConfigurationFileStatic
    {
        public static string ConnStrLog { get; set; }
        public static string IpServer { get; set; }
        public static string DBServer { get; set; }
        public static string UsernameServer { get; set; }
        public static string PasswordServer { get; set; }
        public static string PathImgWeb { get; set; }
        public static string VFDPort { get; set; }
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
        public static string VFDPort { get; set; }
    }

    public class DataCashback
    {
        public string id { get; set; }
        public string NamaCashback { get; set; }
        public decimal Nominal { get; set; }
    }



    public class StockOpnameModel
    {
        public string idItem { get; set; }
        public string NamaTenant { get; set; }
        public string NamaItem { get; set; }
        public decimal BykStok { get; set; }
        public decimal BykStokUpdate { get; set; }
    }

    public class KeranjangStockOpnameModel
    {
        public string idItem { get; set; }
        public string NamaTenant { get; set; }
        public string NamaItem { get; set; }
        public decimal BykStok { get; set; }
        public decimal BykStokUpdate { get; set; }
    }

    public static class TempAllTransaksiModel
    {
        public static string IdTrx { get; set; }
        public static string Datetime { get; set; }
        public static string JenisTransaksi { get; set; }
        public static decimal Nominal { get; set; }
        public static string CashierBy { get; set; }
    }

    public class AllTransaksiModel
    {
        public string IdTrx { get; set; }
        public string Datetime { get; set; }
        public string JenisTransaksi { get; set; }
        public decimal Nominal { get; set; }
        public string CashierBy { get; set; }
    }
    public class GetDataTransaksiRefundReprintModel
    {
        public string IdTransaction { get; set; }
        public string Datetime { get; set; }
        public string MerchantName { get; set; }
        public string NamaKasir { get; set; }
        public string AccounNumber { get; set; }
        public string SaldoEmoney { get; set; }
        
        public string SaldoJaminan { get; set; }
        public string TotalRefund { get; set; }
    }

    public class GetDataTransaksiFoodCourtReprintModel
    {
        public string IdTransaction { get; set; }
        public string Datetime { get; set; }
        public string MerchantName { get; set; }
        public string NamaKasir { get; set; }
        public string TotalBelanja { get; set; }
        public string PaymentMethod { get; set; }
        public string UseEmoney { get; set; }
        public string AccountNumber { get; set; }
        public string SaldoSebelum { get; set; }
        public string SaldoSetelah { get; set; }
        public string IdItemKeranjang { get; set; }
    }

    public class GetDataTransaksiTopupModel
    {
        public string IdTransaction { get; set; }
        public string Datetime { get; set; }
        public string MerchantName { get; set; }
        public string NamaKasir { get; set; }
        public string NominalTopup { get; set; }
        public string UangDibayarkan { get; set; }
        public string Kembalian { get; set; }
        public string AccountNumber { get; set; }
        public string SaldoSebelumnya { get; set; }
        public string SaldoSetelahnya { get; set; }

        public string PaymentMethod { get; set; }
        public string IdLogEDCTransaksi { get; set; }
        public string BankCode { get; set; }
        public string NamaBank { get; set; }
        public string DiskonBank { get; set; }
        public string NominalDiskon { get; set; }
        public string AdminCharges { get; set; }
        public string TotalDebit { get; set; }
        public string NoATM { get; set; }
        public string NoReffEddPrint { get; set; }
    }

    public class GetDataTransaksiRegistrasiModel
    {             
        public string idTrx { get; set; }
        public string Datetime { get; set; }
        public string ComputerName { get; set; }
        public string CashierBy { get; set; }
        public string AccountNumber { get; set; }
        public decimal SaldoJaminan { get; set; }

        public decimal SaldoEmoneyBefore { get; set; }
        public decimal SaldoEmoneyAfter { get; set; }
        public string IdTicketTrx { get; set; }
        public decimal Cashback { get; set; }
        public decimal topup { get; set; }
        public decimal Asuransi { get; set; }
        public decimal QtyTotalTiket { get; set; }
        public decimal TotalBeliTiket { get; set; }
        public decimal TotalAll { get; set; }
        public string JenisTransaksi { get; set; }
        public decimal TotalBayar { get; set; }
        public decimal PayEmoney { get; set; }
        public decimal PayCash { get; set; }
        public decimal TerimaUang { get; set; }
        public decimal Kembalian { get; set; }

        public string BankCode { get; set; }
        public string NamaBank { get; set; }
        public decimal DiskonBank { get; set; }
        public decimal NominalDiskon { get; set; }
        public decimal AdminCharges { get; set; }
        public decimal TotalDebit { get; set; }

        public string NoATM { get; set; }
        public string NoReffEddPrint { get; set; }

        
    }

    public class TambahModalCashbox
    {
        public string Id { get; set; }
        public string NamaUser { get; set; }
        public string ComputerName { get; set; }
        public decimal Nominal { get; set; }
        public string Datetime { get; set; }
    }

    public class DataUser
    {
        public string ID { get; set; }
        public string NamaLengkap { get; set; }
        public string email { get; set; }
        public string Gender { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public string NoHp { get; set; }
        public string JenisIdentitas { get; set; }
        public string NoIdentitas { get; set; }
        public string Agama { get; set; }
        public string TempatLahir { get; set; }
        public string TanggalLahir { get; set; }
        public string NamaPanggilan { get; set; }
        public string alamat { get; set; }
    }

    public class OLogin
    {
        public string HakAkses { get; set; }
        public string ID { get; set; }
    }

    public class Ticket
    {
        public string IdTicket { get; set; }
        public string namaticket { get; set; }
        public decimal harga { get; set; }
        public decimal CashbackNominal { get; set; }
        public decimal CashbackPersen { get; set; }
    }

    public class GetGridTicketModel
    {
        public string NamaTicket { get; set; }
        public string HargaSatuan { get; set; }
        public string Qty { get; set; }
        public string Total { get; set; }
        public string NamaDiskon { get; set; }
        public string BesarDiskon { get; set; }
        public string TotalDiskon { get; set; }
        public string TotalAkhir { get; set; }
    }

    public class GetGridKeranjangModel
    {
        public string NamaItem { get; set; }
        public string HargaSatuan { get; set; }
        public string Qty { get; set; }
        public string Total { get; set; }
    }

    public class DataPromo
    {
        public string idPromo { get; set; }
        public string NamaPromo { get; set; }
        public string CatPromo { get; set; }
        public string Diskon { get; set; }
        public string Status { get; set; }
        public string BerlakuDari { get; set; }
        public string BerlakuSampai { get; set; }
    }

    public static class registrasiModel
    {
        public static string IdCard { get; set; }
        public static decimal SaldoSebelum { get; set; }
        public static decimal SaldoSesudah { get; set; }
        public static decimal Ticket { get; set; }
        public static decimal JaminanGelangYangTerbaca { get; set; }

        public static DataPromo Promo { get; set; }
        public static string namaticket { get; set; }
        public static decimal harga { get; set; }
        public static decimal QtyTiket { get; set; }
        public static decimal TotalTicket { get; set; }

        public static decimal JaminanGelangToday { get; set; }
        public static decimal Asuransin { get; set; }

        public static decimal Diskon { get; set; }
        public static decimal NominalDiskon { get; set; }
        public static decimal TotalAfterDiskon { get; set; }

        public static decimal TotalAll { get; set; }

        public static bool UseEmoney { get; set; }
        public static decimal TotalBayar { get; set; }
    }

    public static class RefundModel
    {
        public static string AccountNumber { get; set; }
        public static decimal Saldo { get; set; }
        public static decimal Ticket { get; set; }
        public static decimal JaminanGelang { get; set; }
        public static decimal TotalRefund { get; set; }
    }

    public class KeranjangTicket
    {
        public string IdTicket { get; set; }
        public string NamaTicket { get; set; }
        public decimal Harga { get; set; }
        public decimal Qty { get; set; }
        public decimal Total { get; set; }
        public string IdDiskon { get; set; }
        public string NamaDiskon { get; set; }
        public decimal Diskon { get; set; }
        public decimal TotalDiskon { get; set; }
        public decimal TotalAfterDiskon { get; set; }
    }

    public class KeranjangFoodcourt
    {
        public string IdTrx { get; set; }
        public string NamaItem { get; set; }
        public decimal Harga { get; set; }
        public decimal Qtx { get; set; }
    }

    public class KeranjangPosTotal
    {
        public string IdTrx { get; set; }
        public string NamaTenant { get; set; }
        public string NamaItem { get; set; }
        public decimal HargaSatuan { get; set; }
        public decimal Qtx { get; set; }
        public decimal HargaTotal { get; set; }
        public decimal Stok { get; set; }
    }

    public class DataAccount
    {
        public string NamaUser { get; set; }
        public string ComputerName { get; set; }
        public string AccountNumber { get; set; }
        public string JenisTransaksi { get; set; }
        public string IdCard { get; set; }
        public decimal BalancedSebelum { get; set; }
        public decimal BalancedSesudah { get; set; }
        public decimal Ticket { get; set; }
        public decimal JaminanGelangYgTerbaca { get; set; }
        public decimal JaminanGelangToday { get; set; }
        public string CreateDate { get; set; }
        public string ExpiredDate { get; set; }
        public string UpdateDate { get; set; }
        public int Status { get; set; }

        public string idPromo { get; set; }
        public string NamaPromo { get; set; }
        public string CatPromo { get; set; }
        public string Diskon { get; set; }

        public string namaticket { get; set; }
        public decimal hargaTicket { get; set; }
        public decimal QtyTiket { get; set; }
        public decimal TotalTicket { get; set; }

        public decimal Asuransin { get; set; }
        
        public decimal NominalDiskon { get; set; }
        public decimal TotalAfterDiskon { get; set; }

        public decimal NominalTopup { get; set; }
        public decimal NominalRefund { get; set; }

        public string PaymentMethod { get; set; }
        public decimal TotalPembayaran { get; set; }
        public decimal TerimaUang { get; set; }
        public decimal Kembalian { get; set; }
        public bool UseEmoney { get; set; }

    }

    public static class CashPayment
    {
        public static string JenisTransaksi { get; set; }
        public static decimal TotalPembayaran { get; set; }
        public static decimal TerimaUang { get; set; }
        public static decimal Kembalian { get; set; }

    }

    public static class DebitPayment
    {
        public static string JenisTransaksi { get; set; }
        public static decimal TotalPembayaran { get; set; }
    }

    public class displayOut
    {
        public int errType { get; set; }
        public int retVal { get; set; }
        public string PrintText { get; set; }
    }

    public class ReturnResult
    {
        public int RetCode { get; set; }
        public bool Success { get; set; }
        public string Message { get; set; }
    }

    public class ReaderCard
    {
        public string IdCard { get; set; }
        public decimal SaldoEmoney { get; set; }
        public decimal TicketWeekDay { get; set; }
        public decimal TicketWeekEnd { get; set; }
        public decimal SaldoJaminan { get; set; }
        public string CodeId { get; set; }

        public decimal SaldoEmoneyAfter { get; set; }
        public decimal TicketWeekDayAfter { get; set; }
        public decimal TicketWeekEndAfter { get; set; }
        public decimal SaldoJaminanAfter { get; set; }
        public string CodeIdAfter { get; set; }

        public bool Success { get; set; }
        public string Message { get; set; }
    }

    public class RegistrasiCheckout
    {
        public ReaderCard Card { get; set; }
        public List<KeranjangTicket> tiket { get; set; }
        public decimal Cashback { get; set; }
        public decimal Topup { get; set; }
        public decimal SaldoJaminan { get; set; }
        public decimal Asuransi { get; set; }
        public decimal QtyTotalTiket { get; set; }
        public decimal TotalBeliTiket { get; set; }
        public decimal TotalAll { get; set; }
    }

    public static class RegisCashPayment
    {
        public static ReaderCard Card { get; set; }
        public static List<KeranjangTicket> tiket { get; set; }
        public static List<KeranjangFoodcourt> Sewa { get; set; }
        public static PaymentMethod Payment { get; set; }
        public static decimal TotalSewa { get; set; }
        public static decimal Cashback { get; set; }
        public static decimal Topup { get; set; }
        public static decimal SaldoJaminan { get; set; }
        public static decimal Asuransi { get; set; }
        public static decimal QtyTotalTiket { get; set; }
        public static decimal TotalBeliTiket { get; set; }
        public static decimal TotalAll { get; set; }
    }
    public static class RegisDebitPayment
    {
        public static ReaderCard Card { get; set; }
        public static List<KeranjangTicket> tiket { get; set; }
        public static List<KeranjangFoodcourt> Sewa { get; set; }
        public static DebitPaymentMethod Payment { get; set; }
        public static decimal TotalSewa { get; set; }
        public static decimal Cashback { get; set; }
        public static decimal Topup { get; set; }
        public static decimal SaldoJaminan { get; set; }
        public static decimal Asuransi { get; set; }
        public static decimal QtyTotalTiket { get; set; }
        public static decimal TotalBeliTiket { get; set; }
        public static decimal TotalAll { get; set; }
    }


    public class SaveRegisTrx
    {
        public ReaderCard Card { get; set; }
        public List<KeranjangTicket> tiket { get; set; }
        public List<KeranjangFoodcourt> Sewa { get; set; }
        public PaymentMethod Payment { get; set; }
        public decimal Cashback { get; set; }
        public decimal Topup { get; set; }
        public decimal SaldoJaminan { get; set; }
        public decimal Asuransi { get; set; }
        public decimal QtyTotalTiket { get; set; }
        public decimal TotalBeliTiket { get; set; }
        public decimal TotalSewa { get; set; }
        public decimal TotalAll { get; set; }
    }

    public class SaveRegisDebitTrx
    {
        public ReaderCard Card { get; set; }
        public List<KeranjangTicket> tiket { get; set; }
        public List<KeranjangFoodcourt> Sewa { get; set; }
        public DebitPaymentMethod Payment { get; set; }
        public decimal Cashback { get; set; }
        public decimal Topup { get; set; }
        public decimal SaldoJaminan { get; set; }
        public decimal Asuransi { get; set; }
        public decimal QtyTotalTiket { get; set; }
        public decimal TotalBeliTiket { get; set; }
        public decimal TotalSewa { get; set; }
        public decimal TotalAll { get; set; }
    }

    public static class TopupCashPayment
    {
        public static ReaderCard Card { get; set; }
        public static decimal NominalTopup { get; set; }
        public static PaymentMethod Pay {get;set;}
    }

    public static class TopupDebitPayment
    {
        public static ReaderCard Card { get; set; }
        public static decimal NominalTopup { get; set; }
        public static DebitPaymentMethod Pay { get; set; }
    }


    public class SaveTopupTrx
    {
        public ReaderCard Card { get; set; }
        public decimal NominalTopup { get; set; }
        public PaymentMethod Pay { get; set; }
        public string NamaUser { get; set; }
        public string ComputerName { get; set; }
    }

    public class SaveDebitTopupTrx
    {
        public ReaderCard Card { get; set; }
        public decimal NominalTopup { get; set; }
        public DebitPaymentMethod Pay { get; set; }
        public string NamaUser { get; set; }
        public string ComputerName { get; set; }
    }

    public static class FoodCourtPayment
    {
        public static ReaderCard Card { get; set; }
        public static List<KeranjangFoodcourt> Keranjang { get; set; }
        public static PaymentMethod Pay { get; set; }
    }

    public class SaveFoodCourtPayment
    {
        public ReaderCard Card { get; set; }
        public List<KeranjangFoodcourt> Keranjang { get; set; }
        public PaymentMethod Pay { get; set; }
    }

    public static class RefundCash
    {
        public static ReaderCard Card { get; set; }
        public static decimal NominalRefund { get; set; }
    }

    public class SaveRefundCash
    {
        public ReaderCard Card { get; set; }
        public decimal NominalRefund { get; set; }
        public string NamaUser { get; set; }
        public string ComputerName { get; set; }

    }

    public class PaymentMethod
    {
        public string JenisTransaksi { get; set; }
        public decimal TotalBayar { get; set; }
        public decimal PayEmoney { get; set; }
        public decimal PayCash { get; set; }
        public decimal TerimaUang { get; set; }
        public decimal Kembalian { get; set; }
    }

    public class DebitPaymentMethod
    {
        public string JenisTransaksi { get; set; }
        public decimal TotalBayar { get; set; }
        public decimal PayEmoney { get; set; }
        public decimal PayTunai { get; set; }
        public decimal DebitNominal { get; set; }
        public string KodeBank { get; set; }
        public string NamaBank { get; set; }
        public decimal DiskonBank { get; set; }
        public decimal NominalDiskonBank { get; set; }
        public decimal AdminCharges { get; set; }
        public string NoATM { get; set; }
        public string NoReff { get; set; }
        public decimal Kembalian { get; set; }
    }

    public class DataBank
    {
        public string idLog { get; set; }
        public string KodeBank { get; set; }
        public string NamaBank { get; set; }
        public string DiskonBank { get; set; }
        public string AdminCharges { get; set; }
        public string status { get; set; }
    }


    public class PrintHeader
    {
        public string Datetime { get; set; }
        public string IdTrx { get; set; }
        public string MerchantName { get; set; }
        public string NamaPetugas { get; set; }
    }

    public class DataTenant
    {
        public string Id { get; set; }
        public string NamaTenant { get; set; }
    }

    public class DataBarang
    {
        public string IdMenu { get; set; }
        public string NamaBarang { get; set; }
        public decimal Harga { get; set; }
        public string LinkPic { get; set; }
        public decimal Stok { get; set; }
    }

    public class LogHistoryAccModel
    {
        public string idlog { get; set; }
        public string Datetime { get; set; }
        public string JenisTransaksi { get; set; }
        public string Uraian { get; set; }
        public string Nominal { get; set; }
    }

    public class DashboardModel
    {
        public string TotalTopup { get; set; }
        public string TotalRefund { get; set; }
        public string TotalRegis { get; set; }
        public string TotalFoodcourtCash { get; set; }
        public string TotalFoodcourtEmoney { get; set; }
        public string TotalTicketPayEmoney { get; set; }
        public string TotalTransaksi { get; set; }

        public string TotalDanaModal { get; set; }
        public string TotalCashIn{ get; set; }
        public string TotalCashOut { get; set; }
        public string TotalCashBox { get; set; }
        public string TotalAllTicket { get; set; }

        public string TotalNominalEdcRegis { get; set; }
        public string TotalNominalEdcTopup { get; set; }

        public string TotalTrxEdc { get; set; }
        public string TotalNominalDebit { get; set; }

        public string TotalRegistCount { get; set; }
        public string TotalRefundCount { get; set; }

        public string TotalTrxEmoney { get; set; }
        public string TotalNominalDebitEmoney { get; set; }
    }

    public class ComboboxItem
    {
        public string Text { get; set; }
        public object Value { get; set; }

        public override string ToString()
        {
            return Text;
        }
    }

    public class DataDashTicket
    {
        public string JenisTicket { get; set; }
        public string HargaSatuan { get; set; }
        public string Count { get; set; }
        public string Total { get; set; }
    }

    public class DataKolomPrintInputVisitor
    {
        public string Visible { get; set; }
        public string Title { get; set; }
        public string Nama { get; set; }
        public string MoKtp { get; set; }
        public string NoTelp { get; set; }
        public string Alamat { get; set; }
        public string NoTicket { get; set; }
    }

}
