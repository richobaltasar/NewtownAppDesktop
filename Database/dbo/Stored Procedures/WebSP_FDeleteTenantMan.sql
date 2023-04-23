-- Batch submitted through debugger: dbewats.sql|2551|0|C:\Users\Administrator\Desktop\dbewats.sql

CREATE PROCEDURE [dbo].[WebSP_FDeleteTenantMan]
	@id bigint
AS
BEGIN
	SET NOCOUNT ON;
	declare @title nvarchar(max)
	declare @message nvarchar(max)
	declare @icon nvarchar(max)

	if exists(select*from DataTenant where idTenant = @id)
	begin
		declare @nameTenant nvarchar(max)
		set @nameTenant = (select NamaTenant from DataTenant where idTenant = @id)

		delete from DataTenant
		where idTenant = @id
		set @title='Delete data Success'
		set @message='Data Tenant : '+ @nameTenant +' berhasil dihapus'
		set @icon='success'
	end
	else
	begin
		set @title='Delete data Failed'
		set @message='Data tidak ditemukan'
		set @icon='error'
	end

	select @title title, @message message,@icon icon
END











