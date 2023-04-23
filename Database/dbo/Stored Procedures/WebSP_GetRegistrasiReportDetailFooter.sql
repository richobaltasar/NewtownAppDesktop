CREATE PROCEDURE [dbo].[WebSP_GetRegistrasiReportDetailFooter]
	@DatetimeFrom nvarchar(50),
	@DatetimeUntil nvarchar(50),
	@SetFilter nvarchar(max)
AS
BEGIN
	SET NOCOUNT ON;
	if(@SetFilter = 'All')
	begin
		select  
		sum(QtyTotalTiket) QtyTotalTiket,
		sum(Asuransi) Asuransi,
		sum(Cashback) Cashback,
		sum(SaldoJaminan) SaldoJaminan,
		sum(SaldoJaminanAfter) SaldoJaminanAfter,
		sum(Topup) Topup,
		sum(TotalAll) TotalAll,
		sum(TotalBayar) TotalBayar,
		sum(TotalBeliTiket) TotalBeliTiket,
		sum(PayCash) PayCash,
		sum(PayEmoney) PayEmoney,
		sum(Kembalian) Kembalian,
		sum(TotalDebit) TotalDebit
		from LogRegistrasiDetail
		where 
		--left(Datetime,10) between @DatetimeFrom and @DatetimeUntil
		replace(CONVERT(VARCHAR(10), CONVERT(date, replace(left(Datetime,10),'/','-'), 105), 23),'-','')
		between 
		replace(CONVERT(VARCHAR(10), CONVERT(date, replace(@DatetimeFrom,'/','-'), 105), 23),'-','') 
		and replace(CONVERT(VARCHAR(10), CONVERT(date, replace(@DatetimeUntil,'/','-'), 105), 23),'-','')


	end
	else
	begin
		select  
		sum(QtyTotalTiket) QtyTotalTiket,
		sum(Asuransi) Asuransi,
		sum(Cashback) Cashback,
		sum(SaldoJaminan) SaldoJaminan,
		sum(SaldoJaminanAfter) SaldoJaminanAfter,
		sum(Topup) Topup,
		sum(TotalAll) TotalAll,
		sum(TotalBayar) TotalBayar,
		sum(TotalBeliTiket) TotalBeliTiket,
		sum(PayCash) PayCash,
		sum(PayEmoney) PayEmoney,
		sum(Kembalian) Kembalian,
		sum(TotalDebit) TotalDebit
		from LogRegistrasiDetail
		where 
		--left(Datetime,10) between @DatetimeFrom and @DatetimeUntil
		replace(CONVERT(VARCHAR(10), CONVERT(date, replace(left(Datetime,10),'/','-'), 105), 23),'-','')
		between 
		replace(CONVERT(VARCHAR(10), CONVERT(date, replace(@DatetimeFrom,'/','-'), 105), 23),'-','') 
		and replace(CONVERT(VARCHAR(10), CONVERT(date, replace(@DatetimeUntil,'/','-'), 105), 23),'-','')
		and JenisTransaksi = @SetFilter
	end
END












