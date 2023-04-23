-- Batch submitted through debugger: dbewats.sql|4087|0|C:\Users\Administrator\Desktop\dbewats.sql
--WebSPDash_GetDashRefundSalesDonut '10/11/2018','10/11/2018'
CREATE PROCEDURE [dbo].[WebSPDash_GetDashTotalRefundSales]
	@SetAwal  nvarchar(50),
	@SetAkhir  nvarchar(50)
AS
BEGIN
	SET NOCOUNT ON;
	select 
			sum(TotalNominalRefund) Total
		from LogRefundDetail
			where replace(CONVERT(VARCHAR(10), CONVERT(date, replace(left(Datetime,10),'/','-'), 105), 23),'-','')
			between 
			replace(CONVERT(VARCHAR(10), CONVERT(date, replace(@SetAwal,'/','-'), 105), 23),'-','') 
			and replace(CONVERT(VARCHAR(10), CONVERT(date, replace(@SetAkhir,'/','-'), 105), 23),'-','')
		group by left(Datetime,10)
END












