CREATE PROCEDURE [dbo].[SP_GetRekapanTotalA]
	@setTanggal nvarchar(max),
	@Payment nvarchar(max)
AS
BEGIN
	SET NOCOUNT ON;
	select (w.PayCash+w.PayEmoney+w.TotalDebit) Jumlah from
	(
		select sum(q.PayCash) PayCash,sum(q.TotalDebit) TotalDebit,sum(q.PayEmoney) PayEmoney,q.JenisTransaksi from 
		(
			select 
			sum(isnull(TotalDebit,0)) TotalDebit,
			sum(isnull(PayCash,0)) PayCash,
			sum(isnull(PayEmoney,0)) PayEmoney, 
			Upper(JenisTransaksi)JenisTransaksi from [dbo].[LogRegistrasiDetail] 
			where 
			replace(CONVERT(VARCHAR(10), CONVERT(date, replace(left(Datetime,10),'/','-'), 105), 23),'-','') = replace(CONVERT(VARCHAR(10), CONVERT(date, replace(@setTanggal,'/','-'), 105), 23),'-','') 
			group by JenisTransaksi

			union all
			select 
			sum(isnull(q1.TotalDebit,0)) TotalDebit,sum(isnull(q1.PayCash,0)) PayCash,sum(isnull(q1.PayEmoney,0)) PayEmoney,q1.PaymentMethod 
			from 
			(
				select 
				case when upper(PaymentMethod) = 'CASH' then isnull(NominalTopup,0) else 0 end as PayCash,
				case when upper(PaymentMethod) = 'EDC' then isnull(TotalDebit,0)  else 0 end as TotalDebit,
				0 PayEmoney,PaymentMethod from LogTopupDetail
				where 
				replace(CONVERT(VARCHAR(10), CONVERT(date, replace(left(Datetime,10),'/','-'), 105), 23),'-','') 
				= replace(CONVERT(VARCHAR(10), CONVERT(date, replace(@setTanggal,'/','-'), 105), 23),'-','') 
			) q1
			group by PaymentMethod
		) q
		group by q.JenisTransaksi	
	)w
	where w.JenisTransaksi = @Payment
END












