
CREATE PROCEDURE [dbo].[SP_getLogDeposit]
AS
BEGIN
	SET NOCOUNT ON;
	select*from LogDeposit where StatusUpload is null
END


