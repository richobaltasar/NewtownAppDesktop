
CREATE PROCEDURE [dbo].[SP_ApiUploadLogClosing]
	@IdLog    bigint,
	@Datetime    nvarchar(50),
	@NamaComputer    nvarchar(MAX),
	@NamaUser    nvarchar(MAX),
	@TotalAllTicket    float,
	@TotalTransaksi    float,
	@TotalTopup    float,
	@TotalRegis    float,
	@TotalRefund    float,
	@TotalFoodcourt    float,
	@TotalDanaModal    float,
	@TotalCashOut    float,
	@TotalCashIn    float,
	@TotalCashBox    float,
	@TotalCashirInputMoneyCashbox    float,
	@MinusIndikasiMoneyCashBox    float,
	@MatchingSucces    nvarchar(MAX),
	@StatusAcceptanceBySPV    nvarchar(MAX),
	@KeteranganAcceptance    nvarchar(MAX),
	@UangDiterimaFinnance    float,
	@TotalTrxEdc    float,
	@TotalNominalDebit    float,
	@TotalTrxEmoney    float,
	@TotalNominalDebitEmoney    float,
	@Status    int
AS
BEGIN
	SET NOCOUNT ON;
	if not exists(select*from LogClosing where idLog = @idLog)
	begin
		insert into 
		LogClosing
		(
			IdLog
			,Datetime
			,NamaComputer
			,NamaUser
			,TotalAllTicket
			,TotalTransaksi
			,TotalTopup
			,TotalRegis
			,TotalRefund
			,TotalFoodcourt
			,TotalDanaModal
			,TotalCashOut
			,TotalCashIn
			,TotalCashBox
			,TotalCashirInputMoneyCashbox
			,MinusIndikasiMoneyCashBox
			,MatchingSucces
			,StatusAcceptanceBySPV
			,KeteranganAcceptance
			,UangDiterimaFinnance
			,TotalTrxEdc
			,TotalNominalDebit
			,TotalTrxEmoney
			,TotalNominalDebitEmoney
			,Status

		)
		values(
			@IdLog
			,@Datetime
			,@NamaComputer
			,@NamaUser
			,@TotalAllTicket
			,@TotalTransaksi
			,@TotalTopup
			,@TotalRegis
			,@TotalRefund
			,@TotalFoodcourt
			,@TotalDanaModal
			,@TotalCashOut
			,@TotalCashIn
			,@TotalCashBox
			,@TotalCashirInputMoneyCashbox
			,@MinusIndikasiMoneyCashBox
			,@MatchingSucces
			,@StatusAcceptanceBySPV
			,@KeteranganAcceptance
			,@UangDiterimaFinnance
			,@TotalTrxEdc
			,@TotalNominalDebit
			,@TotalTrxEmoney
			,@TotalNominalDebitEmoney
			,@Status
		)
		select @idLog as Id
	end
END



