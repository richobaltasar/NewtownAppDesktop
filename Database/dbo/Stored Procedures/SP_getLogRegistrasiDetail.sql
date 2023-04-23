
CREATE PROCEDURE [dbo].[SP_getLogRegistrasiDetail]
AS
BEGIN
	SET NOCOUNT ON;
	select*from LogRegistrasiDetail where StatusUpload is null
END


