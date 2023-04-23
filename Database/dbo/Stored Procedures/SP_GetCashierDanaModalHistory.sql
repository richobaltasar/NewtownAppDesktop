CREATE PROCEDURE [dbo].[SP_GetCashierDanaModalHistory]	
	@ComputerName nvarchar(max)
AS
BEGIN
	SET NOCOUNT ON;

	select*from LogCashierTambahModal
	where 
	replace(CONVERT(VARCHAR(10), CONVERT(date, replace(left(Datetime,10),'/','-'), 105), 23),'-','') = FORMAT(GETDATE() , 'yyyyMMdd')
	and	NamaComputer = @ComputerName

END








