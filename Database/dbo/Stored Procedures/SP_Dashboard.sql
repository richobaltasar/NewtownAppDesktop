CREATE PROCEDURE [dbo].[SP_Dashboard]
	@ComputerName nvarchar(max),
	@Username nvarchar(max)
AS
BEGIN
	SET NOCOUNT ON;
	declare @TotalRefund float
	declare @TotalTopup float
	declare @TotalRegis float
	
	declare @TotalFoodCourtCash float
	declare @TotalFoodCourtEmoney float
	declare @TotalTicketPayEmoney float

	declare @TotalDanaModal float
	declare @TotalChasin float
	declare @TotalChasout float
	declare @TotalUangDiBox float
	declare @TotalAllTicket float
	declare @TotalTransaksi float

	declare @TotalNominalEdcRegis float
	declare @TotalNominalEdcTopup float
	declare @TotalTrxEDC float
	declare @TotalNominalDebit float

	declare @TotalTrxEmoney float
	declare @TotalNominalDebitEmoney float

	declare @TotalRegistCount float
	declare @TotalRefundCount float

	set @TotalRefund = isnull((select sum(TotalNominalRefund) as TotalRefund from LogRefundDetail where replace(CONVERT(VARCHAR(10), CONVERT(date, replace(left(Datetime,10),'/','-'), 105), 23),'-','') = FORMAT(GETDATE() , 'yyyyMMdd') and ComputerName = @ComputerName and Status = 1 and ChasierBy = @Username),0)
	set @TotalTopup = isnull((select sum(NominalTopup) as TotalTopup from LogTopupDetail where replace(CONVERT(VARCHAR(10), CONVERT(date, replace(left(Datetime,10),'/','-'), 105), 23),'-','') = FORMAT(GETDATE() , 'yyyyMMdd') and ComputerName = @ComputerName and Status = 1 and Chasierby = @Username),0)
	set @TotalRegis = isnull((select sum(TotalAll) as TotalRegis from [dbo].[LogRegistrasiDetail] where replace(CONVERT(VARCHAR(10), CONVERT(date, replace(left(Datetime,10),'/','-'), 105), 23),'-','') = FORMAT(GETDATE() , 'yyyyMMdd') and ComputerName = @ComputerName and Status = 1 and CashierBy=@Username),0)
	set @TotalTicketPayEmoney = isnull((select sum(PayEmoney) as TotalRegis from [dbo].[LogRegistrasiDetail] where replace(CONVERT(VARCHAR(10), CONVERT(date, replace(left(Datetime,10),'/','-'), 105), 23),'-','') = FORMAT(GETDATE() , 'yyyyMMdd') and ComputerName = @ComputerName and Status = 1 and CashierBy=@Username),0)

	set @TotalFoodCourtCash = ISNULL((select sum(TotalBayar) as TotalBayar from [dbo].[LogFoodcourtTransaksi] where replace(CONVERT(VARCHAR(10), CONVERT(date, replace(left(Datetime,10),'/','-'), 105), 23),'-','') = FORMAT(GETDATE() , 'yyyyMMdd') and ComputerName= @ComputerName and Status = 1 and CashierBy = @Username and upper(JenisTransaksi) = 'CASH'),0)
	set @TotalFoodCourtEmoney = ISNULL((select sum(TotalBayar) as TotalBayar from [dbo].[LogFoodcourtTransaksi] where replace(CONVERT(VARCHAR(10), CONVERT(date, replace(left(Datetime,10),'/','-'), 105), 23),'-','') = FORMAT(GETDATE() , 'yyyyMMdd') and ComputerName= @ComputerName and Status = 1 and CashierBy = @Username and upper(JenisTransaksi) = 'EMONEY'),0)

	set @TotalDanaModal = ISNULL((select DanaModalSetelah from DataChasierBox where NamaComputer = @ComputerName and replace(CONVERT(VARCHAR(10), CONVERT(date, replace(left(Datetime,10),'/','-'), 105), 23),'-','') = FORMAT(GETDATE() , 'yyyyMMdd') and status=1 and OpenBy = @Username),0)
	set @TotalChasin = ISNULL((select TotalUangMasuk from DataChasierBox where NamaComputer = @ComputerName and replace(CONVERT(VARCHAR(10), CONVERT(date, replace(left(Datetime,10),'/','-'), 105), 23),'-','') = FORMAT(GETDATE() , 'yyyyMMdd') and status =1 and OpenBy = @Username),0)

	set @TotalChasout = ISNULL((select TotalUangKeluar from DataChasierBox where NamaComputer = @ComputerName and replace(CONVERT(VARCHAR(10), CONVERT(date, replace(left(Datetime,10),'/','-'), 105), 23),'-','') = FORMAT(GETDATE() , 'yyyyMMdd') and status =1 and OpenBy = @Username),0)
	set @TotalUangDiBox = ISNULL((select TotalUangDiBox from DataChasierBox where NamaComputer = @ComputerName and replace(CONVERT(VARCHAR(10), CONVERT(date, replace(left(Datetime,10),'/','-'), 105), 23),'-','') = FORMAT(GETDATE() , 'yyyyMMdd')and status =1 and OpenBy = @Username),0)
	set @TotalAllTicket = ISNULL((select sum(QtyTotalTiket) from LogRegistrasiDetail where ComputerName = @ComputerName and replace(CONVERT(VARCHAR(10), CONVERT(date, replace(left(Datetime,10),'/','-'), 105), 23),'-','') = FORMAT(GETDATE() , 'yyyyMMdd') and CashierBy=@Username and status = 1 ),0)

	set @TotalTransaksi = (@TotalRegis+@TotalTopup)
	
	set @TotalNominalEdcRegis = 
	(
		select isnull(sum(TotalDebit),0) from LogRegistrasiDetail where JenisTransaksi = 'EDC' 
		and ComputerName = @ComputerName and replace(CONVERT(VARCHAR(10), CONVERT(date, replace(left(Datetime,10),'/','-'), 105), 23),'-','') = FORMAT(GETDATE() , 'yyyyMMdd') 
		and CashierBy=@Username and status = 1
	)
	set @TotalNominalEdcTopup = 
	(
		select isnull(sum(TotalDebit),0) from LogTopupDetail where upper(PaymentMethod)='EDC'
		and replace(CONVERT(VARCHAR(10), CONVERT(date, replace(left(Datetime,10),'/','-'), 105), 23),'-','') = FORMAT(GETDATE() , 'yyyyMMdd') 
		and ChasierBy=@Username and status = 1
	)

	set @TotalTrxEDC = 
	((
		select COUNT(idTrx) from LogRegistrasiDetail 
		where Upper(JenisTransaksi) = 'EDC' 
		and ComputerName = @ComputerName 
		and replace(CONVERT(VARCHAR(10), CONVERT(date, replace(left(Datetime,10),'/','-'), 105), 23),'-','') = FORMAT(GETDATE() , 'yyyyMMdd') 
		and CashierBy=@Username and status = 1
	) 
	+ 
	(
		select COUNT(IdTopup) from LogTopupDetail where upper(PaymentMethod)='EDC'
		and replace(CONVERT(VARCHAR(10), CONVERT(date, replace(left(Datetime,10),'/','-'), 105), 23),'-','') = FORMAT(GETDATE() , 'yyyyMMdd') 
		and ChasierBy=@Username and status = 1
	))
	
	set @TotalNominalDebit = (
	(
		select isnull(sum(TotalDebit),0) from LogRegistrasiDetail where upper(JenisTransaksi) = 'EDC' 
		and ComputerName = @ComputerName and replace(CONVERT(VARCHAR(10), CONVERT(date, replace(left(Datetime,10),'/','-'), 105), 23),'-','') = FORMAT(GETDATE() , 'yyyyMMdd') 
		and CashierBy=@Username and status = 1
	)
	+
	(
		select isnull(sum(TotalDebit),0) from LogTopupDetail where upper(PaymentMethod)='EDC'
		and replace(CONVERT(VARCHAR(10), CONVERT(date, replace(left(Datetime,10),'/','-'), 105), 23),'-','') = FORMAT(GETDATE() , 'yyyyMMdd') 
		and ChasierBy=@Username and status = 1
	))


	set @TotalTrxEmoney =  
	(
		select count(idTrx) from LogRegistrasiDetail
		where JenisTransaksi in ('eMoney','eMoney & Cash') 
		and ComputerName = @ComputerName and left(Datetime,10) = FORMAT(GETDATE() , 'dd/MM/yyyy')
		and CashierBy=@Username
		and status = 1
	)

	set @TotalNominalDebitEmoney = (
		select isnull(sum(PayEmoney),0) from LogRegistrasiDetail
		where JenisTransaksi in ('eMoney','eMoney & Cash') 
		and ComputerName = @ComputerName and left(Datetime,10) = FORMAT(GETDATE() , 'dd/MM/yyyy')
		and CashierBy=@Username and status = 1
	)

	set @TotalRegistCount = (
		select count(AccountNumber) from LogRegistrasiDetail 
		where replace(CONVERT(VARCHAR(10), CONVERT(date, replace(left(Datetime,10),'/','-'), 105), 23),'-','') = FORMAT(GETDATE() , 'yyyyMMdd')
		and ComputerName = @ComputerName and CashierBy=@Username
		and status = 1
	)


	set @TotalRefundCount = (
			select count(AccountNumber) from LogRefundDetail 
			where replace(CONVERT(VARCHAR(10), CONVERT(date, replace(left(Datetime,10),'/','-'), 105), 23),'-','') = FORMAT(GETDATE() , 'yyyyMMdd')
			and ComputerName = @ComputerName and ChasierBy=@Username
			and Status = 1
		)

	select 
	@TotalTopup as TotalTopup, 
	@TotalRefund as TotalRefund, 
	@TotalRegis as TotalRegis,
	@TotalFoodCourtCash as TotalFoodcourtCash,
	@TotalFoodCourtEmoney as TotalFoodcourtEmoney,
	@TotalDanaModal as TotalDanaModal,
	@TotalChasin as TotalCashIn,
	@TotalChasout  as TotalCashOut,
	@TotalUangDiBox as TotalCashBox,
	@TotalAllTicket as TotalAllTicket,
	@TotalTransaksi as TotalTransaksi,
	@TotalTrxEDC as TotalTrxEDC,
	@TotalNominalDebit as TotalNominalDebit,
	@TotalTrxEmoney as TotalTrxEmoney,
	@TotalNominalDebitEmoney as TotalNominalDebitEmoney,
	@TotalRegistCount as  TotalRegistCount,
	@TotalRefundCount as TotalRefundCount,
	@TotalTicketPayEmoney as TotalTicketPayEmoney,
	@TotalNominalEdcRegis as TotalNominalEdcRegis,
	@TotalNominalEdcTopup as TotalNominalEdcTopup

end





