-- Batch submitted through debugger: dbewats.sql|2150|0|C:\Users\Administrator\Desktop\dbewats.sql

CREATE PROCEDURE [dbo].[WebReportSP_RefundSummary]
	@SetAwal nvarchar(max),
	@SetAkhir nvarchar(max)
AS
BEGIN
	SET NOCOUNT ON;
	select
	LEFT(Datetime,10) Datetime,
	sum(SaldoEmoney) SaldoEmoney,
	sum(SaldoJaminan) SaldoJaminan,
	sum(TicketWeekDay) TicketWeekDay,
	sum(TicketWeekEnd) TicketWeekEnd,
	sum(TotalNominalRefund) TotalNominalRefund
	from LogRefundDetail
	where Status = 1
	and 
	--LEFT(Datetime,10) between @SetAwal and @SetAkhir
	replace(CONVERT(VARCHAR(10), CONVERT(date, replace(left(Datetime,10),'/','-'), 105), 23),'-','')
	between 
	replace(CONVERT(VARCHAR(10), CONVERT(date, replace(@SetAwal,'/','-'), 105), 23),'-','') 
	and replace(CONVERT(VARCHAR(10), CONVERT(date, replace(@SetAkhir,'/','-'), 105), 23),'-','')


	group by LEFT(Datetime,10) 
END











