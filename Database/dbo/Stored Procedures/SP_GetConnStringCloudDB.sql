CREATE PROCEDURE [dbo].[SP_GetConnStringCloudDB]
AS
BEGIN
	SET NOCOUNT ON;
	select val1 ServerName,val2 Username ,val3 Password,val4 DBName from DataParam where NamaParam = 'ConnStringCloudDB'
END


