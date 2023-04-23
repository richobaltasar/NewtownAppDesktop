CREATE PROCEDURE [dbo].[SP_GetRekapanTotalDepositAkunHariSebelumnya]
	@setTanggal nvarchar(max)
AS
BEGIN
	SET NOCOUNT ON;
	select
		isnull(Deposit,0) DepositSebelumnya,Datetime
	from DataDeposit 
	where Datetime = FORMAT(dateadd(day, -1,CONVERT(varchar(10), CONVERT(date, @setTanggal, 103), 120)),'dd/MM/yyyy')

END








