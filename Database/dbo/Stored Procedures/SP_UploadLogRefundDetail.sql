-- sp_helptext 
CREATE PROCEDURE [dbo].[SP_UploadLogRefundDetail]
	@IdRefund    bigint
	,@Datetime    nvarchar(50)
	,@AccountNumber    nvarchar(50)
	,@SaldoEmoney    float
	,@SaldoJaminan    float
	,@TicketWeekDay    float
	,@TicketWeekEnd    float
	,@TotalNominalRefund    float
	,@ChasierBy    nvarchar(MAX)
	,@ComputerName    nvarchar(MAX)
	,@Status    int
AS
BEGIN
	SET NOCOUNT ON;
	if not exists(select*from LogRefundDetail where  IdRefund= @IdRefund)
	begin
		insert into 
		LogRefundDetail
		(
			IdRefund
			,Datetime
			,AccountNumber
			,SaldoEmoney
			,SaldoJaminan
			,TicketWeekDay
			,TicketWeekEnd
			,TotalNominalRefund
			,ChasierBy
			,ComputerName
			,Status
		)
		values(
			@IdRefund
			,@Datetime
			,@AccountNumber
			,@SaldoEmoney
			,@SaldoJaminan
			,@TicketWeekDay
			,@TicketWeekEnd
			,@TotalNominalRefund
			,@ChasierBy
			,@ComputerName
			,@Status
		)
		select @IdRefund as Id
	end
END



