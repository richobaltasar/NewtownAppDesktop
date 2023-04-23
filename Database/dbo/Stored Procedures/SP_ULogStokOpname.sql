
create PROCEDURE [dbo].[SP_ULogStokOpname]
	@Id bigint
AS
BEGIN
	SET NOCOUNT ON;
	update [dbo].[LogStokOpname]
	set StatusUpload = 1
	where
	[idLog] = @Id
END


