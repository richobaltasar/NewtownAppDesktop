
CREATE PROCEDURE [dbo].[SP_GetRekapanTotalDepositUseHaria]
	@SetTanggal nvarchar(max)
AS
BEGIN
	SET NOCOUNT ON;
	select sum(Nominal) Jumlah from [dbo].[LogDeposit]
	where TransactionType = 'DEBIT'
	and LEFT(Datetime,10) = @SetTanggal
END








