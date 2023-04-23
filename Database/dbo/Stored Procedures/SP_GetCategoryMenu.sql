-- Batch submitted through debugger: dbewats.sql|564|0|C:\Users\Administrator\Desktop\dbewats.sql

CREATE PROCEDURE [dbo].[SP_GetCategoryMenu]
AS
BEGIN
	SET NOCOUNT ON;
	select
	val2 Text,
	val2 Value
	from DataParam
	where NamaParam = 'CategoryMenu'
END











