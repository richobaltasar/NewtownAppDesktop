

CREATE PROCEDURE [dbo].[SP_GETFoodCourtSalesLog]
	@Search nvarchar(50),
	@ComputerName nvarchar(50)
AS
BEGIN
	SET NOCOUNT ON;	
	select
		c.NamaTenant,a.NamaItem,a.Harga,sum(a.Qtx) Qty,sum(a.Total) Total, b.Stok
	from [LogItemsF&BTrx] a
	left join DataBarang b on b.idMenu = a.KodeBarang
	left join DataTenant c on c.idTenant = b.IdTenant
	where 
		--left(Datetime,10) = FORMAT(GETDATE() , 'dd/MM/yyyy') 
		replace(CONVERT(VARCHAR(10), CONVERT(date, replace(left(Datetime,10),'/','-'), 105), 23),'-','') = FORMAT(GETDATE() , 'yyyyMMdd')
		and ComputerName = @ComputerName
		and NamaItem like '%'+@Search+'%'
		and a.Status = 1
	group by a.NamaItem,b.Stok,a.Harga,c.NamaTenant
	order by b.Stok asc

END











