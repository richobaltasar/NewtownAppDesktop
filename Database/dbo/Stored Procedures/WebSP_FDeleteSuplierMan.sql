-- Batch submitted through debugger: dbewats.sql|2515|0|C:\Users\Administrator\Desktop\dbewats.sql

CREATE PROCEDURE [dbo].[WebSP_FDeleteSuplierMan]
	@id bigint
AS
BEGIN
	SET NOCOUNT ON;
	declare @title nvarchar(max)
	declare @message nvarchar(max)
	declare @icon nvarchar(max)
	if exists (select*from DataSuplier where idSuplier = @id)
	begin
		declare @NamaSuplier nvarchar(max)
		set @NamaSuplier = (select NamaSuplier from DataSuplier where idSuplier = @id)

		delete from DataSuplier where idSuplier = @id
		set @title='Delete data Success'
		set @message='Data Suplier : '+ @NamaSuplier +' berhasil dihapus'
		set @icon='success'
	end
	else
	begin
		set @title='Delete data Failed'
		set @message='Data tidak ditemukan'
		set @icon='error'
	end
	select @title title, @message message,@icon icon
END











