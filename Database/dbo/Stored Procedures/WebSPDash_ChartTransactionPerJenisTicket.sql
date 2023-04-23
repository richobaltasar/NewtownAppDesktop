-- Batch submitted through debugger: dbewats.sql|3412|0|C:\Users\Administrator\Desktop\dbewats.sql
-- exec WebSPDash_ChartTransactionPerJenisTicket '10/11/2018','16/11/2018'
CREATE PROCEDURE [dbo].[WebSPDash_ChartTransactionPerJenisTicket]
	@setAwal nvarchar(50),
	@setAkhir nvarchar(50)
AS
BEGIN
	SET NOCOUNT ON;
	if(@setAwal = @setAkhir)
	begin
		select left(right(Datetime,8),2)+':00' Datetime,sum(TotalAfterDiskon) Total from LogTicketDetail 
		where 
		replace(CONVERT(VARCHAR(10), CONVERT(date, replace(left(Datetime,10),'/','-'), 105), 23),'-','')
		between 
		replace(CONVERT(VARCHAR(10), CONVERT(date, replace(@setAwal,'/','-'), 105), 23),'-','') 
		and replace(CONVERT(VARCHAR(10), CONVERT(date, replace(@setAkhir,'/','-'), 105), 23),'-','')
		group by left(right(Datetime,8),2)
	end
	else
	begin
		select left(Datetime,10) Datetime,sum(TotalAfterDiskon) Total from LogTicketDetail 
		where 
		replace(CONVERT(VARCHAR(10), CONVERT(date, replace(left(Datetime,10),'/','-'), 105), 23),'-','')
		between 
		replace(CONVERT(VARCHAR(10), CONVERT(date, replace(@setAwal,'/','-'), 105), 23),'-','') 
		and replace(CONVERT(VARCHAR(10), CONVERT(date, replace(@setAkhir,'/','-'), 105), 23),'-','')
		group by left(Datetime,10)
	end

END













