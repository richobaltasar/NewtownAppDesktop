
CREATE PROCEDURE [dbo].[SP_GetListTenant]
AS
BEGIN
	SET NOCOUNT ON;
	select NamaTenant from DataTenant where StatusKepemilikan = 'Management' and StatusJual != 'Karyawan'
END







