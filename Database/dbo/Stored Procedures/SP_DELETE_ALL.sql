
CREATE PROCEDURE [dbo].[SP_DELETE_ALL]
AS
BEGIN
	SET NOCOUNT ON;
	delete from [dbo].[DataAccount] 
	delete from [dbo].[DataChasierBox] 
	delete from [dbo].[LogClosing]
	delete from [dbo].[LogEmoneyTrxAccount]
	delete from [dbo].[LogFoodcourtTransaksi]
	delete from [dbo].[LogItemsF&BTrx]
	delete from [dbo].[LogRefundDetail]
	delete from [dbo].[LogRegistrasiDetail]
	delete from [dbo].[LogTicketDetail]
	delete from [dbo].[LogTopupDetail]
	delete from LogCashierTambahModal
	delete from [LogDeposit]
	delete from LogEDCTransaksi
	delete from [dbo].[LogSetoranDepositExpired]
	delete from [dbo].[LogStokOpname]
	delete from DataDeposit
END












