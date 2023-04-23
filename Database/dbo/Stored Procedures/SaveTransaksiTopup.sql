CREATE PROCEDURE [dbo].[SaveTransaksiTopup]
	@AccountNumber nvarchar(max),
	@NominalTopup float,
	@JenisPayment nvarchar(max),
	@PayCash float,
	@TotalBayar float,
	@TerimaUang float,
	@Kembalian float,
	@SaldoSebelum float,
	@SaldoSetelah float,
	@Chasierby nvarchar(max),
	@ComputerName nvarchar(max)
AS
BEGIN
	SET NOCOUNT ON;
	

	if exists (select*from DataChasierBox where NamaComputer = @ComputerName and left(Datetime,10) =   FORMAT(GETDATE() , 'dd/MM/yyyy') and Status = 1)
	begin
		insert into LogTopupDetail
		(Datetime,AccountNumber,NominalTopup,JenisPayment,PayCash,TotalBayar,PaymentMethod,
		TerimaUang,Kembalian,SaldoSebelum,SaldoSetelah,Chasierby,ComputerName,Status)
		values(
			FORMAT(GETDATE() , 'dd/MM/yyyy HH:mm:ss'),
			@AccountNumber,@NominalTopup,@JenisPayment,@PayCash,@TotalBayar,'CASH',
			@TerimaUang,@Kembalian,@SaldoSebelum,@SaldoSetelah,@Chasierby,@ComputerName,1
		)
		-- Save log Deposit untuk Dana yang terutang kas Finance
		Insert into [dbo].[LogDeposit]
		([Datetime],[AccountNumber],[TransactionType],[Nominal],[Status])
		values
		(
			FORMAT(GETDATE() , 'dd/MM/yyyy HH:mm:ss'),@AccountNumber,'CREDIT',@NominalTopup,1
		)

		declare @Deposit float
		declare @Credit float
		declare @Debit float

		set @Deposit = (select sum(isnull(Balanced,0)+isnull(UangJaminan,0)) deposit from DataAccount where UangJaminan > 0 and status = 1)
		set @Credit = (select sum(Nominal) from LogDeposit where TransactionType = 'CREDIT' and left(Datetime,10) =  FORMAT(GETDATE() , 'dd/MM/yyyy') and status = 1)
		set @Debit = (select sum(Nominal) from LogDeposit where TransactionType = 'DEBIT' and left(Datetime,10) =  FORMAT(GETDATE() , 'dd/MM/yyyy') and status = 1)

		if exists(select*from DataDeposit where Datetime = FORMAT(GETDATE() , 'dd/MM/yyyy') and Status = 1)
		begin
			update DataDeposit
			set Deposit = @Deposit,
			Credit = @Credit,
			Debit = @Debit
			where Datetime = FORMAT(GETDATE() , 'dd/MM/yyyy')
			and Status = 1
		end
		else
		begin 
			insert into DataDeposit (Datetime,Deposit,Credit,Debit,Status)
			values(FORMAT(GETDATE() , 'dd/MM/yyyy'),@Deposit,@Credit,@Debit,1)
		end

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

		select 'TRUE' as Success, 'Insert log Topup berhasil dilakukan ~ '+FORMAT(GETDATE() , 'dd/MM/yyyy HH:mm:ss') as _Message
	end
	else 
	begin
		select 'FALSE' as Success, 'Insert log Topup Gagal dilakukan' as _Message
	end
	
END










