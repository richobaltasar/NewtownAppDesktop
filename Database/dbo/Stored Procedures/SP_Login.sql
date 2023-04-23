-- Batch submitted through debugger: dbewats.sql|1315|0|C:\Users\Administrator\Desktop\dbewats.sql

CREATE PROCEDURE [dbo].[SP_Login]
	@Username nvarchar(max),
	@Password nvarchar(max)
AS
BEGIN
	SET NOCOUNT ON;
	select*from UserData
	where username = @Username and password = @Password
END











