--sp_helptext SP_GetStockMonitoring
CREATE PROCEDURE [dbo].[SP_GetStockMonitoring]
	@awal nvarchar(max),
	@akhir nvarchar(max)
AS
BEGIN
	SET NOCOUNT ON;
	
	select a.idMenu,a.Category,a.NamaMenu,a.Stok,a.ImgLink,b.NamaTenant 
	from DataBarang a
	left join DataTenant b on b.idTenant = a.IdTenant
	where a.IdTenant in (select idTenant from DataTenant where MonitoringStock = 1 and (FollowTenant = '' or FollowTenant = null or FollowTenant is null))
	order by a.IdTenant asc
END






