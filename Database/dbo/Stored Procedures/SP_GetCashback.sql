-- Batch submitted through debugger: dbewats.sql|547|0|C:\Users\Administrator\Desktop\dbewats.sql

CREATE PROCEDURE [dbo].[SP_GetCashback]
	@Search nvarchar(max)
AS
BEGIN
	SET NOCOUNT ON;
	select*from DataCashback where NamaCashback like '%'+@Search+'%'
END











