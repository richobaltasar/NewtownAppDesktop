CREATE PROCEDURE [dbo].[SP_CheckExpired]
	@CodeId nvarchar(max)
AS
BEGIN
	SET NOCOUNT ON;
	select 
		case when Expired > FORMAT(GETDATE() , 'yyyyMMdd') then 'Aktif' else 'Account Expired' end as Status
	from
	(
		select
		CreateDate,
		 replace(CONVERT(varchar(10), CONVERT(date, left(ExpiredDate,10), 103), 120),'-','') Expired
		from DataAccount 
		where AccountNumber = @CodeId
	) q

END








