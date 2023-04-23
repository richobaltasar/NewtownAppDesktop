CREATE PROCEDURE [dbo].[SP_GetRekapanTotalDepositUsePeriodik]
	@SetAwal nvarchar(max),
	@SetAkhir nvarchar(max)
AS
BEGIN
	SET NOCOUNT ON;
	select sum(Nominal) Jumlah from [dbo].[LogDeposit]
	where TransactionType = 'DEBIT'
	and 
	--LEFT(Datetime,10) = @SetTanggal
	replace(CONVERT(VARCHAR(10), CONVERT(date, replace(left(Datetime,10),'/','-'), 105), 23),'-','') 
	between replace(CONVERT(VARCHAR(10), CONVERT(date, replace(@SetAwal,'/','-'), 105), 23),'-','') 
	and replace(CONVERT(VARCHAR(10), CONVERT(date, replace(@SetAkhir,'/','-'), 105), 23),'-','') 
END









