-- Batch submitted through debugger: dbewats.sql|1175|0|C:\Users\Administrator\Desktop\dbewats.sql

CREATE PROCEDURE [dbo].[SP_GetHakakses]
	
AS
BEGIN
	SET NOCOUNT ON;
	select val2 Value ,val2 Text from [dbo].[DataParam] where NamaParam = 'HakAkses'
END











