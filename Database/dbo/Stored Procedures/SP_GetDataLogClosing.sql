CREATE PROCEDURE [dbo].[SP_GetDataLogClosing]
	@IdLog Bigint
AS
BEGIN
	SET NOCOUNT ON;
	select
	IdLog,Datetime,NamaComputer,
	NamaUser,isnull(TotalAllTicket,0) TotalAllTicket,isnull(TotalTopup,0) TotalTopup,isnull(TotalRegis,0) TotalRegis,
	isnull(TotalRefund,0) TotalRefund,isnull(TotalFoodcourt,0) TotalFoodcourt,isnull(TotalDanaModal,0)TotalDanaModal,
	isnull(TotalCashOut,0) TotalCashOut,isnull(TotalRefund,0) TotalRefund,isnull(TotalCashIn,0) TotalCashIn,isnull(TotalCashBox,0) TotalCashBox,
	isnull(TotalCashBox,0) TotalCashBox,isnull(TotalDanaModal,0) TotalDanaModal, isnull(TotalNominalDebitEmoney,0) TotalNominalDebitEmoney,
	isnull(TotalNominalDebit,0) TotalNominalDebit, isnull(TotalCashirInputMoneyCashbox,0) TotalCashirInputMoneyCashbox,
	isnull(MinusIndikasiMoneyCashBox,0) MinusIndikasiMoneyCashBox,MatchingSucces,StatusAcceptanceBySPV,
	Status,isnull(UangDiterimaFinnance,0) UangDiterimaFinnance,KeteranganAcceptance
	from LogClosing
	where 
	IdLog = @IdLog
	--and status = 1
END







