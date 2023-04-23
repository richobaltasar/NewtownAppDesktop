-- Batch submitted through debugger: dbewats.sql|4178|0|C:\Users\Administrator\Desktop\dbewats.sql

--WebSPDash_TransactionPerJenisTransaksi '',''
CREATE PROCEDURE [dbo].[WebSPDash_TransactionPerJenisTransaksi]
	@SetAwal nvarchar(50),
	@SetAkhir nvarchar(50)
AS
BEGIN
	SET NOCOUNT ON;
	--set @setAwal = '10/11/2018'
	--set @setAkhir = '10/11/2018'

	select 'Ticket Sales' JenisTrx ,sum(TotalBeliTiket) Total from LogRegistrasiDetail 
	where	replace(CONVERT(VARCHAR(10), CONVERT(date, replace(left(Datetime,10),'/','-'), 105), 23),'-','')
			between 
			replace(CONVERT(VARCHAR(10), CONVERT(date, replace(@SetAwal,'/','-'), 105), 23),'-','') 
			and replace(CONVERT(VARCHAR(10), CONVERT(date, replace(@SetAkhir,'/','-'), 105), 23),'-','')
	union all
	select q.JenisTrx,sum(q.Total) Total from
	(
		select
		'Jaminan Gelang' JenisTrx,
		case when SaldoJaminan = 0 then SaldoJaminanAfter else 0 end as Total
		from LogRegistrasiDetail
		where replace(CONVERT(VARCHAR(10), CONVERT(date, replace(left(Datetime,10),'/','-'), 105), 23),'-','')
			between 
			replace(CONVERT(VARCHAR(10), CONVERT(date, replace(@setAwal,'/','-'), 105), 23),'-','') 
			and replace(CONVERT(VARCHAR(10), CONVERT(date, replace(@SetAkhir,'/','-'), 105), 23),'-','')
	) q
	group by q.JenisTrx
	union all
	select 'Asuransi' JenisTrx ,sum(Asuransi) Total from LogRegistrasiDetail 
	where replace(CONVERT(VARCHAR(10), CONVERT(date, replace(left(Datetime,10),'/','-'), 105), 23),'-','')
			between 
			replace(CONVERT(VARCHAR(10), CONVERT(date, replace(@SetAwal,'/','-'), 105), 23),'-','') 
			and replace(CONVERT(VARCHAR(10), CONVERT(date, replace(@SetAkhir,'/','-'), 105), 23),'-','')
	union all
	select 'Topup' JenisTrx,sum(TotalBayar) Total  from LogTopupDetail 
	where replace(CONVERT(VARCHAR(10), CONVERT(date, replace(left(Datetime,10),'/','-'), 105), 23),'-','')
			between 
			replace(CONVERT(VARCHAR(10), CONVERT(date, replace(@SetAwal,'/','-'), 105), 23),'-','') 
			and replace(CONVERT(VARCHAR(10), CONVERT(date, replace(@SetAkhir,'/','-'), 105), 23),'-','')
	union all
	select 'Foodcourt' JenisTrx, sum(TotalBayar) Total from LogFoodcourtTransaksi
	where replace(CONVERT(VARCHAR(10), CONVERT(date, replace(left(Datetime,10),'/','-'), 105), 23),'-','')
			between 
			replace(CONVERT(VARCHAR(10), CONVERT(date, replace(@SetAwal,'/','-'), 105), 23),'-','') 
			and replace(CONVERT(VARCHAR(10), CONVERT(date, replace(@SetAkhir,'/','-'), 105), 23),'-','')
	union all
	select 'Refund' JenisTrx, sum(TotalNominalRefund) Total from LogRefundDetail
	where replace(CONVERT(VARCHAR(10), CONVERT(date, replace(left(Datetime,10),'/','-'), 105), 23),'-','')
			between 
			replace(CONVERT(VARCHAR(10), CONVERT(date, replace(@SetAwal,'/','-'), 105), 23),'-','') 
			and replace(CONVERT(VARCHAR(10), CONVERT(date, replace(@SetAkhir,'/','-'), 105), 23),'-','')
END













