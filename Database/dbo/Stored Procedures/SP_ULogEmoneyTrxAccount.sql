
CREATE PROCEDURE [dbo].[SP_ULogEmoneyTrxAccount]
	@Id bigint
AS
BEGIN
	SET NOCOUNT ON;
	update LogEmoneyTrxAccount
	set StatusUpload = 1
	where
	IdLog = @Id
END


