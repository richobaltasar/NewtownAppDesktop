-- Batch submitted through debugger: dbewats.sql|918|0|C:\Users\Administrator\Desktop\dbewats.sql
CREATE PROCEDURE [dbo].[SP_GetDataTransaksiFoodCourtReprint]
	@IdTrx bigint,
	@Datetime nvarchar(max),
	@Nominal float,
	@CashierBy  nvarchar(max)
AS
BEGIN
	SET NOCOUNT ON;
	select
		IdTrx,Datetime,ComputerName,CashierBy, TotalBayar,
		JenisTransaksi,PayEmoney,AccountNumber,SaldoEmoney,IdItemsKeranjang,
		SaldoEmoneyAfter from LogFoodcourtTransaksi
	where IdTrx = @IdTrx and Datetime = @Datetime 
	and TotalBayar = @Nominal and CashierBy = @CashierBy
END












