CREATE PROCEDURE [dbo].[SP_GetChasierCashbox]	
AS
BEGIN
	SET NOCOUNT ON;
	select
	IdModal,Datetime,NamaComputer,isnull(DanaModalSetelah,0) DanaModalSetelah,
	isnull(TotalUangDiBox,0)TotalUangDiBox,
	isnull(TotalUangKeluar,0) TotalUangKeluar,
	isnull(TotalUangMasuk,0) TotalUangMasuk,
	OpenBy,UpdateBy,CloseBy,Status
	from DataChasierBox where 
	replace(CONVERT(VARCHAR(10), CONVERT(date, replace(Datetime,'/','-'), 105), 23),'-','') = FORMAT(GETDATE() , 'yyyyMMdd')
	--and Status = 1
END










