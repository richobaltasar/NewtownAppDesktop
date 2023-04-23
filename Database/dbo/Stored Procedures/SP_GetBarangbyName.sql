ALTER PROCEDURE [dbo].[SP_GetListSewabyName]
AS
BEGIN
	SET NOCOUNT ON;

	declare @IdTenantq bigint
	declare @NamaTenant nvarchar(max)

	set @NamaTenant = (select FollowTenant from DataTenant where NamaTenant = 'PERSEWAAN' and StatusKepemilikan = 'Management')
	set @IdTenantq = (select idTenant from DataTenant where NamaTenant = 'PERSEWAAN')

	select idMenu,NamaMenu,HargaKaryawan as HargaJual,
	ImgLink,Stok from DataBarang 
	where IdTenant = @IdTenantq

	and Status = 'Aktif'

END








