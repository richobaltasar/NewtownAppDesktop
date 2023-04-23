CREATE PROCEDURE [dbo].[WebSP_FSaveNewFormSTenant]
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

	if not exists(select*from [dbo].DataTenant where NamaTenant = @NamaTenant)
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

		insert into DataTenant
		(
			NamaTenant,PemilikTenant,OpenDateTenant,StatusKepemilikan,StatusJual,FollowTenant,MonitoringStock,Status
		)
		values(
			@NamaTenant,@PemilikTenant,@OpenDateTenant,@StatusKepemilikan,
			@StatusJual,@FollowTenantQ,@StatMonitoringStock,@Stat
		)
		
		set @title='Add data Success'
		set @message='Data Tenant '+ @NamaTenant +' berhasil diregistrasi'
		set @icon='success'

	end
	else
	begin
		set @title='Nama Tenan sudah teregistrasi'
		set @message='Data already exists'
		set @icon='error'
	end

	select @title title, @message message,@icon icon
END












