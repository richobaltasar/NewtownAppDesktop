CREATE PROCEDURE [dbo].[WebReportSP_RegistrasiReportSummary]
	@SetAwal nvarchar(max),
	@SetAkhir nvarchar(max)
AS
BEGIN
	SET NOCOUNT ON;
	select
	left(Datetime,10) tgl,
	JenisTransaksi,
	sum(SaldoEmoney) SaldoEmoney,
	sum(SaldoEmoneyAfter) SaldoEmoneyAfter,
	--sum(TicketWeekDay) TicketWeekDay,
	sum(TicketWeekDayAfter) TicketWeekDay,
	--sum(TicketWeekEnd) TicketWeekEnd,
	sum(TicketWeekEndAfter) TicketWeekEndAfter,
	sum(SaldoJaminan) SaldoJaminan,
	sum(SaldoJaminanAfter) SaldoJaminanAfter,
	sum(Cashback) Cashback,
	sum(Topup) Topup,
	sum(Asuransi) Asuransi,
	sum(QtyTotalTiket) QtyTotalTiket,
	sum(TotalBeliTiket) TotalBeliTiket,
	sum(TotalAll) TotalAll,
	sum(TotalBayar) TotalBayar,
	sum(PayEmoney) PayEmoney,
	sum(PayCash) PayCash,
	sum(TerimaUang) TerimaUang,
	sum(Kembalian) Kembalian
	From LogRegistrasiDetail
	where 
	--status = 1 and 
	--left(Datetime,10) between @SetAwal and @SetAkhir
	replace(CONVERT(VARCHAR(10), CONVERT(date, replace(left(Datetime,10),'/','-'), 105), 23),'-','')
	between 
	replace(CONVERT(VARCHAR(10), CONVERT(date, replace(@SetAwal,'/','-'), 105), 23),'-','') 
	and replace(CONVERT(VARCHAR(10), CONVERT(date, replace(@SetAkhir,'/','-'), 105), 23),'-','')


	group by left(Datetime,10),JenisTransaksi
	order by left(Datetime,10) asc
END












