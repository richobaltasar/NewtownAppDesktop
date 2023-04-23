-- Batch submitted through debugger: dbewats.sql|3593|0|C:\Users\Administrator\Desktop\dbewats.sql

--WebSPDash_GetDashHourlyAllTransaction '11/11/2018','11/11/2018'
CREATE PROCEDURE [dbo].[WebSPDash_GetDashHourlyAllTransaction]
	@setAwal nvarchar(50),
	@setAkhir nvarchar(50)
AS
BEGIN
	SET NOCOUNT ON;
	
	if(@setAwal = @setAkhir)
	begin
		select 
			* into #temp
		from
		(
			select left(datetime,13)+':00' Datetime, 'Ticket Sales' JenisTrx ,sum(TotalBeliTiket) Total from LogRegistrasiDetail 
			where replace(CONVERT(VARCHAR(10), CONVERT(date, replace(left(Datetime,10),'/','-'), 105), 23),'-','')
					between 
					replace(CONVERT(VARCHAR(10), CONVERT(date, replace(@setAwal,'/','-'), 105), 23),'-','') 
					and replace(CONVERT(VARCHAR(10), CONVERT(date, replace(@setAkhir,'/','-'), 105), 23),'-','')
			group by left(datetime,13)
			union all
			select q.Datetime, q.JenisTrx,sum(q.Total) Total from
			(
				select 
				left(datetime,13)+':00' Datetime,
				'Jaminan Gelang' JenisTrx,
				case when SaldoJaminan = 0 then SaldoJaminanAfter else 0 end as Total
				from LogRegistrasiDetail
				where replace(CONVERT(VARCHAR(10), CONVERT(date, replace(left(Datetime,10),'/','-'), 105), 23),'-','')
					between 
					replace(CONVERT(VARCHAR(10), CONVERT(date, replace(@setAwal,'/','-'), 105), 23),'-','') 
					and replace(CONVERT(VARCHAR(10), CONVERT(date, replace(@setAkhir,'/','-'), 105), 23),'-','')
			) q
			group by q.JenisTrx,q.Datetime
			union all
			select left(datetime,13)+':00' Datetime, 'Asuransi' JenisTrx ,sum(Asuransi) Total from LogRegistrasiDetail 
			where replace(CONVERT(VARCHAR(10), CONVERT(date, replace(left(Datetime,10),'/','-'), 105), 23),'-','')
				between 
				replace(CONVERT(VARCHAR(10), CONVERT(date, replace(@setAwal,'/','-'), 105), 23),'-','') 
				and replace(CONVERT(VARCHAR(10), CONVERT(date, replace(@setAkhir,'/','-'), 105), 23),'-','')
			group by left(datetime,13)
			union all
			select left(datetime,13)+':00' Datetime,'Topup' JenisTrx,sum(TotalBayar) Total  from LogTopupDetail 
			where replace(CONVERT(VARCHAR(10), CONVERT(date, replace(left(Datetime,10),'/','-'), 105), 23),'-','')
			between 
			replace(CONVERT(VARCHAR(10), CONVERT(date, replace(@setAwal,'/','-'), 105), 23),'-','') 
			and replace(CONVERT(VARCHAR(10), CONVERT(date, replace(@setAkhir,'/','-'), 105), 23),'-','')
			group by left(datetime,13)
			union all
			select left(datetime,13)+':00' Datetime, 'Foodcourt' JenisTrx, sum(TotalBayar) Total from LogFoodcourtTransaksi
			where replace(CONVERT(VARCHAR(10), CONVERT(date, replace(left(Datetime,10),'/','-'), 105), 23),'-','')
			between 
			replace(CONVERT(VARCHAR(10), CONVERT(date, replace(@setAwal,'/','-'), 105), 23),'-','') 
			and replace(CONVERT(VARCHAR(10), CONVERT(date, replace(@setAkhir,'/','-'), 105), 23),'-','')
			group by left(datetime,13)
			union all
			select left(datetime,13)+':00' Datetime,'Refund' JenisTrx, sum(TotalNominalRefund) Total from LogRefundDetail
			where replace(CONVERT(VARCHAR(10), CONVERT(date, replace(left(Datetime,10),'/','-'), 105), 23),'-','')
			between 
			replace(CONVERT(VARCHAR(10), CONVERT(date, replace(@setAwal,'/','-'), 105), 23),'-','') 
			and replace(CONVERT(VARCHAR(10), CONVERT(date, replace(@setAkhir,'/','-'), 105), 23),'-','')
			group by left(datetime,13)
		) e
		select right(r.Datetime,5) Datetime, 
			sum(r.TicketSales) TicketSales,  
			sum(r.JaminanGelang) JaminanGelang,
			sum(r.Asuransi) Asuransi,
			sum(r.Topup) Topup,
			sum(r.Foodcourt) Foodcourt,
			sum(r.Refund) Refund
		from
		(
			select
				Datetime,
				case when JenisTrx = 'Ticket Sales' then sum(Total) else 0 end as TicketSales,
				case when JenisTrx = 'Jaminan Gelang' then sum(Total) else 0 end as JaminanGelang,
				case when JenisTrx = 'Asuransi' then sum(Total) else 0 end as Asuransi,
				case when JenisTrx = 'Topup' then sum(Total) else 0 end as Topup,
				case when JenisTrx = 'Foodcourt' then sum(Total) else 0 end as Foodcourt,
				case when JenisTrx = 'Refund' then sum(Total) else 0 end as Refund
			from #temp
			group by Datetime,JenisTrx
		) r
		group by r.Datetime
	end
	else if(@setAwal != @setAkhir)
	begin
		select 
			* into #temp2
		from
		(
			select left(datetime,10) Datetime, 'Ticket Sales' JenisTrx ,sum(TotalBeliTiket) Total from LogRegistrasiDetail 
			where replace(CONVERT(VARCHAR(10), CONVERT(date, replace(left(Datetime,10),'/','-'), 105), 23),'-','')
			between 
			replace(CONVERT(VARCHAR(10), CONVERT(date, replace(@setAwal,'/','-'), 105), 23),'-','') 
			and replace(CONVERT(VARCHAR(10), CONVERT(date, replace(@setAkhir,'/','-'), 105), 23),'-','')
			group by left(datetime,10)
			union all
			select q.Datetime, q.JenisTrx,sum(q.Total) Total from
			(
				select 
				left(datetime,10) Datetime,
				'Jaminan Gelang' JenisTrx,
				case when SaldoJaminan = 0 then SaldoJaminanAfter else 0 end as Total
				from LogRegistrasiDetail
				where left(Datetime,10) between @setAwal and @setAkhir
			) q
			group by q.JenisTrx,q.Datetime
			union all
			select left(datetime,10) Datetime, 'Asuransi' JenisTrx ,sum(Asuransi) Total from LogRegistrasiDetail 
			where replace(CONVERT(VARCHAR(10), CONVERT(date, replace(left(Datetime,10),'/','-'), 105), 23),'-','')
			between 
			replace(CONVERT(VARCHAR(10), CONVERT(date, replace(@setAwal,'/','-'), 105), 23),'-','') 
			and replace(CONVERT(VARCHAR(10), CONVERT(date, replace(@setAkhir,'/','-'), 105), 23),'-','')
			group by left(datetime,10)
			union all
			select left(datetime,10) Datetime,'Topup' JenisTrx,sum(TotalBayar) Total  from LogTopupDetail 
			where replace(CONVERT(VARCHAR(10), CONVERT(date, replace(left(Datetime,10),'/','-'), 105), 23),'-','')
			between 
			replace(CONVERT(VARCHAR(10), CONVERT(date, replace(@setAwal,'/','-'), 105), 23),'-','') 
			and replace(CONVERT(VARCHAR(10), CONVERT(date, replace(@setAkhir,'/','-'), 105), 23),'-','')
			group by left(datetime,10)
			union all
			select left(datetime,10) Datetime, 'Foodcourt' JenisTrx, sum(TotalBayar) Total from LogFoodcourtTransaksi
			where replace(CONVERT(VARCHAR(10), CONVERT(date, replace(left(Datetime,10),'/','-'), 105), 23),'-','')
			between 
			replace(CONVERT(VARCHAR(10), CONVERT(date, replace(@setAwal,'/','-'), 105), 23),'-','') 
			and replace(CONVERT(VARCHAR(10), CONVERT(date, replace(@setAkhir,'/','-'), 105), 23),'-','')
			group by left(datetime,10)
			union all
			select left(datetime,10) Datetime,'Refund' JenisTrx, sum(TotalNominalRefund) Total from LogRefundDetail
			where replace(CONVERT(VARCHAR(10), CONVERT(date, replace(left(Datetime,10),'/','-'), 105), 23),'-','')
			between 
			replace(CONVERT(VARCHAR(10), CONVERT(date, replace(@setAwal,'/','-'), 105), 23),'-','') 
			and replace(CONVERT(VARCHAR(10), CONVERT(date, replace(@setAkhir,'/','-'), 105), 23),'-','')
			group by left(datetime,10)
		) e
		select r.Datetime,
			sum(r.TicketSales) TicketSales,  
			sum(r.JaminanGelang) JaminanGelang,
			sum(r.Asuransi) Asuransi,
			sum(r.Topup) Topup,
			sum(r.Foodcourt) Foodcourt,
			sum(r.Refund) Refund
		from
		(
			select
				Datetime,
				case when JenisTrx = 'Ticket Sales' then sum(Total) else 0 end as TicketSales,
				case when JenisTrx = 'Jaminan Gelang' then sum(Total) else 0 end as JaminanGelang,
				case when JenisTrx = 'Asuransi' then sum(Total) else 0 end as Asuransi,
				case when JenisTrx = 'Topup' then sum(Total) else 0 end as Topup,
				case when JenisTrx = 'Foodcourt' then sum(Total) else 0 end as Foodcourt,
				case when JenisTrx = 'Refund' then sum(Total) else 0 end as Refund
			from #temp2
			group by Datetime,JenisTrx
		) r
		group by r.Datetime
	end

END












