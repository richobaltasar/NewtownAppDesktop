CREATE  PROCEDURE [dbo].[SP_GetRekapanGelangJaminanPeriodik]
	@SetAwal nvarchar(max),
	@SetAkhir nvarchar(max)
AS
BEGIN
	SET NOCOUNT ON;
	
	select 
	'Saldo Jaminan untuk Gelang' as Uraian,
	q.SaldoJaminanAfter HargaSatuan,
	q.Qty,'' NamaDiskon,'' TotalDiskon ,JenisTransaksi,
	(q.Qty*q.SaldoJaminanAfter) Jumlah from 
	(
		select
		distinct SaldoJaminanAfter,COUNT(AccountNumber) Qty,JenisTransaksi
		from LogRegistrasiDetail
		where 
		replace(CONVERT(VARCHAR(10), CONVERT(date, replace(left(Datetime,10),'/','-'), 105), 23),'-','')
		between 
		replace(CONVERT(VARCHAR(10), CONVERT(date, replace(@SetAwal,'/','-'), 105), 23),'-','') 
		and replace(CONVERT(VARCHAR(10), CONVERT(date, replace(@SetAkhir,'/','-'), 105), 23),'-','')
		and SaldoJaminan = 0
		group by SaldoJaminanAfter,JenisTransaksi
	) q

END












