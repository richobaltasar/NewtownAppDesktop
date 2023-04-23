-- Batch submitted through debugger: dbewats.sql|7|0|C:\Users\Administrator\Desktop\dbewats.sql

CREATE PROCEDURE [dbo].[GetDashChartProductSales]
	@SetAwal nvarchar(max),
	@SetAkhir nvarchar(max)
AS
BEGIN
	SET NOCOUNT ON;
	
	select
	NamaItem,sum(Qtx) Qtx
	from [LogItemsF&BTrx]
	where Status = 1 
	and 
	replace(CONVERT(VARCHAR(10), CONVERT(date, replace(left(Datetime,10),'/','-'), 105), 23),'-','')
	between 
	replace(CONVERT(VARCHAR(10), CONVERT(date, replace(@SetAwal,'/','-'), 105), 23),'-','') 
	and replace(CONVERT(VARCHAR(10), CONVERT(date, replace(@SetAkhir,'/','-'), 105), 23),'-','')	
	group by NamaItem
	order by Qtx desc

END










