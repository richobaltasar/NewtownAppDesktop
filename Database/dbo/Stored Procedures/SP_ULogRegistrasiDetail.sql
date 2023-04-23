
create PROCEDURE [dbo].[SP_ULogRegistrasiDetail]
	@Id bigint
AS
BEGIN
	SET NOCOUNT ON;
	update [dbo].[LogRegistrasiDetail]
	set StatusUpload = 1
	where
	[idTrx] = @Id
END


