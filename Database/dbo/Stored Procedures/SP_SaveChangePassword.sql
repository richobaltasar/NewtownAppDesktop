
CREATE PROCEDURE [dbo].[SP_SaveChangePassword]
	@Username nvarchar(max),
	@CurrentPassword nvarchar(max),
	@NewPassword nvarchar(max)
AS
BEGIN
	SET NOCOUNT ON;
	if exists(select*from UserData where username = @Username and password = @CurrentPassword)
	begin 
		update UserData
		set password = @NewPassword
		where username = @Username and password = @CurrentPassword

		select 'Change Password' as title, 'Perubahan password berhasil dilakukan' as message,
		'success' as icon
	end
	else
	begin
		select 'Change Password' as title, 'Perubahan password gagal dilakukan, karena current password tidak valid' as message,
		'error' as icon
	end
END










