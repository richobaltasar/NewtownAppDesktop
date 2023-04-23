CREATE PROCEDURE [dbo].[SP_SaveDataTambahModal]
	@ComputerName Nvarchar(max),
	@NamaUser nvarchar(max),
	@Nominal float
AS
BEGIN
	SET NOCOUNT ON;
	declare @msg nvarchar(max)

	if exists(select*from [dbo].[DataChasierBox] 
			where NamaComputer = @ComputerName and left(Datetime,10) = FORMAT(GETDATE() , 'dd/MM/yyyy') and status = 1)
	begin
		declare @DanaSebelumnya float
		declare @TotalUangDiBox float
		declare @TotalUangMasuk float

		set @DanaSebelumnya = (select DanaModalSetelah from DataChasierBox where NamaComputer = @ComputerName and left(Datetime,10) = FORMAT(GETDATE() , 'dd/MM/yyyy') and Status = 1)
		set @TotalUangDiBox = (select TotalUangDiBox from DataChasierBox where NamaComputer = @ComputerName and left(Datetime,10) = FORMAT(GETDATE() , 'dd/MM/yyyy') and Status = 1)
		set @TotalUangMasuk = (select TotalUangMasuk from DataChasierBox where NamaComputer = @ComputerName and left(Datetime,10) = FORMAT(GETDATE() , 'dd/MM/yyyy') and Status = 1)

		update DataChasierBox 
		set DanaModalSebelum =  @DanaSebelumnya,
		DanaModalSetelah = (@DanaSebelumnya + @Nominal),
		TotalUangDiBox = (@TotalUangDiBox + @Nominal),
		TotalUangMasuk = (@TotalUangMasuk + @Nominal ),
		UpdateBy = @NamaUser
		where NamaComputer = @ComputerName and left(Datetime,10) = FORMAT(GETDATE() , 'dd/MM/yyyy')
		and status = 1

		set @msg = 'Update dana Cashbox berhasil'
		-- save log tambah modal
		insert into [dbo].[LogCashierTambahModal]
		([Datetime],[NamaComputer],[NamaUser],[NominalTambahModal],[Status])
		values(
		FORMAT(GETDATE() , 'dd/MM/yyyy HH:mm:ss'),
		@ComputerName,@NamaUser,@Nominal,1)
	end
	else
	begin
		insert into DataChasierBox
		([Datetime],[NamaComputer],[DanaModalSebelum],[DanaModalSetelah],
		[TotalUangDiBox],[TotalUangMasuk],[Status],
		[OpenBy])
		values(FORMAT(GETDATE() , 'dd/MM/yyyy'), @ComputerName, 0, @Nominal,
		@Nominal,@Nominal,1,@NamaUser)

		-- save log tambah modal
		insert into [dbo].[LogCashierTambahModal]
		([Datetime],[NamaComputer],[NamaUser],[NominalTambahModal],[Status])
		values(
		FORMAT(GETDATE() , 'dd/MM/yyyy HH:mm:ss'),
		@ComputerName,@NamaUser,@Nominal,1)

		set @msg = 'Insert dana ke Cashbox berhasil'
	end

	select @msg as _Message, 'TRUE' as Success
END













