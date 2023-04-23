CREATE PROCEDURE [dbo].[SP_GetRekapanTotalRefundHarian]
	@SetTanggal nvarchar(max)
AS
BEGIN
	SET NOCOUNT ON;
	select sum(TotalNominalRefund) Jumlah from LogRefundDetail
	where 
	--left(Datetime,10) = @setTanggal
	replace(CONVERT(VARCHAR(10), CONVERT(date, replace(left(Datetime,10),'/','-'), 105), 23),'-','') 
	= replace(CONVERT(VARCHAR(10), CONVERT(date, replace(@setTanggal,'/','-'), 105), 23),'-','') 
END









