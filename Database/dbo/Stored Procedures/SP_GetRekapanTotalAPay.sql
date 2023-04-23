CREATE PROCEDURE [dbo].[SP_GetRekapanTotalAPay]
	@setTanggal nvarchar(max),
	@Payment nvarchar(max),
	@Pay nvarchar(max)
AS
BEGIN
	SET NOCOUNT ON;
	select w.PayCash,w.PayEmoney,w.TotalDebit into #temp from
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
			replace(CONVERT(VARCHAR(10), CONVERT(date, replace(left(Datetime,10),'/','-'), 105), 23),'-','') = replace(CONVERT(VARCHAR(10), CONVERT(date, replace(@setTanggal,'/','-'), 105), 23),'-','') 
			group by JenisTransaksi
			union all
			select 
			sum(isnull(TotalDebit,0)) TotalDebit,sum(isnull(PayCash,0)) PayCash,0 PayEmoney,PaymentMethod from LogTopupDetail
			where 
			--LEFT(Datetime,10) = @SetTanggal
			replace(CONVERT(VARCHAR(10), CONVERT(date, replace(left(Datetime,10),'/','-'), 105), 23),'-','') = replace(CONVERT(VARCHAR(10), CONVERT(date, replace(@setTanggal,'/','-'), 105), 23),'-','') 
			group by PaymentMethod
		) q
		group by q.JenisTransaksi	
	)w
	where w.JenisTransaksi = 'eMoney & Cash'

	if(@Pay = 'Cash')
	begin
		select PayCash Jumlah from #temp
	end
	else
	begin
		select PayEmoney Jumlah from #temp
	end
	
	drop table #temp
END










