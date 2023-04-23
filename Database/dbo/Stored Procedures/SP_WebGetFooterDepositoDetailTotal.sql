CREATE PROCEDURE [dbo].[SP_WebGetFooterDepositoDetailTotal]
	@StatusAkun nvarchar(max)
AS
BEGIN
	SET NOCOUNT ON;
	if(@StatusAkun = 'All')
	begin
		select
			q.Balanced,q.UangJaminan, (q.Balanced+q.UangJaminan) TotalDeposit
		from 
		(
			select sum(isnull(Balanced,0)) Balanced,sum(isnull(UangJaminan,0)) UangJaminan from DataAccount
			where UangJaminan > 0 and RefundDate is null and Status =1
		) q
	end
	else if(@StatusAkun = 'Aktif')
	begin
		select w.* from 
		(
			select
				isnull(q.Balanced,0) Balanced,isnull(q.UangJaminan,0) UangJaminan, isnull((q.Balanced+q.UangJaminan),0) TotalDeposit
			from 
			(
				select sum(isnull(a.Balanced,0)) Balanced,sum(isnull(a.UangJaminan,0)) UangJaminan,StatusAktif
				from 
				(
					select*,
					case when (replace(CONVERT(VARCHAR(10), CONVERT(date, replace(left(ExpiredDate,10),'/','-'), 105), 23),'-','')) < (FORMAT(GETDATE() , 'yyyyMMdd') )
					then 'Expired' else 'Aktif' end as StatusAktif
					from DataAccount
					where UangJaminan > 0 and RefundDate is null and Status =1
				) a
				where a.UangJaminan > 0
				group by a.StatusAktif
			) q
			where StatusAktif = 'Aktif'
		) w
	end
	else if(@StatusAkun = 'Expired')
	begin
		select w.* from 
		(
			select
				isnull(q.Balanced,0) Balanced,isnull(q.UangJaminan,0) UangJaminan, isnull((q.Balanced+q.UangJaminan),0) TotalDeposit
			from 
			(
				select sum(isnull(a.Balanced,0)) Balanced,sum(isnull(a.UangJaminan,0)) UangJaminan,StatusAktif
				from 
				(
					select*,
					case when (replace(CONVERT(VARCHAR(10), CONVERT(date, replace(left(ExpiredDate,10),'/','-'), 105), 23),'-','')) < (FORMAT(GETDATE() , 'yyyyMMdd') )
					then 'Expired' else 'Aktif' end as StatusAktif
					from DataAccount
					where UangJaminan > 0 and RefundDate is null and Status =1
				) a
				where a.UangJaminan > 0
				group by a.StatusAktif
			) q
			where StatusAktif = 'Expired'
		) w
	end
END










