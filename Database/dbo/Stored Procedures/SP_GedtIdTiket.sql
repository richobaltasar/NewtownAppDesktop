-- Batch submitted through debugger: dbewats.sql|469|0|C:\Users\Administrator\Desktop\dbewats.sql
--exec SP_GedtIdTiket
CREATE PROCEDURE [dbo].[SP_GedtIdTiket]
AS
BEGIN
	SET NOCOUNT ON;
	declare @Itung bigint

	set @Itung = 0;
	set @Itung = (select top 1 isnull(idTicket,0) as IdTicket from [dbo].[LogTicketDetail]
					order by idTicket desc)
	select isnull(@itung,0)+1 IdTicket

END











