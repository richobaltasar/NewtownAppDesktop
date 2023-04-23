-- Batch submitted through debugger: dbewats.sql|1243|0|C:\Users\Administrator\Desktop\dbewats.sql

CREATE PROCEDURE [dbo].[SP_GetNamaUser]
	@IdUser int
AS
BEGIN
	SET NOCOUNT ON;
	select NamaLengkap from UserData where id=@IdUser and Status = 'Aktif'
END











