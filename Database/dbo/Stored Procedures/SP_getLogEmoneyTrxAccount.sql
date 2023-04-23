
CREATE PROCEDURE [dbo].[SP_getLogEmoneyTrxAccount]
	@IdLog    bigint,
	@AcountNumber    nvarchar(50),
	@Datetime    nvarchar(50),
	@SaldoSebelumnya    float,
	@Credit    float,
	@Debit    float,
	@SisaSaldo    float,
	@Status    int

AS
BEGIN
	SET NOCOUNT ON;
	if not exists(select*from LogEmoneyTrxAccount where  IdLog= @IdLog)
	begin
		insert into 
		LogEmoneyTrxAccount
		(
			IdLog
			,AcountNumber
			,Datetime
			,SaldoSebelumnya
			,Credit
			,Debit
			,SisaSaldo
			,Status
		)
		values(
			@IdLog
			,@AcountNumber
			,@Datetime
			,@SaldoSebelumnya
			,@Credit
			,@Debit
			,@SisaSaldo
			,@Status
		)
		select @IdLog as Id
	end
END



