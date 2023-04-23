
CREATE PROCEDURE [dbo].[SP_ULogDeposit]
	@Id bigint
AS
BEGIN
	SET NOCOUNT ON;
	update LogDeposit
	set StatusUpload = 1
	where
	LogId = @Id
END


