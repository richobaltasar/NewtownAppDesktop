
CREATE PROCEDURE [dbo].[SP_FSubmitFormClosing_V1_0_0_9]
	@LogId bigint,
	@TanggalSetor nvarchar(max),
	@KeteranganAcceptance nvarchar(max),
	@UangDiterimaFinnance float,
	@TotalAmountStrukEDC float
AS
BEGIN
	SET NOCOUNT ON;
	BEGIN TRY  
	declare @title nvarchar(max)
	declare @message nvarchar(max)
	declare @icon nvarchar(max)
	if exists(select*from LogClosingV2 where LogId = @LogId)
	begin
		if(@UangDiterimaFinnance = 0 and @TotalAmountStrukEDC = 0)
		begin
			set @title='Sorry'
			set @icon='error'
			set @message ='Uang Setoran & Struk EDC masih kosong'	
			
		end
		else
		begin
			declare @NamaKasir nvarchar(max)

			select @NamaKasir = Username from LogClosingV2 where LogId = @LogId
			
			update LogClosingV2
			set StatusAcceptanceBySPV = 'Diterima', KeteranganAcceptance = @KeteranganAcceptance,
			UangDiterimaFinnance=@UangDiterimaFinnance,TotalAmountStrukEDC=@TotalAmountStrukEDC,
			Status = 2, TanggalSetoran = @TanggalSetor
			where LogId = @LogId

			set @title='Success'
			set @icon='success'
			set @message ='Setoran Kasir :'	+ @NamaKasir +' uang sebesar '+ cast(@UangDiterimaFinnance as nvarchar(max)) +' dan Total Struk EDC : '+cast(@TotalAmountStrukEDC as nvarchar(max))+' diterima'
		end
	end
	else
	begin
		set @title='Sorry'
		set @icon='error'
		set @message ='Id Closing tidak valid / tidak ditemukan'	
	end

	select @title title, @message message,@icon icon

	END TRY  
	BEGIN CATCH  
		select 'Error' title, 'Code:'+ERROR_NUMBER()+' -' +ERROR_MESSAGE() message,'error' icon
	END CATCH;  
END
