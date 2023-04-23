-- Batch submitted through debugger: dbewats.sql|859|0|C:\Users\Administrator\Desktop\dbewats.sql

CREATE PROCEDURE [dbo].[SP_GetDataStok]
	@search nvarchar(max)
AS
BEGIN
	SET NOCOUNT ON;
	select q.* from
	(
	select a.idMenu,b.NamaTenant,a.NamaMenu,a.Stok from DataBarang a
	left join DataTenant b on b.idTenant = a.IdTenant
	where a.status = 'Aktif' and a.NamaMenu like '%'+@search+'%'
	
	) q where q.NamaTenant != ''
	order by q.Stok asc
END










