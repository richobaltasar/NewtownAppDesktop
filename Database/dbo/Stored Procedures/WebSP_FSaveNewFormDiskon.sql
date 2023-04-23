-- Batch submitted through debugger: dbewats.sql|2897|0|C:\Users\Administrator\Desktop\dbewats.sql

CREATE PROCEDURE [dbo].[WebSP_FSaveNewFormDiskon]
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
	if not exists(select*from DataPromo where NamaPromo = @NamaPromo)
	begin
		insert into DataPromo
		(
			NamaPromo,CategoryPromo,Diskon,Status,BerlakuDari,BerlakuSampai,ImgLink
		)
		values(
			@NamaPromo,@CategoryPromo,@Diskon,@Status,@BerlakuDari,@BerlakuSampai,@ImgLink
		)

		select 'Penambahan Promo' as title, 'Promo : '+@NamaPromo+' berhasil ditambahkan' as message,
		'success' as icon
	end
	else
	begin
		select 'Penambahan Promo' as title, 'Promo : '+@NamaPromo+' sudah ada' as message,
		'error' as icon
	end
END











