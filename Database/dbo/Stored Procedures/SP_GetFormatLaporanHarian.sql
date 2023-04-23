--sp_helptext SP_GetFormatLaporanHarian
CREATE PROCEDURE [dbo].[SP_GetFormatLaporanHarian]
AS
BEGIN
	SET NOCOUNT ON;
	select val1,val2 from DataParam where NamaParam = 'FormatLaporanHarian'
END

