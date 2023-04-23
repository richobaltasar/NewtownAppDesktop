
---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
-- Batch submitted through debugger: dbewats.sql|3229|0|C:\Users\Administrator\Desktop\dbewats.sql
--exec WebSP_LoginProc 'cho','q1w2e3r4'
CREATE PROCEDURE [dbo].[WebSP_LoginProc]
	@Username nvarchar(max),
	@Password nvarchar(max)
AS
BEGIN
	SET NOCOUNT ON;
	declare @title nvarchar(max)
	declare @message nvarchar(max)
	declare @icon nvarchar(max)
	declare @Akses nvarchar(max)

	if exists (select*from UserData where username = @Username and password = @Password and hakakses !='Kasir')
	begin
		set @title='Login Success'
		set @message='Akses anda diterima'
		set @icon='success'

		set @Akses = 'Admin'

	end
	else
	begin
		set @title='Login Failed'
		set @message='Username atau password anda tidak teregistrasi'
		set @icon='error'
	end
	select @title title, @message message,@icon icon,@Username Username,@Akses Akses
END












