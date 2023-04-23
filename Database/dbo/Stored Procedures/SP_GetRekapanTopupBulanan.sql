﻿CREATE PROCEDURE [dbo].[SP_GetRekapanTopupBulanan]
	@SetBulan nvarchar(max)
AS
BEGIN
	SET NOCOUNT ON;
	select dbo.Roman(w.WEEK_OF_MONTH) WEEK_OF_MONTH,w.Uraian + ' ('+w.JenisTransaksi+')' Uraian,sum(w.Qty) Qty,sum(w.Jumlah) Jumlah,w.JenisTransaksi into #temp from
	(
		select 
			DATEPART(WEEK,Tanggal )  -
					DATEPART(WEEK, DATEADD(MM, DATEDIFF(MM,0,Tanggal ), 0))+ 1 AS WEEK_OF_MONTH,q.Uraian,
					sum(Qty) Qty,sum(Jumlah) Jumlah,q.PaymentMethod JenisTransaksi 
		from 
		(
			select
			CONVERT(VARCHAR(10), CONVERT(date, replace(left(Datetime,10),'/','-'), 105), 23) Tanggal,
			'Pembelian Topup ' Uraian,sum(NominalTopup) Jumlah,PaymentMethod,count(AccountNumber) Qty
			from LogTopupDetail
			where right(left(Datetime,10),7) = @SetBulan
			group by PaymentMethod,CONVERT(VARCHAR(10), CONVERT(date, replace(left(Datetime,10),'/','-'), 105), 23)
		) q
		group by q.Uraian,q.PaymentMethod,q.Tanggal
	) w
	group by w.WEEK_OF_MONTH,w.JenisTransaksi,Uraian

	
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
	while EXISTS (select*from #temp)
	begin
		 SELECT TOP 1 @Uraian = Uraian,@Qty = Qty,@Jumlah=Jumlah,@JenisTransaksi = JenisTransaksi,@WEEK_OF_MONTH = WEEK_OF_MONTH FROM  #Temp 
		 if(@WEEK_OF_MONTH = 'I')
		 begin
			insert into #Db (no,Uraian,Jumlah1,Qty1,Jumlah2,Qty2,Jumlah3,Qty3,Jumlah4,Qty4,Jumlah5,Qty5,JenisTransaksi,Total)
			values(@no,@Uraian,@Jumlah,@Qty,0,0,0,0,0,0,0,0,@JenisTransaksi,@Jumlah)
		 end
		 else if(@WEEK_OF_MONTH = 'II')
		 begin
			insert into #Db (no,Uraian,Jumlah1,Qty1,Jumlah2,Qty2,Jumlah3,Qty3,Jumlah4,Qty4,Jumlah5,Qty5,JenisTransaksi,Total)
			values(@no,@Uraian,0,0,@Jumlah,@Qty,0,0,0,0,0,0,@JenisTransaksi,@Jumlah)
		 end
		 else if(@WEEK_OF_MONTH = 'III')
		 begin
			insert into #Db (no,Uraian,Jumlah1,Qty1,Jumlah2,Qty2,Jumlah3,Qty3,Jumlah4,Qty4,Jumlah5,Qty5,JenisTransaksi,Total)
			values(@no,@Uraian,0,0,0,0,@Jumlah,@Qty,0,0,0,0,@JenisTransaksi,@Jumlah)
		 end
		 else if(@WEEK_OF_MONTH = 'IV')
		 begin
			insert into #Db (no,Uraian,Jumlah1,Qty1,Jumlah2,Qty2,Jumlah3,Qty3,Jumlah4,Qty4,Jumlah5,Qty5,JenisTransaksi,Total)
			values(@no,@Uraian,0,0,0,0,0,0,@Jumlah,@Qty,0,0,@JenisTransaksi,@Jumlah)
		 end
		 else if(@WEEK_OF_MONTH = 'V')
		 begin
			insert into #Db (no,Uraian,Jumlah1,Qty1,Jumlah2,Qty2,Jumlah3,Qty3,Jumlah4,Qty4,Jumlah5,Qty5,JenisTransaksi,Total)
			values(@no,@Uraian,0,0,0,0,0,0,0,0,@Jumlah,@Qty,@JenisTransaksi,@Jumlah)
		 end
		 delete from #temp where WEEK_OF_MONTH = @WEEK_OF_MONTH and JenisTransaksi = @JenisTransaksi
		 set @no = @no+ 1
	end
	
	select Uraian,sum(Jumlah1)Jumlah1,sum(Qty1)Qty1,sum(Jumlah2)Jumlah2,sum(Qty2)Qty2,sum(Jumlah3)Jumlah3,sum(Qty3)Qty3,
	sum(Jumlah4)Jumlah4,sum(Qty4)Qty4,sum(Jumlah5)Jumlah5,sum(Qty5)Qty5,sum(Total) Total, JenisTransaksi from #Db
	group by JenisTransaksi,Uraian
	
	drop table #Db
	drop table #temp
END








