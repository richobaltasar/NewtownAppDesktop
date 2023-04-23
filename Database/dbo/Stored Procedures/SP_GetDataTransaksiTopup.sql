CREATE PROCEDURE [dbo].[SP_GetDataTransaksiTopup]
	@IdTrx bigint,
	@Datetime nvarchar(max),
	@Nominal float,
	@CashierBy nvarchar(max)
AS
BEGIN
	SET NOCOUNT ON;
	select
	a.AccountNumber,a.Datetime,IdTopup,Kembalian,a.ComputerName,Chasierby,NominalTopup,
	SaldoSebelum,SaldoSetelah,TerimaUang,PaymentMethod,
	IdLogEDCTransaksi,BankCode,a.NamaBank,a.DiskonBank,a.NominalDiskon,a.AdminCharges,a.TotalDebit,b.NoATM,b.NoReffEddPrint
	from LogTopupDetail a
	left join LogEDCTransaksi b on b.IdLog = a.IdLogEDCTransaksi
	where IdTopup = @IdTrx and a.Datetime = @Datetime and NominalTopup = @Nominal and Chasierby = @CashierBy

	
END












