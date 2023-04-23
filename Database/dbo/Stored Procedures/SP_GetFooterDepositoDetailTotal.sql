
CREATE PROCEDURE [dbo].[SP_GetFooterDepositoDetailTotal]
AS
BEGIN
	SET NOCOUNT ON;
	select Sum(isnull(w.TotalDeposit,0)) TotalSetor from 
	(
		select
			q.Balanced,q.UangJaminan, (q.Balanced+q.UangJaminan) TotalDeposit
		from 
		(
			select sum(isnull(a.Balanced,0)) Balanced,sum(isnull(a.UangJaminan,0)) UangJaminan,StatusAktif
			from 
			(
				select*,
				case when (replace(CONVERT(VARCHAR(10), CONVERT(date, replace(left(ExpiredDate,10),'/','-'), 105), 23),'-','')) < (FORMAT(GETDATE() , 'yyyyMMdd') )
				then 'Expired' else 'Aktif' end as StatusAktif
				from DataAccount
				where UangJaminan > 0
			) a
			where a.UangJaminan > 0
			group by a.StatusAktif
		) q
		where StatusAktif = 'Expired'
	) w
END







