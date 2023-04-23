-- Batch submitted through debugger: dbewats.sql|2630|0|C:\Users\Administrator\Desktop\dbewats.sql

CREATE PROCEDURE [dbo].[WebSP_FDeleteUserMan]
	@id bigint
AS
BEGIN
	SET NOCOUNT ON;

	if exists(select*from UserData where id=@id)
	begin
		delete from UserData where id=@id
		select 'Delete User' title,'success' icon,'Penghapusan user berhasil dilakukan' message
	end
	else
	begin
		select 'Delete User' title,'error' icon,'Penghapusan user tidak berhasil dilakukan' message
	end
END











