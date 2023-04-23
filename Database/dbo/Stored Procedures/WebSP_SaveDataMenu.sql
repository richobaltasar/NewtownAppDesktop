CREATE PROCEDURE [dbo].[WebSP_SaveDataMenu]
	@idMenu bigint,
	@NamaMenu nvarchar(max),
	@Category nvarchar(max),
	@Harga float,
	@HargaJual float,
	@HargaKaryawan float,
	@Stok float,
	@ImgLink nvarchar(max),
	@Status nvarchar(50)
AS
BEGIN
	SET NOCOUNT ON;
	if exists(select*from DataBarang where idMenu = @idMenu)
	begin
		declare @stat int  
		update DataBarang
		set NamaMenu = @NamaMenu,
		Harga = @Harga,
		HargaJual = @HargaJual,
		HargaKaryawan = @HargaKaryawan,
		Stok = @Stok,
		ImgLink = @ImgLink,
		Status = @Status,
		Category = @Category
		where idMenu = @idMenu

		select 'Update data Product' as title, 'Product: '+@NamaMenu+' berhasil diupdate' as message,
		'success' as icon
	end
	else
	begin
		select 'Update Product' as title, 'Product: '+@NamaMenu+' gagal diupdate' as message,
		'error' as icon
	end
END













