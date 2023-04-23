-- Batch submitted through debugger: dbewats.sql|882|0|C:\Users\Administrator\Desktop\dbewats.sql

CREATE PROCEDURE [dbo].[SP_GetDataSuplier]
	@search nvarchar(max)
AS
BEGIN
	SET NOCOUNT ON;
	select*from DataSuplier
	where NamaSuplier like '%'+@search+'%'
END











