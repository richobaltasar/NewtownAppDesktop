-- Batch submitted through debugger: dbewats.sql|513|0|C:\Users\Administrator\Desktop\dbewats.sql

CREATE PROCEDURE [dbo].[SP_GetAsuransi]
AS
BEGIN
	SET NOCOUNT ON;
	select  val2 Harga from DataParam where NamaParam = 'Asuransi' and val1 = 'Asuransi Kecelakaan'
END











