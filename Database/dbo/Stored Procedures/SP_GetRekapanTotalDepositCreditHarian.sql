
CREATE PROCEDURE [dbo].[SP_GetRekapanTotalDepositCreditHarian]
	@SetTanggal nvarchar(max)
AS
BEGIN
	SET NOCOUNT ON;
	select sum(Nominal) Jumlah from [dbo].[LogDeposit]
	where TransactionType = 'CREDIT'
	and LEFT(Datetime,10) = @SetTanggal
END







