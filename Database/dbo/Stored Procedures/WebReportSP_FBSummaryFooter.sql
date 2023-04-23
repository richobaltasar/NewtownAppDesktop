CREATE PROCEDURE [dbo].[WebReportSP_FBSummaryFooter]
	@SetAwal nvarchar(max),
	@SetAkhir nvarchar(max)
AS
BEGIN
	SET NOCOUNT ON;
	select  
		sum(c.Qty) Qty,sum(c.Total) Total
	from
	(
		select left(qry.Datetime,10) Datetime,c.NamaTenant, qry.NamaItem,qry.Harga,sum(qry.Qtx) Qty,sum(qry.Total) Total from
		(
			select a.Datetime,a.NamaItem,a.Harga,a.Qtx,a.Total,a.KodeBarang,b.IdTenant
			from [LogItemsF&BTrx] a
			left join (select idMenu,IdTenant from ewats.dbo.DataBarang) b on b.idMenu = a.KodeBarang
			--where status = 1 
		) qry
		left join ewats.dbo.DataTenant c on c.idTenant = qry.IdTenant
		where 
--		LEFT(Datetime,10) between @SetAwal and @SetAkhir
		replace(CONVERT(VARCHAR(10), CONVERT(date, replace(left(Datetime,10),'/','-'), 105), 23),'-','')
		between 
		replace(CONVERT(VARCHAR(10), CONVERT(date, replace(@SetAwal,'/','-'), 105), 23),'-','') 
		and replace(CONVERT(VARCHAR(10), CONVERT(date, replace(@SetAkhir,'/','-'), 105), 23),'-','')


		group by left(qry.Datetime,10), qry.NamaItem,qry.Harga,c.NamaTenant
	) c
	
END















