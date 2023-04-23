-- Batch submitted through debugger: dbewats.sql|1158|0|C:\Users\Administrator\Desktop\dbewats.sql

CREATE PROCEDURE [dbo].[SP_GetGridTicket]
	@IdTicket bigint
AS
BEGIN
	SET NOCOUNT ON;
	select*from LogTicketDetail where IdTicket = @IdTicket
END











