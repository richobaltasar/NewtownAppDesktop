CREATE PROCEDURE [dbo].[Tool_SP_GetDashboard_V1_0_0_9]
	@ComputerName nvarchar(max),
	@Username nvarchar(max),
	@Datetime nvarchar(max)
AS
BEGIN
	SET NOCOUNT ON;
	declare @TicketSalesCounting float
	declare @KartuBaruCounting float
	declare @TotalKartuRefundCounting float
	
	declare @TotalKartuBaruNominal float

	declare @TicketPayCash float
	declare @TicketPaySaldo float
	declare @TicketPaySaldoNCash float
	declare @PayEDC float
	declare @PaySaldoEDC float
	declare @TicketTotalAmount float

	declare @TotalTopupCash float
	declare @TotalTopupEDC float
	declare @TotalTopup float

	declare @FNBPayCash float
	declare @FNBPaySaldo float
	declare @FNBAll float

	declare @RefundSaldo float
	declare @RefundJaminan float
	declare @TotalRefund float

	select @TicketSalesCounting = sum(isnull(QtyTotalTiket,0)) from LogRegistrasiDetail
	where replace(CONVERT(VARCHAR(10), CONVERT(date, replace(left(Datetime,10),'/','-'), 105), 23),'-','') = replace(CONVERT(VARCHAR(10), CONVERT(date, replace(left(@Datetime,10),'/','-'), 105), 23),'-','')
	and ComputerName = @ComputerName and CashierBy = @Username

	select @KartuBaruCounting = count(idTrx) from LogRegistrasiDetail
	where replace(CONVERT(VARCHAR(10), CONVERT(date, replace(left(Datetime,10),'/','-'), 105), 23),'-','') = replace(CONVERT(VARCHAR(10), CONVERT(date, replace(left(@Datetime,10),'/','-'), 105), 23),'-','')
	and SaldoJaminan = 0 and SaldoJaminanAfter > 0
	and ComputerName = @ComputerName  and CashierBy = @Username
	
	select @TotalKartuRefundCounting = count(IdRefund) from LogRefundDetail
	where replace(CONVERT(VARCHAR(10), CONVERT(date, replace(left(Datetime,10),'/','-'), 105), 23),'-','') = replace(CONVERT(VARCHAR(10), CONVERT(date, replace(left(@Datetime,10),'/','-'), 105), 23),'-','')
	and ComputerName = @ComputerName  and ChasierBy = @Username

	select @TotalKartuBaruNominal = sum(isnull(SaldoJaminanAfter,0)) from LogRegistrasiDetail
	where replace(CONVERT(VARCHAR(10), CONVERT(date, replace(left(Datetime,10),'/','-'), 105), 23),'-','') = replace(CONVERT(VARCHAR(10), CONVERT(date, replace(left(@Datetime,10),'/','-'), 105), 23),'-','')
	and SaldoJaminan = 0 and SaldoJaminanAfter > 0
	and ComputerName = @ComputerName  and CashierBy = @Username
	
	----Ticket transaksi
	select @TicketPayCash = sum(isnull(PayCash,0) - isnull(Topup,0)) from LogRegistrasiDetail
	where replace(CONVERT(VARCHAR(10), CONVERT(date, replace(left(Datetime,10),'/','-'), 105), 23),'-','') = replace(CONVERT(VARCHAR(10), CONVERT(date, replace(left(@Datetime,10),'/','-'), 105), 23),'-','')
	and isnull(PayEmoney,0) = 0 and isnull(TotalDebit,0) = 0 and ComputerName = @ComputerName  and CashierBy = @Username	

	select @TicketPaySaldo = sum(isnull(PayEmoney,0)) from LogRegistrasiDetail
	where replace(CONVERT(VARCHAR(10), CONVERT(date, replace(left(Datetime,10),'/','-'), 105), 23),'-','') = replace(CONVERT(VARCHAR(10), CONVERT(date, replace(left(@Datetime,10),'/','-'), 105), 23),'-','')
	and  isnull(PayCash,0)= 0 and isnull(TotalDebit,0) = 0 and  ComputerName = @ComputerName  and CashierBy = @Username

	select @TicketPaySaldoNCash = isnull((sum(isnull(PayEmoney,0))+sum(isnull(PayCash,0))- sum(isnull(Topup,0))),0) from LogRegistrasiDetail
	where replace(CONVERT(VARCHAR(10), CONVERT(date, replace(left(Datetime,10),'/','-'), 105), 23),'-','') = replace(CONVERT(VARCHAR(10), CONVERT(date, replace(left(@Datetime,10),'/','-'), 105), 23),'-','')
	and  isnull(PayCash,0) > 0 and isnull(PayEmoney,0) > 0 and isnull(TotalDebit,0) = 0 and ComputerName = @ComputerName  and CashierBy = @Username

	select @PayEDC=sum(isnull(TotalDebit,0)- isnull(Topup,0)) from LogRegistrasiDetail
	where replace(CONVERT(VARCHAR(10), CONVERT(date, replace(left(Datetime,10),'/','-'), 105), 23),'-','') = replace(CONVERT(VARCHAR(10), CONVERT(date, replace(left(@Datetime,10),'/','-'), 105), 23),'-','')
	and  isnull(PayCash,0) = 0 and isnull(PayEmoney,0) = 0 and isnull(TotalDebit,0) > 0 and ComputerName = @ComputerName  and CashierBy = @Username

	select @PaySaldoEDC=(sum(isnull(TotalDebit,0))+sum(isnull(PayEmoney,0))- sum(isnull(Topup,0))) from LogRegistrasiDetail
	where replace(CONVERT(VARCHAR(10), CONVERT(date, replace(left(Datetime,10),'/','-'), 105), 23),'-','') = replace(CONVERT(VARCHAR(10), CONVERT(date, replace(left(@Datetime,10),'/','-'), 105), 23),'-','')
	and  isnull(PayCash,0) = 0 and isnull(PayEmoney,0) > 0 and isnull(TotalDebit,0) > 0 and ComputerName = @ComputerName  and CashierBy = @Username

	select @TicketTotalAmount=((sum(isnull(PayCash,0)) + sum(isnull(PayEmoney,0))+sum(isnull(TotalDebit,0)))-sum(isnull(Topup,0))) from LogRegistrasiDetail
	where replace(CONVERT(VARCHAR(10), CONVERT(date, replace(left(Datetime,10),'/','-'), 105), 23),'-','') = replace(CONVERT(VARCHAR(10), CONVERT(date, replace(left(@Datetime,10),'/','-'), 105), 23),'-','')
	and ComputerName = @ComputerName  and CashierBy = @Username
	
	--Topup Transaksi

	select @TotalTopupCash =
	(
		isnull(
		(
			select sum(isnull(NominalTopup,0)) from LogTopupDetail
			where replace(CONVERT(VARCHAR(10), CONVERT(date, replace(left(Datetime,10),'/','-'), 105), 23),'-','') = replace(CONVERT(VARCHAR(10), CONVERT(date, replace(left(@Datetime,10),'/','-'), 105), 23),'-','')
			and ComputerName = @ComputerName  and Chasierby = @Username
			and PayCash > 0 and ISNULL(TotalDebit,0) = 0
		),0)
		+
		isnull(
		(
			select sum(isnull(Topup,0)) from LogRegistrasiDetail
			where replace(CONVERT(VARCHAR(10), CONVERT(date, replace(left(Datetime,10),'/','-'), 105), 23),'-','') = replace(CONVERT(VARCHAR(10), CONVERT(date, replace(left(@Datetime,10),'/','-'), 105), 23),'-','')
			and Topup > 0 and isnull(PayCash,0) > 0 and isnull(TotalDebit,0) = 0
			and ComputerName = @ComputerName  and CashierBy = @Username
		),0)
	)

	select @TotalTopupEDC =
	(
		isnull(
		(
			select sum(isnull(NominalTopup,0)) from LogTopupDetail
			where replace(CONVERT(VARCHAR(10), CONVERT(date, replace(left(Datetime,10),'/','-'), 105), 23),'-','') = replace(CONVERT(VARCHAR(10), CONVERT(date, replace(left(@Datetime,10),'/','-'), 105), 23),'-','')
			and isnull(PayCash,0) = 0 and ISNULL(TotalDebit,0) > 0
			and ComputerName = @ComputerName  and Chasierby = @Username
			
		),0)
		+
		isnull(
		(
			select sum(isnull(Topup,0)) from LogRegistrasiDetail
			where replace(CONVERT(VARCHAR(10), CONVERT(date, replace(left(Datetime,10),'/','-'), 105), 23),'-','') = replace(CONVERT(VARCHAR(10), CONVERT(date, replace(left(@Datetime,10),'/','-'), 105), 23),'-','')
			and Topup > 0 and isnull(PayCash,0) = 0 and isnull(TotalDebit,0) > 0
			and ComputerName = @ComputerName  and CashierBy = @Username
		),0)
	)

	select @TotalTopup =
	(
		isnull(
		(
			select sum(isnull(NominalTopup,0)) from LogTopupDetail
			where replace(CONVERT(VARCHAR(10), CONVERT(date, replace(left(Datetime,10),'/','-'), 105), 23),'-','') = replace(CONVERT(VARCHAR(10), CONVERT(date, replace(left(@Datetime,10),'/','-'), 105), 23),'-','')
			and ComputerName = @ComputerName  and Chasierby = @Username
		),0)
		+
		isnull(
		(
			select sum(isnull(Topup,0)) from LogRegistrasiDetail
			where replace(CONVERT(VARCHAR(10), CONVERT(date, replace(left(Datetime,10),'/','-'), 105), 23),'-','') = replace(CONVERT(VARCHAR(10), CONVERT(date, replace(left(@Datetime,10),'/','-'), 105), 23),'-','')
			and Topup > 0
			and ComputerName = @ComputerName  and CashierBy = @Username
		),0)
	)

	select @FNBPayCash = sum(isnull(PayCash,0)) from LogFoodcourtTransaksi
	where replace(CONVERT(VARCHAR(10), CONVERT(date, replace(left(Datetime,10),'/','-'), 105), 23),'-','') = replace(CONVERT(VARCHAR(10), CONVERT(date, replace(left(@Datetime,10),'/','-'), 105), 23),'-','')
	and isnull(PayCash,0) > 0 and ISNULL(PayEmoney,0) = 0
	and ComputerName = @ComputerName  and CashierBy = @Username

	select @FNBPaySaldo = sum(isnull(PayEmoney,0)) from LogFoodcourtTransaksi
	where replace(CONVERT(VARCHAR(10), CONVERT(date, replace(left(Datetime,10),'/','-'), 105), 23),'-','') = replace(CONVERT(VARCHAR(10), CONVERT(date, replace(left(@Datetime,10),'/','-'), 105), 23),'-','')
	and isnull(PayCash,0) = 0 and ISNULL(PayEmoney,0) > 0
	and ComputerName = @ComputerName  and CashierBy = @Username

	select @FNBAll = (@FNBPayCash+@FNBPaySaldo)

	select @RefundJaminan = sum(isnull(SaldoJaminan,0)) from LogRefundDetail 
	where replace(CONVERT(VARCHAR(10), CONVERT(date, replace(left(Datetime,10),'/','-'), 105), 23),'-','') = replace(CONVERT(VARCHAR(10), CONVERT(date, replace(left(@Datetime,10),'/','-'), 105), 23),'-','')
	and ComputerName = @ComputerName  and ChasierBy = @Username

	select @RefundSaldo = sum(isnull(SaldoEmoney,0)) from LogRefundDetail 
	where replace(CONVERT(VARCHAR(10), CONVERT(date, replace(left(Datetime,10),'/','-'), 105), 23),'-','') = replace(CONVERT(VARCHAR(10), CONVERT(date, replace(left(@Datetime,10),'/','-'), 105), 23),'-','')
	and ComputerName = @ComputerName  and ChasierBy = @Username

	select @TotalRefund = (@RefundJaminan+@RefundSaldo)

	declare @DanaModal float

	select @DanaModal=sum(isnull(NominalTambahModal,0)) from LogCashierTambahModal
	where NamaUser = @Username and NamaComputer = @ComputerName 
	and replace(CONVERT(VARCHAR(10), CONVERT(date, replace(left(Datetime,10),'/','-'), 105), 23),'-','') = replace(CONVERT(VARCHAR(10), CONVERT(date, replace(left(@Datetime,10),'/','-'), 105), 23),'-','')

	
	declare @TotalCashin float
	select @TotalCashin =
	(
		isnull(
		(
			select sum(isnull(TerimaUang,0)) from LogRegistrasiDetail
			where replace(CONVERT(VARCHAR(10), CONVERT(date, replace(left(Datetime,10),'/','-'), 105), 23),'-','') = replace(CONVERT(VARCHAR(10), CONVERT(date, replace(left(@Datetime,10),'/','-'), 105), 23),'-','')
			and ComputerName = @ComputerName  and CashierBy = @Username
		),0)
	+
		isnull(
		(
			select sum(isnull(TerimaUang,0)) from LogTopupDetail
			where replace(CONVERT(VARCHAR(10), CONVERT(date, replace(left(Datetime,10),'/','-'), 105), 23),'-','') = replace(CONVERT(VARCHAR(10), CONVERT(date, replace(left(@Datetime,10),'/','-'), 105), 23),'-','')
			and ComputerName = @ComputerName  and Chasierby = @Username
		),0)
	+
		isnull(
		(
			select sum(isnull(TerimaUang,0)) from LogFoodcourtTransaksi
			where replace(CONVERT(VARCHAR(10), CONVERT(date, replace(left(Datetime,10),'/','-'), 105), 23),'-','') = replace(CONVERT(VARCHAR(10), CONVERT(date, replace(left(@Datetime,10),'/','-'), 105), 23),'-','')
			and ComputerName = @ComputerName  and CashierBy = @Username
		),0)
	)

	declare @TotalCashOut float
	select @TotalCashOut =
	(
		isnull((
			select sum(isnull(Kembalian,0)) from LogRegistrasiDetail
			where replace(CONVERT(VARCHAR(10), CONVERT(date, replace(left(Datetime,10),'/','-'), 105), 23),'-','') = replace(CONVERT(VARCHAR(10), CONVERT(date, replace(left(@Datetime,10),'/','-'), 105), 23),'-','') 
			and ComputerName = @ComputerName  and CashierBy = @Username
		),0)
		+
		isnull((
			select sum(isnull(Kembalian,0)) from LogTopupDetail
			where replace(CONVERT(VARCHAR(10), CONVERT(date, replace(left(Datetime,10),'/','-'), 105), 23),'-','') = replace(CONVERT(VARCHAR(10), CONVERT(date, replace(left(@Datetime,10),'/','-'), 105), 23),'-','') 
			and ComputerName = @ComputerName  and Chasierby = @Username
		),0)
		+
		isnull((
			select sum(isnull(Kembalian,0)) from LogFoodcourtTransaksi
			where replace(CONVERT(VARCHAR(10), CONVERT(date, replace(left(Datetime,10),'/','-'), 105), 23),'-','') = replace(CONVERT(VARCHAR(10), CONVERT(date, replace(left(@Datetime,10),'/','-'), 105), 23),'-','') 
			and ComputerName = @ComputerName  and CashierBy = @Username
		),0)
		+
		isnull((
			select sum(ISNULL(TotalNominalRefund,0)) from LogRefundDetail 
			where replace(CONVERT(VARCHAR(10), CONVERT(date, replace(left(Datetime,10),'/','-'), 105), 23),'-','') = replace(CONVERT(VARCHAR(10), CONVERT(date, replace(left(@Datetime,10),'/','-'), 105), 23),'-','') 
			and ComputerName = @ComputerName  and ChasierBy = @Username
		),0)
	)

	declare @TotalEDC float

	select @TotalEDC =
	(
		isnull((
			select sum(isnull(TotalDebit,0)) from LogRegistrasiDetail
			where replace(CONVERT(VARCHAR(10), CONVERT(date, replace(left(Datetime,10),'/','-'), 105), 23),'-','') = replace(CONVERT(VARCHAR(10), CONVERT(date, replace(left(@Datetime,10),'/','-'), 105), 23),'-','') 
			and ComputerName = @ComputerName  and CashierBy = @Username
		),0)
		+
		isnull((
			select sum(isnull(TotalDebit,0)) from LogTopupDetail
			where replace(CONVERT(VARCHAR(10), CONVERT(date, replace(left(Datetime,10),'/','-'), 105), 23),'-','') = replace(CONVERT(VARCHAR(10), CONVERT(date, replace(left(@Datetime,10),'/','-'), 105), 23),'-','') 
			and ComputerName = @ComputerName  and Chasierby = @Username
		),0)
	)

	declare @TotalEmoney float
	select @TotalEmoney =
	(
		isnull((
			select sum(isnull(PayEmoney,0)) from LogRegistrasiDetail
			where replace(CONVERT(VARCHAR(10), CONVERT(date, replace(left(Datetime,10),'/','-'), 105), 23),'-','') = replace(CONVERT(VARCHAR(10), CONVERT(date, replace(left(@Datetime,10),'/','-'), 105), 23),'-','') 
			and ComputerName = @ComputerName  and CashierBy = @Username
		),0)
		+
		isnull((
			select sum(isnull(PayEmoney,0)) from LogFoodcourtTransaksi
			where replace(CONVERT(VARCHAR(10), CONVERT(date, replace(left(Datetime,10),'/','-'), 105), 23),'-','') = replace(CONVERT(VARCHAR(10), CONVERT(date, replace(left(@Datetime,10),'/','-'), 105), 23),'-','')
			and ComputerName = @ComputerName  and CashierBy = @Username
		),0)
	)


	declare @TotalCashBox float

	select @TotalCashBox = ((isnull(@TotalCashin,0) - isnull(@TotalCashOut,0)) + isnull(@DanaModal,0))

	declare @TotalTransaksiKasir float
	select @TotalTransaksiKasir = ((isnull(@TotalCashBox,0)-isnull(@DanaModal,0)) + (isnull(@TotalEDC,0) + isnull(@TotalEmoney,0)))	


	select isnull(@TicketSalesCounting,0) TicketSalesCounting,isnull(@KartuBaruCounting,0) KartuBaruCounting,isnull(@TotalKartuRefundCounting,0) TotalKartuRefundCounting,
	isnull(@TotalKartuBaruNominal,0) TotalKartuBaruNominal,
	isnull(@TicketPayCash,0) TicketPayCash,isnull(@TicketPaySaldo,0) TicketPaySaldo,isnull(@TicketPaySaldoNCash,0) TicketPaySaldoNCash,
	isnull(@PayEDC,0) PayEDC,isnull(@PaySaldoEDC,0) PaySaldoEDC,isnull(@TicketTotalAmount,0) TicketTotalAmount,
	isnull(@TotalTopupCash,0) TotalTopupCash,isnull(@TotalTopupEDC,0) TotalTopupEDC,isnull(@TotalTopup,0) TotalTopup,
	isnull(@FNBPayCash,0) FNBPayCash,isnull(@FNBPaySaldo,0) FNBPaySaldo,isnull(@FNBAll,0) FNBAll,
	isnull(@RefundJaminan,0) RefundJaminan,isnull(@RefundSaldo,0) RefundSaldo, isnull(@TotalRefund,0) TotalRefund,
	isnull(@DanaModal,0) DanaModal, isnull(@TotalCashin,0) TotalCashin, isnull(@TotalCashOut,0) TotalCashOut, isnull(@TotalEDC,0) TotalEDC, isnull(@TotalEmoney,0) TotalEmoney, isnull(@TotalCashBox,0) TotalCashBox,isnull(@TotalTransaksiKasir,0) TotalTransaksiKasir 
END

