-- Batch submitted through debugger: dbewats.sql|2460|0|C:\Users\Administrator\Desktop\dbewats.sql

CREATE PROCEDURE [dbo].[WebSP_FDeleteDiskonMan]
	@id bigint
AS
BEGIN
	SET NOCOUNT ON;
	if exists(select*from DataPromo where idPromo=@id)
	begin
		declare @NamaPromo nvarchar(max)
		set @NamaPromo = (select NamaPromo from DataPromo where idPromo=@id)
		delete from DataPromo where idPromo = @id
		select 'Delete Promo' as title, 'Promo : '+@NamaPromo+' berhasil dihapus' as message,
		'success' as icon
	end
	else
	begin
		select 'Delete Promo' as title, 'Promo : '+@NamaPromo+' gagal dihapus, karena data tidak ditemukan' as message,
		'error' as icon
	end
END











