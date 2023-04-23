CREATE PROCEDURE [dbo].[SP_SaveTicketKeranjang]
	@AccountNumber nvarchar(max),
	@IdTicket bigint,
	@NamaTicket nvarchar(max),
	@Harga float,
	@Qty float,
	@Total float,
	@IdDiskon bigint,
	@NamaDiskon nvarchar(max),
	@Diskon float,
	@TotalDiskon float,
	@TotalAfterDiskon float,
	@ChasierBy nvarchar(max),
	@ComputerName nvarchar(max)
AS
BEGIN
	SET NOCOUNT ON;
	insert into LogTicketDetail 
	(Datetime,IdTicket,NamaTicket,Harga,Qty,Total,IdDiskon,NamaDiskon,Diskon,TotalDiskon,TotalAfterDiskon,status,
	[ChasierBy],[ComputerName],AccountNumber)
	values(
		FORMAT(GETDATE() , 'dd/MM/yyyy HH:mm:ss'),@IdTicket,@NamaTicket,@Harga,@Qty,@Total,
		@IdDiskon,@NamaDiskon,@Diskon,@TotalDiskon,@TotalAfterDiskon,1,
		@ChasierBy,@ComputerName,@AccountNumber
	)

	select 'Insert LogticketDetail success' as _Message, 'TRUE' as Success
END












