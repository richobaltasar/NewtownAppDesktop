CREATE PROCEDURE [dbo].[SP_GetRekapanTotalABulanan]
	@SetBulan nvarchar(max),
	@Payment nvarchar(max)
AS
BEGIN
	SET NOCOUNT ON;
	select 
	sum(q.PayCash) PayCash,sum(q.TotalDebit) TotalDebit,sum(q.PayEmoney) PayEmoney,q.JenisTransaksi into #temp from 
	(
		select 
		CONVERT(VARCHAR(10), CONVERT(date, replace(left(Datetime,10),'/','-'), 105), 23) Tanggal,
		sum(isnull(TotalDebit,0)) TotalDebit,
		sum(isnull(PayCash,0)) PayCash,
		sum(isnull(PayEmoney,0)) PayEmoney, 
		case when JenisTransaksi ='Cash' then 'CASH' else JenisTransaksi end as JenisTransaksi
		from [dbo].[LogRegistrasiDetail] 
		where right(left(Datetime,10),7) = @SetBulan
		group by JenisTransaksi,CONVERT(VARCHAR(10), CONVERT(date, replace(left(Datetime,10),'/','-'), 105), 23)
		union all
		select 
		CONVERT(VARCHAR(10), CONVERT(date, replace(left(Datetime,10),'/','-'), 105), 23) Tanggal,
		sum(isnull(TotalDebit,0)) TotalDebit,sum(isnull(PayCash,0)) PayCash,0 PayEmoney,PaymentMethod from LogTopupDetail
		where right(left(Datetime,10),7) = @SetBulan 
		group by PaymentMethod,CONVERT(VARCHAR(10), CONVERT(date, replace(left(Datetime,10),'/','-'), 105), 23)
	) q
	group by q.JenisTransaksi,Tanggal	

	if(@Payment ='CASH')
	begin
		select
			sum(PayCash) Jumlah
		from #temp
	end
	else if(@Payment ='EDC')
	begin
		select
			sum(TotalDebit) Jumlah
		from #temp
	end
	else if(@Payment ='eMoney') 
	begin
		select
			sum(PayEmoney) Jumlah
		from #temp
	end

END










