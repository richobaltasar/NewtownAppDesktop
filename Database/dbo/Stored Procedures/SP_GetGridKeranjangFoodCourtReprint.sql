-- Batch submitted through debugger: dbewats.sql|1138|0|C:\Users\Administrator\Desktop\dbewats.sql

CREATE PROCEDURE [dbo].[SP_GetGridKeranjangFoodCourtReprint]
	@ItemKeranjang bigint
AS
BEGIN
	SET NOCOUNT ON;
	select
	Harga,NamaItem,Qtx,Total
	from [LogItemsF&BTrx]
	where IdItemsKeranjang = @ItemKeranjang
END











