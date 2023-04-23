
CREATE PROCEDURE [dbo].[SP_getLogClosingUpload]
AS
BEGIN
	SET NOCOUNT ON;
	select*from LogClosing where StatusUpload is null
END


