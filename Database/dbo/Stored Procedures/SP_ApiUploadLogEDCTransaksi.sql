
CREATE PROCEDURE [dbo].[SP_ApiUploadLogEDCTransaksi]
	@IdLog    bigint,
	@Datetime    nvarchar(50),
	@TotalBelanja    float,
	@CodeBank    nvarchar(50),
	@NamaBank    nvarchar(MAX),
	@DiskonBank    float,
	@NominalDiskon    float,
	@AdminCharges    float,
	@TotalDebit    float,
	@NoATM    nvarchar(MAX),
	@NoReffEddPrint    nvarchar(MAX),
	@ComputerName    nvarchar(MAX),
	@CashierBy    nvarchar(MAX),
	@status    int
AS
BEGIN
	SET NOCOUNT ON;
	if not exists(select*from LogEDCTransaksi where  IdLog= @IdLog)
	begin
		insert into 
		LogEDCTransaksi
		(
			IdLog
			,Datetime
			,TotalBelanja
			,CodeBank
			,NamaBank
			,DiskonBank
			,NominalDiskon
			,AdminCharges
			,TotalDebit
			,NoATM
			,NoReffEddPrint
			,ComputerName
			,CashierBy
			,status
		)
		values(
			@IdLog
			,@Datetime
			,@TotalBelanja
			,@CodeBank
			,@NamaBank
			,@DiskonBank
			,@NominalDiskon
			,@AdminCharges
			,@TotalDebit
			,@NoATM
			,@NoReffEddPrint
			,@ComputerName
			,@CashierBy
			,@status
		)
		select @IdLog as Id
	end
END



