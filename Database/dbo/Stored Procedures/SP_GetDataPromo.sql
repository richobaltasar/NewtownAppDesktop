-- Batch submitted through debugger: dbewats.sql|842|0|C:\Users\Administrator\Desktop\dbewats.sql
CREATE PROCEDURE [dbo].[SP_GetDataPromo]
	@search nvarchar(max)
AS
BEGIN
	SET NOCOUNT ON;
	select*from DataPromo 
	where NamaPromo like '%'+@search+'%'
END











