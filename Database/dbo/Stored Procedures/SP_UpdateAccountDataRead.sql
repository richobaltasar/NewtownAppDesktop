CREATE PROCEDURE [dbo].[SP_UpdateAccountDataRead] 
	@AccountNumber nvarchar(50), -- Account number == ID card
	@Balanced float,
	@JaminanGelang float,
	@Ticket float
AS
BEGIN
	SET NOCOUNT ON;
	if exists(select*from DataAccount where AccountNumber = @AccountNumber)
	begin
		update DataAccount set Balanced = @Balanced, Ticket = @Ticket,UpdateDate = FORMAT(GetDate(), 'dd/MM/yyyy HH:mm:ss'),
		UangJaminan = @JaminanGelang
		--ExpiredDate = FORMAT(dateadd(day, 30, getdate()),'dd/MM/yyyy HH:mm:ss')
		where AccountNumber = @AccountNumber
		select 'Update Success' as Result
	end
	else
	begin
		insert into DataAccount (AccountNumber, Balanced, Ticket,UangJaminan, CreateDate, ExpiredDate,UpdateDate, Status)
		values(@AccountNumber, @Balanced,@Ticket,@JaminanGelang,FORMAT(GetDate(), 'dd/MM/yyyy HH:mm:ss'),FORMAT(dateadd(day, 30, getdate()),'dd/MM/yyyy HH:mm:ss'),FORMAT(GetDate(), 'yyyy-MM-dd hh:mm:ss'),1)
		select 'Insert Success' as Result
	end

	declare @Deposit float
	declare @Credit float
	declare @Debit float

	set @Deposit = (select sum(isnull(Balanced,0)+isnull(UangJaminan,0)) deposit from DataAccount where UangJaminan > 0 and status = 1)
	set @Credit = (select sum(Nominal) from LogDeposit where TransactionType = 'CREDIT' and left(Datetime,10) =  FORMAT(GETDATE() , 'dd/MM/yyyy') and status = 1)
	set @Debit = (select sum(Nominal) from LogDeposit where TransactionType = 'DEBIT' and left(Datetime,10) =  FORMAT(GETDATE() , 'dd/MM/yyyy') and status = 1)

	if exists(select*from DataDeposit where Datetime = FORMAT(GETDATE() , 'dd/MM/yyyy'))
	begin
		update DataDeposit
		set Deposit = @Deposit,
		Credit = @Credit,
		Debit = @Debit
		where Datetime = FORMAT(GETDATE() , 'dd/MM/yyyy')
	end
	else
	begin 
		insert into DataDeposit (Datetime,Deposit,Credit,Debit,Status)
		values(FORMAT(GETDATE() , 'dd/MM/yyyy'),@Deposit,@Credit,@Debit,1)
	end

END















