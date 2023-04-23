CREATE PROCEDURE [dbo].[SP_GetRekapanTotalDepositAkunPeriodik]
	@SetAwal nvarchar(max),
	@SetAkhir nvarchar(max)
AS
BEGIN
	SET NOCOUNT ON;
	select
	Deposit Jumlah,
	replace(CONVERT(VARCHAR(10), CONVERT(date, replace(Datetime,'/','-'), 105), 23),'-','') 
	from DataDeposit 
	where 
	status = 1 and
	--Datetime = @SetTanggal and status = 1
	replace(CONVERT(VARCHAR(10), CONVERT(date, replace(Datetime,'/','-'), 105), 23),'-','') 
	between replace(CONVERT(VARCHAR(10), CONVERT(date, replace(@SetAwal,'/','-'), 105), 23),'-','') 
	and replace(CONVERT(VARCHAR(10), CONVERT(date, replace(@SetAkhir,'/','-'), 105), 23),'-','') 
END









