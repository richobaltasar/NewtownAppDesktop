
CREATE PROCEDURE [dbo].[SP_GetLogHistoryAccountFNB]
	@NoAkun nvarchar(max)
AS
BEGIN
	SET NOCOUNT ON;

	select a.Datetime,AccountNumber,NamaItem,a.Harga,Qtx,Total from [dbo].[LogItemsF&BTrx] a
	where AccountNumber != '' and AccountNumber like ''+@NoAkun+'%'
	order by a.Datetime asc

END







