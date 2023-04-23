
--exec WEBSP_GetLogClosing_V1_0_0_9 '10/01/2020','10/01/2020'

CREATE PROCEDURE [dbo].[WEBSP_GetLogClosing_V1_0_0_9]
	@setAwal nvarchar(max),
	@setAkhir nvarchar(max)
AS
BEGIN
	SET NOCOUNT ON;
	select *from LogClosingV2 
	where 
	replace(CONVERT(VARCHAR(10), CONVERT(date, replace(left(Datetime,10),'/','-'), 105), 23),'-','') between 
	replace(CONVERT(VARCHAR(10), CONVERT(date, replace(@setAwal,'/','-'), 105), 23),'-','') 
	and replace(CONVERT(VARCHAR(10), CONVERT(date, replace(@setAkhir,'/','-'), 105), 23),'-','')
END
