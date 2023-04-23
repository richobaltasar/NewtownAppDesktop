
CREATE PROCEDURE [dbo].[GetLogHistoryAccountRegistrasi]
	@NoAkun nvarchar(max)
AS
BEGIN
	SET NOCOUNT ON;
	select Datetime,IdTicketTrx,AccountNumber,JenisTransaksi,QtyTotalTiket,Topup,
	isnull(TotalBayar,0) TotalBayar ,
	isnull(PayEmoney,0) PayEmoney ,isnull(PayCash,0) PayCash,isnull(TotalDebit,0) PayEDC
	from [dbo].[LogRegistrasiDetail] 
	where AccountNumber like ''+ @NoAkun+ '%' and AccountNumber != ''
	order by Datetime desc

END







