CREATE PROCEDURE [dbo].[SP_Closing]
	@TotalTransaksi float,
	@TotalTopup  float,
	@TotalRegis float,
	@TotalRefund float,
	@TotalFoodcourt float,
	@TotalDanaModal float,
	@TotalCashOut float,
	@TotalCashIn float,
	@TotalCashBox float,
	@TotalAllTicket float,

	@TotalTrxEdc float,
	@TotalNominalDebit float,
	@TotalTrxEmoney float,
	@TotalNominalDebitEmoney float,

	@TotalCashirInputMoneyCashbox float,
	@ComputerName nvarchar(max),
	@NamaUser nvarchar(max)
AS
BEGIN
	SET NOCOUNT ON;
	declare @MinusIndikasiMoneyCashBox float
	declare @MatchingSucces nvarchar(max)

	if(@TotalCashirInputMoneyCashbox = @TotalCashBox)
	begin
		set @MinusIndikasiMoneyCashBox = 0;
		set @MatchingSucces = 'Matching Success'
	end
	else
	begin
		if(@TotalCashBox > @TotalCashirInputMoneyCashbox)
		begin
			set @MinusIndikasiMoneyCashBox = @TotalCashBox - @TotalCashirInputMoneyCashbox;
			set @MatchingSucces = 'Matching Failed, Cashier minus :'+cast(@MinusIndikasiMoneyCashBox as nvarchar(max))
		end
		else
		begin
			set @MinusIndikasiMoneyCashBox = @TotalCashirInputMoneyCashbox - @TotalCashBox;
			set @MatchingSucces = 'Matching Failed, Cashier Kelebihan :'+cast(@MinusIndikasiMoneyCashBox as nvarchar(max))
		end
	end

	insert into LogClosing
	(
		Datetime,NamaComputer,NamaUser,TotalAllTicket,TotalTransaksi,TotalTopup,TotalRegis,
		TotalRefund,TotalFoodcourt,TotalDanaModal,TotalCashOut,TotalCashIn,TotalCashBox,
		TotalCashirInputMoneyCashbox,MinusIndikasiMoneyCashBox,MatchingSucces,StatusAcceptanceBySPV,Status,
		TotalTrxEdc,TotalNominalDebit,TotalTrxEmoney,TotalNominalDebitEmoney
	)
	values
	(
		FORMAT(GETDATE() , 'dd/MM/yyyy HH:mm:ss'),
		@ComputerName,@NamaUser,@TotalAllTicket,@TotalTransaksi,@TotalTopup,@TotalRegis,
		@TotalRefund,@TotalFoodcourt,@TotalDanaModal,@TotalCashOut,@TotalCashIn,@TotalCashBox,
		@TotalCashirInputMoneyCashbox,@MinusIndikasiMoneyCashBox,@MatchingSucces,'Waiting Approve',1,
		@TotalTrxEdc,@TotalNominalDebit,@TotalTrxEmoney,@TotalNominalDebitEmoney
	)

	update LogRefundDetail set Status = 2
	where replace(CONVERT(VARCHAR(10), CONVERT(date, replace(left(Datetime,10),'/','-'), 105), 23),'-','') = FORMAT(GETDATE() , 'yyyyMMdd')
	and ChasierBy = @NamaUser and ComputerName = @ComputerName and Status = 1

	update LogTopupDetail set Status = 2
	where replace(CONVERT(VARCHAR(10), CONVERT(date, replace(left(Datetime,10),'/','-'), 105), 23),'-','') = FORMAT(GETDATE() , 'yyyyMMdd')
	and Chasierby = @NamaUser and ComputerName = @ComputerName and Status = 1

	update LogRegistrasiDetail set status = 2
	where replace(CONVERT(VARCHAR(10), CONVERT(date, replace(left(Datetime,10),'/','-'), 105), 23),'-','') = FORMAT(GETDATE() , 'yyyyMMdd')
	and CashierBy = @NamaUser and ComputerName =@ComputerName and status = 1

	update LogTicketDetail set Status = 2
	where replace(CONVERT(VARCHAR(10), CONVERT(date, replace(left(Datetime,10),'/','-'), 105), 23),'-','') = FORMAT(GETDATE() , 'yyyyMMdd')
	and ChasierBy = @NamaUser and ComputerName = @ComputerName and Status = 1

	update LogFoodcourtTransaksi set Status = 2
	where replace(CONVERT(VARCHAR(10), CONVERT(date, replace(left(Datetime,10),'/','-'), 105), 23),'-','') = FORMAT(GETDATE() , 'yyyyMMdd')
	and CashierBy = @NamaUser and ComputerName = @ComputerName and Status = 1

	update [LogItemsF&BTrx] set Status = 2
	where replace(CONVERT(VARCHAR(10), CONVERT(date, replace(left(Datetime,10),'/','-'), 105), 23),'-','') = FORMAT(GETDATE() , 'yyyyMMdd')
	and Chasierby = @NamaUser and ComputerName = @ComputerName and Status = 1

	select 'TRUE' as Success, 'Merchant berhasil melakukan closing' as _Message
END














