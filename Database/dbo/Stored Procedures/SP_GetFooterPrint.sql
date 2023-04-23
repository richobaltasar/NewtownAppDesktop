-- Batch submitted through debugger: dbewats.sql|1121|0|C:\Users\Administrator\Desktop\dbewats.sql

CREATE PROCEDURE [dbo].[SP_GetFooterPrint]
AS
BEGIN
	SET NOCOUNT ON;
	select val3 from DataParam where NamaParam = 'PrintSetting' and val1 ='Footer'
	order by val2 asc
END











