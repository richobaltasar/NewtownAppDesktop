-- Batch submitted through debugger: dbewats.sql|1207|0|C:\Users\Administrator\Desktop\dbewats.sql

CREATE PROCEDURE [dbo].[SP_GetJaminan]
AS
BEGIN
	SET NOCOUNT ON;
	select val2 Harga from DataParam where NamaParam ='Jaminan' and val1 ='Jaminan Gelang'
END











