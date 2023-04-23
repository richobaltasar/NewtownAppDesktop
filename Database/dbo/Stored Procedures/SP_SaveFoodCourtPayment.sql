CREATE PROCEDURE [dbo].[SP_SaveFoodCourtPayment]
	@AccountNumber nvarchar(max),
	@SaldoEmoney float,
	@SaldoEmoneyAfter float,
	@IdItemsKeranjang bigint,
	@JenisTransaksi nvarchar(max),
	@TotalBayar float,
	@PayEmoney float,
	@PayCash float,
	@TerimaUang float,
	@Kembalian float,
	@ComputerName nvarchar(max),
	@CashierBy nvarchar(max)
AS
BEGIN
	SET NOCOUNT ON;
	
	insert into [dbo].[LogFoodcourtTransaksi]
	(Datetime,AccountNumber,SaldoEmoney,SaldoEmoneyAfter,IdItemsKeranjang,
	JenisTransaksi,TotalBayar,PayEmoney,PayCash,TerimaUang,Kembalian,
	ComputerName,CashierBy,Status)
	values(
		FORMAT(GETDATE() , 'dd/MM/yyyy HH:mm:ss'),
		@AccountNumber,@SaldoEmoney,@SaldoEmoneyAfter,@IdItemsKeranjang,
		@JenisTransaksi,@TotalBayar,@PayEmoney,@PayCash,@TerimaUang,@Kembalian,
		@ComputerName,@CashierBy,1
		)
	
	if(@JenisTransaksi = 'Cash')
	begin
	declare @TotalChasin float
	declare @TotalChasout float
	declare @TotalUangDiBox float
	set @TotalChasin = ISNULL((select TotalUangMasuk from DataChasierBox where NamaComputer = @ComputerName and left(Datetime,10) = FORMAT(GETDATE() , 'dd/MM/yyyy') and status =1),0)
	set @TotalChasout = ISNULL((select TotalUangKeluar from DataChasierBox where NamaComputer = @ComputerName and left(Datetime,10) = FORMAT(GETDATE() , 'dd/MM/yyyy') and status =1),0)
	set @TotalUangDiBox = ISNULL((select TotalUangDiBox from DataChasierBox where NamaComputer = @ComputerName and left(Datetime,10) = FORMAT(GETDATE() , 'dd/MM/yyyy') and status =1),0)


	update DataChasierBox 
	set TotalUangMasuk =(@TotalChasin+@TerimaUang),
	TotalUangKeluar = (@TotalChasout+@Kembalian),
	TotalUangDiBox = ((@TotalChasin+@TerimaUang) - (@TotalChasout+@Kembalian))
	where NamaComputer = @ComputerName and left(Datetime,10) =   FORMAT(GETDATE() , 'dd/MM/yyyy')
	and Status = 1

	end
	else
	begin
		if(@PayEmoney > 0)
		begin
			Insert into [dbo].[LogDeposit]
			([Datetime],[AccountNumber],[TransactionType],[Nominal],[Status])
			values
			(
				FORMAT(GETDATE() , 'dd/MM/yyyy HH:mm:ss'),@AccountNumber,'DEBIT',@PayEmoney,1
			)
		end
		
	end
	select 'Insert LogFoodcourtTransaksi Success ~'+FORMAT(GETDATE() , 'dd/MM/yyyy HH:mm:ss') _Message,'TRUE' Success


END
















