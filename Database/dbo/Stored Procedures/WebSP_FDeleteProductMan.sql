-- Batch submitted through debugger: dbewats.sql|2489|0|C:\Users\Administrator\Desktop\dbewats.sql

CREATE PROCEDURE [dbo].[WebSP_FDeleteProductMan] 
	@id bigint
AS
BEGIN
	SET NOCOUNT ON;
	if exists (select*from DataBarang where idMenu = @id)
	begin
		delete from DataBarang
		where idMenu = @id
		select 'Delete Success' as title, 'success' as icon, 'Penghapusa data berhasil' as message
	end
	else
	begin
		select 'Delete Success' as title, 'error' as icon, 'Penghapusan data tidak berhasil' as message
	end
END











