CREATE PROCEDURE [dbo].[SP_GetDanaModalLog]
	@ComputerName nvarchar(max),
	@NamaUser nvarchar(max)
AS
BEGIN
	SET NOCOUNT ON;
	select*from [dbo].[LogCashierTambahModal]
	where 
	replace(CONVERT(VARCHAR(10), CONVERT(date, replace(left(Datetime,10),'/','-'), 105), 23),'-','') = FORMAT(GETDATE() , 'yyyyMMdd')
	and [NamaComputer] =  @ComputerName
	and NamaUser = @NamaUser
	and status = 1
END













