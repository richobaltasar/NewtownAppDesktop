CREATE PROCEDURE [dbo].[SP_SaveItemsFB]
	@IdItemsKeranjang bigint,
	@KodeBarang bigint,
	@NamaItem nvarchar(max),
	@Harga float,
	@Qtx float,
	@AccountNumber nvarchar(max),
	@Chasierby nvarchar(max),
	@ComputerName nvarchar(max)
AS
BEGIN
	SET NOCOUNT ON;
	
	declare @NamaTenant nvarchar(max)
	set @NamaTenant = (select NamaTenant from DataTenant where idTenant in (select IdTenant from DataBarang where idMenu = @KodeBarang))

	insert into [dbo].[LogItemsF&BTrx]
	(IdItemsKeranjang,Datetime,KodeBarang,NamaItem,Harga,Qtx,Total,Status,Chasierby,ComputerName,AccountNumber,NamaTenant)
	values(
		@IdItemsKeranjang,FORMAT(GETDATE() , 'dd/MM/yyyy HH:mm:ss'),
		@KodeBarang,@NamaItem,@Harga,@Qtx,(@Harga*@Qtx),1,@Chasierby,@ComputerName,@AccountNumber,@NamaTenant
	)

	declare @Stok float
	declare @SisaStok float
	set @Stok = (select Stok from DataBarang where idMenu = @KodeBarang and status = 'Aktif')
	set @SisaStok = @Stok - @Qtx
	
	update DataBarang
	set Stok = @SisaStok
	where idMenu = @KodeBarang and status ='Aktif'

	select 'Insert LogItemsF&BTrx Success' _Message,'TRUE' Success
END















