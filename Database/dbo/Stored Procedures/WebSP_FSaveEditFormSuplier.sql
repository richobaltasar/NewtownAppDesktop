-- Batch submitted through debugger: dbewats.sql|2753|0|C:\Users\Administrator\Desktop\dbewats.sql

CREATE PROCEDURE [dbo].[WebSP_FSaveEditFormSuplier]
	@idSuplier bigint,
	@Alamat nvarchar(max),
	@Email nvarchar(max),
	@Keterangan nvarchar(max),
	@NamaSuplier nvarchar(max),
	@NameSales nvarchar(max),
	@NoHp nvarchar(max),
	@NoTelpon nvarchar(max),
	@PIC nvarchar(max),
	@status nvarchar(max)
AS
BEGIN
	SET NOCOUNT ON;
	declare @title nvarchar(max)
	declare @message nvarchar(max)
	declare @icon nvarchar(max)

	if exists(select*from [dbo].[DataSuplier] where idSuplier = @idSuplier)
	begin
		update DataSuplier
		set NamaSuplier = @NamaSuplier,
		Alamat = @Alamat,
		Email = @Email,
		NoHp = @NoHp,
		NoTelpon = @NoTelpon,
		NameSales = @NameSales,
		PIC = @PIC,
		status = @status,
		Keterangan = @Keterangan
		where idSuplier = @idSuplier
		
		set @title='Update data Success'
		set @message='DataParam Suplier '+ @NamaSuplier +' berhasil diupdate'
		set @icon='success'

	end
	else
	begin
		set @title='Update data Failed'
		set @message='Data tidak ditemukan'
		set @icon='error'
	end

	select @title title, @message message,@icon icon
END











