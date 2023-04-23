CREATE PROCEDURE [dbo].[SP_SaveClosing_V1_0_0_9]
	@ComputerName nvarchar(max),
	@Username nvarchar(max),
	@TotalCashirInputMoneyCashbox float
AS
BEGIN
	SET NOCOUNT ON;
	
	declare @temp table
	(
		TicketSalesCounting	float, 
		KartuBaruCounting	float, 
		TotalKartuRefundCounting	float, 
		TotalKartuBaruNominal	float, 
		TicketPayCash	float, 
		TicketPaySaldo	float, 
		TicketPaySaldoNCash	float, 
		PayEDC	float, 
		PaySaldoEDC	float, 
		TicketTotalAmount	float, 
		TotalTopupCash	float, 
		TotalTopupEDC	float, 
		TotalTopup	float, 
		FNBPayCash	float, 
		FNBPaySaldo	float, 
		FNBAll	float, 
		RefundJaminan	float, 
		RefundSaldo	float, 
		TotalRefund	float, 
		DanaModal	float, 
		TotalCashin	float, 
		TotalCashOut	float, 
		TotalEDC	float, 
		TotalEmoney	float, 
		TotalCashBox	float, 
		TotalTransaksiKasir float
	);
	
	insert @temp
	exec SP_GetDashboard_V1_0_0_9 @ComputerName,@Username

	declare @MoneyCashbox float
	declare @MatchingStatus nvarchar(max)
	declare @LogId bigint

	select @MoneyCashbox = (isnull(@TotalCashirInputMoneyCashbox,0) - isnull(TotalCashBox,0)) from @temp
	if(@MoneyCashbox < 0)
	begin
		select @MatchingStatus = 'Data tidak Matching, Kasir MINUS'
	end
	else if(@MoneyCashbox > 0)
	begin
		select @MatchingStatus = 'Data tidak Matching, Kasir kelebihan uang'
	end
	else
	begin
		select @MatchingStatus = 'Data System dengan Uang Real Sesuai'
	end

	if exists(select*from LogClosingV2 
	where replace(CONVERT(VARCHAR(10), CONVERT(date, replace(left(Datetime,10),'/','-'), 105), 23),'-','') = FORMAT(GETDATE() , 'yyyyMMdd') 
	and ComputerName = @ComputerName and Username = @Username and status = 1)
	begin
		select @LogId = LogId from LogClosingV2 
		where replace(CONVERT(VARCHAR(10), CONVERT(date, replace(left(Datetime,10),'/','-'), 105), 23),'-','') = FORMAT(GETDATE() , 'yyyyMMdd') 
		and ComputerName = @ComputerName and Username = @Username and status = 1
		select 'FALSE' as Success, 'Merchant telah melakukan Closing sebelumnya,LogId:'+cast(@LogId as nvarchar(max)) as _Message
	end
	else 
	begin
		--select*from LogClosingV2
		insert LogClosingV2 
		select FORMAT(GETDATE() , 'dd/MM/yyyy HH:mm:ss') Datetime,
		@TotalCashirInputMoneyCashbox KasirInputNominal,@ComputerName ComputerName,@Username Username,
		@MoneyCashbox MoneyCashboxSelisih, @MatchingStatus MatchingStatus,
		*,
		'Waiting Approve' StatusAcceptanceBySPV,
		'' KeteranganAcceptance,	
		0 UangDiterimaFinnance,
		0 TotalAmountStrukEDC,
		1 Status,
		null TanggalSetor
		from @temp

		select @LogId = LogId from LogClosingV2 where LogId = SCOPE_IDENTITY()

		update LogRefundDetail set Status = 2
		where replace(CONVERT(VARCHAR(10), CONVERT(date, replace(left(Datetime,10),'/','-'), 105), 23),'-','') = FORMAT(GETDATE() , 'yyyyMMdd')
		and ChasierBy = @Username and ComputerName = @ComputerName and Status = 1

		update LogTopupDetail set Status = 2
		where replace(CONVERT(VARCHAR(10), CONVERT(date, replace(left(Datetime,10),'/','-'), 105), 23),'-','') = FORMAT(GETDATE() , 'yyyyMMdd')
		and Chasierby = @Username and ComputerName = @ComputerName and Status = 1

		update LogRegistrasiDetail set status = 2
		where replace(CONVERT(VARCHAR(10), CONVERT(date, replace(left(Datetime,10),'/','-'), 105), 23),'-','') = FORMAT(GETDATE() , 'yyyyMMdd')
		and CashierBy = @Username and ComputerName =@ComputerName and status = 1

		update LogTicketDetail set Status = 2
		where replace(CONVERT(VARCHAR(10), CONVERT(date, replace(left(Datetime,10),'/','-'), 105), 23),'-','') = FORMAT(GETDATE() , 'yyyyMMdd')
		and ChasierBy = @Username and ComputerName = @ComputerName and Status = 1

		update LogFoodcourtTransaksi set Status = 2
		where replace(CONVERT(VARCHAR(10), CONVERT(date, replace(left(Datetime,10),'/','-'), 105), 23),'-','') = FORMAT(GETDATE() , 'yyyyMMdd')
		and CashierBy = @Username and ComputerName = @ComputerName and Status = 1

		update [LogItemsF&BTrx] set Status = 2
		where replace(CONVERT(VARCHAR(10), CONVERT(date, replace(left(Datetime,10),'/','-'), 105), 23),'-','') = FORMAT(GETDATE() , 'yyyyMMdd')
		and Chasierby = @Username and ComputerName = @ComputerName and Status = 1

		--select * from [DataChasierBox] 
		update [DataChasierBox]  set Status = 2,CloseBy=@Username
		where replace(CONVERT(VARCHAR(10), CONVERT(date, replace(left(Datetime,10),'/','-'), 105), 23),'-','') = FORMAT(GETDATE() , 'yyyyMMdd')
		and OpenBy = @Username and NamaComputer = @ComputerName and Status = 1


		--select*from LogCashierTambahModal
		update LogCashierTambahModal set Status=2
		where replace(CONVERT(VARCHAR(10), CONVERT(date, replace(left(Datetime,10),'/','-'), 105), 23),'-','') = FORMAT(GETDATE() , 'yyyyMMdd')
		and NamaUser = @Username and NamaComputer = @ComputerName and Status = 1

		select 'TRUE' as Success, 'Merchant berhasil melakukan closing,LogId:'+cast(@LogId as nvarchar(max)) as _Message
	end
END


