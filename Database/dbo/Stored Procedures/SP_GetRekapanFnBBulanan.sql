--SP_GetRekapanFnBBulanan '02/2020','Emoney'

CREATE PROCEDURE [dbo].[SP_GetRekapanFnBBulanan]
	@SetBulan nvarchar(max),
	@JenisTransaksiQ nvarchar(max)
AS
BEGIN
	SET NOCOUNT ON;
	if(@JenisTransaksiQ = 'Cash')
	begin
		select dbo.Roman(w.WEEK_OF_MONTH) WEEK_OF_MONTH,w.Uraian,sum(w.Qty) Qty,sum(w.Jumlah) Jumlah,w.JenisTransaksi into #temp from
		(
			select DATEPART(WEEK,q.Tanggal )  -
					DATEPART(WEEK, DATEADD(MM, DATEDIFF(MM,0,q.Tanggal ), 0))+ 1 AS WEEK_OF_MONTH,
			q.Uraian,q.HargaSatuan,sum(Qtx) Qty, q.NamaDiskon,sum(TotalDiskon) TotalDiskon,
			sum(Jumlah) Jumlah,q.JenisTransaksi
			from 
			(
				select CONVERT(VARCHAR(10), CONVERT(date, replace(left(a.Datetime,10),'/','-'), 105), 23) Tanggal,
				d.JenisTransaksi, 
				a.NamaTenant Uraian,0 HargaSatuan,a.Qtx,'' NamaDiskon,0 TotalDiskon,a.Total Jumlah
				from [LogItemsF&BTrx] a
				left join [LogFoodcourtTransaksi] d on d.IdItemsKeranjang = a.IdItemsKeranjang
				--left join ewats.dbo.DataBarang b on b.idMenu = a.KodeBarang
				--left join ewats.dbo.DataTenant c on c.idTenant = b.IdTenant
				where d.JenisTransaksi = 'Cash' 
				and right(left(a.Datetime,10),7) = @SetBulan
			) q
			group by q.Uraian,q.HargaSatuan,q.NamaDiskon,q.Tanggal,q.JenisTransaksi
		) w
		group by w.WEEK_OF_MONTH,w.JenisTransaksi,Uraian

	end
	else if(@JenisTransaksiQ = 'Emoney')
	begin
		select dbo.Roman(w.WEEK_OF_MONTH) WEEK_OF_MONTH,w.Uraian,sum(w.Qty) Qty,sum(w.Jumlah) Jumlah,w.JenisTransaksi into #temp2 from
		(
			select DATEPART(WEEK,q.Tanggal )  -
					DATEPART(WEEK, DATEADD(MM, DATEDIFF(MM,0,q.Tanggal ), 0))+ 1 AS WEEK_OF_MONTH,
			q.Uraian,q.HargaSatuan,sum(Qtx) Qty, q.NamaDiskon,sum(TotalDiskon) TotalDiskon,
			sum(Jumlah) Jumlah,q.JenisTransaksi
			from 
			(
				select CONVERT(VARCHAR(10), CONVERT(date, replace(left(a.Datetime,10),'/','-'), 105), 23) Tanggal,
				d.JenisTransaksi, 
				a.NamaTenant Uraian,0 HargaSatuan,a.Qtx,'' NamaDiskon,0 TotalDiskon,a.Total Jumlah
				from [LogItemsF&BTrx] a
				left join [LogFoodcourtTransaksi] d on d.IdItemsKeranjang = a.IdItemsKeranjang
				--left join ewats.dbo.DataBarang b on b.idMenu = a.KodeBarang
				--left join ewats.dbo.DataTenant c on c.idTenant = b.IdTenant
				where d.JenisTransaksi = 'emoney' 
				and right(left(a.Datetime,10),7) = @SetBulan
			) q
			group by q.Uraian,q.HargaSatuan,q.NamaDiskon,q.Tanggal,q.JenisTransaksi
		) w
		group by w.WEEK_OF_MONTH,w.JenisTransaksi,Uraian
	end

	
	create table #Db
	(
		no bigint,
		Uraian nvarchar(max),
		Qty1 float,
		Jumlah1 float,
		Qty2 float,
		Jumlah2 float,
		Qty3 float,
		Jumlah3 float,
		Qty4 float,
		Jumlah4 float,
		Qty5 float,
		Jumlah5 float,
		Qty6 float,
		Jumlah6 float,
		Total float,
		JenisTransaksi nvarchar(max)
	)
	
	declare @no bigint
	declare @Uraian nvarchar(max)
	declare @Qty float
	declare @Jumlah float
	declare @JenisTransaksi nvarchar(max)
	declare @WEEK_OF_MONTH nvarchar(max)
	set @no = 1
	if(@JenisTransaksiQ = 'Cash')
	begin
	
		while EXISTS (select*from #temp)
																															begin
		 SELECT TOP 1 @Uraian = Uraian,@Qty = Qty,@Jumlah=Jumlah,@JenisTransaksi = JenisTransaksi,@WEEK_OF_MONTH = WEEK_OF_MONTH FROM  #temp 
		 
		 if(@WEEK_OF_MONTH = 'I')
		 begin
			insert into #Db (no,Uraian,Jumlah1,Qty1,Jumlah2,Qty2,Jumlah3,Qty3,Jumlah4,Qty4,Jumlah5,Qty5,Jumlah6,Qty6,JenisTransaksi,Total)
			values(@no,@Uraian,@Jumlah,@Qty,0,0,0,0,0,0,0,0,0,0,@JenisTransaksi,@Jumlah)
		 end
		 else if(@WEEK_OF_MONTH = 'II')
		 begin
			insert into #Db (no,Uraian,Jumlah1,Qty1,Jumlah2,Qty2,Jumlah3,Qty3,Jumlah4,Qty4,Jumlah5,Qty5,Jumlah6,Qty6,JenisTransaksi,Total)
			values(@no,@Uraian,0,0,@Jumlah,@Qty,0,0,0,0,0,0,0,0,@JenisTransaksi,@Jumlah)
		 end
		 else if(@WEEK_OF_MONTH = 'III')
		 begin
			insert into #Db (no,Uraian,Jumlah1,Qty1,Jumlah2,Qty2,Jumlah3,Qty3,Jumlah4,Qty4,Jumlah5,Qty5,Jumlah6,Qty6,JenisTransaksi,Total)
			values(@no,@Uraian,0,0,0,0,@Jumlah,@Qty,0,0,0,0,0,0,@JenisTransaksi,@Jumlah)
		 end
		 else if(@WEEK_OF_MONTH = 'IV')
		 begin
			insert into #Db (no,Uraian,Jumlah1,Qty1,Jumlah2,Qty2,Jumlah3,Qty3,Jumlah4,Qty4,Jumlah5,Qty5,Jumlah6,Qty6,JenisTransaksi,Total)
			values(@no,@Uraian,0,0,0,0,0,0,@Jumlah,@Qty,0,0,0,0,@JenisTransaksi,@Jumlah)
		 end
		 else if(@WEEK_OF_MONTH = 'V')
		 begin
			insert into #Db (no,Uraian,Jumlah1,Qty1,Jumlah2,Qty2,Jumlah3,Qty3,Jumlah4,Qty4,Jumlah5,Qty5,Jumlah6,Qty6,JenisTransaksi,Total)
			values(@no,@Uraian,0,0,0,0,0,0,0,0,@Jumlah,@Qty,0,0,@JenisTransaksi,@Jumlah)
		 end
		 else if(@WEEK_OF_MONTH = 'VI')
		 begin
			insert into #Db (no,Uraian,Jumlah1,Qty1,Jumlah2,Qty2,Jumlah3,Qty3,Jumlah4,Qty4,Jumlah5,Qty5,Jumlah6,Qty6,JenisTransaksi,Total)
			values(@no,@Uraian,0,0,0,0,0,0,0,0,0,0,@Jumlah,@Qty,@JenisTransaksi,@Jumlah)
		 end
		 delete from #temp where WEEK_OF_MONTH = @WEEK_OF_MONTH and JenisTransaksi = @JenisTransaksi and Uraian = @Uraian
		 set @no = @no+ 1
	end
		select 
			Uraian,sum(Jumlah1)Jumlah1,sum(Qty1)Qty1,sum(Jumlah2)Jumlah2,sum(Qty2)Qty2,sum(Jumlah3)Jumlah3,sum(Qty3)Qty3,
			sum(Jumlah4)Jumlah4,sum(Qty4)Qty4,sum(Jumlah5)Jumlah5,sum(Qty5)Qty5,sum(Jumlah6)Jumlah6,sum(Qty6)Qty6,sum(Total) Total, JenisTransaksi 
		from #Db
		group by JenisTransaksi,Uraian
	
		drop table #temp

	end
	else if(@JenisTransaksiQ = 'Emoney')
	begin
		while EXISTS (select*from #temp2)
		begin
		 SELECT TOP 1 @Uraian = Uraian,@Qty = Qty,@Jumlah=Jumlah,@JenisTransaksi = JenisTransaksi,@WEEK_OF_MONTH = WEEK_OF_MONTH FROM  #temp2 
		 
		 if(@WEEK_OF_MONTH = 'I')
		 begin
			insert into #Db (no,Uraian,Jumlah1,Qty1,Jumlah2,Qty2,Jumlah3,Qty3,Jumlah4,Qty4,Jumlah5,Qty5,Jumlah6,Qty6,JenisTransaksi,Total)
			values(@no,@Uraian,@Jumlah,@Qty,0,0,0,0,0,0,0,0,0,0,@JenisTransaksi,@Jumlah)
		 end
		 else if(@WEEK_OF_MONTH = 'II')
		 begin
			insert into #Db (no,Uraian,Jumlah1,Qty1,Jumlah2,Qty2,Jumlah3,Qty3,Jumlah4,Qty4,Jumlah5,Qty5,Jumlah6,Qty6,JenisTransaksi,Total)
			values(@no,@Uraian,0,0,@Jumlah,@Qty,0,0,0,0,0,0,0,0,@JenisTransaksi,@Jumlah)
		 end
		 else if(@WEEK_OF_MONTH = 'III')
		 begin
			insert into #Db (no,Uraian,Jumlah1,Qty1,Jumlah2,Qty2,Jumlah3,Qty3,Jumlah4,Qty4,Jumlah5,Qty5,Jumlah6,Qty6,JenisTransaksi,Total)
			values(@no,@Uraian,0,0,0,0,@Jumlah,@Qty,0,0,0,0,0,0,@JenisTransaksi,@Jumlah)
		 end
		 else if(@WEEK_OF_MONTH = 'IV')
		 begin
			insert into #Db (no,Uraian,Jumlah1,Qty1,Jumlah2,Qty2,Jumlah3,Qty3,Jumlah4,Qty4,Jumlah5,Qty5,Jumlah6,Qty6,JenisTransaksi,Total)
			values(@no,@Uraian,0,0,0,0,0,0,@Jumlah,@Qty,0,0,0,0,@JenisTransaksi,@Jumlah)
		 end
		 else if(@WEEK_OF_MONTH = 'V')
		 begin
			insert into #Db (no,Uraian,Jumlah1,Qty1,Jumlah2,Qty2,Jumlah3,Qty3,Jumlah4,Qty4,Jumlah5,Qty5,Jumlah6,Qty6,JenisTransaksi,Total)
			values(@no,@Uraian,0,0,0,0,0,0,0,0,@Jumlah,@Qty,0,0,@JenisTransaksi,@Jumlah)
		 end
		 else if(@WEEK_OF_MONTH = 'VI')
		 begin
			insert into #Db (no,Uraian,Jumlah1,Qty1,Jumlah2,Qty2,Jumlah3,Qty3,Jumlah4,Qty4,Jumlah5,Qty5,Jumlah6,Qty6,JenisTransaksi,Total)
			values(@no,@Uraian,0,0,0,0,0,0,0,0,0,0,@Jumlah,@Qty,@JenisTransaksi,@Jumlah)
		 end

		 delete from #temp2 where WEEK_OF_MONTH = @WEEK_OF_MONTH and JenisTransaksi = @JenisTransaksi and Uraian = @Uraian

		 set @no = @no+ 1
	end

		select 
			Uraian,sum(Jumlah1)Jumlah1,sum(Qty1)Qty1,sum(Jumlah2)Jumlah2,sum(Qty2)Qty2,sum(Jumlah3)Jumlah3,sum(Qty3)Qty3,
			sum(Jumlah4)Jumlah4,sum(Qty4)Qty4,sum(Jumlah5)Jumlah5,sum(Qty5)Qty5,sum(Jumlah6)Jumlah6,sum(Qty6)Qty6,sum(Total) Total, JenisTransaksi 
		from #Db
		group by JenisTransaksi,Uraian
	
		drop table #temp2
	end
	drop table #Db
END
