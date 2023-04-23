
CREATE PROCEDURE [dbo].[SP_GetTDD]	
AS
BEGIN
	SET NOCOUNT ON;
	select val2 TitleTTD,val3 NamaTTD,val4 NIPTTD
	from DataParam where NamaParam = 'KolomTandaTanganLaporan'
	order by val1 asc
END

