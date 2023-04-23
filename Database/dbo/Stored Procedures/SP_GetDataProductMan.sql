CREATE PROCEDURE [dbo].[SP_GetDataProductMan]
	@search nvarchar(max),
	@idTenant int
AS
BEGIN
	SET NOCOUNT ON;

	if exists(select*from DataTenant where idTenant= @idTenant
	and StatusJual = 'Karyawan' and StatusKepemilikan = 'Management' and FollowTenant != '')
	begin
		declare @IdTenantq bigint
		declare @NamaTenant nvarchar(max)

		set @NamaTenant = (select FollowTenant from DataTenant where idTenant= @idTenant and StatusJual = 'Karyawan' and StatusKepemilikan = 'Management' and FollowTenant != '')
		set @IdTenantq = (select idTenant from DataTenant where NamaTenant = @NamaTenant)

		select a.*,b.NamaTenant,'Karyawan' StatusStock from DataBarang a
		left join DataTenant b on b.idTenant = a.IdTenant
		where a.IdTenant = @IdTenantq
		and NamaMenu like '%'+@search+'%'	
	end
	else
	begin
		select a.*,b.NamaTenant,'Public' StatusStock  from DataBarang a
		left join DataTenant b on b.idTenant = a.IdTenant
		where 
		a.IdTenant = @idTenant --@idTenant
		and NamaMenu like '%'+@search+'%'	
	end
	
END









