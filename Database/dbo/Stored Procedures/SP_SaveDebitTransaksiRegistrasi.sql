CREATE PROCEDURE [dbo].[SP_SaveDebitTransaksiRegistrasi]
	@AccountNumber nvarchar(max),
	@SaldoEmoney float,
	@SaldoEmoneyAfter float,
	@TicketWeekDay float,
	@TicketWeekDayAfter float,
	@TicketWeekEnd float,
	@TicketWeekEndAfter float,
	@SaldoJaminan float,
	@SaldoJaminanAfter float,
	@IdTicketTrx bigint,
	@Cashback float,
	@Topup float,
	@Asuransi float,
	@QtyTotalTiket float,
	@TotalBeliTiket float,
	@TotalAll float,
	@JenisTransaksi nvarchar(max),
	@TotalBayar float,
	@PayEmoney float,

	@KodeBank nvarchar(50),
	@NamaBank nvarchar(MAX),
	@DiskonBank float,
	@NominalDiskonBank float,
	@AdminCharges float,
	@NoATM nvarchar(50),
	@NoReff nvarchar(50),
	@DebitNominal float,

	@ComputerName nvarchar(max),
	@Chasier nvarchar(max)
AS
BEGIN
	SET NOCOUNT ON;
	declare @IdLogEDCTransaksi bigint
	set @IdLogEDCTransaksi = isnull((select top 1 IdLog from [dbo].[LogEDCTransaksi] order by IdLog desc),0)+1
	
	if(@IdLogEDCTransaksi > 0)
	begin
		insert into LogEDCTransaksi 
		(IdLog,Datetime,TotalBelanja,CodeBank,NamaBank,DiskonBank,NominalDiskon,AdminCharges,TotalDebit,NoATM,
		NoReffEddPrint,ComputerName,CashierBy,status)
		values(@IdLogEDCTransaksi,FORMAT(GETDATE() , 'dd/MM/yyyy HH:mm:ss'),
			@TotalBayar,@KodeBank,@NamaBank,@DiskonBank,@NominalDiskonBank,@AdminCharges,@DebitNominal,@NoATM,
			@NoReff,@ComputerName,@Chasier,1
		)
	end

	insert into [dbo].[LogRegistrasiDetail]
	(Datetime,AccountNumber,SaldoEmoney,SaldoEmoneyAfter,TicketWeekDay,TicketWeekDayAfter,TicketWeekEnd,
	TicketWeekEndAfter,SaldoJaminan,SaldoJaminanAfter,IdTicketTrx,Cashback,Topup,Asuransi,QtyTotalTiket,
	TotalBeliTiket,TotalAll,JenisTransaksi,TotalBayar,PayEmoney,IdLogEDCTransaksi,BankCode,NamaBank,DiskonBank,
	NominalDiskon,AdminCharges,TotalDebit,CashierBy,ComputerName,status)
	values(
		FORMAT(GETDATE() , 'dd/MM/yyyy HH:mm:ss'),
		@AccountNumber,@SaldoEmoney,@SaldoEmoneyAfter,@TicketWeekDay,@TicketWeekDayAfter,@TicketWeekEnd,
		@TicketWeekEndAfter,@SaldoJaminan,@SaldoJaminanAfter,@IdTicketTrx,@Cashback,@Topup,@Asuransi,@QtyTotalTiket,
		@TotalBeliTiket,@TotalAll,@JenisTransaksi,@TotalBayar,@PayEmoney,@IdLogEDCTransaksi,@KodeBank,@NamaBank,@DiskonBank,
		@NominalDiskonBank,@AdminCharges,@DebitNominal,@Chasier,@ComputerName,1
	)

	-- Save log Deposit untuk Dana yang terutang kas Finance
	if(@PayEmoney = 0)
	begin
		declare @TotalSaldo float
		
		if exists(select*from DataAccount where AccountNumber=@AccountNumber)
		begin
			if(@Topup > 0)
			begin
				Insert into [dbo].[LogDeposit]
				([Datetime],[AccountNumber],[TransactionType],[Nominal],[Status])
				values
				(
					FORMAT(GETDATE() , 'dd/MM/yyyy HH:mm:ss'),@AccountNumber,'CREDIT',@Topup,1
				)
			end
		end
		else
		begin
			set @TotalSaldo = (@SaldoEmoneyAfter+@SaldoJaminanAfter)
			Insert into [dbo].[LogDeposit]
			([Datetime],[AccountNumber],[TransactionType],[Nominal],[Status])
			values
			(
				FORMAT(GETDATE() , 'dd/MM/yyyy HH:mm:ss'),@AccountNumber,'CREDIT',@TotalSaldo,1
			)
		end
		
	end
	else
	begin
		Insert into [dbo].[LogDeposit]
		([Datetime],[AccountNumber],[TransactionType],[Nominal],[Status])
		values
		(
			FORMAT(GETDATE() , 'dd/MM/yyyy HH:mm:ss'),@AccountNumber,'DEBIT',@PayEmoney,1
		)
	end

	select 'Insert LogRegistrasiDetail Success ~'+FORMAT(GETDATE() , 'dd/MM/yyyy HH:mm:ss') as _Message,
	'TRUE' Success
END



















