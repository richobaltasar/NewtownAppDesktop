
CREATE PROCEDURE [dbo].[WebSPDash_GetDashTotalTopupSales]
	@SetAwal  nvarchar(50),
	@SetAkhir  nvarchar(50)
AS
BEGIN
	SET NOCOUNT ON;

	select 
	sum(TotalBayar) Total
	from LogTopupDetail
	where 
	replace(CONVERT(VARCHAR(10), CONVERT(date, replace(left(Datetime,10),'/','-'), 105), 23),'-','')
	between 
	replace(CONVERT(VARCHAR(10), CONVERT(date, replace(@SetAwal,'/','-'), 105), 23),'-','') 
	and replace(CONVERT(VARCHAR(10), CONVERT(date, replace(@SetAkhir,'/','-'), 105), 23),'-','')

END











