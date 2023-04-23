-- Batch submitted through debugger: dbewats.sql|2809|0|C:\Users\Administrator\Desktop\dbewats.sql

CREATE PROCEDURE [dbo].[WebSP_FSaveEditFormTicket]
	@idTicket bigint,
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
	if exists (select*from DataTicket where IdTicket = @idTicket)
	begin
		update DataTicket
		set 
		namaticket = @NamaTicket,
		Monday = @Monday,
		Tuesday = @Tuesday,
		Wednesday = @Wednesday,
		Thursday = @Thursday,
		Friday = @Friday,
		Saturday = @Saturday,
		Sunday = @Sunday,
		status = @Status
		where IdTicket = @idTicket

		select 'Perubahan Ticket' as title, 'Ticket : '+@NamaTicket+' berhasil diupdate' as message,
		'success' as icon
	end
	else
	begin
		select 'perubahan Ticket' as title, 'Ticket : '+@NamaTicket+' tidak ada' as message,
		'error' as icon
	end
END











