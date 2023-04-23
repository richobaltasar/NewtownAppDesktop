CREATE PROCEDURE [dbo].[SP_SaveTransaksiRegistrasi]
	@AccountNumber nvarchar(max),
	@SaldoEmoney float,
	@SaldoEmoneyAfter float,
	@TicketWeekDay float,
	@TicketWeekDayAfter float,
	@TicketWeekEnd float,
	@TicketWeekEndAfter float,
	@SaldoJaminan float,
	@SaldoJaminanAfter float,
	@IdTicketTrx bigint,
	@Cashback float,
	@Topup float,
	@Asuransi float,
	@QtyTotalTiket float,
	@TotalBeliTiket float,
	@TotalAll float,
	@JenisTransaksi nvarchar(max),
	@TotalBayar float,
	@PayEmoney float,
	@PayCash float,
	@TerimaUang float,
	@Kembalian float,
	@ComputerName nvarchar(max),
	@Chasier nvarchar(max)
AS
BEGIN
	SET NOCOUNT ON;
	
	insert into [dbo].[LogRegistrasiDetail]
	(Datetime,AccountNumber,SaldoEmoney,SaldoEmoneyAfter,TicketWeekDay,TicketWeekDayAfter,TicketWeekEnd,
	TicketWeekEndAfter,SaldoJaminan,SaldoJaminanAfter,IdTicketTrx,Cashback,Topup,Asuransi,QtyTotalTiket,
	TotalBeliTiket,TotalAll,JenisTransaksi,TotalBayar,PayEmoney,PayCash,TerimaUang,Kembalian,
	CashierBy,ComputerName,status)
	values(
		FORMAT(GETDATE() , 'dd/MM/yyyy HH:mm:ss'),
		@AccountNumber,@SaldoEmoney,@SaldoEmoneyAfter,@TicketWeekDay,@TicketWeekDayAfter,@TicketWeekEnd,
		@TicketWeekEndAfter,@SaldoJaminan,@SaldoJaminanAfter,@IdTicketTrx,@Cashback,@Topup,@Asuransi,@QtyTotalTiket,
		@TotalBeliTiket,@TotalAll,@JenisTransaksi,@TotalBayar,@PayEmoney,@PayCash,@TerimaUang,@Kembalian,
		@Chasier,@ComputerName,1
	)

	-- Save log Deposit untuk Dana yang terutang kas Finance
	if(@PayEmoney = 0)
	begin
		declare @TotalSaldo float
		
		if exists(select*from DataAccount where AccountNumber=@AccountNumber and Status = 1)
		begin
			if(@Topup > 0)
			begin
				Insert into [dbo].[LogDeposit]
				([Datetime],[AccountNumber],[TransactionType],[Nominal],[Status])
				values
				(
					FORMAT(GETDATE() , 'dd/MM/yyyy HH:mm:ss'),@AccountNumber,'CREDIT',@Topup,1
				)
			end
		end
		else
		begin
			set @TotalSaldo = (@SaldoEmoneyAfter+@SaldoJaminanAfter)
			Insert into [dbo].[LogDeposit]
			([Datetime],[AccountNumber],[TransactionType],[Nominal],[Status])
			values
			(
				FORMAT(GETDATE() , 'dd/MM/yyyy HH:mm:ss'),@AccountNumber,'CREDIT',@TotalSaldo,1
			)
		end
	end
	else
	begin
		Insert into [dbo].[LogDeposit]
		([Datetime],[AccountNumber],[TransactionType],[Nominal],[Status])
		values
		(
			FORMAT(GETDATE() , 'dd/MM/yyyy HH:mm:ss'),@AccountNumber,'DEBIT',@PayEmoney,1
		)
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
	and status = 1

	select 'Insert LogRegistrasiDetail Success ~'+FORMAT(GETDATE() , 'dd/MM/yyyy HH:mm:ss') _Message,'TRUE' Success
END


















