-- sp_helptext SP_UploadLogRegistrasiDetail
CREATE PROCEDURE [dbo].[SP_UploadLogRegistrasiDetail]
	@idTrx    bigint
	,@Datetime    nvarchar(50)
	,@AccountNumber    nvarchar(50)
	,@SaldoEmoney    float
	,@SaldoEmoneyAfter    float
	,@TicketWeekDay    float
	,@TicketWeekDayAfter    float
	,@TicketWeekEnd    float
	,@TicketWeekEndAfter    float
	,@SaldoJaminan    float
	,@SaldoJaminanAfter    float
	,@IdTicketTrx    bigint
	,@Cashback    float
	,@Topup    float
	,@Asuransi    float
	,@QtyTotalTiket    float
	,@TotalBeliTiket    float
	,@TotalAll    float
	,@JenisTransaksi    nvarchar(MAX)
	,@TotalBayar    float
	,@PayEmoney    float
	,@PayCash    float
	,@TerimaUang    float
	,@Kembalian    float
	,@CashierBy    nvarchar(MAX)
	,@ComputerName    nvarchar(MAX)
	,@status    int
	,@IdLogEDCTransaksi    bigint
	,@BankCode    nvarchar(50)
	,@NamaBank    nvarchar(MAX)
	,@DiskonBank    float
	,@NominalDiskon    float
	,@AdminCharges    float
	,@TotalDebit    float
AS
BEGIN
	SET NOCOUNT ON;
	if not exists(select*from LogRegistrasiDetail where  idTrx= @idTrx)
	begin
		insert into 
		LogRegistrasiDetail
		(
			idTrx
			,Datetime
			,AccountNumber
			,SaldoEmoney
			,SaldoEmoneyAfter
			,TicketWeekDay
			,TicketWeekDayAfter
			,TicketWeekEnd
			,TicketWeekEndAfter
			,SaldoJaminan
			,SaldoJaminanAfter
			,IdTicketTrx
			,Cashback
			,Topup
			,Asuransi
			,QtyTotalTiket
			,TotalBeliTiket
			,TotalAll
			,JenisTransaksi
			,TotalBayar
			,PayEmoney
			,PayCash
			,TerimaUang
			,Kembalian
			,CashierBy
			,ComputerName
			,status
			,IdLogEDCTransaksi
			,BankCode
			,NamaBank
			,DiskonBank
			,NominalDiskon
			,AdminCharges
			,TotalDebit
		)
		values(
			@idTrx
			,@Datetime
			,@AccountNumber
			,@SaldoEmoney
			,@SaldoEmoneyAfter
			,@TicketWeekDay
			,@TicketWeekDayAfter
			,@TicketWeekEnd
			,@TicketWeekEndAfter
			,@SaldoJaminan
			,@SaldoJaminanAfter
			,@IdTicketTrx
			,@Cashback
			,@Topup
			,@Asuransi
			,@QtyTotalTiket
			,@TotalBeliTiket
			,@TotalAll
			,@JenisTransaksi
			,@TotalBayar
			,@PayEmoney
			,@PayCash
			,@TerimaUang
			,@Kembalian
			,@CashierBy
			,@ComputerName
			,@status
			,@IdLogEDCTransaksi
			,@BankCode
			,@NamaBank
			,@DiskonBank
			,@NominalDiskon
			,@AdminCharges
			,@TotalDebit
		)
		select @idTrx as Id
	end
END



