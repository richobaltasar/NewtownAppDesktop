-- Batch submitted through debugger: dbewats.sql|491|0|C:\Users\Administrator\Desktop\dbewats.sql
--exec SP_GedtIdTiket
CREATE PROCEDURE [dbo].[SP_GedtIdTrxF&B]
AS
BEGIN
	SET NOCOUNT ON;
	declare @Itung bigint

	set @Itung = 0;
	set @Itung = (select top 1 isnull(IdItemsKeranjang,0) as IdTicket from [dbo].[LogItemsF&BTrx]
					order by IdItemsKeranjang desc)
	select isnull(@itung,0)+1 IdItems

END











