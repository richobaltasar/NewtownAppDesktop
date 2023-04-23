CREATE PROCEDURE [dbo].[SP_GetFBCounterDashPerMenu]
	@SetAwal nvarchar(max),
	@SetAkhir nvarchar(max)
AS
BEGIN
	SET NOCOUNT ON;
	declare @Setting nvarchar(max)

	set @Setting = (select val3 from DataParam where NamaParam ='DashboardChart' and val1 = 'Setting' 
	and val2 = 'FNBChart' and val7 = '1')
	
	if(@Setting = 'Product')
	begin
		select
		sum(a.Qtx) Qtx,c.NamaTenant+' - '+a.NamaItem Category,@Setting Setting
		from [LogItemsF&BTrx] a
		left join DataBarang b on b.idMenu = a.KodeBarang
		left join DataTenant c on c.idTenant = b.IdTenant
		where 
		--a.status = 1 and 
		replace(CONVERT(VARCHAR(10), CONVERT(date, replace(left(Datetime,10),'/','-'), 105), 23),'-','')
		between 
		replace(CONVERT(VARCHAR(10), CONVERT(date, replace(@SetAwal,'/','-'), 105), 23),'-','') 
		and replace(CONVERT(VARCHAR(10), CONVERT(date, replace(@SetAkhir,'/','-'), 105), 23),'-','')
		group by c.NamaTenant,a.NamaItem
		order by Qtx desc
	end
	else if(@Setting = 'Tenant')
	begin
		--select
		----sum(a.Qtx) Qtx,
		--sum(a.Total) Qtx,c.NamaTenant Category,@Setting Setting
		--from [LogItemsF&BTrx] a
		--left join DataBarang b on b.idMenu = a.KodeBarang
		--left join DataTenant c on c.idTenant = b.IdTenant
		--where a.status = 1 
		--and 
		--replace(CONVERT(VARCHAR(10), CONVERT(date, replace(left(Datetime,10),'/','-'), 105), 23),'-','')
		--between 
		--replace(CONVERT(VARCHAR(10), CONVERT(date, replace(@SetAwal,'/','-'), 105), 23),'-','') 
		--and replace(CONVERT(VARCHAR(10), CONVERT(date, replace(@SetAkhir,'/','-'), 105), 23),'-','')
		--group by c.NamaTenant
		--order by c.NamaTenant desc
		select
		sum(a.Qtx) Qtx,c.NamaTenant+' - '+a.NamaItem Category,@Setting Setting
		from [LogItemsF&BTrx] a
		left join DataBarang b on b.idMenu = a.KodeBarang
		left join DataTenant c on c.idTenant = b.IdTenant
		where 
		--a.status = 1 and 
		replace(CONVERT(VARCHAR(10), CONVERT(date, replace(left(Datetime,10),'/','-'), 105), 23),'-','')
		between 
		replace(CONVERT(VARCHAR(10), CONVERT(date, replace(@SetAwal,'/','-'), 105), 23),'-','') 
		and replace(CONVERT(VARCHAR(10), CONVERT(date, replace(@SetAkhir,'/','-'), 105), 23),'-','')
		and c.StatusKepemilikan = 'Management'
		group by c.NamaTenant,a.NamaItem
		order by Qtx desc

		--select*from DataTenant
	end
END










