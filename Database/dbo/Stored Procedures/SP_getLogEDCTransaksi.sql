
CREATE PROCEDURE [dbo].[SP_getLogEDCTransaksi]
AS
BEGIN
	SET NOCOUNT ON;
	select*from LogEDCTransaksi where StatusUpload is null
END


