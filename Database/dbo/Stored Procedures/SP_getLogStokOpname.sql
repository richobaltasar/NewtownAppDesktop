
CREATE PROCEDURE [dbo].[SP_getLogStokOpname]
AS
BEGIN
	SET NOCOUNT ON;
	select*from LogStokOpname where StatusUpload is null
END


