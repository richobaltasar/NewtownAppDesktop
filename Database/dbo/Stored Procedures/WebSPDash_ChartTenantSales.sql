-- Batch submitted through debugger: dbewats.sql|3345|0|C:\Users\Administrator\Desktop\dbewats.sql

CREATE PROCEDURE [dbo].[WebSPDash_ChartTenantSales]
	@SetAwal  nvarchar(50),
	@SetAkhir  nvarchar(50)
AS
BEGIN
	
	SET NOCOUNT ON;
	if(@SetAwal = @SetAkhir)
	begin
		select left(right(datetime,8),2)+':00' datetime,sum(total) total
		from [LogItemsF&BTrx]
		where 
			replace(CONVERT(VARCHAR(10), CONVERT(date, replace(left(Datetime,10),'/','-'), 105), 23),'-','')
			between 
			replace(CONVERT(VARCHAR(10), CONVERT(date, replace(@SetAwal,'/','-'), 105), 23),'-','') 
			and replace(CONVERT(VARCHAR(10), CONVERT(date, replace(@SetAkhir,'/','-'), 105), 23),'-','')
		group by left(right(datetime,8),2)
	end
	else
	begin
		select left(datetime,10) datetime,sum(total) total
		from [LogItemsF&BTrx]
		where 
			replace(CONVERT(VARCHAR(10), CONVERT(date, replace(left(Datetime,10),'/','-'), 105), 23),'-','')
			between 
			replace(CONVERT(VARCHAR(10), CONVERT(date, replace(@SetAwal,'/','-'), 105), 23),'-','') 
			and replace(CONVERT(VARCHAR(10), CONVERT(date, replace(@SetAkhir,'/','-'), 105), 23),'-','')
		group by left(datetime,10),KodeBarang
	end
END












