
create PROCEDURE [dbo].[SP_ULogTopupDetail]
	@Id bigint
AS
BEGIN
	SET NOCOUNT ON;
	update [dbo].[LogTopupDetail]
	set StatusUpload = 1
	where
	[IdTopup]= @Id
END


