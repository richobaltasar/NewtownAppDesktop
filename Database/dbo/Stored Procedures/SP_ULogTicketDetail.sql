
create PROCEDURE [dbo].[SP_ULogTicketDetail]
	@Id bigint
AS
BEGIN
	SET NOCOUNT ON;
	update [dbo].[LogTicketDetail]
	set StatusUpload = 1
	where
	[IdTicket]= @Id
END


