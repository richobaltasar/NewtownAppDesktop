CREATE PROCEDURE [dbo].[SP_GetDataAllTransaksi]
	@ComputerName nvarchar(max),
	@NamaUser nvarchar(max)
AS
BEGIN
	SET NOCOUNT ON;
	select * from 
	(
		select Datetime Datetime,'REG'+cast(idTrx as nvarchar) IdTrx,'Transaksi Registrasi Ticket' JenisTransaksi,
		TotalAll Nominal,CashierBy CashierBy from [dbo].[LogRegistrasiDetail] 
		where 
		--left(Datetime,10) = FORMAT(GETDATE() , 'dd/MM/yyyy')
		replace(CONVERT(VARCHAR(10), CONVERT(date, replace(left(Datetime,10),'/','-'), 105), 23),'-','') = FORMAT(GETDATE() , 'yyyyMMdd')
		and ComputerName = @ComputerName
		and CashierBy = @NamaUser
		and status = 1
		--select*from LogRegistrasiDetail

		union all
		select Datetime,'TOPUP'+cast(IdTopup as nvarchar) IdTrx,'Transaksi Topup' JenisTransaksi,TotalBayar Nominal,
		Chasierby CashierBy from LogTopupDetail 
		where 
		--LEFT(Datetime,10) = FORMAT(GETDATE() , 'dd/MM/yyyy')
		replace(CONVERT(VARCHAR(10), CONVERT(date, replace(left(Datetime,10),'/','-'), 105), 23),'-','') = FORMAT(GETDATE() , 'yyyyMMdd')
		and ComputerName = @ComputerName
		and Chasierby = @NamaUser
		and Status = 1

		union all
		select Datetime,'REFUND'+Cast(IdRefund as nvarchar) IdTrx,'Transaksi Refund' JenisTransaksi,
		TotalNominalRefund Nominal,ChasierBy CashierBy  from [dbo].[LogRefundDetail] 
		where 
		--left(Datetime,10) = FORMAT(GETDATE() , 'dd/MM/yyyy')
		replace(CONVERT(VARCHAR(10), CONVERT(date, replace(left(Datetime,10),'/','-'), 105), 23),'-','') = FORMAT(GETDATE() , 'yyyyMMdd')
		and ComputerName = @ComputerName
		and ChasierBy = @NamaUser
		and Status = 1

		union all
		select Datetime,'FOODCOURT'+cast(IdTrx as nvarchar) IdTrx,'Transaksi Foodcourt' JenisTransaksi,
		TotalBayar Nominal, CashierBy CashierBy from [dbo].[LogFoodcourtTransaksi]
		where 
		--left(Datetime,10) = FORMAT(GETDATE() , 'dd/MM/yyyy')
		replace(CONVERT(VARCHAR(10), CONVERT(date, replace(left(Datetime,10),'/','-'), 105), 23),'-','') = FORMAT(GETDATE() , 'yyyyMMdd')
		and ComputerName = @ComputerName
		and CashierBy = @NamaUser
		and Status = 1
	) as qry
	order by Datetime desc
END













