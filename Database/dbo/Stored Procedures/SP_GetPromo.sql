-- Batch submitted through debugger: dbewats.sql|1260|0|C:\Users\Administrator\Desktop\dbewats.sql

CREATE PROCEDURE [dbo].[SP_GetPromo]
	@Search nvarchar(max)
AS
BEGIN
	SET NOCOUNT ON;
	select*from DataPromo where NamaPromo like '%'+@Search+'%'
END











