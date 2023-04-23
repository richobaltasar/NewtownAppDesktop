-- Batch submitted through debugger: dbewats.sql|943|0|C:\Users\Administrator\Desktop\dbewats.sql

CREATE PROCEDURE [dbo].[SP_GetDataTransaksiRefund]
	@IdTrx bigint,
	@Datetime nvarchar(max),
	@Nominal float,
	@CashierBy nvarchar(max)
AS
BEGIN
	SET NOCOUNT ON;
	select*from LogRefundDetail where IdRefund=@IdTrx
	and Datetime =@Datetime and TotalNominalRefund = @Nominal and ChasierBy=@CashierBy
END











