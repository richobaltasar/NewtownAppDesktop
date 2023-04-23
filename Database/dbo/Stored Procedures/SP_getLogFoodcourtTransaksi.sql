
CREATE PROCEDURE [dbo].[SP_getLogFoodcourtTransaksi]
AS
BEGIN
	SET NOCOUNT ON;
	select*from LogFoodcourtTransaksi where StatusUpload is null
END


