CREATE PROCEDURE [dbo].[SP_GetRekapanAsuramsiHarian]
	@SetTanggal nvarchar(max)
AS
BEGIN
	SET NOCOUNT ON;
	select 
		q.Uraian,q.HargaSatuan,sum(Qty) Qty,
		NamaDiskon,sum(TotalDiskon) TotalDiskon,sum(Jumlah) Jumlah,JenisTransaksi 
	from 
	(
		select
		'Biaya Asuransi' Uraian,(Asuransi/QtyTotalTiket) HargaSatuan,
		QtyTotalTiket Qty,'' NamaDiskon,0 TotalDiskon,Asuransi Jumlah,JenisTransaksi
		from LogRegistrasiDetail
		where 
		--left(Datetime,10) = @setTanggal--'08/01/2019'--
		replace(CONVERT(VARCHAR(10), CONVERT(date, replace(left(Datetime,10),'/','-'), 105), 23),'-','') = replace(CONVERT(VARCHAR(10), CONVERT(date, replace(@setTanggal,'/','-'), 105), 23),'-','') 
	) q
	group by q.Uraian,q.HargaSatuan,NamaDiskon,JenisTransaksi

END











