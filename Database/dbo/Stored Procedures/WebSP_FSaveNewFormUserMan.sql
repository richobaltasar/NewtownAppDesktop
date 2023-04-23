-- Batch submitted through debugger: dbewats.sql|3120|0|C:\Users\Administrator\Desktop\dbewats.sql

CREATE PROCEDURE [dbo].[WebSP_FSaveNewFormUserMan]
	@NamaLengkap nvarchar(max),
	@username nvarchar(max),
	@Email nvarchar(max),
	@NoHp nvarchar(max),
	@Alamat nvarchar(max),
	@Gender nvarchar(max),
	@password nvarchar(max),
	@hakakses nvarchar(max),
	@ImgLink nvarchar(max),
	@Status nvarchar(max)
AS
BEGIN
	
	SET NOCOUNT ON;
	if not exists(select*from UserData where username = @username)
	begin
		insert into UserData
		(
			username,password,hakakses,NamaLengkap,Email,Gender,NoHp,Status,ImgLink,Alamat
		)
		values
		(
			@username,@password,@hakakses,@NamaLengkap,@Email,@Gender,@NoHp,@Status,@ImgLink,@Alamat
		)
		select 'Penambahan User' title,'success' icon,'Penambahan user berhasil dilakukan' message
	end
	else
	begin
		select 'Penambahan User' title,'error' icon,'Penambahan user gagal, karena username / nama lengkap already exists' message
	end
END











