CREATE PROCEDURE [dbo].[SP_WebGetDepositoDetailTotal]
	@StatusAkun nvarchar(max)
AS
BEGIN
	SET NOCOUNT ON;
	if(@StatusAkun = 'All')
	begin
		select*,
		case when (replace(CONVERT(VARCHAR(10), CONVERT(date, replace(left(ExpiredDate,10),'/','-'), 105), 23),'-','')) < (FORMAT(GETDATE() , 'yyyyMMdd') )
		then 'Expired' else 'Aktif' end as StatusAktif
		from DataAccount
		where UangJaminan > 0 and RefundDate is null and Status= 1
	end
	else if(@StatusAkun = 'Aktif')
	begin
		select q.* from 
		(
			select*,
			case when (replace(CONVERT(VARCHAR(10), CONVERT(date, replace(left(ExpiredDate,10),'/','-'), 105), 23),'-','')) < (FORMAT(GETDATE() , 'yyyyMMdd') )
			then 'Expired' else 'Aktif' end as StatusAktif
			from DataAccount
			where UangJaminan > 0 and RefundDate is null and Status= 1
		) q
		where q.StatusAktif = 'Aktif'
	end
	else if(@StatusAkun = 'Expired')
	begin
		select q.* from 
		(
			select*,
			case when (replace(CONVERT(VARCHAR(10), CONVERT(date, replace(left(ExpiredDate,10),'/','-'), 105), 23),'-','')) < (FORMAT(GETDATE() , 'yyyyMMdd') )
			then 'Expired' else 'Aktif' end as StatusAktif
			from DataAccount
			where UangJaminan > 0 and RefundDate is null and Status= 1
		) q
		where q.StatusAktif = 'Expired'
	end
END










