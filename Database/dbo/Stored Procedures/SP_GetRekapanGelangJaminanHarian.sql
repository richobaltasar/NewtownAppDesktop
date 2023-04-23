CREATE  PROCEDURE [dbo].[SP_GetRekapanGelangJaminanHarian]
	@SetTanggal nvarchar(max)
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
		LEFT(Datetime,10) = @SetTanggal
		and SaldoJaminan = 0
		group by SaldoJaminanAfter,JenisTransaksi
	) q

END












