CREATE PROCEDURE [dbo].[SP_GetDashTicketCount]
	@ComputerName nvarchar(max),
	@NamaUser nvarchar(max)
AS
BEGIN
	SET NOCOUNT ON;
	select NamaTicket,sum(Qty) as Qty
	from [dbo].[LogTicketDetail]
	where ComputerName= @ComputerName
	and 
	--left(Datetime,10) = FORMAT(GETDATE(),'dd/MM/yyyy')
	replace(CONVERT(VARCHAR(10), CONVERT(date, replace(left(Datetime,10),'/','-'), 105), 23),'-','') = FORMAT(GETDATE() , 'yyyyMMdd')
	and ChasierBy = @NamaUser
	and status = 1
	group by NamaTicket

	--select*from LogTicketDetail
END













