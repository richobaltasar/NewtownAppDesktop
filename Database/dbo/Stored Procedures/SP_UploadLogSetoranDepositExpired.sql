-- sp_helptext SP_UploadLogRegistrasiDetail
CREATE PROCEDURE [dbo].[SP_UploadLogSetoranDepositExpired]
	@LogId    bigint
	,@Datetime    nvarchar(50)
	,@AccountNumber    nvarchar(MAX)
	,@Saldo    float
	,@UangJaminan    float
	,@TotalDeposit    float
	,@TanggalExpired    nvarchar(50)
	,@NamaPenyetor    nvarchar(MAX)
	,@TanggalSetor    nvarchar(50)
	,@StatusSetor    int
AS
BEGIN
	SET NOCOUNT ON;
	if not exists(select*from LogSetoranDepositExpired where  LogId= @LogId)
	begin
		insert into 
		LogSetoranDepositExpired
		(
			LogId
			,Datetime
			,AccountNumber
			,Saldo
			,UangJaminan
			,TotalDeposit
			,TanggalExpired
			,NamaPenyetor
			,TanggalSetor
			,StatusSetor
		)
		values(
			@LogId
			,@Datetime
			,@AccountNumber
			,@Saldo
			,@UangJaminan
			,@TotalDeposit
			,@TanggalExpired
			,@NamaPenyetor
			,@TanggalSetor
			,@StatusSetor
		)
		select @LogId as Id
	end
END



