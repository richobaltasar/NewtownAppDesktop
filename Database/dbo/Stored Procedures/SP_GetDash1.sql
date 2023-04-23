CREATE PROCEDURE [dbo].[SP_GetDash1]
	@SetAwal nvarchar(50),
	@SetAkhir nvarchar(50)
AS
BEGIN
	SET NOCOUNT ON;

	declare @TotalAsuransi float
	declare @TotalFBEmoney float
	declare @TotalFBCash float
	declare @TotalJaminan float
	declare @TotalPemasukan1 float
	declare @TotalRefund float
	declare @TotalRegistrasi float
	declare @TotalTopup float
	declare @TotalVisitor float
	declare @TotalPemasukan float
	declare @TotalMarginFB float
	declare @TotalAsuransiNonVisitor float
	declare @TotalAsuransiVisitor float
	declare @TotalFBCounter float
	declare @GelangRegisToday float
	declare @TotalGelangBelomRefund float
	declare @TotalGelangRefund float
	declare @DepositExpired float
	declare @DepositAktif float
	declare @DepositTotal float
	
	--set @TotalGelangBelomRefund = (select count(AccountNumber) from DataAccount where UangJaminan > 0)

	set @DepositAktif = 	
	(
		select (q.Balanced+q.UangJaminan) TotalDeposit
		from 
		(
			select sum(isnull(a.Balanced,0)) Balanced,sum(isnull(a.UangJaminan,0)) UangJaminan,StatusAktif
			from 
			(
				select*,
				case when (replace(CONVERT(VARCHAR(10), CONVERT(date, replace(left(ExpiredDate,10),'/','-'), 105), 23),'-','')) < (FORMAT(GETDATE() , 'yyyyMMdd') )
				then 'Expired' else 'Aktif' end as StatusAktif
				from DataAccount
				where UangJaminan > 0 and status = 1
			) a
			where a.UangJaminan > 0
			group by a.StatusAktif
		) q
		where StatusAktif = 'Aktif'
	)

	set @DepositTotal = (select sum((isnull(Balanced,0)+isnull(UangJaminan,0))) Deposit from DataAccount where UangJaminan > 0 and Status=1)

	set @DepositExpired = 	
	(select
		sum(isnull(qry.Deposit,0)) DepositExpired
		from
		(
			select 
			AccountNumber,(isnull(Balanced,0)+isnull(UangJaminan,0)) Deposit
			from DataAccount 
			where (replace(CONVERT(VARCHAR(10), CONVERT(date, replace(left(ExpiredDate,10),'/','-'), 105), 23),'-','')) < (FORMAT(GETDATE() , 'yyyyMMdd') ) 
			and UangJaminan > 0 and status = 1
		) qry
	)

	set @TotalGelangRefund = (select distinct count(AccountNumber) 
		from LogRefundDetail 
		where 
		replace(CONVERT(VARCHAR(10), CONVERT(date, replace(left(Datetime,10),'/','-'), 105), 23),'-','') = FORMAT(GETDATE() , 'yyyyMMdd')
		and AccountNumber in (select distinct AccountNumber from DataAccount 
		where 
		replace(CONVERT(VARCHAR(10), CONVERT(date, replace(left(Datetime,10),'/','-'), 105), 23),'-','') = FORMAT(GETDATE() , 'yyyyMMdd')))


	set @TotalGelangBelomRefund = (select distinct count(AccountNumber) from DataAccount where UangJaminan > 0)

	set @TotalMarginFB = isnull(
	(select sum(q.marginAll) from (
	select c.NamaTenant,a.NamaItem,a.Harga ,a.Qtx,a.Total,b.IdTenant,b.Harga hargamodal,(a.Harga-b.Harga) marginPcs,((a.Harga-b.Harga)*a.Qtx) marginAll 
	from [LogItemsF&BTrx] a
	left join ewats.dbo.DataBarang b on b.idMenu = a.KodeBarang
	left join ewats.dbo.DataTenant c on c.idTenant = b.IdTenant
	where 
	replace(CONVERT(VARCHAR(10), CONVERT(date, replace(left(Datetime,10),'/','-'), 105), 23),'-','')
	between 
	replace(CONVERT(VARCHAR(10), CONVERT(date, replace(@SetAwal,'/','-'), 105), 23),'-','') 
	and replace(CONVERT(VARCHAR(10), CONVERT(date, replace(@SetAkhir,'/','-'), 105), 23),'-','')
	--and a.Status = 1
	)q),0)	

	set @TotalAsuransi = isnull((select sum(Asuransi) from [dbo].[LogRegistrasiDetail] where 
	replace(CONVERT(VARCHAR(10), CONVERT(date, replace(left(Datetime,10),'/','-'), 105), 23),'-','')
	between 
	replace(CONVERT(VARCHAR(10), CONVERT(date, replace(@SetAwal,'/','-'), 105), 23),'-','') 
	and replace(CONVERT(VARCHAR(10), CONVERT(date, replace(@SetAkhir,'/','-'), 105), 23),'-','')
	--and Status = 1
	),0)
	
	set @TotalAsuransiNonVisitor 
	= isnull(
	(
		select sum(e.total) total from 
		(
			select (w.Asuransi * w.Qty) total from 
			(
				select (q.Asuransi/q.QtyTotalTiket) Asuransi,q.Qty  from 
				(
					select a.IdTicket,sum(a.Qty) Qty,b.Asuransi,b.QtyTotalTiket 
					from [dbo].[LogTicketDetail] a
					inner join (select Asuransi,IdTicketTrx,QtyTotalTiket from LogRegistrasiDetail) b on b.IdTicketTrx = a.IdTicket
					where a.Harga = 0 
					and replace(CONVERT(VARCHAR(10), CONVERT(date, replace(left(Datetime,10),'/','-'), 105), 23),'-','')
					between 
					replace(CONVERT(VARCHAR(10), CONVERT(date, replace(@SetAwal,'/','-'), 105), 23),'-','') 
					and replace(CONVERT(VARCHAR(10), CONVERT(date, replace(@SetAkhir,'/','-'), 105), 23),'-','')
					--and a.Status = 1
					group by a.IdTicket,b.Asuransi,b.QtyTotalTiket
				) q
			) w
		) e
	),0)

	set @TotalAsuransiVisitor
	= isnull(
	(
		select sum(e.total) total from 
		(
			select (w.Asuransi * w.Qty) total from 
			(
				select (q.Asuransi/q.QtyTotalTiket) Asuransi,q.Qty  from 
				(
					select a.IdTicket,sum(a.Qty) Qty,b.Asuransi,b.QtyTotalTiket 
					from [dbo].[LogTicketDetail] a
					inner join (select Asuransi,IdTicketTrx,QtyTotalTiket from LogRegistrasiDetail) b on b.IdTicketTrx = a.IdTicket
					where a.Harga > 0 
					and 
					replace(CONVERT(VARCHAR(10), CONVERT(date, replace(left(Datetime,10),'/','-'), 105), 23),'-','')
					between 
					replace(CONVERT(VARCHAR(10), CONVERT(date, replace(@SetAwal,'/','-'), 105), 23),'-','') 
					and replace(CONVERT(VARCHAR(10), CONVERT(date, replace(@SetAkhir,'/','-'), 105), 23),'-','')
					--and a.Status = 1
					group by a.IdTicket,b.Asuransi,b.QtyTotalTiket
				) q
			) w
		) e
	),0)

	set @TotalFBEmoney = isnull((select sum(TotalBayar) from [dbo].LogFoodcourtTransaksi 
							where 
							replace(CONVERT(VARCHAR(10), CONVERT(date, replace(left(Datetime,10),'/','-'), 105), 23),'-','')
							between 
							replace(CONVERT(VARCHAR(10), CONVERT(date, replace(@SetAwal,'/','-'), 105), 23),'-','') 
							and replace(CONVERT(VARCHAR(10), CONVERT(date, replace(@SetAkhir,'/','-'), 105), 23),'-','')
							--and Status = 1 
							and JenisTransaksi='eMoney'),0)
	set @TotalFBCash = isnull((select sum(TotalBayar) from [dbo].LogFoodcourtTransaksi 
							where 
							replace(CONVERT(VARCHAR(10), CONVERT(date, replace(left(Datetime,10),'/','-'), 105), 23),'-','')
							between 
							replace(CONVERT(VARCHAR(10), CONVERT(date, replace(@SetAwal,'/','-'), 105), 23),'-','') 
							and replace(CONVERT(VARCHAR(10), CONVERT(date, replace(@SetAkhir,'/','-'), 105), 23),'-','')
							--and Status = 1 
							and JenisTransaksi='Cash'),0)

	set @TotalJaminan = isnull((select sum(qry.Jaminan) from ( select case when SaldoJaminan > 0 then 0 else  SaldoJaminanAfter end as Jaminan from [dbo].LogRegistrasiDetail 
						where 
						replace(CONVERT(VARCHAR(10), CONVERT(date, replace(left(Datetime,10),'/','-'), 105), 23),'-','')
						between 
						replace(CONVERT(VARCHAR(10), CONVERT(date, replace(@SetAwal,'/','-'), 105), 23),'-','') 
						and replace(CONVERT(VARCHAR(10), CONVERT(date, replace(@SetAkhir,'/','-'), 105), 23),'-','')
						--and Status = 1 
						) as qry),0)
	set @TotalRefund = isnull((select sum(TotalNominalRefund) from LogRefundDetail 
						where 
						replace(CONVERT(VARCHAR(10), CONVERT(date, replace(left(Datetime,10),'/','-'), 105), 23),'-','')
						between 
						replace(CONVERT(VARCHAR(10), CONVERT(date, replace(@SetAwal,'/','-'), 105), 23),'-','') 
						and replace(CONVERT(VARCHAR(10), CONVERT(date, replace(@SetAkhir,'/','-'), 105), 23),'-','')
						--and Status = 1
						),0)
	
	set @TotalRegistrasi = isnull((select sum(TotalBeliTiket) from LogRegistrasiDetail 
						where 
						replace(CONVERT(VARCHAR(10), CONVERT(date, replace(left(Datetime,10),'/','-'), 105), 23),'-','')
						between 
						replace(CONVERT(VARCHAR(10), CONVERT(date, replace(@SetAwal,'/','-'), 105), 23),'-','') 
						and replace(CONVERT(VARCHAR(10), CONVERT(date, replace(@SetAkhir,'/','-'), 105), 23),'-','')
						--and Status = 1
						),0)

	set @TotalTopup = isnull((
							select sum(q.Total) from 
							(
								select sum(q.Topup) Total from
								(
									select isnull(Topup,0) Topup
									from LogRegistrasiDetail 
									where Topup > 0
									and replace(CONVERT(VARCHAR(10), CONVERT(date, replace(left(Datetime,10),'/','-'), 105), 23),'-','')
									between 
									replace(CONVERT(VARCHAR(10), CONVERT(date, replace(@SetAwal,'/','-'), 105), 23),'-','') 
									and replace(CONVERT(VARCHAR(10), CONVERT(date, replace(@SetAkhir,'/','-'), 105), 23),'-','')
								) q
								union all
								select sum(TotalBayar) Total from LogTopupDetail 
								where 
								replace(CONVERT(VARCHAR(10), CONVERT(date, replace(left(Datetime,10),'/','-'), 105), 23),'-','')
								between 
								replace(CONVERT(VARCHAR(10), CONVERT(date, replace(@SetAwal,'/','-'), 105), 23),'-','') 
								and replace(CONVERT(VARCHAR(10), CONVERT(date, replace(@SetAkhir,'/','-'), 105), 23),'-','')
								--and Status = 1
							) q
						),0)

	set @TotalVisitor = isnull((select sum(qty) from LogTicketDetail 
						where 
						replace(CONVERT(VARCHAR(10), CONVERT(date, replace(left(Datetime,10),'/','-'), 105), 23),'-','')
						between 
						replace(CONVERT(VARCHAR(10), CONVERT(date, replace(@SetAwal,'/','-'), 105), 23),'-','') 
						and replace(CONVERT(VARCHAR(10), CONVERT(date, replace(@SetAkhir,'/','-'), 105), 23),'-','')
						--and Status = 1
						),0)
	set @TotalPemasukan1 = isnull(((@TotalRegistrasi+@TotalTopup+@TotalJaminan+@TotalAsuransi)-(@TotalRefund+@TotalFBEmoney)),0)
	set @TotalPemasukan = isnull(((@TotalRegistrasi+@TotalTopup+@TotalJaminan+@TotalAsuransi)- @TotalRefund),0)
	
	set @TotalFBCounter = 
	(
	select sum(a.Qtx) Qtx
	from [LogItemsF&BTrx] a
	left join ewats.dbo.DataBarang b on b.idMenu = a.KodeBarang
	where 
	--a.status = 1 and 
	left(Datetime,10) between @SetAwal and @SetAkhir)

	set @GelangRegisToday = (select distinct count(AccountNumber) from DataAccount where left(CreateDate,10) = FORMAT(GETDATE() , 'dd/MM/yyyy'))

	select 
		@TotalAsuransi as TotalAsuransi, 
		@TotalFBEmoney as TotalFBEmoney, 
		@TotalFBCash as TotalFBCash, 
		@TotalJaminan as TotalJaminan,
		@TotalRefund as TotalRefund,
		@TotalRegistrasi as TotalRegistrasi,
		@TotalTopup as TotalTopup,
		@TotalVisitor as TotalVisitor,
		@TotalPemasukan1  as TotalPemasukan1,
		@TotalPemasukan as TotalIncome,
		@TotalMarginFB as TotalMarginFB,
		@TotalAsuransiNonVisitor as TotalAsuransiNonVisitor,
		@TotalAsuransiVisitor as TotalAsuransiVisitor,
		@TotalFBCounter as TotalFBCounter,
		@GelangRegisToday as GelangRegisToday,
		@TotalGelangRefund as TotalGelangRefund,
		@TotalGelangBelomRefund as TotalGelangBelomRefund,
		@DepositExpired as DepositExpired,
		@DepositAktif as DepositAktif,
		@DepositTotal as DepositTotal
END




















