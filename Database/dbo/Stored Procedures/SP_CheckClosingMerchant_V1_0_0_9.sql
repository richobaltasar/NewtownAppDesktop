--exec SP_CheckClosingMerchant_V1_0_0_9 'DESKTOP-IMD4D2C','vita'

CREATE PROCEDURE [dbo].[SP_CheckClosingMerchant_V1_0_0_9]
	@NamComputer nvarchar(max),
	@NamaUser nvarchar(max)
AS
BEGIN
	SET NOCOUNT ON;
	declare @OpenBy nvarchar(max)
	if exists(
		select*from LogClosingV2
		where 
		replace(CONVERT(VARCHAR(10), CONVERT(date, replace(left(Datetime,10),'/','-'), 105), 23),'-','') = FORMAT(GETDATE() , 'yyyyMMdd')  
		and ComputerName = @NamComputer and status = 1
		)
	begin
		if exists
		(
			select*from LogClosingV2
			where 
			replace(CONVERT(VARCHAR(10), CONVERT(date, replace(left(Datetime,10),'/','-'), 105), 23),'-','') = FORMAT(GETDATE() , 'yyyyMMdd') 
			and ComputerName = @NamComputer
			and StatusAcceptanceBySPV = 'Waiting Approve'
			and Status = 1
		)
		begin
			declare @input float
			declare @NamaUserP nvarchar(max)
			set @input  = ( select top 1 KasirInputNominal from LogClosingV2
							where 
							replace(CONVERT(VARCHAR(10), CONVERT(date, replace(left(Datetime,10),'/','-'), 105), 23),'-','') = FORMAT(GETDATE() , 'yyyyMMdd') 
							and ComputerName = @NamComputer
							and Status = 1
							)
			set @NamaUserP = (
			select top 1 Username from LogClosingV2
			where replace(CONVERT(VARCHAR(10), CONVERT(date, replace(left(Datetime,10),'/','-'), 105), 23),'-','') = FORMAT(GETDATE() , 'yyyyMMdd') 
			and ComputerName = @NamComputer
			and Status = 1)
		
			if(@NamaUserP != @NamaUser)
			begin
				select 'Merchant ini telah melakukan tutup buku sebesar : Rp '+CAST((FORMAT(@input,'#,0')) AS varchar) + ', dilakukan Oleh ' + @NamaUserP
				as _Message, 'TRUE' as Success
			end
			else 
			begin
				select 'Merchant ini telah melakukan tutup buku sebesar : Rp '+CAST((FORMAT(@input,'#,0')) AS varchar) + ', dilakukan Oleh ' + @NamaUserP
				as _Message, 'TRUE' as Success
			end
		end
		else
		begin
			if exists(
				select*from DataChasierBox 
				where 
				replace(CONVERT(VARCHAR(10), CONVERT(date, replace(left(Datetime,10),'/','-'), 105), 23),'-','') = FORMAT(GETDATE() , 'yyyyMMdd')
				and NamaComputer = @NamComputer
				and Status = 1)
			begin
				set @OpenBy =  (
									select OpenBy from DataChasierBox 
									where 
									replace(CONVERT(VARCHAR(10), CONVERT(date, replace(left(Datetime,10),'/','-'), 105), 23),'-','') = FORMAT(GETDATE() , 'yyyyMMdd')
									and NamaComputer = @NamComputer
									and Status = 1
								)
				if(@OpenBy = @NamaUser)
				begin
					select '' as _Message, 'FALSE' as Success	
				end
				else
				begin
					select 'Merchant Ini telah dibuka oleh '+@OpenBy+', silahkan diclosing dulu oleh '+@OpenBy as _Message, 'TRUE' as Success		
				end
			end
			else
			begin
				select '' as _Message, 'FALSE' as Success	
			end
		end

	end
	else
	begin
		if exists(
				select*from DataChasierBox 
				where 
				replace(CONVERT(VARCHAR(10), CONVERT(date, replace(left(Datetime,10),'/','-'), 105), 23),'-','') = FORMAT(GETDATE() , 'yyyyMMdd')
				and NamaComputer = @NamComputer
				and Status = 1
				)
			begin
				

				set @OpenBy =  (
									select OpenBy from DataChasierBox 
									where 
									replace(CONVERT(VARCHAR(10), CONVERT(date, replace(left(Datetime,10),'/','-'), 105), 23),'-','') = FORMAT(GETDATE() , 'yyyyMMdd')
									and NamaComputer = @NamComputer
									and Status = 1
								)
				if(@OpenBy = @NamaUser)
				begin
					select '' as _Message, 'FALSE' as Success	
				end
				else
				begin
					select 'Merchant Ini telah dibuka oleh '+@OpenBy+', silahkan diclosing dulu oleh '+@OpenBy as _Message, 'TRUE' as Success		
				end
			end
			else
			begin
				select '' as _Message, 'FALSE' as Success	
			end
	end
END


