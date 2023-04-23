CREATE PROCEDURE [dbo].[SP_GetRekapanTotalDepositAkunHarian]
	@SetTanggal nvarchar(max)
AS
BEGIN
	SET NOCOUNT ON;
	select
	Deposit Jumlah
	from DataDeposit where Datetime = @SetTanggal and status = 1
END








