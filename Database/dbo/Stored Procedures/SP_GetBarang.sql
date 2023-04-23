CREATE PROCEDURE [dbo].[SP_GetBarang]
	@IdTenant bigint
AS
BEGIN
	
	SET NOCOUNT ON;
	if exists(select*from DataTenant where idTenant = @IdTenant and StatusJual = 'Karyawan' 
	and StatusKepemilikan = 'Management' and FollowTenant != '')
	begin
		declare @IdTenantq bigint
		declare @NamaTenant nvarchar(max)

		set @NamaTenant = (select FollowTenant from DataTenant where idTenant= @idTenant and StatusJual = 'Karyawan' and StatusKepemilikan = 'Management' and FollowTenant != '')
		set @IdTenantq = (select idTenant from DataTenant where NamaTenant = @NamaTenant)

		select idMenu,NamaMenu,HargaKaryawan as HargaJual,
		ImgLink,Stok from DataBarang 
		where IdTenant = @IdTenantq
		and Status = 'Aktif'
	end
	else
	begin
		select idMenu,NamaMenu,HargaJual,ImgLink,Stok from DataBarang 
		where IdTenant = @IdTenant
		and Status = 'Aktif'
	end

	
END








