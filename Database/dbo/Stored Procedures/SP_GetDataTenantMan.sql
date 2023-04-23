-- Batch submitted through debugger: dbewats.sql|900|0|C:\Users\Administrator\Desktop\dbewats.sql

CREATE PROCEDURE [dbo].[SP_GetDataTenantMan]
	@Search nvarchar(max)
AS
BEGIN
	SET NOCOUNT ON;
	select*from DataTenant where NamaTenant like '%'+@Search+'%'
END












