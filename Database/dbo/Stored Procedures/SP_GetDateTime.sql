CREATE PROCEDURE [dbo].[SP_GetDateTime]
AS
BEGIN
	SET NOCOUNT ON;
	SELECT FORMAT(GETDATE() , 'dd/MM/yyyy HH:mm:ss') as tanggal
END






