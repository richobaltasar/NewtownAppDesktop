CREATE PROCEDURE [dbo].[SP_SaveTransaksiRefund]
	@AccountNumber nvarchar(max),
	@SaldoEmoney float,
	@SaldoJaminan float,
	@TicketWeekDay float,
	@TicketWeekEnd float,
	@TotalNominalRefund float,
	@ChasierBy nvarchar(max),
	@ComputerName nvarchar(max)
AS
BEGIN
	SET NOCOUNT ON;

	insert into LogRefundDetail
	(Datetime,AccountNumber,SaldoEmoney,SaldoJaminan,TicketWeekDay,
	TicketWeekEnd,TotalNominalRefund,ChasierBy,ComputerName,Status)
	values(FORMAT(GETDATE() , 'dd/MM/yyyy HH:mm:ss'),@AccountNumber,@SaldoEmoney,@SaldoJaminan,@TicketWeekDay,
	@TicketWeekEnd,@TotalNominalRefund,@ChasierBy,@ComputerName,1)

	Insert into [dbo].[LogDeposit]
	([Datetime],[AccountNumber],[TransactionType],[Nominal],[Status])
	values
	(
		FORMAT(GETDATE() , 'dd/MM/yyyy HH:mm:ss'),@AccountNumber,'DEBIT',@TotalNominalRefund,1
	)

	if exists(select*from DataDeposit where Datetime = FORMAT(GETDATE() , 'dd/MM/yyyy') and Status = 1)
	begin
		declare @Deposit float
		declare @SisaDeposit float
		set @Deposit = (select Deposit from DataDeposit where Datetime = FORMAT(GETDATE() , 'dd/MM/yyyy') and Status = 1)
		set @SisaDeposit = @Deposit - @TotalNominalRefund

		update DataDeposit set Deposit = @SisaDeposit
		where Datetime = FORMAT(GETDATE() , 'dd/MM/yyyy')
		and Status = 1

		if exists(select*from DataAccount where AccountNumber = @AccountNumber and Status = 1)
		begin
			update DataAccount 
			set Balanced = 0,UangJaminan = 0,RefundDate = FORMAT(GETDATE() , 'dd/MM/yyyy HH:mm:ss')
			where AccountNumber = @AccountNumber
			and Status = 1
		end

		
	end

	declare @TotalChasin float
	declare @TotalChasout float
	declare @TotalUangDiBox float
	set @TotalChasin = ISNULL((select TotalUangMasuk from DataChasierBox where NamaComputer = @ComputerName and left(Datetime,10) = FORMAT(GETDATE() , 'dd/MM/yyyy') and status =1),0)
	set @TotalChasout = ISNULL((select TotalUangKeluar from DataChasierBox where NamaComputer = @ComputerName and left(Datetime,10) = FORMAT(GETDATE() , 'dd/MM/yyyy') and status =1),0)
	set @TotalUangDiBox = ISNULL((select TotalUangDiBox from DataChasierBox where NamaComputer = @ComputerName and left(Datetime,10) = FORMAT(GETDATE() , 'dd/MM/yyyy') and status =1),0)

	update DataChasierBox 
	set 
	TotalUangKeluar = (@TotalChasout+@TotalNominalRefund),
	TotalUangDiBox = ((@TotalChasin) - (@TotalChasout+@TotalNominalRefund))
	where NamaComputer = @ComputerName and left(Datetime,10) =   FORMAT(GETDATE() , 'dd/MM/yyyy')
	and Status = 1

	select 'TRUE' as Success, 'Insert log Refund berhasil dilakukan ~'+FORMAT(GETDATE() , 'dd/MM/yyyy HH:mm:ss') as _Message

END


















