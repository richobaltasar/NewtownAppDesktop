-- Batch submitted through debugger: dbewats.sql|747|0|C:\Users\Administrator\Desktop\dbewats.sql

CREATE PROCEDURE [dbo].[SP_GetDashTotalToup]
	@ComputerName nvarchar(max)
AS
BEGIN
	SET NOCOUNT ON;
	select sum(NominalTopup) as TotalTopup from LogTopupDetail where 
	left(Datetime,10) = FORMAT(GETDATE() , 'dd/MM/yyyy') 
	and ComputerName = @ComputerName
	and Status = 1
END











