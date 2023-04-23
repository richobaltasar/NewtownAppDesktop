-- Batch submitted through debugger: dbewats.sql|4110|0|C:\Users\Administrator\Desktop\dbewats.sql
CREATE PROCEDURE [dbo].[WebSPDash_GetDashTotalTicketSalesPerJenisTicket]
	@setAwal nvarchar(50),
	@setAkhir nvarchar(50)
AS
BEGIN
	SET NOCOUNT ON;
	
	select
		sum(TotalAfterDiskon) Total
	from LogTicketDetail 
	where replace(CONVERT(VARCHAR(10), CONVERT(date, replace(left(Datetime,10),'/','-'), 105), 23),'-','')
	between 
	replace(CONVERT(VARCHAR(10), CONVERT(date, replace(@setAwal,'/','-'), 105), 23),'-','') 
	and replace(CONVERT(VARCHAR(10), CONVERT(date, replace(@setAkhir,'/','-'), 105), 23),'-','')

END












