
create PROCEDURE [dbo].[SP_ULogItemsFBTrx]
	@Id bigint
AS
BEGIN
	SET NOCOUNT ON;
	update [dbo].[LogItemsF&BTrx]
	set StatusUpload = 1
	where
	IdItemsKeranjang = @Id
END


