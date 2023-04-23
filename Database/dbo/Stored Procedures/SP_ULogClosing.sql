
CREATE PROCEDURE [dbo].[SP_ULogClosing]
	@Id bigint
AS
BEGIN
	SET NOCOUNT ON;
	update LogClosing
	set StatusUpload = 1
	where
	idLog = @Id
END


