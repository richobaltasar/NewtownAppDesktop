
--exec SP_GetLogAccount @NoAkun=''

CREATE PROCEDURE [dbo].[SP_GetLogAccount]
	@NoAkun nvarchar(max)
AS
BEGIN
	SET NOCOUNT ON;
	select top 100
	a.AccountNumber,a.CreateDate,a.ExpiredDate,b.Datetime RefundDate
	from DataAccount a 
	left join LogRefundDetail b on b.AccountNumber = a.AccountNumber
	where a.AccountNumber like '%'+@NoAkun+'%'
END






