-- Batch submitted through debugger: dbewats.sql|1192|0|C:\Users\Administrator\Desktop\dbewats.sql

CREATE PROCEDURE [dbo].[SP_GetHeaderPrint]
	@Line nvarchar(max)
AS
BEGIN
	SET NOCOUNT ON;
	select*from DataParam where NamaParam = 'PrintSetting' and val1 = 'Header' and val2 = @Line
END









