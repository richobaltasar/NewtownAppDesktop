-- Batch submitted through debugger: dbewats.sql|2938|0|C:\Users\Administrator\Desktop\dbewats.sql
CREATE PROCEDURE [dbo].[WebSP_FSaveNewFormSProduct]
	@IdTenant bigint,
	@NamaMenu nvarchar(max),
	@Category nvarchar(max),
	@HargaModal float,
	@HargJual float,
	@Stock float,
	@ImgLink nvarchar(max),
	@Status nvarchar(max)
AS
BEGIN
	SET NOCOUNT ON;
	if not exists(select*from DataBarang where NamaMenu = @NamaMenu and IdTenant = @IdTenant)
	begin
		insert into DataBarang 
		([IdTenant],[NamaMenu],[Category],[Harga],[HargaJual],[Stok],[ImgLink],[DatetimeCreate],[Status]
		)
		values(
			@IdTenant,@NamaMenu,@Category,@HargaModal,@HargJual,@Stock,@ImgLink,FORMAT(GETDATE() , 'dd/MM/yyyy HH:mm:ss'),'Aktif'
		)
		select 'Penambahan Product' as title, 'Product: '+@NamaMenu+' berhasil ditambahkan' as message,
		'success' as icon
	end
	else
	begin
		select 'Penambahan Product' as title, 'Product: '+@NamaMenu+' sudah ada' as message,
		'error' as icon
	end
END












