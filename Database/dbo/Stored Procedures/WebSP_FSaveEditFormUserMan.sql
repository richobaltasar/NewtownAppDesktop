-- Batch submitted through debugger: dbewats.sql|2858|0|C:\Users\Administrator\Desktop\dbewats.sql

CREATE PROCEDURE [dbo].[WebSP_FSaveEditFormUserMan]
	@id bigint,
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
	if exists(select*from UserData where id = @id and username=@username)
	begin
		update UserData 
		set NamaLengkap = @NamaLengkap,Email=@Email,NoHp=@NoHp,
		Alamat=@Alamat,Gender=@Gender,password=@password,hakakses=@hakakses,ImgLink=@ImgLink,Status=@Status
		where id=@id and username=@username
		select 'Perubahan Data User' title,'success' icon,'Perubahan data user berhasil dilakukan' message
	end
	else
	begin
		select 'Perubahan Data User' title,'error' icon,'Perubahan data user gagal, karena username tidak ditemukan' message
	end
END











