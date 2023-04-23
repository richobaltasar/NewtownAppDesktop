
CREATE PROCEDURE [dbo].[SP_ApiUploadLogDeposit]
	@LogId    bigint,
	@Datetime    nvarchar(50),
	@AccountNumber    nvarchar(50),
	@TransactionType    nvarchar(50),
	@Nominal    float,
	@Status    int
AS
BEGIN
	SET NOCOUNT ON;
	if not exists(select*from LogDeposit where logid = @LogId)
	begin
		insert into 
		LogDeposit
		(
			LogId
			,Datetime
			,AccountNumber
			,TransactionType
			,Nominal
			,Status
		)
		values(
			@LogId
			,@Datetime
			,@AccountNumber
			,@TransactionType
			,@Nominal
			,@Status
		)
		select @LogId as Id
	end
END



