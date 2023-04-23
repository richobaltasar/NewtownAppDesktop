CREATE PROCEDURE [dbo].[SP_GetDataUser]
	@username nvarchar(max)
AS
BEGIN
	SET NOCOUNT ON;
	declare @web nvarchar(max)
	set @web = (select val2 from DataParam where NamaParam='WebPublic' and val1='LinkMenu')
	select*,@web as WebPublic from UserData where username = @username
END










