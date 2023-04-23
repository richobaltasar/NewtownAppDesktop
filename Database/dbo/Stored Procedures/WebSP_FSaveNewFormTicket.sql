-- Batch submitted through debugger: dbewats.sql|3077|0|C:\Users\Administrator\Desktop\dbewats.sql

CREATE PROCEDURE [dbo].[WebSP_FSaveNewFormTicket]
	@ImgLink nvarchar(max),
	@Friday float,
	@Monday float,
	@NamaTicket nvarchar(max),
	@Saturday float,
	@Sunday float,
	@Thursday float,
	@Tuesday float,
	@Wednesday float,
	@Status nvarchar(50)
AS
BEGIN
	SET NOCOUNT ON;
	if not exists (select*from DataTicket where namaticket = @NamaTicket)
	begin
		insert into DataTicket
		(
			namaticket,Monday,Tuesday,Wednesday,Thursday,Friday,Saturday,Sunday,status
		)
		values
		(
			@NamaTicket,@Monday,@Tuesday,@Wednesday,@Thursday,@Friday,@Saturday,@Sunday,@Status
		)
		select 'Penambahan Ticket baru' as title, 'Ticket : '+@NamaTicket+' berhasil ditambahkan' as message,
		'success' as icon
	end
	else
	begin
		select 'Penambahan Ticket Baru' as title, 'Ticket : '+@NamaTicket+' sudah ada' as message,
		'error' as icon
	end
END











