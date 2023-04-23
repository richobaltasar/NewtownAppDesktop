CREATE PROCEDURE [dbo].[SP_GetTotalLogClosing]
	@setAwal nvarchar(max),
	@setAkhir nvarchar(max)
AS
BEGIN
	SET NOCOUNT ON;
	select sum(isnull(TotalDanaModal,0)) TotalDanaModal,
	sum(isnull((TotalCashBox-TotalDanaModal),0)) as TotalTransaksi, 
	sum(isnull(TotalCashBox,0)) TotalCashBox,
	sum(isnull(TotalCashirInputMoneyCashbox,0)) TotalCashirInputMoneyCashbox,
	sum(isnull(UangDiterimaFinnance,0)) as UangDiterimaFinnance,
	sum(isnull(TotalTrxEdc,0)) as TotalTrxEdc,
	sum(isnull(TotalNominalDebit,0)) as TotalNominalDebit,
	sum(isnull(TotalTrxEmoney,0)) as TotalTrxEmoney,
	sum(isnull(TotalNominalDebitEmoney,0)) as TotalNominalDebitEmoney
	from LogClosing
	where 
	Status = 1
	and left(Datetime,10) between @setAwal and @setAkhir
END









