CREATE PROCEDURE [dbo].[SP_GetRekapanAsuramsiPeriodik]
	@SetAwal nvarchar(max),
	@SetAkhir nvarchar(max)
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
		replace(CONVERT(VARCHAR(10), CONVERT(date, replace(left(Datetime,10),'/','-'), 105), 23),'-','') 
		between replace(CONVERT(VARCHAR(10), CONVERT(date, replace(@SetAwal,'/','-'), 105), 23),'-','') 
		and replace(CONVERT(VARCHAR(10), CONVERT(date, replace(@SetAkhir,'/','-'), 105), 23),'-','') 
	) q
	group by q.Uraian,q.HargaSatuan,NamaDiskon,JenisTransaksi

END











