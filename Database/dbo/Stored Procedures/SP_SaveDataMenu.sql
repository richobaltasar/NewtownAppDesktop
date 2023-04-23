-- Batch submitted through debugger: dbewats.sql|1334|0|C:\Users\Administrator\Desktop\dbewats.sql

CREATE PROCEDURE [dbo].[SP_SaveDataMenu]
	@idMenu bigint,
	@NamaMenu nvarchar(max),
	@Category nvarchar(max),
	@Harga float,
	@HargaJual float,
	@Stok float,
	@ImgLink nvarchar(max),
	@Status nvarchar(50)
AS
BEGIN
	SET NOCOUNT ON;
	if exists(select*from DataBarang where idMenu = @idMenu)
	begin
		declare @stat int  
		if(@Status = 'Aktif')
		begin
			set @stat = 1
		end
		else
		begin
			set @stat = 0
		end

		update DataBarang
		set NamaMenu = @NamaMenu,
		Harga = @Harga,
		HargaJual = @HargaJual,
		Stok = @Stok,
		ImgLink = @ImgLink,
		Status = @stat,
		Category = @Category
		where idMenu = @idMenu

		select 'Update success' as _Message, 'TRUE' as Success
	end
	else
	begin
		select 'Update Faild' as _Message, 'False' as Success
	end
END











