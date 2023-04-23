CREATE PROCEDURE [dbo].[SP_GetRekapanTotalATahunan]
	@SetTahunan nvarchar(max),
	@Payment nvarchar(max)
AS
BEGIN
	SET NOCOUNT ON;
	select Month(s.Tanggal) Bulan,sum(TotalDebit) TotalDebit,sum(PayCash) PayCash,sum(PayEmoney) PayEmoney into #temp
	from
	(
		select 
			replace(CONVERT(VARCHAR(10), CONVERT(date, replace(left(Datetime,10),'/','-'), 105), 23),'-','/') Tanggal,
			sum(isnull(TotalDebit,0)) TotalDebit,
			sum(isnull(PayCash,0)) PayCash,
			sum(isnull(PayEmoney,0)) PayEmoney, 
			case when JenisTransaksi ='Cash' then 'CASH' else JenisTransaksi end as PaymentMethod
			from [dbo].[LogRegistrasiDetail] 
			where right(left(Datetime,10),4) = @SetTahunan
			group by JenisTransaksi,
			replace(CONVERT(VARCHAR(10), CONVERT(date, replace(left(Datetime,10),'/','-'), 105), 23),'-','/')
			union all
			select 
			replace(CONVERT(VARCHAR(10), CONVERT(date, replace(left(Datetime,10),'/','-'), 105), 23),'-','/') Tanggal,
			sum(isnull(TotalDebit,0)) TotalDebit,
			sum(isnull(PayCash,0)) PayCash,
			0 PayEmoney,isnull(PaymentMethod,'CASH')  PaymentMethod
		from LogTopupDetail
		where right(left(Datetime,10),4) = @SetTahunan
		group by PaymentMethod,replace(CONVERT(VARCHAR(10), CONVERT(date, replace(left(Datetime,10),'/','-'), 105), 23),'-','/')
	) s
	group by Month(s.Tanggal)

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










