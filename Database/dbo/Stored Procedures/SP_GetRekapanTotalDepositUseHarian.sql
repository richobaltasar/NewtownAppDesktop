CREATE PROCEDURE [dbo].[SP_GetRekapanTotalDepositUseHarian]
	@SetTanggal nvarchar(max)
AS
BEGIN
	SET NOCOUNT ON;
	select sum(Nominal) Jumlah from [dbo].[LogDeposit]
	where TransactionType = 'DEBIT'
	and 
	--LEFT(Datetime,10) = @SetTanggal
	replace(CONVERT(VARCHAR(10), CONVERT(date, replace(left(Datetime,10),'/','-'), 105), 23),'-','') 
	= replace(CONVERT(VARCHAR(10), CONVERT(date, replace(@SetTanggal,'/','-'), 105), 23),'-','') 
END









