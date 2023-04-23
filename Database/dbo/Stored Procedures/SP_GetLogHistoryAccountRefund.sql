
CREATe PROCEDURE [dbo].[SP_GetLogHistoryAccountRefund]
	@NoAkun nvarchar(max)
AS
BEGIN
	SET NOCOUNT ON;

	select*from LogRefundDetail
	where AccountNumber like ''+@NoAkun+'%'
END







