CREATE PROCEDURE [dbo].[SP_FSaveTerimaClosing]
	@IdLog bigint,
	@Setoran float,
	@Catatan nvarchar(max),
	@Action nvarchar(max)
AS
BEGIN
	SET NOCOUNT ON;
	if exists(select * from LogClosing where IdLog = @IdLog)
	begin
		declare @CashBox float
		select @CashBox = TotalCashBox from LogClosing where IdLog = @IdLog
		if(@Action = 'Terima')
		begin	
			if(@Setoran = @CashBox)
			begin
				update LogClosing set MinusIndikasiMoneyCashBox = 0,
				MatchingSucces = 'Matching Success',
				StatusAcceptanceBySPV = 'Diterima',
				KeteranganAcceptance =@Catatan,
				UangDiterimaFinnance = @Setoran,
				Status = 2
				where IdLog = @IdLog				

				select Datetime,NamaComputer,NamaUser into #temp from LogClosing where IdLog = @IdLog
				
				declare @NamaUser nvarchar(100)
				set @NamaUser = (select NamaUser from #temp)

				update DataChasierBox set Status = 2, CloseBy = @NamaUser
				where NamaComputer in (select NamaComputer from #temp)
				and Datetime in (select left(Datetime,10) from #temp)
				and Status = 1

				update LogCashierTambahModal set Status = 2
				where replace(CONVERT(VARCHAR(10), CONVERT(date, replace(left(Datetime,10),'/','-'), 105), 23),'-','') = FORMAT(GETDATE() , 'yyyyMMdd')
				and NamaComputer in (select NamaComputer from #temp)
				and NamaUser = @NamaUser
				and Status = 1

				select 'Info' as title, 'Data Closing telah disetujui' as message,
				'success' as icon

			end
			else
			begin
				update LogClosing set MinusIndikasiMoneyCashBox = 0,
				MatchingSucces = 'Matching Fail',
				StatusAcceptanceBySPV = 'Diterima',
				KeteranganAcceptance =@Catatan,
				UangDiterimaFinnance = @Setoran,
				Status = 2
				where IdLog = @IdLog				

				select Datetime,NamaComputer,NamaUser into #temp2 from LogClosing where IdLog = @IdLog
				
				declare @NamaUser2 nvarchar(100)
				set @NamaUser2 = (select NamaUser from #temp2)

				update DataChasierBox set Status = 2, CloseBy = @NamaUser
				where NamaComputer in (select NamaComputer from #temp)
				and Datetime in (select left(Datetime,10) from #temp)
				and Status = 1

				update LogCashierTambahModal set Status = 2
				where replace(CONVERT(VARCHAR(10), CONVERT(date, replace(left(Datetime,10),'/','-'), 105), 23),'-','') = FORMAT(GETDATE() , 'yyyyMMdd')
				and NamaComputer in (select NamaComputer from #temp)
				and NamaUser = @NamaUser
				and Status = 1

				select 'Warning' as title, 'Data Closing telah diterima' as message,
				'error' as icon
			end
		end
		else
		begin
			if(@Setoran = @CashBox)
			begin
				update LogClosing set MinusIndikasiMoneyCashBox = 0,
				MatchingSucces = 'Matching Success',
				StatusAcceptanceBySPV = 'Ditolak',
				KeteranganAcceptance =@Catatan,
				UangDiterimaFinnance = @Setoran,
				Status = 2
				where IdLog = @IdLog
				
				select 'Warning' as title, 'Data Closing tidak disetujui' as message,
				'error' as icon
			end
			else
			begin
				update LogClosing set MinusIndikasiMoneyCashBox = 0,
				MatchingSucces = 'Matching Fail',
				StatusAcceptanceBySPV = 'Ditolak',
				KeteranganAcceptance =@Catatan,
				UangDiterimaFinnance = @Setoran,
				Status = 2
				where IdLog = @IdLog

				select 'Warning' as title, 'Data Closing tidak Valid' as message,
				'error' as icon
			end
		end
	end
	else
	begin
		select 'Warning' as title, 'Data Closing tidak Valid' as message,
		'error' as icon
	end
END








