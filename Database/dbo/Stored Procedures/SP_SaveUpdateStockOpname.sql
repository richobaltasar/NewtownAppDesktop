-- Batch submitted through debugger: dbewats.sql|1845|0|C:\Users\Administrator\Desktop\dbewats.sql

CREATE PROCEDURE [dbo].[SP_SaveUpdateStockOpname]
	@idItem bigint,
	@NamaTenant nvarchar(max),
	@NamaItem nvarchar(max),
	@BykStok bigint,
	@BykStokUpdate bigint,
	@NamaUser nvarchar(max)
AS
BEGIN
	SET NOCOUNT ON;
	if exists(select*from DataBarang where idMenu = @idItem)
	begin
		update DataBarang set Stok = @BykStokUpdate where idMenu = @idItem
		insert into [dbo].[LogStokOpname]
		(Datetime,NamaTenant,NamaItem,StockSebelumnya,StockUpdate,UpdateBy,Status)
		values
		(
			FORMAT(GETDATE() , 'dd/MM/yyyy HH:mm:ss'),
			@NamaTenant,@NamaItem,@BykStok,@BykStokUpdate,@NamaUser,1
		)
		select 'TRUE' as Success, 'Insert SP_SaveUpdateStockOpname berhasil dilakukan' as _Message
	end
	else
	begin
		select 'FALSE' as Success, 'Insert SP_SaveUpdateStockOpname gagal dilakukan' as _Message
	end
END










