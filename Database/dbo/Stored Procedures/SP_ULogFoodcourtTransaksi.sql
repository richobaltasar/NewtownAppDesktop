
create PROCEDURE [dbo].[SP_ULogFoodcourtTransaksi]
	@Id bigint
AS
BEGIN
	SET NOCOUNT ON;
	update LogFoodcourtTransaksi
	set StatusUpload = 1
	where
	IdTrx = @Id
END


