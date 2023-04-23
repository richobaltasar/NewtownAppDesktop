
CREATE PROCEDURE [dbo].[SP_ULogCashierTambahModal]
	@Id bigint
AS
BEGIN
	SET NOCOUNT ON;
	update LogCashierTambahModal
	set StatusUpload = 1
	where
	idLog = @Id
END


