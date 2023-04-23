
CREATE PROCEDURE [dbo].[SP_GetLogHistoryAccountTicket]
	@NoAkun nvarchar(max)
AS
BEGIN
	SET NOCOUNT ON;
	select a.Datetime,b.AccountNumber,a.NamaTicket,a.Qty,a.Harga,Total,a.Diskon,TotalDiskon,TotalAfterDiskon from [dbo].[LogTicketDetail] a
	left join LogRegistrasiDetail b on b.IdTicketTrx = a.IdTicket
	where b.AccountNumber like ''+@NoAkun+'%'
	order by a.Datetime asc

END







