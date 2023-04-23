
CREATE PROCEDURE [dbo].[SP_getLogCashierTambahModal]
AS
BEGIN
	SET NOCOUNT ON;
	select*from LogCashierTambahModal where StatusUpload is null
END


