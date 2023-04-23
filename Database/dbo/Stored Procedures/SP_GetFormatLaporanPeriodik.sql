CREATE PROCEDURE [dbo].[SP_GetFormatLaporanPeriodik]
AS
BEGIN
	SET NOCOUNT ON;
	select val1,val2 from DataParam where NamaParam = 'FormatLaporanHarian'
END
