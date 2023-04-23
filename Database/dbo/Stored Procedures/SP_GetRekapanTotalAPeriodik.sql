Create PROCEDURE [dbo].[SP_GetRekapanTotalAPeriodik]
	@SetAwal nvarchar(max),
	@SetAkhir nvarchar(max),
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
			JenisTransaksi from [dbo].[LogRegistrasiDetail] 
			where 
			--LEFT(Datetime,10) = @SetTanggal
			replace(CONVERT(VARCHAR(10), CONVERT(date, replace(left(Datetime,10),'/','-'), 105), 23),'-','') 
			between replace(CONVERT(VARCHAR(10), CONVERT(date, replace(@SetAwal,'/','-'), 105), 23),'-','') 
			and replace(CONVERT(VARCHAR(10), CONVERT(date, replace(@SetAkhir,'/','-'), 105), 23),'-','') 
			group by JenisTransaksi
			union all
			select 
			sum(isnull(TotalDebit,0)) TotalDebit,sum(isnull(PayCash,0)) PayCash,0 PayEmoney,PaymentMethod from LogTopupDetail
			where 
			--LEFT(Datetime,10) = @SetTanggal
			replace(CONVERT(VARCHAR(10), CONVERT(date, replace(left(Datetime,10),'/','-'), 105), 23),'-','') 
			between replace(CONVERT(VARCHAR(10), CONVERT(date, replace(@SetAwal,'/','-'), 105), 23),'-','') 
			and replace(CONVERT(VARCHAR(10), CONVERT(date, replace(@SetAkhir,'/','-'), 105), 23),'-','') 
			group by PaymentMethod
		) q
		group by q.JenisTransaksi	
	)w
	where w.JenisTransaksi = @Payment
END










