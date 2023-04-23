CREATE PROCEDURE [dbo].[WebSP_FSaveEditFormSTenant]
	@idTenant bigint,
	@NamaTenant nvarchar(max),
	@OpenDateTenant nvarchar(max),
	@PemilikTenant nvarchar(max),
	@StatusKepemilikan nvarchar(max),
	@StatusJual nvarchar(max),
	@MonitoringStock nvarchar(max),
	@FollowTenant nvarchar(max),
	@Status nvarchar(max)
AS
BEGIN
	SET NOCOUNT ON;
	declare @title nvarchar(max)
	declare @message nvarchar(max)
	declare @icon nvarchar(max)

	if exists(select*from [dbo].DataTenant where idTenant = @idTenant)
	begin
		declare @Stat int
		declare @StatMonitoringStock int
		declare @FollowTenantQ nvarchar(max)

		if(@Status = 'Aktif')
		begin
			set @Stat = 1;
		end
		else
		begin
			set @Stat = 0;
		end

		if(@MonitoringStock = 'Enable')
		begin
			set @StatMonitoringStock = 1;
		end
		else
		begin
			set @StatMonitoringStock = 0;
		end

		if(@StatusJual = 'Karyawan')
		begin
			set @FollowTenantQ = @FollowTenant
		end
		else
		begin
			set @FollowTenantQ = ''
		end

		update DataTenant
		set NamaTenant = @NamaTenant,
		PemilikTenant= @PemilikTenant,
		OpenDateTenant = @OpenDateTenant,
		Status = @Stat,
		StatusKepemilikan = @StatusKepemilikan,
		StatusJual = @StatusJual,
		MonitoringStock = @StatMonitoringStock,
		FollowTenant = @FollowTenantQ
		where idTenant = @idTenant
		
		set @title='Update data Success'
		set @message='Data Tenan '+ @NamaTenant +' berhasil diupdate'
		set @icon='success'

	end
	else
	begin
		set @title='Update data Failed'
		set @message='Data tidak ditemukan'
		set @icon='error'
	end

	select @title title, @message message,@icon icon
END












