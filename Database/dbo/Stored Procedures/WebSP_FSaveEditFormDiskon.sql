-- Batch submitted through debugger: dbewats.sql|2656|0|C:\Users\Administrator\Desktop\dbewats.sql

CREATE PROCEDURE [dbo].[WebSP_FSaveEditFormDiskon]
	@idPromo nvarchar(50),
	@BerlakuDari nvarchar(50),
	@BerlakuSampai nvarchar(50),
	@CategoryPromo nvarchar(MAX),
	@Diskon float,
	@NamaPromo nvarchar(MAX),
	@ImgLink nvarchar(MAX),
	@Status nvarchar(50)

AS
BEGIN
	SET NOCOUNT ON;
	if exists(select*from DataPromo where idPromo = @idPromo)
	begin
		update DataPromo
		set NamaPromo = @NamaPromo,CategoryPromo=@CategoryPromo,
		Diskon=@Diskon,Status=@Status,BerlakuDari=@BerlakuDari,BerlakuSampai=@BerlakuSampai,
		ImgLink=@ImgLink
		where idPromo = @idPromo

		select 'Perubahan Promo' as title, 'Promo : '+@NamaPromo+' berhasil diupdate' as message,
		'success' as icon
	end
	else
	begin
		select 'Perubahan Promo' as title, 'Data Promo : '+@NamaPromo+' tidak ditemukan' as message,
		'error' as icon
	end
END











