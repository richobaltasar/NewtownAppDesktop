
CREATE PROCEDURE [dbo].[SP_GetMingguanHeader]
	@SetBulan nvarchar(max)
AS
BEGIN
	SET NOCOUNT ON;
	select distinct dbo.Roman(WEEK_OF_MONTH) MingguKe from 
	(
		SELECT
		DATEPART(WEEK, q.Tanggal )  -
			DATEPART(WEEK, DATEADD(MM, DATEDIFF(MM,0,q.Tanggal ), 0))+ 1 AS WEEK_OF_MONTH
		from
		(
			select distinct 
			CONVERT(VARCHAR(10), CONVERT(date, replace(left(Datetime,10),'/','-'), 105), 23) Tanggal
			from LogRegistrasiDetail
			where right(left(Datetime,10),7) = '01/2019'
		) q
	) w
END








