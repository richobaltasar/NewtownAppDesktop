-- Batch submitted through debugger: dbewats.sql|964|0|C:\Users\Administrator\Desktop\dbewats.sql
CREATE PROCEDURE [dbo].[SP_GetDataTransaksiRegistrasi]
	@IdTrx bigint,
	@Datetime nvarchar(max),
	@JenisTransaksi nvarchar(max),
	@Nominal float,
	@CashierBy nvarchar(max)
AS
BEGIN
	SET NOCOUNT ON;
	select a.idTrx,a.Datetime,a.AccountNumber,a.SaldoEmoney,a.SaldoEmoneyAfter,
		a.TicketWeekDay,a.TicketWeekDayAfter,a.TicketWeekEnd,a.TicketWeekEndAfter,
		case when a.SaldoJaminan = 0  then a.SaldoJaminanAfter else 0 end as SaldoJaminan,
		a.IdTicketTrx,a.Cashback,a.Topup,a.Asuransi,
		a.QtyTotalTiket,a.TotalBeliTiket,a.TotalAll,a.JenisTransaksi,a.TotalBayar,a.PayEmoney,
		a.PayCash,a.TerimaUang,a.Kembalian,a.BankCode,a.NamaBank,isnull(a.DiskonBank,0) DiskonBank ,isnull(a.NominalDiskon,0) NominalDiskon,
		isnull(a.AdminCharges,0) AdminCharges,isnull(a.TotalDebit,0) TotalDebit,a.CashierBy,a.ComputerName,a.status,
		b.NoATM,b.NoReffEddPrint from LogRegistrasiDetail a
		Left join LogEDCTransaksi b on b.idLog = a.IdLogEDCTransaksi
		where a.idTrx = @IdTrx and a.Datetime = @Datetime 
		and a.TotalAll = @Nominal 
		and a.CashierBy = @CashierBy
	
END













