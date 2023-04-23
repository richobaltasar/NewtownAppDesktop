
CREATE PROCEDURE [dbo].[SP_UploadLogTicketDetail]
	@Datetime      nvarchar(50),
	@IdTicket      bigint,
	@AccountNumber      nvarchar(MAX),
	@NamaTicket      nvarchar(MAX),
	@Harga      float,
	@Qty      float,
	@Total      float,
	@IdDiskon      bigint,
	@NamaDiskon      nvarchar(MAX),
	@Diskon      float,
	@TotalDiskon      float,
	@TotalAfterDiskon      float,
	@Status      int,
	@ChasierBy      nvarchar(MAX),
	@ComputerName      nvarchar(MAX)
AS
BEGIN
	SET NOCOUNT ON;
	if not exists(select*from [dbo].[LogTicketDetail] where IdTicket = @IdTicket)
	begin
		insert into LogTicketDetail
		(
			Datetime,IdTicket,AccountNumber,NamaTicket,Harga,
			Qty,Total,IdDiskon,NamaDiskon,Diskon,TotalDiskon,TotalAfterDiskon,Status,ChasierBy,ComputerName
		)
		values
		(
			@Datetime,
			@IdTicket,
			@AccountNumber,
			@NamaTicket,
			@Harga,
			@Qty,
			@Total,
			@IdDiskon,
			@NamaDiskon,
			@Diskon,
			@TotalDiskon,
			@TotalAfterDiskon,
			@Status,
			@ChasierBy,
			@ComputerName
		)
		select @IdTicket as Id
	end
END




