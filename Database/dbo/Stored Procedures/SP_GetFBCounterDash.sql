CREATE PROCEDURE [dbo].[SP_GetFBCounterDash]
	@SetAwal nvarchar(max),
	@SetAkhir nvarchar(max)
AS
BEGIN
	SET NOCOUNT ON;
	select
	sum(a.Qtx) Qtx,b.Category
	from [LogItemsF&BTrx] a
	left join ewats.dbo.DataBarang b on b.idMenu = a.KodeBarang
	where 
	--a.status = 1 and 
	--left(Datetime,10) between @SetAwal and @SetAkhir
	replace(CONVERT(VARCHAR(10), CONVERT(date, replace(left(Datetime,10),'/','-'), 105), 23),'-','')
	between 
	replace(CONVERT(VARCHAR(10), CONVERT(date, replace(@SetAwal,'/','-'), 105), 23),'-','') 
	and replace(CONVERT(VARCHAR(10), CONVERT(date, replace(@SetAkhir,'/','-'), 105), 23),'-','')
	group by b.Category
END











