
create PROCEDURE [dbo].[SP_ULogSetoranDepositExpired]
	@Id bigint
AS
BEGIN
	SET NOCOUNT ON;
	update [dbo].[LogSetoranDepositExpired]
	set StatusUpload = 1
	where
	[LogId] = @Id
END


