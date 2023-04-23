
CREATE PROCEDURE [dbo].[SP_UploadLogItemsFBTrx]
	@IdItemsKeranjang    bigint,
	@Datetime    nvarchar(50),
	@NamaTenant    nvarchar(MAX),
	@KodeBarang    bigint,
	@NamaItem    nvarchar(MAX),
	@Harga    float,
	@Qtx    float,
	@Total    float,
	@Status    int,
	@Chasierby    nvarchar(MAX),
	@ComputerName    nvarchar(MAX),
	@AccountNumber    nvarchar(50)
AS
BEGIN
	SET NOCOUNT ON;
	if not exists(select*from [LogItemsF&BTrx] where  IdItemsKeranjang= @IdItemsKeranjang)
	begin
		insert into 
		[LogItemsF&BTrx]
		(
			IdItemsKeranjang
			,Datetime
			,NamaTenant
			,KodeBarang
			,NamaItem
			,Harga
			,Qtx
			,Total
			,Status
			,Chasierby
			,ComputerName
			,AccountNumber
		)
		values(
			@IdItemsKeranjang
			,@Datetime
			,@NamaTenant
			,@KodeBarang
			,@NamaItem
			,@Harga
			,@Qtx
			,@Total
			,@Status
			,@Chasierby
			,@ComputerName
			,@AccountNumber
		)
		select @IdItemsKeranjang as Id
	end
END



