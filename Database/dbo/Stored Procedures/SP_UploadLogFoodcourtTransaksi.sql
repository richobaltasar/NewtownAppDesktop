
CREATE PROCEDURE [dbo].[SP_UploadLogFoodcourtTransaksi]
	@IdTrx    bigint,
	@Datetime    nvarchar(50),
	@AccountNumber    nvarchar(50),
	@SaldoEmoney    float,
	@SaldoEmoneyAfter    float,
	@IdItemsKeranjang    bigint,
	@JenisTransaksi    nvarchar(MAX),
	@TotalBayar    float,
	@PayEmoney    float,
	@PayCash    float,
	@TerimaUang    float,
	@Kembalian    float,
	@ComputerName    nvarchar(MAX),
	@CashierBy    nvarchar(MAX),
	@Status    int
AS
BEGIN
	SET NOCOUNT ON;
	if not exists(select*from LogFoodcourtTransaksi where  IdTrx= @IdTrx)
	begin
		insert into 
		LogFoodcourtTransaksi
		(
			IdTrx
			,Datetime
			,AccountNumber
			,SaldoEmoney
			,SaldoEmoneyAfter
			,IdItemsKeranjang
			,JenisTransaksi
			,TotalBayar
			,PayEmoney
			,PayCash
			,TerimaUang
			,Kembalian
			,ComputerName
			,CashierBy
			,Status
		)
		values(
			@IdTrx
			,@Datetime
			,@AccountNumber
			,@SaldoEmoney
			,@SaldoEmoneyAfter
			,@IdItemsKeranjang
			,@JenisTransaksi
			,@TotalBayar
			,@PayEmoney
			,@PayCash
			,@TerimaUang
			,@Kembalian
			,@ComputerName
			,@CashierBy
			,@Status
		)
		select @IdTrx as Id
	end
END



