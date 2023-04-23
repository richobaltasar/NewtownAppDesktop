CREATE PROCEDURE [dbo].[WebSPDash_GetDashTotalFoodCourtSalesPerTenant]
	@SetAwal  nvarchar(50),
	@SetAkhir  nvarchar(50)
AS
BEGIN
	
	SET NOCOUNT ON;

	select sum(total) Total from
	(
		select left(datetime,10) datetime,KodeBarang,sum(total) total
		from [LogItemsF&BTrx]
		where replace(CONVERT(VARCHAR(10), CONVERT(date, replace(left(Datetime,10),'/','-'), 105), 23),'-','')
		between 
		replace(CONVERT(VARCHAR(10), CONVERT(date, replace(@SetAwal,'/','-'), 105), 23),'-','') 
		and replace(CONVERT(VARCHAR(10), CONVERT(date, replace(@SetAkhir,'/','-'), 105), 23),'-','')
		group by left(datetime,10),KodeBarang
	) q
	left join 
	(select a.idMenu,b.NamaTenant from ewats.dbo.DataBarang a
	left join ewats.dbo.DataTenant b on b.idTenant = a.IdTenant) w on w.idMenu = q.KodeBarang
	
END













