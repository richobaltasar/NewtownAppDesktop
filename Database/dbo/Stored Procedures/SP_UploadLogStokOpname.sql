-- sp_helptext SP_UploadLogStokOpname
CREATE PROCEDURE [dbo].[SP_UploadLogStokOpname]
	@idLog    bigint
	,@Datetime    nvarchar(50)
	,@NamaTenant    nvarchar(MAX)
	,@NamaItem    nvarchar(MAX)
	,@StockSebelumnya    float
	,@StockUpdate    float
	,@UpdateBy    nvarchar(50)
	,@Status    int
AS
BEGIN
	SET NOCOUNT ON;
	if not exists(select*from LogStokOpname where  idLog= @idLog)
	begin
		insert into 
		LogStokOpname
		(
			idLog
			,Datetime
			,NamaTenant
			,NamaItem
			,StockSebelumnya
			,StockUpdate
			,UpdateBy
			,Status
		)
		values(
			@idLog
			,@Datetime
			,@NamaTenant
			,@NamaItem
			,@StockSebelumnya
			,@StockUpdate
			,@UpdateBy
			,@Status
		)
		select @idLog as Id
	end
END



