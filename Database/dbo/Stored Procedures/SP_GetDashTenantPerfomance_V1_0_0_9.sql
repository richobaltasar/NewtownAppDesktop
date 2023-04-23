
CREATE PROCEDURE [dbo].[SP_GetDashTenantPerfomance_V1_0_0_9]
	@ComputerName nvarchar(max),
	@Username nvarchar(max)
AS
BEGIN
	SET NOCOUNT ON;
	
	select distinct NamaTenant NamaTenant,sum(Total) Qty from [LogItemsF&BTrx]
	where replace(CONVERT(VARCHAR(10), CONVERT(date, replace(left(Datetime,10),'/','-'), 105), 23),'-','') = FORMAT(GETDATE() , 'yyyyMMdd') 
	and ComputerName = @ComputerName and Status = 1 and Chasierby = @Username
	group by NamaTenant
END
