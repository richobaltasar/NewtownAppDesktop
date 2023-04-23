-- Batch submitted through debugger: dbewats.sql|2590|0|C:\Users\Administrator\Desktop\dbewats.sql
CREATE PROCEDURE [dbo].[WebSP_FDeleteTicketMan]
	@id bigint
AS
BEGIN
	SET NOCOUNT ON;
	declare @title nvarchar(max)
	declare @message nvarchar(max)
	declare @icon nvarchar(max)

	if exists(select*from DataTicket where IdTicket = @id)
	begin
		declare @nameTicket nvarchar(max)
		set @nameTicket = (select namaticket from DataTicket where IdTicket=@id)
		
		delete from DataTicket
		where IdTicket=@id

		set @title='Delete data Success'
		set @message='Data Ticket '+ @nameTicket +' berhasil dihapus'
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












