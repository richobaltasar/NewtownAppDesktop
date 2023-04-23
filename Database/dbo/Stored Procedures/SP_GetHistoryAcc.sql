CREATE PROCEDURE [dbo].[SP_GetHistoryAcc]
	@AccountNumber nvarchar(max)
AS
BEGIN
	SET NOCOUNT ON;
	select top 100 q.* from
	(
	select
	idTrx idlog,Datetime,'TICKETING' JenisTransaksi,
	'Jaminan: '+cast(cast(SaldoJaminanAfter as float) as nvarchar(50))+', Ticket : '+cast(cast(QtyTotalTiket as float) as nvarchar(50))+', Asuransi :'+cast(cast(Asuransi as float) as nvarchar(50))+', Topup : '+cast(cast(isnull(Topup,0) as float) as nvarchar(50)) as Uraian,
	TotalBayar Nominal
	from LogRegistrasiDetail where AccountNumber = @AccountNumber
	union all
	select IdTopup idlog,Datetime,JenisPayment JenisTransaksi, 'Topup Nominal : '+cast(cast(NominalTopup as float) as nvarchar(50)) as Uraian,
	cast(cast(TotalBayar as float) as nvarchar(50)) Nominal
	from [dbo].[LogTopupDetail] where AccountNumber = @AccountNumber
	union all
	select IdItemsKeranjang idlog, Datetime,'POS' JenisTransaksi,
	NamaTenant +' - '+NamaItem+' ('+cast(cast(Qtx as float) as nvarchar(50))+' X '+cast(cast(Harga as float) as nvarchar(50))+') ' as Uraian,
	cast(cast(Total as float) as nvarchar(50)) Nominal
	from [LogItemsF&BTrx]
	where AccountNumber = @AccountNumber
	union all
	select
	IdRefund idlog,Datetime,'REFUND' JenisTransaksi,
	'Refund Nominal : '+cast(cast(TotalNominalRefund as float) as nvarchar(50)) as Uraian,
	cast(cast(TotalNominalRefund as float) as nvarchar(50)) Nominal
	from LogRefundDetail
	where AccountNumber = @AccountNumber
	) q
	order by q.Datetime asc

--select*from LogRegistrasiDetail
--select*from [dbo].[LogTopupDetail] 
--select*from [LogItemsF&BTrx]
--select*from LogFoodcourtTransaksi
END









