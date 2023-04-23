-- Batch submitted through debugger: dbewats.sql|3378|0|C:\Users\Administrator\Desktop\dbewats.sql
--WebSPDash_GetDashTopupSalesDonut '10/11/2018','10/11/2018'
CREATE PROCEDURE [dbo].[WebSPDash_ChartTopupTransaction]
	@SetAwal  nvarchar(50),
	@SetAkhir  nvarchar(50)
AS
BEGIN
	SET NOCOUNT ON;
	if(@SetAwal = @SetAkhir)
	begin
		select left(right(Datetime,8),2)+':00' Datetime,
		sum(TotalBayar) Total
		from LogTopupDetail
		where 
		replace(CONVERT(VARCHAR(10), CONVERT(date, replace(left(Datetime,10),'/','-'), 105), 23),'-','')
		between 
		replace(CONVERT(VARCHAR(10), CONVERT(date, replace(@SetAwal,'/','-'), 105), 23),'-','') 
		and replace(CONVERT(VARCHAR(10), CONVERT(date, replace(@SetAkhir,'/','-'), 105), 23),'-','')
		group by left(right(Datetime,8),2)
	end
	else
	begin
		select left(Datetime,10) Datetime,
		sum(TotalBayar) Total
		from LogTopupDetail
		where 
		replace(CONVERT(VARCHAR(10), CONVERT(date, replace(left(Datetime,10),'/','-'), 105), 23),'-','')
		between 
		replace(CONVERT(VARCHAR(10), CONVERT(date, replace(@SetAwal,'/','-'), 105), 23),'-','') 
		and replace(CONVERT(VARCHAR(10), CONVERT(date, replace(@SetAkhir,'/','-'), 105), 23),'-','')
		group by left(Datetime,10)
	end
END












