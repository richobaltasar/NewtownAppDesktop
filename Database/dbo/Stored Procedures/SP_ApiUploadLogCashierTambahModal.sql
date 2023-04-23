
CREATE PROCEDURE [dbo].[SP_ApiUploadLogCashierTambahModal]
	@idLog bigint,
	@Datetime nvarchar(50),
	@NamaComputer nvarchar(MAX),
	@NamaUser nvarchar(MAX),
	@NominalTambahModal float,
	@Status int
AS
BEGIN
	SET NOCOUNT ON;
    if not exists(select*from LogCashierTambahModal where idLog = @idLog)
	begin
		insert into 
		LogCashierTambahModal
		(
			idLog,Datetime,NamaComputer,NamaUser,NominalTambahModal,Status
		)
		values(
			@idLog,@Datetime,@NamaComputer,@NamaUser,@NominalTambahModal,@Status
		)
		select @idLog as Id
	end
END



