CREATE PROCEDURE [dbo].[SP_GetRekapanFnBHarian]
	@SetTanggal nvarchar(max),
	@JenisTransaksi nvarchar(max)
AS
BEGIN
	SET NOCOUNT ON;
	if(@JenisTransaksi = 'Cash')
	begin
		select q.Uraian,q.HargaSatuan,sum(Qtx) Qty, q.NamaDiskon,sum(TotalDiskon) TotalDiskon,
		sum(Jumlah) Jumlah
		from 
		(
			select 
			d.JenisTransaksi, 
			a.NamaTenant Uraian,0 HargaSatuan,a.Qtx,'' NamaDiskon,0 TotalDiskon,a.Total Jumlah
			from [LogItemsF&BTrx] a
			left join [LogFoodcourtTransaksi] d on d.IdItemsKeranjang = a.IdItemsKeranjang
			--left join ewats.dbo.DataBarang b on b.idMenu = a.KodeBarang
			--left join ewats.dbo.DataTenant c on c.idTenant = b.IdTenant
			where d.JenisTransaksi = 'Cash' 
			and 
			--left(a.Datetime,10) = @setTanggal
			replace(CONVERT(VARCHAR(10), CONVERT(date, replace(left(a.Datetime,10),'/','-'), 105), 23),'-','') 
			= replace(CONVERT(VARCHAR(10), CONVERT(date, replace(@setTanggal,'/','-'), 105), 23),'-','') 
		) q
		group by q.Uraian,q.HargaSatuan,q.NamaDiskon
	end
	else if(@JenisTransaksi = 'Emoney')
	begin
		select q.Uraian,q.HargaSatuan,sum(Qtx) Qty, q.NamaDiskon,sum(TotalDiskon) TotalDiskon,
		sum(Jumlah) Jumlah
		from 
		(
			select 
			d.JenisTransaksi, 
			a.NamaTenant Uraian,0 HargaSatuan,a.Qtx,'' NamaDiskon,0 TotalDiskon,a.Total Jumlah
			from [LogItemsF&BTrx] a
			left join [LogFoodcourtTransaksi] d on d.IdItemsKeranjang = a.IdItemsKeranjang
			--left join ewats.dbo.DataBarang b on b.idMenu = a.KodeBarang
			--left join ewats.dbo.DataTenant c on c.idTenant = b.IdTenant
			where d.JenisTransaksi = 'eMoney' 
			and 
			--left(a.Datetime,10) = @setTanggal
			replace(CONVERT(VARCHAR(10), CONVERT(date, replace(left(a.Datetime,10),'/','-'), 105), 23),'-','') 
			= replace(CONVERT(VARCHAR(10), CONVERT(date, replace(@setTanggal,'/','-'), 105), 23),'-','') 
		) q
		group by q.Uraian,q.HargaSatuan,q.NamaDiskon
	end
END













