-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[WebSP_FUpdateStockMan]
	@idMenu bigint,
	@Stok bigint,
	@UpdateStok bigint,
	@UserAdmin nvarchar(max)
AS
BEGIN
	SET NOCOUNT ON;
	if exists(select*from DataBarang where idMenu = @idMenu)
	begin
		declare @NamaTenant nvarchar(max)
		declare @NamaItem nvarchar(max)

		set @NamaTenant = (select distinct b.NamaTenant from DataBarang a
		left join DataTenant b on b.idTenant = a.IdTenant
		where a.idMenu = @idMenu)

		set @NamaItem = (select NamaMenu from DataBarang where idMenu = @idMenu)

		update DataBarang set Stok = @UpdateStok where idMenu = @idMenu
		insert into LogStokOpname
		(Datetime,NamaTenant,NamaItem,StockSebelumnya,StockUpdate,UpdateBy,Status)
		values
		(
			FORMAT(GETDATE() , 'dd/MM/yyyy HH:mm:ss'),@NamaTenant,@NamaItem,@Stok,@UpdateStok,@UserAdmin,1

		)
		select 'Info' as title, 'Update stock menu : '+@NamaItem+' berhasil dilakukan' as message,
		'success' as icon
	end
	else
	begin
		select 'Warning' as title, 'Update stock gagal dilakukan' as message,
		'error' as icon
	end
END







