
create PROCEDURE [dbo].[SP_ULogRefundDetail]
	@Id bigint
AS
BEGIN
	SET NOCOUNT ON;
	update [dbo].[LogRefundDetail]
	set StatusUpload = 1
	where
	IdRefund = @Id
END


