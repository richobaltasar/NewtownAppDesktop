
CREATE PROCEDURE [dbo].[SP_getLogTicketDetail]
AS
BEGIN
	SET NOCOUNT ON;
	select*from LogTicketDetail where StatusUpload is null
END


