-- Batch submitted through debugger: dbewats.sql|1277|0|C:\Users\Administrator\Desktop\dbewats.sql

CREATE PROCEDURE [dbo].[SP_GetTenant]
AS
BEGIN
	SET NOCOUNT ON;
	select*from [dbo].[DataTenant] where status = 1
END











