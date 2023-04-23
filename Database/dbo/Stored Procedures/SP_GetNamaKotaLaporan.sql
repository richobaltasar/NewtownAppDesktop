
CREATE PROCEDURE [dbo].[SP_GetNamaKotaLaporan]
AS
BEGIN
	SET NOCOUNT ON;
	select val1 NamaKota from DataParam where NamaParam = 'NamaKotaLaporan'
END

