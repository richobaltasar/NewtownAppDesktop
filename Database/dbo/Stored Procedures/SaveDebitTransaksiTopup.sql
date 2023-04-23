CREATE PROCEDURE [dbo].[SaveDebitTransaksiTopup]
	@AccountNumber nvarchar(max),
	@NominalTopup float,
	@JenisPayment nvarchar(max),
	@TotalBayar float,
	@KodeBank nvarchar(max),
	@NamaBank nvarchar(max),

	@DiskonBank float,
	@NominalDiskonBank float,
	@AdminCharges float,
	@DebitNominal float,

	@NoATM nvarchar(max),
	@NoReff nvarchar(max),
	@SaldoSebelum float,
	@SaldoSetelah float,
	
	@Chasierby nvarchar(max),
	@ComputerName nvarchar(max)
AS
BEGIN
	SET NOCOUNT ON;
	
	if exists (select*from DataChasierBox where NamaComputer = @ComputerName and left(Datetime,10) =   FORMAT(GETDATE() , 'dd/MM/yyyy') and Status = 1 )
	begin
		--select*from LogEDCTransaksi
		declare @IdLogEDCTransaksi bigint
		set @IdLogEDCTransaksi = isnull((select top 1 IdLog from [dbo].[LogEDCTransaksi] order by IdLog desc),0)+1
		if(@IdLogEDCTransaksi > 0)
		begin
			insert into LogEDCTransaksi 
			(IdLog,Datetime,TotalBelanja,CodeBank,NamaBank,DiskonBank,NominalDiskon,AdminCharges,TotalDebit,NoATM,
			NoReffEddPrint,ComputerName,CashierBy,status)
			values(@IdLogEDCTransaksi,FORMAT(GETDATE() , 'dd/MM/yyyy HH:mm:ss'),
				@TotalBayar,@KodeBank,@NamaBank,@DiskonBank,@NominalDiskonBank,@AdminCharges,@DebitNominal,@NoATM,
				@NoReff,@ComputerName,@Chasierby,1
			)
		end
		--select*from LogTopupDetail
		insert into LogTopupDetail
		(Datetime,AccountNumber,NominalTopup,JenisPayment,TotalBayar,PaymentMethod,IdLogEDCTransaksi,BankCode,NamaBank,
		DiskonBank,NominalDiskon,AdminCharges,TotalDebit,SaldoSebelum,SaldoSetelah,Chasierby,ComputerName,Status)
		values(
			FORMAT(GETDATE() , 'dd/MM/yyyy HH:mm:ss'),
			@AccountNumber,@NominalTopup,@JenisPayment,@TotalBayar,'EDC',@IdLogEDCTransaksi,@KodeBank,@NamaBank,
			@DiskonBank,@NominalDiskonBank,@AdminCharges,@DebitNominal,@SaldoSebelum,@SaldoSetelah,@Chasierby,@ComputerName,1
		)

		-- Save log Deposit untuk Dana yang terutang kas Finance
		Insert into [dbo].[LogDeposit]
		([Datetime],[AccountNumber],[TransactionType],[Nominal],[Status])
		values
		(
			FORMAT(GETDATE() , 'dd/MM/yyyy HH:mm:ss'),@AccountNumber,'CREDIT',@NominalTopup,1
		)

		select 'TRUE' as Success, 'Insert log Topup berhasil dilakukan ~'+FORMAT(GETDATE() , 'dd/MM/yyyy HH:mm:ss') as _Message
	end
	else 
	begin
		select 'FALSE' as Success, 'Insert log Topup Gagal dilakukan' as _Message
	end
	
END




















