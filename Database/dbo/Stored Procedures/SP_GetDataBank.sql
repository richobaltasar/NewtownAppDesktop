-- Batch submitted through debugger: dbewats.sql|804|0|C:\Users\Administrator\Desktop\dbewats.sql

CREATE PROCEDURE [dbo].[SP_GetDataBank]
	@search nvarchar(max)
AS
BEGIN
	SET NOCOUNT ON;
	select*from [dbo].[DataBank]
	where [NamaBank] like '%'+@search+'%' and status = 'Aktif'
END









