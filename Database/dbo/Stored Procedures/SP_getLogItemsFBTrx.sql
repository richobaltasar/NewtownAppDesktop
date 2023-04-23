
CREATE PROCEDURE [dbo].[SP_getLogItemsFBTrx]
AS
BEGIN
	SET NOCOUNT ON;
	select*from [LogItemsF&BTrx] where StatusUpload is null
END


