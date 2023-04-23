CREATE PROCEDURE [dbo].[WebSP_GetRegistrasiReportDetail]
	@DatetimeFrom nvarchar(50),
	@DatetimeUntil nvarchar(50),
	@SetFilter nvarchar(50)
AS
BEGIN
	SET NOCOUNT ON;
	if(@SetFilter = 'All')
	begin
		select a.*,b.NoATM,b.NoReffEddPrint from LogRegistrasiDetail a
		left join LogEDCTransaksi b on b.IdLog = a.IdLogEDCTransaksi
		where 
		--left(a.Datetime,10) between @DatetimeFrom and @DatetimeUntil
		replace(CONVERT(VARCHAR(10), CONVERT(date, replace(left(a.Datetime,10),'/','-'), 105), 23),'-','')
		between 
		replace(CONVERT(VARCHAR(10), CONVERT(date, replace(@DatetimeFrom,'/','-'), 105), 23),'-','') 
		and replace(CONVERT(VARCHAR(10), CONVERT(date, replace(@DatetimeUntil,'/','-'), 105), 23),'-','')


	end
	else
	begin
		select a.*,b.NoATM,b.NoReffEddPrint from LogRegistrasiDetail a
		left join LogEDCTransaksi b on b.IdLog = a.IdLogEDCTransaksi
		where 
		--left(a.Datetime,10) between @DatetimeFrom and @DatetimeUntil
		replace(CONVERT(VARCHAR(10), CONVERT(date, replace(left(a.Datetime,10),'/','-'), 105), 23),'-','')
		between 
		replace(CONVERT(VARCHAR(10), CONVERT(date, replace(@DatetimeFrom,'/','-'), 105), 23),'-','') 
		and replace(CONVERT(VARCHAR(10), CONVERT(date, replace(@DatetimeUntil,'/','-'), 105), 23),'-','')
		and JenisTransaksi = @SetFilter
	end
END









