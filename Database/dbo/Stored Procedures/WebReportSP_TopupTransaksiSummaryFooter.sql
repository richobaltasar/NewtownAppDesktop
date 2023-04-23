CREATE PROCEDURE [dbo].[WebReportSP_TopupTransaksiSummaryFooter]
	@SetAwal nvarchar(max),
	@SetAkhir nvarchar(max)
AS
BEGIN
	SET NOCOUNT ON;
	select
		sum(c.NominalTopup) NominalTopup,
		sum(c.TerimaUang) TerimaUang,
		sum(c.Kembalian) Kembalian
	from
	(
    select
	left(Datetime,10) Datetime,
	sum(NominalTopup) NominalTopup,
	sum(TerimaUang) TerimaUang,
	sum(Kembalian) Kembalian
	from LogTopupDetail
	where 
	replace(CONVERT(VARCHAR(10), CONVERT(date, replace(left(Datetime,10),'/','-'), 105), 23),'-','')
	between 
	replace(CONVERT(VARCHAR(10), CONVERT(date, replace(@SetAwal,'/','-'), 105), 23),'-','') 
	and replace(CONVERT(VARCHAR(10), CONVERT(date, replace(@SetAkhir,'/','-'), 105), 23),'-','')
	group by left(Datetime,10)
	) c


END













