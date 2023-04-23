-- Batch submitted through debugger: dbewats.sql|3212|0|C:\Users\Administrator\Desktop\dbewats.sql

CREATE PROCEDURE [dbo].[WebSP_GetTicketMan]
	@SearchKategori nvarchar(max)
AS
BEGIN
	SET NOCOUNT ON;
	select *from DataTicket
END











