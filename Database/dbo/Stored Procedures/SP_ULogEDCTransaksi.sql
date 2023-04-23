
CREATE PROCEDURE [dbo].[SP_ULogEDCTransaksi]
	@Id bigint
AS
BEGIN
	SET NOCOUNT ON;
	update LogEDCTransaksi
	set StatusUpload = 1
	where
	IdLog = @Id
END


