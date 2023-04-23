
CREATE PROCEDURE [dbo].[SP_LogRefundDetail]
AS
BEGIN
	SET NOCOUNT ON;
	select*from LogRefundDetail where StatusUpload is null
END


