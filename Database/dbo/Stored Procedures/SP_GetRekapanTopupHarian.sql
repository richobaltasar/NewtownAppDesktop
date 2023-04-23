CREATE PROCEDURE [dbo].[SP_GetRekapanTopupHarian]
	@SetTanggal nvarchar(max)
AS
BEGIN
	SET NOCOUNT ON;
	
	select r.Uraian,sum(isnull(jumlah,0)) Jumlah,PaymentMethod,sum(Qty) Qty from 
	(
		select q.* from
		(
			select
			'Pembelian Topup ' Uraian,sum(NominalTopup) Jumlah,PaymentMethod,count(AccountNumber) Qty
			from LogTopupDetail
			where 
			--left(Datetime,10) = @SetTanggal --'06/02/2019' --
			replace(CONVERT(VARCHAR(10), CONVERT(date, replace(left(Datetime,10),'/','-'), 105), 23),'-','') = replace(CONVERT(VARCHAR(10), CONVERT(date, replace(@setTanggal,'/','-'), 105), 23),'-','') 
			group by PaymentMethod
		) q

		union all
	
		select q.Uraian,sum(q.Topup) Jumlah,q.PaymentMethod,count(q.AccountNumber) Qty from
		(
			select
			'Pembelian Topup ' Uraian,isnull(Topup,0) Topup,
			case 
				when PayCash > 0 and PayEmoney > 0 then 'Emoney & Cash'
				when PayCash > 0 and PayEmoney = 0 then 'Cash'
				when PayCash = 0 and PayEmoney > 0 then 'Emoney'
				when TotalDebit > 0 then 'EDC'
			end as PaymentMethod,AccountNumber
			from LogRegistrasiDetail 
			where 
			--left(Datetime,10) = @SetTanggal --'06/02/2019'--
			replace(CONVERT(VARCHAR(10), CONVERT(date, replace(left(Datetime,10),'/','-'), 105), 23),'-','') = replace(CONVERT(VARCHAR(10), CONVERT(date, replace(@setTanggal,'/','-'), 105), 23),'-','') 
			and Topup > 0
		) q
		group by PaymentMethod,q.Uraian
	) r
	group by r.PaymentMethod,r.Uraian
	order by r.PaymentMethod
END














