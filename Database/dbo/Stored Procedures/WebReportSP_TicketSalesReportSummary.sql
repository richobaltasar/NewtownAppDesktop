CREATE PROCEDURE [dbo].[WebReportSP_TicketSalesReportSummary]
	@SetAwal nvarchar(max),
	@SetAkhir nvarchar(max)
AS
BEGIN
	SET NOCOUNT ON;
	select
	left(Datetime,10) tgl,
	NamaTicket,
	sum(Qty) Qty,
	sum(Total) Total,
	sum(TotalDiskon) TotalDiskon,
	sum(TotalAfterDiskon) TotalAfterDiskon
	from LogTicketDetail
	where 
	--Status = 1 and 
	--left(Datetime,10) between @SetAwal and @SetAkhir 
	replace(CONVERT(VARCHAR(10), CONVERT(date, replace(left(Datetime,10),'/','-'), 105), 23),'-','')
	between 
	replace(CONVERT(VARCHAR(10), CONVERT(date, replace(@SetAwal,'/','-'), 105), 23),'-','') 
	and replace(CONVERT(VARCHAR(10), CONVERT(date, replace(@SetAkhir,'/','-'), 105), 23),'-','')
	group by left(Datetime,10),NamaTicket
END












