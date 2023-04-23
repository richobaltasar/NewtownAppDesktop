CREATE PROCEDURE [dbo].[SP_GetLogClosing]
	@setAwal nvarchar(max),
	@setAkhir nvarchar(max)
AS
BEGIN
	SET NOCOUNT ON;
	select*from LogClosing
	where 
	--Status = 1
	--and 
	--left(Datetime,10) between @setAwal and @setAkhir
	replace(CONVERT(VARCHAR(10), CONVERT(date, replace(left(Datetime,10),'/','-'), 105), 23),'-','')
	between 
	replace(CONVERT(VARCHAR(10), CONVERT(date, replace(@setAwal,'/','-'), 105), 23),'-','') 
	and replace(CONVERT(VARCHAR(10), CONVERT(date, replace(@setAkhir,'/','-'), 105), 23),'-','')


END









