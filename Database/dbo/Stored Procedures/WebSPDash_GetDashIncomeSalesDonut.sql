-- Batch submitted through debugger: dbewats.sql|3818|0|C:\Users\Administrator\Desktop\dbewats.sql
--WebSPDash_GetDashIncomeSalesDonut '10/11/2018','10/11/2018'
CREATE PROCEDURE [dbo].[WebSPDash_GetDashIncomeSalesDonut]
	@SetAwal  nvarchar(50),
	@SetAkhir  nvarchar(50)
AS
BEGIN
	SET NOCOUNT ON;
	if(@SetAwal = @SetAkhir)
	begin
		select q.Datetime,sum(q.Total) Total from 
		(
			select
				left(right(Datetime,8),2)+':00' Datetime,
				sum(TotalBayar) Total
			from LogRegistrasiDetail
			where replace(CONVERT(VARCHAR(10), CONVERT(date, replace(left(Datetime,10),'/','-'), 105), 23),'-','')
			between 
			replace(CONVERT(VARCHAR(10), CONVERT(date, replace(@SetAwal,'/','-'), 105), 23),'-','') 
			and replace(CONVERT(VARCHAR(10), CONVERT(date, replace(@SetAkhir,'/','-'), 105), 23),'-','')
			group by left(right(Datetime,8),2)
			union all
			select
				left(right(Datetime,8),2)+':00' Datetime,
				sum(TotalBayar) Total
			from LogTopupDetail
			where replace(CONVERT(VARCHAR(10), CONVERT(date, replace(left(Datetime,10),'/','-'), 105), 23),'-','')
			between 
			replace(CONVERT(VARCHAR(10), CONVERT(date, replace(@SetAwal,'/','-'), 105), 23),'-','') 
			and replace(CONVERT(VARCHAR(10), CONVERT(date, replace(@SetAkhir,'/','-'), 105), 23),'-','')
			group by left(right(Datetime,8),2)
			union all
			select
				left(right(Datetime,8),2)+':00' Datetime,
				sum(TotalBayar) Total
			from LogFoodcourtTransaksi
			where replace(CONVERT(VARCHAR(10), CONVERT(date, replace(left(Datetime,10),'/','-'), 105), 23),'-','')
			between 
			replace(CONVERT(VARCHAR(10), CONVERT(date, replace(@SetAwal,'/','-'), 105), 23),'-','') 
			and replace(CONVERT(VARCHAR(10), CONVERT(date, replace(@SetAkhir,'/','-'), 105), 23),'-','')
			group by left(right(Datetime,8),2)
			union all
			select
				left(right(Datetime,8),2)+':00' Datetime,
				sum(TotalNominalRefund) Total
			from LogRefundDetail
			where replace(CONVERT(VARCHAR(10), CONVERT(date, replace(left(Datetime,10),'/','-'), 105), 23),'-','')
			between 
			replace(CONVERT(VARCHAR(10), CONVERT(date, replace(@SetAwal,'/','-'), 105), 23),'-','') 
			and replace(CONVERT(VARCHAR(10), CONVERT(date, replace(@SetAkhir,'/','-'), 105), 23),'-','')
			group by left(right(Datetime,8),2)
		) q
		group by q.Datetime
	end
	else
	begin
		select q.Datetime,sum(q.Total) Total from 
		(
			select
				left(Datetime,10) Datetime,
				sum(TotalBayar) Total
			from LogRegistrasiDetail
			where replace(CONVERT(VARCHAR(10), CONVERT(date, replace(left(Datetime,10),'/','-'), 105), 23),'-','')
			between 
			replace(CONVERT(VARCHAR(10), CONVERT(date, replace(@SetAwal,'/','-'), 105), 23),'-','') 
			and replace(CONVERT(VARCHAR(10), CONVERT(date, replace(@SetAkhir,'/','-'), 105), 23),'-','')
			group by left(Datetime,10)
			union all
			select
				left(Datetime,10) Datetime,
				sum(TotalBayar) Total
			from LogTopupDetail
			where replace(CONVERT(VARCHAR(10), CONVERT(date, replace(left(Datetime,10),'/','-'), 105), 23),'-','')
			between 
			replace(CONVERT(VARCHAR(10), CONVERT(date, replace(@SetAwal,'/','-'), 105), 23),'-','') 
			and replace(CONVERT(VARCHAR(10), CONVERT(date, replace(@SetAkhir,'/','-'), 105), 23),'-','')
			group by left(Datetime,10)
			union all
			select
				left(Datetime,10) Datetime,
				sum(TotalBayar) Total
			from LogFoodcourtTransaksi
			where replace(CONVERT(VARCHAR(10), CONVERT(date, replace(left(Datetime,10),'/','-'), 105), 23),'-','')
			between 
			replace(CONVERT(VARCHAR(10), CONVERT(date, replace(@SetAwal,'/','-'), 105), 23),'-','') 
			and replace(CONVERT(VARCHAR(10), CONVERT(date, replace(@SetAkhir,'/','-'), 105), 23),'-','')
			group by left(Datetime,10)
			union all
			select
				left(Datetime,10) Datetime,
				sum(TotalNominalRefund) Total
			from LogRefundDetail
			where replace(CONVERT(VARCHAR(10), CONVERT(date, replace(left(Datetime,10),'/','-'), 105), 23),'-','')
			between 
			replace(CONVERT(VARCHAR(10), CONVERT(date, replace(@SetAwal,'/','-'), 105), 23),'-','') 
			and replace(CONVERT(VARCHAR(10), CONVERT(date, replace(@SetAkhir,'/','-'), 105), 23),'-','')
			group by left(Datetime,10)
		) q
		group by q.Datetime
	end
END












