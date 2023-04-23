-- Batch submitted through debugger: dbewats.sql|1038|0|C:\Users\Administrator\Desktop\dbewats.sql

CREATE PROCEDURE [dbo].[SP_GetDataUserMan]
	@Search nvarchar(max)
AS
BEGIN
	SET NOCOUNT ON;
	select*from UserData
END











