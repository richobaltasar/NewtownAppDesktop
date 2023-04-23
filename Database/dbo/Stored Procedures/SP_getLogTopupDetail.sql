
CREATE PROCEDURE [dbo].[SP_getLogTopupDetail]
AS
BEGIN
	SET NOCOUNT ON;
	select*from LogTopupDetail where StatusUpload is null
END


