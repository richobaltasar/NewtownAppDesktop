-- Batch submitted through debugger: dbewats.sql|1293|0|C:\Users\Administrator\Desktop\dbewats.sql

CREATE PROCEDURE [dbo].[SP_GETTICKETPRICE]
	@search nvarchar(max)
AS
BEGIN
	SET NOCOUNT ON;
	select* into #temp from DataTicket where status = 'Aktif'
	DECLARE @QRY NVARCHAR(MAX)
	SET @QRY = 'select IdTicket id,namaticket NamaTicket,'+ datename(dw,getdate()) +' as Harga from #temp where NamaTicket like ''%'+@search+'%'' '
	EXEC SP_EXECUTESQL @QRY

END












