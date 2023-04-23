-- Batch submitted through debugger: dbewats.sql|1223|0|C:\Users\Administrator\Desktop\dbewats.sql
CREATE PROCEDURE [dbo].[SP_GetJenisTicketDash]
	@SetAwal nvarchar(50),
	@SetAkhir nvarchar(50)
AS
BEGIN
	SET NOCOUNT ON;
	select
		sum(qty) qty,NamaTicket
	from LogTicketDetail 
	where 
	--left(Datetime,10) between @SetAwal and @SetAkhir
	replace(CONVERT(VARCHAR(10), CONVERT(date, replace(left(Datetime,10),'/','-'), 105), 23),'-','')
	between 
	replace(CONVERT(VARCHAR(10), CONVERT(date, replace(@SetAwal,'/','-'), 105), 23),'-','') 
	and replace(CONVERT(VARCHAR(10), CONVERT(date, replace(@SetAkhir,'/','-'), 105), 23),'-','')
	group by NamaTicket
END











