CREATE PROCEDURE [dbo].[WebReportSP_GetRegistrasiReportSummaryFooter]
	@SetAwal nvarchar(max),
	@SetAkhir nvarchar(max)
AS
BEGIN
	SET NOCOUNT ON;
	select 
		sum(qry.SaldoEmoney) SaldoEmoney,
		sum(qry.SaldoEmoneyAfter) SaldoEmoneyAfter,
		--sum(TicketWeekDay) TicketWeekDay,
		sum(qry.TicketWeekDayAfter) TicketWeekDay,
		--sum(TicketWeekEnd) TicketWeekEnd,
		sum(qry.TicketWeekEndAfter) TicketWeekEndAfter,
		sum(qry.SaldoJaminan) SaldoJaminan,
		sum(qry.SaldoJaminanAfter) SaldoJaminanAfter,
		sum(qry.Cashback) Cashback,
		sum(qry.Topup) Topup,
		sum(qry.Asuransi) Asuransi,
		sum(qry.QtyTotalTiket) QtyTotalTiket,
		sum(qry.TotalBeliTiket) TotalBeliTiket,
		sum(qry.TotalAll) TotalAll,
		sum(qry.TotalBayar) TotalBayar,
		sum(qry.PayEmoney) PayEmoney,
		sum(qry.PayCash) PayCash,
		sum(qry.TerimaUang) TerimaUang,
		sum(qry.Kembalian) Kembalian from
	(
		select
		left(Datetime,10) tgl,
		JenisTransaksi,
		sum(SaldoEmoney) SaldoEmoney,
		sum(SaldoEmoneyAfter) SaldoEmoneyAfter,
		--sum(TicketWeekDay) TicketWeekDay,
		sum(TicketWeekDayAfter) TicketWeekDayAfter,
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
	) qry
END













