
CREATE PROCEDURE [dbo].[SP_UploadLogTopupDetail]
	@IdTopup    bigint,
	@Datetime    nvarchar(50),
	@JenisPayment    nvarchar(MAX),
	@AccountNumber    nvarchar(50),
	@NominalTopup    float,
	@TotalBayar    float,
	@PayCash    float,
	@TerimaUang    float,
	@Kembalian    float,
	@SaldoSebelum    float,
	@SaldoSetelah    float,
	@Chasierby    nvarchar(MAX),
	@ComputerName    nvarchar(MAX),
	@Status    int,
	@IdLogEDCTransaksi    bigint,
	@BankCode    nvarchar(50),
	@NamaBank    nvarchar(MAX),
	@DiskonBank    float,
	@NominalDiskon    float,
	@AdminCharges    float,
	@TotalDebit    float,
	@PaymentMethod    nvarchar(MAX)
AS
BEGIN
	SET NOCOUNT ON;
	if not exists(select*from LogTopupDetail where IdTopup = @IdTopup)
	begin
		insert into LogTopupDetail
		(
			IdTopup
			,Datetime
			,JenisPayment
			,AccountNumber
			,NominalTopup
			,TotalBayar
			,PayCash
			,TerimaUang
			,Kembalian
			,SaldoSebelum
			,SaldoSetelah
			,Chasierby
			,ComputerName
			,Status
			,IdLogEDCTransaksi
			,BankCode
			,NamaBank
			,DiskonBank
			,NominalDiskon
			,AdminCharges
			,TotalDebit
			,PaymentMethod
		)
		values(
			@IdTopup
			,@Datetime
			,@JenisPayment
			,@AccountNumber
			,@NominalTopup
			,@TotalBayar
			,@PayCash
			,@TerimaUang
			,@Kembalian
			,@SaldoSebelum
			,@SaldoSetelah
			,@Chasierby
			,@ComputerName
			,@Status
			,@IdLogEDCTransaksi
			,@BankCode
			,@NamaBank
			,@DiskonBank
			,@NominalDiskon
			,@AdminCharges
			,@TotalDebit
			,@PaymentMethod
		)
		select @IdTopup as Id
	end

END



