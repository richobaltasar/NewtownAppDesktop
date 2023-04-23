CREATE PROCEDURE [dbo].[SP_GetLogStockOpname]
	@awal nvarchar(max),
	@akhir nvarchar(max)
AS
BEGIN
	SET NOCOUNT ON;
	select*from LogStokOpname
	where 
	Status = 1
	--and left(Datetime,10) between @awal and @akhir
	and replace(CONVERT(VARCHAR(10), CONVERT(date, replace(left(Datetime,10),'/','-'), 105), 23),'-','')
	between 
	replace(CONVERT(VARCHAR(10), CONVERT(date, replace(@awal,'/','-'), 105), 23),'-','') 
	and replace(CONVERT(VARCHAR(10), CONVERT(date, replace(@akhir,'/','-'), 105), 23),'-','')
END






