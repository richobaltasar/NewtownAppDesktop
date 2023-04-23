-- Batch submitted through debugger: dbewats.sql|3032|0|C:\Users\Administrator\Desktop\dbewats.sql

CREATE PROCEDURE [dbo].[WebSP_FSaveNewFormSuplier]
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

	if not exists(select*from DataSuplier where NamaSuplier = @NamaSuplier)
	begin
		insert into DataSuplier
		(NamaSuplier,Alamat,Email,NoHp,NoTelpon,NameSales,PIC,status,Keterangan)
		values(@NamaSuplier,@Alamat,@Email,@NoHp,@NoTelpon,@NameSales,@PIC,@status,@Keterangan)

		set @title='Add data Success'
		set @message='Data Suplier '+ @NamaSuplier +' berhasil diregistrasi'
		set @icon='success'
	end
	else
	begin
		set @title='Nama Suplier sudah teregistrasi'
		set @message='Data already exists'
		set @icon='error'
	end
	select @title title, @message message,@icon icon
END











