
create PROCEDURE [dbo].[SP_GetLogHistoryAccountTopup]
	@NoAkun nvarchar(max)
AS
BEGIN
	SET NOCOUNT ON;

	select
	Datetime,AccountNumber,JenisPayment,isnull(NominalTopup,0) NominalTopup,isnull(TotalBayar,0)TotalBayar,
	isnull(PayCash,0) PayCash, isnull(TotalDebit,0) TotalDebit
	from LogTopupDetail 
	where AccountNumber like ''+@NoAkun+'%'
	order by Datetime asc

END







