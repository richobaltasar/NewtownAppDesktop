-- Batch submitted through debugger: dbewats.sql|390|0|C:\Users\Administrator\Desktop\dbewats.sql
CREATE PROCEDURE [dbo].[SP_GateProc]
	@IdCard nvarchar(max),
	@Saldo float,
	@IdGate nvarchar(max)
AS
BEGIN
	SET NOCOUNT ON;
	declare @Success nvarchar(max)
	declare @Message nvarchar(max)

	if exists(select*from DataParam where NamaParam = 'DataGate' and val2 = @IdGate and val3='Aktif')
	begin
		select* into #temp from DataTicket where status = 'Aktif'
		create table #Ticket
		(
			Harga float
		)
		DECLARE @QRY NVARCHAR(MAX)
		SET @QRY = 'insert into #Ticket select '+ datename(dw,getdate()) +' as Harga from #temp where NamaTicket = ''Regular'' '
		EXEC SP_EXECUTESQL @QRY 
		declare @HargaTicket float
		set @HargaTicket = (select distinct Harga from #Ticket)
		drop table #Ticket
		if(@HargaTicket > 0)
		begin
			if exists(select*from DataAccount where AccountNumber = @IdCard)
			begin
				if(@Saldo > @HargaTicket)
				begin
					declare @SisaSaldo float
					set @SisaSaldo = @Saldo - @HargaTicket
					set @Success = 'True'
					set @Message = @SisaSaldo
					update DataAccount set Balanced = @SisaSaldo
					where AccountNumber = @IdCard

					Insert into [dbo].[LogDeposit]
					([Datetime],[AccountNumber],[TransactionType],[Nominal],[Status])
					values
					(
						FORMAT(GETDATE() , 'dd/MM/yyyy HH:mm:ss'),@IdCard,'DEBIT',@HargaTicket,1
					)

				end
				else
				begin
					set @Success = 'False'
					set @Message = 'Saldo tidak Cukup'
				end
			end
			else
			begin
				set @Success = 'False'
				set @Message = 'Kartu Tidak dikenali'
			end
		end
		else
		begin
			set @Success = 'False'
			set @Message = 'Harga Ticket Regular Nol?'
		end
	end
	else
	begin
		set @Success = 'False'
		set @Message = 'Gate Tidak Dikenali'
	end

	select @Success Success,@Message Message
	

END









