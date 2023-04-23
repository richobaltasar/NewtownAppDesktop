CREATE PROCEDURE [dbo].[SP_FSaveTolakClosing]
	@IdLog bigint,
	@Setoran float,
	@Catatan nvarchar(max),
	@Action nvarchar(max)
AS
BEGIN
	SET NOCOUNT ON;
	if exists(select * from LogClosing where IdLog = @IdLog)
	begin
		declare @datetime nvarchar(50)
		declare @NamaComputer nvarchar(max)
		declare @NamaUser nvarchar(50)
		select @NamaComputer=NamaComputer,@NamaUser=NamaUser,@datetime=left(Datetime,10) from LogClosing where IdLog = @IdLog
		if( @datetime = FORMAT(GETDATE() , 'dd/MM/yyyy'))
		begin 
			--select* from LogClosing 
			update LogClosing 
			set StatusAcceptanceBySPV = 'Rejected',
			KeteranganAcceptance=@Catatan,
			UangDiterimaFinnance= @Setoran,
			status = 2
			where Idlog = @IdLog

			--select*from [dbo].[LogTopupDetail] 
			update [dbo].[LogTopupDetail] set Status = 1
			where left(Datetime,10) = @datetime 
			and Status = 2 and ComputerName= @NamaComputer 
			and Chasierby = @NamaUser

			--select*from [dbo].[LogTicketDetail]
			update [dbo].[LogTicketDetail] set Status = 1
			where left(Datetime,10) = @datetime 
			and Status = 2 and ComputerName= @NamaComputer 
			and ChasierBy = @NamaUser

			--select*from [dbo].[LogRegistrasiDetail]
			update [dbo].[LogRegistrasiDetail] set status=1
			where left(Datetime,10) = @datetime 
			and status = 2 and ComputerName = @NamaComputer
			and CashierBy = @NamaUser
			
			--select*from [dbo].[LogRefundDetail]
			update [dbo].[LogRefundDetail] set Status= 1
			where left(Datetime,10) = @datetime 
			and status = 2 and ComputerName = @NamaComputer
			and ChasierBy = @NamaUser
			
			--select*from [dbo].[LogItemsF&BTrx]
			update [dbo].[LogItemsF&BTrx] set Status=1
			where left(Datetime,10) = @datetime 
			and Status = 2 and ComputerName = @NamaComputer
			and ChasierBy = @NamaUser
			
			--select*from [dbo].[LogFoodcourtTransaksi]
			update [dbo].[LogFoodcourtTransaksi] set Status=1
			where left(Datetime,10) = @datetime 
			and Status = 2 and ComputerName = @NamaComputer
			and CashierBy = @NamaUser
			
			select 'Success' as title, 'Data Closing berhasil ditolak' as message,
			'success' as icon
		end
		else
		begin
			select 'Warning' as title, 'Data Closing sudah expired untuk ditolak' as message,
			'error' as icon
		end
	end
	else
	begin
		select 'Warning' as title, 'Data Closing tidak Valid' as message,
		'error' as icon
	end
END










