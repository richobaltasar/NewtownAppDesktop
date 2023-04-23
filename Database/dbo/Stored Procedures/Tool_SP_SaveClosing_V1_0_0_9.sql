CREATE PROCEDURE [dbo].[Tool_SP_SaveClosing_V1_0_0_9]
	@ComputerName nvarchar(max),
	@Username nvarchar(max),
	@TotalCashirInputMoneyCashbox float,
	@Datetime nvarchar(max),
	@MatchingSucces nvarchar(max),
	@StatusAcceptanceBySPV nvarchar(max),
	@KeteranganAcceptance nvarchar(max),
	@UangDiterimaFinnance float,
	@Status bigint,
	@TotalNominalDebit float
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
	exec Tool_SP_GetDashboard_V1_0_0_9 @ComputerName,@Username,@Datetime

	declare @MoneyCashboxSelisih float
	declare @MatchingStatus nvarchar(max)
	declare @LogId bigint

	select @MoneyCashboxSelisih = (isnull(@TotalCashirInputMoneyCashbox,0) - isnull(TotalCashBox,0)) from @temp
	if(@MoneyCashboxSelisih < 0)
	begin
		select @MatchingStatus = 'Data tidak Matching, Kasir MINUS'
	end
	else if(@MoneyCashboxSelisih > 0)
	begin
		select @MatchingStatus = 'Data tidak Matching, Kasir kelebihan uang'
	end
	else if(@MoneyCashboxSelisih = 0)
	begin
		select @MatchingStatus = 'Data System dengan Uang Real Sesuai'
	end

	if exists(select*from LogClosingV2 
	where Datetime = @Datetime
	and ComputerName = @ComputerName and Username = @Username)
	begin
		select @LogId = LogId from LogClosingV2 
		where Datetime = @Datetime and ComputerName = @ComputerName and Username = @Username and status = 1

		select 'FALSE' as Success, 'Merchant telah melakukan Closing sebelumnya,LogId:'+cast(@LogId as nvarchar(max)) as _Message
	end
	else 
	begin
		--select*from LogClosingV2
		insert LogClosingV2 
		select @Datetime Datetime,
		@TotalCashirInputMoneyCashbox KasirInputNominal,@ComputerName ComputerName,@Username Username,
		@MoneyCashboxSelisih MoneyCashboxSelisih, @MatchingStatus MatchingStatus,
		*,
		@StatusAcceptanceBySPV StatusAcceptanceBySPV,
		@KeteranganAcceptance KeteranganAcceptance,	
		@UangDiterimaFinnance UangDiterimaFinnance,
		@TotalNominalDebit TotalAmountStrukEDC,
		@Status Status,
		@Datetime TanggalSetoran
		from @temp

		select @LogId = LogId from LogClosingV2 where LogId = SCOPE_IDENTITY()

		update LogRefundDetail set Status = 2
		where replace(CONVERT(VARCHAR(10), CONVERT(date, replace(left(Datetime,10),'/','-'), 105), 23),'-','') = replace(CONVERT(VARCHAR(10), CONVERT(date, replace(left(@Datetime,10),'/','-'), 105), 23),'-','')
		and ChasierBy = @Username and ComputerName = @ComputerName and Status = 1

		update LogTopupDetail set Status = 2
		where replace(CONVERT(VARCHAR(10), CONVERT(date, replace(left(Datetime,10),'/','-'), 105), 23),'-','') = replace(CONVERT(VARCHAR(10), CONVERT(date, replace(left(@Datetime,10),'/','-'), 105), 23),'-','')
		and Chasierby = @Username and ComputerName = @ComputerName and Status = 1

		update LogRegistrasiDetail set status = 2
		where replace(CONVERT(VARCHAR(10), CONVERT(date, replace(left(Datetime,10),'/','-'), 105), 23),'-','') = replace(CONVERT(VARCHAR(10), CONVERT(date, replace(left(@Datetime,10),'/','-'), 105), 23),'-','')
		and CashierBy = @Username and ComputerName =@ComputerName and status = 1

		update LogTicketDetail set Status = 2
		where replace(CONVERT(VARCHAR(10), CONVERT(date, replace(left(Datetime,10),'/','-'), 105), 23),'-','') = replace(CONVERT(VARCHAR(10), CONVERT(date, replace(left(@Datetime,10),'/','-'), 105), 23),'-','')
		and ChasierBy = @Username and ComputerName = @ComputerName and Status = 1

		update LogFoodcourtTransaksi set Status = 2
		where replace(CONVERT(VARCHAR(10), CONVERT(date, replace(left(Datetime,10),'/','-'), 105), 23),'-','') = replace(CONVERT(VARCHAR(10), CONVERT(date, replace(left(@Datetime,10),'/','-'), 105), 23),'-','')
		and CashierBy = @Username and ComputerName = @ComputerName and Status = 1

		update [LogItemsF&BTrx] set Status = 2
		where replace(CONVERT(VARCHAR(10), CONVERT(date, replace(left(Datetime,10),'/','-'), 105), 23),'-','') = replace(CONVERT(VARCHAR(10), CONVERT(date, replace(left(@Datetime,10),'/','-'), 105), 23),'-','')
		and Chasierby = @Username and ComputerName = @ComputerName and Status = 1

		select 'TRUE' as Success, 'Merchant berhasil melakukan closing,LogId:'+cast(@LogId as nvarchar(max)) as _Message
	end
END


