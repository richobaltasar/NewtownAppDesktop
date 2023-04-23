
CREATE PROCEDURE [dbo].[SP_SubmitSetorDepositExp]
	@AccountNumber nvarchar(max), 
	@Balanced float,
	@UangJaminan float,
	@TotalDeposit float,
	@NamaPenyetor nvarchar(max),
	@TanggalSetor nvarchar(max)
AS
BEGIN	
	SET NOCOUNT ON;
	if exists(select*from DataAccount where AccountNumber = @AccountNumber)
	begin
		select * into #temp from DataAccount where AccountNumber = @AccountNumber
		declare @Tanggaexpired nvarchar(50)
		set @Tanggaexpired = (select top 1 ExpiredDate from #temp)

		delete DataAccount where AccountNumber = @AccountNumber
		insert into LogSetoranDepositExpired
		(Datetime,AccountNumber,Saldo,UangJaminan,TotalDeposit,TanggalExpired,NamaPenyetor,TanggalSetor,StatusSetor)
		values(
			FORMAT(GETDATE() , 'dd/MM/yyyy HH:mm:ss'),@AccountNumber,@Balanced,@UangJaminan,@TotalDeposit,@Tanggaexpired,@NamaPenyetor,@TanggalSetor,1
		)
	end
END







