CREATE PROCEDURE [dbo].[WebSPDash_GetDashChartProductSales]
	@SetAwal nvarchar(max),
	@SetAkhir nvarchar(max)
AS
BEGIN
	SET NOCOUNT ON;
	
	declare @Setting nvarchar(max)

	set @Setting = (select val3 from ewats.dbo.DataParam where NamaParam ='DashboardChart' and val1 = 'Setting' 
	and val2 = 'FNBChart' and val7 = '1')
	
	if(@Setting = 'Tenant')
	begin
		select
			case when a.NamaTenant is null then '' else a.NamaTenant end as NamaItem,
			sum(Qtx) Qtx
		from dbo.[LogItemsF&BTrx] a
		left join ewats.dbo.DataBarang b on b.idMenu = a.KodeBarang
		left join ewats.dbo.DataTenant c on c.idTenant = b.IdTenant
		where 
		--a.Status = 1 and 
		replace(CONVERT(VARCHAR(10), CONVERT(date, replace(left(Datetime,10),'/','-'), 105), 23),'-','')
		between 
		replace(CONVERT(VARCHAR(10), CONVERT(date, replace(@SetAwal,'/','-'), 105), 23),'-','') 
		and replace(CONVERT(VARCHAR(10), CONVERT(date, replace(@SetAkhir,'/','-'), 105), 23),'-','')
		group by c.NamaTenant,a.NamaTenant
		order by Qtx desc	
	end
	else if(@Setting = 'Product')
	begin
		select
		a.NamaItem,
		sum(Qtx) Qtx
		from [LogItemsF&BTrx] a
		where 
		--a.Status = 1 and 
		replace(CONVERT(VARCHAR(10), CONVERT(date, replace(left(Datetime,10),'/','-'), 105), 23),'-','')
		between 
		replace(CONVERT(VARCHAR(10), CONVERT(date, replace(@SetAwal,'/','-'), 105), 23),'-','') 
		and replace(CONVERT(VARCHAR(10), CONVERT(date, replace(@SetAkhir,'/','-'), 105), 23),'-','')
		group by a.NamaItem
		order by Qtx desc	
	end
	

END










